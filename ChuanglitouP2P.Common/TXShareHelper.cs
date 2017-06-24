using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Xml;

namespace ChuanglitouP2P.Common
{
    public class TXShareHelper
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        public string _ts;
        public string ts { get { return this._ts; } }
        /// <summary>
        /// 获取签名随机串
        /// </summary>
        public string _ns;
        public string ns { get { return this._ns; } }

        public string appid { get; set; }
        /// <summary>
        /// 获取签名
        /// </summary>
        public string _sign;
        public string sign { get { return this._sign; } }

        public string title { get; set; }
        public string desc { get; set; }
        public string link { get; set; }
        public string imgUrl { get; set; }

        public string ticket{ get; set; }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <returns></returns>
        public  string CheckSignature(string urlPath = "")
        {
            string tmpStr = "";
            string noncestr = "gemmy" + new Random().Next(1, 1000) + "W";
            _ns = noncestr.Trim();
            string jsapi_ticket = GetJsapi_ticket();
            ticket = jsapi_ticket;
            string timestamp = ConvertDateTimeInt(DateTime.Now).ToString();
            _ts = timestamp.Trim();

            string url = urlPath.Trim();
            //string url = WebMaster.Domain + urlPath;
            SortedList<string, string> SLString = new SortedList<string, string>();
            SLString.Add("noncestr", noncestr);
            SLString.Add("url", url);
            SLString.Add("timestamp", timestamp);
            SLString.Add("jsapi_ticket", jsapi_ticket);
            foreach (KeyValuePair<string, string> des in SLString)  //返回的是KeyValuePair，在学习的时候尽量少用var，起码要知道返回的是什么
            {
                tmpStr += des.Key + "=" + des.Value + "&";
            }
            if (tmpStr.Length > 0)
                tmpStr = tmpStr.Substring(0, tmpStr.Length - 1);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            _sign = tmpStr.ToLower().Trim();
            return tmpStr.ToLower().Trim();
        }


        /// <summary>  
        /// DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time"> DateTime时间格式</param>  
        /// <returns>Unix时间戳格式</returns>  
        public  int ConvertDateTimeInt(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }


        /// <summary>
        /// 获取微信Token
        /// </summary>
        /// <returns></returns>
        public  string GetJsapi_ticket()
        {
            string ACCESS_TOKEN = IsExistAccess_Token();
            string Jsapi_str = HttpGet("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + ACCESS_TOKEN + "&type=jsapi");
            Jsapi_ticket obj =JsonHelper.JsonToObject<Jsapi_ticket>(Jsapi_str);
            return obj.ticket;
        }

        /// <summary>
        /// http请求
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>0
        public  string HttpGet(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        public  Access_token GetAccess_token()
        {
            string aid = Utils.GetAppSetting("WeiXinAppid");
            string secret = Utils.GetAppSetting("WeiXinCode");
            string strUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + aid + "&secret=" + secret;
            Access_token mode = new Access_token();

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();

                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

                string content = reader.ReadToEnd();
                //Response.Write(content);
                //在这里对Access_token 赋值
                Access_token token = new Access_token();
                token = JsonHelper.JsonToObject<Access_token>(content);
                mode.access_token = token.access_token;
                mode.expires_in = token.expires_in;
            }
            return mode;
        }

        /// <summary>
        /// 根据当前日期 判断Access_Token 是否超期  如果超期返回新的Access_Token   否则返回之前的Access_Token
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public  string IsExistAccess_Token()
        {

            string Token = string.Empty;
            DateTime YouXRQ;
            // 读取XML文件中的数据，并显示出来 ，注意文件路径

            string filepath = System.Web.HttpContext.Current.Server.MapPath("/XMLWXACCESS.xml");

            StreamReader str = new StreamReader(filepath, System.Text.Encoding.UTF8);
            XmlDocument xml = new XmlDocument();
            xml.Load(str);
            str.Close();
            //str.Dispose();
            Token = xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText;
            YouXRQ = Convert.ToDateTime(xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText);

            if (DateTime.Now > YouXRQ)
            {
                DateTime _youxrq = DateTime.Now;
                Access_token mode = GetAccess_token();
                if (mode.access_token != null && mode.expires_in != null)
                {
                    xml.SelectSingleNode("xml").SelectSingleNode("Access_Token").InnerText = mode.access_token;
                    //_youxrq = _youxrq.AddSeconds(int.Parse(mode.expires_in));
                    _youxrq = _youxrq.AddSeconds(300);
                    xml.SelectSingleNode("xml").SelectSingleNode("Access_YouXRQ").InnerText = _youxrq.ToString();
                    xml.Save(filepath);
                    Token = mode.access_token;
                }
            }
            return Token;
            //string _access_token = IsExistAccess_Token();
        }

        public class Jsapi_ticket
        {
            public string errcode { get; set; }
            public string errmsg { get; set; }
            public string ticket { get; set; }
            public string expires_in { get; set; }
        }

    }
    /// <summary>
    ///Access_token 的摘要说明
    /// </summary>
    public class Access_token
    {
        public Access_token()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        string _access_token;
        string _expires_in;

        /// <summary>
        /// 获取到的凭证 
        /// </summary>
        public string access_token
        {
            get { return _access_token; }
            set { _access_token = value; }
        }

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public string expires_in
        {
            get { return _expires_in; }
            set { _expires_in = value; }
        }
    }

}
