///////////////////////////////////////////////////////////
//Name:框架模型-工作室提供的服务类
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System;
using System.Threading;
using System.Threading.Tasks;
namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// 工作室提供的服务类
    /// </summary>
    public class WorkRoomService
    {
        /// <summary>
        /// 工作室提供的服务类
        /// </summary>
        public WorkRoomService() { }

        public System.Timers.Timer CreateNewTimer()
        {
            return new System.Timers.Timer();
        }

        public Thread CreateNewThread(string name, AnonymousFunctionHandler function)
        {
            return new Thread(new ThreadStart(function));
        }

        public Task CreateNewTask(Action action)
        {
            return new Task(action);
        }
    }
}
