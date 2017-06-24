using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.BLL.EF
{

    public class CheckEXISTS
    {
        chuangtouEntities ef = new chuangtouEntities();



        #region 检查手机号是否已经注册 + string checkmobile(string mobile)
        /// <summary>
        /// 检查手机号是否已经注册
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public string checkmobile(string mobile, int uid=0)
        {
            string str = "";

            hx_member_table ProPer = (from a in ef.hx_member_table where a.mobile == mobile || a.username==mobile select a).FirstOrDefault();

            if (ProPer != null)
            {
                if (ProPer.registerid == uid && uid > 0)
                {
                    str = "y";
                }
                else
                {
                    str = "手机号已经被注册";
                }
            }
            else
            {
                str = "y";
            }

            return str;
        }
        #endregion
        
        #region ip间隔的60秒后才能再次发送  为 false 不能再发了 +bool checkipsess(string ip, int smstype, int smstype1)
        /// <summary>
        /// ip间隔的60秒后才能再次发送  为 false 不能再发了
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="smstype"></param>
        /// <param name="smstype1"></param>
        /// <returns></returns>
        public bool checkipsess(string ip, int smstype, int smstype1)
        {
            bool t = false;
            string sql = "select  ip ,sendtime, hits  from  hx_td_SMS_record  where ip='" + ip + "' and ( smstype=" + smstype + "  or  smstype=" + smstype1 + ")   order by sms_record_id desc ";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                long sec = Utils.DateDiff("Second", DateTime.Parse(dt.Rows[0]["sendtime"].ToString()), DateTime.Now);
                if (sec > 60)
                {
                    if (int.Parse(dt.Rows[0]["hits"].ToString()) <= 8)
                    {
                        t = true;
                    }
                    else
                    {
                        t = false;
                    }
                }
                else
                {
                    t = false;
                }

            }
            else
            {
                t = true;
            }
            return t;
        } 
        #endregion

        #region 返回同一ip发送某种类别短信的次数 +checkipnum(string ip, int smstype, int smstype1)
        /// <summary>
        /// 返回同一ip发送某种类别短信的次数
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="smstype"></param>
        /// <param name="smstype1"></param>
        /// <returns></returns>
        public int checkipnum(string ip, int smstype, int smstype1)
        {
            int num = 0;
            string sql = "select  count(ip) as ipnum  from  hx_td_SMS_record  where ip='" + ip + "' and ( smstype=" + smstype + "  or  smstype=" + smstype1 + ") and sendtime > DATEADD(day,-5,GETDATE())";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                try
                {
                    num = int.Parse(dt.Rows[0]["ipnum"].ToString());
                }
                catch
                {
                    num = 0;
                }

            }
            return num;
        }
        #endregion



        #region 验证码证码是正确 +string GetVcode(string param, string mobilec)
        /// <summary>
        /// 验证码证码是正确
        /// </summary>
        /// <param name="param">验证码</param>
        /// <param name="mobilec">手机号</param>
        /// <returns></returns>
        public string GetVcode(string param, string mobilec)
        {
            if (param == Utils.GetAppSetting("TPubk")  && Utils.GetAppSetting("DeBug")=="1")
            {
                return "y";
            }
            string str = "";

            int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.短信验证码.ToString());

            int smstype1 = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.语音短信验证码.ToString());

            string sql = "select sms_record_id,smscontext,phone_number,vcode from hx_td_SMS_record where ( smstype=" + smstype + "  or  smstype=" + smstype1 + " ) and phone_number='" + mobilec + "' and  DATEDIFF(MINUTE,sendtime,getDate())<3  order by sms_record_id desc";

            DataTable dt1 = DbHelperSQL.GET_DataTable_List(sql);
            if (dt1.Rows.Count > 0)
            {
                if (dt1.Rows[0]["vcode"].ToString() == param)
                {
                    str = "y";
                }
                else
                {
                    str = "验证码不正确!";
                }
            }
            else
            {
                str = "验证码不存在!";
            }
            return str;
        }

        
        /// <summary>
        /// 验证码证码是正确
        /// </summary>
        /// <param name="param">验证码</param>
        /// <param name="mobilec">手机号</param>
        /// <returns></returns>
        public string GetVcodeWX(string param, string mobilec)
        {
            string str = "";

            int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.短信验证码.ToString());

            int smstype1 = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.语音短信验证码.ToString());

            string sql = "select sms_record_id,smscontext,phone_number,vcode from hx_td_SMS_record where ( smstype=" + smstype + "  or  smstype=" + smstype1 + " ) and phone_number='" + mobilec + "' and  DATEDIFF(MINUTE,sendtime,getDate())<3  order by sms_record_id desc";

            DataTable dt1 = DbHelperSQL.GET_DataTable_List(sql);
            if (dt1.Rows.Count > 0)
            {
                if (dt1.Rows[0]["vcode"].ToString() == param)
                {
                    str = "y";
                }
                else
                {
                    str = "验证码不正确!";
                }
            }
            else
            {
                str = "验证码不存在!";
            }
            return str;
        }
        #endregion




    }
}
