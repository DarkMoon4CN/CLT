using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Common
{
    /// <summary>
    /// Task manager
    /// </summary>
    public class TaskManager
    {
        private static readonly TaskManager _taskManager = new TaskManager();

        public int NewTaskId { get; set; }

        /// <summary>
        /// Get TaskManager instance
        /// </summary>
        public static TaskManager Instance
        {
            get
            {
                return _taskManager;
            }
        }

        private List<TaskBase> _tasks = new List<TaskBase>();

        /// <summary>
        /// Get or set task list
        /// </summary>
        public List<TaskBase> Tasks
        {
            get { return _tasks; }
            set { _tasks = value; }
        }

        /// <summary>
        /// Stop task sys
        /// </summary>
        public void ReleaseAll()
        {
            foreach (TaskBase task in Tasks)
                task.Release();
        }

        /// <summary>
        /// Add a task to the taskmanager
        /// </summary>
        /// <param name="task">The Task</param>
        /// <param name="interval">Interval in seconds</param>
        /// <param name="start">Automatically start, after it's added. You can also manually start by task.Start()</param>
        private void AddTask(TaskBase task, long interval, bool start)
        {
            task.Interval = interval;
            this.Tasks.Add(task);
            if (start) task.Start();
        }

        public void LoadPersistentTasks()
        {

            foreach (String file in Directory.GetFiles(Settings.Instance.TaskSettingFolder, "*.xml"))
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                if (fileNameWithoutExtension != null)
                {
                    String[] parts = fileNameWithoutExtension.Split(new char[] { '_' });

                    if (parts.Length >= 2)
                    {
                        Type type = Type.GetType(typeof(TaskBase).Namespace + "." + parts[0]);
                        int productId = Int32.Parse(parts[1]);

                        if (type != null)
                        {


                            TaskBase existingTask = FindTask(type, productId);
                            if (existingTask == null)
                                CreateTask(type, productId);
                        }
                    }
                }
            }
        }

        public TaskBase FindTask(Type type, int settingId)
        {
            return Tasks.FirstOrDefault(t => t.GetType() == type && t.SettingID == settingId);
        }

        public TaskBase CreateTask(Type t, int settingId)
        {

            TaskBase task = (TaskBase)Activator.CreateInstance(t);
            task.SettingID = settingId;
            task.LoadSettings();
            task.RunOnStart = true;
            AddTask(task, task.DefaultInterval, task.RunOnStart);
            return task;

        }

    }
}
