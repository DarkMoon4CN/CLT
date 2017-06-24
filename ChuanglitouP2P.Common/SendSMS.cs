using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;



namespace ChuanglitouP2P.Common
{
    public class SendSMS
    {
       
        //道漫
        private static string sn = "SDK-WSS-010-07900";//序列号
        private static string password = "SMS@Clt#";//密码
        private static string pwd = "";


        //营销
        private static string ymsn = "6SDK-EMY-6688-JBUOO";//序列号
        private static string ympassword = "541095";//密码

        //验证码
        private static string yzsn = "6SDK-EMY-6688-JBUOQ";//序列号
        private static string yzpassword = "984312";//密码

        private static Webservice.SDKService sdkService = new Webservice.SDKService();

   

        /// <summary>
        /// 发送语音短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="txtcontent"></param>
        /// <returns></returns>
        public static decimal Send_Audio(string mobile, string txtcontent)
        {
            decimal str = 0;

          
            
          sdkService.sendVoice(yzsn, yzpassword, null, mobile.Split(new char[] { ',' }), txtcontent, null, "GBK", 5, Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")));

         
           /*
            pwd = getMD5(sn + password);

            Audio.WebServiceSoapClient audio = new Audio.WebServiceSoapClient();           
            string title = "语音短信";
            string content = "";
            string stime = "";//定时时间

            string scrnumber = "";
            string result = audio.mdAudioSend(sn, pwd, title, mobile, txtcontent, content, scrnumber, stime);

          

            if (result.StartsWith("-"))
            {
                str = -1M;

            }
            else
            {
                str = decimal.Parse(result);

            }
           */

            return str;
        }




        /// <summary>
        /// 等于0 发送成功
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static decimal Send_SMS(string mobile, string content)
        {
            decimal str = 0;

            //亿美发送  营销
            //str = sdkService.sendSMS(ymsn, ympassword, null, mobile.Split(new char[] { ',' }), content, null, "GBK", 5, Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
            LogInfo.WriteLog("mobiles:"+mobile);
            //亿美 验证码
            if (Settings.Instance.SiteDomain.IndexOf("chuanglitou.com") >= 0 || Settings.Instance.SiteDomain.IndexOf(PublicURL.NewUrl) >= 0)
            {

                LogInfo.WriteLog("send");
                str = sdkService.sendSMS(yzsn, yzpassword, null, mobile.Split(new char[] { ',' }), content, null, "GBK", 5, Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                LogInfo.WriteLog(string.Format("mobiles:{0}   result:{1}",mobile,str));
            }
            /*

             pwd = getMD5(sn + password);

             string url = "http://sdk.entinfo.cn:8061/mdsmssend.ashx";
             //content = HttpUtility.UrlEncode(content, Encoding.GetEncoding("gb2312"));
             string canshu = "sn=" + sn + "&pwd=" + pwd + "&mobile=" + mobile + "&content=" + content + "&ext=&stime=&rrid=&msgfmt=";
             string result = Post_Http(url, canshu);

             if (result.StartsWith("-") || result == "")
             {

                 str = -1M;
             }
             else
             {
                 str =decimal.Parse(result);
             }

          */
            return str;
        
        }



        public static decimal Send(string mobile, string content, int type = 1)
        {
            if (type == 1)
                return Send_SMS(mobile, content);
            return Send_Audio(mobile, content);
        }


        /// <summary>
        /// post方式提交数据
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="xmlbody">xml数据</param>
        /// <returns>执行结果</returns>
        public static string Post_Http(string url, string xmlbody)
        {
            try
            {
                Encoding encoding = Encoding.UTF8;

                string strUrl = url;
                byte[] data = encoding.GetBytes(xmlbody);

                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(strUrl);
                myRequest.Method = "post";
                myRequest.ContentType = "application/x-www-form-urlencoded;charset=utf8";
                myRequest.ContentLength = data.Length;

                myRequest.Timeout = 600000;

                Stream newStream = myRequest.GetRequestStream();
                // 发送数据 
                newStream.Write(data, 0, data.Length);
                newStream.Close();
                // Get response
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();

                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                //Response.Write(content);  
                return content;
            }
            catch (Exception exp)
            {
                return "发送异常";
            }
        }
        /// <summary>
        /// 获取md5码
        /// </summary>
        /// <param name="source">待转换字符串</param>
        /// <returns>md5加密后的字符串</returns>
        public static string getMD5(string source)
        {
            string result = "";
            try
            {
                MD5 getmd5 = new MD5CryptoServiceProvider();
                byte[] targetStr = getmd5.ComputeHash(UnicodeEncoding.UTF8.GetBytes(source));
                result = BitConverter.ToString(targetStr).Replace("-", "");
                return result;
            }
            catch (Exception)
            {
                return "0";
            }

        }


    }
}
