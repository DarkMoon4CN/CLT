using ChuanglitouP2P.BLL;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.InitiativeTender;
using ChuanglitouP2P.Model.VeryCodes.NetCreditAssistant.Model;
using ChuangLitouP2P.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WeiXin.Controllers
{
    public class HomeController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();
        B_usercenter o = new B_usercenter();
        public ActionResult Index()
        {
            var actFanXian = ef.hx_ActivityTable.Where(c => c.ActName == "12月投资立得返现奖励").OrderByDescending(c => c.ActEndtime).FirstOrDefault();
            ViewBag.FanXianSTime = actFanXian == null ? "" : (actFanXian.ActStarttime == null ? "" : Convert.ToDateTime(actFanXian.ActStarttime).ToString("yyyy-MM-dd HH:mm:ss"));
            ViewBag.FanXianETime = actFanXian == null ? "" : (actFanXian.ActEndtime == null ? "" : Convert.ToDateTime(actFanXian.ActEndtime).ToString("yyyy-MM-dd HH:mm:ss"));

            string sql = "SELECT top 1 targetid,loan_number,borrowing_title,project_type_id,borrowing_thumbnail,annual_interest_rate,borrowing_balance,start_time,end_time,life_of_loan,unit_day,release_date,month_payment_date,repayment_date,minimum,company_name,guarantee_way_name,fundraising_amount,tender_state,sys_time ,item_details,borrower_circumstances,risk_control_measures,independent_advice,IsUse,companyid from V_borrowing_target_addlist where tender_state between  2 and  5 and project_type_id!=6 and  recommend=1 order by tender_state asc,indexorder desc,targetid desc";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            ViewBag.Recommend_Dt = dt;  //推荐项目
            ViewBag.targetid = dt != null && dt.Rows.Count > 0 ? dt.Rows[0]["targetid"].ToString() : "0";
            return View();
        }

        [HttpPost]
        public ActionResult IndexProject(int tid, int page = 1)
        {
            var pagesize = 5;
            var RecordCount = 0;
            var pagecount = 1;
            StringBuilder str = new StringBuilder();
            string TableName = "V_borrowing_target_addlist";
            string strFields = "targetid,loan_number,borrowing_title,borrowing_thumbnail,annual_interest_rate,borrowing_balance,life_of_loan,unit_day,release_date,month_payment_date,repayment_date,end_time,minimum,company_name,guarantee_way_name,fundraising_amount,tender_state,start_time,sys_time,IsUse,companyid,indexorder ";
            string fldName = "tender_state asc,indexorder desc,targetid desc";

            string strWhere = " targetid >0  and  tender_state between  2 and  5 and project_type_id != 6";

            if (tid > 0)
            {
                strWhere += " and  targetid !=" + tid.ToString();
            }

            DataTable dt = new DataTable();

            B_PublicPageList o = new B_PublicPageList();

            dt = o.GetListByPage(TableName, strFields, fldName, pagesize, page, strWhere, out RecordCount);

            //计算总页数
            pagecount = RecordCount / pagesize;
            if ((RecordCount % pagesize) > 0)
                pagecount++;
            if (page == 0)
                page = 1;

            //生成分页字符串
            // pagenumbers = CommonOperate.GetPageNumbersDiv(pageid, pagecount, "IndexProject.html", 6, "page").ToString();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int isStart = 0;
                DateTime online = DateTime.Parse(dt.Rows[i]["sys_time"].ToString());


                var actFanXian = ef.hx_ActivityTable.Where(c => c.ActName == "12月投资立得返现奖励").FirstOrDefault();
                bool isShow = actFanXian == null ? false : TActivity_Luck.GetCurJiaoBiao(Convert.ToDateTime(actFanXian.ActStarttime), Convert.ToDateTime(actFanXian.ActEndtime), online, Convert.ToInt32(dt.Rows[i]["tender_state"]), Convert.ToDateTime(dt.Rows[i]["end_time"]));

                DateTime rpdt = DateTime.Parse(dt.Rows[i]["repayment_date"].ToString());
                DateTime rest = DateTime.Parse(dt.Rows[i]["release_date"].ToString());
                long diffdays = Utils.DateDiff("Day", DateTime.Parse(rest.ToString("yyyy-MM-dd")), DateTime.Parse(rpdt.ToString("yyyy-MM-dd")));

                str.Append("<div class=\"pitem_title_main\"><a href=\"/home/ProjectDetail?targetid=" + dt.Rows[i]["targetid"].ToString() + " \">");
                str.Append("<div style=\"width:100%; border:#ccc solid 1px;-webkit-border-radius:5px;border-radius:5px;\">");
                str.Append("<div>");
                str.Append("<i style=\"float:left; margin:0px 5px; \"><strong class=\"recom_title\">" + dt.Rows[i]["borrowing_title"].ToString() + "</strong></i>");
                //if (isShow)
                //{
                //    string fanxian = "1%";
                //    if (diffdays >= 180)
                //    {
                //        fanxian = "2%";
                //    }
                //    str.Append(" <i style=\"float:right; background-color:red; color:#fff;-webkit-border-radius:5px;border-radius:5px; margin:1px 1px; padding:0px 3px; font-size:9px;\">" + fanxian + " 返现</i>");
                //}
                str.Append("</div>");
                str.Append(" <div style=\"clear: both; width: 100 %; height: 1px; \"></div>");
                str.Append("<dl>");
                DateTime Endtime = DateTime.Parse(dt.Rows[i]["end_time"].ToString());
                DateTime Stime = DateTime.Parse(dt.Rows[i]["start_time"].ToString());
                if (Stime <= online && online > DateTime.Now)
                {
                    isStart = 1;
                }
                str.Append("<dt style=\"float:left; margin-left:2%; width:37%\"><h3>" + decimal.Parse(dt.Rows[i]["annual_interest_rate"].ToString()).ToString("0.0") + "<small>%</small></h3></dt>");
                str.Append("<dd style=\"float:right; margin:13px 1% 0 0; width:58%\"><div style=\"height:100%; float:left; text-align:left; width:45% \"><p>");
                if (diffdays < 60)
                {
                    int daysr = int.Parse(diffdays.ToString());

                    if (daysr == 30 || daysr == 31)
                    {
                        str.Append("<span><img src=\"../images/w_index_biaotu_02.png\" width=\"12\" height=\"12\">&nbsp;<small>" + Utils.GetLife_of_loan(dt.Rows[i]["life_of_loan"].ToString(), dt.Rows[i]["unit_day"].ToString()) + "</small></span>");
                    }
                    else
                    {
                        str.Append("<span><img src=\"../images/w_index_biaotu_02.png\" width=\"12\" height=\"12\">&nbsp;<small>" + daysr.ToString() + "天</small></span>");
                    }
                }
                else
                {
                    str.Append("<span><img src=\"../images/w_index_biaotu_02.png\" width=\"12\" height=\"12\">&nbsp;<small>" + Utils.GetLife_of_loan(dt.Rows[i]["life_of_loan"].ToString(), dt.Rows[i]["unit_day"].ToString()) + "</small></span>");
                }
                str.Append("</p><p> &nbsp;锁定期</p></div>");
                str.Append("<div style=\"height:100%;float:right;text-align:left;width:55% \"><p>");
                decimal borrowing_balance = decimal.Parse(dt.Rows[i]["borrowing_balance"].ToString());
                decimal fundraising_amount = decimal.Parse(dt.Rows[i]["fundraising_amount"].ToString());
                decimal Percentage = fundraising_amount / borrowing_balance * 100;
                if (Percentage > 100) { Percentage = 100M; }
                decimal Difference = borrowing_balance - fundraising_amount;
                if (Difference < 0) { Difference = 0M; }
                if (dt.Rows[i]["tender_state"].ToString() == "2")
                {
                    str.Append("<span><img src=\"../images/w_index_biaotu_01.png\" width=\"12\" height=\"12\">&nbsp;<small>" + RMB.GetWebConvertdisp(Difference, 2, true) + "</small>元</span>");
                }
                else if (int.Parse(dt.Rows[i]["tender_state"].ToString()) > 3)
                {
                    str.Append("<span><img src=\"../images/w_index_biaotu_01.png\" width=\"12\" height=\"12\">&nbsp;<small>0</small>元</span>");
                }
                str.Append("</p><p>&nbsp;剩余可投</p></div></dd> ");
                str.Append("</dl><p style=\"clear:both\"></p>");
                if (isStart == 1)
                {
                    str.Append("<button>即将开始</button>");
                }
                else
                {
                    if (dt.Rows[i]["tender_state"].ToString() == "2")
                    {

                        if (DateTime.Compare(Endtime, DateTime.Now) <= 0 && Percentage < 100.00M)
                        {
                            str.Append("<button style=\"background:#b6b6b6;\">已结束</button>");
                        }
                        else if (Percentage == 100.00M)
                        {
                            str.Append("<button style=\"background:#b6b6b6;\">满标</button>");
                        }
                        else
                        {
                            str.Append("<button>立即投资</button>");
                        }
                    }
                    else if (dt.Rows[i]["tender_state"].ToString() == "3")
                    {
                        str.Append("<button style=\"background: #b6b6b6;\">满标</button>");
                    }

                    else if (dt.Rows[i]["tender_state"].ToString() == "4")
                    {
                        str.Append("<button style=\"background: #b6b6b6;\">还款中</button>");
                    }
                    else if (dt.Rows[i]["tender_state"].ToString() == "5")
                    {
                        str.Append("<button style=\"background: #b6b6b6;\">已还清</button>");
                    }
                }

                str.Append("<p></p></div></div>");
            }
            var json = "{\"html\":\"" + str.ToString().Replace("\"", "\\\"") + "\",\"totalpage\":" + pagecount + "}";
            return Content(json, "text/json");

        }

        /// <summary>
        /// 项目详情
        /// </summary>
        /// <param name="targetid"></param>
        /// <returns></returns>
        public ActionResult ProjectDetail(int targetid = 0)
        {
            string cInvitedcode = Utils.CheckSQLHtml(DNTRequest.GetString("channel"));
            if (!string.IsNullOrEmpty(cInvitedcode))
            {
                var keyValue = new Dictionary<string, string>();
                keyValue.Add("Invitedcode", cInvitedcode);
                Utils.SetInvCookie("channel", keyValue);
            }


            if (targetid < 1)
            {
                return RedirectToAction("Index");
            }
            string sql = "SELECT top 1 targetid,loan_number,borrowing_title,borrowing_thumbnail,annual_interest_rate,borrowing_balance,life_of_loan,unit_day,start_time,end_time, recommend,release_date,month_payment_date,repayment_date,minimum,maxmum,company_name,guarantee_way_name,fundraising_amount,tender_state,payment_options,G_contract_Path,sys_time,IsUse,companyid from V_borrowing_target_addlist where tender_state>=2 and targetid = " + targetid.ToString() + " order by  targetid desc";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            string sql_top5 = "select top 5 bid_records_id,username,mobile,investment_amount,invest_time from V_hx_Bid_records_borrowing_target where targetid =" + targetid + " order by  bid_records_id desc";
            DataTable dt_top5 = DbHelperSQL.GET_DataTable_List(sql_top5);

            ViewBag.Project_Dt = dt;
            ViewBag.Top5 = dt_top5;
            ViewBag.targetid = targetid;

            return View();
        }

        /// <summary>
        /// 通过投资金额    显示收益
        /// </summary>
        /// <param name="targetid"></param>
        /// <param name="investdisp"></param>
        /// <returns></returns>
        public ActionResult InvestCalculator(int id = 0, string data = "")
        {
            var targetid = id;
            string investdisp = Utils.CheckSQL(data);

            var json = @"{""amount"":""amiunt100"",""principal"":""principal102""}";

            B_borrowing_target o = new B_borrowing_target();

            M_borrowing_target m = new M_borrowing_target();

            decimal investmentamountd = decimal.Parse(investdisp);

            m = o.GetModel(targetid);


            InvestmentParameters mp = new InvestmentParameters();
            mp.Amount = investmentamountd;
            mp.Circle = m.life_of_loan;
            mp.CircleType = m.unit_day;
            mp.NominalYearRate = double.Parse(m.annual_interest_rate.ToString());
            mp.OverheadsRate = 0f;
            mp.RepaymentMode = m.payment_options;
            mp.RewardRate = 0f;
            mp.IsThirtyDayMonth = false;
            mp.InvestDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            mp.Investmentenddate = DateTime.Parse(m.repayment_date.ToString("yyyy-MM-dd"));
            mp.Payinterest = m.month_payment_date;
            mp.InvestObject = 1;


            investdisp = RMB.GetWebConvertdisp(investmentamountd, 0, true);


            decimal deci = Calculationofinterest(mp);


            string strdisp = RMB.GetWebConvertdisp(deci, 2, false);

            json = json.Replace("amiunt100", investdisp);

            json = json.Replace("principal102", strdisp);

            return Content(json, "text/json");
        }

        /// <summary>
        /// 项目详情介绍
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public ActionResult ProjectDetails(int tid = 0)
        {
            if (tid < 1)
            {
                return RedirectToAction("Index");
            }
            string minimum = "0";
            string maxmum = "0";
            B_borrowing_target_detailed o = new B_borrowing_target_detailed();
            var p = o.GetModelByTargetid(tid);
            DataTable Dt_DBCL = GetImageList(tid, 2);    //担保材料

            var StrTouZi = setContext(tid, out maxmum, out minimum);

            ViewBag.p = p;
            ViewBag.Dt_DBCL = Dt_DBCL;
            ViewBag.tid = tid;
            ViewBag.StrTouZi = StrTouZi;
            ViewBag.maxmum = maxmum;
            ViewBag.minimum = minimum;

            return View();
        }

        /// <summary>
        /// 项目投资记录 
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public ActionResult InvestmentRecord(int tid = 0, int pageid = 1, int pagesize = 12, int state = 0, decimal dif = 0, int nrt = 0, int etime = 0)
        {
            if (tid < 1)
            {
                return RedirectToAction("Index");
            }
            if (pageid == 0)
                pageid = 1;

            string TableName = "V_hx_Bid_records_borrowing_target";
            string strFields = "  bid_records_id,username,mobile,investment_amount,invest_time ";
            string fldName = "bid_records_id";
            string Sort = "desc";
            string strWhere = " targetid = " + tid.ToString() + " ";

            B_PublicPageList o = new B_PublicPageList();

            var RecordCount = 0;
            DataTable Record_Dt = o.GetPageIndexListByPage(TableName, strFields, fldName, pagesize, pageid, strWhere, Sort, out RecordCount);
            //计算总页数
            var pagecount = RecordCount / pagesize;
            if ((RecordCount % pagesize) > 0)
                pagecount++;

            ViewBag.Record_Dt = Record_Dt;
            ViewBag.pagecount = pagecount;
            ViewBag.pageid = pageid;
            ViewBag.tid = tid;
            ViewBag.state = state;
            ViewBag.dif = dif;
            ViewBag.nrt = nrt;
            ViewBag.etime = etime;

            return View();
        }

        /// <summary>
        /// 确认投资 -页面  [验证登录]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 


        public ActionResult InvestConfirm(int id = 0)
        {
            int userid = Settings.Instance.CurrentUserId;

            ///TODO 验证登录
            if (userid < 1)
            {
                return RedirectToAction("Index", "login", new { RedirectUrl = "/home/ProjectDetail?targetid=" + id });
            }
            if (id < 1 || userid == 0)
            {
                return RedirectToAction("Index");
            }
            decimal lixi = 0.00M;
            decimal investmentamount = DNTRequest.GetDecimal("investmentamount", 100M);
            B_member_table b = new B_member_table();
            M_member_table p = new M_member_table();
            int targetid = DNTRequest.GetInt("id", 0);

            Utils.UpdateUserAct();
            p = b.GetModel(userid);
            V_borrowing_target_addlist vbta = ef.V_borrowing_target_addlist.Where(c => c.targetid == targetid && c.tender_state == 2).OrderByDescending(c => c.targetid).FirstOrDefault();
            if (vbta != null)
            {
                if (vbta.borrowing_balance == vbta.fundraising_amount || vbta.tender_state > 2)
                {
                    return Content(StringAlert.Alert("该标的已满标，不能再投资!"), "text/html");
                }

                decimal Difference = Utils.GetDifference(decimal.Parse(vbta.borrowing_balance.ToString()), decimal.Parse(vbta.fundraising_amount.ToString()));

                if (investmentamount > Difference)
                {
                    return Content(StringAlert.Alert("投资金额大于标的差额，不能再投资!"), "text/html");
                }


                if (vbta.project_type_id == 6)
                {
                    return Content(StringAlert.Alert("新手标仅限app投资!"), "text/html");
                    //if (p.useridentity != 5)
                    //{
                    //    if (investmentamount > decimal.Parse(vbta.maxmum.ToString()))
                    //    {
                    //        return Content(StringAlert.Alert("投资金额超出最大可投限额!"), "text/html");

                    //    }
                    //    else
                    //    {
                    //        if (B_usercenter.GetInvestCountByUserid(userid) >= 1)
                    //        {
                    //            return Content(StringAlert.Alert("抱歉，新手体验标仅限新用户首次投资!"), "text/html");

                    //        }
                    //    }
                    //}

                    //decimal dfc = decimal.Parse(vbta.borrowing_balance.ToString()) - decimal.Parse(vbta.fundraising_amount.ToString());
                    //if (investmentamount > dfc)
                    //{

                    //    return Content(StringAlert.Alert("投资金额超出可投金额!"), "text/html");
                    //}
                    //else if (decimal.Parse(vbta.fundraising_amount.ToString()) + investmentamount > decimal.Parse(vbta.borrowing_balance.ToString()))
                    //{

                    //    return Content(StringAlert.Alert("项目已经满标，不能投资!"), "text/html");
                    //}


                }



                InvestCalc ic = new InvestCalc();
                lixi = ic.InvCalc(targetid, investmentamount);

            }
            else
            {
                return Content(StringAlert.Alert("标的信息不存在!"), "text/html");

            }
            List<hx_UserAct> UsrAct = new List<hx_UserAct>();

            //if (vbta.Isinterest == 0) //抵扣券
            //{
            //    UsrAct = ef.hx_UserAct.Where(c => c.registerid == userid && c.UseState == 0 && c.RewTypeID == 2).ToList();




            //}
            //else if (vbta.Isinterest == 1) //加息券
            //{
            //    UsrAct = ef.hx_UserAct.Where(c => c.registerid == userid && c.UseState == 0 && c.RewTypeID == 3).ToList();
            //}

            UsrAct = ef.hx_UserAct.Where(c => c.registerid == userid && c.UseState == 0).ToList();

            ActBase aBase = new ActBase();
            string limitStr = "";

            List<hx_UserAct> us = new List<hx_UserAct>();
            foreach (hx_UserAct item in UsrAct)
            {

                if (DateTime.Compare(item.AmtEndtime.Value.Date, DateTime.Now.Date) >= 0)
                {
                    List<int> limitInt = aBase.GetCanUseLimit(item.UseLifeLoan, out limitStr);

                    if (!aBase.CheckLimit(limitInt, vbta.unit_day ?? 0, vbta.life_of_loan ?? 0))
                        continue;
                    us.Add(item);
                }
            }

            //ViewBag.UsrAct = us;


            ViewBag.jiaxiCount = us.Where(w => w.RewTypeID == 3).Count().ToString();
            ViewBag.xianjinMoney = us.Where(w => w.ActID > 0 && w.RewTypeID == 2).Sum(w => w.Amt ?? 0).ToString("0.00");
            ViewBag.investmentamountd = investmentamount;
            ViewBag.lixi = lixi;
            ViewBag.countAmt = lixi + investmentamount;
            ViewBag.Username = p.username;
            ViewBag.accamt = p.available_balance;

            return View(vbta);


            ///TODO 获取用户信息
            /// 
            /// TODO 选择奖励列表
            /// 

            /*
            var Difference = 0M;
            var borrowing_balance = 0M;
            var fundraising_amount = 0M;
            var minimum = "0";
            StringBuilder str = new StringBuilder();
            string sql = "SELECT top 1 targetid,loan_number,borrowing_title,borrowing_thumbnail,annual_interest_rate,borrowing_balance,life_of_loan,unit_day,start_time,end_time, recommend,release_date,month_payment_date,repayment_date,minimum,maxmum,company_name,guarantee_way_name,fundraising_amount,tender_state,payment_options,G_contract_Path,sys_time,IsUse,companyid from V_borrowing_target_addlist where tender_state>=2 and targetid = " + id.ToString() + " order by  targetid desc";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                borrowing_balance = decimal.Parse(dt.Rows[0]["borrowing_balance"].ToString());
                fundraising_amount = decimal.Parse(dt.Rows[0]["fundraising_amount"].ToString());
                decimal Percentage = fundraising_amount / borrowing_balance * 100;
                Difference = borrowing_balance - fundraising_amount;
                minimum = decimal.Parse(dt.Rows[0]["minimum"].ToString()).ToString("0");

            }

            ViewBag.Difference = Difference;
            ViewBag.fundraising_amount = fundraising_amount;
            ViewBag.minimum = minimum;
            */
            //return View();
        }


        /// <summary>
        /// 根据类型ID获取抵扣券或加息券
        /// </summary>
        /// <param name="isinterest">券类型</param>
        /// <param name="targetid">标的ID</param>
        /// <param name="Rewardsids">选中券ID集合</param>
        [HttpPost]
        public void investUserActRewType(int isinterest, int targetid, string Rewardsids)
        {





            StringBuilder strBui = new StringBuilder();
            V_borrowing_target_addlist vbta = ef.V_borrowing_target_addlist.Where(c => c.targetid == targetid && c.tender_state == 2).OrderByDescending(c => c.targetid).FirstOrDefault();
            if (vbta != null)
            {
                int userid = Settings.Instance.CurrentUserId;
                List<hx_UserAct> UsrAct = new List<hx_UserAct>();

                if (isinterest == 0) //抵扣券
                {
                    UsrAct = ef.hx_UserAct.Where(c => c.registerid == userid && c.UseState == 0 && c.RewTypeID == 2).ToList();
                }
                else if (isinterest == 1) //加息券
                {
                    UsrAct = ef.hx_UserAct.Where(c => c.registerid == userid && c.UseState == 0 && c.RewTypeID == 3).ToList();
                }
                if (UsrAct.Count > 0)
                {
                    List<hx_UserAct> us = new List<hx_UserAct>();
                    foreach (hx_UserAct item in UsrAct)
                    {

                        if (DateTime.Compare(item.AmtEndtime.Value.Date, DateTime.Now.Date) >= 0)
                        {
                            us.Add(item);
                        }
                    }
                    strBui.Append("<table width=\"100%\" id=\"list_tb\">");
                    strBui.Append("<thead><tr>");
                    strBui.Append("<td width = \"20%\" height=\"30\" bgcolor=\"#eeeeee\"><input name = \"checkall\" id=\"checkall\" type=\"checkbox\" value=\"\" disabled=\"disabled\" /></td>");
                    strBui.Append("<td width = \"20%\" bgcolor=\"#eeeeee\">类型</td>");
                    strBui.Append("<td width = \"20%\" bgcolor=\"#eeeeee\">金额</td>");
                    strBui.Append("<td width = \"40%\" bgcolor=\"#eeeeee\">最低使用限额</td>");
                    strBui.Append("</tr></thead>");
                    strBui.Append("<tbody>");
                    ActBase aBase = new ActBase();
                    foreach (hx_UserAct item in us)
                    {
                        string limitStr = "";
                        List<int> limitInt = aBase.GetCanUseLimit(item.UseLifeLoan, out limitStr);
                        bool isbool = false;
                        if (Rewardsids.Count() > 0)
                        {
                            string[] sArray = Rewardsids.Split(',');//选中券ID集合
                            foreach (var sa in sArray)
                            {
                                if (item.UserAct == Convert.ToInt32(sa) && isbool == false)
                                {
                                    isbool = true;
                                    break;
                                }
                            }
                        }
                        if (!aBase.CheckLimit(limitInt, vbta.unit_day ?? 0, vbta.life_of_loan ?? 0))
                            continue;
                        strBui.Append("<tr>");
                        if (isbool == true)
                        {
                            strBui.Append("<td height = \"36\"><input name = \"check\" checked=\"checked\" type = \"checkbox\" value = \"" + item.UserAct + "\" /></td>");
                        }
                        else
                        {
                            strBui.Append("<td height = \"36\"><input name = \"check\" type =\"checkbox\" value = \"" + item.UserAct + "\" /></td>");
                        }
                        if (item.RewTypeID == 2)//抵扣券
                        {
                            //strBui.Append("<tr>");
                            //if (Rewardsids.Count() > 0)
                            //{
                            //string[] sArray = Rewardsids.Split(',');//选中券ID集合
                            //bool isbool = false;
                            //foreach (var sa in sArray)
                            //{
                            //    if (item.UserAct == Convert.ToInt32(sa) && isbool == false)
                            //    {
                            //        isbool = true;
                            //        break;
                            //    }
                            //}
                            //if (isbool == true)
                            //{
                            //    strBui.Append("<td height = \"36\"><input name = \"check\" checked=\"checked\" type = \"checkbox\" value = \"" + item.UserAct + "\" /></td>");
                            //}
                            //else
                            //{
                            //    strBui.Append("<td height = \"36\"><input name = \"check\" type = \"checkbox\" value = \"" + item.UserAct + "\" /></td>");
                            //}
                            //}
                            //else
                            //{
                            //    strBui.Append("<td height = \"36\"><input name = \"check\" type = \"checkbox\" value = \"" + item.UserAct + "\" /></td>");
                            //}
                            strBui.Append("<td>抵扣券<input id = \"u" + item.UserAct + "\" name = \"u" + item.UserAct + "\" type = \"hidden\" value = \"" + item.AmtUses + "\" /></td>");
                            strBui.Append("<td>" + item.Amt + "元</td>");
                            strBui.Append("<td>");
                            if (item.Uselower <= 0) { strBui.Append("无投资门槛"); }
                            else { strBui.Append(item.Uselower); }
                            strBui.Append("</td>");
                        }
                        else if (item.RewTypeID == 3) //加息券
                        {
                            strBui.Append("<td>加息券<input id = \"u" + item.UserAct + "\" name = \"u" + item.UserAct + "\" type = \"hidden\" value = \"" + item.AmtUses + "\" /></td>");
                            strBui.Append("<td>" + item.Amt + "%</td>");
                            strBui.Append("<td>");
                            if (item.Uselower <= 0) { strBui.Append("无投资门槛"); }
                            else { strBui.Append(item.Uselower); }
                            strBui.Append("</td>");
                            //bool isbool = false;
                            //if (Rewardsids.Count() > 0)
                            //{
                            //    string[] sArray = Rewardsids.Split(',');//选中券ID集合
                            //    foreach (var sa in sArray)
                            //    {
                            //        if (item.UserAct == Convert.ToInt32(sa) && isbool == false)
                            //        {
                            //            isbool = true;
                            //            break;
                            //        }
                            //    }
                            //}
                            //if (vbta.unit_day == 1 && vbta.life_of_loan == 3 && item.Amt.ToString() == "1.00")//三月标
                            //{
                            //strBui.Append("<tr>");
                            //if (isbool == true)
                            //{
                            //    strBui.Append("<td height = \"36\"><input name = \"check\" checked=\"checked\" type = \"checkbox\" value = \"" + item.UserAct + "\" /></td>");
                            //}
                            //else
                            //{
                            //    strBui.Append("<td height = \"36\"><input name = \"check\" type =\"checkbox\" value = \"" + item.UserAct + "\" /></td>");
                            //}

                            //strBui.Append("<td>加息券<input id = \"u" + item.UserAct + "\" name = \"u" + item.UserAct + "\" type = \"hidden\" value = \"" + item.AmtUses + "\" /></td>");
                            //strBui.Append("<td>" + item.Amt + "%</td>");
                            //strBui.Append("<td>");
                            //if (item.Uselower <= 0) { strBui.Append("无投资门槛"); }
                            //else { strBui.Append(item.Uselower); }
                            //strBui.Append("</td>");
                            //strBui.Append("</tr>");
                            //}
                            //else if (vbta.unit_day == 1 && vbta.life_of_loan == 6 && item.Amt.ToString() == "2.00")//6月标
                            //{
                            //strBui.Append("<tr>");
                            //if (isbool == true)
                            //{
                            //    strBui.Append("<td height = \"36\"><input name = \"check\" checked=\"checked\" type = \"checkbox\" value = \"" + item.UserAct + "\" /></td>");
                            //}
                            //else
                            //{
                            //    strBui.Append("<td height = \"36\"><input name = \"check\" type =\"checkbox\" value = \"" + item.UserAct + "\" /></td>");
                            //}
                            //strBui.Append("<td>加息券<input id = \"u" + item.UserAct + "\" name = \"u" + item.UserAct + "\" type = \"hidden\" value = \"" + item.AmtUses + "\" /></td>");
                            //strBui.Append("<td>" + item.Amt + "%</td>");
                            //strBui.Append("<td>");
                            //if (item.Uselower <= 0) { strBui.Append("无投资门槛"); }
                            //else { strBui.Append(item.Uselower); }
                            //strBui.Append("</td>");
                            //strBui.Append("</tr>");
                            //}
                            //else
                            //{
                            //    if (item.Amt.ToString() == "3.00")
                            //    {
                            //strBui.Append("<tr>");
                            //if (isbool == true)
                            //{
                            //    strBui.Append("<td height = \"36\"><input name = \"check\" checked=\"checked\" type = \"checkbox\" value = \"" + item.UserAct + "\" /></td>");
                            //}
                            //else
                            //{
                            //    strBui.Append("<td height = \"36\"><input name = \"check\" type =\"checkbox\" value = \"" + item.UserAct + "\" /></td>");
                            //}
                            //strBui.Append("<td>加息券<input id = \"u" + item.UserAct + "\" name = \"u" + item.UserAct + "\" type = \"hidden\" value = \"" + item.AmtUses + "\" /></td>");
                            //strBui.Append("<td>" + item.Amt + "%</td>");
                            //strBui.Append("<td>");
                            //if (item.Uselower <= 0) { strBui.Append("无投资门槛"); }
                            //else { strBui.Append(item.Uselower); }
                            //strBui.Append("</td>");
                            //strBui.Append("</tr>");
                            //}
                            //else
                            //{
                            //strBui.Append("<td>-</td>");
                            //}
                            //}
                        }
                    }
                    strBui.Append("</tr>");
                    strBui.Append("</tbody>");
                    strBui.Append("</table>");
                }
            }
            else
            {
                strBui.Append("标的信息不存在!");
            }
            Response.Write(strBui.ToString());
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            ViewBag.rndstr = Utils.RndNumChar(10).ToString();
            return View();
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        public ActionResult DoLogin()
        {
            string json = "";
            var mobile = Utils.CheckSQLHtml(DNTRequest.GetString("mobile").Trim());
            string userpassword = DESEncrypt.Encrypt(Utils.CheckSQLHtml(DNTRequest.GetString("userpassword").Trim()), ConfigurationManager.AppSettings["webp"].ToString());
            string Validatecode = Utils.CheckSQLHtml(DNTRequest.GetString("vcodec").Trim());
            if (Session["LoginValidateCode"] == null || Session["LoginValidateCode"].ToString() != Validatecode)
            {
                json = @" {""rs""    : ""y"", ""error""      :  ""验证码过期!""}";
                return Content(json, "text/json");
            }
            var userInfo = ef.hx_member_table.Where(a => a.mobile == mobile || a.username == mobile).SingleOrDefault();
            if (userInfo == null || userInfo.registerid < 1)
            {
                json = @" {""rs""    : ""n"", ""error""      :  ""该手机号未注册用户!""}";
                return Content(json, "text/json");
            }
            string ip = Utils.GetRealIP();
            if (userInfo.password != userpassword)
            {
                #region 登录失败
                try
                {
                    hx_td_usrlogininfo usrmode = new hx_td_usrlogininfo();
                    usrmode.logintime = DateTime.Now;
                    usrmode.Loginusrname = userInfo.username;
                    usrmode.loginusrpass = "********";
                    usrmode.registerid = userInfo.registerid;
                    usrmode.loginIP = ip;
                    usrmode.logincity = GetIpToCity.GetAddressByIp(ip);
                    usrmode.loginsource = 0;
                    usrmode.loginstate = 1;
                    ef.hx_td_usrlogininfo.Add(usrmode);
                    int ie = ef.SaveChanges();
                }
                catch { }
                #endregion

                json = @" {""rs""    : ""n"", ""error""      :  ""用户名或密码错误!""}";
                return Content(json, "text/json");
            }
            M_login mlogin = new M_login();
            mlogin.userid = userInfo.registerid;
            mlogin.username = userInfo.username;
            mlogin.codeno = Utils.SetSessioncode();
            mlogin.UsrName = userInfo.realname;

            if (Utils.LoginWriteSession(mlogin, 0) > 0)
            {
                // string sql = "update hx_member_table set  LoginNum=LoginNum+1,lastlogintime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',lastloginIP='" + ip + "' where registerid=" + userid.ToString();

                ///LogInfo.WriteLog("登录信息更新" + sql);


                ///  DbHelperSQL.ExecuteSql(sql);

                #region 登录成功
                try
                {
                    hx_td_usrlogininfo usrmode = new hx_td_usrlogininfo();
                    usrmode.logintime = DateTime.Now;
                    usrmode.Loginusrname = userInfo.username;
                    usrmode.loginusrpass = "********";
                    usrmode.registerid = userInfo.registerid;
                    usrmode.loginIP = ip;
                    usrmode.logincity = GetIpToCity.GetAddressByIp(ip);
                    usrmode.loginsource = 0;
                    usrmode.loginstate = 0;
                    ef.hx_td_usrlogininfo.Add(usrmode);
                    int ie = ef.SaveChanges();
                }
                catch (Exception tx)
                {

                    throw (tx);
                }
                #endregion



                if (Session["returnpage"] != null)
                {
                    json = @"{""rs""    : ""y"", ""url""      :  ""/""}";
                    //  json = json.Replace("/", Session["returnpage"].ToString());
                    Session["returnpage"] = null;
                }
                else
                {
                    json = @" {""rs""    : ""y"", ""url""      :  ""/""}";
                }
                return Content(json, "text/json");
            }

            json = @" {""rs""    : ""n"", ""error""      :  ""用户名或密码错误!""}";
            return Content(json, "text/json");

        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }


        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetValidateCode()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(4);
            Session["LoginValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        #region 私有方法

        private decimal Calculationofinterest(InvestmentParameters Minverst)
        {
            decimal dec = 0.00M;

            InvestmentParameters inverstParas = new InvestmentParameters
            {
                Amount = Minverst.Amount, //投资金额
                Circle = Minverst.Circle,//期限
                CircleType = Minverst.CircleType,//期限类型 月 / 天
                NominalYearRate = Minverst.NominalYearRate,//年利率
                OverheadsRate = Minverst.OverheadsRate,//管理费率
                RepaymentMode = Minverst.RepaymentMode,//还款方式
                RewardRate = Minverst.RewardRate,//奖励比例
                InvestDate = Minverst.InvestDate,
                IsThirtyDayMonth = false,
                Investmentenddate = Minverst.Investmentenddate,
                Payinterest = Minverst.Payinterest,
                InvestObject = Minverst.InvestObject

            };

            InvestmentTotalIncome income = ChuanglitouP2P.Bll.VeryCodes.NetCreditAssistant.BLL.InvestCalculator.CalculateTotalIncome(inverstParas);

            List<InvestmentReceiveRecordInfo> records = income.ReceiveRecords;

            foreach (InvestmentReceiveRecordInfo p in records)
            {

                dec = dec + p.Interest;
            }


            //  dec = dec + Minverst.Amount;  //投资总收益


            return dec;


        }

        private string setContext(int id, out string maxmum, out string minimum)
        {
            maxmum = "0";
            minimum = "0";
            var tender_state = 0;
            DateTime Endtime = DateTime.Now;
            var borrowing_balance = 0M;
            var fundraising_amount = 0M;
            var Difference = 0M;
            var pagetitle = "";
            //var minimum = "0";
            //var maxmum = "0";
            var notstart = 0;

            StringBuilder str = new StringBuilder();
            string sql = "SELECT top 1 targetid,loan_number,borrowing_title,borrowing_thumbnail,annual_interest_rate,borrowing_balance,life_of_loan,unit_day,start_time,end_time, recommend,release_date,month_payment_date,repayment_date,minimum,maxmum,company_name,guarantee_way_name,fundraising_amount,tender_state,payment_options,G_contract_Path,sys_time,IsUse,companyid from V_borrowing_target_addlist where tender_state>=2 and targetid = " + id.ToString() + " order by  targetid desc";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {

                tender_state = int.Parse(dt.Rows[0]["tender_state"].ToString());

                DateTime Stime = DateTime.Parse(dt.Rows[0]["start_time"].ToString());
                DateTime online = DateTime.Parse(dt.Rows[0]["sys_time"].ToString());
                Endtime = DateTime.Parse(dt.Rows[0]["end_time"].ToString());


                borrowing_balance = decimal.Parse(dt.Rows[0]["borrowing_balance"].ToString());
                fundraising_amount = decimal.Parse(dt.Rows[0]["fundraising_amount"].ToString());
                decimal Percentage = fundraising_amount / borrowing_balance * 100;
                Difference = borrowing_balance - fundraising_amount;


                pagetitle = dt.Rows[0]["borrowing_title"].ToString() + "--创利投P2B网络借贷金融平台";



                minimum = decimal.Parse(dt.Rows[0]["minimum"].ToString()).ToString("0");

                maxmum = decimal.Parse(dt.Rows[0]["maxmum"].ToString()).ToString("0");

                if (Stime < online && online > DateTime.Now)
                {
                    notstart = 1;


                }



                DateTime rpdt = DateTime.Parse(dt.Rows[0]["repayment_date"].ToString());

                DateTime rest = DateTime.Parse(dt.Rows[0]["release_date"].ToString());

                long diffdays = Utils.DateDiff("Day", DateTime.Parse(rest.ToString("yyyy-MM-dd")), DateTime.Parse(rpdt.ToString("yyyy-MM-dd")));
                if (diffdays < 60)
                {
                    int daysr = int.Parse(diffdays.ToString());


                }
                else
                {
                }




            }


            str.Clear();

            string stc = "";

            if (tender_state == 2)
            {

                if (Endtime > DateTime.Now)
                {
                    if (Difference == 0)
                    {
                        stc = "<div class=\"touzi_btn\">  <a href=\"javascript:void(0)\">满标</a></div>";
                    }
                    else
                    {
                        if (notstart == 0)
                        {
                            //  stc="<div class=\"touzi_btn\">  <a href=\"/invest_borrow_"+targetid +"_confirm.html\">立即投资</a></div>";
                            stc = "<div class=\"touzi_btn\">  <a href=\"/home/InvestConfirm?id=" + id.ToString() + "\"  id=\"touzi_btn11111\">立即投资</a></div>";
                        }
                        else
                        {
                            stc = "<div class=\"touzi_btn\">  <a href=\"javascript:void(0)\">未开始</a></div>";
                        }


                    }


                }
                else
                {
                    stc = "<div class=\"touzi_btn\">  <a href=\"javascript:void(0)\" class=\"gry\">项目已结束</a></div>";
                }





            }
            else if (tender_state == 3)
            {
                stc = "<div class=\"touzi_btn\">  <a href=\"javascript:void(0)\">满标</a></div>";
            }
            else if (tender_state == 4)
            {
                stc = "<div class=\"touzi_btn\">  <a href=\"javascript:void(0)\">还款中</a></div>";
            }
            else if (tender_state == 5)
            {
                stc = "<div class=\"touzi_btn\">  <a href=\"javascript:void(0)\">已还清</a></div>";

            }
            return stc;

        }

        private DataTable GetImageList(int targetid, int type_picture)
        {
            StringBuilder str = new StringBuilder();
            string sql = "SELECT borrower_guarantor_picture_id,picture_path,picture_name from hx_borrower_guarantor_picture where targetid=" + targetid.ToString() + " and type_picture=" + type_picture.ToString();
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            /*
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                str.Append("<li><a href=\"" + dt.Rows[i]["picture_path"].ToString() + "\" title=\"" + dt.Rows[i]["picture_name"].ToString() + "\" data-gallery=\"\"><img src=\"" + dt.Rows[i]["picture_path"].ToString() + "\" /> </a>" + dt.Rows[i]["picture_name"].ToString() + "</li>");

            }

            return str.ToString();*/

            return dt;

        }

        #endregion




        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult PostInvest()
        {
            int userid = Settings.Instance.CurrentUserId;
            StringBuilder strz = new StringBuilder();
            decimal lixi = 0.00M;
            decimal investmentamountd = DNTRequest.GetDecimal("investamount", 100M);
            int Isinterest = DNTRequest.GetInt("Isinterest", -1);
            decimal jiaxi = 0.00M; //加息点数

            B_member_table bu = new B_member_table();
            M_member_table pu = new M_member_table();
            pu = bu.GetModel(userid);
            int targetid = DNTRequest.GetInt("id", 0);
            string Rewardsids = DNTRequest.GetString("Rewardsid");


            decimal vocheramttemp = GetUseRewards(Rewardsids, userid);


            if (Isinterest < 0 || Isinterest > 1)
            {
                return Content(StringAlert.Alert("数据有误!"), "text/html");
            }

            if (Isinterest == 0)  //代金券
            {

            }
            else if (Isinterest == 1) //加息券
            {
                string[] sArray = Rewardsids.Split(',');

                if (sArray.Length > 1)
                {
                    return Content(StringAlert.Alert("加息券使用有误!", "/"), "text/html");

                }

                jiaxi = vocheramttemp;
                vocheramttemp = 0.00M;
            }

            if (investmentamountd > pu.available_balance + vocheramttemp)
            {
                return Content(StringAlert.Alert("帐户余额不足，请充值!", "/usercenter/Recharge"), "text/html");

            }

            string sql = "SELECT top 1 targetid,borrower_registerid,loan_number,borrowing_title,borrowing_thumbnail,annual_interest_rate,borrowing_balance,life_of_loan,unit_day,release_date,month_payment_date,repayment_date,minimum,maxmum,company_name,guarantee_way_name,fundraising_amount,tender_state,payment_options,loan_management_fee,username,realname,BorrUsrCustId,project_type_id from V_borrowing_target_addlist where tender_state>=2 and targetid = " + targetid.ToString() + " order by  targetid desc";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["project_type_id"].ToString() == "6")
                {
                    return Content(StringAlert.Alert("新手标仅限app投资!"), "text/html");
                }

                if (dt.Rows[0]["borrowing_balance"].ToString() == dt.Rows[0]["fundraising_amount"].ToString() || int.Parse(dt.Rows[0]["tender_state"].ToString()) > 2)
                {
                    return Content(StringAlert.Alert("该标的已满标，不能再投资!"), "text/html");
                }

                decimal Difference = Utils.GetDifference(decimal.Parse(dt.Rows[0]["borrowing_balance"].ToString()), decimal.Parse(dt.Rows[0]["fundraising_amount"].ToString()));

                if (investmentamountd > Difference)
                {
                    return Content(StringAlert.Alert("投资金额大于标的差额，不能再投资!"), "text/html");
                }


                if (GetBidReCount(int.Parse(dt.Rows[0]["targetid"].ToString()), userid) >= 3)
                {
                    return Content(StringAlert.Alert("本标的投资未付款已超过三次，如有疑问可咨询创利投客服010-53732056!"), "text/html");
                }

                if (decimal.Parse(dt.Rows[0]["fundraising_amount"].ToString()) + investmentamountd > decimal.Parse(dt.Rows[0]["borrowing_balance"].ToString()))
                {
                    return Content(StringAlert.Alert("项目已经满标，不能投资!"), "text/html");
                }

                InvestmentParameters mp = new InvestmentParameters();
                mp.Amount = investmentamountd;
                mp.Circle = int.Parse(dt.Rows[0]["life_of_loan"].ToString());
                mp.CircleType = int.Parse(dt.Rows[0]["unit_day"].ToString());
                mp.NominalYearRate = double.Parse(dt.Rows[0]["annual_interest_rate"].ToString()) + double.Parse(jiaxi.ToString());
                mp.OverheadsRate = 0f;
                mp.RepaymentMode = int.Parse(dt.Rows[0]["payment_options"].ToString());
                mp.RewardRate = 0f;
                mp.IsThirtyDayMonth = false;

                mp.InvestDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                mp.ReleaseDate = DateTime.Parse(dt.Rows[0]["release_date"].ToString()).ToString("yyyy-MM-dd");//新加借款日期字段
                mp.Investmentenddate = DateTime.Parse(DateTime.Parse(dt.Rows[0]["repayment_date"].ToString()).ToString("yyyy-MM-dd"));
                mp.Payinterest = int.Parse(dt.Rows[0]["month_payment_date"].ToString());
                mp.InvestObject = 1;
                List<InvestmentReceiveRecordInfo> records = ChuanglitouP2P.Bll.VeryCodes.NetCreditAssistant.BLL.InvestCalculator.CalculateReceiveRecord(mp);
                B_Bid_records o = new B_Bid_records();
                M_Bid_records p = new M_Bid_records();

                ///需要生成订单号哦 为以后放款做准备

                p.borrower_registerid = int.Parse(dt.Rows[0]["borrower_registerid"].ToString());
                p.targetid = int.Parse(dt.Rows[0]["targetid"].ToString());
                p.loan_number = decimal.Parse(dt.Rows[0]["loan_number"].ToString());
                p.annual_interest_rate = decimal.Parse(dt.Rows[0]["annual_interest_rate"].ToString());
                p.current_period = records.Count;
                p.investment_amount = investmentamountd;
                p.value_date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));  //这里应该是日 整型需要确认[这个是需求后来变动改的，字段暂时没什么用处]
                p.investment_maturity = DateTime.Parse(dt.Rows[0]["repayment_date"].ToString());
                p.invest_time = DateTime.Now;
                p.invest_state = 1;
                p.flow_return = 1;
                p.repayment_amount = 0.00M;
                p.repayment_period = DateTime.Parse(dt.Rows[0]["repayment_date"].ToString());
                p.investor_registerid = userid;
                p.payment_status = 0; // 还款支付状态为0


                InvestCalc ic = new InvestCalc();
                lixi = ic.InvCalc(targetid, investmentamountd, double.Parse(jiaxi.ToString()));

                p.withoutinterest = lixi;
                p.OrdId = decimal.Parse(Utils.Createcode());
                p.JiaxiNum = jiaxi;
                p.BonusAmt = vocheramttemp;





                string invcode = "";  //这里需要加入邀请码 业务逻业，查询邀请获得邀请的用户

                // invcode = B_usercenter.GetUserInvCode(userid.ToString());

                string sqlcode = "select top 1 invcode from hx_td_Userinvitation where  invpersonid=" + userid + "  order by invitationid asc ";
                DataTable dat = DbHelperSQL.GET_DataTable_List(sqlcode);
                if (dat.Rows.Count > 0)
                {
                    p.invitationcode = dat.Rows[0]["invcode"].ToString();
                }
                else
                {
                    p.invitationcode = "";
                }


                int bid_records_id = o.Add(p);
                if (bid_records_id > 0)
                {

                    B_income_statement b1 = new B_income_statement();
                    M_income_statement m1 = new M_income_statement();

                    B_member_table uo = new B_member_table();
                    M_member_table up = new M_member_table();
                    up = uo.GetModel(p.investor_registerid);
                    int i = 1;
                    foreach (InvestmentReceiveRecordInfo pr in records)
                    {
                        m1.targetid = p.targetid;

                        m1.bid_records_id = bid_records_id;

                        m1.loan_number = p.loan_number;
                        m1.borrower_registerid = p.borrower_registerid;
                        m1.OutCustId = dt.Rows[0]["BorrUsrCustId"].ToString();

                        //加息券处理

                        m1.annual_revenue = p.annual_interest_rate;

                        m1.investment_amount = p.investment_amount;
                        m1.invest_time = p.invest_time;
                        m1.current_investment_period = i;
                        m1.value_date = DateTime.Parse(pr.interestvalue_date.ToString("yyyy-MM-dd"));
                        m1.interest_payment_date = DateTime.Parse(pr.NominalReceiveDate.ToString("yyyy-MM-dd"));
                        m1.repayment_amount = pr.Balance;
                        m1.investor_registerid = p.investor_registerid;
                        m1.InCustId = up.UsrCustId;   //调出投资用户客户id
                        m1.payment_status = 0;  //还款款成功
                        m1.interestpayment = pr.Interest;

                        m1.BidOrderid = p.OrdId;
                        m1.Principal = pr.Principal;
                        m1.TotalInstallments = pr.TotalInstallments;
                        m1.interestDay = pr.TotalDays;

                        if (b1.Add(m1) <= 0)
                        {
                            break;
                        }

                        i = i + 1;

                    }

                    M_InitiativeTender Mt = new M_InitiativeTender();
                    Mt.Version = "20";
                    Mt.CmdId = "InitiativeTender";
                    Mt.MerCustId = Utils.GetMerCustID();
                    Mt.OrdId = p.OrdId.ToString();
                    Mt.OrdDate = p.invest_time.ToString("yyyyMMdd");
                    Mt.TransAmt = p.investment_amount.ToString("0.00");
                    Mt.UsrCustId = up.UsrCustId;
                    Mt.MaxTenderRate = "0.20";



                    TenderJosnPro mtp = new TenderJosnPro();
                    mtp.BorrowerCustId = dt.Rows[0]["BorrUsrCustId"].ToString();
                    //mtp.BorrowerAmt =decimal.Parse(dt.Rows[0]["borrowing_balance"].ToString()).ToString("0.00");
                    mtp.BorrowerAmt = p.investment_amount.ToString("0.00");

                    // mtp.BorrowerRate = decimal.Parse( dt.Rows[0]["loan_management_fee"].ToString()).ToString("0.00");

                    mtp.BorrowerRate = "1.00"; //风控范围

                    mtp.ProId = dt.Rows[0]["targetid"].ToString();

                    Mt.BorrowerDetails = "[" + FastJSON.toJOSN(mtp) + "]";


                    #region 此处判断优惠类型

                    #endregion

                    if (Isinterest == 0)  //代金券
                    {
                        TenderAccPro ret = new TenderAccPro();
                        ret.AcctId = Utils.GetMERDT();
                        ret.VocherAmt = vocheramttemp.ToString("0.00");

                        if (Rewardsids.Length > 0)
                        {
                            Mt.ReqExt = "{" + "\"Vocher\":" + FastJSON.toJOSN(ret) + "}";
                        }
                    }

                    //冻结记录写入-------
                    M_td_frozen mf = new M_td_frozen();
                    B_td_frozen bf = new B_td_frozen();

                    mf.MBT_Registerid = p.investor_registerid;
                    mf.targetid = int.Parse(dt.Rows[0]["targetid"].ToString());
                    mf.FrozenState = 0;
                    mf.FrozenidNo = Utils.Createcode();
                    if (Isinterest == 0)  //代金券
                    {
                        mf.FrozenidAmount = RMB.GetDecimal(p.investment_amount - vocheramttemp, 2, true);
                    }
                    else if (Isinterest == 1)
                    {
                        mf.FrozenidAmount = RMB.GetDecimal(p.investment_amount, 2, true);
                    }
                    mf.FrozenDate = DateTime.Now;
                    mf.UsrCustId = up.UsrCustId;
                    mf.bid_records_id = bid_records_id;
                    bf.Add(mf);
                    //冻结记录写入结束-------

                    Mt.IsFreeze = "Y";
                    Mt.FreezeOrdId = mf.FrozenidNo;

                    Mt.RetUrl = Utils.GetRe_url("Home/investment_success/" + targetid.ToString());

                    //Mt.RetUrl = "http://localhost:17745/investment_success_" + targetid.ToString() + ".html";


                    Mt.BgRetUrl = Utils.GetRe_url("Thirdparty/BG_investment_success");

                    //Mt.BgRetUrl = Utils.GetRe_url("666Thirdparty/BG_investment_success");

                    Mt.MerPriv = Rewardsids;


                    LogInfo.WriteLog("优惠券使用的id:" + Rewardsids);


                    StringBuilder chkVal = new StringBuilder();
                    chkVal.Append(Mt.Version);
                    chkVal.Append(Mt.CmdId);
                    chkVal.Append(Mt.MerCustId);
                    chkVal.Append(Mt.OrdId);
                    chkVal.Append(Mt.OrdDate);
                    chkVal.Append(Mt.TransAmt);
                    chkVal.Append(Mt.UsrCustId);
                    chkVal.Append(Mt.MaxTenderRate);
                    chkVal.Append(Mt.BorrowerDetails);
                    chkVal.Append(Mt.IsFreeze);
                    chkVal.Append(Mt.FreezeOrdId);
                    chkVal.Append(Mt.RetUrl);
                    chkVal.Append(Mt.BgRetUrl);
                    chkVal.Append(Mt.MerPriv);
                    chkVal.Append(Mt.ReqExt);
                    string chkv = chkVal.ToString();
                    LogInfo.WriteLog(chkv);

                    //私钥文件的位置(这里是放在了站点的根目录下)
                    string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
                    //需要指定提交字符串的长度
                    int len = Encoding.UTF8.GetBytes(chkv).Length;
                    StringBuilder sbChkValue = new StringBuilder(256);
                    //加签
                    int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

                    LogInfo.WriteLog(str.ToString());

                    Mt.ChkValue = sbChkValue.ToString();

                    if (str == 0)
                    {
                        //这里需要加入数据优惠或抵扣券处理业务

                        if (Rewardsids.Length > 0)
                        {
                            sql = "update hx_UserAct set UseState=3,AmtProid=" + bid_records_id.ToString() + " where UserAct in (" + Rewardsids + ") and UseState=0  and registerid=" + userid;

                            LogInfo.WriteLog("锁定优惠券sql:" + sql);
                            DbHelperSQL.RunSql(sql);
                        }




                        strz.Append(" <form id=\"formauto\" name=\"formauto\"  action=\"" + Utils.GetChinapnrUrl() + "\" method=\"post\">");

                        strz.Append("<input id=\"Version\"  name=\"Version\"  type=\"hidden\"  value=\"" + Mt.Version + "\" />");

                        strz.Append("<input id=\"CmdId\"  name=\"CmdId\"    type=\"hidden\"  value=\"" + Mt.CmdId + "\" />");

                        strz.Append("<input id=\"MerCustId\" name=\"MerCustId\"   type=\"hidden\"  value=\"" + Mt.MerCustId + "\" />");

                        strz.Append("<input id=\"OrdId\" name=\"OrdId\" type=\"hidden\"  value=\"" + Mt.OrdId + "\" />");

                        strz.Append("<input id=\"OrdDate\" name=\"OrdDate\" type=\"hidden\"  value=\"" + Mt.OrdDate + "\" />");

                        strz.Append("<input id=\"TransAmt\" name=\"TransAmt\" type=\"hidden\"  value=\"" + Mt.TransAmt + "\" />");

                        strz.Append("<input id=\"UsrCustId\"  name=\"UsrCustId\" type=\"hidden\"  value=\"" + Mt.UsrCustId + "\" />");

                        strz.Append("<input id=\"MaxTenderRate\"   name=\"MaxTenderRate\" type=\"hidden\"  value=\"" + Mt.MaxTenderRate + "\" />");

                        strz.Append("<input id=\"BorrowerDetails\" name=\"BorrowerDetails\" type=\"hidden\"  value=" + Mt.BorrowerDetails + " />");

                        strz.Append("<input id=\"IsFreeze\" name=\"IsFreeze\" type=\"hidden\"  value=\"" + Mt.IsFreeze + "\" />");
                        strz.Append("<input id=\"FreezeOrdId\" name=\"FreezeOrdId\" type=\"hidden\"  value=\"" + Mt.FreezeOrdId + "\" />");
                        strz.Append("<input id=\"RetUrl\" name=\"RetUrl\" type=\"hidden\"  value=\"" + Mt.RetUrl + "\" />");
                        strz.Append("<input id=\"BgRetUrl\" name=\"BgRetUrl\" type=\"hidden\"  value=\"" + Mt.BgRetUrl + "\" />");
                        strz.Append("<input id=\"MerPriv\" name=\"MerPriv\" type=\"hidden\"  value=\"" + Mt.MerPriv + "\" />");
                        strz.Append("<input id=\"ReqExt\" name=\"ReqExt\" type=\"hidden\"  value=" + Mt.ReqExt + " >");

                        strz.Append("<input id=\"ChkValue\" name=\"ChkValue\" type=\"hidden\"  value=\"" + Mt.ChkValue + "\" />");
                        strz.Append(" </form>");
                        strz.Append("<script type=\"text/javascript\">document.getElementById('formauto').submit();</script>");

                        LogInfo.WriteLog(strz.ToString());



                    }
                    else
                    {
                        return Content(StringAlert.Alert("数据有误，签名失败!", "/"), "text/html");
                    }

                }

            }


            ViewBag.strz = strz.ToString();

            return View();
        }


        #region 获取本次使用奖励金额 +decimal GetUseRewards(string strid, int userid)
        /// <summary>
        /// 获取本次使用奖励金额
        /// </summary>
        /// <param name="strid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        private decimal GetUseRewards(string strid, int userid)
        {
            decimal dec = 0.00M;

            if (strid.Length > 0)
            {
                // string sql = "SELECT  COALESCE(SUM( amount_of_reward),0.00) as  amount_of_reward  from bonus_account  where reward_state=0 and  bonus_account_id in (" + strid + ")  and membertable_registerid=" + userid.ToString();
                string sql = "SELECT  COALESCE(SUM(amt),0.00) as  amount_of_reward  from hx_UserAct  where UseState=0 and  UserAct in (" + strid + ")  and registerid=" + userid.ToString();
                LogInfo.WriteLog("查询奖励语句:" + sql);
                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                dec = decimal.Parse(dt.Rows[0][0].ToString());
            }
            return dec;
        }

        #endregion


        #region 统计投资用户同一标的投资失败次数 +GetBidReCount(int targetid, int investor_registerid)
        /// <summary>
        /// 统计投资用户同一标的投资失败次数
        /// </summary>
        /// <param name="targetid"></param>
        /// <param name="investor_registerid"></param>
        /// <returns></returns>
        public int GetBidReCount(int targetid, int investor_registerid)
        {
            string sql = "SELECT COALESCE(count(ordstate),0) as vicount from hx_Bid_records where targetid=" + targetid + " and investor_registerid=" + investor_registerid + " and ordstate=0";

            return int.Parse(DbHelperSQL.Re_String(sql));
        }
        #endregion




        /// <summary>
        /// 计算页面上显示的利息
        /// </summary>
        /// <param name="targetid"></param>
        /// <param name="investmentamountd"></param>
        /// <param name="jiaxi"></param>
        /// <returns></returns>
        public ActionResult jixi(int targetid, decimal investmentamountd, string jiaxi)
        {
            InvestCalc ic = new InvestCalc();
            decimal lixi = ic.InvCalc(targetid, investmentamountd, double.Parse(jiaxi.ToString()));
            return Content(lixi.ToString());
        }



        /// <summary>
        /// 投资成功，汇付回调页面
        /// </summary>
        /// <returns></returns>
        public ActionResult investment_success()
        {
            ReInitiativeTender p = new ReInitiativeTender();
            int id = DNTRequest.GetInt("id", 0);
            id = DNTRequest.GetInt("id", 0);
            p.CmdId = DNTRequest.GetString("CmdId");
            p.RespCode = DNTRequest.GetString("RespCode");
            p.RespDesc = HttpUtility.UrlDecode(DNTRequest.GetString("RespDesc"));
            p.MerCustId = DNTRequest.GetString("MerCustId");
            p.OrdId = DNTRequest.GetString("OrdId");
            p.OrdDate = DNTRequest.GetString("OrdDate");
            p.TransAmt = DNTRequest.GetString("TransAmt");
            p.UsrCustId = DNTRequest.GetString("UsrCustId");
            p.TrxId = DNTRequest.GetString("TrxId");
            p.IsFreeze = DNTRequest.GetString("IsFreeze");
            p.FreezeOrdId = DNTRequest.GetString("FreezeOrdId");
            p.FreezeTrxId = DNTRequest.GetString("FreezeTrxId");
            p.RetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("RetUrl"));
            p.BgRetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("BgRetUrl"));
            string merp = DNTRequest.GetString("MerPriv");
            if (merp.Length > 0)
            {
                p.MerPriv = HttpUtility.UrlDecode(merp);
            }
            else
            {
                p.MerPriv = merp;
            }
            p.RespExt = HttpUtility.UrlDecode(DNTRequest.GetString("RespExt"));
            p.ChkValue = DNTRequest.GetString("ChkValue");
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(p.CmdId);
            chkVal.Append(p.RespCode);
            chkVal.Append(p.MerCustId);
            chkVal.Append(p.OrdId);
            chkVal.Append(p.OrdDate);
            chkVal.Append(p.TransAmt);
            chkVal.Append(p.UsrCustId);
            chkVal.Append(p.TrxId);
            chkVal.Append(p.IsFreeze);
            chkVal.Append(p.FreezeOrdId);
            chkVal.Append(p.FreezeTrxId);
            chkVal.Append(p.RetUrl);
            chkVal.Append(p.BgRetUrl);
            chkVal.Append(p.MerPriv);
            chkVal.Append(p.RespExt);
            //   LogInfo.WriteLog("投标RespCode" + p.RespCode + "RespExt:" + p.RespExt);
            string chkv = chkVal.ToString();
            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
            int ret = DllInterop.VeriSignMsg(merKeyFile, chkv, chkv.Length, p.ChkValue);
            // LogInfo.WriteLog("验签：" + ret.ToString());
            LogInfo.WriteLog("前台主动投标返回报文:" + FastJSON.toJOSN(p));

            #region 验签
            if (ret == 0)
            {
                if (p.RespCode == "000" || p.RespCode == "322" || p.RespCode == "534" || p.RespCode == "360" || p.RespCode == "099")
                {
                    CheckXiCai(decimal.Parse(p.OrdId));
                    string cachename = p.OrdId + "Invest" + p.UsrCustId;
                    if (Utils.GeTThirdCache(cachename) == 0)
                    {
                        Utils.SetThirdCache(cachename);
                        if (p.FreezeTrxId != "")
                        {
                            string sql = "select ordstate  from hx_Bid_records  where ordstate =0 and  OrdId='" + p.OrdId + "'";
                            DataTable dts = DbHelperSQL.GET_DataTable_List(sql);
                            if (dts.Rows.Count > 0)
                            {
                                //同步处理用户金额
                                B_usercenter BUC = new B_usercenter();
                                int d = BUC.ReInvest_success(p.UsrCustId, p.FreezeOrdId, p.TransAmt, p.FreezeTrxId, p.OrdId, p.MerPriv);
                                LogInfo.WriteLog("前台投标:id" + id.ToString() + "返回唯一冻结标识:" + p.FreezeTrxId + "事务执行结果:" + d.ToString());
                                if (d > 0)
                                {
                                    sql = "select targetid,bid_records_id, borrowing_title,investor_registerid ,username,mobile,invitationcode,investment_amount,life_of_loan,unit_day,borrowing_balance,bonusAmt  from  V_hx_Bid_records_borrowing_target where OrdId='" + p.OrdId + "'";

                                    DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                                    if (dt.Rows.Count > 0)
                                    {
                                        ViewBag.borrowing_title = dt.Rows[0]["borrowing_title"].ToString();
                                        decimal investAmt = decimal.Parse(dt.Rows[0]["investment_amount"].ToString());
                                        string OrdId = p.OrdId;
                                        int registerid = int.Parse(dt.Rows[0]["investor_registerid"].ToString());
                                        ViewBag.userid = registerid;
                                        string targetid = dt.Rows[0]["targetid"].ToString();

                                        #region 待提取为公共方法
                                        #region MyRegion  系统消息
                                        DateTime dti = DateTime.Now;
                                        M_td_System_message pm = new M_td_System_message();
                                        pm.MReg = registerid;
                                        pm.Mstate = 0;
                                        pm.MTitle = "投资成功";
                                        pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功投资了项目【" + dt.Rows[0]["borrowing_title"].ToString() + "】，投资金额是：" + investAmt + "。如有问题可咨询创利投的客服！谢谢！";
                                        pm.PubTime = dti;
                                        pm.Mtype = 1;
                                        B_usercenter.AddMessage(pm);
                                        #endregion

                                        #region MyRegion//短信通知
                                        string contxt = Utils.GetMSMEmailContext(15, 1); // 获取注册成功邮件内容
                                        StringBuilder sbsms = new StringBuilder(contxt);
                                        sbsms = sbsms.Replace("#USERANEM#", dt.Rows[0]["username"].ToString());
                                        sbsms = sbsms.Replace("#PID#", targetid);
                                        sbsms = sbsms.Replace("#MONEY#", investAmt.ToString());
                                        string mobile = dt.Rows[0]["mobile"].ToString();
                                        M_td_SMS_record psms = new M_td_SMS_record();
                                        B_td_SMS_record osms = new B_td_SMS_record();
                                        int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.投资成功.ToString());
                                        psms.phone_number = mobile;
                                        psms.sendtime = DateTime.Now;
                                        psms.senduserid = registerid;
                                        psms.smstype = smstype;
                                        psms.smscontext = sbsms.ToString();
                                        psms.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());
                                        psms.vcode = "";
                                        osms.Add(psms);
                                        #endregion

                                        #region 远程调用生成合同？？？ 稍后替换为本地方法调用  微信端可远程调用
                                        string postString = "action=MUserPDF&data=" + targetid.ToString() + "&uc=" + registerid.ToString() + "&OrdId=" + OrdId;
                                        string sr = Utils.PostWebRequest(Utils.GetRemote_url("pdf/index"), postString, Encoding.UTF8);
                                        #endregion

                                        #region 渠道合作 第一投标调用接口？？？
                                        B_member_table bmt = new B_member_table();
                                        M_member_table mmt = new M_member_table();
                                        mmt = bmt.GetModel(registerid);
                                        if (mmt.Tid != null && mmt.Channelsource == 1)
                                        {
                                            if (B_usercenter.GetInvestCountByUserid(mmt.registerid) == 1)
                                            {
                                                string ret3 = Utils.GetCoopAPI(mmt.Tid, investAmt.ToString("0.00"), 2);
                                                LogInfo.WriteLog("前台渠道合作第一次返回结果:" + ret3 + "  用户id:" + mmt.registerid + " 订单id " + OrdId);
                                            }
                                        }
                                        #endregion
                                        #endregion 待提取为公共方法

                                        //发放奖励
                                        ActFacade act = new ActFacade();
                                        act.SendBonusAfterInvest(dt, EnumCommon.E_hx_ActivityTable.E_ActTargetPlatform.wap);
                                    }
                                }
                            }
                        }
                    }
                }/*缓存检查结束位置*/
            }
            if (p.OrdId != "")
            {

                string sql = "select targetid,bid_records_id, borrowing_title,investor_registerid ,username,mobile,invitationcode,investment_amount,life_of_loan,unit_day,borrowing_balance  from  V_hx_Bid_records_borrowing_target where OrdId='" + p.OrdId + "'";
                DataTable dt2 = DbHelperSQL.GET_DataTable_List(sql);
                if (dt2.Rows.Count > 0)
                {
                    ViewBag.borrowing_title = dt2.Rows[0]["borrowing_title"].ToString();
                }
            }
            return View(p);
            #endregion
        }

        private void CheckXiCai(decimal orderID)
        {
            //var dataTotal = (from item in ef.hx_Bid_records
            //                 join target in ef.hx_borrowing_target
            //                 on item.targetid equals target.targetid
            //                 join user in ef.hx_member_table
            //                 on item.investor_registerid equals user.registerid
            //                 join channel in ef.hx_Channel
            //                 on user.channel_invitedcode equals channel.Invitedcode
            //                 where item.OrdId == orderID && channel.ChannelName == "xicai"
            //                 select item).FirstOrDefault();
            //if (dataTotal == null) return;
            var bidRecord = ef.hx_Bid_records.Where(c => c.OrdId == orderID).FirstOrDefault();
            if (bidRecord == null) return;
            var target = ef.hx_borrowing_target.Where(c => c.targetid == bidRecord.targetid).FirstOrDefault();
            if (target == null) return;
            hx_member_table mmt = ef.hx_member_table.Where(c => c.registerid == bidRecord.investor_registerid).FirstOrDefault();
            if (mmt == null) return;
            string invitedcode = ef.hx_Channel.Where(c => c.ChannelName == "xicai").Select(c => c.Invitedcode).FirstOrDefault();
            if (invitedcode == null) return;
            if (mmt.channel_invitedcode != invitedcode) return;
            new XiCaiHelper("").InvestCallBack(bidRecord.bid_records_id, Request);
        }
        public ActionResult ClearCache()
        {
            string CacheKey = "td_web_Ad_type";

            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            ArrayList al = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                if (CacheEnum.Entry.Key.ToString().Contains(CacheKey))
                {
                    HttpRuntime.Cache.Remove(CacheEnum.Entry.Key.ToString());
                }
            }
            return Content("ok");
        }

        public ActionResult DownLoad()
        {

            return View();
        }

        public ActionResult App()
        {
            return View();
        }




        public ActionResult GetAndroidAPP()
        {
            B_AppUpdatePackage bll = new B_AppUpdatePackage();
            var cModel = bll.GetLastModel("Android", "CLT", "0.1");
            var model = bll.GetDownloadModel(cModel.Code);
            string androidUrl = string.Empty;
            if (model != null)
            {
                try
                {
                    string filePath = Settings.Instance.GetWebsitePhysicalRootPath + "\\" + model.VirtualPath.Replace("/", "\\");
                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    return File(fs, "application/octet-stream", model.Code + ".apk");
                }
                catch (Exception ex)
                {
                    LogInfo.WriteLog("Android 服务器下载失败,未找到文件路径: " + ex.Message);
                    return Content("Android 最新版本还没有发布！");
                }
            }
            return View();
        }
    }
}