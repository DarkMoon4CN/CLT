///////////////////////////////////////////////////////////
//Name:实体模型-调用者信息模型类
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// 调用者信息模型类
    /// </summary>
    public class CallerModel
    {
        /// <summary>
        /// 类名称
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 物理文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 所在行位置
        /// </summary>
        public int LineNumber { get; set; }
        /// <summary>
        /// 所在列位置
        /// </summary>
        public int ColumnNumber { get; set; }
    }
}
