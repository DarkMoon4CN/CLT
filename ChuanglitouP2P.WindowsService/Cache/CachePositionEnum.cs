///////////////////////////////////////////////////////////
//Name:缓存模型-数据缓存位置枚举
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// 数据缓存位置枚举
    /// </summary>
    public enum CachePositionEnum
    {
        /// <summary>
        /// IIS缓存
        /// </summary>
        Http,
        /// <summary>
        /// Memcached服务器
        /// </summary>
        Memcached,
        /// <summary>
        /// 服务器内存
        /// </summary>
        Memory,
        /// <summary>
        /// Redis服务器
        /// </summary>
        Redis,
        /// <summary>
        /// MongoDb数据
        /// </summary>
        MongoDb
    }
}
