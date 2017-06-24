using ChuanglitouP2P.Common.Extensionses;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using EntityFramework.Extensions;
using PagedList;
using ChuanglitouP2P.Common;
using System.IO;
using System.Data;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.BLL;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using ChuanglitouP2P.Common.pdf;
using iTextSharp.text.html;
using ChuanglitouP2P.Model.chinapnr.Loans;
using System.Collections.Specialized;
using System.Net;
using ChuanglitouP2P.Model.chinapnr.Repayment;
using ChuanglitouP2P.Model.VeryCodes.NetCreditAssistant.Model;
using ChuanglitouP2P.Bll.VeryCodes.NetCreditAssistant.BLL;
using ChuanglitouP2P.BLL.Calculator;
using ChuanglitouP2P.Model.chinapnr.AddBidInfo;
using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using Org.BouncyCastle.Utilities.Encoders;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    /// <summary>
    /// 贷款审核
    /// </summary>
    public class ExamineController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/Examine
        public ActionResult Index()
        {
            return View();
        }

        #region 满标放款

        /// <summary>
        /// 满标放款
        /// </summary>
        /// <param name="targetid"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult lending(int targetid)
        {
            hx_borrowing_target target = (from a in ef.hx_borrowing_target where a.targetid == targetid select a).SingleOrDefault();
            IEnumerable<V_borrowing_target_review> list = (from a in ef.V_borrowing_target_review where a.targetid == targetid select a).OrderBy(a => a.reviewtime);
            var tarid = DNTRequest.GetInt("targetid", 0);
            ViewBag.targetid = tarid;
            ViewBag.borrowing_balance = RMB.GetWebConvertdisp((decimal)target.borrowing_balance, 2, true) + "元";
            ViewBag.consultingAMT = RMB.GetWebConvertdisp((decimal)target.fundraising_amount, 2, true) + "元";
            ViewBag.guaranteeAMT = RMB.GetWebConvertdisp((decimal)target.borrowing_balance - (decimal)target.fundraising_amount, 2, true) + "元";

            return View(list);
        }

        #endregion
        #region 放款数据处理
        /// <summary>
        /// 放款数据处理
        /// </summary>
        /// <param name="targetid"></param>
        /// <param name="tender_state"></param>
        /// <param name="reviewremarks"></param>
        /// <returns></returns>
        [AdminVaildate(false)]
        public ActionResult Dolending(string targetid, int tender_state, string reviewremarks)
        {
            string json = "{\"ret\":-1,\"msg\":\"操作失败\"}";
            reviewremarks = Utils.CheckSQLHtml(HttpUtility.UrlDecode(reviewremarks));
            int id = DNTRequest.GetInt("targetid", 0);
            string sql = "";
            B_reviewremarks o = new B_reviewremarks();
            M_reviewremarks p = new M_reviewremarks();

            sql = "update  hx_borrowing_target set  tender_state=" + tender_state.ToString() + " where targetid=" + id.ToString();

            if (DbHelperSQL.ExecuteSql(sql) > 0)
            {
                p.targetid = id;
                p.tender_state = tender_state;
                p.reviewremarks = reviewremarks;
                p.reviewtime = DateTime.Now;
                p.admin_operator = Utils.GetAdmUserID();

                if (o.Add(p) > 0)
                {
                    sql = "SELECT top 1 targetid,borrower_registerid,loan_number,borrowing_title,borrowing_thumbnail,annual_interest_rate,borrowing_balance,life_of_loan,unit_day,release_date,month_payment_date,repayment_date,minimum,maxmum,company_name,guarantee_way_name,fundraising_amount,tender_state,payment_options,username,realname,B_Rates,loan_management_fee,BorrUsrCustId from V_borrowing_target_addlist where tender_state=4 and targetid = " + id.ToString() + " order by  targetid desc";

                    //Response.Write(sql + "<br>");
                    DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

                    if (dt.Rows.Count > 0)
                    {
                        InvestmentParameters mp = new InvestmentParameters();
                        mp.Amount = decimal.Parse(dt.Rows[0]["borrowing_balance"].ToString());
                        mp.Circle = int.Parse(dt.Rows[0]["life_of_loan"].ToString());
                        mp.CircleType = int.Parse(dt.Rows[0]["unit_day"].ToString());
                        mp.NominalYearRate = double.Parse(dt.Rows[0]["B_Rates"].ToString());
                        mp.OverheadsRate = 0f;
                        mp.RepaymentMode = int.Parse(dt.Rows[0]["payment_options"].ToString());
                        mp.RewardRate = 0f;
                        mp.IsThirtyDayMonth = false;
                        //  mp.InvestDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

                        mp.InvestDate = DateTime.Parse(DateTime.Parse(dt.Rows[0]["release_date"].ToString()).ToString("yyyy-MM-dd"));

                        mp.Investmentenddate = DateTime.Parse(DateTime.Parse(dt.Rows[0]["repayment_date"].ToString()).ToString("yyyy-MM-dd"));
                        mp.Payinterest = int.Parse(dt.Rows[0]["month_payment_date"].ToString());
                        mp.InvestObject = 1;
                        List<InvestmentReceiveRecordInfo> records = Bll.VeryCodes.NetCreditAssistant.BLL.InvestCalculator.CalculateReceiveRecord(mp);
                        int i = 1;

                        decimal cou = 0.0M;

                        B_repayment_plan b1 = new B_repayment_plan();
                        M_repayment_plan m = new M_repayment_plan();

                        foreach (InvestmentReceiveRecordInfo pr in records)
                        {

                            m.borrower_registerid = int.Parse(dt.Rows[0]["borrower_registerid"].ToString());

                            m.BorrUsrCustId = dt.Rows[0]["BorrUsrCustId"].ToString();

                            m.targetid = int.Parse(dt.Rows[0]["targetid"].ToString());

                            m.current_period = i;

                            m.repayment_period = DateTime.Parse(pr.NominalReceiveDate.ToString("yyyy-MM-dd"));

                            m.repayment_type = 0;

                            m.repayment_amount = pr.Balance;
                            m.actual_amount_repayment = 0.00M;
                            m.repayment_state = 0;
                            m.createtime = DateTime.Now;
                            m.interestpayment = pr.Interest;

                            m.fees = Calculator.C_fees(decimal.Parse(dt.Rows[0]["loan_management_fee"].ToString()), m.repayment_amount);

                            if (b1.Exists(m.targetid, m.current_period, m.repayment_period, m.BorrUsrCustId))
                            {

                                if (b1.Add(m) <= 0)
                                {
                                    break;
                                }
                            }
                            i = i + 1;
                        }

                    }

                    //Response.Redirect("/admin/SubLoans.aspx?targetid=" + id.ToString());
                    json = SubLoans(id);


                    //  return Redirect("/admin/Examine/SubLoans?targetid="+id);

                }
                else
                {
                    json = "{\"ret\":0,\"msg\":\"审核操作失败\"}";

                }
            }

            ViewBag.json = json;

            return View();

            // return Content(json, "text/json");
        }
        #endregion



        public JsonResult NewDolending(string targetid, int tender_state, string reviewremarks)
        {
            string json = "{\"ret\":-1,\"msg\":\"操作失败\"}";
            reviewremarks = Utils.CheckSQLHtml(HttpUtility.UrlDecode(reviewremarks));
            int id = DNTRequest.GetInt("targetid", 0);
            string sql = "";
            B_reviewremarks o = new B_reviewremarks();
            M_reviewremarks p = new M_reviewremarks();

            sql = "update  hx_borrowing_target set end_time=GETDATE(),tender_state=" + tender_state.ToString() + " where targetid=" + id.ToString();

            if (DbHelperSQL.ExecuteSql(sql) > 0)
            {
                p.targetid = id;
                p.tender_state = tender_state;
                p.reviewremarks = reviewremarks;
                p.reviewtime = DateTime.Now;
                p.admin_operator = Utils.GetAdmUserID();

                if (o.Add(p) > 0)
                {
                    sql = "SELECT top 1 targetid,borrower_registerid,loan_number,borrowing_title,borrowing_thumbnail,annual_interest_rate,borrowing_balance,life_of_loan,unit_day,release_date,month_payment_date,repayment_date,minimum,maxmum,company_name,guarantee_way_name,fundraising_amount,tender_state,payment_options,username,realname,B_Rates,loan_management_fee,BorrUsrCustId from V_borrowing_target_addlist where tender_state=4 and targetid = " + id.ToString() + " order by  targetid desc";

                    //Response.Write(sql + "<br>");
                    DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

                    if (dt.Rows.Count > 0)
                    {
                        InvestmentParameters mp = new InvestmentParameters();
                        mp.Amount = decimal.Parse(dt.Rows[0]["borrowing_balance"].ToString());
                        mp.Circle = int.Parse(dt.Rows[0]["life_of_loan"].ToString());
                        mp.CircleType = int.Parse(dt.Rows[0]["unit_day"].ToString());
                        mp.NominalYearRate = double.Parse(dt.Rows[0]["B_Rates"].ToString());
                        mp.OverheadsRate = 0f;
                        mp.RepaymentMode = int.Parse(dt.Rows[0]["payment_options"].ToString());
                        mp.RewardRate = 0f;
                        mp.IsThirtyDayMonth = false;
                        //  mp.InvestDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

                        mp.InvestDate = DateTime.Parse(DateTime.Parse(dt.Rows[0]["release_date"].ToString()).ToString("yyyy-MM-dd"));

                        mp.Investmentenddate = DateTime.Parse(DateTime.Parse(dt.Rows[0]["repayment_date"].ToString()).ToString("yyyy-MM-dd"));
                        mp.Payinterest = int.Parse(dt.Rows[0]["month_payment_date"].ToString());
                        mp.InvestObject = 1;


                        #region 百日贷款  特殊处理
                        if (mp.Circle == 100 && mp.CircleType == 3)
                        {
                            int k = 0; //间隔月数
                            var falgInvestDate = mp.InvestDate;
                            while (falgInvestDate < mp.Investmentenddate)
                            {
                                falgInvestDate = falgInvestDate.AddMonths(1);
                                k++;
                            }
                            mp.CircleType = 1;
                            mp.Circle = k > 0 ? k - 1 : 0;
                        }
                        #endregion 

                        List<InvestmentReceiveRecordInfo> records = Bll.VeryCodes.NetCreditAssistant.BLL.InvestCalculator.CalculateReceiveRecord(mp);
                        int i = 1;

                        decimal cou = 0.0M;

                        B_repayment_plan b1 = new B_repayment_plan();
                        M_repayment_plan m = new M_repayment_plan();

                        foreach (InvestmentReceiveRecordInfo pr in records)
                        {

                            m.borrower_registerid = int.Parse(dt.Rows[0]["borrower_registerid"].ToString());

                            m.BorrUsrCustId = dt.Rows[0]["BorrUsrCustId"].ToString();

                            m.targetid = int.Parse(dt.Rows[0]["targetid"].ToString());

                            m.current_period = i;

                            m.repayment_period = DateTime.Parse(pr.NominalReceiveDate.ToString("yyyy-MM-dd"));

                            m.repayment_type = 0;

                            m.repayment_amount = pr.Balance;
                            m.actual_amount_repayment = 0.00M;
                            m.repayment_state = 0;
                            m.createtime = DateTime.Now;
                            m.interestpayment = pr.Interest;

                            m.fees = Calculator.C_fees(decimal.Parse(dt.Rows[0]["loan_management_fee"].ToString()), m.repayment_amount);

                            if (b1.Exists(m.targetid, m.current_period, m.repayment_period, m.BorrUsrCustId))
                            {

                                if (b1.Add(m) <= 0)
                                {
                                    break;
                                }
                            }
                            i = i + 1;
                        }

                    }

                    //Response.Redirect("/admin/SubLoans.aspx?targetid=" + id.ToString());
                    json = SubLoans(id);


                    //  return Redirect("/admin/Examine/SubLoans?targetid="+id);

                }
                else
                {
                    json = "{\"ret\":0,\"msg\":\"审核操作失败\"}";

                }
            }
            return Json(json);
        }


        #region 提交汇付放款接口
        /// <summary>
        /// 提交汇付放款接口
        /// </summary>
        /// <param name="targetid"></param>
        /// <returns></returns>
        /// 
        [AdminVaildate(false)]
        private string SubLoans(int targetid)
        {

            M_borrowing_target m = new M_borrowing_target();
            B_borrowing_target o = new B_borrowing_target();

            B_member_table o1 = new B_member_table();
            M_member_table p1 = new M_member_table();
            M_member_table p2 = new M_member_table();
            B_usercenter BUC = new B_usercenter();
            int susskefu = 0;
            int loftkefu = 0;
            m = o.GetModel(targetid);

            //service_charge 成交服务费

            string sql = "SELECT borrowing_title,bid_records_id,borrower_registerid,targetid,loan_number,annual_interest_rate,current_period,investment_amount,value_date,investment_maturity,invest_time,invest_state,flow_return,repayment_amount,repayment_period,investor_registerid,payment_status,withoutinterest,haveinterest,username,realname,registerid,payment_options,contractid,contractpath,life_of_loan,unit_day,mobile,UsrCustId,FrozenidAmount,FrozenidNo,FrozenState,FreezeTrxId,borrUsrCustid,available_balance,OrdId,BonusAmt FROM V_hx_Bid_records_borrowing_target where  tender_state=4 and  targetid=" + targetid.ToString();

            DataTable dt = new DataTable();
            dt = DbHelperSQL.GET_DataTable_List(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string strodid = Utils.Createcode();
                M_Loans Mt = new M_Loans();

                decimal investment_amount = decimal.Parse(dt.Rows[i]["investment_amount"].ToString());

                Mt.Version = "20";
                Mt.CmdId = "Loans";
                Mt.MerCustId = Utils.GetMerCustID();
                Mt.OrdId = strodid;
                Mt.OrdDate = DateTime.Now.ToString("yyyyMMdd");
                Mt.OutCustId = dt.Rows[i]["UsrCustId"].ToString(); //投资人帐户(出帐)
                Mt.TransAmt = investment_amount.ToString("0.00");
                //Mt.Fee = "0.00";
                Mt.SubOrdId = dt.Rows[i]["OrdId"].ToString(); //这位置需要订用新生成的订单号 新增一个字段
                Mt.SubOrdDate = DateTime.Parse(dt.Rows[i]["invest_time"].ToString()).ToString("yyyyMMdd");
                Mt.InCustId = dt.Rows[i]["borrUsrCustid"].ToString(); //借款客户号

                //担保费用
                decimal guaranteeAMT = 0;

                //  guaranteeAMT = RMB.GetDecimal(m.guaranteeAMT, 2, true);

                guaranteeAMT = investment_amount * m.guarantee_fee / 10;

                // guaranteeAMT = investment_amount * m.guarantee_fee;

                //咨询服务费
                decimal consultingAMT = 0;

                // consultingAMT = RMB.GetDecimal(m.consultingAMT, 2, true);

                consultingAMT = investment_amount * m.service_charge / 10;
                // consultingAMT = investment_amount * m.service_charge ;

                //手续费
                decimal loan_managementAmt = 0;
                loan_managementAmt = investment_amount * m.loan_management_fee / 10;

                // loan_managementAmt = investment_amount * m.loan_management_fee ;  //按百分比
                decimal fee1 = 0;
                fee1 = guaranteeAMT + consultingAMT + Math.Round(loan_managementAmt, 2);


                Mt.Fee = fee1.ToString("0.00");
                // Response.Write(Mt.Fee);

                M_DivDetails md = new M_DivDetails();
                md.DivCustId = Utils.GetMerCustID();
                md.DivAcctId = Utils.GetMERDT();
                md.DivAmt = Mt.Fee;

                if (Mt.Fee == "0.00")
                {
                    Mt.DivDetails = "";
                }
                else
                {
                    Mt.DivDetails = "[" + FastJSON.toJOSN(md) + "]";
                }

                Mt.FeeObjFlag = "I";

                Mt.IsDefault = "N";

                Mt.IsUnFreeze = "Y";

                // Mt.UnFreezeOrdId = dt.Rows[i]["FrozenidNo"].ToString();

                Mt.UnFreezeOrdId = Utils.Createcode();

                Mt.FreezeTrxId = dt.Rows[i]["FreezeTrxId"].ToString();
                Mt.BgRetUrl = Utils.GetRe_url("admin/Thirdparty/BgLoans");
                ///这位置需要调用代金券
                ///
                //string lvamt = BUC.GetLoansVocherAmt(dt.Rows[i]["investor_registerid"].ToString(), dt.Rows[i]["bid_records_id"].ToString());

                ReqExtLoans rel = new ReqExtLoans();

                string lvamt = "0.00";
                DataTable lvamtd = BUC.GetLoansVocherAmtDT(dt.Rows[i]["investor_registerid"].ToString(), dt.Rows[i]["bid_records_id"].ToString());
                if (lvamtd.Rows.Count > 0)
                {
                    lvamt = decimal.Parse(lvamtd.Rows[0]["amount_of_reward"].ToString()).ToString("0.00");
                    if (lvamt != "0.00")
                    {
                        //此处需要判断抵扣券或加息券，注意处理
                        if (int.Parse(lvamtd.Rows[0]["RewTypeID"].ToString()) == 2)
                        {
                            rel.LoansVocherAmt = lvamt;
                        }
                    }
                }
                //暂未启用  增加放款时红包金额获取逻辑需要修改为投标记录中的bonus_amt,避免因红包记录错误导致放款失败
                //lvamt = decimal.Parse(dt.Rows[i]["BonusAmt"].ToString()).ToString("0.00");
                //if (lvamt != "0.00")
                //{
                //    rel.LoansVocherAmt = lvamt;
                //}
                //标的id,借款人id,投资人id,投资记录bid,抵扣券金额

                string Merpriv = targetid.ToString() + "_" + dt.Rows[i]["borrower_registerid"].ToString() + "_" + dt.Rows[i]["investor_registerid"].ToString() + "_" + dt.Rows[i]["bid_records_id"].ToString() + "_" + lvamt;
                Mt.MerPriv = Merpriv;
                rel.ProId = dt.Rows[i]["targetid"].ToString();

                Mt.ReqExt = FastJSON.toJOSN(rel);

                StringBuilder chkVal = new StringBuilder();
                chkVal.Append(Mt.Version);
                chkVal.Append(Mt.CmdId);
                chkVal.Append(Mt.MerCustId);
                chkVal.Append(Mt.OrdId);
                chkVal.Append(Mt.OrdDate);
                chkVal.Append(Mt.OutCustId);
                chkVal.Append(Mt.TransAmt);
                chkVal.Append(Mt.Fee);
                chkVal.Append(Mt.SubOrdId);
                chkVal.Append(Mt.SubOrdDate);
                chkVal.Append(Mt.InCustId);
                chkVal.Append(Mt.DivDetails);
                chkVal.Append(Mt.FeeObjFlag);
                chkVal.Append(Mt.IsDefault);
                chkVal.Append(Mt.IsUnFreeze);
                chkVal.Append(Mt.UnFreezeOrdId);
                chkVal.Append(Mt.FreezeTrxId);
                chkVal.Append(Mt.BgRetUrl);
                chkVal.Append(Mt.MerPriv);
                chkVal.Append(Mt.ReqExt);
                string chkv = chkVal.ToString();

                LogInfo.WriteLog("加签chkv字符:" + chkv);

                //私钥文件的位置(这里是放在了站点的根目录下)
                string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
                //需要指定提交字符串的长度
                int len = Encoding.UTF8.GetBytes(chkv).Length;
                StringBuilder sbChkValue = new StringBuilder(256);
                //加签
                int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

                LogInfo.WriteLog("加签字符:" + str.ToString());

                Mt.ChkValue = sbChkValue.ToString();

                LogInfo.WriteLog("提交信息：" + FastJSON.toJOSN(Mt));
                LogInfo.WriteLog("ChkValue:" + Mt.ChkValue);

                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values.Add("Version", Mt.Version);
                    values.Add("CmdId", Mt.CmdId);
                    values.Add("MerCustId", Mt.MerCustId);
                    values.Add("OrdId", Mt.OrdId);
                    values.Add("OrdDate", Mt.OrdDate);
                    values.Add("OutCustId", Mt.OutCustId);
                    values.Add("TransAmt", Mt.TransAmt);
                    values.Add("Fee", Mt.Fee);
                    values.Add("SubOrdId", Mt.SubOrdId);
                    values.Add("SubOrdDate", Mt.SubOrdDate);
                    values.Add("InCustId", Mt.InCustId);
                    values.Add("DivDetails", Mt.DivDetails);
                    values.Add("FeeObjFlag", Mt.FeeObjFlag);
                    values.Add("IsDefault", Mt.IsDefault);
                    values.Add("IsUnFreeze", Mt.IsUnFreeze);
                    values.Add("UnFreezeOrdId", Mt.UnFreezeOrdId);
                    values.Add("FreezeTrxId", Mt.FreezeTrxId);
                    values.Add("BgRetUrl", Mt.BgRetUrl);
                    values.Add("MerPriv", Mt.MerPriv);
                    values.Add("ReqExt", Mt.ReqExt);
                    values.Add("ChkValue", Mt.ChkValue);
                    string url = Utils.GetChinapnrUrl();
                    //同步发送form表单请求
                    byte[] result = client.UploadValues(url, "POST", values);
                    var retStr = Encoding.UTF8.GetString(result);


                    //Response.Write(retStr);

                    LogInfo.WriteLog(retStr);

                    ReLoans retLoan = new ReLoans();
                    var retloan = (ReLoans)FastJSON.ToObject(retStr, retLoan);
                    LogInfo.WriteLog("放款返回报文：" + FastJSON.toJOSN(retloan));

                    StringBuilder builder = new StringBuilder();
                    builder.Append(retloan.CmdId);
                    builder.Append(retloan.RespCode);

                    builder.Append(retloan.MerCustId);
                    builder.Append(retloan.OrdId);
                    builder.Append(retloan.OrdDate);
                    builder.Append(retloan.OutCustId);
                    builder.Append(retloan.OutAcctId);
                    builder.Append(retloan.TransAmt);
                    builder.Append(retloan.Fee);
                    builder.Append(retloan.InCustId);
                    builder.Append(retloan.InAcctId);
                    builder.Append(retloan.SubOrdId);
                    builder.Append(retloan.SubOrdDate);
                    builder.Append(retloan.FeeObjFlag);
                    builder.Append(retloan.IsDefault);
                    builder.Append(retloan.IsUnFreeze);
                    builder.Append(retloan.UnFreezeOrdId);
                    builder.Append(retloan.FreezeTrxId);
                    builder.Append(HttpUtility.UrlDecode(retloan.BgRetUrl));
                    builder.Append(retloan.MerPriv);
                    builder.Append(HttpUtility.UrlDecode(retloan.RespExt));
                    //builder.Append(retloan.ChkValue);

                    var msg = builder.ToString();
                    LogInfo.WriteLog("返回参数:" + msg);
                    //验签
                    string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
                    int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, retloan.ChkValue);

                    LogInfo.WriteLog("验签ret:" + ret.ToString());

                    if (ret == 0)
                    {
                        if (retloan.RespCode == "000")
                        {
                            string cachename = retloan.OrdId + "Loans" + retloan.FreezeTrxId;

                            if (Utils.GeTThirdCache(cachename) == 0)
                            {
                                Utils.SetThirdCache(cachename);
                                susskefu = susskefu + 1;
                                M_Capital_account_water baw = new M_Capital_account_water(); //借款人流水对象
                                p1 = o1.GetModel(int.Parse(dt.Rows[i]["borrower_registerid"].ToString())); //借款人用户对象
                                LogInfo.WriteLog("dt.Rows[i][borrower_registerid].ToString():" + dt.Rows[i]["borrower_registerid"].ToString());
                                baw.membertable_registerid = p1.registerid;
                                baw.income = decimal.Parse(retloan.TransAmt);
                                baw.expenditure = 0.00M;

                                baw.time_of_occurrence = DateTime.Now;
                                // decimal ff = p1.available_balance + decimal.Parse(retloan.TransAmt);
                                baw.account_balance = p1.available_balance;  //面要得么帐户余额

                                LogInfo.WriteLog("借款人余额:" + p1.available_balance);
                                baw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.借款转入.ToString());
                                baw.createtime = DateTime.Now;
                                baw.keyid = 0;
                                baw.remarks = retloan.OrdId + "," + retloan.OrdDate;

                                M_Capital_account_water iaw = new M_Capital_account_water(); //投资人流水对象
                                p2 = o1.GetModel(int.Parse(dt.Rows[i]["investor_registerid"].ToString())); //投款人用户对象
                                iaw.membertable_registerid = p2.registerid;
                                iaw.income = 0.00M;
                                iaw.expenditure = decimal.Parse(retloan.TransAmt);
                                iaw.time_of_occurrence = DateTime.Now;
                                // decimal df=p1.available_balance - decimal.Parse(retloan.TransAmt);
                                iaw.account_balance = p2.available_balance;  //面要得么帐户余额
                                LogInfo.WriteLog("投资人余额:" + p2.ToString());

                                iaw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.项目投资.ToString());
                                iaw.createtime = DateTime.Now;
                                iaw.keyid = 0;
                                iaw.remarks = retloan.OrdId + "," + retloan.OrdDate;

                                LogInfo.WriteLog("放款bid_records_id:" + dt.Rows[i]["bid_records_id"].ToString());

                                if (BUC.Loan_Successfully(retloan, baw, iaw, dt.Rows[i]["bid_records_id"].ToString(), decimal.Parse(lvamt)) > 0)
                                {
                                    //成功
                                    //  Response.Redirect("/usercenter/index.html");
                                }
                            }
                        }
                        else
                        {
                            loftkefu = loftkefu + 1;
                        }
                    }
                    else
                    {
                        sql = "update  hx_borrowing_target set  tender_state=3 where targetid=" + targetid.ToString();
                        DbHelperSQL.ExecuteSql(sql);
                        //Response.Write("验签失败！");
                        return "{\"ret\":0,\"msg\":\"验签失败\"}";
                    }
                }
            }


            //Response.Write(susskefu.ToString() + "位客户放款成功!" + loftkefu.ToString() + "位客户放款失败!");
            string tempStr = susskefu.ToString() + "笔投资放款成功!" + loftkefu.ToString() + "笔投资放款失败!";
            return "{\"ret\":1,\"msg\":\"" + tempStr + "\"}";


            // return View();
        }
        #endregion



        #region 满标

        /// <summary>
        /// 满标
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Fullscale(int id, string action = "")
        {
            hx_borrowing_target target = (from a in ef.hx_borrowing_target where a.targetid == id select a).SingleOrDefault();
            IEnumerable<V_borrowing_target_review> list = (from a in ef.V_borrowing_target_review where a.targetid == id select a).OrderBy(a => a.reviewtime);
            var consamt = GetconsultingAMT(id);

            ViewBag.target = target;
            ViewBag.consultingAMT = consamt;
            ViewBag.guaranteeAMT = RMB.GetWebConvertdisp((decimal)target.borrowing_balance - consamt, 2, true);

            return View(list);
        }






        /// <summary>
        /// 统计投标记录，可用标金额不含未成功投标
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private decimal GetconsultingAMT(int id)
        {

            string sql = "select COALESCE(sum(investment_amount),0.00) as 有效投资金额   from  hx_Bid_records where  ordstate=1 and targetid=" + id;


            return decimal.Parse(DbHelperSQL.Re_String(sql));

        }

        public ActionResult DoFullscale(int id, int tender_state, string reviewremarks)
        {
            B_reviewremarks o = new B_reviewremarks();
            M_reviewremarks p = new M_reviewremarks();
            string json = "{\"ret\":-1,\"msg\":\"操作失败\"}";
            reviewremarks = Utils.CheckSQLHtml(HttpUtility.UrlDecode(reviewremarks));
            /*
             注意如果流标的话需要能过接口进行流标操作
             */

            string sql = "";
            if (tender_state == 3) //满标更改状态
            {
                sql = "update  hx_borrowing_target set  tender_state=" + tender_state.ToString() + " where targetid=" + id.ToString();

                if (DbHelperSQL.ExecuteSql(sql) > 0)
                {
                    p.targetid = id;
                    p.tender_state = tender_state;
                    p.reviewremarks = reviewremarks;
                    p.reviewtime = DateTime.Now;
                    p.admin_operator = Utils.GetAdmUserID();

                    if (o.Add(p) > 0)
                    {
                        // Response.Redirect("Fullscale.aspx?id=" + id.ToString());
                        json = "{\"ret\":1,\"msg\":\"操作成功\"}";

                    }
                    else
                    {
                        json = "{\"ret\":0,\"msg\":\"审核操作失败\"}";
                    }
                }
            }
            else if (tender_state == 8) //流标通过接口处理付款事务
            {

                /*

                M_TenderCancle mt = new M_TenderCancle();

                mt.Version = "20";
                mt.CmdId = "TenderCancle";
                mt.MerCustId = Utils.GetMerCustID();

                */


                sql = "select  investment_amount,UsrCustId,FrozenidNo,FreezeTrxId from V_hx_Bid_records_borrowing_target where  targetid=" + id.ToString() + "  and tender_state=2";

                DataTable dt = new DataTable();

                if (dt.Rows.Count > 0)
                {

                    /*
                    for (int i = 0; i < dt.Rows.Count; i++)
                    { 
                    
                    }
                     */

                    //Response.Write("还有记录没有撒销不能流标!");
                    json = "{\"ret\":0,\"msg\":\"还有记录没有撒销不能流标\"}";
                }
                else
                {
                    //Response.Write("没有投标记录直接更改标的状态!");
                    json = "{\"ret\":0,\"msg\":\"没有投标记录直接更改标的状态\"}";

                }
            }
            return Content(json, "text/json");
        }


        public JsonResult NewDoFullscale(int id, int tender_state, string reviewremarks)
        {
            B_reviewremarks o = new B_reviewremarks();
            M_reviewremarks p = new M_reviewremarks();
            string json = "{\"ret\":-1,\"msg\":\"操作失败\"}";
            reviewremarks = Utils.CheckSQLHtml(HttpUtility.UrlDecode(reviewremarks));
            /*
             注意如果流标的话需要能过接口进行流标操作
             */

            string sql = "";
            if (tender_state == 3) //满标更改状态
            {
                sql = "update  hx_borrowing_target set  tender_state=" + tender_state.ToString() + " where targetid=" + id.ToString();

                if (DbHelperSQL.ExecuteSql(sql) > 0)
                {
                    p.targetid = id;
                    p.tender_state = tender_state;
                    p.reviewremarks = reviewremarks;
                    p.reviewtime = DateTime.Now;
                    p.admin_operator = Utils.GetAdmUserID();

                    if (o.Add(p) > 0)
                    {
                        // Response.Redirect("Fullscale.aspx?id=" + id.ToString());
                        json = "{\"ret\":1,\"msg\":\"操作成功\"}";

                    }
                    else
                    {
                        json = "{\"ret\":0,\"msg\":\"审核操作失败\"}";
                    }
                }
            }
            else if (tender_state == 8) //流标通过接口处理付款事务
            {

                /*

                M_TenderCancle mt = new M_TenderCancle();

                mt.Version = "20";
                mt.CmdId = "TenderCancle";
                mt.MerCustId = Utils.GetMerCustID();

                */


                sql = "select  investment_amount,UsrCustId,FrozenidNo,FreezeTrxId from V_hx_Bid_records_borrowing_target where  targetid=" + id.ToString() + "  and tender_state=2";

                DataTable dt = new DataTable();

                if (dt.Rows.Count > 0)
                {

                    /*
                    for (int i = 0; i < dt.Rows.Count; i++)
                    { 
                    
                    }
                     */

                    //Response.Write("还有记录没有撒销不能流标!");
                    json = "{\"ret\":0,\"msg\":\"还有记录没有撒销不能流标\"}";
                }
                else
                {
                    //Response.Write("没有投标记录直接更改标的状态!");
                    json = "{\"ret\":0,\"msg\":\"没有投标记录直接更改标的状态\"}";

                }
            }
            return Json(json);
        }

        #endregion

        /// <summary>
        /// 投资记录
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult InvesRecord(int targetid)
        {
            IEnumerable<V_hx_Bid_records_borrowing_target> list = (from a in ef.V_hx_Bid_records_borrowing_target where a.targetid == targetid && a.ordstate == 1 select a).OrderBy(a => a.invest_time).ToList();

            return View(list);
        }

        /// <summary>
        /// 放款
        /// </summary>
        /// <param name="bid"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult SetLoans(string bid, int tid)
        {
            M_borrowing_target m = new M_borrowing_target();
            B_borrowing_target o = new B_borrowing_target();
            B_member_table o1 = new B_member_table();
            M_member_table p1 = new M_member_table();
            M_member_table p2 = new M_member_table();
            B_usercenter BUC = new B_usercenter();

            int targetid = DNTRequest.GetInt("tid", 0);

            m = o.GetModel(targetid);

            string sql = "SELECT borrowing_title,bid_records_id,borrower_registerid,targetid,loan_number,annual_interest_rate,current_period,investment_amount,value_date,investment_maturity,invest_time,invest_state,flow_return,repayment_amount,repayment_period,investor_registerid,payment_status,withoutinterest,haveinterest,username,realname,registerid,payment_options,contractid,contractpath,life_of_loan,unit_day,mobile,UsrCustId,FrozenidAmount,FrozenidNo,FrozenState,FreezeTrxId,borrUsrCustid,available_balance,OrdId FROM V_hx_Bid_records_borrowing_target where   tender_state=4  and ordstate=1 and  IsLoans=0  and  targetid=" + targetid.ToString() + " and  bid_records_id=" + bid.ToString();

            DataTable dt = new DataTable();
            dt = DbHelperSQL.GET_DataTable_List(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //Response.Write(dt.Rows[i]["bid_records_id"].ToString());


                string strodid = Utils.Createcode();

                M_Loans Mt = new M_Loans();

                decimal investment_amount = decimal.Parse(dt.Rows[i]["investment_amount"].ToString());

                Mt.Version = "20";
                Mt.CmdId = "Loans";
                Mt.MerCustId = Utils.GetMerCustID();
                Mt.OrdId = strodid;
                Mt.OrdDate = DateTime.Now.ToString("yyyyMMdd");
                Mt.OutCustId = dt.Rows[i]["UsrCustId"].ToString(); //投资人帐户(出帐)
                Mt.TransAmt = investment_amount.ToString("0.00");
                //Mt.Fee = "0.00";
                Mt.SubOrdId = dt.Rows[i]["OrdId"].ToString(); //这位置需要订用新生成的订单号 新增一个字段
                Mt.SubOrdDate = DateTime.Parse(dt.Rows[i]["invest_time"].ToString()).ToString("yyyyMMdd");
                Mt.InCustId = dt.Rows[i]["borrUsrCustid"].ToString(); //借款客户号

                //担保费用
                decimal guaranteeAMT = 0;

                //  guaranteeAMT = RMB.GetDecimal(m.guaranteeAMT, 2, true);

                guaranteeAMT = investment_amount * m.guarantee_fee / 10;

                // guaranteeAMT = investment_amount * m.guarantee_fee;

                //咨询服务费
                decimal consultingAMT = 0;

                // consultingAMT = RMB.GetDecimal(m.consultingAMT, 2, true);

                consultingAMT = investment_amount * m.service_charge / 10;
                // consultingAMT = investment_amount * m.service_charge ;

                //手续费
                decimal loan_managementAmt = 0;
                loan_managementAmt = investment_amount * m.loan_management_fee / 10;

                // loan_managementAmt = investment_amount * m.loan_management_fee ;  //按百分比
                decimal fee1 = 0;
                fee1 = guaranteeAMT + consultingAMT + Math.Round(loan_managementAmt, 2);


                Mt.Fee = fee1.ToString("0.00");


                // Response.Write(Mt.Fee);

                M_DivDetails md = new M_DivDetails();
                md.DivCustId = Utils.GetMerCustID();
                md.DivAcctId = Utils.GetMERDT();
                md.DivAmt = Mt.Fee;

                if (Mt.Fee == "0.00")
                {
                    Mt.DivDetails = "";
                }
                else
                {
                    Mt.DivDetails = "[" + FastJSON.toJOSN(md) + "]";
                }

                Mt.FeeObjFlag = "I";

                Mt.IsDefault = "N";

                Mt.IsUnFreeze = "Y";

                // Mt.UnFreezeOrdId = dt.Rows[i]["FrozenidNo"].ToString();

                Mt.UnFreezeOrdId = Utils.Createcode();

                Mt.FreezeTrxId = dt.Rows[i]["FreezeTrxId"].ToString();

                Mt.BgRetUrl = Utils.GetRe_url("admin/Thirdparty/BgLoans.aspx");

                Mt.MerPriv = "chuanglitou";


                ///这位置需要调用代金券
                ///


                string lvamt = BUC.GetLoansVocherAmt(dt.Rows[i]["investor_registerid"].ToString(), dt.Rows[i]["bid_records_id"].ToString());
                ReqExtLoans rel = new ReqExtLoans();
                if (lvamt == "0.00")
                { }
                else
                {
                    rel.LoansVocherAmt = lvamt;

                }

                rel.ProId = dt.Rows[i]["targetid"].ToString();

                Mt.ReqExt = FastJSON.toJOSN(rel);

                StringBuilder chkVal = new StringBuilder();
                chkVal.Append(Mt.Version);
                chkVal.Append(Mt.CmdId);
                chkVal.Append(Mt.MerCustId);
                chkVal.Append(Mt.OrdId);
                chkVal.Append(Mt.OrdDate);
                chkVal.Append(Mt.OutCustId);
                chkVal.Append(Mt.TransAmt);
                chkVal.Append(Mt.Fee);
                chkVal.Append(Mt.SubOrdId);
                chkVal.Append(Mt.SubOrdDate);
                chkVal.Append(Mt.InCustId);
                chkVal.Append(Mt.DivDetails);
                chkVal.Append(Mt.FeeObjFlag);
                chkVal.Append(Mt.IsDefault);
                chkVal.Append(Mt.IsUnFreeze);
                chkVal.Append(Mt.UnFreezeOrdId);
                chkVal.Append(Mt.FreezeTrxId);
                chkVal.Append(Mt.BgRetUrl);
                chkVal.Append(Mt.MerPriv);
                chkVal.Append(Mt.ReqExt);
                string chkv = chkVal.ToString();

                LogInfo.WriteLog("单个放款加签chkv字符:" + chkv);

                //私钥文件的位置(这里是放在了站点的根目录下)
                string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
                //需要指定提交字符串的长度
                int len = Encoding.UTF8.GetBytes(chkv).Length;
                StringBuilder sbChkValue = new StringBuilder(256);
                //加签
                int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

                LogInfo.WriteLog("单个放款加签字符:" + str.ToString());

                Mt.ChkValue = sbChkValue.ToString();

                LogInfo.WriteLog("单个放款提交信息：" + FastJSON.toJOSN(Mt));
                LogInfo.WriteLog("单个放款ChkValue:" + Mt.ChkValue);

                using (var client = new WebClient())
                {

                    var values = new NameValueCollection();
                    values.Add("Version", Mt.Version);
                    values.Add("CmdId", Mt.CmdId);
                    values.Add("MerCustId", Mt.MerCustId);
                    values.Add("OrdId", Mt.OrdId);
                    values.Add("OrdDate", Mt.OrdDate);
                    values.Add("OutCustId", Mt.OutCustId);
                    values.Add("TransAmt", Mt.TransAmt);
                    values.Add("Fee", Mt.Fee);
                    values.Add("SubOrdId", Mt.SubOrdId);
                    values.Add("SubOrdDate", Mt.SubOrdDate);
                    values.Add("InCustId", Mt.InCustId);
                    values.Add("DivDetails", Mt.DivDetails);
                    values.Add("FeeObjFlag", Mt.FeeObjFlag);
                    values.Add("IsDefault", Mt.IsDefault);
                    values.Add("IsUnFreeze", Mt.IsUnFreeze);
                    values.Add("UnFreezeOrdId", Mt.UnFreezeOrdId);
                    values.Add("FreezeTrxId", Mt.FreezeTrxId);
                    values.Add("BgRetUrl", Mt.BgRetUrl);
                    values.Add("MerPriv", Mt.MerPriv);
                    values.Add("ReqExt", Mt.ReqExt);
                    values.Add("ChkValue", Mt.ChkValue);
                    string url = Utils.GetChinapnrUrl();
                    //同步发送form表单请求
                    byte[] result = client.UploadValues(url, "POST", values);
                    var retStr = Encoding.UTF8.GetString(result);


                    //Response.Write(retStr);

                    LogInfo.WriteLog(retStr);

                    ReLoans retLoan = new ReLoans();
                    var retloan = (ReLoans)FastJSON.ToObject(retStr, retLoan);
                    LogInfo.WriteLog("单个放款返回报文：" + FastJSON.toJOSN(retloan));

                    StringBuilder builder = new StringBuilder();
                    builder.Append(retloan.CmdId);
                    builder.Append(retloan.RespCode);

                    builder.Append(retloan.MerCustId);
                    builder.Append(retloan.OrdId);
                    builder.Append(retloan.OrdDate);
                    builder.Append(retloan.OutCustId);
                    builder.Append(retloan.OutAcctId);
                    builder.Append(retloan.TransAmt);
                    builder.Append(retloan.Fee);
                    builder.Append(retloan.InCustId);
                    builder.Append(retloan.InAcctId);
                    builder.Append(retloan.SubOrdId);
                    builder.Append(retloan.SubOrdDate);
                    builder.Append(retloan.FeeObjFlag);
                    builder.Append(retloan.IsDefault);
                    builder.Append(retloan.IsUnFreeze);
                    builder.Append(retloan.UnFreezeOrdId);
                    builder.Append(retloan.FreezeTrxId);
                    builder.Append(HttpUtility.UrlDecode(retloan.BgRetUrl));
                    builder.Append(retloan.MerPriv);
                    builder.Append(HttpUtility.UrlDecode(retloan.RespExt));
                    //builder.Append(retloan.ChkValue);

                    var msg = builder.ToString();

                    LogInfo.WriteLog("单个放款返回参数:" + msg);
                    //验签
                    string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
                    int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, retloan.ChkValue);

                    LogInfo.WriteLog("单个放款验签ret:" + ret.ToString());


                    if (ret == 0)
                    {
                        if (retloan.RespCode == "000")
                        {

                            M_Capital_account_water baw = new M_Capital_account_water(); //借款人流水对象

                            p1 = o1.GetModel(int.Parse(dt.Rows[i]["borrower_registerid"].ToString())); //借款人用户对象

                            LogInfo.WriteLog("testtdt.Rows[i][borrower_registerid].ToString():" + dt.Rows[i]["borrower_registerid"].ToString());
                            baw.membertable_registerid = p1.registerid;
                            baw.income = decimal.Parse(retloan.TransAmt);
                            baw.expenditure = 0.00M;

                            baw.time_of_occurrence = DateTime.Now;

                            // decimal ff = p1.available_balance + decimal.Parse(retloan.TransAmt);
                            baw.account_balance = p1.available_balance;  //面要得么帐户余额

                            LogInfo.WriteLog("单个放款借款人余额:" + p1.available_balance);

                            baw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.借款转入.ToString());
                            baw.createtime = DateTime.Now;
                            baw.keyid = 0;
                            baw.remarks = retloan.OrdId + "," + retloan.OrdDate;

                            M_Capital_account_water iaw = new M_Capital_account_water(); //投资人流水对象


                            p2 = o1.GetModel(int.Parse(dt.Rows[i]["investor_registerid"].ToString())); //投款人用户对象

                            iaw.membertable_registerid = p2.registerid;
                            iaw.income = 0.00M;
                            iaw.expenditure = decimal.Parse(retloan.TransAmt);
                            iaw.time_of_occurrence = DateTime.Now;
                            // decimal df=p1.available_balance - decimal.Parse(retloan.TransAmt);
                            iaw.account_balance = p2.available_balance;  //面要得么帐户余额
                            LogInfo.WriteLog("单个放款投资人余额:" + p2.ToString());

                            iaw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.项目投资.ToString());
                            iaw.createtime = DateTime.Now;
                            iaw.keyid = 0;
                            iaw.remarks = retloan.OrdId + "," + retloan.OrdDate;






                            LogInfo.WriteLog("单个放款放款bid_records_id:" + dt.Rows[i]["bid_records_id"].ToString());


                            if (BUC.Loan_Successfully(retloan, baw, iaw, dt.Rows[i]["bid_records_id"].ToString(), decimal.Parse(lvamt)) > 0)
                            {
                                //成功


                                return Content("放款成功");

                                //  Response.Redirect("/usercenter/index.html");

                            }

                        }
                        else
                        {
                            return Content("放款失败: " + HttpUtility.UrlDecode(retloan.RespDesc));
                        }
                    }
                    else
                    {
                        return Content("非法操作，验签失败!");
                    }
                }
            }
            return Content("操作失败");

        }

        #region 招标中贷款
        /// <summary>
        /// 招标中贷款
        /// </summary>
        /// <param name="borrowing_title"></param>
        /// <param name="realname"></param>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <param name="Page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult bidding(string borrowing_title = "", string realname = "", string time1 = "", string time2 = "", int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<V_borrowing_target_addlist, bool>> where = PredicateExtensionses.True<V_borrowing_target_addlist>();
            where = where.And(a => (a.tender_state == 2 || a.tender_state == 3));

            #region 条件
            if (!string.IsNullOrEmpty(borrowing_title))
            {
                where = where.And(a => a.borrowing_title.Contains(borrowing_title));
            }
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(a => a.realname.Contains(realname));
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

            IPagedList<V_borrowing_target_addlist> list = ef.V_borrowing_target_addlist.Where(where).OrderByDescending(p => p.targetid).ToPagedList(pageNumber, pageSize);


            ViewBag.borrowing_title = borrowing_title;
            ViewBag.realname = realname;
            ViewBag.time1 = time1;
            ViewBag.time2 = time2;

            return View(list);
        }

        #endregion

        #region 待初审贷款

        /// <summary>
        /// 初审
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action">look:只读</param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Submitwaitverify(int id, string action = "")
        {
            hx_borrowing_target target = (from a in ef.hx_borrowing_target where a.targetid == id select a).SingleOrDefault();



            IEnumerable<V_borrowing_target_review> list = (from a in ef.V_borrowing_target_review where a.targetid == id select a).OrderBy(a => a.reviewtime).ToList();



            ViewBag.id = id;
            ViewBag.action = action;
            ViewBag.target = target;
            ViewBag.consultingAMT = ((decimal)target.borrowing_balance * (decimal)target.service_charge / 10).ToString("0.00");
            ViewBag.guaranteeAMT = ((decimal)target.borrowing_balance * (decimal)target.guarantee_fee).ToString("0.00");

            return View(list);
        }

        /// <summary>
        /// 初审操作
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tender_state"></param>
        /// <param name="reviewremarks"></param>
        /// <returns></returns>
        [AdminVaildate(false)]
        public ActionResult DoSubmitwaitverify(int id, int tender_state, string reviewremarks, decimal consultingAMT, decimal guaranteeAMT)
        {
            reviewremarks = Utils.CheckSQLHtml(HttpUtility.UrlDecode(reviewremarks));
            var json = "{\"ret\":-1,\"msg\":\"参数错误\"}";
            if (id > 0 && (tender_state == 6 || tender_state == 1) && !string.IsNullOrEmpty(reviewremarks))
            {
                var result = ef.hx_borrowing_target.Where(a => a.targetid == id).Update(a => new hx_borrowing_target { tender_state = tender_state, consultingAMT = consultingAMT, guaranteeAMT = guaranteeAMT });

                if (result > 0)
                {
                    hx_td_reviewremarks model = new hx_td_reviewremarks();
                    model.targetid = id;
                    model.tender_state = tender_state;
                    model.reviewremarks = reviewremarks;
                    model.reviewtime = DateTime.Now;
                    model.admin_operator = Utils.GetAdmUserID();

                    ef.hx_td_reviewremarks.Add(model);
                    if (ef.SaveChanges() > 0)
                    {
                        json = "{\"ret\":1,\"msg\":\"操作成功\"}";
                    }
                    else
                    {
                        json = "{\"ret\":0,\"msg\":\"审核操作失败\"}";
                    }
                }
                else
                {
                    json = "{\"ret\":0,\"msg\":\"审核操作失败\"}";
                }


            }

            return Content(json, "text/json");
        }

        /// <summary>
        /// 待初审贷款
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult waitverify(string borrowing_title = "", string realname = "", string time1 = "", string time2 = "", int Page = 1, int pageSize = 10, int acttype = 0)
        {
            int pageNumber = Page / 1;
            Expression<Func<V_borrowing_target_addlist, bool>> where = PredicateExtensionses.True<V_borrowing_target_addlist>();

            if (acttype == 0)
            {
                where = where.And(p => p.tender_state == 0);
            }
            else
            {

                where = where.And(p => p.tender_state == 1);
            }


            #region 条件
            if (!string.IsNullOrEmpty(borrowing_title))
            {
                where = where.And(a => a.borrowing_title.Contains(borrowing_title));
            }
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(a => a.realname.Contains(realname));
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

            IPagedList<V_borrowing_target_addlist> list = ef.V_borrowing_target_addlist.Where(where).OrderByDescending(p => p.targetid).ToPagedList(pageNumber, pageSize);


            ViewBag.borrowing_title = borrowing_title;
            ViewBag.realname = realname;
            ViewBag.time1 = time1;
            ViewBag.time2 = time2;

            return View(list);
        }

        #endregion

        #region 待复审贷款

        /// <summary>
        /// 复审
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult SubmitwaitverifyCase(int id, string action = "333")
        {
            hx_borrowing_target target = (from a in ef.hx_borrowing_target where a.targetid == id select a).SingleOrDefault();

            IEnumerable<V_borrowing_target_review> list = (from a in ef.V_borrowing_target_review where a.targetid == id select a).OrderBy(a => a.reviewtime).ToList();

            ViewBag.id = id;
            ViewBag.action1 = action;
            ViewBag.target = target;

            return View(list);
        }

        /// <summary>
        /// 待复审贷款
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Rewaitverify(string borrowing_title = "", string realname = "", string time1 = "", string time2 = "", int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<V_borrowing_target_addlist, bool>> where = PredicateExtensionses.True<V_borrowing_target_addlist>();
            where = where.And(p => p.tender_state == 1);

            #region 条件
            if (!string.IsNullOrEmpty(borrowing_title))
            {
                where = where.And(a => a.borrowing_title.Contains(borrowing_title));
            }
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(a => a.realname.Contains(realname));
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

            IPagedList<V_borrowing_target_addlist> list = ef.V_borrowing_target_addlist.Where(where).OrderByDescending(p => p.targetid).ToPagedList(pageNumber, pageSize);


            ViewBag.borrowing_title = borrowing_title;
            ViewBag.realname = realname;
            ViewBag.time1 = time1;
            ViewBag.time2 = time2;

            return View(list);
        }
        #endregion

        #region PDF

        /// <summary>
        /// 生成pdf
        /// </summary>
        /// <param name="action"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public ActionResult pdf(string action, string method, string data)
        {

            string sql = "";
            int id = DNTRequest.GetInt("data", 0);
            string json = "";

            if (action == "pdf" && id > 0)
            {

                sql = " SELECT targetid,loan_number, borrower_registerid,borrowing_title,annual_interest_rate,payment_options,borrowing_balance,life_of_loan,unit_day,release_date,month_payment_date,repayment_date,end_time,companyid,company_name,agent_name,agent_id_card,username,realname,iD_number,legal_representative,usertypes,CopName  from V_borrowing_target_bonding where targetid=" + id;

                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

                if (dt.Rows.Count > 0)
                {


                    string str = "";

                    if (dt.Rows[0]["companyid"].ToString() == "6")
                    {
                        str = ContractText(6);
                    }
                    else
                    {
                        str = ContractText(1);

                    }

                    string fileName = "template" + dt.Rows[0]["targetid"].ToString() + dt.Rows[0]["loan_number"].ToString();

                    string path = "/PDF/" + fileName + ".pdf";


                    M_Contract_management p = new M_Contract_management();

                    B_Contract_management o = new B_Contract_management();

                    DateTime dte = DateTime.Parse(dt.Rows[0]["release_date"].ToString());

                    p.loan_number = decimal.Parse(dt.Rows[0]["loan_number"].ToString());
                    p.targetid = id;

                    p.lender_username = "";
                    p.lender_registerid = 0;
                    p.lender_id_card = "";
                    p.lenders_account_name = "";
                    p.lender_bank_account = "";
                    p.lender_bank = "";

                    p.lenders_telephone = "";
                    p.lenders_email = "";
                    p.lendres_date_contract = dte;

                    p.borrower_registerid = int.Parse(dt.Rows[0]["borrower_registerid"].ToString());


                    p.borrower_name = dt.Rows[0]["realname"].ToString();

                    if (dt.Rows[0]["usertypes"].ToString() == "2")
                    {
                        p.borrower_username = dt.Rows[0]["CopName"].ToString();
                    }
                    else
                    {
                        p.borrower_username = dt.Rows[0]["username"].ToString();
                    }

                    p.borrower_id_card = dt.Rows[0]["iD_number"].ToString();
                    p.borrower_account_name = dt.Rows[0]["realname"].ToString();
                    p.borrower_bank_account = "";
                    p.borrower_date_contract = dte.ToString();
                    p.borrower_bank = "";

                    p.surety_company_name = dt.Rows[0]["company_name"].ToString();
                    p.guarantor_agent = dt.Rows[0]["agent_name"].ToString();
                    p.guarantor_companyid = int.Parse(dt.Rows[0]["companyid"].ToString());
                    p.guarantor_agent_idate_contract = dte;

                    p.guarantor_agent_usernqme = dt.Rows[0]["legal_representative"].ToString();

                    p.witness_date_contract = dte;


                    p.contract_money = str;

                    p.contract_amount = decimal.Parse(dt.Rows[0]["borrowing_balance"].ToString());

                    p.createtime = dte;

                    p.mode_payment = Utils.Getpayment_options(int.Parse(dt.Rows[0]["payment_options"].ToString()));

                    p.contract_type = 0;

                    sql = "select contract,targetid from Contract_management where contract_type=0  and  targetid=" + id;

                    DataTable dt1 = DbHelperSQL.GET_DataTable_List(sql);
                    if (dt1.Rows.Count > 0)
                    {
                        p.contract = int.Parse(dt1.Rows[0]["contract"].ToString());

                        o.Update(p);


                    }
                    else
                    {
                        o.Add(p);

                    }





                    StringBuilder sb = new StringBuilder(str);

                    sb = sb.Replace("#loan_number#", p.loan_number.ToString());

                    sb = sb.Replace("#borrower_username#", p.borrower_username);

                    sb = sb.Replace("#borrower_name#", p.borrower_name);


                    sb = sb.Replace("#borrower_id_card#", p.borrower_id_card);

                    sb = sb.Replace("#lender_username#", p.lender_username);

                    sb = sb.Replace("#lender_name#", p.lender_name);

                    sb = sb.Replace("#lender_id_card#", p.lender_id_card);

                    sb = sb.Replace("#surety_company_name#", p.surety_company_name);

                    sb = sb.Replace("#guarantor_agent_usernqme#", p.guarantor_agent_usernqme);


                    sb = sb.Replace("#contract_amount#", RMB.GetDecimal(p.contract_amount, 2, true).ToString());

                    sb = sb.Replace("#annual_interest_rate#", decimal.Parse(dt.Rows[0]["annual_interest_rate"].ToString()).ToString("0.00"));

                    DateTime date1 = DateTime.Parse(dt.Rows[0]["release_date"].ToString());


                    DateTime date2 = DateTime.Parse(dt.Rows[0]["repayment_date"].ToString());

                    sb = sb.Replace("#release_date#", date1.ToString("yyyy-MM-dd"));

                    sb = sb.Replace("#repayment_date#", date2.ToString("yyyy-MM-dd"));


                    sb = sb.Replace("#days#", Utils.DateDiff("Day", date1, date2).ToString());



                    if (HTMLToPDF(sb.ToString(), fileName))
                    {
                        json = @" {""rs""    : ""y"", ""datainfo"" :  ""add""}";
                        json = json.Replace("add", path);

                        return Content(json, "text/json");
                    }
                    else
                    {
                        json = @" {""rs""    : ""n"", ""datainfo"" :  ""PDF合同生成失败""}";
                        return Content(json, "text/json");
                    }

                }

            }
            else if (action == "UserPDF" && id > 0)
            {
                //生成用户合同


                sql = " SELECT targetid,loan_number, borrower_registerid,borrowing_title,annual_interest_rate,payment_options,borrowing_balance,life_of_loan,unit_day,release_date,month_payment_date,repayment_date,end_time,companyid,company_name,agent_name,agent_id_card,username,realname,iD_number,usertypes,CopName  from V_borrowing_target_bonding where targetid=" + id;

                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

                if (dt.Rows.Count > 0)
                {
                    string str = UserContactText(id);

                    B_member_table ub = new B_member_table();
                    M_member_table up = new M_member_table();

                    int uid = Utils.checkloginsessiontop();

                    //int uid = 9;
                    if (uid <= 0)
                    {

                        json = @" {""rs""    : ""n"", ""datainfo"" :  ""操作失败""}";
                        return Content(json, "text/json");
                    }

                    up = ub.GetModel(uid);



                    string fileName = "U_" + up.registerid.ToString() + "_" + dt.Rows[0]["targetid"].ToString() + "_" + dt.Rows[0]["loan_number"].ToString() + "_" + Utils.RndNum(3);

                    string path = "/PDF/" + fileName + ".pdf";


                    M_Contract_management p = new M_Contract_management();

                    B_Contract_management o = new B_Contract_management();


                    sql = "select top 1 bid_records_id,investment_amount,value_date,investment_maturity from hx_Bid_records where targetid=" + id.ToString() + " and  investor_registerid=" + uid.ToString() + " order by bid_records_id desc";

                    DataTable dtbid = DbHelperSQL.GET_DataTable_List(sql);


                    DateTime dte = DateTime.Now;

                    p.loan_number = decimal.Parse(dt.Rows[0]["loan_number"].ToString());
                    p.targetid = id;
                    p.bid_records_id = int.Parse(dtbid.Rows[0]["bid_records_id"].ToString());
                    p.lender_username = up.username;
                    p.lender_name = up.realname;
                    p.lender_registerid = uid;
                    p.lender_id_card = up.iD_number;
                    p.lenders_account_name = "";
                    p.lender_bank_account = "";
                    p.lender_bank = "";

                    p.lenders_telephone = up.mobile;
                    p.lenders_email = up.email;
                    p.lendres_date_contract = dte;

                    p.borrower_registerid = int.Parse(dt.Rows[0]["borrower_registerid"].ToString());
                    p.borrower_name = dt.Rows[0]["realname"].ToString();


                    if (dt.Rows[0]["usertypes"].ToString() == "2")
                    {
                        p.borrower_username = dt.Rows[0]["CopName"].ToString();
                    }
                    else
                    {
                        p.borrower_username = dt.Rows[0]["username"].ToString();
                    }
                    p.borrower_id_card = dt.Rows[0]["iD_number"].ToString();
                    p.borrower_account_name = dt.Rows[0]["realname"].ToString();
                    p.borrower_bank_account = "";
                    p.borrower_date_contract = dte.ToString();
                    p.borrower_bank = "";

                    p.surety_company_name = dt.Rows[0]["company_name"].ToString();
                    p.guarantor_agent = dt.Rows[0]["agent_name"].ToString();

                    p.guarantor_agent_usernqme = dt.Rows[0]["agent_name"].ToString();
                    p.guarantor_companyid = int.Parse(dt.Rows[0]["companyid"].ToString());
                    p.guarantor_agent_idate_contract = dte;


                    p.witness_date_contract = dte;


                    p.contract_money = str;

                    p.contract_amount = decimal.Parse(dtbid.Rows[0]["investment_amount"].ToString());

                    p.createtime = dte;

                    p.mode_payment = Utils.Getpayment_options(int.Parse(dt.Rows[0]["payment_options"].ToString()));

                    p.contract_type = 1;


                    StringBuilder sb = new StringBuilder(str);

                    sb = sb.Replace("#loan_number#", p.loan_number.ToString());

                    sb = sb.Replace("#borrower_username#", p.borrower_username);

                    sb = sb.Replace("#borrower_name#", p.borrower_name);


                    sb = sb.Replace("#borrower_id_card#", p.borrower_id_card);

                    sb = sb.Replace("#lender_username#", p.lender_username);

                    sb = sb.Replace("#lender_name#", p.lender_name);

                    sb = sb.Replace("#lender_id_card#", p.lender_id_card);

                    sb = sb.Replace("#surety_company_name#", p.surety_company_name);

                    sb = sb.Replace("#guarantor_agent_usernqme#", p.guarantor_agent_usernqme);


                    sb = sb.Replace("#contract_amount#", RMB.GetDecimal(p.contract_amount, 2, true).ToString());

                    sb = sb.Replace("#annual_interest_rate#", decimal.Parse(dt.Rows[0]["annual_interest_rate"].ToString()).ToString("0.00"));

                    DateTime date1 = DateTime.Parse(dtbid.Rows[0]["value_date"].ToString());


                    DateTime date2 = DateTime.Parse(dtbid.Rows[0]["investment_maturity"].ToString());

                    sb = sb.Replace("#release_date#", date1.ToString("yyyy-MM-dd"));

                    sb = sb.Replace("#repayment_date#", date2.ToString("yyyy-MM-dd"));



                    sb = sb.Replace("#days#", Utils.DateDiff("Day", date1, date2).ToString());



                    p.contract_money = sb.ToString();

                    p.contractpath = path;

                    int cid = o.Add(p);
                    if (cid > 0)
                    {

                        sql = "update hx_Bid_records set contractid=" + cid + ",contractpath= '" + p.contractpath + "' where bid_records_id=" + p.bid_records_id;

                        DbHelperSQL.ExecuteSql(sql);

                        if (HTMLToPDF(sb.ToString(), fileName))
                        {
                            json = @" {""rs""    : ""y"", ""datainfo"" :  ""/usercenter/myinvest.html""}";


                            return Content(json, "text/json");
                        }
                        else
                        {
                            json = @" {""rs""    : ""n"", ""datainfo"" :  ""PDF合同生成失败""}";
                            return Content(json, "text/json");
                        }

                    }


                }

            }

            else if (action == "MUserPDF" && id > 0)
            {
                //生成用户合同

                LogInfo.WriteLog("生成用户合同响应");
                sql = " SELECT targetid,loan_number, borrower_registerid,borrowing_title,annual_interest_rate,payment_options,borrowing_balance,life_of_loan,unit_day,release_date,month_payment_date,repayment_date,end_time,companyid,company_name,agent_name,agent_id_card,username,realname,iD_number,usertypes,CopName  from V_borrowing_target_bonding where targetid=" + id;

                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

                if (dt.Rows.Count > 0)
                {
                    string str = UserContactText(id);

                    B_member_table ub = new B_member_table();
                    M_member_table up = new M_member_table();

                    // int uid = Utils.checkloginsessiontop();

                    int uid = DNTRequest.GetInt("uc", 0);
                    string OrdId = DNTRequest.GetString("OrdId");


                    LogInfo.WriteLog("是否有接收到信息： OrdId=" + OrdId + " uc:" + uid);

                    //int uid = 9;
                    if (uid <= 0)
                    {
                        json = @" {""rs""    : ""n"", ""datainfo"" :  ""操作失败""}";
                        return Content(json, "text/json");
                    }

                    up = ub.GetModel(uid);



                    string fileName = "U_" + up.registerid.ToString() + "_" + dt.Rows[0]["targetid"].ToString() + "_" + dt.Rows[0]["loan_number"].ToString() + "_" + Utils.RndNum(3);

                    string path = "/PDF/" + fileName + ".pdf";


                    M_Contract_management p = new M_Contract_management();

                    B_Contract_management o = new B_Contract_management();


                    sql = "select top 1 bid_records_id,investment_amount,value_date,investment_maturity from hx_Bid_records where targetid=" + id.ToString() + " and  investor_registerid=" + uid.ToString() + " and  OrdId ='" + OrdId + "' order by bid_records_id desc";

                    DataTable dtbid = DbHelperSQL.GET_DataTable_List(sql);


                    DateTime dte = DateTime.Now;

                    p.loan_number = decimal.Parse(dt.Rows[0]["loan_number"].ToString());
                    p.targetid = id;
                    p.bid_records_id = int.Parse(dtbid.Rows[0]["bid_records_id"].ToString());
                    p.lender_username = up.username;
                    p.lender_name = up.realname;
                    p.lender_registerid = uid;
                    p.lender_id_card = up.iD_number;
                    p.lenders_account_name = "";
                    p.lender_bank_account = "";
                    p.lender_bank = "";

                    p.lenders_telephone = up.mobile;
                    p.lenders_email = up.email;
                    p.lendres_date_contract = dte;

                    p.borrower_registerid = int.Parse(dt.Rows[0]["borrower_registerid"].ToString());
                    p.borrower_name = dt.Rows[0]["realname"].ToString();


                    if (dt.Rows[0]["usertypes"].ToString() == "2")
                    {
                        p.borrower_username = dt.Rows[0]["CopName"].ToString();
                    }
                    else
                    {
                        p.borrower_username = dt.Rows[0]["username"].ToString();
                    }
                    p.borrower_id_card = dt.Rows[0]["iD_number"].ToString();
                    p.borrower_account_name = dt.Rows[0]["realname"].ToString();
                    p.borrower_bank_account = "";
                    p.borrower_date_contract = dte.ToString();
                    p.borrower_bank = "";

                    p.surety_company_name = dt.Rows[0]["company_name"].ToString();
                    p.guarantor_agent = dt.Rows[0]["agent_name"].ToString();

                    p.guarantor_agent_usernqme = dt.Rows[0]["agent_name"].ToString();
                    p.guarantor_companyid = int.Parse(dt.Rows[0]["companyid"].ToString());
                    p.guarantor_agent_idate_contract = dte;


                    p.witness_date_contract = dte;


                    p.contract_money = str;

                    p.contract_amount = decimal.Parse(dtbid.Rows[0]["investment_amount"].ToString());

                    p.createtime = dte;

                    p.mode_payment = Utils.Getpayment_options(int.Parse(dt.Rows[0]["payment_options"].ToString()));

                    p.contract_type = 1;


                    StringBuilder sb = new StringBuilder(str);

                    sb = sb.Replace("#loan_number#", p.loan_number.ToString());

                    sb = sb.Replace("#borrower_username#", p.borrower_username);

                    sb = sb.Replace("#borrower_name#", p.borrower_name);


                    sb = sb.Replace("#borrower_id_card#", p.borrower_id_card);

                    sb = sb.Replace("#lender_username#", p.lender_username);

                    sb = sb.Replace("#lender_name#", p.lender_name);

                    sb = sb.Replace("#lender_id_card#", p.lender_id_card);

                    sb = sb.Replace("#surety_company_name#", p.surety_company_name);

                    sb = sb.Replace("#guarantor_agent_usernqme#", p.guarantor_agent_usernqme);


                    sb = sb.Replace("#contract_amount#", RMB.GetDecimal(p.contract_amount, 2, true).ToString());

                    sb = sb.Replace("#annual_interest_rate#", decimal.Parse(dt.Rows[0]["annual_interest_rate"].ToString()).ToString("0.00"));

                    DateTime date1 = DateTime.Parse(dtbid.Rows[0]["value_date"].ToString());


                    DateTime date2 = DateTime.Parse(dtbid.Rows[0]["investment_maturity"].ToString());

                    sb = sb.Replace("#release_date#", date1.ToString("yyyy-MM-dd"));

                    sb = sb.Replace("#repayment_date#", date2.ToString("yyyy-MM-dd"));



                    sb = sb.Replace("#days#", Utils.DateDiff("Day", date1, date2).ToString());



                    p.contract_money = sb.ToString();

                    p.contractpath = path;

                    int cid = o.Add(p);
                    if (cid > 0)
                    {

                        sql = "update hx_Bid_records set contractid=" + cid + ",contractpath= '" + p.contractpath + "' where bid_records_id=" + p.bid_records_id;

                        DbHelperSQL.ExecuteSql(sql);

                        if (HTMLToPDF(sb.ToString(), fileName))
                        {
                            json = @" {""rs""    : ""y"", ""datainfo"" :  ""/usercenter/myinvest.html""}";



                            return Content(json, "text/json");
                        }
                        else
                        {
                            json = @" {""rs""    : ""n"", ""datainfo"" :  ""PDF合同生成失败""}";
                            return Content(json, "text/json");
                        }

                    }


                }

            }

            json = @" {""rs""    : ""n"", ""datainfo"" :  ""操作失败""}";
            return Content(json, "text/json");

        }

        private string UserContactText(int targetid)
        {

            string sql = "SELECT  contract_money from Contract_management where contract_type=0 and  targetid= " + targetid;

            return DbHelperSQL.Re_String(sql);
        }

        private string ContractText(int contract_type_id)
        {
            string sql = "SELECT  contract_template_context from hx_Contract_template where usestate=1 and contract_type_id= " + contract_type_id;

            return DbHelperSQL.Re_String(sql);
        }

        public Boolean HTMLToPDF(string html, String fileName)
        {
            Boolean isOK = false;
            try
            {
                //  FontFactory.RegisterFamily("宋体", "simsun", @"c:\windows\fonts\SIMSUN.TTC,0");




                TextReader reader = new StringReader(html);



                // step 1: creation of a document-object
                //  Document document = new Document(PageSize.A4.Rotate(), 30, 30, 30, 30);
                Document document = new Document(PageSize.A4, 30, 30, 36, 36);//左右上下
                // step 2:
                // we create a writer that listens to the document
                // and directs a XML-stream to a file
                fileName = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\PDF\\" + fileName + ".pdf";
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                HTMLWorker worker = new HTMLWorker(document);
                document.Open();

                document.AddTitle("创利投网站金融平台");
                document.AddAuthor("创利投");
                document.AddCreationDate();
                document.AddHeader("p2p合同", "p2p合同");
                document.AddCreator("创利投科技发展有限公司");
                document.AddKeywords("P2B合同");
                document.AddSubject("创利投四方合同");
                document.AddProducer();


                writer.PageEvent = new HeaderAndFooterEvent();

                HeaderAndFooterEvent.PAGE_NUMBER = true;//不实现页眉跟页脚  
                First(document, writer);//封面页  



                worker.StartDocument();
                StyleSheet css = new StyleSheet();



                Dictionary<String, Object> font = new Dictionary<string, object>();
                font.Add(HTMLWorker.FONT_PROVIDER, new MyFontFactory());



                Dictionary<String, String> dict = new Dictionary<string, string>();
                dict.Add(HtmlTags.BGCOLOR, "#01366C");
                dict.Add(HtmlTags.COLOR, "#00ff00");
                dict.Add(HtmlTags.SIZE, "25");
                css.LoadStyle("css", dict);



                List<IElement> p = HTMLWorker.ParseToList(reader, css, font);
                // List<IElement> p = HTMLWorker.ParseToList(reader, css);




                for (int k = 0; k < p.Count; k++)
                {
                    document.Add((IElement)p[k]);


                }





                worker.EndDocument();

                writer.Flush();
                writer.CloseStream = true;
                worker.Close();
                document.Close();
                reader.Close();
                isOK = true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
                LogInfo.WriteLog("生成PDF异常：" + ex.Message + ",strace:" + ex.StackTrace);
                isOK = false;
            }
            finally
            {

            }
            return isOK;
        }
        private void First(Document doc, PdfWriter writer)
        {
            string tmp = "创利投金融平台网站合同";
            doc.Add(HeaderAndFooterEvent.InsertTitleContent(tmp));

            //tmp = "(正文     页,附件 0 页)";
            tmp = "(时间: " + DateTime.Now.ToString("yyyy-MM-dd") + ")";
            doc.Add(HeaderAndFooterEvent.InsertTitleContent(tmp));

            //模版 显示总共页数  
            HeaderAndFooterEvent.tpl = writer.DirectContent.CreateTemplate(100, 100); //模版的宽度和高度  
            PdfContentByte cb = writer.DirectContent;
            cb.AddTemplate(HeaderAndFooterEvent.tpl, 266, 914);//调节模版显示的位置  


        }

        #endregion





        public ActionResult SubmitReview_case()
        {
            string str1 = "";
            string sql = "";
            string dfd = "0";
            int tender_state1 = DNTRequest.GetInt("tender_state", 7);
            string contract = Utils.CheckSQL(DNTRequest.GetString("contract"));
            if (contract.Length > 0)
            {
                int id = DNTRequest.GetInt("hid_id", 0);
                string reviewremarks1 = Utils.CheckSQLHtml(DNTRequest.GetString("reviewremarks"));
                if (tender_state1 == 2)
                {
                    sql = "select top 1 * from V_borrowing_target_addlist  where targetid=" + id.ToString();
                    DataTable dt = new DataTable();
                    dt = DbHelperSQL.GET_DataTable_List(sql);
                    if (dt.Rows.Count > 0)
                    {
                        M_AddBidInfo ma = new M_AddBidInfo();
                        ma.Version = "10";
                        ma.CmdId = "AddBidInfo";
                        ma.MerCustId = Utils.GetMerCustID();
                        ma.ProId = id.ToString();
                        ma.BorrCustId = dt.Rows[0]["BorrUsrCustId"].ToString();
                        ma.BorrTotAmt = decimal.Parse(dt.Rows[0]["borrowing_balance"].ToString()).ToString("0.00");

                        decimal yearrate = decimal.Parse(dt.Rows[0]["annual_interest_rate"].ToString()) / 100;

                        ma.YearRate = yearrate.ToString("0.00");
                        ma.RetType = "03";   //这个位置需要用方法转换入动态写入
                        ma.BidStartDate = DateTime.Parse(dt.Rows[0]["release_date"].ToString()).ToString("yyyyMMddhhmmss");
                        ma.BidEndDate = DateTime.Parse(dt.Rows[0]["repayment_date"].ToString()).ToString("yyyyMMddhhmmss");

                        InvestmentParameters mp = new InvestmentParameters();
                        mp.Amount = decimal.Parse(dt.Rows[0]["borrowing_balance"].ToString());
                        mp.Circle = int.Parse(dt.Rows[0]["life_of_loan"].ToString());
                        mp.CircleType = int.Parse(dt.Rows[0]["unit_day"].ToString());
                        mp.NominalYearRate = double.Parse(dt.Rows[0]["annual_interest_rate"].ToString());
                        mp.OverheadsRate = 0f;
                        mp.RepaymentMode = int.Parse(dt.Rows[0]["payment_options"].ToString());
                        mp.RewardRate = 0f;
                        mp.IsThirtyDayMonth = false;
                        mp.InvestDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                        mp.Investmentenddate = DateTime.Parse(dt.Rows[0]["repayment_date"].ToString());
                        mp.Payinterest = int.Parse(dt.Rows[0]["month_payment_date"].ToString());
                        mp.InvestObject = 1;

                        InvestCalculatorB dd = new InvestCalculatorB();
                        decimal lixi = dd.Calculationofinterest(mp);
                        decimal RetAmt = mp.Amount + lixi;

                        ma.RetAmt = RetAmt.ToString("0.00"); // 本金+利息

                        ma.RetDate = DateTime.Parse(dt.Rows[0]["release_date"].ToString()).ToString("yyyyMMdd");

                        if (dt.Rows[0]["GuarType"].ToString() == "1")
                        {
                            ma.GuarCompId = dt.Rows[0]["GuarCompUsrCustId"].ToString();

                            if (dt.Rows[0]["GuarType"].ToString() == "0")
                            {

                                ma.GuarAmt = "0.00";
                            }
                            else
                            {
                                ma.GuarAmt = decimal.Parse(dt.Rows[0]["borrowing_balance"].ToString()).ToString("0.00");
                            }

                        }


                        ma.ProArea = "1100"; //这里是根据项目地区调过来 暂时固定

                        ma.BgRetUrl = Utils.GetRe_url("admin/Thirdparty/BgReviw_case");

                        ma.MerPriv = dt.Rows[0]["GuarType"].ToString();

                        StringBuilder chkVal = new StringBuilder();
                        chkVal.Append(ma.Version);
                        chkVal.Append(ma.CmdId);
                        chkVal.Append(ma.MerCustId);
                        chkVal.Append(ma.ProId);
                        chkVal.Append(ma.BorrCustId);
                        chkVal.Append(ma.BorrTotAmt);
                        chkVal.Append(ma.YearRate);
                        chkVal.Append(ma.RetType);
                        chkVal.Append(ma.BidStartDate);
                        chkVal.Append(ma.BidEndDate);
                        chkVal.Append(ma.RetAmt);
                        chkVal.Append(ma.RetDate);


                        if (dt.Rows[0]["IsUse"].ToString() == "1")
                        {
                            chkVal.Append(ma.GuarCompId);
                            chkVal.Append(ma.GuarAmt);
                        }


                        // chkVal.Append(ma.GuarAmt);//dfsf
                        chkVal.Append(ma.ProArea);
                        chkVal.Append(ma.BgRetUrl);
                        chkVal.Append(ma.MerPriv);
                        chkVal.Append(ma.ReqExt);

                        string chkv = chkVal.ToString();

                        LogInfo.WriteLog("新标chkv字符:" + chkv);

                        //私钥文件的位置(这里是放在了站点的根目录下)
                        string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
                        //需要指定提交字符串的长度
                        int len = Encoding.UTF8.GetBytes(chkv).Length;
                        StringBuilder sbChkValue = new StringBuilder(256);
                        //加签
                        int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

                        LogInfo.WriteLog("新标加签字符:" + str.ToString());

                        ma.ChkValue = sbChkValue.ToString();


                        LogInfo.WriteLog("新标标的录入:" + FastJSON.toJOSN(ma));
                        if (str == 0)
                        {

                            sql = "update  hx_borrowing_target set  tender_state=" + tender_state1.ToString() + ",G_contract_Path='" + contract + "' where targetid=" + id.ToString();

                            if (DbHelperSQL.ExecuteSql(sql) > 0)
                            {
                                string xicaiRes = new XiCaiHelper("").PushP2PData(id.ToString());
                                LogInfo.WriteLog("推送给希财渠道的标的数据：" + id.ToString() + ",返回数据：" + xicaiRes);
                                B_reviewremarks o = new B_reviewremarks();
                                M_reviewremarks p = new M_reviewremarks();

                                p.targetid = id;
                                p.tender_state = tender_state1;
                                p.reviewremarks = reviewremarks1;
                                p.reviewtime = DateTime.Now;
                                p.admin_operator = Utils.GetAdmUserID();

                                if (o.Add(p) > 0)
                                {

                                    StringBuilder strz = new StringBuilder();

                                    strz.Append(" <form id=\"formauto\" name=\"formauto\"  action=\"" + Utils.GetChinapnrUrl() + "\" method=\"post\">");

                                    strz.Append("<input id=\"Version\"  name=\"Version\"  type=\"hidden\"  value=\"" + ma.Version + "\" />");

                                    strz.Append("<input id=\"CmdId\"  name=\"CmdId\"    type=\"hidden\"  value=\"" + ma.CmdId + "\" />");

                                    strz.Append("<input id=\"MerCustId\" name=\"MerCustId\"   type=\"hidden\"  value=\"" + ma.MerCustId + "\" />");

                                    strz.Append("<input id=\"ProId\" name=\"ProId\" type=\"hidden\"  value=\"" + ma.ProId + "\" />");

                                    strz.Append("<input id=\"BorrCustId\" name=\"BorrCustId\" type=\"hidden\"  value=\"" + ma.BorrCustId + "\" />");

                                    strz.Append("<input id=\"BorrTotAmt\" name=\"BorrTotAmt\" type=\"hidden\"  value=\"" + ma.BorrTotAmt + "\" />");

                                    strz.Append("<input id=\"YearRate\"  name=\"YearRate\" type=\"hidden\"  value=\"" + ma.YearRate + "\" />");

                                    strz.Append("<input id=\"RetType\"  name=\"RetType\" type=\"hidden\"  value=\"" + ma.RetType + "\" />");

                                    strz.Append("<input id=\"BidStartDate\"   name=\"BidStartDate\" type=\"hidden\"  value=\"" + ma.BidStartDate + "\" />");

                                    strz.Append("<input id=\"BidEndDate\" name=\"BidEndDate\" type=\"hidden\"  value=" + ma.BidEndDate + " />");


                                    strz.Append("<input id=\"RetAmt\" name=\"RetAmt\" type=\"hidden\"  value=\"" + ma.RetAmt + "\" />");
                                    strz.Append("<input id=\"RetDate\" name=\"RetDate\" type=\"hidden\"  value=\"" + ma.RetDate + "\" />");

                                    if (dt.Rows[0]["IsUse"].ToString() == "1")
                                    {
                                        strz.Append("<input id=\"GuarCompId\" name=\"GuarCompId\" type=\"hidden\"  value=\"" + ma.GuarCompId + "\" />");

                                        strz.Append("<input id=\"GuarAmt\" name=\"GuarAmt\" type=\"hidden\"  value=\"" + ma.GuarAmt + "\" />");
                                    }
                                    strz.Append("<input id=\"ProArea\" name=\"ProArea\" type=\"hidden\"  value=\"" + ma.ProArea + "\" />");
                                    strz.Append("<input id=\"BgRetUrl\" name=\"BgRetUrl\" type=\"hidden\"  value=\"" + ma.BgRetUrl + "\" />");
                                    strz.Append("<input id=\"MerPriv\" name=\"MerPriv\" type=\"hidden\"  value=\"" + ma.MerPriv + "\" />");
                                    strz.Append("<input id=\"ReqExt\" name=\"ReqExt\" type=\"hidden\"  value=\"" + ma.ReqExt + "\" />");

                                    strz.Append("<input id=\"ChkValue\" name=\"ChkValue\" type=\"hidden\"  value=\"" + ma.ChkValue + "\" />");
                                    strz.Append(" </form>");
                                    strz.Append("<script type=\"text/javascript\">document.getElementById('formauto').submit();</script>");

                                    LogInfo.WriteLog("录入标的提交信息：" + strz.ToString());
                                    str1 = strz.ToString();
                                }
                                else
                                {
                                    return Content(StringAlert.Alert("审核操作失败"), "text/html");

                                }
                            }
                        }
                        else
                        {
                            return Content(StringAlert.Alert("签名失败"), "text/html");

                        }

                    }
                    else
                    {
                        return Content(StringAlert.Alert("参数或数据有误"), "text/html");

                    }

                }
                else
                {

                    //复审不通过
                    sql = "update  hx_borrowing_target set  tender_state=" + tender_state1.ToString() + " where targetid=" + id.ToString();
                    if (DbHelperSQL.ExecuteSql(sql) > 0)
                    {
                        dfd = "1";

                        // return Content(StringAlert.Alert("复审未通过，自动退回") + "<script>  </script>", "text/html");                 
                    }
                }
            }
            else
            {
                return Content(StringAlert.Alert("请先生成合同"), "text/html");
            }
            ViewBag.str1 = str1;
            ViewBag.dfd = dfd;
            return View();
        }
    }
}
