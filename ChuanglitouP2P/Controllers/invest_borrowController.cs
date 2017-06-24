using ChuanglitouP2P.BLL;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.InitiativeTender;
using ChuanglitouP2P.Model.VeryCodes.NetCreditAssistant.Model;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Webdiyer.WebControls.Mvc;

namespace ChuanglitouP2P.Controllers
{
    public class invest_borrowController : Controller
    {

        chuangtouEntities ef = new chuangtouEntities();
        B_usercenter o = new B_usercenter();

        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Index(int id = 0, int? pageIndex = 1, int pgaesize = 5)
        {
            string cInvitedcode = Utils.CheckSQLHtml(DNTRequest.GetString("channel"));
            if (string.IsNullOrEmpty(cInvitedcode))
            {
                //此前放出了一批错误连接channnel
                cInvitedcode = Utils.CheckSQLHtml(DNTRequest.GetString("channnel"));
            }
            if (!string.IsNullOrEmpty(cInvitedcode))
            {
                var keyValue = new Dictionary<string, string>();
                keyValue.Add("Invitedcode", cInvitedcode);
                Utils.SetInvCookie("channel", keyValue, 30);
            }

            int uid = Utils.checkloginsessiontop();
            ViewBag.uid = uid;
            InvestMode modelList = new InvestMode();

            if (id <= 0)
            {
                return Content(StringAlert.Alert("参数有误"), "text/html");
            }
            V_borrowing_target_addlist vbta = ef.V_borrowing_target_addlist.Where(p => p.targetid == id && p.tender_state >= 2).FirstOrDefault();

            if (vbta == null)
            {
                return Content(StringAlert.Alert("参数有误，数据不存在!"), "text/html");
            }


            ViewBag.imgstr = GetImageList(id, 1); ///借款人基本材料

            //担保材料
            ViewBag.daibao = GetImageList(id, 2);

            ViewBag.xianchuang = GetImageList(id, 3);   //现场图片 


            modelList.vbtaMode = vbta;

            var list = ef.V_hx_Bid_records_borrowing_target.Where(p => p.targetid == id).OrderByDescending(p => p.bid_records_id).ToPagedList(pageIndex ?? 1, pgaesize);

            modelList.Bid_record = list;
            if (Request.IsAjaxRequest())
            {
                return PartialView("InvestRecord", modelList.Bid_record);
            }
            //防刷新
            Session["RefreshToken"] = "invest_borrow_index";
            return View(modelList);
        }

