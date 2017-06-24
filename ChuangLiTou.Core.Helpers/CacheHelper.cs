using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Caching;
using ChuangLiTou.Core.Helpers.Util;
using ChuanglitouP2P.Common;
namespace ChuangLiTou.Core.Helpers
{


    public class CacheHelper
    {
        public delegate String GenerateText();

        public delegate object GenerateObject();

        public static String GetText(CacheType type, int cacheKey, GenerateText callback)
        {
            return GetTextByName(type, cacheKey.ToString(CultureInfo.InvariantCulture), callback);
        }

        private static readonly Dictionary<String, object> Cachelock = new Dictionary<String, object>();

        public static String GetTextByName(CacheType type, string name, GenerateText callback)
        {
            if (String.IsNullOrEmpty(name)) return callback();
            if (HttpRuntime.Cache == null) return callback();

            var cacheKey = type + "-" + name;
            var cachedVersion = HttpRuntime.Cache[cacheKey] as String;

            if (cachedVersion == null)
            {
                if (!Cachelock.ContainsKey(cacheKey)) Cachelock.Add(cacheKey, new Object());

                lock (Cachelock[cacheKey])
                {
                    cachedVersion = callback();
                    CacheObject(type, cacheKey, cachedVersion);
                }
            }

            return cachedVersion;
        }


        public static object GetObject(CacheType type, int cacheKey, GenerateObject callback)
        {
            return GetObjectByName(type, cacheKey.ToString(CultureInfo.InvariantCulture), callback);
        }

        public static object GetObjectByName(CacheType type, string name, GenerateObject callback)
        {
            if (HttpRuntime.Cache == null) return callback();

            var cacheKey = type + "-" + name;
            var cachedVersion = HttpRuntime.Cache[cacheKey];

            if (cachedVersion == null)
            {
                if (!Cachelock.ContainsKey(cacheKey)) Cachelock.Add(cacheKey, new Object());

                lock (Cachelock[cacheKey])
                {
                    cachedVersion = HttpRuntime.Cache[cacheKey];
                    if (cachedVersion == null)
                    {
                        cachedVersion = callback();
                        CacheObject(type, cacheKey, cachedVersion);
                    }
                }
            }

            return cachedVersion;
        }
        //到期策略NoSlidingExpiration:绝对过期时间 NoAbsoluteExpiration:弹性过期时间
        private static void CacheObject(CacheType type, string cacheKey, object cachedVersion)
        {
            if (cachedVersion == null) return;
            switch (type)
            {
                case CacheType.LowCache:
                    HttpRuntime.Cache.Insert(cacheKey, cachedVersion, null, Settings.Instance.LowCache, Cache.NoSlidingExpiration);
                    break;
                case CacheType.LowerCache:
                    HttpRuntime.Cache.Insert(cacheKey, cachedVersion, null, Settings.Instance.LowerCache, Cache.NoSlidingExpiration);
                    break;
                case CacheType.HighCache:
                    HttpRuntime.Cache.Insert(cacheKey, cachedVersion, null, Settings.Instance.HighCache, Cache.NoSlidingExpiration);
                    break;
                case CacheType.HigherCache:
                    HttpRuntime.Cache.Insert(cacheKey, cachedVersion, null, Settings.Instance.HigherCache, Cache.NoSlidingExpiration);
                    break;
                case CacheType.HighestCache:
                    HttpRuntime.Cache.Insert(cacheKey, cachedVersion, null, Settings.Instance.HighestCache, Cache.NoSlidingExpiration);
                    break;
                default:
                    HttpRuntime.Cache.Insert(cacheKey, cachedVersion, null, Settings.Instance.NoSlidingExpirationTimespan, Cache.NoSlidingExpiration, CacheItemPriority.Low, null);
                    break;
            }
        }

        public static void ClearCache()
        {
            var cacheContents =
               HttpRuntime.Cache.GetEnumerator();
            while (cacheContents.MoveNext())
            {
                var currentKey = cacheContents.Key.ToString();
                HttpRuntime.Cache.Remove(currentKey);
            }
        }

        public static void ClearCache(string cacheKey)
        {
            var cache =
              HttpRuntime.Cache.Get(cacheKey);
            if (cache != null)
                HttpRuntime.Cache.Remove(cacheKey);
        }

        public static object GetCache(string cacheKey)
        {
            var level2 = HttpRuntime.Cache.Get(cacheKey);
            if (level2 == null && Settings.Instance.EnableLevel1Cache)
            {
                level2 = GetLevel1Cache(cacheKey);
                if (level2 != null)
                {
                    //缓存二级
                    SetLevel2Cache(cacheKey, level2);
                }
            }
            return level2;
        }
        /// <summary>
        /// 获取系统缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static object GetSystemCache(string cacheKey)
        {
            var cv = HttpRuntime.Cache.Get(cacheKey);

            return cv;
        }


        public static string GetCacheKeyByParam(IEnumerable<object> other)
        {
            var sbList = new StringBuilder();

            if (other != null)
            {
                var i = 1;
                foreach (var o in other)
                {
                    sbList.AppendFormat("-filter{0}-{1}", i, o);
                    i++;
                }
            }
            if (!string.IsNullOrEmpty(sbList.ToString()))
            {
                return sbList.ToString().TrimEnd('-');
            }
            return "";
        }


