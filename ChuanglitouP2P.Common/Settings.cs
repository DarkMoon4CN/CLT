using ChuanglitouP2P.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Xml;
using System.Threading;
using System.Xml.Linq;
using System.Linq;
using System.Text;
namespace ChuanglitouP2P.Common
{
    public class Settings
    {
        public const string Salt = "chuantlitou";

        private static Settings _instance = new Settings();

        /// <summary>
        /// Get setting
        /// </summary>
        public static Settings Instance
        {
            get { return _instance ?? (_instance = new Settings()); }
        }
        public T GetSetting<T>(String key, T defaultValue)
        {
            if (ConfigurationManager.AppSettings[key] == null)
                return defaultValue;
            if (typeof(T) == typeof(String))
                return (T)(object)ConfigurationManager.AppSettings[key];
            if (typeof(T) == typeof(long))
                return (T)(object)(Int64.Parse(ConfigurationManager.AppSettings[key]));
            if (typeof(T) == typeof(int))
                return (T)(object)(Int32.Parse(ConfigurationManager.AppSettings[key]));
            if (typeof(T) == typeof(bool))
                return (T)(object)(bool.Parse(ConfigurationManager.AppSettings[key]));
            return (T)(object)ConfigurationManager.AppSettings[key];
        }


        public string SiteDomain
        {
            get
            {
                // if it's called from eg a Task
                if (System.Web.HttpContext.Current == null)
                {
                    return GetSetting("SiteDomain", "/");
                }

                string url = HttpContext.Current.Request.Headers["Host"];

                return "http://" + url;
            }
        }

        public string GetErrorMsg(string code)
        {
            lock (this)
            {
                var p = DataRoot + "/Static/files/Errors.xml";
                var xml = new XmlHelper();
                xml.Load(p);
                var lst = xml.SelectNodes("Error");
                if (lst != null && lst.Count > 0)
                {
                    foreach (XmlElement item in from XmlElement item in lst where item != null where item.Attributes["ErrorCode"].Value.ToLowerInvariant() == code.ToLowerInvariant() select item)
                    {
                        return item.Attributes["ErrorInfo"].Value;
                    }
                }
            }
            return "";
        }

        public string DataRoot
        {
            get
            {
                var st = ResolvePath(GetSetting("DataRoot", ""));
                if (string.IsNullOrEmpty(st))
                {
                    st = HttpRuntime.AppDomainAppPath.ToString();
                }
                return st;
            }
        }

        public string GetSql<T>(object data)
        {
            ObjectQuery<T> oq = data as ObjectQuery<T>;
            String sql = oq.ToTraceString();
            return sql;
        }

        private string ResolvePath(string p)
        {
            if (p.ToLower().StartsWith(@"%dataroot%\"))
                p = Path.Combine(DataRoot, p.Substring(11));
            if (p.ToLower().StartsWith(@"%dataroot%"))
                p = Path.Combine(DataRoot, p.Substring(10));
            var extension = Path.GetExtension(p);
            if (!string.IsNullOrEmpty(extension) && !extension.ToLower().EndsWith("dat"))
            {
                var dir = new DirectoryInfo(p);
                if (!dir.Exists) dir.Create();
            }
            else if (!string.IsNullOrEmpty(p) && string.IsNullOrEmpty(extension))
            {
                var dir = new DirectoryInfo(p);
                if (!dir.Exists) dir.Create();
            }

            return p;

        }

        public String LogPath
        {
            get
            {
                return ResolvePath(GetSetting("LogPath", @"%dataroot%\Log"));

            }
        }

        public int CurrentUserId
        {
            get
            {
                var uId = 0;
                var ent = GetUser();
                if (ent != null)
                {
                    uId = ent.userid;
                }
                return uId;


            }
        }

        public M_login GetUser()
        {

            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];//获取cookie
            if (authCookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);//解密
                return SerializeHelper.Instance.JsonDeserialize<M_login>(ticket.UserData);//反序列化
            }
            return null;
        }
        public string ProjectDatabase
        {
            get
            {

                return ConfigurationManager.ConnectionStrings["ProjectDatabase"].ConnectionString;

            }
        }

        /// <summary>
        /// 返回两个日期之间的时间间隔
        /// </summary>
        /// <param name="Interval"> 返回类型 秒 ,分,小时,天等</param>
        /// <param name="StartDate">起始时间</param>
        /// <param name="EndDate">结束时间</param>
        /// <returns>返回两个日期之间的时间间隔</returns>

