///////////////////////////////////////////////////////////
//Name:缓存模型-缓存对象默认抽象接口类
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System;
using System.Web.Caching;
namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// 缓存对象默认抽象接口类
    /// </summary>
    public interface ICaching
    {
        ILogger Logger { get; set; }
        ICaching GetInstance { get; }
        long DefaultCachingSecond { get; set; }
        CacheTypeEnum DefaultCachingType { get; set; }

        T Get<T>(string key);
        T Get<T>(string key, Func<T> data);
        T Get<T>(string key, CacheTypeEnum type);
        T Get<T>(string key, Func<T> data, CacheTypeEnum type);
        T Get<T>(string key, long CachingSecond, Func<T> data);
        T Get<T>(string key, long CachingSecond, Func<T> data, CacheTypeEnum type);
        T Get<T>(string key, Func<T> data, CacheItemRemovedCallback removedCallback);
        T Get<T>(string key, Func<T> data, CacheTypeEnum type, CacheItemRemovedCallback removedCallback);
        T Get<T>(string key, long CachingSecond, Func<T> data, CacheItemRemovedCallback removedCallback);
        T Get<T>(string key, long CachingSecond, Func<T> data, CacheTypeEnum type, CacheItemRemovedCallback removedCallback);
    }
}
