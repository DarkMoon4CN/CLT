using ChuanglitouP2P.BLL;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Controllers
{
    /// <summary>
    /// 我要投资
    /// 作者：阿飞
    /// </summary>
    public class InvestlistController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();
        // GET: Investlist
        public ActionResult Index()
        {
            //InvestSearch search = new InvestSearch();

            //ViewBag.arp = arp;// search.GetInvestSearchByType(Common.EnumInvestSearch.Arp);
            //ViewBag.repayment = repayment;// search.GetInvestSearchByType(Common.EnumInvestSearch.Repayment);
            //ViewBag.account = account;// search.GetInvestSearchByType(Common.EnumInvestSearch.Account);
            //ViewBag.schedule = schedule;// search.GetInvestSearchByType(Common.EnumInvestSearch.Schedule);
            //ViewBag.status = status;// search.GetInvestSearchByType(Common.EnumInvestSearch.Status);
            //ViewBag.project = project;// search.ProjectTypeCache();
            //ViewBag.sort = sort;

            var actFanXian = ef.hx_ActivityTable.Where(c => c.ActName == "12月投资立得返现奖励").OrderByDescending(c => c.ActEndtime).FirstOrDefault();
            ViewBag.FanXianSTime = actFanXian.ActStarttime == null ? "" : Convert.ToDateTime(actFanXian.ActStarttime).ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.FanXianETime = actFanXian.ActEndtime == null ? "" : Convert.ToDateTime(actFanXian.ActEndtime).ToString("yyyy-MM-dd HH:mm:ss");
            return View();
        }

        public ActionResult GetInvestList(int arp = 0, int repayment = 0, int account = 0, int schedule = 0, int status = 0, int project = 0, int sort = 0, int Page = 1, int pageSize = 12)
        {
            string TableName = "V_borrowing_target_addlist";
            string strFields = "targetid,loan_number,borrowing_title,borrowing_thumbnail,annual_interest_rate,borrowing_balance,life_of_loan,unit_day,release_date,month_payment_date,repayment_date,end_time,minimum,company_name,guarantee_way_name,fundraising_amount,tender_state,start_time,sys_time,IsUse,companyid,indexorder,payment_options ";
            string fldName = "tender_state asc,indexorder desc,targetid desc";

            switch (sort)
            {
                case 1: //期   限
                    fldName = "unit_day asc,life_of_loan desc,indexorder desc,tender_state asc,targetid desc";
                    break;
                case 2: //发标时间
                    fldName = "start_time desc,indexorder desc,tender_state asc,targetid desc";
                    break;
                case 3: //预期年化收益
                    fldName = "annual_interest_rate desc,indexorder desc,tender_state asc,targetid desc";
                    break;
                default:
                    break;
            }

            string strWhere = " targetid >0 and tender_state>=2 and annual_interest_rate<=15 AND project_type_id!=6  ";//显示利率小于15 类型不是新手标的项目
            DataTable dt = new DataTable();
            if (arp > 0)
            {
                string[] sta = getarray(arp);

                if (sta.Length > 0)
                {
                    if (sta[1].ToString() == "0")
                    {
                        strWhere += " and tender_state =" + sta[0].ToString() + "   ";
                    }
                    else
                    {
                        strWhere += " and annual_interest_rate   between " + sta[0].ToString() + " and " + sta[1].ToString() + "   ";
                    }
                }
            }
            else
            {
                strWhere += " and tender_state between  2 and  5 ";
            }

            if (repayment > 0)
            {
                string[] sta = getarray(repayment);

                if (sta.Length > 0)
                {
                    //strWhere += " and DATEDIFF(MONTH,release_date,repayment_date)  between " + sta[0].ToString() + " and " + sta[1].ToString() + "   ";
                    if (repayment == 5)//3个月以下
                    {
                        strWhere += " AND (life_of_loan BETWEEN  " + sta[0].ToString() + " AND " + sta[1].ToString() + "  AND unit_day=1 OR life_of_loan <90  AND unit_day=3)   ";
                    }
                    else
                    {
                        strWhere += " AND life_of_loan BETWEEN  " + sta[0].ToString() + " AND " + sta[1].ToString() + " AND unit_day=1 ";
                    }
                }
            }

            if (account > 0)
            {
                string[] sta = getarray(account);
                if (sta.Length > 0)
                {
                    strWhere += " and borrowing_balance  between " + int.Parse(sta[0].ToString()) * 10000 + " and " + int.Parse(sta[1].ToString()) * 10000 + "   ";
                }
            }

            if (schedule > 0)
            {
                string[] sta = getarray(schedule);
                if (sta.Length > 0)
                {
                    strWhere += " and  fundraising_amount/borrowing_balance*100  between " + sta[0].ToString() + " and " + sta[1].ToString() + "   ";
                }
            }

            if (status > 0)
            {
                string[] sta = getarray(status);
                if (sta.Length > 0)
                {
                    strWhere += " and  tender_state =" + sta[0].ToString() + "   ";
                }
            }



            if (project > 0)
            {
                strWhere += " and  project_type_id =" + project.ToString() + "   ";

            }

            int RecordCount = 0;
            B_PublicPageList o = new B_PublicPageList();
            dt = o.GetListByPage(TableName, strFields, fldName, pageSize, Page, strWhere, out RecordCount);
            //计算总页数
            var pagecount = RecordCount / pageSize;
            if ((RecordCount % pageSize) > 0)
            {
                pagecount++;
            }
            StringBuilder json = new StringBuilder();
            json.Append("{");
            json.AppendFormat("\"ret\":1,\"pagecount\":{0},\"data\":[", pagecount);
            int _index = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (_index > 0)
                    {
                        json.Append(",");
                    }
                    _index++;
                    json.Append("{");
                    json.AppendFormat("\"targetid\":{0}", dr["targetid"].ToString());
                    json.AppendFormat(",\"borrowing_title\":\"{0}\"", dr["borrowing_title"].ToString());
                    json.AppendFormat(",\"companyid\":\"{0}\"", dr["companyid"].ToString());
                    json.AppendFormat(",\"company_name\":\"{0}\"", dr["company_name"].ToString());
                    json.AppendFormat(",\"borrowing_thumbnail\":\"{0}\"", dr["borrowing_thumbnail"].ToString());
                    json.AppendFormat(",\"annual_interest_rate\":\"{0}\" ", decimal.Parse(dr["annual_interest_rate"].ToString()).ToString("0.0"));
                    json.AppendFormat(",\"tender_state\":{0}", dr["tender_state"].ToString());
                    json.AppendFormat(",\"IsUse\":{0}", Convert.ToInt32(dr["IsUse"].ToString()));
                    //借款期限
                    DateTime rpdt = DateTime.Parse(dr["repayment_date"].ToString());
                    DateTime rest = DateTime.Parse(dr["release_date"].ToString());
                    long diffdays = Utils.DateDiff("Day", DateTime.Parse(rest.ToString("yyyy-MM-dd")), DateTime.Parse(rpdt.ToString("yyyy-MM-dd")));
                    int unit_day = dr["unit_day"].ToInt();
                    json.AppendFormat(",\"jkqx\":\"{0}\"", dr["life_of_loan"].ToString());
                    if (unit_day == 3)
                    {
                        json.AppendFormat(",\"jkday\":\"天\"");
                    }
                    else if (unit_day == 1)
                    {
                        json.AppendFormat(",\"jkday\":\"个月\"");
                    }
                    //else  if (diffdays < 60)
                    //{
                    //    int daysr = int.Parse(diffdays.ToString());
                    //    if (daysr == 30 || daysr == 31)
                    //    {
                    //        json.AppendFormat(",\"jkday\":\"天\"", Utils.GetLife_of_loans(dr["life_of_loan"].ToString(), dr["unit_day"].ToString()));
                    //    }
                    //    else
                    //    {
                    //        json.AppendFormat(",\"jkqx\":\"{0}天\"", daysr);
                    //    }
                    //}
                    //else
                    //{
                    //    json.AppendFormat(",\"jkqx\":\"{0}\"", Utils.GetLife_of_loans(dr["life_of_loan"].ToString(), dr["unit_day"].ToString()));
                    //}
                    string str1 = "";
                    string str2 = "";
                    if (int.Parse(dr["tender_state"].ToString()) > 2)
                    {
                        str1 = Utils.Getpayment_options(int.Parse(dr["payment_options"].ToString()));
                    }
                    else
                    {
                        str2 = DateTime.Parse(dr["repayment_date"].ToString()).ToString("yyyy-MM-dd");
                    }
                    json.AppendFormat(",\"payment_options\":\"{0}\"", str1);    //还款方式
                    json.AppendFormat(",\"repayment_date\":\"{0}\"", str2);     //还款日期
                    json.AppendFormat(",\"borrowing_balance\":\"{0}\"", RMB.GetWebConvertdisp(decimal.Parse(dr["borrowing_balance"].ToString()), 2, true));

                    decimal borrowing_balance = decimal.Parse(dr["borrowing_balance"].ToString());
                    decimal fundraising_amount = decimal.Parse(dr["fundraising_amount"].ToString());
                    decimal Percentage = fundraising_amount / borrowing_balance * 100;
                    if (Percentage > 100.00M) Percentage = 100.00M;
                    decimal Difference = borrowing_balance - fundraising_amount;
                    if (Difference < 0.00M) Percentage = 0.00M;

                    //还需金额
                    var needMoney = "0";
                    if (dr["tender_state"].ToString() == "2")
                    {
                        needMoney = RMB.GetWebConvertdisp(Difference, 2, true);
                    }
                    json.AppendFormat(",\"needMoney\":\"{0}\"", needMoney);
                    json.AppendFormat(",\"Percentage\":\"{0}\"", Percentage.ToString("0.00"));  //融资进度
                    json.AppendFormat(",\"minimum\":\"{0}\"", decimal.Parse(dr["minimum"].ToString()).ToString("0"));
                    DateTime Endtime = DateTime.Parse(dr["end_time"].ToString());
                    DateTime Stime = DateTime.Parse(dr["start_time"].ToString());
                    DateTime online = DateTime.Parse(dr["sys_time"].ToString());
                    json.AppendFormat(",\"systime\":\"{0}\"", online.ToString("yyyy-MM-dd HH:mm:ss"));
                    int operState = 0;
                    if (dr["tender_state"].ToString() == "2")
                    {
                        if (Stime <= online && online > DateTime.Now)
                        {
                            //str.Append("<a href=\"javascript:void(0);\" class=\"grey_btn\">未开始</a>");
                            operState = 11;
                        }
                        else
                        {
                            if (DateTime.Compare(Endtime, DateTime.Now) <= 0 && Percentage < 100.00M)
                            {
                                //str.Append("<a href=\"javascript:void(0);\" class=\"grey_btn\">项目已结束</a>");
                                operState = 12;
                            }
                            else if (Percentage >= 100.00M)
                            {
                                //str.Append("<a href=\"javascript:void(0);\" class=\"grey_btn\">满标</a>");
                                operState = 13;
                            }
                            else
                            {
                                //str.Append("<a href=\"invest_borrow_" + dt.Rows[i]["targetid"].ToString() + ".html\" title=\"" + dt.Rows[i]["borrowing_title"].ToString() + "\">立即投资</a>");
                                operState = 14;
                            }
                        }
                    }
                    else if (dr["tender_state"].ToString() == "3")
                    {
                        //str.Append("<a href=\"invest_borrow_" + dt.Rows[i]["targetid"].ToString() + ".html\" title=\"" + dt.Rows[i]["borrowing_title"].ToString() + "\" class=\"grey_btn\">满标</a>");
                        operState = 3;
                    }

                    else if (dr["tender_state"].ToString() == "4")
                    {
                        //str.Append("<a href=\"invest_borrow_" + dt.Rows[i]["targetid"].ToString() + ".html\" title=\"" + dt.Rows[i]["borrowing_title"].ToString() + "\" class=\"grey_btn\">还款中</a>");
                        operState = 4;
                    }
                    else if (dr["tender_state"].ToString() == "5")
                    {
                        //str.Append("<a href=\"invest_borrow_" + dt.Rows[i]["targetid"].ToString() + ".html\" title=\"" + dt.Rows[i]["borrowing_title"].ToString() + "\" class=\"grey_btn\">已还清</a>");
                        operState = 5;
                    }


                    json.AppendFormat(",\"operState\":\"{0}\"", operState);

                    var actFanXian = ef.hx_ActivityTable.Where(c => c.ActName == "12月投资立得返现奖励").OrderByDescending(c => c.ActEndtime).FirstOrDefault();
                    bool isShow = actFanXian == null ? false : TActivity_Luck.GetCurJiaoBiao(Convert.ToDateTime(actFanXian.ActStarttime), Convert.ToDateTime(actFanXian.ActEndtime), online, Convert.ToInt32(dr["tender_state"]), Convert.ToDateTime(dr["end_time"]));
                    json.AppendFormat(",\"isShowJiaoBiao\":\"{0}\"", isShow);

                    json.Append("}");

                }
            }
            json.Append("]}");

            return Content(json.ToString(), "text/json");


        }

        private string[] getarray(int key)
        {
            InvestSearch search = new InvestSearch();
            var searchModel = search.GetModelById(key);

            return searchModel.splitname.Split('|');

        }
    }
}