using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Common
{
    public static class PublicURL
    {
        static string RawUrl = System.Web.HttpContext.Current.Request.Url.Host;
        public static string NewPCUrl = "www.chuanglitou.cn";                               // "www.chuanglitou.cn";
        public static string NewWXUrl = "m.chuanglitou.cn";          //"m.chuanglitou.cn";
        public static string NewUrl = "chuanglitou.cn";       //"chuanglitou.cn";

        /// <summary>
        /// 测试域名
        /// </summary>
        /// <returns></returns>
        public static string getTestUrl()
        {
            string url = string.Empty;
            switch (RawUrl)
            {
                case "test.chuanglitou.cn":
                    url = "test.chuanglitou.cn";
                    break;
                case "testm.chuanglitou.cn":
                    url = "testm.chuanglitou.cn";
                    break;
            }
            return url;
        }
    }
}
