/***
 * 2016.4.20
 * 解志辉整理
 * ***/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;

namespace ChuanglitouP2P.Common
{
    public class RedisHelper
    {
        public static readonly string MasterRedisPath = Settings.Instance.MasterRedisPath;//主 写
        public static readonly string SlaveRedisPath = Settings.Instance.SlaveRedisPath;//从 读

        #region -- 连接信息 --
        public static PooledRedisClientManager prcm = CreateManager(new string[] { MasterRedisPath }, new string[] { SlaveRedisPath });
        private static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        {
            // 支持读写分离，均衡负载 
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
            {
                MaxWritePoolSize = 50, // “写”链接池链接数 
                MaxReadPoolSize = 50, // “读”链接池链接数 
                AutoStart = true,
            });
        }
        #endregion

        #region -- Item --
        /// <summary>
        /// 设置单体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static bool Add<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.Set<T>(key, t, new TimeSpan(1,0,0));
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
            }
            return false;
        }

        /// <summary>
        /// 获取单体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key) where T : class
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.Get<T>(key);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// 移除单体
        /// </summary>
        /// <param name="key"></param>
        public static bool Remove(string key)
        {
            try
            {


                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.Remove(key);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
            }
            return false;
        }

        #endregion

        #region -- List --
        /// <summary>
        /// 设置集合缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        public static void AddList<T>(string key, T t)
        {

            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    var redisTypedClient = redis.GetTypedClient<T>();
                    redisTypedClient.AddItemToList(redisTypedClient.Lists[key], t);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
            } 
        }


        /// <summary>
        /// 移除集合缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool RemoveList<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    var redisTypedClient = redis.GetTypedClient<T>();
                    return redisTypedClient.RemoveItemFromList(redisTypedClient.Lists[key], t) > 0;
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }

            return false;

        }

        /// <summary>
        /// 移除所有集合缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public static void RemoveAllList<T>(string key)
        {
            try
            {

                using (IRedisClient redis = prcm.GetClient())
                {
                    var redisTypedClient = redis.GetTypedClient<T>();
                    redisTypedClient.Lists[key].RemoveAll();
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }
        }
        /// <summary>
        /// 获取集合缓存个数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long GetListCount(string key)
        {
            try
            {

                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.GetListCount(key);
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }
            return 0;
        }
        /// <summary>
        /// 获取集合缓存某一范围的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<T> GetRangeList<T>(string key, int start, int count)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    var c = redis.GetTypedClient<T>();
                    return c.Lists[key].GetRange(start, start + count - 1);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// 获取集合缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(string key)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    var c = redis.GetTypedClient<T>();
                    return c.Lists[key].GetRange(0, c.Lists[key].Count);
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// 缓存List分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(string key, int pageIndex, int pageSize)
        {
            int start = pageSize * (pageIndex - 1);
            return GetRangeList<T>(key, start, pageSize);
        }

        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public static void SetListExpire(string key, DateTime datetime)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    redis.ExpireEntryAt(key, datetime);
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }

        }
        #endregion

        #region -- Set --
        public static void AddSet<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    var redisTypedClient = redis.GetTypedClient<T>();
                    redisTypedClient.Sets[key].Add(t);
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }

        }
        /// <summary>
        /// 判断Set集合是否包含指定的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool ContainsSet<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    var redisTypedClient = redis.GetTypedClient<T>();
                    return redisTypedClient.Sets[key].Contains(t);
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }
            return false;
        }
        /// <summary>
        /// 移除set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool RemoveSet<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    var redisTypedClient = redis.GetTypedClient<T>();
                    return redisTypedClient.Sets[key].Remove(t);
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }
            return false;
        }
        #endregion

        #region -- Hash --
        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool ExistHash<T>(string key, string dataKey)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.HashContainsEntry(key, dataKey);
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }
            return false;
        }
        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool AddHash<T>(string key, string dataKey, T t)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                    return redis.SetEntryInHash(key, dataKey, value);
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }
            return false;
        }
        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool RemoveHash(string key, string dataKey)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.RemoveEntryFromHash(key, dataKey);
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }
            return false;
        }
        /// <summary>
        /// 移除整个hash
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool RemoveHash(string key)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.Remove(key);
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }
            return false;
        }
        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static T GetHash<T>(string key, string dataKey)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    string value = redis.GetValueFromHash(key, dataKey);
                    return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(value);
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }
            return default(T);
        }

        /// <summary>
        /// 获取整个hash的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> GetAllHash<T>(string key)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    var list = redis.GetHashValues(key);
                    if (list != null && list.Count > 0)
                    {
                        List<T> result = new List<T>();
                        foreach (var item in list)
                        {
                            var value = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                            result.Add(value);
                        }
                        return result;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }
            return null;
        }
        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public static void SetExpireHash(string key, DateTime datetime)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    redis.ExpireEntryAt(key, datetime);
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }

        }
        #endregion

        #region -- SortedSet --
        /// <summary>
        ///  添加数据到 SortedSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="score"></param>
        public static bool AddSortedSet<T>(string key, T t, double score)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                    return redis.AddItemToSortedSet(key, value, score);
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }
            return false;
        }
        /// <summary>
        /// 移除数据从SortedSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool RemoveSortedSet<T>(string key, T t)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                    return redis.RemoveItemFromSortedSet(key, value);
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }
            return false;
        }
        /// <summary>
        /// 修剪SortedSet
        /// </summary>
        /// <param name="key"></param>
        /// <param name="size">保留的条数</param>
        /// <returns></returns>
        public static long TrimSortedSet(string key, int size)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.RemoveRangeFromSortedSet(key, size, 9999999);
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }
            return 0;
        }
        /// <summary>
        /// 获取SortedSet的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long GetSortedSetCount(string key)
        {
            try
            {

                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.GetSortedSetCount(key);
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }
            return 0;
        }

        /// <summary>
        /// 获取SortedSet的分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> GetSortedSetList<T>(string key, int pageIndex, int pageSize)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    var list = redis.GetRangeFromSortedSet(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1);
                    if (list != null && list.Count > 0)
                    {
                        List<T> result = new List<T>();
                        foreach (var item in list)
                        {
                            var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                            result.Add(data);
                        }
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }

            return null;
        }


        /// <summary>
        /// 获取SortedSet的全部数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> GetAllSortedSet<T>(string key)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    var list = redis.GetRangeFromSortedSet(key, 0, 9999999);
                    if (list != null && list.Count > 0)
                    {
                        return list.Select(ServiceStack.Text.JsonSerializer.DeserializeFromString<T>).ToList();
                    }
                }

            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public static void SetExpireSortedSet(string key, DateTime datetime)
        {
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    redis.ExpireEntryAt(key, datetime);
                }
            }
            catch (Exception ex)
            {

                LoggerHelper.Error(ex.ToString());
            }

        }

        #endregion
    }
}
