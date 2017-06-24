///////////////////////////////////////////////////////////
//Name:缓存模型-内存缓存提供类
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System;
using System.Web.Caching;
using System.Runtime.Caching;
namespace ChuanglitouP2P.WindowsService.Cache
{
    /// <summary>
    /// 内存缓存提供类
    /// <remark>
    /// 1.不支持CachingType.Activity类型,所有类型都被转换为CachingType.OnTime.
    /// 2.不支持CacheItemRemovedCallback方法,不提供缓存对象过期后执行回调操作.
    /// 3.共享内存块默认命名为 ChuangLiTouP2PCachingGeraintXue.可以使用MemoryName属性变更,但重命名后原命名内存块资源无法访问.建议默认命名即可.
    /// </remark>
    /// </summary>
    public class CacheMemory : Caching
    {
        private string _MemoryName = "ChuangLiTouP2PCachingGeraintXue";
        MemoryCache _cache = null;

        /// <summary>
        /// 内存缓存提供类
        /// <remark>
        /// 1.不支持CachingType.Activity类型,所有类型都被转换为CachingType.OnTime.
        /// 2.不支持CacheItemRemovedCallback方法,不提供缓存对象过期后执行回调操作.
        /// 3.共享内存块默认命名为 ChuangLiTouP2PCachingGeraintXue.可以使用MemoryName属性变更,但重命名后原命名内存块资源无法访问.建议默认命名即可.
        /// </remark>
        /// </summary>
        public CacheMemory() : base()
        {
            GetInstance = this;
        }

        public string MemoryName
        {
            get { return _MemoryName; }
            set { _MemoryName = value; }
        }

        public override T Get<T>(string key, long CachingSecond, Func<T> data, CacheTypeEnum type, CacheItemRemovedCallback removedCallback)
        {
            if (_cache == null)
            {
                try
                {
                    _cache = new MemoryCache(MemoryName);
                }
                catch (Exception ex)
                {
                    throw new PlatformNotSupportedException("System.Runtime.Caching.CacheMemory can be used in this environment! try another way pls.");
                }
            }
            T result = (T)_cache.Get(key);
            if (result == null)
            {
                if (data != null)
                {
                    _cache.Add(key, data, DateTimeOffset.Now.AddSeconds(CachingSecond));
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
            return result;
        }
    }
}
