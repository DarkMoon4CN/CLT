///////////////////////////////////////////////////////////
//Name:日志模型-默认日志对象抽象接口类
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System;
namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// 默认日志对象抽象接口类
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message">日志内容</param>
        void WriteError(string message);
        /// <summary>
        /// 记录运行时异常日志
        /// </summary>
        /// <param name="message">日志内容</param>
        void WriteException(string message);
        /// <summary>
        /// 记录运行时异常日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="ex">运行时截获的异常实体对象</param>
        void WriteException(string message, Exception ex);
        /// <summary>
        /// 记录运行时信息日志
        /// </summary>
        /// <param name="message">日志内容</param>
        void WriteInfo(string message);
        /// <summary>
        /// 记录运行时调试日志
        /// </summary>
        /// <param name="message">日志内容</param>
        void WriteDebug(string message);
    }
}
