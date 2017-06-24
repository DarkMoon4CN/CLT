using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fastJSON;

namespace ChuanglitouP2P.Common
{
    public class FastJSON
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>json串</returns>
        public static string toJOSN(object obj)
        {

            JSONParameters jsparam = new JSONParameters();
            jsparam.UseExtensions = false;
            return fastJSON.JSON.Instance.ToJSON(obj, jsparam);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json">json串</param>
        /// <param name="obj">代转的对象</param>
        /// <returns>对象</returns>
        public static object ToObject(string json, object obj)
        {
            return fastJSON.JSON.Instance.ToObject(json, obj.GetType());
        }

        public static T ToObject<T>(string json)
        {

            return fastJSON.JSON.Instance.ToObject<T>(json);
        }


    }
}
