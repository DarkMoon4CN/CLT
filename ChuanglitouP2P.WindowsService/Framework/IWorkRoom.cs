namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// 工作室抽象接口类
    /// </summary>
    public interface IWorkRoom
    {
        /// <summary>
        /// 获取或设置工作室内的缓存对象
        /// </summary>
        ICaching Cache { get; set; }

        /// <summary>
        /// 获取或设置工作室内的数据库提供器对象
        /// </summary>
        IDatabaseProvider DatabaseProvider { get; set; }

        /// <summary>
        /// 获取或设置工作室提供的扩展性服务对象
        /// </summary>
        WorkRoomService ExtendService { get; set; }

        /// <summary>
        /// 获取或设置日志服务对象
        /// </summary>
        ILogger Logger { get; set; }

        /// <summary>
        /// 获取或设置工作室内所有工人集合
        /// </summary>
        WorkerCollection Workers { get; set; }

        /// <summary>
        /// 全部工人开始工作
        /// </summary>
        void StartWork();

        /// <summary>
        /// 全部工人结束工作
        /// </summary>
        void StopWork();

        /// <summary>
        /// 让特定命名的工人,立即开始工作
        /// </summary>
        void StartWork(string workerName);

        /// <summary>
        /// 让特定命名的工人,立刻结束工作
        /// </summary>
        /// <param name="workerName"></param>
        void StopWork(string workerName);
    }
}