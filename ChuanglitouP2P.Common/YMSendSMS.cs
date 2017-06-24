using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace ChuanglitouP2P.Common
{

    public class YMSendSMS
    {
        /*
        private static string sn = "6SDK-EMY-6688-JBUOQ";//序列号
        private static string password = "514533";//密码
         */

        //营销
        private static string ymsn = "6SDK-EMY-6688-JBUOO";//序列号
        private static string ympassword = "541095";//密码
        private static Webservice.SDKService sdkService = new Webservice.SDKService();

        //验证码
        private static string yzsn = "6SDK-EMY-6688-JBUOQ";//序列号
        private static string yzpassword = "984312";//密码
        /// <summary>
        /// 发送短信   亿美发送短信方式
        /// </summary>
        /// <param name="mobiles">mobiles 最多为 200个号码，中间用 ","隔开</param>
        /// <param name="smscontext">短信内容(最多500个汉字或1000个纯英文，emay服务器程序能够自动分割</param>
        /// <returns></returns>
        public static int Send_SMS(string mobiles, string smscontext)
        {
            smscontext = smscontext + "回复td退订";
            return sdkService.sendSMS(ymsn, ympassword, null, mobiles.Split(new char[] { ',' }), smscontext, null, "GBK", 5, Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
        }



        /// <summary>
        /// 发送语间短信   亿美发送短信方式
        /// </summary>
        /// <param name="mobile"> mobiles 最多为 200个号码，中间用 ","隔开</param>
        /// <param name="checkCode"> 语音验证码(长度≥4且≤6，格式必须为0~9的全英文半角数字字符)</param>
        /// <returns></returns>
        public static string Send_Audio(string mobile, string checkCode)
        {

            return sdkService.sendVoice(ymsn, ympassword, null, mobile.Split(new char[] { ',' }), checkCode, null, "GBK", 5, Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff")));
        }


    }
}