        public long DateDiff(string Interval, DateTime StartDate, DateTime EndDate)
        {
            long lngDateDiffValue = 0;


            TimeSpan TS = new TimeSpan(EndDate.Ticks - StartDate.Ticks);
            switch (Interval)
            {
                case "Second":
                    lngDateDiffValue = (long)TS.TotalSeconds;
                    break;
                case "Minute":
                    lngDateDiffValue = (long)TS.TotalMinutes;
                    break;
                case "Hour":
                    lngDateDiffValue = (long)TS.TotalHours;
                    break;
                case "Day":
                    lngDateDiffValue = (long)TS.Days;
                    break;
                case "Week":
                    lngDateDiffValue = (long)(TS.Days / 7);
                    break;
                case "Month":
                    //lngDateDiffValue = (long)(TS.Days / 30);
                    //应取两个时间的月份之差(季度和年同理) 
                    lngDateDiffValue = (long)(EndDate.Year - StartDate.Year) * 12 + (EndDate.Month - StartDate.Month);
                    break;
                case "Quarter":
                    //lngDateDiffValue = (long)((TS.Days / 30) / 3);
                    lngDateDiffValue = (long)(EndDate.Year - StartDate.Year) * 4 + (Quarter(EndDate) - Quarter(StartDate));
                    break;
                case "Year":
                    //lngDateDiffValue = (long)(TS.Days / 365);
                    lngDateDiffValue = (long)(EndDate.Year - StartDate.Year);
                    break;
            }
            return (lngDateDiffValue);
        }

        /// <summary>
        /// 以万为单位显示 t 为真四舍五入 t=false 不进行四舍五入
        /// </summary>
        /// <param name="number">金额</param>
        /// <param name="dec">小数位数</param>
        /// <param name="t">是否四舍五入</param>
        /// <returns></returns>
        public string GetWebConvertdisp(decimal number, int dec, bool t)
        {
            string str = "";
            decimal deci = 10000M;
            if (number < deci)
            {

                if (dec == 0)
                {
                    str = number.ToString("0");
                }
                else
                {
                    if (t == true)
                    {
                        str = GetDecimal(number, dec, true).ToString();
                    }
                    else
                    {
                        str = GetDecimal(number, dec, false).ToString();
                    }

                }
            }
            else if (number >= deci)
            {

                decimal df = number / deci;

                //   string strDecPart = "";                    // 存放小数部分的处理结果 
                // 存放整数部分的处理结果 
                string[] tmp = null;
                string strDigital = df.ToString();

                tmp = strDigital.Split(cDelim, 2); // 将数据分为整数和小数部分 

                if (tmp.Length > 1) //分解出了小数
                {
                    // strDecPart = ConvertDecimal(tmp[1]);


                    if (decimal.Parse(tmp[1]) > 0)
                    {
                        str = df.ToString("0.00") + "万";
                    }
                    else
                    {
                        str = df.ToString("0") + "万";
                    }

                }
                else
                {
                    str = df.ToString("0") + "万";
                }
            }



            return str;


        }



        #region GetDecimal
        private char[] cDelim = { '.' }; //小数分隔标识
        /// <summary>
        /// 获取几位小数点 bool t为真则四舍五入返回小数点位数,t为假则不进行四舍五入返回小数位数,默认为真
        /// </summary>
        /// <param name="rmb">The RMB.</param>
        /// <param name="n">保留小数点位数</param>
        /// <param name="t">if set to <c>true</c> [t].</param>
        /// <returns>返回字符串</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 10:04:29
        public decimal GetDecimal(decimal rmb = 0.00m, int n = 2, bool t = true)
        {
            decimal dec = 0.00M;

            if (t)
            {
                dec = Math.Round(rmb, n, MidpointRounding.AwayFromZero);
            }
            else
            {
                string[] tmp = null;
                string strDigital = rmb.ToString();
                tmp = strDigital.Split(cDelim, 2); // 将数据分为整数和小数部分 
                if (tmp.Length > 1) //分解出了小数
                {
                    if (tmp[1].Length <= n)
                    {
                        dec = rmb;
                    }
                    else
                    {
                        string dec1 = tmp[1].Substring(0, n);
                        strDigital = tmp[0] + "." + dec1;
                        dec = decimal.Parse(strDigital);
                    }

                }
                else
                {
                    dec = rmb;
                }
            }
            return dec;
        }
        #endregion



