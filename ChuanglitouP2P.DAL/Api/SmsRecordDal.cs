using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ChuangLiTou.Core.Entities;
using ChuangLiTou.Core.Entities.P2Peye;
using ChuangLiTou.Core.Entities.Response.SmsEmail;
using ChuanglitouP2P.Common.Util;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.DAL.Api
{
    public class SmsRecordDal : ImplBase
    {
        public Int32 AddRecord(SmsRecordEntity srEntity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into hx_td_SMS_record(");
            strSql.Append("senduserid,phone_number,smscontext,smstype,sendtime,orderid,vcode,ip)");
            strSql.Append(" values (");
            strSql.Append("@senduserid,@phone_number,@smscontext,@smstype,@sendtime,@orderid,@vcode,@ip)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@senduserid", SqlDbType.Int,4),
					new SqlParameter("@phone_number", SqlDbType.VarChar,11),
					new SqlParameter("@smscontext", SqlDbType.VarChar,4000),
					new SqlParameter("@smstype", SqlDbType.Int,4),
					new SqlParameter("@sendtime", SqlDbType.DateTime),
                    new SqlParameter("@orderid", SqlDbType.Decimal,28),
                    new SqlParameter("@vcode", SqlDbType.VarChar,50),
                    new SqlParameter("@ip", SqlDbType.VarChar,50)};
            parameters[0].Value = srEntity.senduserid;
            parameters[1].Value = srEntity.phone_number;
            parameters[2].Value = srEntity.smscontext;
            parameters[3].Value = srEntity.smstype;
            parameters[4].Value = srEntity.sendtime;
            parameters[5].Value = srEntity.orderid;
            parameters[6].Value = srEntity.vcode;
            parameters[7].Value = srEntity.ip;

            var obj = DbHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 验证当前IP是否在1分钟之内操作过 总的操作次数不能超过8次.
        /// </summary>
        /// <param name="clientIp">The client ip.</param>
        /// <returns><c>true</c> 可以操作, <c>false</c> 不可以操作.</returns>
        public bool CheckInOneMinute(string clientIp, int type1, int type2)
        {
            bool t = false;

            string sql = "select  ip ,sendtime, hits  from  hx_td_SMS_record  where ip='" + clientIp + "' and ( smstype=" + type1 + "  or  smstype=" + type2 + ")   order by sms_record_id desc ";

            var ds = DbHelper.Query(sql);

            if (DataSetIsNotNull(ds))
            {
                long sec = Settings.Instance.DateDiff("Second", DateTime.Parse(ds.Tables[0].Rows[0]["sendtime"].ToString()), DateTime.Now);
                if (sec > 60)
                {
                    t = int.Parse(ds.Tables[0].Rows[0]["hits"].ToString()) <= 8;
                    if (!t)
                    {
                        LoggerHelper.Error("ip短信限制：" + clientIp + " T1:" + type1 + " T2:" + type2 + " Sql:" + sql);
                    }
                }
            }
            else
            {
                t = true;
            }


            return t;

        }

        /// <summary>
        /// 同一IP 同种类型发送的次数.
        /// </summary>
        /// <param name="clientIp">The client ip.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <returns>System.Int32.</returns>
        public int CheckIpSendTimes(string clientIp, int t1, int t2)
        {
            int num = 0;

            string sql = "select  count(ip) as ipnum  from  hx_td_SMS_record  where ip='" + clientIp + "' and ( smstype=" + t1 + "  or  smstype=" + t2 + ") ";

            var rs = DbHelper.GetSingle(sql);
            try
            {
                num = int.Parse(rs.ToString());
            }
            catch (Exception ex)
            {
                num = 0;
                LoggerHelper.Error("  public int CheckIpSendTimes(string clientIp, int t1, int t2):" + ex.ToString());
            }
            return num;
        }

        /// <summary>
        /// 获取3分钟之内 历史发送记录.
        /// </summary>
        /// <param name="mobile">The client ip.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <returns>SmsRecordEntity.</returns>
        public SmsRecordEntity SelectHistory(string mobile, int t1, int t2)
        {
            string sql = "select top 1 sms_record_id,smscontext,phone_number,hits from hx_td_SMS_record where ( smstype=" + t1 + "  or  smstype=" + t2 + " ) and phone_number='" + mobile + "' and  DATEDIFF(MINUTE,sendtime,getDate())<3  order by sms_record_id desc";

            var ds = DbHelper.Query(sql);
            if (DataSetIsNotNull(ds))
            {
                var se = new SmsRecordEntity
                {
                    smscontext = ds.Tables[0].Rows[0]["smscontext"].ToString(),
                    hits = ConvertHelper.ParseValue(ds.Tables[0].Rows[0]["hits"], 0),
                    sms_record_id = ConvertHelper.ParseValue(ds.Tables[0].Rows[0]["sms_record_id"], 0)
                };
                return se;
            }
            return null;
        }

        /// <summary>
        /// 更新发送记录.
        /// </summary>
        /// <param name="ent">The ent.</param>
        public void UpdateRecord(SmsRecordEntity ent)
        {
            var sql = "update hx_td_SMS_record set orderid=" + ent.orderid + ",hits=hits+1 where sms_record_id=" + ent.sms_record_id;
            DbHelper.ExecuteSql(sql);
        }


        /// <summary>
        /// 验证code合法性 90秒
        /// </summary>
        /// <param name="code">v</param>
        /// <param name="type">类型</param>
        /// <param name="mobile"></param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CheckCode(string code, int type, string mobile)
        {
            int num = 0;

            string sql = string.Format("select  count(1) as Cnt  from  hx_td_SMS_record  WHERE (smstype=8 or smstype=7) AND vcode='{0}' AND phone_number='{1}'" +
                                      
                                       "-- and DATEDIFF(SECOND,sendtime,getDate()) < 90  ", code, mobile);

            var rs = DbHelper.GetSingle(sql);
            try
            {
                num = int.Parse(rs.ToString());
            }
            catch (Exception ex)
            {
                num = 0;
                LoggerHelper.Error("  public int CheckCode(string code, int type, string mobile):" + ex.ToString());
            }
            return num > 0;

        }
    }
}
