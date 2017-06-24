using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.pdf;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuangLitouP2P.Models;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Controllers
{
    public class pdfController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        string json = "";
        // GET: pdf
        public ActionResult Index()
        {
            string str = "pdf";
            string sql = "";
            int id = DNTRequest.GetInt("data", 0);
            string action = Utils.CheckSQLHtml(DNTRequest.GetString("action"));
            if (action == "PDF" && id > 0)
            {//生成某项目借款合同范本
                sql = " SELECT targetid,loan_number, borrower_registerid,borrowing_title,annual_interest_rate,payment_options,borrowing_balance,life_of_loan,unit_day,release_date,month_payment_date,repayment_date,end_time,companyid,company_name,agent_name,agent_id_card,username,realname,iD_number,legal_representative,usertypes,CopName  from V_borrowing_target_bonding where targetid=" + id;

                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                if (dt.Rows.Count > 0)
                {
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

                    /*

                    string expr = @"#+([a-zA-Z0-9_\u4e00-\u9fa5]*)+#";
                    MatchCollection mc = Regex.Matches(sb.ToString(), expr);

                    string[] array = new string[mc.Count];
                    array.SetValue(p.loan_number.ToString(), 0);
                    array.SetValue(p.borrower_username, 1);
                    array.SetValue(p.borrower_name, 2);
                    array.SetValue(p.borrower_id_card, 3);
                    array.SetValue(p.lender_username, 4);
                    array.SetValue(p.lender_name, 5);
                    array.SetValue(p.lender_id_card, 6);
                    array.SetValue(p.surety_company_name, 7);
                    array.SetValue(p.guarantor_agent_usernqme, 8);
                    array.SetValue("", 9);
                    array.SetValue(dt.Rows[0]["annual_interest_rate"].ToString(), 10);
                    DateTime date1 = DateTime.Parse(dt.Rows[0]["release_date"].ToString());
                    DateTime date2 = DateTime.Parse(dt.Rows[0]["repayment_date"].ToString());
                    array.SetValue(date1.ToString("yyyy-MM-dd"), 11);
                    array.SetValue(date2.ToString("yyyy-MM-dd"), 12);
                    array.SetValue(Utils.DateDiff("Day",date1,date2).ToString(), 13);

                    array.SetValue(p.borrower_name, 14);
                    array.SetValue(p.borrower_id_card, 15);
                    array.SetValue("", 16);
                    array.SetValue("", 17);
                    array.SetValue(p.surety_company_name, 18);
                    array.SetValue(p.guarantor_agent_usernqme, 19);

                    for (int i = 0; i < mc.Count; i++)
                    {
                        sb = sb.Replace(mc[i].ToString(), array[i].ToString());
                    }
                    */      

                    if (HTMLToPDF(sb.ToString(), fileName))
                    {
                        json = @" {""rs""    : ""y"", ""datainfo"" :  ""add""}";
                        json = json.Replace("add", path);
                        Response.Write(json);
                        Response.End();
                    }
                    else
                    {
                        json = @" {""rs""    : ""n"", ""datainfo"" :  ""PDF合同生成失败""}";
                        Response.Write(json);
                        Response.End();
                    }
                }
                return Content(str);
                //return View();
            }
            else if (action == "UserPDF" && id > 0)
            {
                //生成用户合同
                sql = " SELECT targetid,loan_number, borrower_registerid,borrowing_title,annual_interest_rate,payment_options,borrowing_balance,life_of_loan,unit_day,release_date,month_payment_date,repayment_date,end_time,companyid,company_name,agent_name,agent_id_card,username,realname,iD_number,usertypes,CopName  from V_borrowing_target_bonding where targetid=" + id;

                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                if (dt.Rows.Count > 0)
                {
                    str = UserContactText(id);
                    B_member_table ub = new B_member_table();
                    M_member_table up = new M_member_table();
                    int uid = Utils.checkloginsessiontop();
                    //int uid = 9;
                    if (uid <= 0)
                    {
                        Response.Redirect("/login.html");
                        Response.End();
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
                            Response.Write(json);
                            Response.End();
                        }
                        else
                        {
                            json = @" {""rs""    : ""n"", ""datainfo"" :  ""PDF合同生成失败""}";
                            Response.Write(json);
                            Response.End();
                        }
                    }
                }
            }
            else if (action == "MUserPDF" && id > 0)
            { //生成用户合同
                LogInfo.WriteLog("生成用户合同响应");
                sql = " SELECT targetid,loan_number, borrower_registerid,borrowing_title,annual_interest_rate,payment_options,borrowing_balance,life_of_loan,unit_day,release_date,month_payment_date,repayment_date,end_time,companyid,company_name,agent_name,agent_id_card,username,realname,iD_number,usertypes,CopName  from V_borrowing_target_bonding where targetid=" + id;

                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                if (dt.Rows.Count > 0)
                {
                    str = UserContactText(id);
                    B_member_table ub = new B_member_table();
                    M_member_table up = new M_member_table();
                    // int uid = Utils.checkloginsessiontop();
                    int uid = DNTRequest.GetInt("uc", 0);
                    string OrdId = DNTRequest.GetString("OrdId");
                    LogInfo.WriteLog("是否有接收到信息： OrdId=" + OrdId + " uc:" + uid);

                    //int uid = 9;
                    if (uid <= 0)
                    {
                        Response.End();
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
                            Response.Write(json);
                            Response.End();
                        }
                        else
                        {
                            json = @" {""rs""    : ""n"", ""datainfo"" :  ""PDF合同生成失败""}";
                            Response.Write(json);
                            Response.End();
                        }
                    }
                }
            }
            return Content(str);//权限？
            //return View();
        }

        //用户查看pdf（我的投资列表---合同 ---查看） 增加权限判断
        //waitfor 将多处生成合同代码整合复用
        public JsonResult MakePdf(int id)
        {
            string contractpath = "";

            hx_Bid_records hbr = ef.hx_Bid_records.Where(p => p.bid_records_id == id).FirstOrDefault();
            int uid = Utils.checkloginsessiontop();
            if (hbr != null && hbr.investor_registerid == uid)
            {
                contractpath = hbr.contractpath;
            }
            else
            {
                return Json(new PdfDetail { Path = "" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                if (string.IsNullOrEmpty(contractpath) || !System.IO.File.Exists(Server.MapPath(contractpath)))
                {
                    #region 生成合同
                    //生成用户合同
                    LogInfo.WriteLog("生成用户合同响应");
                    var sql = " SELECT targetid,loan_number, borrower_registerid,borrowing_title,annual_interest_rate,payment_options,borrowing_balance,life_of_loan,unit_day,release_date,month_payment_date,repayment_date,end_time,companyid,company_name,agent_name,agent_id_card,username,realname,iD_number,usertypes,CopName  from V_borrowing_target_bonding where targetid=" + hbr.targetid;

                    DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                    if (dt.Rows.Count > 0)
                    {
                        var str = UserContactText(Convert.ToInt32(hbr.targetid));
                        B_member_table ub = new B_member_table();
                        M_member_table up = new M_member_table();
                        // int uid = Utils.checkloginsessiontop();
                       // int uid = Convert.ToInt32(hbr.investor_registerid);
                        string OrdId = hbr.OrdId.ToString();
                        LogInfo.WriteLog("是否有接收到信息： OrdId=" + OrdId + " uc:" + uid);
                        //if (uid <= 0)
                        //{
                        //    Response.End();
                        //}
                        up = ub.GetModel(uid);
                        string fileName = "U_" + up.registerid.ToString() + "_" + dt.Rows[0]["targetid"].ToString() + "_" + dt.Rows[0]["loan_number"].ToString() + "_" + Utils.RndNum(3);
                        string path = "/PDF/" + fileName + ".pdf";

                        M_Contract_management p = new M_Contract_management();
                        B_Contract_management o = new B_Contract_management();

                        sql = "select top 1 bid_records_id,investment_amount,value_date,investment_maturity from hx_Bid_records where targetid=" + hbr.targetid.ToString() + " and  investor_registerid=" + uid.ToString() + " and  OrdId ='" + OrdId + "' order by bid_records_id desc";

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
                                return Json(new PdfDetail { Path = p.contractpath }, JsonRequestBehavior.AllowGet);
                            }

                        }
                    }
                    #endregion
                }
                else {
                    return Json(new PdfDetail { Path = contractpath }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new PdfDetail { Path = "" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new PdfDetail { Path = contractpath }, JsonRequestBehavior.AllowGet);
        }

        // 用户个人中心 投资列表--合同---下载 增加权限判断
        //waitfor 使用远程调用生成合同 生成合同代码待整合
        public ActionResult DownPDf(int id)
        {
            string contractpath = "";
            hx_Bid_records hbr = ef.hx_Bid_records.Where(p => p.bid_records_id == id).FirstOrDefault();
            int uid = Utils.checkloginsessiontop();
            if (uid <= 0)
            {
                Response.Redirect("/login.html");
                Response.End();
            }

            if (hbr != null&& hbr.investor_registerid==uid)
            {
                contractpath = hbr.contractpath;
            }else
            {
                Response.Write("路径无效，请在投资列表页重新下载合同！");
                Response.End();
            }
            try
            {
                if (string.IsNullOrEmpty(contractpath) || !System.IO.File.Exists(Server.MapPath(contractpath)))
                {
                    var url = Settings.Instance.SiteDomain + "";
                    //远程调用生成合同
                    string postString = "?action=MUserPDF&data=" + hbr.targetid + "&uc=" + hbr.investor_registerid + "&OrdId=" + hbr.OrdId;
                    var urlPath = Settings.Instance.SiteDomain + "/pdf/index" + postString;
                    HttpHelper.Get(urlPath);
                    //重新获取合同路径
                    //hx_Bid_records hbr2;//AsNoTracking().
                    hbr = ef.hx_Bid_records.AsNoTracking().Where(p => p.bid_records_id == id).FirstOrDefault();
                    if (hbr != null)
                    {
                        contractpath = hbr.contractpath;
                    }
                }
                var fileStream = new FileStream(Server.MapPath(contractpath), FileMode.Open);
                var mimeType = "application/pdf";
                var fileDownloadName = "Chuanglitou.pdf";
                return File(fileStream, mimeType, fileDownloadName);
            }
            catch (Exception ex)
            {
                //throw;
                Response.Write("请稍候重试!");
                Response.End();
            }
            return Content("");

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
                Console.WriteLine(ex);
                isOK = false;
            }
            finally
            {

            }
            return isOK;
        }
        private void First(Document doc, PdfWriter writer)
        {
            string tmp = "创利投金服平台合同";
            doc.Add(HeaderAndFooterEvent.InsertTitleContent(tmp));

            //tmp = "(正文     页,附件 0 页)";
            tmp = "(时间: " + DateTime.Now.ToString("yyyy-MM-dd") + ")";
            doc.Add(HeaderAndFooterEvent.InsertTitleContent(tmp));

            //模版 显示总共页数  
            HeaderAndFooterEvent.tpl = writer.DirectContent.CreateTemplate(100, 100); //模版的宽度和高度  
            PdfContentByte cb = writer.DirectContent;
            cb.AddTemplate(HeaderAndFooterEvent.tpl, 266, 914);//调节模版显示的位置  


        }


    }
    public class PdfDetail
    {

        public string Path { get; set; }
    }

}