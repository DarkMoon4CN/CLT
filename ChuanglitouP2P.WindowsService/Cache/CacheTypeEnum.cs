///////////////////////////////////////////////////////////
//Name:缓存模型-数据缓存的类型
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// 数据缓存的类型
    /// </summary>
    public enum CacheTypeEnum
    {
        /// <summary>
        /// 数据缓存到时间后,立即删除
        /// </summary>
        OnTime,
        /// <summary>
        /// 数据缓存时间内,如果被获取,立即重置缓存时间，重新开始计时
        /// </summary>
        Activity,
        /// <summary>
        /// 数据被一次获取后,即刻删除
        /// </summary>
        Once
    }
}