        /// <summary>
        /// 取得某个日期是本年度的第几个季度.
        /// </summary>
        /// <param name="tDate">The t date.</param>
        /// <returns>System.Int32.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 10:04:36
        public int Quarter(DateTime tDate)
        {
            switch (tDate.Month)
            {
                case 1:
                case 2:
                case 3:
                    return 1;

                case 4:
                case 5:
                case 6:
                    return 2;

                case 7:
                case 8:
                case 9:
                    return 3;

                default:
                    return 4;
            }

        }

        /// <summary>
        /// 汇付商户号
        /// </summary>
        public string MerId { get { return GetSetting("MerId", ""); } }

        /// <summary>
        /// 汇付客户号
        /// </summary>
        public string MerCustId { get { return GetSetting("MerCustId", ""); } }
        /// <summary>
        ///汇付接口接址
        /// </summary>
        public string ChinapnrUrl { get { return GetSetting("ChinapnrUrl", ""); } }
        /// <summary>
        /// 汇付接口返回地址
        /// </summary>
        public string ReUrl { get { return GetSetting("ReUrl", ""); } }
        /// <summary>
        /// 汇付商户公钥
        /// </summary>
        public string PgPubk { get { return GetSetting("PgPubk", ""); } }
        /// <summary>
        /// 汇付商户私钥
        /// </summary>
        public string MerPr { get { return GetSetting("MerPr", ""); } }



        public bool EnableCache { get { return GetSetting("EnableCache", false); } }
        public bool EnableLevel1Cache { get { return GetSetting("EnableLevel1Cache", false); } }
        public bool EnableLevel2Cache { get { return GetSetting("EnableLevel2Cache", false); } }


        public string Random { get { return GetSetting("Random", DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture)); } }


        public DateTime LowCache
        {
            get { return GetSetting("LowCache", DateTime.Now.AddMinutes(2)); }
        }
        public DateTime LowerCache
        {
            get { return GetSetting("LowerCache", DateTime.Now.AddMinutes(20)); }
        }
        public DateTime NormalCache
        {
            get { return GetSetting("NormalCache", DateTime.Now.AddHours(2)); }
        }
        public DateTime HighCache
        {
            get { return GetSetting("HighCache", DateTime.Now.AddDays(1)); }
        }
        public DateTime HigherCache
        {
            get { return GetSetting("HigherCache", DateTime.Now.AddDays(7)); }
        }
        public DateTime HighestCache
        {
            get { return GetSetting("HigherCache", DateTime.Now.AddDays(30)); }
        }

        public string Guid
        {
            get { return System.Guid.NewGuid().ToString("N"); }
        }

