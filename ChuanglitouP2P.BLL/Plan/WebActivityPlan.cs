using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.Transfer;

namespace ChuanglitouP2P.Bll.Plan
{
    public class WebActivityPlan
    {


        /// <summary>
        /// 元旦活动
        /// </summary>
        /// <param name="TrAMT"></param>
        /// <param name="moth"></param>
        /// <param name="mc"></param>
        /// <param name="username"></param>
        public void Yuandai(decimal TrAMT, M_CashAwards mc, string username)
        {
            decimal TransAmt = 0.00M;
            M_Activity_schedule ma = new M_Activity_schedule();
            B_Activity_schedule ba = new B_Activity_schedule();

            DateTime nowdate = DateTime.Now;

            DateTime startdate = new DateTime(2015, 12, 23, 00, 00, 00);
            DateTime enddate = new DateTime(2016, 01, 01, 23, 59, 59);
            if (startdate < nowdate && nowdate < enddate)   //在有效范围之内则执行活动计划
            {
                //  TransAmt = GetSeptAmt(TrAMT);

                TransAmt = TrAMT * 0.02M;

                mc.Amounts = TransAmt;

                B_CashAwards o = new B_CashAwards();

                if (o.Add(mc) > 0)
                {
                    YuandaiToUserTransfer(mc, TrAMT, TransAmt);
                }
            }
        }




        /// <summary>
        /// 九 月15 号到十月15号活动
        /// </summary>
        /// <param name="TrAMT"></param>
        /// <param name="moth"></param>
        /// <param name="mc"></param>
        /// <param name="username"></param>
        public void Sept(decimal TrAMT, int moth, M_CashAwards mc, string username)
        {
            decimal TransAmt = 0.00M;


            if (moth >= 3)
            {

                M_Activity_schedule ma = new M_Activity_schedule();
                B_Activity_schedule ba = new B_Activity_schedule();

                DateTime nowdate = DateTime.Now;

                DateTime startdate = new DateTime(2015, 09, 15, 00, 00, 00);
                DateTime enddate = new DateTime(2016, 02, 28, 23, 59, 59);
                if (startdate < nowdate && nowdate < enddate)   //在有效范围之内则执行活动计划
                {
                    TransAmt = GetSeptAmt(TrAMT);

                    mc.Amounts = TransAmt;

                    B_CashAwards o = new B_CashAwards();

                    if (o.Add(mc) > 0)
                    {
                        ToUserTransfer(mc, TrAMT, TransAmt);
                    }
                }
            }
        }






        /// <summary>
        /// 八月份活动   期限30天以上的标的才能享受
        /// </summary>
        /// <param name="TrAMT"></param>
        /// <param name="days"></param>
        /// <param name="Reg"></param>
        public void AugPan(decimal TrAMT, int days, M_CashAwards mc,string username)
        {
            decimal TransAmt = 0.00M;
            if (days >= 30)
            {

                M_Activity_schedule ma = new M_Activity_schedule();
                B_Activity_schedule ba = new B_Activity_schedule();

                DateTime nowdate = DateTime.Now;

                DateTime startdate = new DateTime(2015, 07, 26, 00, 00, 00);
                DateTime enddate = new DateTime(2015, 09, 15, 23, 59, 59);
                if (startdate < nowdate && nowdate < enddate)   //在有效范围之内则执行活动计划
                {
                    TransAmt = GetAugPanAmt(TrAMT);

                    mc.Amounts = TransAmt;



                    B_CashAwards o = new B_CashAwards();

                    if (o.Add(mc) > 0)
                    {
                        ToUserTransfer(mc, TrAMT, TransAmt);
                    }



                }







            }





        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="TrAMT">投资金额</param>
        /// <param name="TransAmt">奖励金额</param>
        public void ToUserTransfer(M_CashAwards mc, decimal TrAMT, decimal TransAmt)
        {


            M_Transfer m = new M_Transfer();
            m.Version = "10";
            m.CmdId = "Transfer";

            // m.OrdId = Utils.Createcode();

            m.OrdId = mc.OrdId.ToString();
            m.OutCustId = Utils.GetMerCustID();
            m.OutAcctId = "MDT000001";
            m.TransAmt = mc.Amounts.ToString("0.00");
            m.InCustId = mc.UsrCustId;
            m.BgRetUrl = Utils.GetRe_url("Thirdparty/ToUserTransfer.aspx");
            m.MerPriv = mc.proid.ToString();


            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.Version);
            chkVal.Append(m.CmdId);
            chkVal.Append(m.OrdId);
            chkVal.Append(m.OutCustId);
            chkVal.Append(m.OutAcctId);
            chkVal.Append(m.TransAmt);
            chkVal.Append(m.InCustId);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.MerPriv);

