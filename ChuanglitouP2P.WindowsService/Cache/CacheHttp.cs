///////////////////////////////////////////////////////////
//Name:缓存模型-IIS缓存实现类
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System;
using System.Web.Caching;
namespace ChuanglitouP2P.WindowsService
{
    public class CacheHttp : Caching
    {
        System.Web.Caching.Cache _cache = System.Web.HttpRuntime.Cache;

        /// <summary>
        /// IIS缓存实现类
        /// </summary>
        public CacheHttp() : base()
        {
            GetInstance = this;
        }

        public override T Get<T>(string key, long CachingSecond, Func<T> data, CacheTypeEnum type, CacheItemRemovedCallback removedCallback)
        {
            if (_cache != null)
            {
                var result = _cache.Get(key);
                if (result == null)
                {
                    if (data != null)
                    {
                        if (type == CacheTypeEnum.Activity)
                        {
                            if (removedCallback != null)
                                _cache.Add(key, data, null, DateTime.Now.AddSeconds(CachingSecond), new TimeSpan(CachingSecond * 10), System.Web.Caching.CacheItemPriority.Default, removedCallback);
                            else
                                _cache.Add(key, data, null, DateTime.Now.AddSeconds(CachingSecond), new TimeSpan(CachingSecond * 10), System.Web.Caching.CacheItemPriority.Default, null);
                        }
                        else if (type == CacheTypeEnum.OnTime)
                        {
                            if (removedCallback != null)
                                _cache.Add(key, data, null, DateTime.Now.AddSeconds(CachingSecond), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, removedCallback);
                            else
                                _cache.Add(key, data, null, DateTime.Now.AddSeconds(CachingSecond), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
                        }
                        return (T)_cache.Get(key);
                    }
                    else
                        return default(T);
                }
                else
                {
                    if (type == CacheTypeEnum.Once)
                        _cache.Remove(key);
                }
            }
            throw new PlatformNotSupportedException("System.Web.HttpRuntime.Cache can be used in this environment! try another way pls.");
        }
    }
}