        public string ClientIp2
        {
            get
            {
                string Ip = string.Empty;

                Ip = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                if (string.IsNullOrEmpty(Ip))
                {


                    if (HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_FORWARDED_FOR"] == null)
                    {
                        if (HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null)
                            Ip = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].ToString();
                        else
                            if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                            Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                        else
                            Ip = null;
                    }
                    else
                        Ip = HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_FORWARDED_FOR"];

                }
                return Ip;
            }
        }
        public string ClientIp
        {
            get
            {
                object clientIp;
                if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    clientIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
                else
                {
                    clientIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (null == clientIp)
                {
                    clientIp = HttpContext.Current.Request.UserHostAddress;
                }
                if (clientIp != null)
                    return clientIp.ToString();
                return null;
            }
        }


        public TimeSpan NoAbsoluteExpirationTimespan { get { return GetSetting("NoAbsoluteExpirationTimespan", TimeSpan.FromDays(1)); } }
        public DateTime NoSlidingExpirationTimespan { get { return GetSetting("NoSlidingExpirationTimespan", DateTime.Now.AddMinutes(20)); } }


        public string EncryptKey
        {
            get
            {
                return GetSetting("EncryptKey", "");
            }
        }


        public string AssemblyNameSpace
        {

            get { return GetSetting("AssemblyNameSpace", "ChuangLiTou.Core.ImplDal"); }
        }

        public int EntityCacheTime
        {

            get { return GetSetting("EntityCacheTime", 20); }
        }

        public int AssemblyCacheTime
        {

            get { return GetSetting("AssemblyCacheTime", 20); }
        }



        public string MasterRedisPath
        {
            get { return GetSetting("MasterRedisPath", ""); }
        }

        public string SlaveRedisPath
        {
            get
            {
                var sr = GetSetting("SlaveRedisPath", "");
                if (string.IsNullOrEmpty(sr))
                    return MasterRedisPath;
                return sr;
            }
        }

        public string ThirdUserXmlPath
        {
            get { return GetSetting("ThirdUserXmlPath", "static/files/ThirdApiUsers.xml"); }


        }

        public string PdfRoot
        {
            get { return GetSetting("PdfRoot", HttpContext.Current.Request.PhysicalApplicationPath); }

        }

        public string OrderCode
        {

            get
            {
                return
                    DateTime.Now.ToString("yyyyMMddHHmmssffff") + Utils.RndNum(2);

            }
        }

        public string SiteVersion { get { return GetSetting("SiteVersion", Guid); } }

        public string TaskSettingFolder { get { return ResolvePath(GetSetting("TaskSettingFolder", @"%dataroot%\Log\Settings")); } }

        /// <summary>
        /// 修改配置文件.
        /// </summary> 
        /// <param name="path"></param>
        /// <param name="nodes"></param>
        /// <param name="prmKey"></param>
        /// <param name="prmValue"></param>
        /// <param name="newAttrInfo">添加的属性值</param>
        /// 创建者：解志辉
        /// ------------------------------------
        public void ModifyConfig(string path = "", string nodes = "", string prmKey = "", string prmValue = "", IEnumerable<KeyValue> newAttrInfo = null)
        {

            lock (this)
            {
                var p = DataRoot + path;
                var doc = new XmlDocument();
                doc.Load(p);
                XmlNodeList nodeList = doc.SelectNodes(nodes);
                foreach (XmlNode node in nodeList)
                {
                    var v = node.Attributes[prmKey].Value;
                    if (v == prmValue)
                    {
                        if (newAttrInfo != null)
                        {
                            var i = 1;
                            foreach (var o in newAttrInfo)
                            {

                                node.Attributes.Append(CreateAttribute(node, o.Key, o.Value));

                            }
                        }

                    }

                }
                doc.Save(p);
            }


        }


        public XmlAttribute CreateAttribute(XmlNode node, string attributeName, string value)
        {
            try
            {
                XmlDocument doc = node.OwnerDocument;
                XmlAttribute attr = null;
                attr = doc.CreateAttribute(attributeName);
                attr.Value = value;
                node.Attributes.SetNamedItem(attr);
                return attr;
            }
            catch (Exception err)
            {
                string desc = err.Message;
                return null;
            }
        }

        public string RemoveScript(string htmlstring)
        {
            //删除脚本   
            htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);


            htmlstring = htmlstring.Replace("<form", "<textarea style='width:900px;height:200px;'><form");
            htmlstring = htmlstring.Replace("</form>", "</form></textarea>");

            return htmlstring;
        }

        public string RemoveHtml(string htmlstring)
        {
            //删除脚本   
            htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            htmlstring = htmlstring.Replace("<", "");
            htmlstring = htmlstring.Replace(">", "");
            htmlstring = htmlstring.Replace("\r\n", "");
            var htmlEncode = HttpContext.Current.Server.HtmlEncode(htmlstring);
            if (htmlEncode != null)
                htmlstring = htmlEncode.Trim();

            return htmlstring;
        }