            string chkv = chkVal.ToString();
            LogInfo.WriteLog("平台向用户活动转账加签chkv字符:" + chkv);

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

            LogInfo.WriteLog("平台向用户活动转账加签字符:" + str.ToString());

            m.ChkValue = sbChkValue.ToString();

            LogInfo.WriteLog("平台向用户活动转账提交信息：" + FastJSON.toJOSN(m));
            LogInfo.WriteLog("ChkValue:" + m.ChkValue);


            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values.Add("Version", m.Version);
                values.Add("CmdId", m.CmdId);
                values.Add("OrdId", m.OrdId);
                values.Add("OutCustId", m.OutCustId);
                values.Add("OutAcctId", m.OutAcctId);
                values.Add("TransAmt", m.TransAmt);
                values.Add("InCustId", m.InCustId);
                values.Add("InAcctId", m.InAcctId);
                values.Add("RetUrl", m.RetUrl);
                values.Add("BgRetUrl", m.BgRetUrl);
                values.Add("MerPriv", m.MerPriv);
                values.Add("ChkValue", m.ChkValue);
                string url = Utils.GetChinapnrUrl();
                //同步发送form表单请求
                byte[] result = client.UploadValues(url, "POST", values);
                var retStr = Encoding.UTF8.GetString(result);
                // Response.Write(retStr);
                LogInfo.WriteLog("自动扣款转账（商户用）返回报文" + retStr);
                ReTransfer reg = new ReTransfer();

                var retloan = (ReTransfer)FastJSON.ToObject(retStr, reg);
                StringBuilder builder = new StringBuilder();
                builder.Append(retloan.CmdId);
                builder.Append(retloan.RespCode);
                builder.Append(retloan.OrdId);
                builder.Append(retloan.OutCustId);
                builder.Append(retloan.OutAcctId);
                builder.Append(retloan.TransAmt);
                builder.Append(retloan.InCustId);
                builder.Append(retloan.InAcctId);
                builder.Append(HttpUtility.UrlDecode(retloan.RetUrl));
                builder.Append(HttpUtility.UrlDecode(retloan.BgRetUrl));
                builder.Append(retloan.MerPriv);
                var msg = builder.ToString();

                LogInfo.WriteLog("平台向用户活动转账返回参数:" + msg);
                //验签
                string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
                int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, retloan.ChkValue);

