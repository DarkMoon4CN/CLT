using ChuanglitouP2P.Common.Extensionses;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using EntityFramework.Extensions;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model.chinapnr.Repayment;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.BLL;
using PagedList;
using System.Data;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Model.chinapnr.QueryBalanceBg;
using ChuanglitouP2P.Common.chinapnr;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class ReplayController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/Replay
        [AdminVaildate(false, true)]
        public ActionResult Index()
        {
            return View();
        }


        #region 未还款
        /// <summary>
        /// 未还款
        /// </summary>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult Individualrepayment(int targetid, string date1, int bid, int repayment_type = 1)
        {
            StringBuilder sb = new StringBuilder();
            //decimal count_AMT = 0M; //应还总金额
            //decimal O_penalty = 0M;//逾期
            decimal fees = 0M;//, Interest_spreads1 = 0M; //管理费
            //bool pre = true;
            //decimal Inves_Repayment_amount1 = 0M;
            int targetid1 = targetid;
            //string date1 = "";
            //int bid = 0;
            string sql = "select targetid, borrower_registerid ,investor_registerid,available_balance,BorrUsrCustId,OutCustId,SubOrdid,SubOrdDate ,repayment_period,repayment_amount,interest_payment_date,loan_management_fee,investment_amount,current_investment_period,bid_records_id,username,mobile,Principal,interestpayment,orderid,income_statement_id,current_period from V_borrowing_Bid_records_income_statement  where targetid=" + targetid1.ToString() + "  and    CONVERT(varchar(10), interest_payment_date, 23)=CONVERT(varchar(10), '" + date1 + "', 23) and payment_status=0 and bid_records_id=" + bid.ToString();
            DataTable dtrep = DbHelperSQL.GET_DataTable_List(sql);
            B_member_table o = new B_member_table();
            M_member_table p = new M_member_table();
            M_member_table p2 = new M_member_table();
            if (dtrep.Rows.Count > 0)
            {
                //判断是否为最后一期还款，并进行相应状态处理
                hx_repayment_plan plan = (from a in ef.hx_repayment_plan where a.targetid == targetid select a).OrderByDescending(a => a.current_period).Take(1).FirstOrDefault();
                var lastcur = plan.current_period;
                DateTime dtime = DateTime.Parse(dtrep.Rows[0]["interest_payment_date"].ToString());
                //  var lastcur = plan.current_period;
                //   DateTime dtime = DateTime.Parse(list[0].repayment_period.ToString());
                // string ordid = Utils.Createcode(); //生成新的订单号

                string ordid = ""; //订单号
                if (dtrep.Rows[0]["orderid"] != null && dtrep.Rows[0]["orderid"].ToString() != "")
                {
                    ordid = dtrep.Rows[0]["orderid"].ToString();
                }
                else
                {
                    ordid = Utils.Createcode(); //生成新的订单号
                    string sql1 = "update hx_income_statement set  orderid ='" + ordid + "' where income_statement_id=" + dtrep.Rows[0]["income_statement_id"].ToString() + " and  payment_status=0";
                    DbHelperSQL.RunSql(sql1);
                }

                M_Repayment MR = new M_Repayment();
                MR.Version = "30";
                MR.CmdId = "Repayment";
                MR.MerCustId = Utils.GetMerCustID();
                MR.OrdId = ordid;
                MR.OrdDate = DateTime.Now.ToString("yyyyMMdd");
                if (repayment_type == 1)//)//1正常还款  2平台代还 3担保公司代还
                {
                    MR.OutCustId = dtrep.Rows[0]["BorrUsrCustId"].ToString();//借款人人帐户(出帐)
                }
                else if (repayment_type == 2)
                {
                    MR.OutCustId = Utils.GetMerCustID();
                    MR.OutAcctId = "MDT000001";
                    MR.DzObject = dtrep.Rows[0]["BorrUsrCustId"].ToString();
                    MR.InAcctId = "MDT000001";
                }
                else if (repayment_type == 3)
                {
                    MR.OutCustId = Utils.GetDanbaoCustID();
                    MR.OutAcctId = "MDT000001";
                    MR.DzObject = dtrep.Rows[0]["BorrUsrCustId"].ToString();
                    MR.InAcctId = "MDT000001";
                }
                //MR.OutCustId = dtrep.Rows[0]["BorrUsrCustId"].ToString(); //投资人帐户(出帐)
                MR.SubOrdId = dtrep.Rows[0]["SubOrdid"].ToString();
                MR.SubOrdDate = DateTime.Parse(dtrep.Rows[0]["SubOrdDate"].ToString()).ToString("yyyyMMdd");

                fees = Calculator.C_fees(Calculator.GetLoan_management_fee(targetid1), decimal.Parse(dtrep.Rows[0]["repayment_amount"].ToString()));
                //MR.TransAmt = decimal.Parse(dtrep.Rows[0]["repayment_amount"].ToString()).ToString("0.00"); 
                MR.PrincipalAmt = (decimal.Parse(dtrep.Rows[0]["repayment_amount"].ToString()) - decimal.Parse(dtrep.Rows[0]["interestpayment"].ToString())).ToString("0.00");//decimal.Parse(item.Principal.ToString()).ToString("0.00");
                MR.InterestAmt = decimal.Parse(dtrep.Rows[0]["interestpayment"].ToString()).ToString("0.00");

                MR.Fee = RMB.GetDecimal(fees, 2, true).ToString("0.00");
                // MR.Fee = fees.ToString("0.00");
                MR.InCustId = dtrep.Rows[0]["OutCustId"].ToString(); //投资人帐户客户id

                if (fees > 0)
                {
                    M_RP_DivDetails mrp = new M_RP_DivDetails();
                    mrp.DivCustId = Utils.GetMerCustID();
                    mrp.DivAcctId = Utils.GetMERDT();
                    mrp.DivAmt = MR.Fee;
                    MR.DivDetails = "[" + FastJSON.toJOSN(mrp) + "]";
                    MR.FeeObjFlag = "O"; // I 向入款客户号收取  O 向出款客户号收取
                }
                MR.BgRetUrl = Utils.GetRe_url("admin/Thirdparty/Su_Repayment");
                M_RP_ProID mrpp = new M_RP_ProID();
                mrpp.ProId = targetid1.ToString();
                MR.ReqExt = FastJSON.toJOSN(mrpp);
                MR.ProId = mrpp.ProId;
                MR.MerPriv = DateTime.Parse(dtrep.Rows[0]["interest_payment_date"].ToString()).ToString("yyyy-MM-dd");

                ReRepayment Re = ChinapnrFacade.Repayment3(MR);
                Re.TransAmt = (decimal.Parse(MR.PrincipalAmt == null ? "0.00" : MR.PrincipalAmt) + decimal.Parse(MR.InterestAmt == null ? "0.00" : MR.InterestAmt)).ToString("0.00");//decimal.Parse(item.Principal.ToString()).ToString("0.00");
                if (Re != null)
                {
                    if (Re.RespCode == "000" || Re.TransAmt == "0.00")
                    {
                        string cachename = "Repayment" + Re.OrdId;
                        if (Utils.GeTThirdCache(cachename) == 0)
                        {
                            Utils.SetThirdCache(cachename);

                            M_Capital_account_water baw = new M_Capital_account_water(); //借款人流水对象
                            if (repayment_type == 2)//平台代偿不记录出账人流入
                            {
                                baw = null;
                            }
                            else
                            {
                                if (repayment_type == 1)
                                {
                                    p = o.GetModel(int.Parse(dtrep.Rows[0]["borrower_registerid"].ToString())); //借款人用户对象
                                }
                                else if (repayment_type == 3)
                                {
                                    p = o.GetModelByUsrCustid(Re.OutCustId); //借款人用户对象
                                }
                                baw.membertable_registerid = p.registerid;
                                baw.income = 0.00M;
                                baw.expenditure = decimal.Parse(Re.TransAmt);
                                baw.time_of_occurrence = DateTime.Now;
                                baw.account_balance = p.available_balance - baw.expenditure;  //面要得么帐户余额
                                baw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.还款.ToString());
                                baw.createtime = DateTime.Now;
                                baw.keyid = 0;
                                baw.remarks = Re.OrdId + "," + Re.OrdDate;
                            }
                            M_Capital_account_water iaw = new M_Capital_account_water(); //投资人流水对象
                            p2 = o.GetModel(int.Parse(dtrep.Rows[0]["investor_registerid"].ToString())); //投资人用户对象
                            iaw.membertable_registerid = p2.registerid;
                            iaw.income = decimal.Parse(Re.TransAmt);
                            iaw.expenditure = 0.00M;
                            iaw.time_of_occurrence = DateTime.Now;
                            iaw.account_balance = p2.available_balance + iaw.income;  //面要得么帐户余额
                            iaw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.借款人还款.ToString());
                            iaw.createtime = DateTime.Now;
                            iaw.keyid = 0;
                            iaw.remarks = Re.OrdId + "," + Re.OrdDate;
                            //需要更新投资记录表已还款金额
                            B_usercenter BUC = new B_usercenter();
                            decimal PrincipalInterest = decimal.Parse(dtrep.Rows[0]["Principal"].ToString()) + decimal.Parse(dtrep.Rows[0]["interestpayment"].ToString());  //本金加利息
                            decimal PInterest = decimal.Parse(dtrep.Rows[0]["interestpayment"].ToString());  //利息
                            bool lastrepamt = false;
                            int bucd = BUC.Repayment_Successfully(Re, baw, iaw, lastrepamt, PrincipalInterest, PInterest);
                            if (bucd > 0)
                            {
                                // 尊敬#UserName#,您投资的第#PID#号标第#ORDER#期还款已到帐,本次已成功还款#MONEY#元.欢迎继续投资!【创利投】
                                //短信通知
                                string contxt = Utils.GetMSMEmailContext(11, 1); // 获取注册成功邮件内容
                                StringBuilder sbsms = new StringBuilder(contxt);
                                sbsms = sbsms.Replace("#USERANEM#", dtrep.Rows[0]["username"].ToString());
                                sbsms = sbsms.Replace("#PID#", dtrep.Rows[0]["targetid"].ToString());
                                sbsms = sbsms.Replace("#ORDER#", dtrep.Rows[0]["current_investment_period"].ToString());
                                sbsms = sbsms.Replace("#MONEY#", dtrep.Rows[0]["repayment_amount"].ToString());

                                string mobile = dtrep.Rows[0]["mobile"].ToString();
                                M_td_SMS_record psms = new M_td_SMS_record();
                                B_td_SMS_record osms = new B_td_SMS_record();
                                int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.投资回款.ToString());
                                psms.phone_number = mobile;
                                psms.sendtime = DateTime.Now;
                                psms.senduserid = int.Parse(dtrep.Rows[0]["investor_registerid"].ToString());
                                psms.smstype = smstype;
                                psms.smscontext = sbsms.ToString();
                                psms.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());
                                psms.vcode = "";
                                osms.Add(psms);

                                //系统消息
                                DateTime dti = DateTime.Now;
                                M_td_System_message pm = new M_td_System_message();
                                pm.MReg = int.Parse(dtrep.Rows[0]["investor_registerid"].ToString());
                                pm.Mstate = 0;
                                pm.MTitle = "投资回款";
                                //  pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功投资了项目【" + dt.Rows[0]["borrowing_title"].ToString() + "】，投资金额是：" + dt.Rows[0]["investment_amount"].ToString() + "。如有问题可咨询创利投的客服！谢谢！";
                                pm.MContext = sbsms.ToString();
                                pm.PubTime = dti;
                                pm.Mtype = 2;
                                B_usercenter.AddMessage(pm);
                                //Response.Write("还款验签成功！");
                                sb.Append("还款验签成功");
                            }
                            else
                            {
                                //Response.Write("还款更新失败！" + bucd.ToString());
                                sb.Append("还款更新失败！" + bucd.ToString());
                            }
                        }
                    }
                    else
                    {
                        //Response.Write("出现错误！ " + Utils.GetReturnCode(Int32.Parse(Re.RespCode)));
                        string cc = Utils.GetReturnCode(Int32.Parse(Re.RespCode));
                        sb.Append("出现错误！ " + cc);
                        if (string.IsNullOrEmpty(cc))
                        {
                            cc = HttpUtility.UrlDecode(Re.RespDesc);
                        }
                        if ((!string.IsNullOrEmpty(cc)) && (cc.Contains("还款金额超过还款总额") || cc.Contains("重复的还款请求")))
                        {
                            int ic = ef.hx_income_statement.Where(c => c.bid_records_id == bid && c.targetid == targetid && c.orderid == Re.OrdId).Update(c => new hx_income_statement { payment_status = 1, repayment_period = DateTime.Now });

                            if (ic > 0)
                            {
                                int userid = int.Parse(dtrep.Rows[0]["investor_registerid"].ToString());
                                UserInfoData ud = new UserInfoData();
                                ReQueryBalanceBg retloan = ud.Querybalance(userid);
                                M_Capital_account_water baw = new M_Capital_account_water(); //借款人流水对象
                                if (repayment_type == 2)//平台代偿不记录出账人流入
                                {
                                    baw = null;
                                }
                                else
                                {
                                    if (repayment_type == 1)
                                    {
                                        p = o.GetModel(int.Parse(dtrep.Rows[0]["borrower_registerid"].ToString())); //借款人用户对象
                                    }
                                    else if (repayment_type == 3)
                                    {
                                        p = o.GetModelByUsrCustid(Re.OutCustId); //借款人用户对象
                                    }
                                    baw.membertable_registerid = p.registerid;
                                    baw.income = 0.00M;
                                    baw.expenditure = decimal.Parse(Re.TransAmt);
                                    baw.time_of_occurrence = DateTime.Now;
                                    baw.account_balance = p.available_balance - baw.expenditure;  //面要得么帐户余额
                                    baw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.还款.ToString());
                                    baw.createtime = DateTime.Now;
                                    baw.keyid = 0;
                                    baw.remarks = Re.OrdId + "," + Re.OrdDate;
                                }

                                M_Capital_account_water iaw = new M_Capital_account_water(); //投资人流水对象    
                                p2 = o.GetModel(int.Parse(dtrep.Rows[0]["investor_registerid"].ToString())); //投资人用户对象
                                iaw.membertable_registerid = p2.registerid;
                                iaw.income = decimal.Parse(Re.TransAmt);
                                iaw.expenditure = 0.00M;
                                iaw.time_of_occurrence = DateTime.Now;
                                iaw.account_balance = p2.available_balance + iaw.income;  //面要得么帐户余额
                                iaw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.借款人还款.ToString());
                                iaw.createtime = DateTime.Now;
                                iaw.keyid = 0;
                                iaw.remarks = Re.OrdId + "," + Re.OrdDate;


                                //需要更新投资记录表已还款金额
                                B_usercenter BUC = new B_usercenter();
                                decimal PrincipalInterest = decimal.Parse(dtrep.Rows[0]["Principal"].ToString()) + decimal.Parse(dtrep.Rows[0]["interestpayment"].ToString());  //本金加利息
                                decimal PInterest = decimal.Parse(dtrep.Rows[0]["interestpayment"].ToString());  //利息
                                bool lastrepamt = false;
                                int bucd = BUC.Repayment_Successfully(Re, baw, iaw, lastrepamt, PrincipalInterest, PInterest);
                                if (bucd > 0)
                                {
                                    // 尊敬#UserName#,您投资的第#PID#号标第#ORDER#期还款已到帐,本次已成功还款#MONEY#元.欢迎继续投资!【创利投】
                                    //短信通知
                                    string contxt = Utils.GetMSMEmailContext(11, 1); // 获取注册成功邮件内容
                                    StringBuilder sbsms = new StringBuilder(contxt);
                                    sbsms = sbsms.Replace("#USERANEM#", dtrep.Rows[0]["username"].ToString());
                                    sbsms = sbsms.Replace("#PID#", dtrep.Rows[0]["targetid"].ToString());
                                    sbsms = sbsms.Replace("#ORDER#", dtrep.Rows[0]["current_investment_period"].ToString());
                                    sbsms = sbsms.Replace("#MONEY#", dtrep.Rows[0]["repayment_amount"].ToString());

                                    string mobile = dtrep.Rows[0]["mobile"].ToString();

                                    M_td_SMS_record psms = new M_td_SMS_record();
                                    B_td_SMS_record osms = new B_td_SMS_record();
                                    int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.投资回款.ToString());
                                    psms.phone_number = mobile;
                                    psms.sendtime = DateTime.Now;
                                    psms.senduserid = int.Parse(dtrep.Rows[0]["investor_registerid"].ToString());
                                    psms.smstype = smstype;
                                    psms.smscontext = sbsms.ToString();
                                    psms.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());
                                    psms.vcode = "";
                                    osms.Add(psms);
                                    //系统消息
                                    DateTime dti = DateTime.Now;

                                    M_td_System_message pm = new M_td_System_message();
                                    pm.MReg = int.Parse(dtrep.Rows[0]["investor_registerid"].ToString());
                                    pm.Mstate = 0;
                                    pm.MTitle = "投资回款";
                                    //  pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功投资了项目【" + dt.Rows[0]["borrowing_title"].ToString() + "】，投资金额是：" + dt.Rows[0]["investment_amount"].ToString() + "。如有问题可咨询创利投的客服！谢谢！";
                                    pm.MContext = sbsms.ToString();
                                    pm.PubTime = dti;
                                    pm.Mtype = 2;
                                    B_usercenter.AddMessage(pm);
                                    //Response.Write("还款验签成功！");
                                    sb.Append("还款验签成功");
                                }
                                else
                                {
                                    //Response.Write("还款更新失败！" + bucd.ToString());
                                    sb.Append("还款更新失败！" + bucd.ToString());
                                }
                                sb.Append("<br>  系统将未还款成功的数据自动纠正!");

                            }
                        }

                    }

                    #region 判断是否本次还款都已经还完，如果还完则更新借款人本次还款计划
                    sql = "select count(*) from V_borrowing_Bid_records_income_statement  where targetid=" + targetid + "  and    CONVERT(varchar(10), interest_payment_date, 23)=CONVERT(varchar(10), '" + date1 + "', 23) and payment_status=0";
                    DataTable waitForRepay = DbHelperSQL.GET_DataTable_List(sql);
                    if (waitForRepay != null && waitForRepay.Rows.Count > 0 && waitForRepay.Rows[0][0].ToInt() == 0)
                    {
                        sql = "update hx_repayment_plan set repayment_state=1 where targetid=" + targetid + " and CONVERT(varchar(10), repayment_period, 23)=CONVERT(varchar(10), '" + date1 + "', 23) and repayment_state=0;";
                        DbHelperSQL.ExecuteSql(sql);

                        #region 更新标的状态为还还清
                        ///更新标的状态为还还清
                        if (dtrep.Rows[0]["current_period"].ToString() == lastcur.ToString())
                        {
                            //更新标的状态为还还清
                            sql = "update  hx_Bid_records  set  payment_status =1 where bid_records_id=" + dtrep.Rows[0]["bid_records_id"].ToString();
                            DbHelperSQL.ExecuteSql(sql);

                            sql = "select  COALESCE(count(targetid),0) as count   FROM   hx_Bid_records  where targetid=" + targetid.ToString() + "  and  payment_status=0";
                            int idf = DbHelperSQL.Execint(sql);
                            if (idf <= 0)
                            {
                                sql = "update hx_borrowing_target set tender_state=5 where targetid=" + targetid.ToString() + "";
                                DbHelperSQL.ExecuteSql(sql);
                            }
                        }
                        #endregion
                    }
                    #endregion

                }
                else
                {
                    //Response.Write("验签失败！");
                    sb.Append("验签失败 ");
                }
            }
            return Content(sb.ToString());
        }
        #endregion

        #region 未完成还款的继续还款
        /// <summary>
        /// 未完成还款的继续还款
        /// </summary>
        /// <param name="targetid"></param>
        /// <param name="date1"></param>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult repayInvestmentdetails(int targetid, string date1)
        {
            ///TODO   路径：/admin/borrowing_target/repayInvestmentdetails.aspx
            //return Content(StringAlert.Alert("功能待完善"), "text/html");
            StringBuilder sb = new StringBuilder();

            decimal count_AMT = 0M; //应还总金额
            decimal O_penalty = 0M;//逾期
            decimal fees = 0M, Interest_spreads1 = 0M; //管理费
            bool pre = true;

            decimal Inves_Repayment_amount1 = 0M;

            string redate = date1;
            string sql = "";

            int suss = 0;
            int lost = 0;


            sql = "select targetid, borrower_registerid ,investor_registerid,available_balance,BorrUsrCustId,OutCustId,SubOrdid,SubOrdDate ,repayment_amount,interest_payment_date,loan_management_fee,investment_amount,current_investment_period,bid_records_id,username,mobile,Principal,interestpayment,orderid,income_statement_id from V_borrowing_Bid_records_income_statement  where targetid=" + targetid.ToString() + "  and    CONVERT(varchar(10), interest_payment_date, 23)=CONVERT(varchar(10), '" + redate + "', 23) and payment_status=0 ";
            DataTable dtrep = DbHelperSQL.GET_DataTable_List(sql);

            B_member_table o = new B_member_table();
            M_member_table p = new M_member_table();
            M_member_table p2 = new M_member_table();
            if (dtrep.Rows.Count > 0)
            {
                Response.Write("未还款的数据有:" + dtrep.Rows.Count.ToString() + "笔 <br>");


                //判断是否为最后一期还款，并进行相应状态处理
                hx_repayment_plan plan = (from a in ef.hx_repayment_plan where a.targetid == targetid select a).OrderByDescending(a => a.current_period).Take(1).FirstOrDefault();
                var lastcur = plan.current_period;




                for (int i = 0; i < dtrep.Rows.Count; i++)
                {
                    // string ordid = Utils.Createcode(); //生成新的订单号



                    string ordid = ""; //订单号

                    if (dtrep.Rows[i]["orderid"] != null && dtrep.Rows[i]["orderid"].ToString() != "")
                    {
                        ordid = dtrep.Rows[i]["orderid"].ToString();
                    }
                    else
                    {
                        ordid = Utils.Createcode(); //生成新的订单号
                        string sql1 = "update hx_income_statement set  orderid ='" + ordid + "' where income_statement_id=" + dtrep.Rows[i]["income_statement_id"].ToString() + " and  payment_status=0";
                        DbHelperSQL.RunSql(sql1);
                    }



                    M_Repayment MR = new M_Repayment();

                    MR.Version = "20";
                    MR.CmdId = "Repayment";
                    MR.MerCustId = Utils.GetMerCustID();
                    MR.OrdId = ordid;
                    MR.OrdDate = DateTime.Now.ToString("yyyyMMdd");
                    MR.OutCustId = dtrep.Rows[i]["BorrUsrCustId"].ToString(); //投资人帐户(出帐)
                    MR.SubOrdId = dtrep.Rows[i]["SubOrdid"].ToString();
                    MR.SubOrdDate = DateTime.Parse(dtrep.Rows[i]["SubOrdDate"].ToString()).ToString("yyyyMMdd");

                    fees = Calculator.C_fees(Calculator.GetLoan_management_fee(targetid), decimal.Parse(dtrep.Rows[i]["repayment_amount"].ToString()));


                    MR.TransAmt = decimal.Parse(dtrep.Rows[i]["repayment_amount"].ToString()).ToString("0.00");
                    MR.Fee = RMB.GetDecimal(fees, 2, true).ToString("0.00");
                    // MR.Fee = fees.ToString("0.00");
                    MR.InCustId = dtrep.Rows[i]["OutCustId"].ToString(); //投资人帐户客户id

                    if (fees > 0)
                    {
                        M_RP_DivDetails mrp = new M_RP_DivDetails();
                        mrp.DivCustId = Utils.GetMerCustID();
                        mrp.DivAcctId = Utils.GetMERDT();
                        mrp.DivAmt = MR.Fee;

                        MR.DivDetails = "[" + FastJSON.toJOSN(mrp) + "]";

                        MR.FeeObjFlag = "O"; // I 向入款客户号收取  O 向出款客户号收取
                    }


                    MR.BgRetUrl = Utils.GetRe_url("admin/Thirdparty/Su_Repayment");

                    M_RP_ProID mrpp = new M_RP_ProID();
                    mrpp.ProId = targetid.ToString();

                    MR.ReqExt = FastJSON.toJOSN(mrpp);
                    //   MR.MerPriv = "chuanglitou";

                    //MR.MerPriv = Utils.Base64Encoder(DateTime.Parse(dtrep.Rows[i]["interest_payment_date"].ToString()).ToString("yyyy-MM-dd"));

                    MR.MerPriv = DateTime.Parse(dtrep.Rows[i]["interest_payment_date"].ToString()).ToString("yyyy-MM-dd");


                    StringBuilder chkVal = new StringBuilder();
                    chkVal.Append(MR.Version);
                    chkVal.Append(MR.CmdId);
                    chkVal.Append(MR.MerCustId);
                    chkVal.Append(MR.OrdId);
                    chkVal.Append(MR.OrdDate);
                    chkVal.Append(MR.OutCustId);
                    chkVal.Append(MR.SubOrdId);
                    chkVal.Append(MR.SubOrdDate);
                    chkVal.Append(MR.OutAcctId);

                    chkVal.Append(MR.TransAmt);
                    chkVal.Append(MR.Fee);
                    chkVal.Append(MR.InCustId);

                    chkVal.Append(MR.InAcctId);
                    chkVal.Append(MR.DivDetails);
                    chkVal.Append(MR.FeeObjFlag);
                    chkVal.Append(MR.BgRetUrl);
                    chkVal.Append(MR.MerPriv);
                    chkVal.Append(MR.ReqExt);

                    string chkv = chkVal.ToString();

                    LogInfo.WriteLog("批量单个还款：" + chkv);
                    //私钥文件的位置(这里是放在了站点的根目录下)
                    string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
                    //需要指定提交字符串的长度
                    int len = Encoding.UTF8.GetBytes(chkv).Length;
                    StringBuilder sbChkValue = new StringBuilder(256);
                    //加签
                    int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

                    LogInfo.WriteLog(str.ToString());

                    MR.ChkValue = sbChkValue.ToString();

                    LogInfo.WriteLog("批量单个还款提交信息：" + FastJSON.toJOSN(MR));
                    LogInfo.WriteLog("批量单个还款ChkValue:" + MR.ChkValue);

                    using (var client = new WebClient())
                    {


                        var values = new NameValueCollection();
                        values.Add("Version", MR.Version);
                        values.Add("CmdId", MR.CmdId);
                        values.Add("MerCustId", MR.MerCustId);
                        values.Add("OrdId", MR.OrdId);
                        values.Add("OrdDate", MR.OrdDate);
                        values.Add("OutCustId", MR.OutCustId);
                        values.Add("SubOrdId", MR.SubOrdId);
                        values.Add("SubOrdDate", MR.SubOrdDate);
                        values.Add("OutAcctId", MR.OutAcctId);
                        values.Add("TransAmt", MR.TransAmt);
                        values.Add("Fee", MR.Fee);
                        values.Add("InCustId", MR.InCustId);
                        values.Add("InAcctId", MR.InAcctId);
                        values.Add("DivDetails", MR.DivDetails);
                        values.Add("FeeObjFlag", MR.FeeObjFlag);
                        values.Add("BgRetUrl", MR.BgRetUrl);
                        values.Add("MerPriv", MR.MerPriv);
                        values.Add("ReqExt", MR.ReqExt);
                        values.Add("ChkValue", MR.ChkValue);

                        string url = Utils.GetChinapnrUrl();
                        //同步发送form表单请求
                        byte[] result = client.UploadValues(url, "POST", values);
                        var retStr = Encoding.UTF8.GetString(result);
                        //  Response.Write(retStr);

                        LogInfo.WriteLog("批量单个还款同步form表单请求" + retStr);

                        ReRepayment Rere = new ReRepayment();

                        var Re = (ReRepayment)FastJSON.ToObject(retStr, Rere);



                        LogInfo.WriteLog("批量单个还款返回报文：" + FastJSON.toJOSN(Re));


                        StringBuilder builder = new StringBuilder();
                        builder.Append(Re.CmdId);
                        builder.Append(Re.RespCode);
                        builder.Append(Re.MerCustId);
                        builder.Append(Re.OrdId);
                        builder.Append(Re.OrdDate);
                        builder.Append(Re.OutCustId);
                        builder.Append(Re.SubOrdId);
                        builder.Append(Re.SubOrdDate);
                        builder.Append(Re.OutAcctId);
                        builder.Append(Re.TransAmt);
                        builder.Append(Re.Fee);
                        builder.Append(Re.InCustId);
                        builder.Append(Re.InAcctId);
                        builder.Append(Re.FeeObjFlag);
                        builder.Append(HttpUtility.UrlDecode(Re.BgRetUrl));
                        builder.Append(HttpUtility.UrlDecode(Re.MerPriv));
                        builder.Append(HttpUtility.UrlDecode(Re.RespExt));
                        // builder.Append(Re.ChkValue);


                        var msg = builder.ToString();

                        LogInfo.WriteLog("批量单个还款返回参数:" + msg);
                        //验签
                        string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
                        int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, Re.ChkValue);

                        LogInfo.WriteLog("批量单个还款验签 ret= " + ret.ToString());

                        if (ret == 0)
                        {
                            if (Re.RespCode == "000" || Re.TransAmt == "0.00")
                            {


                                string cachename = "Repayment" + Re.OrdId;
                                if (Utils.GeTThirdCache(cachename) == 0)
                                {
                                    Utils.SetThirdCache(cachename);


                                    M_Capital_account_water baw = new M_Capital_account_water(); //借款人流水对象

                                    p = o.GetModel(int.Parse(dtrep.Rows[i]["borrower_registerid"].ToString())); //借款人用户对象

                                    baw.membertable_registerid = p.registerid;
                                    baw.income = 0.00M;
                                    baw.expenditure = decimal.Parse(Re.TransAmt);
                                    baw.time_of_occurrence = DateTime.Now;
                                    baw.account_balance = p.available_balance - baw.expenditure;  //面要得么帐户余额
                                    baw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.还款.ToString());
                                    baw.createtime = DateTime.Now;
                                    baw.keyid = 0;
                                    baw.remarks = Re.OrdId + "," + Re.OrdDate;

                                    M_Capital_account_water iaw = new M_Capital_account_water(); //投资人流水对象


                                    p2 = o.GetModel(int.Parse(dtrep.Rows[i]["investor_registerid"].ToString())); //投资人用户对象

                                    iaw.membertable_registerid = p2.registerid;
                                    iaw.income = decimal.Parse(Re.TransAmt);
                                    iaw.expenditure = 0.00M;
                                    iaw.time_of_occurrence = DateTime.Now;
                                    iaw.account_balance = p2.available_balance + iaw.income;  //面要得么帐户余额
                                    iaw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.借款人还款.ToString());
                                    iaw.createtime = DateTime.Now;
                                    iaw.keyid = 0;
                                    iaw.remarks = Re.OrdId + "," + Re.OrdDate;


                                    //判断是否是最后一期，如果是对本金处理。得直接用最后一期还款金额减去本金，这样就解决多出本金问题
                                    bool lastrepamt = false;

                                    if (dtrep.Rows[0]["current_investment_period"].ToString() == lastcur.ToString())
                                    {
                                        lastrepamt = true;
                                    }


                                    //需要更新投资记录表已还款金额

                                    B_usercenter BUC = new B_usercenter();


                                    decimal PrincipalInterest = decimal.Parse(dtrep.Rows[i]["Principal"].ToString()) + decimal.Parse(dtrep.Rows[i]["interestpayment"].ToString());  //本金加利息


                                    decimal PInterest = decimal.Parse(dtrep.Rows[i]["interestpayment"].ToString());  //利息



                                    int bucd = BUC.Repayment_Successfully(Re, baw, iaw, lastrepamt, PrincipalInterest, PInterest);

                                    if (bucd > 0)
                                    {
                                        // 尊敬#UserName#,您投资的第#PID#号标第#ORDER#期还款已到帐,本次已成功还款#MONEY#元.欢迎继续投资!【创利投】


                                        //短信通知

                                        string contxt = Utils.GetMSMEmailContext(11, 1); // 获取注册成功邮件内容

                                        StringBuilder sbsms = new StringBuilder(contxt);

                                        sbsms = sbsms.Replace("#USERANEM#", dtrep.Rows[i]["username"].ToString());

                                        sbsms = sbsms.Replace("#PID#", dtrep.Rows[i]["targetid"].ToString());

                                        sbsms = sbsms.Replace("#ORDER#", dtrep.Rows[i]["current_investment_period"].ToString());

                                        sbsms = sbsms.Replace("#MONEY#", dtrep.Rows[i]["repayment_amount"].ToString());


                                        string mobile = dtrep.Rows[i]["mobile"].ToString();

                                        M_td_SMS_record psms = new M_td_SMS_record();
                                        B_td_SMS_record osms = new B_td_SMS_record();
                                        int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.投资回款.ToString());
                                        psms.phone_number = mobile;
                                        psms.sendtime = DateTime.Now;
                                        psms.senduserid = int.Parse(dtrep.Rows[i]["investor_registerid"].ToString());
                                        psms.smstype = smstype;
                                        psms.smscontext = sbsms.ToString();
                                        psms.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());
                                        psms.vcode = "";

                                        osms.Add(psms);


                                        //系统消息


                                        DateTime dti = DateTime.Now;

                                        M_td_System_message pm = new M_td_System_message();
                                        pm.MReg = int.Parse(dtrep.Rows[i]["investor_registerid"].ToString());
                                        pm.Mstate = 0;
                                        pm.MTitle = "投资回款";
                                        //  pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功投资了项目【" + dt.Rows[0]["borrowing_title"].ToString() + "】，投资金额是：" + dt.Rows[0]["investment_amount"].ToString() + "。如有问题可咨询创利投的客服！谢谢！";
                                        pm.MContext = sbsms.ToString();
                                        pm.PubTime = dti;
                                        pm.Mtype = 2;
                                        B_usercenter.AddMessage(pm);




                                        #region 更新标的状态为还还清
                                        ///更新标的状态为还还清

                                        if (dtrep.Rows[0]["current_investment_period"].ToString() == lastcur.ToString())
                                        {


                                            sql = "update  hx_Bid_records  set  payment_status =1 where bid_records_id=" + dtrep.Rows[i]["bid_records_id"].ToString();
                                            DbHelperSQL.ExecuteSql(sql);

                                            sql = "select  COALESCE(count(targetid),0) as count   FROM   hx_Bid_records  where targetid=" + dtrep.Rows[0]["targetid"].ToString() + "  and  payment_status=0";

                                            int idf = DbHelperSQL.Execint(sql);
                                            if (idf <= 0)
                                            {
                                                //更新标的状态为还还清

                                                sql = "update hx_borrowing_target set tender_state=5 where targetid=" + dtrep.Rows[0]["targetid"].ToString() + "";
                                                DbHelperSQL.ExecuteSql(sql);

                                            }

                                        }
                                        #endregion




                                        suss = suss + 1;

                                        //Response.Write("还款验签成功！<br>");

                                        sb.Append("还款验签成功！<br>");

                                    }
                                    else
                                    {

                                        lost = lost + 1;
                                        //Response.Write("还款更新失败！" + bucd.ToString());

                                        sb.Append("还款更新失败！" + bucd.ToString());
                                    }




                                }

                            }
                            else
                            {
                                lost = lost + 1;


                                string cc = Utils.GetReturnCode(Int32.Parse(Re.RespCode));

                                sb.Append("出现错误！ " + cc);

                                int bid = int.Parse(dtrep.Rows[0]["bid_records_id"].ToString());

                                if (cc.Contains("还款金额超过还款总额") || cc.Contains("重复的还款请求"))
                                {
                                    int ic = ef.hx_income_statement.Where(c => c.bid_records_id == bid && c.targetid == targetid && c.orderid == Re.OrdId).Update(c => new hx_income_statement { payment_status = 1, repayment_period = DateTime.Now });

                                    if (ic > 0)
                                    {
                                        int userid = int.Parse(dtrep.Rows[0]["investor_registerid"].ToString());
                                        UserInfoData ud = new UserInfoData();
                                        ReQueryBalanceBg retloan = ud.Querybalance(userid);



                                        M_Capital_account_water baw = new M_Capital_account_water(); //借款人流水对象

                                        p = o.GetModel(int.Parse(dtrep.Rows[0]["borrower_registerid"].ToString())); //借款人用户对象

                                        baw.membertable_registerid = p.registerid;
                                        baw.income = 0.00M;
                                        baw.expenditure = decimal.Parse(Re.TransAmt);
                                        baw.time_of_occurrence = DateTime.Now;
                                        baw.account_balance = p.available_balance - baw.expenditure;  //面要得么帐户余额
                                        baw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.还款.ToString());
                                        baw.createtime = DateTime.Now;
                                        baw.keyid = 0;
                                        baw.remarks = Re.OrdId + "," + Re.OrdDate;





                                        M_Capital_account_water iaw = new M_Capital_account_water(); //投资人流水对象


                                        p2 = o.GetModel(int.Parse(dtrep.Rows[0]["investor_registerid"].ToString())); //投资人用户对象

                                        iaw.membertable_registerid = p2.registerid;
                                        iaw.income = decimal.Parse(Re.TransAmt);
                                        iaw.expenditure = 0.00M;
                                        iaw.time_of_occurrence = DateTime.Now;
                                        iaw.account_balance = p2.available_balance + iaw.income;  //面要得么帐户余额
                                        iaw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.借款人还款.ToString());
                                        iaw.createtime = DateTime.Now;
                                        iaw.keyid = 0;
                                        iaw.remarks = Re.OrdId + "," + Re.OrdDate;



                                        //需要更新投资记录表已还款金额
                                        B_usercenter BUC = new B_usercenter();


                                        decimal PrincipalInterest = decimal.Parse(dtrep.Rows[0]["Principal"].ToString()) + decimal.Parse(dtrep.Rows[0]["interestpayment"].ToString());  //本金加利息


                                        decimal PInterest = decimal.Parse(dtrep.Rows[0]["interestpayment"].ToString());  //利息

                                        bool lastrepamt = false;





                                        int bucd = BUC.Repayment_Successfully(Re, baw, iaw, lastrepamt, PrincipalInterest, PInterest);

                                        if (bucd > 0)
                                        {
                                            // 尊敬#UserName#,您投资的第#PID#号标第#ORDER#期还款已到帐,本次已成功还款#MONEY#元.欢迎继续投资!【创利投】



                                            //短信通知

                                            string contxt = Utils.GetMSMEmailContext(11, 1); // 获取注册成功邮件内容

                                            StringBuilder sbsms = new StringBuilder(contxt);

                                            sbsms = sbsms.Replace("#USERANEM#", dtrep.Rows[0]["username"].ToString());

                                            sbsms = sbsms.Replace("#PID#", dtrep.Rows[0]["targetid"].ToString());

                                            sbsms = sbsms.Replace("#ORDER#", dtrep.Rows[0]["current_investment_period"].ToString());

                                            sbsms = sbsms.Replace("#MONEY#", dtrep.Rows[0]["repayment_amount"].ToString());


                                            string mobile = dtrep.Rows[0]["mobile"].ToString();

                                            M_td_SMS_record psms = new M_td_SMS_record();
                                            B_td_SMS_record osms = new B_td_SMS_record();
                                            int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.投资回款.ToString());
                                            psms.phone_number = mobile;
                                            psms.sendtime = DateTime.Now;
                                            psms.senduserid = int.Parse(dtrep.Rows[0]["investor_registerid"].ToString());
                                            psms.smstype = smstype;
                                            psms.smscontext = sbsms.ToString();
                                            psms.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());
                                            psms.vcode = "";

                                            osms.Add(psms);

                                            //系统消息
                                            DateTime dti = DateTime.Now;

                                            M_td_System_message pm = new M_td_System_message();
                                            pm.MReg = int.Parse(dtrep.Rows[0]["investor_registerid"].ToString());
                                            pm.Mstate = 0;
                                            pm.MTitle = "投资回款";
                                            //  pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功投资了项目【" + dt.Rows[0]["borrowing_title"].ToString() + "】，投资金额是：" + dt.Rows[0]["investment_amount"].ToString() + "。如有问题可咨询创利投的客服！谢谢！";
                                            pm.MContext = sbsms.ToString();
                                            pm.PubTime = dti;
                                            pm.Mtype = 2;
                                            B_usercenter.AddMessage(pm);


                                            //Response.Write("还款验签成功！");
                                            sb.Append("还款验签成功");


                                            #region 更新标的状态为还还清
                                            ///更新标的状态为还还清

                                            if (dtrep.Rows[0]["current_investment_period"].ToString() == lastcur.ToString())
                                            {


                                                sql = "update  hx_Bid_records  set  payment_status =1 where bid_records_id=" + dtrep.Rows[i]["bid_records_id"].ToString();
                                                DbHelperSQL.ExecuteSql(sql);

                                                sql = "select  COALESCE(count(targetid),0) as count   FROM   hx_Bid_records  where targetid=" + dtrep.Rows[0]["targetid"].ToString() + "  and  payment_status=0";

                                                int idf = DbHelperSQL.Execint(sql);
                                                if (idf <= 0)
                                                {
                                                    //更新标的状态为还还清

                                                    sql = "update hx_borrowing_target set tender_state=5 where targetid=" + dtrep.Rows[0]["targetid"].ToString() + "";
                                                    DbHelperSQL.ExecuteSql(sql);

                                                }

                                            }
                                            #endregion


                                        }
                                        else
                                        {
                                            //Response.Write("还款更新失败！" + bucd.ToString());
                                            sb.Append("还款更新失败！" + bucd.ToString());
                                        }




                                        sb.Append("<br>  系统将未还款成功的数据自动纠正!");

                                    }
                                }


                                //Response.Write("出现错误！ " + Utils.GetReturnCode(Int32.Parse(Re.RespCode)));

                                sb.Append("出现错误！ " + Utils.GetReturnCode(Int32.Parse(Re.RespCode)));

                            }

                        }
                        else
                        {

                            //Response.Write("验签失败！");

                            sb.Append("验签失败！");
                        }

                    }


                }

                //Response.Write("未还款的数据有:" + dtrep.Rows.Count.ToString() + "笔    成功还款: " + suss.ToString() + " 失败: " + lost.ToString() + "<br>");

                sb.Append("未还款的数据有:" + dtrep.Rows.Count.ToString() + "笔    成功还款: " + suss.ToString() + " 失败: " + lost.ToString() + "<br>");
            }

            else
            {
                /*
                hx_borrowing_target bt = ef.hx_borrowing_target.Where(h => h.targetid == targetid).FirstOrDefault();
                if(bt!=null)
                {
                    if(bt.tender_state==4)
                    {

                        ef.hx_borrowing_target.Where(h => h.targetid == targetid).Update(h => new hx_borrowing_target { tender_state = 5 });

                    }


                }
                */

                List<hx_income_statement> ist = ef.hx_income_statement.Where(h => h.targetid == targetid && h.payment_status == 0).ToList();

                if (ist.Count > 0)
                {

                }
                else
                {
                    ef.hx_borrowing_target.Where(h => h.targetid == targetid).Update(h => new hx_borrowing_target { tender_state = 5 });
                }





                //Response.Write("没有款还款数据");

                sb.Append("没有款还款数据");
            }

            return Content(sb.ToString());
        }
        #endregion

        #region 投资明细
        /// <summary>
        /// 投资明细
        /// </summary>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult Investmentdetails(int id, string date1)
        {
            DateTime dt1 = DateTime.Parse(date1);
            IEnumerable<V_borrowing_Bid_records_income_statement_uc> list = (from a in ef.V_borrowing_Bid_records_income_statement_uc where a.targetid == id && ((DateTime)a.interest_payment_date).CompareTo(dt1) == 0 select a).ToList();

            ViewBag.id = id;
            ViewBag.date1 = date1;

            return View(list);
        }
        #endregion



        #region 还款列表
        /// <summary>
        /// 还款
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Repayment(int planid, int targetid)
        {
            Utils.SetSYSDateTimeFormat();
            V_borrow_repayment_plan item = (from a in ef.V_borrow_repayment_plan where a.tender_state == 4 && a.repayment_state == 0 && a.repayment_plan_id == planid select a).OrderBy(a => a.current_period).SingleOrDefault();

            ReQueryBalanceBg balance = ChinapnrFacade.QueryBalance(Utils.GetDanbaoCustID());
            if (balance != null && balance.RespCode == "000")
            {
                ViewBag.DanbaoBalance = balance.AvlBal;
            }
            else
            {
                ViewBag.DanbaoBalance = 0;
            }
            ViewBag.planid = planid;
            ViewBag.targetid = targetid;
            return View(item);
        }
        #endregion

        #region 还款
        /// <summary>
        /// 还款  
        /// </summary>
        /// <param name="repayment_plan_id"></param>
        /// <param name="targetid"></param>
        /// <param name="reviewremarks"></param>
        /// <param name="shall_repayment"></param>
        /// <returns></returns>      
        [AdminVaildate(false, true)]
        public ActionResult PostRepayment(int hid_planid, int hid_targetid, string reviewremarks, decimal shall_repayment, int repayment_type)
        {
            string json = "{\"ret\":0,\"msg\":\"操作失败\"}";


            decimal available_balance = DNTRequest.GetDecimal("available_balance", 0.00M);


            if (shall_repayment > available_balance)
            {
                json = "{\"ret\":0,\"msg\":\"还款余额不足\"}";
                return Content(json);
            }



            if (repayment_type == 1)
            {
                shall_repayment = 0;
            }


            B_member_table o = new B_member_table();
            M_member_table p = new M_member_table();
            M_member_table p2 = new M_member_table();



            var result = ef.hx_repayment_plan.AsNoTracking().Where(a => a.repayment_plan_id == hid_planid).Update(a => new hx_repayment_plan { repayment_type = repayment_type, Rep_Remarks = reviewremarks, pla_amt_generation = shall_repayment });

            List<V_borrow_repayment_plan> list = (from a in ef.V_borrow_repayment_plan where a.tender_state == 4 && ((int)a.repayment_state) == 0 && a.repayment_plan_id == hid_planid select a).OrderBy(a => a.current_period).ToList();

            if (list != null)
            {
                //判断是否为最后一期还款，并进行相应状态处理
                hx_repayment_plan plan = (from a in ef.hx_repayment_plan where a.targetid == hid_targetid select a).OrderByDescending(a => a.current_period).Take(1).FirstOrDefault();

                var lastcur = plan.current_period;

                DateTime dtime = DateTime.Parse(list[0].repayment_period.ToString());

                List<V_borrowing_Bid_records_income_statement> list_dtrep = (from a in ef.V_borrowing_Bid_records_income_statement where a.targetid == hid_targetid && a.payment_status == 0 && ((DateTime)a.interest_payment_date).CompareTo(dtime) == 0 select a).ToList();

                int sussname = 0;

                int failname = 0;
                foreach (V_borrowing_Bid_records_income_statement item in list_dtrep)
                {

                    string ordid = ""; //订单号

                    if (item.orderid != null && item.orderid.ToString() != "")
                    {
                        ordid = item.orderid;
                    }
                    else
                    {
                        ordid = Utils.Createcode(); //生成新的订单号
                        string sql1 = "update hx_income_statement set  orderid ='" + ordid + "' where income_statement_id=" + item.income_statement_id + " and  payment_status=0";
                        DbHelperSQL.RunSql(sql1);
                    }





                    M_Repayment MR = new M_Repayment();

                    MR.Version = "20";
                    MR.CmdId = "Repayment";
                    MR.MerCustId = Utils.GetMerCustID();
                    MR.OrdId = ordid;
                    MR.OrdDate = DateTime.Now.ToString("yyyyMMdd");
                    MR.OutCustId = item.BorrUsrCustId; //投资人帐户(出帐)
                    MR.SubOrdId = item.SubOrdid;
                    MR.SubOrdDate = DateTime.Parse(item.SubOrdDate.ToString()).ToString("yyyyMMdd");

                    var fees = Calculator.C_fees(decimal.Parse(item.loan_management_fee.ToString()), decimal.Parse(item.repayment_amount.ToString()));

                    MR.TransAmt = decimal.Parse(item.repayment_amount.ToString()).ToString("0.00");
                    MR.Fee = RMB.GetDecimal(fees, 2, true).ToString("0.00");
                    MR.InCustId = item.OutCustId; //投资人帐户客户id

                    if (fees > 0)
                    {
                        M_RP_DivDetails mrp = new M_RP_DivDetails();
                        mrp.DivCustId = Utils.GetMerCustID();
                        mrp.DivAcctId = Utils.GetMERDT();
                        mrp.DivAmt = MR.Fee;
                        MR.DivDetails = "[" + FastJSON.toJOSN(mrp) + "]";
                        MR.FeeObjFlag = "O"; // I 向入款客户号收取  O 向出款客户号收取
                    }
                    MR.BgRetUrl = Utils.GetRe_url("admin/Thirdparty/Su_Repayment");
                    M_RP_ProID mrpp = new M_RP_ProID();
                    mrpp.ProId = list[0].targetid.ToString();
                    MR.ReqExt = FastJSON.toJOSN(mrpp);
                    /*这里做了更新 直接传 income_statement_id*/
                    // MR.MerPriv = Utils.Base64Encoder(DateTime.Parse(item.interest_payment_date.ToString()).ToString("yyyy-MM-dd"));
                    MR.MerPriv = item.income_statement_id.ToString();
                    StringBuilder chkVal = new StringBuilder();
                    chkVal.Append(MR.Version);
                    chkVal.Append(MR.CmdId);
                    chkVal.Append(MR.MerCustId);
                    chkVal.Append(MR.OrdId);
                    chkVal.Append(MR.OrdDate);
                    chkVal.Append(MR.OutCustId);
                    chkVal.Append(MR.SubOrdId);
                    chkVal.Append(MR.SubOrdDate);
                    chkVal.Append(MR.OutAcctId);

                    chkVal.Append(MR.TransAmt);
                    chkVal.Append(MR.Fee);
                    chkVal.Append(MR.InCustId);

                    chkVal.Append(MR.InAcctId);
                    chkVal.Append(MR.DivDetails);
                    chkVal.Append(MR.FeeObjFlag);
                    chkVal.Append(MR.BgRetUrl);
                    chkVal.Append(MR.MerPriv);
                    chkVal.Append(MR.ReqExt);

                    string chkv = chkVal.ToString();

                    LogInfo.WriteLog("还款：" + chkv);

                    //私钥文件的位置(这里是放在了站点的根目录下)
                    string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
                    //需要指定提交字符串的长度
                    int len = Encoding.UTF8.GetBytes(chkv).Length;
                    StringBuilder sbChkValue = new StringBuilder(256);
                    //加签
                    int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

                    LogInfo.WriteLog(str.ToString());

                    MR.ChkValue = sbChkValue.ToString();

                    LogInfo.WriteLog("还款提交信息：" + FastJSON.toJOSN(MR));
                    LogInfo.WriteLog("还款ChkValue:" + MR.ChkValue);

                    using (var client = new WebClient())
                    {
                        var values = new NameValueCollection();
                        values.Add("Version", MR.Version);
                        values.Add("CmdId", MR.CmdId);
                        values.Add("MerCustId", MR.MerCustId);
                        values.Add("OrdId", MR.OrdId);
                        values.Add("OrdDate", MR.OrdDate);
                        values.Add("OutCustId", MR.OutCustId);
                        values.Add("SubOrdId", MR.SubOrdId);
                        values.Add("SubOrdDate", MR.SubOrdDate);
                        values.Add("OutAcctId", MR.OutAcctId);
                        values.Add("TransAmt", MR.TransAmt);
                        values.Add("Fee", MR.Fee);
                        values.Add("InCustId", MR.InCustId);
                        values.Add("InAcctId", MR.InAcctId);
                        values.Add("DivDetails", MR.DivDetails);
                        values.Add("FeeObjFlag", MR.FeeObjFlag);
                        values.Add("BgRetUrl", MR.BgRetUrl);
                        values.Add("MerPriv", MR.MerPriv);
                        values.Add("ReqExt", MR.ReqExt);
                        values.Add("ChkValue", MR.ChkValue);

                        string url = Utils.GetChinapnrUrl();
                        //同步发送form表单请求
                        var _result = client.UploadValues(url, "POST", values);
                        var retStr = Encoding.UTF8.GetString(_result);



                        //  Response.Write(retStr);

                        LogInfo.WriteLog("还款同步form表单请求" + retStr);

                        ReRepayment Rere = new ReRepayment();

                        var Re = (ReRepayment)FastJSON.ToObject(retStr, Rere);

                        LogInfo.WriteLog("还款返回报文：" + FastJSON.toJOSN(Re));
                        StringBuilder builder = new StringBuilder();
                        builder.Append(Re.CmdId);
                        builder.Append(Re.RespCode);
                        builder.Append(Re.MerCustId);
                        builder.Append(Re.OrdId);
                        builder.Append(Re.OrdDate);
                        builder.Append(Re.OutCustId);
                        builder.Append(Re.SubOrdId);
                        builder.Append(Re.SubOrdDate);
                        builder.Append(Re.OutAcctId);
                        builder.Append(Re.TransAmt);
                        builder.Append(Re.Fee);
                        builder.Append(Re.InCustId);
                        builder.Append(Re.InAcctId);
                        builder.Append(Re.FeeObjFlag);
                        builder.Append(HttpUtility.UrlDecode(Re.BgRetUrl));
                        builder.Append(HttpUtility.UrlDecode(Re.MerPriv));
                        builder.Append(HttpUtility.UrlDecode(Re.RespExt));
                        // builder.Append(Re.ChkValue);
                        var msg = builder.ToString();

                        LogInfo.WriteLog("还款返回参数:" + msg);
                        //验签
                        string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
                        int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, Re.ChkValue);

                        LogInfo.WriteLog("验签 ret= " + ret.ToString());
                        if (ret == 0)
                        {

                            if (Re.RespCode == "000" || Re.TransAmt == "0.00")
                            {

                                string cachename = "Repayment" + Re.OrdId;
                                if (Utils.GeTThirdCache(cachename) == 0)
                                {
                                    Utils.SetThirdCache(cachename);
                                    M_Capital_account_water baw = new M_Capital_account_water(); //借款人流水对象

                                    p = o.GetModel(int.Parse(item.borrower_registerid.ToString())); //借款人用户对象

                                    baw.membertable_registerid = p.registerid;
                                    baw.income = 0.00M;
                                    baw.expenditure = decimal.Parse(Re.TransAmt);
                                    baw.time_of_occurrence = DateTime.Now;
                                    baw.account_balance = p.available_balance - baw.expenditure;  //面要得么帐户余额
                                    baw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.还款.ToString());
                                    baw.createtime = DateTime.Now;
                                    baw.keyid = 0;
                                    baw.remarks = Re.OrdId + "," + Re.OrdDate;

                                    M_Capital_account_water iaw = new M_Capital_account_water(); //投资人流水对象


                                    p2 = o.GetModel(int.Parse(item.investor_registerid.ToString())); //投资人用户对象

                                    UserInfoData ud = new UserInfoData();
                                    ReQueryBalanceBg retloan = ud.Querybalance(Convert.ToInt32(item.investor_registerid.ToString()));
                                    iaw.membertable_registerid = p2.registerid;
                                    iaw.income = decimal.Parse(Re.TransAmt);
                                    iaw.expenditure = 0.00M;
                                    iaw.time_of_occurrence = DateTime.Now;
                                    iaw.account_balance = decimal.Parse(retloan.AvlBal);  //面要得么帐户余额
                                    iaw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.借款人还款.ToString());
                                    iaw.createtime = DateTime.Now;
                                    iaw.keyid = 0;
                                    iaw.remarks = Re.OrdId + "," + Re.OrdDate;
                                    LogInfo.WriteLog("投资者余额：" + retloan.AvlBal);

                                    //判断是否是最后一期，如果是对本金处理。得直接用最后一期还款金额减去本金，这样就解决多出本金问题
                                    bool lastrepamt = false;

                                    if (list[0].current_period.ToString() == lastcur.ToString())
                                    {
                                        lastrepamt = true;
                                    }



                                    //需要更新投资记录表已还款金额

                                    B_usercenter BUC = new B_usercenter();


                                    decimal PrincipalInterest = decimal.Parse(item.Principal.ToString()) + decimal.Parse(item.interestpayment.ToString());  //本金加利息


                                    decimal PInterest = decimal.Parse(item.interestpayment.ToString());  //利息

                                    Re.MerPriv = DateTime.Parse(item.interest_payment_date.ToString()).ToString("yyyy-MM-dd"); //传入还款日期
                                    int bucd = BUC.Repayment_Successfully(Re, baw, iaw, lastrepamt, PrincipalInterest, PInterest);

                                    if (bucd > 0)
                                    {
                                        // 尊敬#UserName#,您投资的第#PID#号标第#ORDER#期还款已到帐,本次已成功还款#MONEY#元.欢迎继续投资!【创利投】



                                        #region 短信通知
                                        //短信通知

                                        string contxt = Utils.GetMSMEmailContext(11, 1); // 获取注册成功邮件内容

                                        StringBuilder sbsms = new StringBuilder(contxt);

                                        sbsms = sbsms.Replace("#USERANEM#", item.username.ToString());

                                        sbsms = sbsms.Replace("#PID#", item.targetid.ToString());

                                        sbsms = sbsms.Replace("#ORDER#", item.current_investment_period.ToString());

                                        sbsms = sbsms.Replace("#MONEY#", item.repayment_amount.ToString());


                                        string mobile = item.mobile.ToString();

                                        M_td_SMS_record psms = new M_td_SMS_record();
                                        B_td_SMS_record osms = new B_td_SMS_record();
                                        int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.投资回款.ToString());
                                        psms.phone_number = mobile;
                                        psms.sendtime = DateTime.Now;
                                        psms.senduserid = int.Parse(item.investor_registerid.ToString());
                                        psms.smstype = smstype;
                                        psms.smscontext = sbsms.ToString();
                                        psms.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());
                                        psms.vcode = "";

                                        osms.Add(psms);
                                        #endregion


                                        #region 系统消息
                                        //系统消息


                                        DateTime dti = DateTime.Now;

                                        M_td_System_message pm = new M_td_System_message();
                                        pm.MReg = int.Parse(item.investor_registerid.ToString());
                                        pm.Mstate = 0;
                                        pm.MTitle = "投资回款";
                                        //  pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功投资了项目【" + dt.Rows[0]["borrowing_title"].ToString() + "】，投资金额是：" + dt.Rows[0]["investment_amount"].ToString() + "。如有问题可咨询创利投的客服！谢谢！";
                                        pm.MContext = sbsms.ToString();
                                        pm.PubTime = dti;
                                        pm.Mtype = 2;
                                        B_usercenter.AddMessage(pm);


                                        #endregion

                                        #region 更新标的状态为还还清
                                        ///更新标的状态为还还清

                                        if (list[0].current_period.ToString() == lastcur.ToString())
                                        {
                                            //更新标的状态为还还清

                                            string sql = "update hx_borrowing_target set tender_state=5 where targetid=" + list[0].targetid.ToString() + "";
                                            DbHelperSQL.ExecuteSql(sql);

                                            sql = "update  hx_Bid_records  set  payment_status =1 where bid_records_id=" + item.bid_records_id.ToString();
                                            DbHelperSQL.ExecuteSql(sql);

                                        }
                                        #endregion


                                        //Response.Write("还款验签成功！");
                                    }
                                    else
                                    {
                                        //Response.Write("还款更新失败！" + bucd.ToString());
                                    }

                                    sussname = sussname + 1;
                                    //更新投资人帐户信息
                                    //更新借款人帐户信息
                                    //更新投资人还款计划列表
                                }
                            }
                            else
                            {

                                failname = failname + 1;

                                /*  
                                if (dt.Rows[0]["current_period"].ToString() == lastcur.ToString())
                              {
                                  //更新标的状态为还还清
                                  sql = "update hx_borrowing_target set tender_state=4 where targetid=" + dt.Rows[0]["targetid"].ToString() + "";
                                  DbHelperSQL.ExecuteSql(sql);

                              }*/
                                // Response.Write("出现错误！ " + Utils.GetReturnCode(Int32.Parse(Re.RespCode)));
                            }

                        }
                        else
                        {
                            /*
                            if (dt.Rows[0]["current_period"].ToString() == lastcur.ToString())
                            {
                                //更新标的状态为还还清
                                sql = "update hx_borrowing_target set tender_state=4 where targetid=" + dt.Rows[0]["targetid"].ToString() + "";
                                DbHelperSQL.ExecuteSql(sql);

                            }
                            */
                            //Response.Write("验签失败！");
                        }



                        ///


                    }


                    //Response.Write("<br>" + sussname.ToString() + " 位客户还款成功  " + failname.ToString() + "  位客户还款失败 !<br>");
                    string tempStr = sussname.ToString() + " 笔投资还款成功  " + failname.ToString() + "  笔投资还款失败 !";
                    json = "{\"ret\":1,\"msg\":\"" + tempStr + "\"}";
                }


            }
            return Content(json, "text/json");

        }

        [AdminVaildate(false, true)]
        //执行还款操作
        public JsonResult NewPostRepayment(int hid_planid, int hid_targetid, string reviewremarks, decimal shall_repayment, int repayment_type, decimal available_balance)
        {
            string json = "{\"ret\":0,\"msg\":\"操作失败\"}";
            //decimal available_balance = DNTRequest.GetDecimal("available_balance", 0.00M);
            decimal plaform_RepayAmt = 0.00M;//平台代还金额           
            if (repayment_type == 1)//1正常还款  2平台代还 3担保公司代还
            {
                plaform_RepayAmt = 0;
                if (shall_repayment > available_balance)
                {
                    json = "{\"ret\":0,\"msg\":\"还款余额不足\"}";
                    return Json(json);
                }
            }
            else
            {
                plaform_RepayAmt = shall_repayment;//平台待还金额=总金额
            }
            B_member_table o = new B_member_table();
            M_member_table p = new M_member_table();
            M_member_table p2 = new M_member_table();
            var result = ef.hx_repayment_plan.AsNoTracking().Where(a => a.repayment_plan_id == hid_planid).Update(a => new hx_repayment_plan { repayment_type = repayment_type, Rep_Remarks = reviewremarks, pla_amt_generation = plaform_RepayAmt });

            List<V_borrow_repayment_plan> list = (from a in ef.V_borrow_repayment_plan where a.tender_state == 4 && ((int)a.repayment_state) == 0 && a.repayment_plan_id == hid_planid select a).OrderBy(a => a.current_period).ToList();

            if (list != null)
            {
                //判断是否为最后一期还款，并进行相应状态处理
                hx_repayment_plan plan = (from a in ef.hx_repayment_plan where a.targetid == hid_targetid select a).OrderByDescending(a => a.current_period).Take(1).FirstOrDefault();
                var lastcur = plan.current_period;
                DateTime dtime = DateTime.Parse(list[0].repayment_period.ToString());
                List<V_borrowing_Bid_records_income_statement> list_dtrep = (from a in ef.V_borrowing_Bid_records_income_statement where a.targetid == hid_targetid && a.payment_status == 0 && ((DateTime)a.interest_payment_date).CompareTo(dtime) == 0 select a).ToList();

                int sussname = 0;
                int failname = 0;
                if (list_dtrep == null || list_dtrep.Count == 0)
                {
                    #region 判断是否本次还款都已经还完，如果还完则更新借款人本次还款计划
                    var result1 = ef.hx_repayment_plan.AsNoTracking().Where(a => a.repayment_plan_id == hid_planid).Update(a => new hx_repayment_plan { repayment_state = 1 });
                    #endregion

                    json = "{\"ret\":0,\"msg\":\"此回款计划已经结清！\"}";
                    return Json(json);
                }
                LogInfo.WriteLog("标的 "+ hid_targetid + " 批量还款开始。。。。");
                foreach (V_borrowing_Bid_records_income_statement item in list_dtrep)
                {
                    string ordid = ""; //订单号
                    if (item.orderid != null && item.orderid.ToString() != "")
                    {
                        ordid = item.orderid;
                    }
                    else
                    {
                        ordid = Utils.Createcode(); //生成新的订单号
                        string sql1 = "update hx_income_statement set  orderid ='" + ordid + "' where income_statement_id=" + item.income_statement_id + " and  payment_status=0";
                        DbHelperSQL.RunSql(sql1);
                    }
                    M_Repayment MR = new M_Repayment();
                    MR.Version = "30";
                    MR.CmdId = "Repayment";
                    MR.MerCustId = Utils.GetMerCustID();
                    MR.OrdId = ordid;
                    MR.OrdDate = DateTime.Now.ToString("yyyyMMdd");
                    if (repayment_type == 1)//)//1正常还款  2平台代还 3担保公司代还
                    {
                        MR.OutCustId = item.BorrUsrCustId;//借款人人帐户(出帐)
                    }
                    else if (repayment_type == 2)
                    {
                        MR.OutCustId = Utils.GetMerCustID();
                        MR.OutAcctId = "MDT000001";
                        MR.DzObject = item.BorrUsrCustId;
                        MR.InAcctId = "MDT000001";
                    }
                    else if (repayment_type == 3)
                    {
                        MR.OutCustId = Utils.GetDanbaoCustID();
                        MR.OutAcctId = "MDT000001";
                        MR.DzObject = item.BorrUsrCustId;
                        MR.InAcctId = "MDT000001";
                    }
                    MR.SubOrdId = item.SubOrdid;
                    MR.SubOrdDate = DateTime.Parse(item.SubOrdDate.ToString()).ToString("yyyyMMdd");

                    var fees = Calculator.C_fees(decimal.Parse(item.loan_management_fee.ToString()), decimal.Parse(item.repayment_amount.ToString()));
                    //MR.TransAmt = decimal.Parse(item.repayment_amount.ToString()).ToString("0.00");
                    MR.PrincipalAmt = (decimal.Parse(item.repayment_amount.ToString()) - decimal.Parse(item.interestpayment.ToString())).ToString("0.00");//decimal.Parse(item.Principal.ToString()).ToString("0.00");
                    MR.InterestAmt = decimal.Parse(item.interestpayment.ToString()).ToString("0.00");
                    MR.Fee = RMB.GetDecimal(fees, 2, true).ToString("0.00");
                    MR.InCustId = item.OutCustId; //投资人帐户客户id
                    if (fees > 0)
                    {
                        M_RP_DivDetails mrp = new M_RP_DivDetails();
                        mrp.DivCustId = Utils.GetMerCustID();
                        mrp.DivAcctId = Utils.GetMERDT();
                        mrp.DivAmt = MR.Fee;
                        MR.DivDetails = "[" + FastJSON.toJOSN(mrp) + "]";
                        MR.FeeObjFlag = "O"; // I 向入款客户号收取  O 向出款客户号收取
                    }
                    MR.BgRetUrl = Utils.GetRe_url("admin/Thirdparty/Su_Repayment");
                    M_RP_ProID mrpp = new M_RP_ProID();
                    mrpp.ProId = list[0].targetid.ToString();
                    MR.ReqExt = FastJSON.toJOSN(mrpp);
                    /*这里做了更新 直接传 income_statement_id*/
                    // MR.MerPriv = Utils.Base64Encoder(DateTime.Parse(item.interest_payment_date.ToString()).ToString("yyyy-MM-dd"));
                    MR.ProId = mrpp.ProId;
                    MR.MerPriv = item.income_statement_id.ToString();

                    ReRepayment Re = ChinapnrFacade.Repayment3(MR);
                    Re.TransAmt = (decimal.Parse(MR.PrincipalAmt == null ? "0.00" : MR.PrincipalAmt) + decimal.Parse(MR.InterestAmt == null ? "0.00" : MR.InterestAmt)).ToString("0.00");//decimal.Parse(item.Principal.ToString()).ToString("0.00");
                    if (Re != null)
                    {
                        if (Re.RespCode == "000" || Re.TransAmt == "0.00")
                        {
                            string cachename = "Repayment" + Re.OrdId;
                            if (Utils.GeTThirdCache(cachename) == 0)
                            {
                                Utils.SetThirdCache(cachename);
                                M_Capital_account_water baw = new M_Capital_account_water(); //借款人流水对象

                                if (repayment_type == 2)//平台代偿不记录出账人流入
                                {
                                    baw = null;
                                }
                                else
                                {
                                    if (repayment_type == 1)
                                    {
                                        p = o.GetModel(int.Parse(item.borrower_registerid.ToString())); //借款人用户对象
                                    }
                                    else if (repayment_type == 3)
                                    {
                                        p = o.GetModelByUsrCustid(Re.OutCustId); //借款人用户对象
                                    }
                                    baw.membertable_registerid = p.registerid;
                                    baw.income = 0.00M;
                                    baw.expenditure = decimal.Parse(Re.TransAmt);
                                    baw.time_of_occurrence = DateTime.Now;
                                    baw.account_balance = p.available_balance - baw.expenditure;  //帐户余额
                                    baw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.还款.ToString());
                                    baw.createtime = DateTime.Now;
                                    baw.keyid = 0;
                                    baw.remarks = Re.OrdId + "," + Re.OrdDate;
                                }
                                M_Capital_account_water iaw = new M_Capital_account_water(); //投资人流水对象  
                                p2 = o.GetModel(int.Parse(item.investor_registerid.ToString())); //投资人用户对象
                                iaw.membertable_registerid = p2.registerid;
                                iaw.income = decimal.Parse(Re.TransAmt);
                                iaw.expenditure = 0.00M;
                                iaw.time_of_occurrence = DateTime.Now;
                                iaw.account_balance = p2.available_balance + iaw.income;  //帐户余额
                                iaw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.借款人还款.ToString());
                                iaw.createtime = DateTime.Now;
                                iaw.keyid = 0;
                                iaw.remarks = Re.OrdId + "," + Re.OrdDate;
                                //判断是否是最后一期，如果是对本金处理。得直接用最后一期还款金额减去本金，这样就解决多出本金问题
                                bool lastrepamt = false;
                                if (list[0].current_period.ToString() == lastcur.ToString())
                                {
                                    lastrepamt = true;
                                }
                                //需要更新投资记录表已还款金额
                                B_usercenter BUC = new B_usercenter();
                                decimal PrincipalInterest = decimal.Parse(item.Principal.ToString()) + decimal.Parse(item.interestpayment.ToString());  //本金加利息
                                decimal PInterest = decimal.Parse(item.interestpayment.ToString());  //利息
                                Re.MerPriv = DateTime.Parse(item.interest_payment_date.ToString()).ToString("yyyy-MM-dd"); //传入还款日期
                                int bucd = BUC.Repayment_Successfully(Re, baw, iaw, lastrepamt, PrincipalInterest, PInterest);
                                if (bucd > 0)
                                {
                                    // 尊敬#UserName#,您投资的第#PID#号标第#ORDER#期还款已到帐,本次已成功还款#MONEY#元.欢迎继续投资!【创利投】

                                    #region 短信通知
                                    //短信通知

                                    string contxt = Utils.GetMSMEmailContext(11, 1); // 获取注册成功邮件内容

                                    StringBuilder sbsms = new StringBuilder(contxt);

                                    sbsms = sbsms.Replace("#USERANEM#", item.username.ToString());

                                    sbsms = sbsms.Replace("#PID#", item.targetid.ToString());

                                    sbsms = sbsms.Replace("#ORDER#", item.current_investment_period.ToString());

                                    sbsms = sbsms.Replace("#MONEY#", item.repayment_amount.ToString());


                                    string mobile = item.mobile.ToString();

                                    M_td_SMS_record psms = new M_td_SMS_record();
                                    B_td_SMS_record osms = new B_td_SMS_record();
                                    int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.投资回款.ToString());
                                    psms.phone_number = mobile;
                                    psms.sendtime = DateTime.Now;
                                    psms.senduserid = int.Parse(item.investor_registerid.ToString());
                                    psms.smstype = smstype;
                                    psms.smscontext = sbsms.ToString();
                                    psms.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());
                                    psms.vcode = "";

                                    osms.Add(psms);
                                    #endregion

                                    #region 系统消息
                                    //系统消息


                                    DateTime dti = DateTime.Now;

                                    M_td_System_message pm = new M_td_System_message();
                                    pm.MReg = int.Parse(item.investor_registerid.ToString());
                                    pm.Mstate = 0;
                                    pm.MTitle = "投资回款";
                                    //  pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功投资了项目【" + dt.Rows[0]["borrowing_title"].ToString() + "】，投资金额是：" + dt.Rows[0]["investment_amount"].ToString() + "。如有问题可咨询创利投的客服！谢谢！";
                                    pm.MContext = sbsms.ToString();
                                    pm.PubTime = dti;
                                    pm.Mtype = 2;
                                    B_usercenter.AddMessage(pm);


                                    #endregion

                                    #region 更新标的投标记录状态为还还清
                                    if (list[0].current_period.ToString() == lastcur.ToString())
                                    {
                                        //更新标的投标记录状态为还还清
                                        string sql = "update  hx_Bid_records  set  payment_status =1 where bid_records_id=" + item.bid_records_id.ToString();
                                        DbHelperSQL.ExecuteSql(sql);
                                    }
                                    #endregion

                                    //Response.Write("还款验签成功！");
                                }
                                else
                                {
                                    //Response.Write("还款更新失败！" + bucd.ToString());
                                }
                                sussname = sussname + 1;
                                //更新投资人帐户信息
                                //更新借款人帐户信息
                                //更新投资人还款计划列表
                            }
                        }
                        else
                        {
                            failname = failname + 1;
                            /*  
                            if (dt.Rows[0]["current_period"].ToString() == lastcur.ToString())
                          {
                              //更新标的状态为还还清
                              sql = "update hx_borrowing_target set tender_state=4 where targetid=" + dt.Rows[0]["targetid"].ToString() + "";
                              DbHelperSQL.ExecuteSql(sql);

                          }*/
                            // Response.Write("出现错误！ " + Utils.GetReturnCode(Int32.Parse(Re.RespCode)));
                            LogInfo.WriteLog("标的 " + hid_targetid + ";还款账号："+item.registerid+";汇付返回码："+Re.RespCode+"("+Re.RespDesc+")");
                        }
                    }
                    else
                    {
                        /*
                        if (dt.Rows[0]["current_period"].ToString() == lastcur.ToString())
                        {
                            //更新标的状态为还还清
                            sql = "update hx_borrowing_target set tender_state=4 where targetid=" + dt.Rows[0]["targetid"].ToString() + "";
                            DbHelperSQL.ExecuteSql(sql);

                        }
                        */
                        //Response.Write("验签失败！");
                    }
                    
                    //Response.Write("<br>" + sussname.ToString() + " 位客户还款成功  " + failname.ToString() + "  位客户还款失败 !<br>");
                    string tempStr = sussname.ToString() + " 笔投资还款成功  " + failname.ToString() + "  笔投资还款失败 !";
                    LogInfo.WriteLog("标的 " + hid_targetid + " 批量还款结束。"+ tempStr);
                    json = "{\"ret\":1,\"msg\":\"" + tempStr + "\"}";
                }
                list_dtrep = (from a in ef.V_borrowing_Bid_records_income_statement where a.targetid == hid_targetid && a.payment_status == 0 && ((DateTime)a.interest_payment_date).CompareTo(dtime) == 0 select a).ToList();
                var resutate = 0;
                var resutate2 = 0;
                if (list_dtrep == null || list_dtrep.Count == 0)
                {
                    #region 判断是否本次还款都已经还完，如果还完则更新借款人本次还款计划
                    resutate2 = ef.hx_repayment_plan.AsNoTracking().Where(a => a.repayment_plan_id == hid_planid).Update(a => new hx_repayment_plan { repayment_state = 1 });
                    #endregion

                    #region 如果是最后一期还款,更新标的状态为还清  ??判断是否有未还清bid_record?
                    if (list[0].current_period.ToString() == lastcur.ToString())
                    {
                        resutate = ef.hx_borrowing_target.AsNoTracking().Where(a => a.targetid == hid_targetid).Update(a => new hx_borrowing_target { tender_state = 5 });
                    }
                    #endregion
                }
                LogInfo.WriteLog("标的 " + hid_targetid + ";判断是否为最后一期还款:"+ lastcur.ToString() + " 投标状态修改返回：" + resutate+"; 还款计划更新返回："+ resutate2 + ";list_dtrep:" + list_dtrep.Count);
            }
            return Json(json);
        }


        #endregion

        #region 还款明细
        /// <summary>
        /// 还款明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Details(int id)
        {
            Utils.SetSYSDateTimeFormat();
            IEnumerable<V_borrow_repayment_plan> list = (from a in ef.V_borrow_repayment_plan where (a.tender_state == 4 || a.tender_state == 5) && a.targetid == id select a).OrderBy(a => a.current_period).ToList();

            return View(list);
        }
        #endregion

        #region 已经完成贷款
        /// <summary>
        /// 已经完成贷款
        /// </summary>
        /// <param name="borrowing_title"></param>
        /// <param name="realname"></param>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <param name="Page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult LoanCompleted(string borrowing_title = "", string realname = "", string time1 = "", string time2 = "", int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<V_borrowing_target_addlist, bool>> where = PredicateExtensionses.True<V_borrowing_target_addlist>();
            where = where.And(p => p.tender_state == 5);

            #region 条件
            if (!string.IsNullOrEmpty(borrowing_title))
            {
                where = where.And(a => a.borrowing_title.Contains(borrowing_title));
            }
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(a => a.realname == realname);
            }

            if (!string.IsNullOrEmpty(time1))
            {
                DateTime dt1 = Convert.ToDateTime(time1);
                where = where.And(a => ((DateTime)a.release_date).CompareTo(dt1) >= 0);
            }
            if (!string.IsNullOrEmpty(time2))
            {
                DateTime dt2 = Convert.ToDateTime(time2);
                dt2 = Convert.ToDateTime(dt2.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(a => ((DateTime)a.release_date).CompareTo(dt2) <= 0);
            }
            #endregion

            IPagedList<V_borrowing_target_addlist> list = ef.V_borrowing_target_addlist.Where(where).OrderByDescending(p => p.Repay_Time).ThenByDescending(p => p.targetid).ToPagedList(pageNumber, pageSize);

            ViewBag.borrowing_title = borrowing_title;
            ViewBag.realname = realname;
            ViewBag.time1 = time1;
            ViewBag.time2 = time2;

            return View(list);
        }

        #endregion

        #region 还款中贷款
        /// <summary>
        /// 还款中贷款
        /// </summary>
        /// <param name="borrowing_title"></param>
        /// <param name="realname"></param>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <param name="Page"></param>
        /// <param name="pageSize"></param>
        /// <param name="BT_OrderByFiled">排序参数</param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Repayment_loan(string borrowing_title = "", string realname = "", string time1 = "", string time2 = "", int Page = 1, int pageSize = 10,string hid_OrderByFiled = "")
        {
            int pageNumber = Page / 1;
            Expression<Func<V_borrowing_target_addlist, bool>> where = PredicateExtensionses.True<V_borrowing_target_addlist>();
            where = where.And(p => p.tender_state == 4);

            #region 条件
            if (!string.IsNullOrEmpty(borrowing_title))
            {
                where = where.And(a => a.borrowing_title.Contains(borrowing_title));
            }
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(a => a.realname == realname);
            }

            if (!string.IsNullOrEmpty(time1))
            {
                DateTime dt1 = Convert.ToDateTime(time1);
                where = where.And(a => ((DateTime)a.release_date).CompareTo(dt1) >= 0);
            }
            if (!string.IsNullOrEmpty(time2))
            {
                DateTime dt2 = Convert.ToDateTime(time2);
                dt2 = Convert.ToDateTime(dt2.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(a => ((DateTime)a.release_date).CompareTo(dt2) <= 0);
            }
            #endregion

            #region 页面排序字段
            var query = ef.V_borrowing_target_addlist.Where(where);
            if (!string.IsNullOrWhiteSpace(hid_OrderByFiled))
            {
                string[] vals = hid_OrderByFiled.Split(',');
                string filedFilter = vals[1];
                if (vals[0] == "asc")
                {//#end_time,#repayment_date,#Repay_Time
                    if (filedFilter == "end_time")
                    {
                        query = query.OrderBy(c => c.end_time);
                    }
                    else if(filedFilter == "repayment_date")
                    {
                        query = query.OrderBy(c=>c.repayment_date);
                    }else if (filedFilter == "Repay_Time")
                    {
                        query = query.OrderBy(c => c.Repay_Time);
                    }
                    else
                    {
                        query = query.OrderByDescending(p => p.end_time);
                    }
                }
                else
                {
                    if (filedFilter == "end_time")
                    {
                        query = query.OrderByDescending(c => c.end_time);
                    }
                    else if (filedFilter == "repayment_date")
                    {
                        query = query.OrderByDescending(c => c.repayment_date);
                    }
                    else if (filedFilter == "Repay_Time")
                    {
                        query = query.OrderByDescending(c => c.Repay_Time);
                    }
                    else
                    {
                        query = query.OrderByDescending(p => p.end_time);
                    }
                }
                ViewBag.FiledFilter = filedFilter;
                ViewBag.OrderB = vals[0];
                ViewBag.HidValOrderB = hid_OrderByFiled;
            }
            else
            {
                query = query.OrderByDescending(p => p.targetid);
            }
            #endregion

            IPagedList<V_borrowing_target_addlist> list = query.ToPagedList(pageNumber, pageSize);
            ViewBag.borrowing_title = borrowing_title;
            ViewBag.realname = realname;
            ViewBag.time1 = time1;
            ViewBag.time2 = time2;
            ViewBag.TotalItemCount = list.TotalItemCount;
            return View(list);
        }



        public ActionResult RechargeToExcel(string borrowing_title = "", string realname = "", string time1 = "", string time2 = "")
        {
            var userid = Utils.GetAdmUserID();
            var controllerName = this.RouteData.Values["controller"].ToString();
            var actionName = this.RouteData.Values["action"].ToString();
            if (!new UserLimitByEF().CheckAdminLimit(userid, controllerName, actionName))
            {   //无权限
                return Content("您没有操作权限");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT loan_number as '合同编号',borrowing_title as '标题',realname as '借款人',fundraising_amount as '借款金额'");
            sql.Append(",convert(varchar(10),release_date,120) as '借款日期',H_Repayment_Amt as '已还金额',life_of_loan as '期限/方式'");
            sql.Append(",convert(varchar(10),repayment_date,120) as '还款日期',case when ISNULL(Repay_Time,0)=0 then '还未还款' else  convert(varchar(10),Repay_Time,120) end as '最近还款时间' ,consultingAMT as '咨询费',guaranteeAMT as '担保费' ");
            sql.Append("FROM V_borrowing_target_addlist ");
            sql.Append("WHERE tender_state = 4 and targetid > 0 ");

            #region 查询条件
            if (!string.IsNullOrEmpty(borrowing_title))
            {
                sql.AppendFormat("AND borrowing_title like '%{0}%'", borrowing_title);
            }
            if (!string.IsNullOrEmpty(realname))
            {
                sql.AppendFormat("AND realname like '%{0}%'", realname);
            }

            if (!string.IsNullOrEmpty(time1))
            {
                DateTime df = DateTime.Parse(time1);

                sql.AppendFormat(" AND convert(varchar(10),release_date,120)>='{0}' ", df.ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(time2))
            {
                DateTime df = DateTime.Parse(time2);
                sql.AppendFormat(" AND convert(varchar(10),release_date,120)<='{0}' ", df.ToString("yyyy-MM-dd"));
            }
            #endregion

            sql.Append(" order by targetid desc;");

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql.ToString());

            var path = Extensions.ExportExcel(dt);

            return Content(path);
        }
        #endregion

        #region 三日内还款
        /// <summary>
        /// 三日内还款
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult ThreeDay()
        {
            //IEnumerable<V_borrow_repayment_plan> list = (from a in ef.V_borrow_repayment_plan
            //                                             group a by new { a.loan_number, a.targetid, a.borrowing_title, a.realname, a.payment_options, a.life_of_loan, a.unit_day, a.borrowing_balance, a.H_Repayment_Amt, a.repaymentperiods, a.tender_state, a.repayment_period } into b
            //                                             select new V_borrow_repayment_plan
            //                                             {
            //                                                 loan_number = b.Key.loan_number,
            //                                                 targetid = b.Key.targetid,
            //                                                 b.Count(),
            //                                                 borrowing_title = b.Key.borrowing_title,
            //                                                 payment_options = b.Key.payment_options,
            //                                                 life_of_loan = b.Key.life_of_loan,
            //                                                 unit_day = b.Key.unit_day,
            //                                                 realname = b.Key.realname,
            //                                                 borrowing_balance = b.Key.borrowing_balance,
            //                                                 H_Repayment_Amt = b.Key.H_Repayment_Amt,
            //                                                 repaymentperiods = b.Key.repaymentperiods,
            //                                                 tender_state = b.Key.tender_state,
            //                                                 repayment_period = b.Key.repayment_period
            //                                             });

            var start = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            var end = Convert.ToDateTime(DateTime.Now.AddDays(3).ToString("yyyy-MM-dd"));
            List<V_borrow_repayment_plan> list = (from a in ef.V_borrow_repayment_plan where a.tender_state == 4 && ((DateTime)a.repayment_period).CompareTo(start) >= 0 && ((DateTime)a.repayment_period).CompareTo(end) < 0 && (a.repayment_state == 0 || a.repayment_state == 2) select a).ToList();

            return View(list);
        }
        #endregion
    }
}