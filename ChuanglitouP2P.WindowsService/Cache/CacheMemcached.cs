///////////////////////////////////////////////////////////
//Name:缓存模型-Memcached缓存实现类
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System;
using System.Web.Caching;
namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// Memcached缓存实现类
    /// </summary>
    public class CacheMemcached : Caching
    {
        /// <summary>
        /// Memcached缓存实现类
        /// </summary>
        public CacheMemcached() : base()
        {
            GetInstance = this;
        }

        public override T Get<T>(string key, long CachingSecond, Func<T> data, CacheTypeEnum type, CacheItemRemovedCallback removedCallback)
        {
            throw new NotImplementedException();
        }
    }
}