        public string[] SplitString(string strContent, string strSplit)
        {
            if (strContent.IndexOf(strSplit) < 0)
            {
                string[] tmp = { strContent };
                return tmp;
            }
            return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 缓存汇付第三方并发通知 防并发 服务器缓存 3分钟
        /// </summary>
        /// <param name="CacheName">用户ID+操作项id+订单号</param>
        /// <returns>返回0时说明没有缓存</returns>
        public long GeTThirdCache(string CacheName)
        {
            long str = 0;
            string key = CacheName;
            if (HttpRuntime.Cache[key] == null)
            {
                return 0;
            }
            else
            {
                str = (long)HttpRuntime.Cache[key];
            }
            return str;
        }

        /// <summary>
        /// 缓存汇付第三方并发通知   防并发 服务器缓存 3分钟
        /// </summary>
        /// <param name="CacheName"></param>
        public void SetThirdCache(string CacheName)
        {
            object lockObj = new object();
            lock (lockObj)
            {
                long str = DateTime.Now.Ticks;
                lock (this)
                {
                    string key = CacheName;
                    if (HttpRuntime.Cache[key] == null)
                    {
                        HttpRuntime.Cache.Add(key, str, null, DateTime.Now.AddSeconds(180), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                    }
                }
            }
        }

        ///// <summary>
        ///// 获取HttpRuntime.Cache缓存中的数据
        ///// </summary>
        ///// <typeparam name="T">返回的数据类型</typeparam>
        ///// <param name="cacheKey">缓存的关键字</param>
        ///// <param name="getValue">如果缓存值不存在,重新装载数据的执行方法</param>
        ///// <param name="expiredMinutes">缓存数据超时时间(单位：分钟).默认3分钟</param>
        ///// <returns></returns>
        //public T GetCachingValue<T>(string cacheKey, Func<T> getValue, double expiredMinutes = 3)
        //{
        //    object lockObj = new object();
        //    lock (lockObj)
        //    {
        //        lock (this)
        //        {
        //            if (HttpRuntime.Cache[cacheKey] == null)
        //            {
        //                HttpRuntime.Cache.Add(cacheKey, getValue(), null, DateTime.Now.AddSeconds(expiredMinutes * 60), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
        //            }
        //            return (T)HttpRuntime.Cache[cacheKey];
        //        }
        //    }
        //}
        public string GetCallbackUrl(string path)
        {
            return Settings.Instance.SiteDomain + path;
        }
        public string QRCodeLink
        {
            get
            {
                return GetSetting("QRCodeLink", "");
            }
        }
        public string WebPass
        {
            get
            {
                return GetSetting("webp", @"");
                //return ResolvePath(GetSetting("webp", @""));
            }
        }
        /// <summary>
        /// 由于win10系统获取到的时间会带上星期几 上午下午，用此方式重新定义一下格式便于测试
        /// </summary>
        public void SetSYSDateTimeFormat()
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd HH:mm:ss";
            culture.DateTimeFormat.LongTimePattern = "";
            Thread.CurrentThread.CurrentCulture = culture;
        }
        /// <summary>
        /// 汇付专属账号-手续费收取子帐户
        /// </summary>
        public string MerDt
        {
            get
            {
                return GetSetting("MERDT", "");
            }
        }
        public string ImagesDomain { get { return GetSetting("WebsiteUrlAddress", "http://" + PublicURL.NewWXUrl + ""); } }
        /// <summary>
        /// 上传图片返回json地址链接
        /// </summary>
        public string ImagesAvater { get { return GetSetting("WebsiteUrlAddress", "http://" + PublicURL.NewPCUrl + ""); } }

        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Base64Encoder(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            string orgStr = Convert.ToBase64String(bytes);
            return orgStr;
        }

        /// <summary>
        /// 获取xml配置文件数据. demo:  <SiteVersion Value="20150317134602" />
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">xml 路径 demo D:\\Log.xml </param>
        /// <param name="rootAttrName">User</param>
        /// <param name="targetAttrKey">目标属性名称 结合属性值判断唯一：UserName</param>
        /// <param name="targetAttrValue">属性值，判断唯一的：chuanglitou</param>
        /// <param name="getAttrKey">想要获取的属性名称： Password</param>
        /// <returns>20150317134602</returns>
        public string GetConfig(string path = "", string rootAttrName = "", string targetAttrKey = "", string targetAttrValue = "", string getAttrKey = "")
        {
            lock (this)
            {
                var p = DataRoot + path;
                var xml = new XmlHelper();
                xml.Load(p);
                var lst = xml.SelectNodes(rootAttrName);
                if (lst != null && lst.Count > 0)
                {
                    foreach (XmlElement item in from XmlElement item in lst where item != null where item.Attributes[targetAttrKey].Value.ToLowerInvariant() == targetAttrValue.ToLowerInvariant() select item)
                    {
                        return item.Attributes[getAttrKey].Value;
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 获取主站所在机器的物理路径
        /// </summary>
        public string GetWebsitePhysicalRootPath
        {
            get { return GetSetting("WebsitePhysicalRootPath", @"D:\\v2test"); }
        }

        /// <summary>
        /// 获取客户服务电话
        /// </summary>
        public string GetCustomerServiceNumber
        {
            get { return GetSetting("CustomerServiceNumber", "010-53732056"); }
        }

        public string GetPlatformStatisticsReportUrl
        {
            get
            {
                return GetSetting("PlatformStatisticsReportUrl", "http://m.chuanglitou.cn/bigdata.html");
            }
        }

        public string GetRechargeRuleDescriptionUrl
        {
            get
            {
                return GetSetting("RechargeRuleDescriptionUrl", "http://testm.chuanglitou.cn/BigData/RechargeTips");
            }
        }

        public string GetCalendarAppKey
        {
            get
            {
                return GetSetting("CalendarServiceKey", "1b9f82ea3ce818c39bca6cffbfce31a0");
            }
        }
        public string GetWeixinUrlAddress
        {
            get
            {
                return GetSetting("WeiXinUrlAddress", "http://testm.chuanglitou.cn");
            }
        }
    }

    public class KeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }

    }
}
