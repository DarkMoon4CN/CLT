///////////////////////////////////////////////////////////
//Name:框架模型-工作室实体类
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System;
using System.Threading;
namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// 工作室实体类
    /// </summary>
    public class WorkRoom : IWorkRoom
    {
        /// <summary>
        /// 工作室实体类
        /// </summary>
        public WorkRoom() { }

        /// <summary>
        /// 获取或设置日志服务对象
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// 获取或设置工作室内的数据库提供器对象
        /// </summary>
        public IDatabaseProvider DatabaseProvider { get; set; }

        /// <summary>
        /// 获取或设置工作室内的缓存对象
        /// </summary>
        public ICaching Cache { get; set; }

        /// <summary>
        /// 获取或设置工作室提供的扩展性服务对象
        /// </summary>
        public WorkRoomService ExtendService { get; set; }

        /// <summary>
        /// 获取或设置工作室内所有工人集合
        /// </summary>
        public WorkerCollection Workers { get; set; }

        /// <summary>
        /// 全部工人开始工作
        /// </summary>
        public void StartWork()
        {
            StartWork("StartAllWorker");
        }

        /// <summary>
        /// 让特定命名的工人,立即开始工作
        /// </summary>
        /// <param name="workerName">工人名字</param>
        public void StartWork(string workerName)
        {
            foreach (IWorker worker in Workers)
            {
                try
                {
                    if (workerName != "StopAllWorker")
                    {
                        if (worker.Name == workerName)
                        {
                            worker.DoWork(this);
                            break;
                        }
                    }
                    else
                    {
                        worker.DoWork(this);
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteInfo(ex.ToString());
                    Logger.WriteException(worker.Name + ex.ToString());
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(5000);
                        try
                        {
                            Logger.WriteInfo(worker.Name + " try to restart again !");
                            worker.DoWork(this);
                            break;
                        }
                        catch
                        {
                            Logger.WriteInfo(worker.Name + " try to restart failure,would be retry after 5 seconds !");
                            Logger.WriteException(worker.Name + " try to restart failure exception description." + ex.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 让特定命名的工人,立刻结束工作
        /// </summary>
        /// <param name="workerName">工人名字</param>
        public void StopWork(string workerName)
        {
            if (string.IsNullOrWhiteSpace(workerName)) return;
            foreach (IWorker worker in Workers)
            {
                if (worker.Name == workerName && workerName != "StopAllWorker")
                {
                    worker.StopIt = true;
                    break;
                }
                else if (workerName == "StopAllWorker")
                    worker.StopIt = true;
            }
        }

        /// <summary>
        /// 全部工人结束工作
        /// </summary>
        public void StopWork()
        {
            StopWork("StopAllWorker");
        }
    }
}
