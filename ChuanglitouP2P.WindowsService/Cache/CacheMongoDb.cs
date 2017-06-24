///////////////////////////////////////////////////////////
//Name:缓存模型-MongoDb缓存提供类
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System;
using System.Web.Caching;
namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// MongoDb缓存提供类
    /// </summary>
    public class CacheMongoDb : Caching
    {
        /// <summary>
        /// MongoDb缓存提供类
        /// </summary>
        public CacheMongoDb() : base()
        {
            GetInstance = this;
        }

        public override T Get<T>(string key, long CachingSecond, Func<T> data, CacheTypeEnum type, CacheItemRemovedCallback removedCallback)
        {
            throw new NotImplementedException();
        }
    }
}