        public static string GetCacheKeyByParam(PageParam prams, IEnumerable<object> other)
        {
            var sbList = new StringBuilder("Cache");
            if (prams != null)
            {
                if (prams.PageCurrent >= 0)
                {
                    sbList.AppendFormat("-PageCurrent-{0}", prams.PageCurrent);
                }
                if (prams.PageSize >= 0)
                {
                    sbList.AppendFormat("-PageSize-{0}", prams.PageSize);
                }
            }
            if (other != null)
            {
                sbList.Append(GetCacheKeyByParam(other));
            }
            return sbList.ToString().ToLower();
        }

        public static void SetCache(string cacheKey, object objType)
        {
            if (Settings.Instance.EnableLevel1Cache)
            {
                SetLevel1Cache(cacheKey, objType);
            }
            if (Settings.Instance.EnableLevel2Cache)
            {
                var cachedVersion = HttpRuntime.Cache[cacheKey] as String;
                if (cachedVersion == null)
                {
                    SetLevel2Cache(cacheKey, objType);
                }
            }
        }

        public static void SetCache(string cacheKey, object objType, int cacheTime)
        {
            HttpRuntime.Cache.Insert(cacheKey, objType, null, DateTime.Now.AddMinutes(cacheTime),
                                                  Cache.NoSlidingExpiration);
        }
        /// <summary>
        /// 创建缓存，同时添加依赖项
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="objType"></param>
        /// <param name="dependencies">依赖项</param>
        public static void SetCache(string cacheKey, object objType, string[] dependencies)
        {

            //TODO: 管理员后台 供应商后台使用缓存依赖，二级缓存尽量不能使用缓存依赖,以后当有缓存服务器时修改
            if (Settings.Instance.EnableLevel1Cache)
            {
                SetLevel1Cache(cacheKey, objType);
            }
            if (Settings.Instance.EnableLevel2Cache)
            {
                var cachedVersion = HttpRuntime.Cache[cacheKey] as String;
                if (cachedVersion == null)
                {
                    SetLevel2Cache(cacheKey, objType, dependencies);
                }
            }
        }




        //TODO:暂时没有缓存服务器，一级缓存不做处理 以后修改
        static object GetLevel1Cache(string cacheKey)
        {
            return RedisHelper.Get<object>(cacheKey);
        }

        static void SetLevel1Cache(string cacheKey, object objType)
        {
            RedisHelper.Add(cacheKey, objType);
        }
        static void SetLevel2Cache(string cacheKey, object objType)
        {
            var cacheTime = cacheKey.IndexOf("entity", StringComparison.Ordinal) >= 0 ? Settings.Instance.EntityCacheTime : Settings.Instance.AssemblyCacheTime;

            HttpRuntime.Cache.Insert(cacheKey, objType, null, DateTime.Now.AddMinutes(cacheTime),
                                                  Cache.NoSlidingExpiration);
        }
        /// <summary>
        /// 设置二级缓存依赖项
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="objType"></param>
        /// <param name="dependencies"></param>
        private static void SetLevel2Cache(string cacheKey, object objType, string[] dependencies)
        {
            var cacheTime = cacheKey.IndexOf("entity", StringComparison.Ordinal) >= 0 ? Settings.Instance.EntityCacheTime : Settings.Instance.AssemblyCacheTime;

            HttpRuntime.Cache.Insert(cacheKey, objType, new CacheDependency(null, dependencies), DateTime.Now.AddMinutes(cacheTime),
                                                  Cache.NoSlidingExpiration);
        }
        static void ClearCache(string cacheKey, object objType)
        {

            if (Settings.Instance.EnableLevel1Cache)
            {
                ClearLevel1Cache(cacheKey);
                SetLevel1Cache(cacheKey, objType);
            }
            if (Settings.Instance.EnableLevel2Cache)
            {
                ClearLevel2Cache(cacheKey);
                SetLevel2Cache(cacheKey, objType);

            }
        }
        static void ClearLevel1Cache(string cacheKey)
        {

        }
        static void ClearLevel2Cache(string cacheKey)
        {
            var cache =
              HttpRuntime.Cache.Get(cacheKey);
            if (cache != null)
                HttpRuntime.Cache.Remove(cacheKey);
        }


        public static void UpdateCache(string cacheKey, object objType, bool syncCache)
        {


            /*如果是管理员或供应商 清除一级缓存
             *需要体现到前台页面的，清除二级缓存 eg:修改产品的价格 库存
             *注意 修改的只是单独的字段，不是清除此缓存,这样的话用户还要去一级缓存里面取数据，那就是最新的数据了
             *
            if (Settings.Instance.LocalDomain==EnumHelper.GetFieldText(Domains.Manage))/
            /*区分是管理员 供应商 会员*/
            if (syncCache)
            {
                /*如果同步缓存，清除一级和二级缓存,缓存最新数据*/
                ClearCache(cacheKey, objType);
            }
            else
            {
                /*如果开启了一级缓存，只处理一级缓存 || Settings.Instance.LocalDomain == EnumHelper.GetFieldText(Domains.Account)*/
                //if (Settings.Instance.LocalDomain == EnumHelper.GetFieldText(Domains.Manage))
                //{
                //    //如果是管理员后台 删除分页缓存
                //    var allCache=HttpContext.Current.Cache;
                //    for (int i = 0; i < allCache.Count; i++)
                //    {

                //    }
                //}
                if (Settings.Instance.EnableLevel1Cache)
                {
                    ClearLevel1Cache(cacheKey);
                    SetLevel1Cache(cacheKey, objType);
                }
                else
                {
                    //在没有缓存服务器时，一级缓存没有用
                    ClearCache(cacheKey, objType);
                }
            }

        }
     
    }
}
