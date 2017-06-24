///////////////////////////////////////////////////////////
//Name:运行框架-服务提供内核类
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System.Timers;
using System.Reflection;
using System.Diagnostics;
using System.ServiceProcess;
using System.Configuration;
using System;

namespace ChuanglitouP2P.WindowsService
{
    public partial class Service1 : ServiceBase
    {
        bool Canceled = false;
        bool Started = false;
        Timer _timer = new Timer();

        ProcessThread thread = null;
        string mainThreadName = "CLTgeraint.xue";

        ILogger logger = null;
        ICaching cache = null;
        WorkRoom _workRoom = null;
        IDatabaseProvider databaseProvider = null;
        string logPath = Assembly.GetExecutingAssembly().Location + "\\Logs\\";
        public Service1()
        {
            InitializeComponent();
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = 20 * 1000;//每10分钟守护一次
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                StartWork();
            }
            catch (Exception ex)
            {
                logger.WriteException(ex.Message, ex);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                //StartWork();
                //if (!_timer.Enabled)
                //{
                _timer.Enabled = true;
                //}
            }
            catch (Exception ex)
            {
                logger.WriteException(ex.Message, ex);
            }
        }

        protected override void OnStop()
        {
            try
            {
                //_timer.Enabled = false;
                //_workRoom.StopWork();
                //Canceled = true;
                Started = false;
            }
            catch (Exception ex)
            {
                logger.WriteException(ex.Message, ex);
            }
        }

        public void StartWork()
        {
            if (Started) return;
            Started = true;
            Canceled = false;
            InitialLogger();
            InitialCache();
            InitialDatabase();

            if (_workRoom == null)
            {
                _workRoom = new WorkRoom()
                {
                    Cache = cache,
                    DatabaseProvider = databaseProvider,
                    ExtendService = new WorkRoomService(),
                    Logger = logger
                };
                _workRoom.Workers = new WorkerCollection(_workRoom);
            }
            _workRoom.Workers.Add(new InvestSchedule());
            _workRoom.StartWork();
        }

        #region Initial

        private void InitialLogger()
        {
            if (logger == null)
            {
                logger = new Logger(new Log4netProvider(logPath));
            }
        }

        private void InitialCache()
        {
            if (cache == null)
            {
                cache = new CacheHttp();
                cache.Logger = logger;
            }
        }

        private void InitialDatabase()
        {
            if (databaseProvider == null)
            {
                var ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
                databaseProvider = new SqlServerProvider();
                databaseProvider.ConnectString = ConnectionString;
                databaseProvider.Logger = logger;
            }
        }
        #endregion
    }
}