        #region 获取标的相关图片 +List<hx_borrower_guarantor_picture> GetImageList(int targetid, int type_picture)
        /// <summary>
        /// 获取标的相关图片
        /// </summary>
        /// <param name="targetid">标的id</param>
        /// <param name="type_picture">图片类型id</param>
        /// <returns></returns>
        private List<hx_borrower_guarantor_picture> GetImageList(int targetid, int type_picture)
        {
            List<hx_borrower_guarantor_picture> imgList = new List<hx_borrower_guarantor_picture>();

            string key = "GetImageList" + targetid.ToString() + type_picture.ToString();

            if (HttpRuntime.Cache[key] == null)
            {
                imgList = ef.hx_borrower_guarantor_picture.Where(p => p.targetid == targetid && p.type_picture == type_picture).OrderBy(p => p.picture_index).ToList();
                HttpRuntime.Cache.Add(key, imgList, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
            }
            else
            {
                imgList = HttpRuntime.Cache[key] as List<hx_borrower_guarantor_picture>;
            }

            return imgList;
        }
        #endregion

        public ActionResult investconfirm()
        {
            int targetid = DNTRequest.GetInt("id", 0);
            var refreshToken = Session["RefreshToken"];
            if (refreshToken == null)
            {
                return Redirect("/invest_borrow_" + targetid + ".html");
            }
            else
            {
                Session["RefreshToken"] = null;
            }
            int userid = Utils.checkloginsession();
            decimal lixi = 0.00M;
            decimal investmentamountd = DNTRequest.GetDecimal("investmentamount", 100M);
            B_member_table b = new B_member_table();
            M_member_table p = new M_member_table();
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

                if (investmentamountd > Difference)
                {
                    return Content(StringAlert.Alert("投资金额大于标的差额，不能再投资!"), "text/html");
                }
                if (vbta.project_type_id == 6)
                {
                    //return Content(StringAlert.Alert("新手标仅限app投资!"), "text/html");
                    if (p.useridentity != 5)
                    {
                        var maxmum = decimal.Parse(vbta.maxmum.ToString());

                        if (maxmum != 0 && investmentamountd > maxmum)
                        {
                            return Content(StringAlert.Alert("投资金额超出最大可投限额!"), "text/html");

                        }
                        else
                        {
                            //if (B_usercenter.GetInvestCountByUserid(userid) >= 1)
                            //{
                            //    return Content(StringAlert.Alert("抱歉，新手体验标仅限新用户首次投资!"), "text/html");
                            //}
                        }
                    }

                    decimal dfc = decimal.Parse(vbta.borrowing_balance.ToString()) - decimal.Parse(vbta.fundraising_amount.ToString());
                    if (investmentamountd > dfc)
                    {

                        return Content(StringAlert.Alert("投资金额超出可投金额!"), "text/html");
                    }
                    else if (decimal.Parse(vbta.fundraising_amount.ToString()) + investmentamountd > decimal.Parse(vbta.borrowing_balance.ToString()))
                    {

                        return Content(StringAlert.Alert("项目已经满标，不能投资!"), "text/html");
                    }
                }
                InvestCalc ic = new InvestCalc();
                lixi = ic.InvCalc(targetid, investmentamountd);
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
            UsrAct = ef.hx_UserAct.Where(c => c.registerid == userid && c.UseState == 0).ToList();//默认抵扣券

            //List<hx_UserAct> us = new List<hx_UserAct>();
            //foreach (hx_UserAct item in UsrAct)
            //{
            //    if (DateTime.Compare(item.AmtEndtime.Value.Date, DateTime.Now.Date) >= 0)
            //    {
            //        us.Add(item);
            //    }
            //}
            ViewBag.UsrAct = GetUserActHtml(0, targetid); //us;
            ViewBag.investmentamountd = investmentamountd;
            ViewBag.lixi = lixi;
            ViewBag.countAmt = lixi + investmentamountd;
            return View(vbta);
        }

        /// <summary>
        /// 根据类型ID获取抵扣券或加息券
        /// </summary>
        /// <param name="isinterest">券类型</param>
        /// <param name="targetid">标的ID</param>
        [HttpPost]
        public void investUserActRewType(int isinterest, int targetid)
        {
            string strBui = GetUserActHtml(isinterest, targetid);
            Response.Write(strBui);
        }

        private string GetUserActHtml(int isinterest, int targetid)
        {
            StringBuilder strBui = new StringBuilder();
            V_borrowing_target_addlist vbta = ef.V_borrowing_target_addlist.Where(c => c.targetid == targetid && c.tender_state == 2).OrderByDescending(c => c.targetid).FirstOrDefault();
            if (vbta != null)
            {
                int userid = Utils.checkloginsession();
                List<hx_UserAct> UsrAct = new List<hx_UserAct>();
                var ua = ef.hx_UserAct.Where(c => c.registerid == userid && c.UseState == 0);
                if (isinterest == 0) //抵扣券
                {
                    UsrAct = ua.Where(c => c.RewTypeID == 2).ToList();
                }
                else if (isinterest == 1) //加息券
                {
                    UsrAct = ua.Where(c => c.RewTypeID == 3).ToList();
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
                    strBui.Append("<table width = \"100%\" class=\"pop_table\" id=\"list_tb\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:collapse;\">");
                    strBui.Append("<thead><tr>");
                    strBui.Append("<td width = \"11%\" height=\"36\" bgcolor=\"#f5f5f5\"><input name = \"checkall\" id=\"checkall\" type=\"checkbox\" value=\"\" disabled=\"disabled\" /></td>");
                    strBui.Append("<td width = \"22%\" bgcolor=\"#f5f5f5\">类型</td>");
                    strBui.Append("<td width = \"22%\" bgcolor=\"#f5f5f5\">金额</td>");
                    strBui.Append("<td width = \"22%\" bgcolor=\"#f5f5f5\">最低使用限额</td>");
                    strBui.Append("<td width = \"23%\" bgcolor=\"#f5f5f5\">使用限制</td>");
                    strBui.Append("</tr></thead>");
                    strBui.Append("<tbody>");
                    ActBase aBase = new ActBase();
                    foreach (hx_UserAct item in us)
                    {
                        string limitStr = "";
                        List<int> limitInt = aBase.GetCanUseLimit(item.UseLifeLoan, out limitStr);
                        strBui.Append("<tr>");
                        if (aBase.CheckLimit(limitInt, vbta.unit_day ?? 0, vbta.life_of_loan ?? 0))
                            strBui.Append("<td height = \"36\"><input name = \"check\" ck='rad' type = \"checkbox\" value = \"" + item.UserAct + "\" /></td>");
                        else
                            strBui.Append("<td height = \"36\">-</td>");
                        if (item.RewTypeID == 2)//抵扣券
                        {
                            strBui.Append("<td>抵扣券<input id = \"u" + item.UserAct + "\" name = \"u" + item.UserAct + "\" type = \"hidden\" value = \"" + item.AmtUses + "\" /></td>");
                            strBui.Append("<td>" + item.Amt + "元</td>");
                            strBui.Append("<td>");
                            if (item.Uselower <= 0) { strBui.Append("无投资门槛"); }
                            else { strBui.Append(item.Uselower); }
                            strBui.Append("</td>");
                            strBui.Append("<td>" + limitStr + "</td>");
                        }
                        else if (item.RewTypeID == 3) //加息券
                        {
                            //strBui.Append("<td height = \"36\"><input name = \"check\" ck='rad'  type = \"checkbox\" value = \"" + item.UserAct + "\" /></td>");
                            //if (vbta.unit_day == 1 && vbta.life_of_loan == 3 && item.Amt.ToString() == "1.00")//三月标
                            //{
                            //    strBui.Append("<td height = \"36\"><input name = \"check\" ck='rad'  type = \"checkbox\" value = \"" + item.UserAct + "\" /></td>");
                            //}
                            //else if (vbta.unit_day == 1 && vbta.life_of_loan == 6 && item.Amt.ToString() == "2.00")//6月标
                            //{
                            //    strBui.Append("<td height = \"36\"><input name = \"check\" ck='rad'  type = \"checkbox\" value = \"" + item.UserAct + "\" /></td>");
                            //}
                            //else
                            //{
                            //    if (item.Amt.ToString() == "3.00")
                            //    {
                            //        strBui.Append("<td height = \"36\"><input name = \"check\" ck='rad'  type = \"checkbox\" value = \"" + item.UserAct + "\" /></td>");
                            //    }
                            //    else
                            //    {
                            //        strBui.Append("<td height = \"36\">-</td>");
                            //    }
                            //}

                            strBui.Append("<td>加息券<input id = \"u" + item.UserAct + "\" name = \"u" + item.UserAct + "\" type = \"hidden\" value = \"" + item.AmtUses + "\" /></td>");
                            strBui.Append("<td>" + item.Amt + "%</td>");
                            strBui.Append("<td>");
                            if (item.Uselower <= 0) { strBui.Append("无投资门槛"); }
                            else { strBui.Append(item.Uselower); }
                            strBui.Append("</td>");
                            strBui.Append("<td>" + limitStr + "</td>");
                            //if (item.Amt.ToString() == "1.00")
                            //{
                            //    strBui.Append("<td>投资3个月标可用</td>");
                            //}
                            //else if (item.Amt.ToString() == "2.00")//六月标
                            //{
                            //    strBui.Append("<td>投资6个月标可用</td>");
                            //}
                            //else if (item.Amt.ToString() == "3.00")
                            //{
                            //    strBui.Append("<td>不限</td>");
                            //}
                            //else
                            //{
                            //    strBui.Append("<td>不限</td>");
                            //}
                        }
                        strBui.Append("</tr>");
                    }
                    strBui.Append("</tbody>");
                    strBui.Append("</table>");
                }
            }
            else
            {
                strBui.Append("标的信息不存在!");
            }

            return strBui.ToString();
        }

        /// <summary>
        /// 获取抵扣券分档信息（1%或2%）
        /// </summary>
        /// <param name="userAct"></param>
        public int GetInvestType(int userAct)
        {
            int rewType = 0;
            int userid = Utils.checkloginsession();
            List<hx_UserAct> UsrAct = new List<hx_UserAct>();
            UsrAct = ef.hx_UserAct.Where(c => c.registerid == userid && c.UseState == 0 && c.RewTypeID == 2 && c.AmtEndtime >= DateTime.Now && c.UserAct == userAct).ToList();
            if (UsrAct != null)
            {
                foreach (hx_UserAct item in UsrAct)
                {
                    rewType = Convert.ToInt32((Convert.ToDouble(item.Amt) / Convert.ToDouble(item.Uselower)) * 100);
                }
            }
            return rewType;
        }



        /// <summary>
        /// 投资利率计算器
        /// </summary>
        /// <returns></returns>
        public ActionResult InvestCalculator(int id, double jialixi, decimal data = 100M)
        {
            InvestCalc ic = new InvestCalc();
            decimal lixi = ic.InvCalc(id, data, jialixi);
            string json = @" {""amount""    : ""amiunt100"", ""principal""      :  ""principal102""}";
            string strdisp = RMB.GetWebConvertdisp(lixi, 2, false);
            json = json.Replace("amiunt100", data.ToString());
            json = json.Replace("principal102", strdisp);
            return Content(json);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult PostInvest()
        {
            int userid = Utils.checkloginsession();
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
                return Content(StringAlert.Alert("帐户余额不足，请充值!", "/usercenter/recharge"), "text/html");
            }
            string sql = "SELECT top 1 targetid,borrower_registerid,loan_number,borrowing_title,borrowing_thumbnail,annual_interest_rate,borrowing_balance,life_of_loan,unit_day,release_date,month_payment_date,repayment_date,minimum,maxmum,company_name,guarantee_way_name,fundraising_amount,tender_state,payment_options,loan_management_fee,username,realname,BorrUsrCustId,project_type_id from V_borrowing_target_addlist where tender_state>=2 and targetid = " + targetid.ToString() + " order by  targetid desc";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                //if (dt.Rows[0]["project_type_id"].ToString() == "6")
                //{
                //    return Content(StringAlert.Alert("新手标仅限app投资!"), "text/html");
                //}
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
                //mp.InvestDate = DateTime.Parse(DateTime.Parse(dt.Rows[0]["release_date"].ToString()).ToString("yyyy-MM-dd"));
                mp.ReleaseDate = DateTime.Parse(dt.Rows[0]["release_date"].ToString()).ToString("yyyy-MM-dd");//新加借款日期字段
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
                p.investor_registerid = Utils.checkloginsession();
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
                    //Mt.RetUrl = "http://localhost:17745/investment_success_" + targetid.ToString() + ".html";
                    Mt.RetUrl = Utils.GetRe_url("investment_success_" + targetid.ToString() + ".html");
                    Mt.BgRetUrl = Utils.GetRe_url("Thirdparty/BG_investment_success");
                    // Mt.BgRetUrl = Utils.GetRe_url("666Thirdparty/BG_investment_success");
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
                            sql = "update hx_UserAct set UseState=3,AmtProid=" + bid_records_id.ToString() + " where UserAct in (" + Rewardsids + ") and UseState=0 and registerid=" + userid;
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
            //int invcount = 0; //记录用户是否为首次投资,汲及到邀请人的操作

            #region 验签
            if (ret == 0)
            {
                if (p.RespCode == "000" || p.RespCode == "322" || p.RespCode == "360" || p.RespCode == "099")
                {
                    CheckFubaba(decimal.Parse(p.OrdId));
                    CheckXiCai(decimal.Parse(p.OrdId));
                    string cachename = p.OrdId + "InvestWeb" + p.UsrCustId;
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
                                    sql = "select targetid,bid_records_id, borrowing_title,investor_registerid ,username,mobile,invitationcode,investment_amount,life_of_loan,unit_day,borrowing_balance,bonusAmt from  V_hx_Bid_records_borrowing_target where OrdId='" + p.OrdId + "'";
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
                                        act.SendBonusAfterInvest(dt, EnumCommon.E_hx_ActivityTable.E_ActTargetPlatform.web);
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
                    ViewBag.userid = dt2.Rows[0]["investor_registerid"].ToString();
                }
            }
            return View(p);
        }
        /// <summary>
        /// 富爸爸渠道用户首投调用逻辑
        /// </summary>
        /// <param name="mmt"></param>
        /// <param name="dt"></param>
        private void CheckFubaba(decimal orderID)
        {
            LogInfo.WriteLog("富爸爸渠道推送数据开始");
            var bidRecord = ef.hx_Bid_records.Where(c => c.OrdId == orderID).FirstOrDefault();
            if (bidRecord == null) return;
            var target = ef.hx_borrowing_target.Where(c => c.targetid == bidRecord.targetid).FirstOrDefault();
            if (target == null) return;
            var bidRecordCount = (from item in ef.hx_Bid_records
                                  where item.investor_registerid == bidRecord.investor_registerid && item.invest_state == 1
                                  select item).Count();
            if (bidRecordCount != 1) return;
            hx_member_table mmt = ef.hx_member_table.Where(c => c.registerid == bidRecord.investor_registerid).FirstOrDefault();
            string unitDay = target.unit_day.ToString();// dt.Rows[0]["unit_day"].ToString();
            string lifeOfLoan = target.life_of_loan.ToString();//dt.Rows[0]["life_of_loan"].ToString();
            string invitedcode = ef.hx_Channel.Where(c => c.ChannelName == "fubaba").Select(c => c.Invitedcode).FirstOrDefault();
            if (unitDay == "1" && (lifeOfLoan == "1" || lifeOfLoan == "3") && mmt.channel_invitedcode == invitedcode)
            {
                SendData(orderID, bidRecord.investment_amount, target.borrowing_title, mmt.Tid, lifeOfLoan);
            }
            LogInfo.WriteLog("富爸爸渠道推送数据完毕");
        }

        private void SendData(decimal orderID, decimal? investment_amount, string borrowing_title, string Tid, string lifeOfLoan)
        {
            string url, urlParams, sr;
            string goodsmark = "1";//lifeOfLoan == "1" ? "1" : "2";
            string planid = "259";
            string key = "chuanglitou001@";
            //string sig = ChuanglitouP2P.Common.LiumiTools.MD5(orderID + key);
            string sig = Utils.MD5(orderID + key);
            string goodsprice = (investment_amount ?? 0).ToString("0.00"); //Convert.ToDecimal(dt.Rows[0]["investment_amount"].ToString()).ToString("0.00");
            string status = string.Format("首单【{0}元：已付款】", goodsprice);
            string goodsname = string.Format("名称:{0},类型:抵押标,周期:{1}个月", borrowing_title, lifeOfLoan);
            url = "http://www.fbaba.net/track/cps.php";
            urlParams = string.Format("action=create&planid={0}&order={1}&goodsmark={2}&goodsprice={3}&goodsname={4}&sig={5}&status={6}&uid={7}", planid, orderID, goodsmark, goodsprice, goodsname, sig, status, Tid);
            sr = Utils.PostWebRequest(url, urlParams, Encoding.UTF8);
            LogInfo.WriteLog("富爸爸渠道推送数据:url：" + url + "  参数:" + urlParams + " 返回结果 " + sr);
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

            var bidRecord = ef.hx_Bid_records.Where(c => c.OrdId == orderID).FirstOrDefault();
            if (bidRecord == null) return;
            var target = ef.hx_borrowing_target.Where(c => c.targetid == bidRecord.targetid).FirstOrDefault();
            if (target == null) return;
            hx_member_table mmt = ef.hx_member_table.Where(c => c.registerid == bidRecord.investor_registerid).FirstOrDefault();
            if (mmt == null) return;
            string invitedcode = ef.hx_Channel.Where(c => c.ChannelName == "xicai").Select(c => c.Invitedcode).FirstOrDefault();
            if (invitedcode == null) return;
            if (mmt.channel_invitedcode != invitedcode) return;
            //if (dataTotal == null) return;
            new XiCaiHelper("").InvestCallBack(bidRecord.bid_records_id, Request);
        }

        public ActionResult RePostBubabaData(string password, string name, decimal orderID)
        {
            if (name != "cltFubabaSendData" || password != "clt20161115")
            {
                return Content("无效的访问");
            }

            LogInfo.WriteLog("富爸爸渠道推送数据开始");
            var bidRecord = ef.hx_Bid_records.Where(c => c.OrdId == orderID).FirstOrDefault();
            if (bidRecord == null) return Content("投资记录不存在");
            var target = ef.hx_borrowing_target.Where(c => c.targetid == bidRecord.targetid).FirstOrDefault();
            if (target == null) return Content("标的信息不存在");
            //var bidRecordCount = (from item in ef.hx_Bid_records
            //                      where item.investor_registerid == bidRecord.investor_registerid && item.invest_state == 1
            //                      select item).Count();
            //if (bidRecordCount != 1) return Content("不是首次投资");
            hx_member_table mmt = ef.hx_member_table.Where(c => c.registerid == bidRecord.investor_registerid).FirstOrDefault();
            string unitDay = target.unit_day.ToString();// dt.Rows[0]["unit_day"].ToString();
            string lifeOfLoan = target.life_of_loan.ToString();//dt.Rows[0]["life_of_loan"].ToString();
            string invitedcode = ef.hx_Channel.Where(c => c.ChannelName == "fubaba").Select(c => c.Invitedcode).FirstOrDefault();
            if (unitDay == "1" && (lifeOfLoan == "1" || lifeOfLoan == "3") && mmt.channel_invitedcode == invitedcode)
            {
                SendData(orderID, bidRecord.investment_amount, target.borrowing_title, mmt.Tid, lifeOfLoan);
            }
            LogInfo.WriteLog("富爸爸渠道推送数据完毕");
            return Content("已发送");
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


    }
}
#endregion
