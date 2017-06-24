///////////////////////////////////////////////////////////
//Name:框架模型-工人实体抽象类
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System;
using System.Collections;

namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// 工人实体抽象类
    /// </summary>
    public abstract class Worker : IWorker
    {
        /// <summary>
        /// 获取唯一标识的显示名字
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// 获取或设置工作状态,是否停止工作
        /// </summary>
        public virtual bool StopIt
        {
            get; set;
        }

        /// <summary>
        /// 开始工作.抽象方法(派生类必须实现)
        /// </summary>
        /// <param name="workPosition"></param>
        public abstract void DoWork(WorkRoom workPosition);

        /// <summary>
        /// 获取或设置工作室
        /// </summary>
        public virtual WorkRoom WorkRoom { get; set; }
    }
}
