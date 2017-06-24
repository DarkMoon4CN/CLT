///////////////////////////////////////////////////////////
//Name:框架模型-工人实体抽象接口
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
namespace ChuanglitouP2P.WindowsService
{
    public interface IWorker
    {

        /// <summary>
        /// 获取或设置工作室实体
        /// </summary>
        WorkRoom WorkRoom { get; set; }

        /// <summary>
        ///  获取或设置工作状态,是否停止工作
        /// </summary>
        bool StopIt { get; set; }

        /// <summary>
        /// 获取工人名字
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 开始工作
        /// </summary>
        /// <param name="workRoom">工作室实体</param>
        void DoWork(WorkRoom workRoom);
    }
}
