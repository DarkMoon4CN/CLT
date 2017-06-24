using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuangLiTou.Core.Helpers.SMSService;
using ChuanglitouP2P.Common;
namespace ChuangLiTou.Core.Helpers
{
    public class SmsHelper
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

        private static readonly SDKClientClient sdkService = new SDKClientClient();

         
        /// <summary>
        /// 等于0 发送成功
        /// </summary>
        /// <param name="mobile">手机</param>
        /// <param name="content">内容</param>
        /// <param name="type">1 文字短信（默认） 2 语音</param>
        /// <returns></returns>
        public static decimal Send(string mobile, string content, int type = 1)
        {
            decimal str = 0;
            try
            {
                if (type == 1)
                {
                    //亿美 验证码
                    str = sdkService.sendSMS(yzsn, yzpassword, null, mobile.Split(new char[] { ',' }), content, null, "GBK", 5, Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                }
                if (type == 2)
                {
                    sdkService.sendVoice(yzsn, yzpassword, null, mobile.Split(new char[] { ',' }), content, null, "GBK", 5, Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
                }

                 



            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
            }
            return str;
        }
    }
}
