using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ChuanglitouP2P.Common
{
    public class CacheRemove
    {
        public static string _loginCachePrefix = "UserLogin_";


        public static void ClearAllCache()
        {
            RemoveWebCache("");
            //IDictionaryEnumerator CacheEnum = HttpRuntime.Cache.GetEnumerator();
            //while (CacheEnum.MoveNext())
            //{
            //    string name = CacheEnum.Key.ToString();
            //    if (HttpRuntime.Cache[name] != null && !name.StartsWith(_loginCachePrefix))
            //    {
            //        HttpRuntime.Cache.Remove(name);
            //    }
            //}
        }


        /// <summary>
        /// 清除友情链接缓存
        /// </summary>
        public static void ReMovetd_web_Links()
        {
            string CachKey = "td_web_Links-";
            //System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            //IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            //ArrayList al = new ArrayList();
            //while (CacheEnum.MoveNext())
            //{
            //    al.Add(CacheEnum.Key);
            //}

            //foreach (string key in al)
            //{
            //    if (key.Contains(CachKey))
            //    {
            //        _cache.Remove(key);
            //    }
            //}
            RemoveWebCache(CachKey);
        }


        /// <summary>
        /// 清除合作伙伴缓存
        /// </summary>
        public static void Removetd_web_Ad_type()
        {

            string CachKey = "td_web_Ad_type";
            //System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            //IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            //ArrayList al = new ArrayList();
            //while (CacheEnum.MoveNext())
            //{
            //    al.Add(CacheEnum.Key);
            //}

            //foreach (string key in al)
            //{
            //    if (key.Contains(CachKey))
            //    {
            //        _cache.Remove(key);
            //    }
            //}

            RemoveWebCache(CachKey);
        }






        /// <summary>
        /// 清除广告缓存
        /// </summary>
        public static void RemoveWebAdtype()
        {
            /*
            IDictionaryEnumerator CacheEnum =  Cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                string name = CacheEnum.Key.ToString();
                if (Cache[name] != null)
                {
                    Cache.Remove(name);
                }
            }
             */
            string CacheKey = "td_web_Ad_type";
            RemoveWebCache(CacheKey);
            //System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            //IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            //ArrayList al = new ArrayList();
            //while (CacheEnum.MoveNext())
            //{
            //    if (CacheEnum.Entry.Key.ToString().Contains(CacheKey))
            //    {
            //        HttpRuntime.Cache.Remove(CacheEnum.Entry.Key.ToString());
            //    }
            //}
            string url = "";
            if (Utils.GetAppSetting("DeBug") == "1")
            {
                url = Utils.GetAppSetting("MDeBugURL") + "Home/ClearCache";
            }
            else
            {
                url = Utils.GetAppSetting("MReleaseURL") + "Home/ClearCache";
            }
            HttpHelper.Get(url);
            //HttpRuntime.Cache.Remove()
            //foreach (string key in al)
            //{
            //    if (key.Contains(CacheKey))
            //    {
            //        _cache.Remove(key);
            //    }
            //}

        }


        #region 清除指定的服务器缓存
        /// <summary>
        /// 清除指定的服务器缓存
        /// </summary>
        public static void RemoveWebCache(string CacheKey)
        {
            /*
            IDictionaryEnumerator CacheEnum =  Cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                string name = CacheEnum.Key.ToString();
                if (Cache[name] != null)
                {
                    Cache.Remove(name);
                }
            }
             */


            //System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            //IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            //ArrayList al = new ArrayList();
            //while (CacheEnum.MoveNext())
            //{
            //    al.Add(CacheEnum.Key);
            //}

            //foreach (string key in al)
            //{
            //    if (key.Contains(CacheKey))
            //    {
            //        _cache.Remove(key);
            //    }
            //}

            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            //ArrayList al = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                string name = CacheEnum.Key.ToString();
                if (HttpRuntime.Cache[name] != null && name.StartsWith(_loginCachePrefix))
                {
                    continue;
                }
                if (string.IsNullOrWhiteSpace(CacheKey) || name.Contains(CacheKey))
                {
                    HttpRuntime.Cache.Remove(CacheEnum.Entry.Key.ToString());
                }
            }

        }
        #endregion


    }
}