                LogInfo.WriteLog("平台向用户活动转账验签ret:" + ret.ToString());
                if (ret == 0)
                {
                    if (retloan.RespCode == "000")
                    {
                        /*
                        string sql = "update hx_CashAwards  set  OrdIdstate=3  where OrdIdstate=1 and OrdId=" + retloan.OrdId + " and  proid =" + retloan.MerPriv;
                        DbHelperSQL.RunSql(sql);
                        LogInfo.WriteLog("平台向用户活动转账验签更新"+ sql);
                         */
                        // Response.Write(retloan.RespCode + "  <br> ");
                        B_usercenter BUC = new B_usercenter();

                        int dint=BUC.UpateAwa(retloan);
                        LogInfo.WriteLog("事务执行返回:"+dint.ToString());
                        if (dint > 0)
                        {

                            B_member_table dmt = new B_member_table();

                            string sql = "SELECT registerid,username,mobile  from hx_member_table where UsrCustId='" + retloan.InCustId + "'";
                            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

                            if (dt.Rows.Count > 0)
                            {


                                //活动奖历
                                M_Activity_schedule ma = new M_Activity_schedule();
                                B_Activity_schedule ba = new B_Activity_schedule();
                                B_bonus_account bb = new B_bonus_account();
                                M_bonus_account mb = new M_bonus_account();

                                M_bonus_account_water mbaw = new M_bonus_account_water();
                                B_bonus_account_water bbaw = new B_bonus_account_water();
                                DateTime dte = DateTime.Now;


                                // ma = ba.GetModel(14);  //测试平台
                                ma = ba.GetModel(13);  //获取奖励对象


                                mb.activity_schedule_id = ma.activity_schedule_id;
                                mb.membertable_registerid = int.Parse(dt.Rows[0]["registerid"].ToString());
                                mb.activity_schedule_name = ma.activity_schedule_name;
                                mb.amount_of_reward = decimal.Parse(retloan.TransAmt);
                                mb.use_lower_limit = ma.use_lower_limit;
                                mb.reward = ma.reward;
                                mb.start_date = dte;
                                mb.end_date = dte;
                                mb.entry_time = dte;
                                mb.reward_state = 3;
                                int bbid = bb.Add(mb);
                                if (bbid > 0) //奖励记录成功后插入明细记录
                                {
                                    mbaw.bonus_account_id = bbid;
                                    mbaw.membertable_registerid = mb.membertable_registerid;
                                    mbaw.income = mb.amount_of_reward;
                                    mbaw.expenditure = 0.00M;
                                    mbaw.time_of_occurrence = mb.entry_time;
                                    // mbaw.
                                    mbaw.award_description = "已汇入个人账户";
                                    mbaw.water_type = 0;
                                    bbaw.Add(mbaw);


                                    //短信通知

                                    #region MyRegion//短信通知
                                    string contxt = Utils.GetMSMEmailContext(19, 1); // 获取注册成功邮件内容

                                    StringBuilder sbsms = new StringBuilder(contxt);

                                    sbsms = sbsms.Replace("#USERANEM#", dt.Rows[0]["username"].ToString());

                                    sbsms = sbsms.Replace("#MONEY#", TrAMT.ToString("0.00"));

                                    sbsms = sbsms.Replace("#AMTM#", TransAmt.ToString("0.00"));


                                    string mobile = dt.Rows[0]["mobile"].ToString();

                                    M_td_SMS_record psms = new M_td_SMS_record();
                                    B_td_SMS_record osms = new B_td_SMS_record();
                                    int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.活动奖励.ToString());
                                    psms.phone_number = mobile;
                                    psms.sendtime = DateTime.Now;
                                    psms.senduserid = int.Parse(dt.Rows[0]["registerid"].ToString());
                                    psms.smstype = smstype;
                                    psms.smscontext = sbsms.ToString();
                                    psms.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());
                                    psms.vcode = "";

                                    osms.Add(psms);
                                    #endregion


                                }




                                #region MyRegion  系统消息
                                DateTime dti = DateTime.Now;
                                M_td_System_message pm = new M_td_System_message();
                                pm.MReg = int.Parse(dt.Rows[0]["registerid"].ToString());
                                pm.Mstate = 0;
                                pm.MTitle = "投资成功现金奖励";
                                pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功投资了项目，现金奖励 " + retloan.TransAmt+ "元。如有问题可咨询创利投的客服！";
                                pm.PubTime = dti;
                                pm.Mtype = 2;
                                B_usercenter.AddMessage(pm);
                                #endregion


                            }

                            LogInfo.WriteLog("平台向用户活动转账验签更新成功，需要写入消息");

                        }




                    }
                    else
                    {

                        //Response.Write(HttpUtility.UrlDecode(retloan.));
                    }
                }



            }



        }


        /// <summary>
        /// 九月奖励活动
        /// </summary>
        /// <param name="amt"></param>
        /// <returns></returns>
        public decimal GetSeptAmt(decimal amt)
        {
            decimal dec = 0.00M;

            if (1000.00M <= amt && amt <= 1900.00M)
            {
                dec = 5.00M;
            }
            else if (2000.00M <= amt && amt <= 2900.00M)
            {
                dec = 10.00M;
            }
            else if (3000.00M <= amt && amt <= 4900.00M)
            {

                dec = 15.00M;
            }
            else if (5000.00M <= amt && amt <= 9900.00M)
            {

                dec = 25.00M;
            }
            else if (10000.00M <= amt && amt <= 49900.00M)
            {
                dec = 60.00M;
            }
            else if (50000.00M <= amt && amt <= 99900.00M)
            {

                dec = 320.00M;
            }
            else if (100000.00M <= amt && amt <= 149900.00M)
            {

                dec = 650.00M;
            }
            else if (150000.00M <= amt)
            {

                dec = 1000.00M;
            }
            return dec;

        }






        /// <summary>
        /// 返回8月份现金额奖励金额
        /// </summary>
        /// <param name="amt"></param>
        /// <returns></returns>
        public decimal GetAugPanAmt(decimal amt)
        {
            decimal dec = 0.00M;
            if (500.00M <= amt && amt <= 900.00M)
            {
                dec = 5.00M;
            }
            else if (1000.00M <= amt && amt <= 1900.00M)
            {
                dec = 10.00M;
            }
            else if (2000.00M <= amt && amt <= 2900.00M)
            {

                dec = 25.00M;
            }
            else if (3000.00M <= amt && amt <= 4900.00M)
            {
                dec = 35.00M;
            }
            else if (5000.00M <= amt && amt <= 9900.00M)
            {
                dec = 50.00M;
            }
            else if (10000.00M <= amt && amt <= 49900.00M)
            {
                dec = 100.00M;
            }
            else if (50000.00M <= amt && amt <= 99900.00M)
            {
                dec = 500.00M;
            }

            else if (100000.00M <= amt)
            {
                dec = 1000.00M;
            }

            return dec;
        }




        /// <summary>
        /// 5月任性赚，“三重好礼”送不停   活动时间 2015年5月4日—2015年5月31日
        /// </summary>
        /// 

        public void MayPan(decimal TrAMT, int month, int Reg)
        {

            if (month == 3 || month == 6)  //只有3个月和6个月才生成奖励记录
            {

                M_Activity_schedule ma = new M_Activity_schedule();
                B_Activity_schedule ba = new B_Activity_schedule();

                B_bonus_account bb = new B_bonus_account();
                M_bonus_account mb = new M_bonus_account();

                M_bonus_account_water mbaw = new M_bonus_account_water();
                B_bonus_account_water bbaw = new B_bonus_account_water();


                DateTime nowdate = DateTime.Now;

                DateTime startdate = new DateTime(2015, 06, 26, 00, 00, 00);
                DateTime enddate = new DateTime(2015, 07, 31, 23, 59, 59);

                if (startdate < nowdate && nowdate < enddate)   //在有效范围之内则执行活动计划
                {




                    ma = ba.GetModel(8);  //获取奖励对象

                    // ma = ba.GetModel(10);  //获取奖励对象
                    //
                    decimal rewardamt = GetAmt(TrAMT, month);
                    mb.activity_schedule_id = ma.activity_schedule_id;
                    mb.membertable_registerid = Reg;
                    mb.activity_schedule_name = ma.activity_schedule_name;
                    mb.amount_of_reward = rewardamt;
                    mb.use_lower_limit = ma.use_lower_limit;
                    mb.reward = ma.reward;
                    mb.start_date = nowdate;
                    mb.end_date = ma.end_date;
                    mb.entry_time = nowdate;
                    int bbid = bb.Add(mb);
                    if (bbid > 0) //奖励记录成功后插入明细记录
                    {
                        mbaw.bonus_account_id = bbid;
                        mbaw.membertable_registerid = Reg;
                        mbaw.income = rewardamt;
                        mbaw.expenditure = 0.00M;
                        mbaw.time_of_occurrence = mb.entry_time;
                        // mbaw.
                        mbaw.award_description = "7月“投资拿现金大奖”  活动时间 2015年7月1日—2015年7月31日 投资" + TrAMT.ToString() + "周期" + month.ToString() + "月 奖励" + rewardamt.ToString();
                        mbaw.water_type = 0;
                        bbaw.Add(mbaw);

                    }




                }
            }
        }



        /// <summary>
        /// 返回活动奖励金额
        /// </summary>
        /// <param name="amt">投资金额</param>
        /// <param name="month">投资月数</param>
        /// <returns></returns>
        public decimal GetAmt(decimal amt, int month)
        {
            decimal LAmt = 0.00M;

            if (1000.00M <= amt && amt <= 2999.00M)
            {

                if (month == 3)
                {
                    LAmt = 5;
                }
                else if (month == 6)
                {
                    LAmt = 10;
                }

            }
            else if (3000.00M <= amt && amt <= 4999.00M)
            {
                if (month == 3)
                {
                    LAmt = 20;
                }
                else if (month == 6)
                {
                    LAmt = 50;
                }

            }
            else if (5000.00M <= amt && amt <= 9999.00M)
            {

                if (month == 3)
                {
                    LAmt = 30;
                }
                else if (month == 6)
                {
                    LAmt = 100;
                }

            }
            else if (10000.00M <= amt && amt <= 19999.00M)
            {

                if (month == 3)
                {
                    LAmt = 80;
                }
                else if (month == 6)
                {
                    LAmt = 200;
                }

            }
            else if (20000.00M <= amt)
            {

                if (month == 3)
                {
                    LAmt =200;
                }
                else if (month == 6)
                {
                    LAmt =500;
                }

            }
            return LAmt;


        }





        /// <summary>
        /// 12月份元旦活动转账专用 获取活动信息  短信通知部分需要更改 
        /// </summary>
        /// <param name="mc"></param>
        /// <param name="TrAMT"></param>
        /// <param name="TransAmt"></param>
        public void YuandaiToUserTransfer(M_CashAwards mc, decimal TrAMT, decimal TransAmt)
        {


            M_Transfer m = new M_Transfer();
            m.Version = "10";
            m.CmdId = "Transfer";

            // m.OrdId = Utils.Createcode();

            m.OrdId = mc.OrdId.ToString();
            m.OutCustId = Utils.GetMerCustID();
            m.OutAcctId = "MDT000001";
            m.TransAmt = mc.Amounts.ToString("0.00");
            m.InCustId = mc.UsrCustId;
            m.BgRetUrl = Utils.GetRe_url("Thirdparty/ToUserTransfer.aspx");
            m.MerPriv = mc.proid.ToString();


            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.Version);
            chkVal.Append(m.CmdId);
            chkVal.Append(m.OrdId);
            chkVal.Append(m.OutCustId);
            chkVal.Append(m.OutAcctId);
            chkVal.Append(m.TransAmt);
            chkVal.Append(m.InCustId);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.MerPriv);

            string chkv = chkVal.ToString();
            LogInfo.WriteLog("12月份元旦活动平台向用户活动转账加签chkv字符:" + chkv);

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

            LogInfo.WriteLog("12月份元旦活动平台向用户活动转账加签字符:" + str.ToString());

            m.ChkValue = sbChkValue.ToString();

            LogInfo.WriteLog("12月份元旦活动平台向用户活动转账提交信息：" + FastJSON.toJOSN(m));
            LogInfo.WriteLog("ChkValue:" + m.ChkValue);


            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values.Add("Version", m.Version);
                values.Add("CmdId", m.CmdId);
                values.Add("OrdId", m.OrdId);
                values.Add("OutCustId", m.OutCustId);
                values.Add("OutAcctId", m.OutAcctId);
                values.Add("TransAmt", m.TransAmt);
                values.Add("InCustId", m.InCustId);
                values.Add("InAcctId", m.InAcctId);
                values.Add("RetUrl", m.RetUrl);
                values.Add("BgRetUrl", m.BgRetUrl);
                values.Add("MerPriv", m.MerPriv);
                values.Add("ChkValue", m.ChkValue);
                string url = Utils.GetChinapnrUrl();
                //同步发送form表单请求
                byte[] result = client.UploadValues(url, "POST", values);
                var retStr = Encoding.UTF8.GetString(result);
                // Response.Write(retStr);
                LogInfo.WriteLog("12月份元旦活动自动扣款转账（商户用）返回报文" + retStr);
                ReTransfer reg = new ReTransfer();

                var retloan = (ReTransfer)FastJSON.ToObject(retStr, reg);
                StringBuilder builder = new StringBuilder();
                builder.Append(retloan.CmdId);
                builder.Append(retloan.RespCode);
                builder.Append(retloan.OrdId);
                builder.Append(retloan.OutCustId);
                builder.Append(retloan.OutAcctId);
                builder.Append(retloan.TransAmt);
                builder.Append(retloan.InCustId);
                builder.Append(retloan.InAcctId);
                builder.Append(HttpUtility.UrlDecode(retloan.RetUrl));
                builder.Append(HttpUtility.UrlDecode(retloan.BgRetUrl));
                builder.Append(retloan.MerPriv);
                var msg = builder.ToString();

                LogInfo.WriteLog("12月份元旦活动平台向用户活动转账返回参数:" + msg);
                //验签
                string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
                int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, retloan.ChkValue);

                LogInfo.WriteLog("12月份元旦活动平台向用户活动转账验签ret:" + ret.ToString());
                if (ret == 0)
                {
                    if (retloan.RespCode == "000")
                    {
                        /*
                        string sql = "update hx_CashAwards  set  OrdIdstate=3  where OrdIdstate=1 and OrdId=" + retloan.OrdId + " and  proid =" + retloan.MerPriv;
                        DbHelperSQL.RunSql(sql);
                        LogInfo.WriteLog("平台向用户活动转账验签更新"+ sql);
                         */
                        // Response.Write(retloan.RespCode + "  <br> ");
                        B_usercenter BUC = new B_usercenter();

                        int dint = BUC.UpateAwa(retloan);
                        LogInfo.WriteLog("12月份元旦活动事务执行返回:" + dint.ToString());
                        if (dint > 0)
                        {

                            B_member_table dmt = new B_member_table();

                            string sql = "SELECT registerid,username,mobile  from hx_member_table where UsrCustId='" + retloan.InCustId + "'";
                            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

                            if (dt.Rows.Count > 0)
                            {


                                //活动奖历
                                M_Activity_schedule ma = new M_Activity_schedule();
                                B_Activity_schedule ba = new B_Activity_schedule();
                                B_bonus_account bb = new B_bonus_account();
                                M_bonus_account mb = new M_bonus_account();

                                M_bonus_account_water mbaw = new M_bonus_account_water();
                                B_bonus_account_water bbaw = new B_bonus_account_water();
                                DateTime dte = DateTime.Now;


                                // ma = ba.GetModel(16);  //测试平台
                                ma = ba.GetModel(15);  //获取奖励对象


                                mb.activity_schedule_id = ma.activity_schedule_id;
                                mb.membertable_registerid = int.Parse(dt.Rows[0]["registerid"].ToString());
                                mb.activity_schedule_name = ma.activity_schedule_name;
                                mb.amount_of_reward = decimal.Parse(retloan.TransAmt);
                                mb.use_lower_limit = ma.use_lower_limit;
                                mb.reward = ma.reward;
                                mb.start_date = dte;
                                mb.end_date = dte;
                                mb.entry_time = dte;
                                mb.reward_state = 3;
                                int bbid = bb.Add(mb);
                                if (bbid > 0) //奖励记录成功后插入明细记录
                                {
                                    mbaw.bonus_account_id = bbid;
                                    mbaw.membertable_registerid = mb.membertable_registerid;
                                    mbaw.income = mb.amount_of_reward;
                                    mbaw.expenditure = 0.00M;
                                    mbaw.time_of_occurrence = mb.entry_time;
                                    // mbaw.
                                    mbaw.award_description = "已汇入个人账户";
                                    mbaw.water_type = 0;
                                    bbaw.Add(mbaw);


                                    //短信通知

                                    #region MyRegion//短信通知
                                    string contxt = Utils.GetMSMEmailContext(20, 1); // 获取注册成功邮件内容

                                    StringBuilder sbsms = new StringBuilder(contxt);

                                    sbsms = sbsms.Replace("#USERANEM#", dt.Rows[0]["username"].ToString());

                                    sbsms = sbsms.Replace("#MONEY#", TrAMT.ToString("0.00"));

                                    sbsms = sbsms.Replace("#AMTM#", TransAmt.ToString("0.00"));


                                    string mobile = dt.Rows[0]["mobile"].ToString();

                                    M_td_SMS_record psms = new M_td_SMS_record();
                                    B_td_SMS_record osms = new B_td_SMS_record();
                                    int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.活动奖励.ToString());
                                    psms.phone_number = mobile;
                                    psms.sendtime = DateTime.Now;
                                    psms.senduserid = int.Parse(dt.Rows[0]["registerid"].ToString());
                                    psms.smstype = smstype;
                                    psms.smscontext = sbsms.ToString();
                                    psms.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());
                                    psms.vcode = "";

                                    osms.Add(psms);
                                    #endregion


                                }




                                #region MyRegion  系统消息
                                DateTime dti = DateTime.Now;
                                M_td_System_message pm = new M_td_System_message();
                                pm.MReg = int.Parse(dt.Rows[0]["registerid"].ToString());
                                pm.Mstate = 0;
                                pm.MTitle = "投资成功现金奖励";
                                pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功投资了项目，现金奖励 " + retloan.TransAmt + "元。如有问题可咨询创利投的客服！";
                                pm.PubTime = dti;
                                B_usercenter.AddMessage(pm);
                                #endregion


                            }

                            LogInfo.WriteLog("12月份元旦活动 平台向用户活动转账验签更新成功，需要写入消息");

                        }




                    }
                    else
                    {

                        //Response.Write(HttpUtility.UrlDecode(retloan.));
                    }
                }



            }



        }




    }
}
