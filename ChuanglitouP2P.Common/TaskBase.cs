using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq; 

namespace ChuanglitouP2P.Common
{
    /// <summary>
    /// Task action interface
    /// </summary>
    public abstract class TaskBase
    {
        private Timer _timer;
        private DateTime _lastRun;
        private DateTime _nextRun;
        private int _taskID;
       // private GPApplication application;

        public enum TaskStatus
        {
            Ok, Fail, Pending
        }



        public virtual String Name { get { return this.GetType().Name; } }

        /// <summary>
        /// the setting id refers to which setting file is used. In most cases it's the productid
        /// </summary>

        public virtual int SettingID { get; set; }

        private long _interval;

        /// <summary>
        /// In seconds
        /// </summary>
        public long Interval
        {
            get { return _interval; }
            set
            {
                _interval = value;
                if (IsStarted) Start(); // apply changes
            }
        }

        public DateTime LastRun { get { return _lastRun; } }

        public DateTime NextRun
        {
            get { return _nextRun; }
            set
            {
                _nextRun = value;
                RecalcTimer();
            }
        }
        public TaskStatus LastRunStatus { get; set; }
        public String LastRunStatusMsg { get; set; }
        public bool IsStarted { get { return _timer != null; } }
        public int TaskID { get { return _taskID; } }

        public virtual String Log { get; set; }
        /// <summary>
        /// Execute task
        /// </summary>
        protected abstract void Execute();

        protected TaskBase()
        {
            Log = "";
            _taskID = TaskManager.Instance.NewTaskId++;
            // save the GPApplication so we can restore it in the new thread.
            //application = GPApplication.Instance;

        }

        #region start/stop/release

        private void RecalcTimer()
        {
            if (IsStarted)
            {
                // we actually don't use the interval, we just reschedule after each run.
                _timer.Change((long)(_nextRun - DateTime.Now).TotalMilliseconds, Timeout.Infinite);
            }
            else
            {
                _timer = new Timer(new TimerCallback(_timerCallback), this, (long)(_nextRun - DateTime.Now).TotalMilliseconds, Timeout.Infinite);
            }
        }

        public virtual void Start()
        {
            // this will calculate first run
            ScheduleNextRun();

            if (_nextRun < DateTime.Now)
            {
                Stop();
            }
            else
            {
                RecalcTimer();
                this.RunOnStart = true;
            }
        }



        public void Release()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }

        public virtual void Stop()
        {
            if (_timer != null)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _timer = null;
            }
            this.RunOnStart = false;
        }

        #endregion
        #region settings

        protected event EventHandler OnSettingsChanged;

        private XDocument _settingNode = XDocument.Parse("<Settings />");
        public XElement SettingNode { get { return _settingNode.Root; } }

        public string TaskSettingsFile
        {
            get
            {
                return Path.Combine(Settings.Instance.TaskSettingFolder, this.GetType().Name + "_" + this.SettingID + ".xml");

            }
        }

        public bool RunOnStart
        {
            get
            {
                return SettingNode.Attribute("RunOnStart") != null && SettingNode.Attribute("RunOnStart").Value == "true";
            }

            set
            {
                if (RunOnStart == value) return;
                SettingNode.SetAttributeValue("RunOnStart", value ? "true" : "false");
                SaveSettings();
            }
        }

        public T GetSetting<T>(String key, T defaultValue)
        {
            XAttribute attr = SettingNode.Attribute(key);
            if (attr == null) return defaultValue;
            else
                return ConvertHelper.ParseValue<T>(attr.Value, defaultValue);
        }

        public void SetSetting<T>(String key, T value)
        {
            XAttribute attr = SettingNode.Attribute(key);
            String strValue = value.ToString();

            if (attr == null)
                SettingNode.SetAttributeValue(key, strValue);
            else
                attr.Value = strValue;
        }


        public void SaveSettings()
        {
            _settingNode.Save(TaskSettingsFile);
        }

        public void LoadSettings()
        {
            if (File.Exists(TaskSettingsFile))
                _settingNode.Root.ReplaceWith(XDocument.Load(TaskSettingsFile).Root);
            else
            {
                SettingNode.ReplaceWith(DefaultSettings);
            }
            if (OnSettingsChanged != null) OnSettingsChanged(this, null);
        }


        public void LoadSettingsFromText(string value)
        {
            XElement doc = XElement.Parse(value);
            if (doc != null)
            {
                SettingNode.ReplaceWith(doc);
                SaveSettings();
                if (OnSettingsChanged != null) OnSettingsChanged(this, null);
            }
        }

        public virtual XElement DefaultSettings
        {
            get
            {
                return XElement.Parse("<Settings />");
            }
        }

        #endregion

        /// <summary>
        /// in seconds
        /// </summary>
        public abstract int DefaultInterval { get; }



        public Thread Run()
        {

            Thread t = new Thread(delegate(object o)
            {
             //   GPApplication.Attach(application);
                if (Monitor.TryEnter(this))
                {
                    TaskBase threadSafeReader = (TaskBase)o;
                    // keep a copy as the others move on.
                    try
                    {
                        threadSafeReader.StartExecute();
                        threadSafeReader.Execute();
                        threadSafeReader.EndExecute(TaskStatus.Ok, "");
                    }
                    catch (Exception e)
                    {

                        LogInfo.WriteLog("Job Exception: ", e);

                        threadSafeReader.EndExecute(TaskStatus.Fail, e.Message);
                        AddLog("An error occured: " + e.Message, false);
                    }
                    finally
                    {
                        Monitor.Exit(this);
                    }
                }
            });

            t.Start((object)this);

            return t;


        }
       // protected GPContext context;



        protected void _timerCallback(object sender)
        {
          //  context = new GPContext();
            Run();
            ScheduleNextRun();
        }

        protected virtual void ScheduleNextRun()
        {

            if (Interval > 0)
            {
                if (NextRun == DateTime.MinValue)
                {
                    // initial start will be either nextrun. If that's not set half of the interval
                    if (NextRun == DateTime.MinValue || NextRun < DateTime.Now && Interval > 0)
                        _nextRun = DateTime.Now.AddMilliseconds(Interval / 2 * 1000);
                }
                else
                {
                    NextRun = DateTime.Now.AddSeconds(Interval);
                }
            }
            else if (NextRun < DateTime.Now)
                Stop();

        }

        protected void AddLog(String text, bool addToLogfile)
        {
            this.Log += DateTime.Now + ":" + text + "\n";
            if (this.Log.Length > 5000) this.Log = this.Log.Remove(3000);
            if (addToLogfile)
                LogInfo.WriteLog(this.Name + "_" + this.SettingID + ":" + text);
        }

        #region Status

        protected virtual void StartExecute()
        {
            _lastRun = DateTime.Now;

            this.LastRunStatusMsg = "";
            this.LastRunStatus = TaskStatus.Pending;
        }

        protected virtual void EndExecute(TaskStatus status, string message)
        {
            // if already set to fail, don't overwrite with an ok.
            if (this.LastRunStatus != TaskStatus.Fail || status == TaskStatus.Fail)
            {
                this.LastRunStatus = status;
                // don't overwrite it if it's already set.
                if (!String.IsNullOrEmpty(message) && !String.IsNullOrEmpty(this.LastRunStatusMsg))
                    this.LastRunStatusMsg = message;
            }
        }

        #endregion





    }
}

