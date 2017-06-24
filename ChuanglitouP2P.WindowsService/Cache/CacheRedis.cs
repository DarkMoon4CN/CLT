///////////////////////////////////////////////////////////
//Name:缓存模型-Redis缓存实现类
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System;
using System.Web.Caching;
namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// Redis缓存实现类
    /// </summary>
    public class CacheRedis : Caching
    {
        /// <summary>
        /// Redis缓存实现类
        /// </summary>
        public CacheRedis() : base()
        {
            GetInstance = this;
        }

        public override T Get<T>(string key, long CachingSecond, Func<T> data, CacheTypeEnum type, CacheItemRemovedCallback removedCallback)
        {
            throw new NotImplementedException();
        }
    }
}
