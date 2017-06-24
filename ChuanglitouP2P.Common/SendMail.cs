using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace ChuanglitouP2P.Common
{
    public class SendMail
    {


        /// <summary>
        /// 发送邮件代码
        /// </summary>
        /// <param name="Tomail">接收人邮件</param>
        /// <param name="object1">网站名称</param>
        /// <param name="title">邮件主题</param>
        /// <param name="html">邮件正文</param>
        /// <returns>返回为真发送成功，为 false 发送不成功</returns>
        public static bool SendMailTitle(string Tomail, string object1, string title, string html)
        {
            bool t = false;

         
            if (object1 == "")
            {
                object1 = "创利投";
            }
            MailMessage message = new MailMessage();
            SmtpClient client = new SmtpClient("smtp.ym.163.com", 25);
            try
            {
                  
                message.From = new MailAddress("system@chuanglitou.com", object1);
                message.To.Add(new MailAddress(Tomail));
                message.Subject = title;
                message.IsBodyHtml = true;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Body = html;
                message.Priority = MailPriority.High;

                client.Credentials = new System.Net.NetworkCredential("system@chuanglitou.com", "zcjgkrclt12345678");
                client.EnableSsl = false;
                object userState = message;
                client.Send(message);
                t = true;
            }
            catch (Exception ee)
            {
                t = false;
                //throw ee;
            }
            finally
            {
                client.Dispose();                
                message.Dispose();
            
            }
            return t;

        }






    }
}
