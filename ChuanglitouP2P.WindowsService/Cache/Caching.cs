///////////////////////////////////////////////////////////
//Name:缓存模型-缓存对象默认实体抽象类
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System;
using System.Web.Caching;
namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// 缓存对象默认实体抽象类
    /// </summary>
    public abstract class Caching : ICaching
    {
        /// <summary>
        /// 缓存对象默认实体抽象类
        /// </summary>
        public Caching()
        {
            DefaultCachingSecond = 30;
            DefaultCachingType = CacheTypeEnum.OnTime;
            GetInstance = this;
        }
        public virtual T Get<T>(string key)
        {
            return Get<T>(key, DefaultCachingSecond, null, DefaultCachingType, null);
        }
        public virtual long DefaultCachingSecond
        {
            get; set;
        }
        public virtual ILogger Logger
        {
            get; set;
        }
        public virtual CacheTypeEnum DefaultCachingType
        {
            get; set;
        }
        public virtual ICaching GetInstance
        {
            get; protected set;
        }
        public virtual T Get<T>(string key, Func<T> data)
        {
            return Get<T>(key, DefaultCachingSecond, data, DefaultCachingType, null);
        }
        public virtual T Get<T>(string key, CacheTypeEnum type)
        {
            return Get<T>(key, DefaultCachingSecond, null, type, null);
        }
        public virtual T Get<T>(string key, Func<T> data, CacheTypeEnum type)
        {
            return Get<T>(key, DefaultCachingSecond, data, type, null);
        }
        public virtual T Get<T>(string key, long CachingSecond, Func<T> data)
        {
            return Get<T>(key, CachingSecond, data, DefaultCachingType, null);
        }
        public T Get<T>(string key, long CachingSecond, Func<T> data, CacheTypeEnum type)
        {
            return Get<T>(key, CachingSecond, data, type, null);
        }
        public T Get<T>(string key, Func<T> data, CacheItemRemovedCallback removedCallback)
        {
            return Get<T>(key, DefaultCachingSecond, data, DefaultCachingType, removedCallback);
        }
        public T Get<T>(string key, Func<T> data, CacheTypeEnum type, CacheItemRemovedCallback removedCallback)
        {
            return Get<T>(key, DefaultCachingSecond, data, DefaultCachingType, removedCallback);
        }
        public T Get<T>(string key, long CachingSecond, Func<T> data, CacheItemRemovedCallback removedCallback)
        {
            return Get<T>(key, DefaultCachingSecond, data, DefaultCachingType, removedCallback);
        }
        public abstract T Get<T>(string key, long CachingSecond, Func<T> data, CacheTypeEnum type, CacheItemRemovedCallback removedCallback);
    }
}
