using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using ChuanglitouP2P.DBUtility;
using System.Data;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.BLL.EF;
using System.Linq.Expressions;
using ChuanglitouP2P.Common.Extensionses;
using ChuangLitouP2P.Models;
using PagedList;
using ChuanglitouP2P.Areas.Admin.Controllers.Filters;

using ChuanglitouP2P.Models;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Model.chinapnr.Transfer;
namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    /// <summary>
    /// 统计
    /// </summary>
    public class StatisticsController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/Statistics
        [AdminVaildate()]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 奖励统计
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult CouponStatistics(string start = "", string end = "")
        {
            if (string.IsNullOrEmpty(start))
            {
                start = DateTime.Now.ToString("yyyy-MM") + "-01";
            }
            if (string.IsNullOrEmpty(end))
            {
                end = DateTime.Now.ToString("yyyy-MM-dd");
            }
            string strWhere = "";
            if (start != null && start != "")
            {
                strWhere += " and Createtime >= '" + start + " 00:00:00'";
            }
            if (end != null && end != "")
            {
                strWhere += " and Createtime <= '" + end + " 23:59:59'";
            }

            //  string sql = "select activity_schedule_id,  count(activity_schedule_id) as  奖励数量 ,SUM(amount_of_reward) as 奖励金额,activity_schedule_name as 奖励分类 ,(select count(activity_schedule_id) as df from V_bonus_account bd  where bd.activity_schedule_id=vb.activity_schedule_id and  bd.reward_state=0  and entry_time between '" + start + " 00:00:00' and  '" + end + " 23:59:59'  ) as 未使用  ,(select count(activity_schedule_id) as df from V_bonus_account bd  where bd.activity_schedule_id=vb.activity_schedule_id and  bd.reward_state=1  and entry_time between '" + start + " 00:00:00' and  '" + end + " 23:59:59' ) as 已使用 ,(select count(activity_schedule_id) as df from V_bonus_account bd  where bd.activity_schedule_id=vb.activity_schedule_id and  bd.reward_state=2  and entry_time between '" + start + " 00:00:00' and  '" + end + " 23:59:59' ) as 已过期,(select count(activity_schedule_id) as df from V_bonus_account bd  where bd.activity_schedule_id=vb.activity_schedule_id and  bd.reward_state=3   and entry_time between '" + start + " 00:00:00' and  '" + end + " 23:59:59' ) as 锁定中    from V_bonus_account vb   where activity_schedule_id>0 " + strWhere + " group by activity_schedule_id,activity_schedule_name ";

            string sql = "select RewTypeID, count(Amt) as  奖励数量,SUM(Amt) as 奖励金额,(select count(Amt) as df from hx_UserAct bd  where bd.RewTypeID=vb.RewTypeID and  bd.UseState=0  and Createtime between '" + start + " 00:00:00' and  '" + end + " 23:59:59'  ) as 未使用,(select count(Amt) as df from hx_UserAct bd  where bd.RewTypeID=vb.RewTypeID and  bd.UseState=1  and Createtime between '" + start + " 00:00:00' and  '" + end + " 23:59:59'  ) as 已使用,(select count(Amt) as df from hx_UserAct bd  where bd.RewTypeID=vb.RewTypeID and  bd.UseState=2  and Createtime between '" + start + " 00:00:00' and  '" + end + " 23:59:59'  ) as 已过期,(select count(Amt) as df from hx_UserAct bd  where bd.RewTypeID=vb.RewTypeID and  bd.UseState=3  and Createtime between '" + start + " 00:00:00' and  '" + end + " 23:59:59'  ) as 锁定中 from hx_UserAct  vb  where   RewTypeID >0 " + strWhere + " group by RewTypeID";

            //  LogInfo.Info("查询语句统计："+sql);

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            ViewBag.start = start;
            ViewBag.end = end;

            return View(dt);
        }


        /// <summary>
        /// 奖励查询
        /// </summary>
        /// <param name="username"></param>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <param name="ddlType"></param>
        /// <param name="Page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Rewardquery(string username = "", string time1 = "", string time2 = "", int ddlType = 0, int Page = 1, int pageSize = 10)
        {
            username = Utils.CheckSQLHtml(username);
            int pageNumber = Page / 1;
            Expression<Func<hx_UserAct, bool>> where = PredicateExtensionses.True<hx_UserAct>();
            where = where.And(p => p.UserAct > 0);

            if (!string.IsNullOrEmpty(username))
            {
                where = where.And(p => p.hx_member_table.username.Contains(username));
            }
            if (!string.IsNullOrEmpty(time1))
            {
                DateTime dt1 = Convert.ToDateTime(time1);
                where = where.And(a => ((DateTime)a.Createtime).CompareTo(dt1) >= 0);
            }
            if (!string.IsNullOrEmpty(time2))
            {
                DateTime dt2 = Convert.ToDateTime(time2);
                dt2 = Convert.ToDateTime(dt2.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(a => ((DateTime)a.Createtime).CompareTo(dt2) <= 0);
            }

            if (ddlType > 0)
            {
                where = where.And(a => a.RewTypeID == ddlType);
            }
            IPagedList<hx_UserAct> list = ef.hx_UserAct.Where(where).OrderByDescending(p => p.UserAct).ToPagedList(pageNumber, pageSize);

            ViewBag.username = username;
            ViewBag.time1 = time1;
            ViewBag.time2 = time2;
            ViewBag.ddlType = ddlType;
            ViewBag.TypeDropDownList = new SelectListByEF().GetScheduleDropDownList("0", "所有分类");

            return View(list);
        }

        /// <summary>
        /// 用户统计
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult UserStatistics(string start = "", string end = "")
        {
            if (string.IsNullOrEmpty(start))
            {
                start = DateTime.Now.ToString("yyyy-MM") + "-01";
            }
            if (string.IsNullOrEmpty(end))
            {
                end = DateTime.Now.ToString("yyyy-MM-dd");
            }
            string strWhere = "";

            if (start != null && start != "")
            {
                strWhere += " and StatisticsDate >= '" + start + " 00:00:00'";
            }
            if (end != null && end != "")
            {
                strWhere += " and StatisticsDate <= '" + end + " 23:59:59'";
            }
            string sql = "  select StatisticsDate as registration_time,RegistCount as regcount,RealNameCount as realNameCount,InvestCount as firstInvestCount from Statistics_Member where 1 = 1 " + strWhere + " order by StatisticsDate desc";
            //string sql = "select  convert(varchar(10),registration_time,120) as registration_time,count(registerid) as regcount  from  hx_member_table  where registerid>0 " + strWhere + "  group by  convert(varchar(10),registration_time,120) order by convert(varchar(10),registration_time,120) desc ";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            ViewBag.start = start;
            ViewBag.end = end;

            return View(dt);
        }

        public ActionResult UserStatisticsToExcel(string starttime, string endtime)
        {
            #region 判断权限
            var userid = Utils.GetAdmUserID();
            var controllerName = this.RouteData.Values["controller"].ToString();
            var actionName = this.RouteData.Values["action"].ToString();
            if (!new UserLimitByEF().CheckAdminLimit(userid, controllerName, actionName))
            {   //无权限
                return Content("您没有操作权限");
            }
            #endregion

            if (string.IsNullOrEmpty(starttime))
            {
                starttime = DateTime.Now.ToString("yyyy-MM") + "-01";
            }
            if (string.IsNullOrEmpty(endtime))
            {
                endtime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            string strWhere = "";

            if (starttime != null && starttime != "")
            {
                strWhere += " and StatisticsDate >= '" + starttime + " 00:00:00'";
            }
            if (endtime != null && endtime != "")
            {
                strWhere += " and StatisticsDate <= '" + endtime + " 23:59:59'";
            }
            string sql = "  select CONVERT(varchar, StatisticsDate) as '时间',RegistCount as '注册人数',RealNameCount as '实名人数',InvestCount as '首投人数' from Statistics_Member where 1 = 1 " + strWhere + " order by '时间' desc";
            //string sql = "select  convert(varchar(10),registration_time,120) as registration_time,count(registerid) as regcount  from  hx_member_table  where registerid>0 " + strWhere + "  group by  convert(varchar(10),registration_time,120) order by convert(varchar(10),registration_time,120) desc ";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            var allregcount = 0;//总用户注册人数
            var allrealNameCount = 0;//总的实名人数
            var allfirstInvestCount = 0;//首投人数

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    allregcount+= Convert.ToInt32(dr["注册人数"]);
                    allrealNameCount += Convert.ToInt32(dr["实名人数"]);
                    allfirstInvestCount += Convert.ToInt32(dr["首投人数"]);
                }
            }

            dt.Rows.Add();
            
            dt.Rows[dt.Rows.Count - 1][0] = "合计";
            dt.Rows[dt.Rows.Count - 1][1] = allregcount.ToString();
            dt.Rows[dt.Rows.Count - 1][2] = allrealNameCount.ToString();
            dt.Rows[dt.Rows.Count - 1][3] = allfirstInvestCount.ToString();

            var path = Extensions.ExportExcel(dt);

            return Content(path);
        }

        /// <summary>
        /// 提现统计
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult CashStatistics(string start = "", string end = "")
        {
            if (string.IsNullOrEmpty(start))
            {
                start = DateTime.Now.ToString("yyyy-MM") + "-01";
            }
            if (string.IsNullOrEmpty(end))
            {
                end = DateTime.Now.ToString("yyyy-MM-dd");
            }
            string strWhere = "";
            if (start != null && start != "")
            {
                strWhere += " and OrdIdTime >= '" + start + " 00:00:00'";
            }
            if (end != null && end != "")
            {
                strWhere += " and OrdIdTime <= '" + end + " 23:59:59'";
            }
            string sql = "select  convert(varchar(10),OrdIdTime,120) as OrdIdTime, coalesce(sum(TransAmt),0) as TransAmt ,count(registerid) as countcs from  hx_td_UserCash   where   OrdIdState=3 " + strWhere + " group by convert(varchar(10),OrdIdTime,120)  order by  OrdIdTime desc ";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            ViewBag.start = start;
            ViewBag.end = end;

            return View(dt);
        }

        public ActionResult CashStatisticsToExcel(string starttime, string endtime)
        {
            #region 判断权限
            var userid = Utils.GetAdmUserID();
            var controllerName = this.RouteData.Values["controller"].ToString();
            var actionName = this.RouteData.Values["action"].ToString();
            if (!new UserLimitByEF().CheckAdminLimit(userid, controllerName, actionName))
            {   //无权限
                return Content("您没有操作权限");
            }
            #endregion

            if (string.IsNullOrEmpty(starttime))
            {
                starttime = DateTime.Now.ToString("yyyy-MM") + "-01";
            }
            if (string.IsNullOrEmpty(endtime))
            {
                endtime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            string strWhere = "";
            if (starttime != null && starttime != "")
            {
                strWhere += " and OrdIdTime >= '" + starttime + " 00:00:00'";
            }
            if (endtime != null && endtime != "")
            {
                strWhere += " and OrdIdTime <= '" + endtime + " 23:59:59'";
            }
            string sql = "select  convert(varchar(10),OrdIdTime,120) as '时间', coalesce(sum(TransAmt),0) as '成功提现金额' ,count(registerid) as '提现人次' from  hx_td_UserCash   where   OrdIdState=3 " + strWhere + " group by convert(varchar(10),OrdIdTime,120)  order by  '时间' desc ";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            var allTransAmt = 0.00;
            var allcountcs = 0;

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    allTransAmt += Convert.ToDouble(dr["成功提现金额"]);
                    allcountcs += Convert.ToInt32(dr["提现人次"]);
                }
            }

            dt.Rows.Add();
            dt.Rows[dt.Rows.Count - 1][0] = "合计";
            dt.Rows[dt.Rows.Count - 1][1] = allTransAmt.ToString();
            dt.Rows[dt.Rows.Count - 1][2] = allcountcs.ToString();

            var path = Extensions.ExportExcel(dt);

            return Content(path);
        }

        /// <summary>
        /// 充值统计
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult RechargeStatistics(string start = "", string end = "")
        {
            if (string.IsNullOrEmpty(start))
            {
                start = DateTime.Now.ToString("yyyy-MM") + "-01";
            }
            if (string.IsNullOrEmpty(end))
            {
                end = DateTime.Now.ToString("yyyy-MM-dd");
            }
            string strWhere = "";

            if (start != null && start != "")
            {
                strWhere += " and recharge_time >= '" + start + " 00:00:00'";
            }
            if (end != null && end != "")
            {
                strWhere += " and recharge_time <= '" + end + " 23:59:59'";
            }

            string sql = "select convert(varchar(10),recharge_time,120) as recharge_time,  Coalesce(sum(recharge_amount),0) as rec_amount,count(membertable_registerid) as countcs  from hx_Recharge_history where recharge_condition=1    " + strWhere + " group by convert(varchar(10),recharge_time,120) order by  convert(varchar(10),recharge_time,120) desc";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            ViewBag.start = start;
            ViewBag.end = end;

            return View(dt);

        }

        public ActionResult RechargeStatisticsToExcel(string starttime, string endtime)
        {
            #region 判断权限
            var userid = Utils.GetAdmUserID();
            var controllerName = this.RouteData.Values["controller"].ToString();
            var actionName = this.RouteData.Values["action"].ToString();
            if (!new UserLimitByEF().CheckAdminLimit(userid, controllerName, actionName))
            {   //无权限
                return Content("您没有操作权限");
            }
            #endregion

            if (string.IsNullOrEmpty(starttime))
            {
                starttime = DateTime.Now.ToString("yyyy-MM") + "-01";
            }
            if (string.IsNullOrEmpty(endtime))
            {
                endtime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            string strWhere = "";

            if (starttime != null && starttime != "")
            {
                strWhere += " and recharge_time >= '" + starttime + " 00:00:00'";
            }
            if (endtime != null && endtime != "")
            {
                strWhere += " and recharge_time <= '" + endtime + " 23:59:59'";
            }

            string sql = "select convert(varchar(10),recharge_time,120) as '时间',  Coalesce(sum(recharge_amount),0) as '充值金额',count(membertable_registerid) as '充值人次'  from hx_Recharge_history where recharge_condition=1    " + strWhere + " group by convert(varchar(10),recharge_time,120) order by  convert(varchar(10),recharge_time,120) desc";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql.ToString());

            var allrec_amount = 0.00;
            var allcountcs = 0;

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    allrec_amount += Convert.ToDouble(dr["充值金额"]);
                    allcountcs += Convert.ToInt32(dr["充值人次"]);
                }
            }

            dt.Rows.Add();
            dt.Rows[dt.Rows.Count - 1][0] = "合计";
            dt.Rows[dt.Rows.Count - 1][1] = allrec_amount.ToString();
            dt.Rows[dt.Rows.Count - 1][2] = allcountcs.ToString();

            var path = Extensions.ExportExcel(dt);

            return Content(path);
        }


        /// <summary>
        /// 会员投资排行
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Investmentrank(string start = "", string end = "")
        {
            if (string.IsNullOrEmpty(start))
            {
                start = DateTime.Now.ToString("yyyy-MM") + "-01";
            }
            if (string.IsNullOrEmpty(end))
            {
                end = DateTime.Now.ToString("yyyy-MM-dd");
            }
            string strWhere = "";

            if (start != null && start != "")
            {
                strWhere += " and invest_time >= '" + start + " 00:00:00'";
            }
            if (end != null && end != "")
            {
                strWhere += " and invest_time <= '" + end + " 23:59:59'";
            }

            string sql = "select investor_registerid,username,realname,sum(investment_amount) as 投资金额,useridentity  from V_hx_Bid_records_borrowing_target  where  investor_registerid>0   " + strWhere + "  group by investor_registerid,username,realname,useridentity order by sum(investment_amount) desc";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            ViewBag.start = start;
            ViewBag.end = end;

            return View(dt);
        }

        #region 借款金额

        /// <summary>
        /// 借款金额
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult BorrowAmt(string start = "", string end = "")
        {
            if (string.IsNullOrEmpty(start))
            {
                start = DateTime.Now.ToString("yyyy-MM") + "-01";
            }
            if (string.IsNullOrEmpty(end))
            {
                end = DateTime.Now.ToString("yyyy-MM-dd");
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("dte");
            dt.Columns.Add("getBroAmt");
            dt.Columns.Add("loansAmt");
            dt.Columns.Add("liubiao");
            DateTime st, ed;
            try
            {
                st = DateTime.Parse(start);
                ed = DateTime.Parse(end);

            }
            catch
            {
                st = DateTime.Now.AddDays(-1);
                ed = DateTime.Now;
            }
            long days = Utils.DateDiff("Day", st, ed);
            int intdays = int.Parse(days.ToString());
            if (intdays == 0)
            {
                intdays = 1;
            }


            for (int i = intdays; i >= 0; i--)
            {

                string dte = st.AddDays(i).ToString("yyyy-MM-dd");

                string getBroAmt1 = getBroAmt(dte);
                string loansAmt1 = loansAmt(dte);

                if (loansAmt1 == "0.00" && getBroAmt1 == "0.00")
                {
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    dr["dte"] = dte;
                    dr["getBroAmt"] = getBroAmt1;
                    dr["loansAmt"] = loansAmt1;
                    dr["liubiao"] = liubiao(dte);

                    dt.Rows.Add(dr);
                }
            }

            /*
            for (int i = 0; i < intdays; i++)
            {

                string dte = st.AddDays(i).ToString("yyyy-MM-dd");

                string getBroAmt1 = getBroAmt(dte);
                string loansAmt1 = loansAmt(dte);

                if (loansAmt1 == "0.00" && getBroAmt1 == "0.00")
                {
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    dr["dte"] = dte;
                    dr["getBroAmt"] = getBroAmt1;
                    dr["loansAmt"] = loansAmt1;
                    dr["liubiao"] = liubiao(dte);

                    dt.Rows.Add(dr);
                }
            }
            */

            ViewBag.start = start;
            ViewBag.end = end;

            return View(dt);
        }


        /// <summary>
        /// 借款金额
        /// </summary>
        /// <param name="sdate"></param>
        /// <returns></returns>
        private string getBroAmt(string sdate)
        {
            string sql = "select  COALESCE(sum(borrowing_balance),0) as borrowing_balance  from hx_borrowing_target where tender_state between 2 and 5  and  convert(varchar(10),release_date,120)='" + sdate + "'";
            return string.Format("{0:C}", DbHelperSQL.Re_String(sql));
        }

        /// <summary>
        /// 满标放款
        /// </summary>
        /// <param name="sdate"></param>
        /// <returns></returns>
        private string loansAmt(string sdate)
        {
            string sql = "select Coalesce(sum(LoanAMT),0) as LoanAMT   from hx_td_Loan_records  where    convert(varchar(10),LoanDate,120)='" + sdate + "'";
            return string.Format("{0:C}", DbHelperSQL.Re_String(sql));
        }

        /// <summary>
        /// 流标
        /// </summary>
        /// <param name="sdate"></param>
        /// <returns></returns>
        private string liubiao(string sdate)
        {
            string sql = "select  COALESCE(sum(borrowing_balance),0) as borrowing_balance  from hx_borrowing_target where tender_state =8  and  convert(varchar(10),release_date,120)='" + sdate + "'";
            return string.Format("{0:C}", DbHelperSQL.Re_String(sql));
        }

        #endregion

        /// <summary>
        /// 借款人数
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult BorrowersNumber(string start = "", string end = "")
        {
            if (string.IsNullOrEmpty(start))
            {
                start = DateTime.Now.ToString("yyyy-MM") + "-01";
            }
            if (string.IsNullOrEmpty(end))
            {
                end = DateTime.Now.ToString("yyyy-MM-dd");
            }
            string strWhere = "";

            if (start != null && start != "")
            {
                strWhere += " and release_date >= '" + start + " 00:00:00'";
            }
            if (end != null && end != "")
            {
                strWhere += " and release_date <= '" + end + " 23:59:59'";
            }

            string sql = "select  convert(varchar(10),release_date,120) as release_date,COALESCE(count(borrower_registerid),0) as  regcount  from hx_borrowing_target where tender_state between 2 and 5   " + strWhere + "   group by  convert(varchar(10),release_date,120)  order by  release_date desc";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            ViewBag.start = start;
            ViewBag.end = end;

            return View(dt);
        }

        #region 借入总统计
        /// <summary>
        /// 借入总统计
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult InBorrowingpresident(string start = "", string end = "")
        {
            ViewBag.inregcount = GetinvAmt(start, end);  //成功借入金额
            ViewBag.bonusAmt = GetbonusAmt(start, end);   //总支出奖励
            ViewBag.daisAmt = GetdaisAmt(start, end); //待收总金额
            ViewBag.daibeiAmt = GetdaibeiAmt(start, end); //待收本金
            ViewBag.HaveAmt = GetHaveAmt(start, end); //已收总额
            ViewBag.daishuAmt = GetdailirenshuAmt(start, end);   //待收利润总额
            ViewBag.ysbeijAmt = GetysbeijAmt(start, end); //已收本金
            ViewBag.yslxAmt = GetyslxAmt(start, end); //已收利润总额

            ViewBag.start = start;
            ViewBag.end = end;

            return View();
        }


        /// <summary>
        /// 成功投资金额
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        private string GetinvAmt(string starttime, string endtime)
        {
            string strWhere = "";

            if (starttime != null && starttime != "")
            {
                strWhere += " and invest_time >= '" + starttime + " 00:00:00'";
            }
            if (endtime != null && endtime != "")
            {
                strWhere += " and invest_time <= '" + endtime + " 23:59:59'";
            }

            string sql = "select COALESCE(sum(investment_amount),0) as 成功投资金额  from hx_Bid_records  where OrdId  is not  null" + strWhere;


            return "￥" + DbHelperSQL.Re_String(sql);

        }

        /// <summary>
        /// 成功投资金额
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        private string GetbonusAmt(string starttime, string endtime)
        {
            string strWhere = "";



            if (starttime != null && starttime != "")
            {
                string dt = DateTime.Parse(starttime).ToString("yyyy-MM-dd");

                strWhere += " and Createtime >= '" + dt + " 00:00:00'";
            }
            if (endtime != null && endtime != "")
            {
                string dt = DateTime.Parse(endtime).ToString("yyyy-MM-dd");

                strWhere += " and Createtime <= '" + dt + " 23:59:59'";
            }

            //  string sql = "select COALESCE(sum(income),0.00) as 总支出奖励  from bonus_account_water  where account_water_id>0 " + strWhere;

            //string sql = "select COALESCE(sum(Amt),0.00) as 总支出奖励  from hx_UserAct  where RewTypeID<3 and UseState=1 or UseState=4 " + strWhere; 

            string sql = "select COALESCE(sum(Amt),0.00) as 总支出奖励  from hx_UserAct  where (RewTypeID=2 or  RewTypeID =1)  and (UseState=1 or UseState=4)  " + strWhere;
            return "￥" + DbHelperSQL.Re_String(sql);

        }
        private string GetyslxAmt(string starttime, string endtime)
        {

            decimal hamt = decimal.Parse(GethaikuanAmt(starttime, endtime));

            decimal famt = decimal.Parse(GetfankuanAmt(starttime, endtime));


            decimal camt = hamt + famt;
            return Math.Round(camt, 2).ToString();
        }
        /// <summary>
        /// 还款手续费
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        private string GethaikuanAmt(string starttime, string endtime)
        {
            string strWhere = "";

            if (starttime != null && starttime != "")
            {
                strWhere += " and invest_time >= '" + starttime + " 00:00:00'";
            }
            if (endtime != null && endtime != "")
            {
                strWhere += " and invest_time <= '" + endtime + " 23:59:59'";
            }

            string sql = "select  COALESCE(sum( BorrFees),0.00) as 已收返款款续费  from  hx_income_statement where payment_status=1 " + strWhere;


            return DbHelperSQL.Re_String(sql);

        }
        /// <summary>
        /// 放款手续费
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        private string GetfankuanAmt(string starttime, string endtime)
        {
            string strWhere = "";

            if (starttime != null && starttime != "")
            {
                strWhere += " and LoanDate >= '" + starttime + " 00:00:00'";
            }
            if (endtime != null && endtime != "")
            {
                strWhere += " and LoanDate <= '" + endtime + " 23:59:59'";
            }

            string sql = "select COALESCE(sum(Free),0.00) as 放款手续费   from hx_td_Loan_records where  bid_orderid is not null" + strWhere;


            return DbHelperSQL.Re_String(sql);

        }

        /// <summary>
        /// 待收本金总金额
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        private string GetysbeijAmt(string starttime, string endtime)
        {
            string strWhere = "";

            if (starttime != null && starttime != "")
            {
                strWhere += " and invest_time >= '" + starttime + " 00:00:00'";
            }
            if (endtime != null && endtime != "")
            {
                strWhere += " and invest_time <= '" + endtime + " 23:59:59'";
            }


            string sql = " select COALESCE(sum(investment_amount),0.00) as 已收本金总额  from V_hx_Bid_records_borrowing_target where payment_status=1 " + strWhere;

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            decimal amt = 0.00M;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                amt = amt + decimal.Parse(dt.Rows[i]["已收本金总额"].ToString());
            }

            return "￥" + Math.Round(amt, 2).ToString();

        }

        /// <summary>
        /// 待收本金总金额
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        private string GetdaibeiAmt(string starttime, string endtime)
        {
            string strWhere = "";

            if (starttime != null && starttime != "")
            {
                strWhere += " and invest_time >= '" + starttime + " 00:00:00'";
            }
            if (endtime != null && endtime != "")
            {
                strWhere += " and invest_time <= '" + endtime + " 23:59:59'";
            }

            // string sql = "select COALESCE(sum(withoutinterest),0) as 待收总额  from hx_Bid_records  where OrdId  is not  null" + strWhere;

            // string sql = "select COALESCE(sum( distinct  borrowing_balance),0) as 待收本金总额 from V_hx_Bid_records_borrowing_target where tender_state between 3 and 4 " + strWhere;

            string sql = " select COALESCE(sum(investment_amount),0.00) as 待收本金总额  from V_hx_Bid_records_borrowing_target where payment_status=0 " + strWhere;

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            decimal amt = 0.00M;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                amt = amt + decimal.Parse(dt.Rows[i]["待收本金总额"].ToString());
            }

            return "￥" + Math.Round(amt, 2).ToString();

        }
        /// <summary>
        /// 待收总金额
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        private string GetdaisAmt(string starttime, string endtime)
        {
            string strWhere = "";

            if (starttime != null && starttime != "")
            {
                strWhere += " and invest_time >= '" + starttime + " 00:00:00'";
            }
            if (endtime != null && endtime != "")
            {
                strWhere += " and invest_time <= '" + endtime + " 23:59:59'";
            }

            string sql = "select COALESCE(sum(withoutinterest),0) as 待收总额  from hx_Bid_records  where OrdId  is not  null" + strWhere;


            return "￥" + DbHelperSQL.Re_String(sql);

        }

        private string GetdailirenshuAmt(string starttime, string endtime)
        {
            string strWhere = "";

            if (starttime != null && starttime != "")
            {
                strWhere += " and invest_time >= '" + starttime + " 00:00:00'";
            }
            if (endtime != null && endtime != "")
            {
                strWhere += " and invest_time <= '" + endtime + " 23:59:59'";
            }

            //string sql = "select  COALESCE(sum(repayment_amount*service_charge/10),0.00) as 待收利润总额  from  V_incomeborr_count  where payment_status=0 " + strWhere;

            string sql = "select COALESCE(sum(investment_amount * service_charge / 10), 0.00) as  待收利润总额  from    V_hx_Bid_records_borrowing_target where isloans = 0 " + strWhere;


            return "￥" + Math.Round(decimal.Parse(DbHelperSQL.Re_String(sql))).ToString();

        }

        /// <summary>
        /// 已收总金额
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        private string GetHaveAmt(string starttime, string endtime)
        {
            string strWhere = "";

            if (starttime != null && starttime != "")
            {
                strWhere += " and invest_time >= '" + starttime + " 00:00:00'";
            }
            if (endtime != null && endtime != "")
            {
                strWhere += " and invest_time <= '" + endtime + " 23:59:59'";
            }

            string sql = "select COALESCE(sum(haveinterest),0.00) as 待收总额  from hx_Bid_records  where OrdId  is not  null" + strWhere;


            return "￥" + DbHelperSQL.Re_String(sql);

        }

        /// <summary>
        /// 投资人数
        /// </summary>
        /// <returns></returns>
        private string Getregcount(string starttime, string endtime)
        {
            string strWhere = "";

            if (starttime != null && starttime != "")
            {
                strWhere += " and invest_time >= '" + starttime + " 00:00:00'";
            }
            if (endtime != null && endtime != "")
            {
                strWhere += " and invest_time <= '" + endtime + " 23:59:59'";
            }

            string sql = "select count(investor_registerid) as 人数  from hx_Bid_records  where OrdId  is not  null  " + strWhere + "    group by investor_registerid";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            return dt.Rows.Count.ToString();
            // return string.Format("{0:C}",dt.Rows.Count.ToString());       

        }

        #endregion

        /// <summary>
        /// 投资金额
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Amountinvestment(string start = "", string end = "")
        {
            if (string.IsNullOrEmpty(start))
            {
                start = DateTime.Now.ToString("yyyy-MM") + "-01";
            }
            if (string.IsNullOrEmpty(end))
            {
                end = DateTime.Now.ToString("yyyy-MM-dd");
            }

            string strWhere = "";
            if (start != null && start != "")
            {
                strWhere += " and invest_time >= '" + start + " 00:00:00'";
            }
            if (end != null && end != "")
            {
                strWhere += " and invest_time <= '" + end + " 23:59:59'";
            }

            // string sql = "select  COALESCE(count(investor_registerid),0) as regcount, convert(varchar(10),invest_time,120) as invest_time,COALESCE(sum(investment_amount),0) as countAmt,(select COALESCE(sum(FrozenidAmount),0) as frozAmt from hx_td_frozen tdf  where   convert(varchar(10),tdf.FrozenDate,120)=convert(varchar(10),brd.invest_time,120)) as 冻结金额,(select COALESCE(sum(amount_of_reward),0)  from bonus_account ba where  convert(varchar(10),ba.entry_time,120)=convert(varchar(10),brd.invest_time,120)) as 已获奖励  from  V_hx_Bid_records_borrowing_target  brd   where investor_registerid>0  " + strWhere + "   group by  convert(varchar(10),invest_time,120) order by  convert(varchar(10),invest_time,120) desc";

            string sql = "select  COALESCE(count(distinct investor_registerid),0) as regcount, convert(varchar(10),invest_time,120) as invest_time,COALESCE(sum(investment_amount),0) as countAmt,(select COALESCE(sum(FrozenidAmount),0) as frozAmt from hx_td_frozen tdf  where  FrozenState=1 and  convert(varchar(10),tdf.FrozenDate,120)=convert(varchar(10),brd.invest_time,120)) as 冻结金额,(select COALESCE(sum(Amt),0)  from hx_UserAct ba where  RewTypeID=1 and UseState=4 and  convert(varchar(10),ba.Createtime,120)=convert(varchar(10),brd.invest_time,120)) as 现金奖励,(select COALESCE(sum(Amt),0)  from hx_UserAct ba where RewTypeID=2   and UseState=1  and  convert(varchar(10),ba.Createtime,120)=convert(varchar(10),brd.invest_time,120)) as 抵扣券,(select COALESCE(sum(Amt),0)  from hx_UserAct ba where RewTypeID=3  and UseState=1  and  convert(varchar(10),ba.Createtime,120)=convert(varchar(10),brd.invest_time,120)) as 优惠券  from  V_hx_Bid_records_borrowing_target  brd   where investor_registerid>0   " + strWhere + "   group by  convert(varchar(10),invest_time,120) order by  convert(varchar(10),invest_time,120) desc";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            ViewBag.start = start;
            ViewBag.end = end;

            return View(dt);
        }

        /// <summary>
        /// 投资人数
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult OutNumberInvestors(string start = "", string end = "")
        {
            if (string.IsNullOrEmpty(start))
            {
                start = DateTime.Now.ToString("yyyy-MM") + "-01";
            }
            if (string.IsNullOrEmpty(end))
            {
                end = DateTime.Now.ToString("yyyy-MM-dd");
            }
            string strWhere = "";

            if (start != null && start != "")
            {
                strWhere += " and invest_time >= '" + start + " 00:00:00'";
            }
            if (end != null && end != "")
            {
                strWhere += " and invest_time <= '" + end + " 23:59:59'";
            }

            string sql = "select  count(distinct investor_registerid) as  regcount , convert(varchar(10),invest_time,120) as invest_time ,COALESCE(sum(investment_amount),0) as  investment_amount   from  V_hx_Bid_records_borrowing_target where investor_registerid>0  and ordstate=1  " + strWhere + "  group by  convert(varchar(10),invest_time,120)   order by convert(varchar(10),invest_time,120) desc ";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);



            ViewBag.start = start;
            ViewBag.end = end;

            return View(dt);
        }

        /// <summary>
        /// 借出总统计
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Borrowingpresident(string start = "", string end = "")
        {
            //人数
            string sql = "select count(1) from( select count(investor_registerid) as 人数  from hx_Bid_records  where OrdId  is not  null  {0}  group by investor_registerid) tb";
            StringBuilder where = new StringBuilder();

            //成功投资金额,待收总额
            StringBuilder sql1 = new StringBuilder();
            sql1.Append("select COALESCE(sum(investment_amount),0) as 成功投资金额,COALESCE(sum(haveinterest),0.00) as 待收总额 ");
            sql1.Append(",COALESCE(sum(withoutinterest),0) as 待收总额1 ");
            sql1.Append("  from hx_Bid_records  where OrdId  is not  null  and   ordstate=1 ");

            //待收利润总额
            StringBuilder sql2 = new StringBuilder();

            //sql2.Append("select  COALESCE(sum(repayment_amount*service_charge/10),0.00) as 待收手续费  ");
            //sql2.Append("from  V_incomeborr_count  where payment_status=0 ");

            // sql2.Append("select COALESCE(sum(borrowing_balance*service_charge/10),0.00)  as 待收手续费   ");
            // sql2.Append("FROM  hx_borrowing_target  where   tender_state=2   ");

            // sql2.Append("select COALESCE(sum(Free),0.00) as 待收手续费   ");
            // sql2.Append(" from hx_td_Loan_records where  bid_orderid is  null ");
            sql2.Append("select COALESCE(sum(investment_amount * service_charge / 10), 0.00) as 待收手续费   ");
            sql2.Append(" FROM  V_hx_Bid_records_borrowing_target where isloans = 0  ");



            //待收本金总额,已收本金总额
            StringBuilder sql3 = new StringBuilder();
            sql3.Append("select COALESCE(sum(case when payment_status=0 then investment_amount else 0 end),0.00) as 待收本金总额,COALESCE(sum(case when payment_status=1 then investment_amount else 0 end),0.00) as 已收本金总额 ");
            sql3.Append(" from V_hx_Bid_records_borrowing_target where payment_status in (0,1) ");


            //放款手续费
            StringBuilder sql4 = new StringBuilder();
            sql4.Append("select COALESCE(sum(Free),0.00) as 放款手续费   ");
            sql4.Append(" from hx_td_Loan_records where  bid_orderid is not null ");


            //已收返款款续费
            StringBuilder sql5 = new StringBuilder();
            sql5.Append("select  COALESCE(sum( BorrFees),0.00) as 已收返款款续费  ");
            sql5.Append("from  hx_income_statement where payment_status=1 ");

            //首投人数及复投人数
            //StringBuilder sql6 = new StringBuilder();
            string whereforsql6or8 = "";
            //sql6.Append("select COUNT(*) as 'FirstCount',SUM(investment_amount) as 'FirstMoney' from (select *, rank() OVER(PARTITION BY investor_registerid ORDER BY bid_records_id) rank from hx_Bid_records where ordstate = 1");


            //当日注册并当日投资人数及金额
            StringBuilder sql7 = new StringBuilder();
            sql7.Append("select count(*) as 'DayRegisterCount',SUM(investment_amount) as 'DayRegisterMoney' from hx_Bid_records a where ordstate = 1 ");

            ////复投人数及复投金额
            //StringBuilder sql8 = new StringBuilder();
            //sql8.Append("select count(*) as 'complexCount',sum(investment_amount) as 'complexMoney' from(select *, rank() OVER(PARTITION BY investor_registerid ORDER BY bid_records_id) rank from hx_Bid_records where ordstate = 1) a where rank > 1");

            #region 条件
            if (!string.IsNullOrEmpty(start))
            {
                sql1.AppendFormat(" and invest_time >= '{0} 00:00:00'", start);
                sql2.AppendFormat(" and invest_time >= '{0} 00:00:00'", start);
                sql3.AppendFormat(" and invest_time >= '{0} 00:00:00'", start);
                sql4.AppendFormat(" and LoanDate >= '{0} 00:00:00'", start);
                sql5.AppendFormat(" and invest_time >= '{0} 00:00:00'", start);
                where.AppendFormat(" and invest_time >= '{0} 00:00:00'", start);
                whereforsql6or8 += " invest_time<='"+start+ " 00:00:00'";
                sql7.AppendFormat(" and invest_time >='{0} 00:00:00'", start);

            }
            if (!string.IsNullOrEmpty(end))
            {
                sql1.AppendFormat(" and invest_time <= '{0} 23:59:59'", end);
                sql2.AppendFormat(" and invest_time <= '{0} 23:59:59'", end);
                sql3.AppendFormat(" and invest_time <= '{0} 23:59:59'", end);
                sql4.AppendFormat(" and LoanDate <= '{0} 23:59:59'", end);
                sql5.AppendFormat(" and invest_time <= '{0} 23:59:59'", end);
                where.AppendFormat(" and invest_time <= '{0} 23:59:59'", end);

                sql7.AppendFormat(" and invest_time <='{0} 23:59:59'", end);
            }

            //查询首投人数
            StringBuilder sql6 = new StringBuilder();
 
            //查询首投金额
            StringBuilder sql6formoney = new StringBuilder();

            //复投人数
            StringBuilder sql8 = new StringBuilder();

            //复投金额
            StringBuilder sql8formoney = new StringBuilder();

            if (whereforsql6or8 != "")
            {
                sql6.AppendFormat("select count(distinct investor_registerid) '首投人数' from hx_Bid_records where invest_time >= '{0} 00:00:00' and invest_time <= '{1} 23:59:59' and investor_registerid not in(select investor_registerid from hx_Bid_records where {2})", start,end,whereforsql6or8);

                sql6formoney.AppendFormat(" select sum(investment_amount) '首投金额' from (select * , rank() OVER(PARTITION BY investor_registerid ORDER BY invest_time)  rank from hx_Bid_records where invest_state = 1 and invest_time >= '{0} 00:00:00' and invest_time <= '{1} 23:59:59' and investor_registerid not in (select investor_registerid from hx_Bid_records where invest_time < '{2} 00:00:00')) a where rank = 1",start,end,start);

                sql8.AppendFormat(" select (select count(distinct investor_registerid) '复投人数' from hx_Bid_records where invest_time >= '{0} 00:00:00' and invest_time <= '{1}  23:59:59' and investor_registerid in (select investor_registerid from hx_Bid_records where invest_time < '{2} 00:00:00'))+(select count(*) from(select investor_registerid '复投人数' from hx_Bid_records where invest_time >= '{3} 00:00:00' and invest_time <= '{4} 23:59:59' group by investor_registerid having count(*) > 1)c) '复投人数'",start,end,start,start,end);

                sql8.AppendFormat(" select count(distinct investor_registerid) '复投人数' from hx_Bid_records where invest_time >= '{0} 00:00:00' and invest_time <= '{1} 23:59:59' and investor_registerid in(select investor_registerid from hx_Bid_records where {2})",start,end,whereforsql6or8);

                sql8formoney.AppendFormat(" select(select sum(investment_amount) from(select * , rank() OVER(PARTITION BY investor_registerid ORDER BY invest_time)  rank from hx_Bid_records where invest_state = 1 and invest_time >= '{0} 00:00:00' and invest_time <= '{1} 23:59:59' and investor_registerid in (select investor_registerid from hx_Bid_records where invest_time < '{2} 00:00:00')) a)+(select sum(investment_amount) from(select * , rank() OVER(PARTITION BY investor_registerid ORDER BY invest_time)  rank from hx_Bid_records where invest_state = 1 and invest_time >= '{3} 00:00:00' and invest_time <= '{4} 23:59:59'and investor_registerid not in (select investor_registerid from hx_Bid_records where invest_time < '{5} 00:00:00')) b where rank > 1)as '复投金额'",start,end,start,start,end,start);
            }
            else
            {

                sql6.Append("select count(distinct investor_registerid) '首投人数' from hx_Bid_records");

                sql6formoney.Append("select sum(investment_amount) '首投金额' from (select * , rank() OVER(PARTITION BY investor_registerid ORDER BY invest_time)  rank from hx_Bid_records where invest_state = 1) a where rank = 1");

                sql8.Append("select count(*) '复投人数' from(select investor_registerid '复投人数' from hx_Bid_records group by investor_registerid having count(*) > 1) a");

                sql8formoney.Append("select sum(investment_amount) '复投金额' from (select * , rank() OVER(PARTITION BY investor_registerid ORDER BY invest_time)  rank from hx_Bid_records where invest_state = 1) a where rank > 1");
            }


            sql7.Append("and investor_registerid in(select registerid from [dbo].[hx_member_table] where CONVERT(varchar(12) , registration_time, 112 )=CONVERT(varchar(12) ,a.invest_time, 112 ))");
            #endregion
            var num = DbHelperSQL.Re_String(string.Format(sql, where.ToString()));
            var invAmt = "0";
            var daisAmt = "0";
            var daibeiAmt = "0";
            var HaveAmt = "0";
            var daishuAmt = "0";
            var ysbeijAmt = "0";
            var yslxAmt = "0";
            var firstcount = "0";
            var firstmoney = "0";
            var dayregistercount = "0";
            var dayregistermoney = "0";
            var complexcount = "0";
            var complexmoney = "0";
            DataTable dt1 = DbHelperSQL.GET_DataTable_List(sql1.ToString());
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                DataRow dr = dt1.Rows[0];
                invAmt = "￥" + dr["成功投资金额"].ToString();
                daisAmt = "￥" + dr["待收总额1"].ToString();
                HaveAmt = "￥" + dr["待收总额"].ToString();
            }
            DataTable dt2 = DbHelperSQL.GET_DataTable_List(sql2.ToString());
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                daishuAmt = "￥" + decimal.Round(Convert.ToDecimal(dt2.Rows[0]["待收手续费"].ToString()), 2);
            }
            DataTable dt3 = DbHelperSQL.GET_DataTable_List(sql3.ToString());
            if (dt3 != null && dt3.Rows.Count > 0)
            {
                daibeiAmt = "￥" + dt3.Rows[0]["待收本金总额"].ToString();
                ysbeijAmt = "￥" + dt3.Rows[0]["已收本金总额"].ToString();
            }

            DataTable dt5 = DbHelperSQL.GET_DataTable_List(sql7.ToString());
            if (dt5 != null && dt5.Rows.Count > 0)
            {
                dayregistercount= dt5.Rows[0]["DayRegisterCount"].ToString();
                dayregistermoney= "￥" + dt5.Rows[0]["DayRegisterMoney"].ToString();
            }
            DataTable dt6 = DbHelperSQL.GET_DataTable_List(sql6.ToString());
            if (dt6 != null && dt6.Rows.Count > 0)
            {
                firstcount = dt6.Rows[0]["首投人数"].ToString();
            }

            DataTable dt6formoney = DbHelperSQL.GET_DataTable_List(sql6formoney.ToString());
            if (dt6formoney != null && dt6formoney.Rows.Count > 0)
            {
                firstmoney = dt6formoney.Rows[0]["首投金额"].ToString();
            }

            DataTable dt8 = DbHelperSQL.GET_DataTable_List(sql8.ToString());
            if (dt8 != null && dt8.Rows.Count > 0)
            {
                complexcount = dt8.Rows[0]["复投人数"].ToString();
            }

            DataTable dt8formoney = DbHelperSQL.GET_DataTable_List(sql8formoney.ToString());
            if (dt8formoney != null && dt8formoney.Rows.Count > 0)
            {
                complexmoney = dt8formoney.Rows[0]["复投金额"].ToString();
            }

            decimal famt = decimal.Parse(DbHelperSQL.Re_String(sql4.ToString()));
            decimal hamt = decimal.Parse(DbHelperSQL.Re_String(sql5.ToString()));
            decimal camt = hamt + famt;
            yslxAmt = Math.Round(camt, 2).ToString();

            ViewBag.num = num;
            ViewBag.invAmt = invAmt;
            ViewBag.daisAmt = daisAmt;
            ViewBag.daibeiAmt = daibeiAmt;
            ViewBag.HaveAmt = HaveAmt;
            ViewBag.daishuAmt = daishuAmt;
            ViewBag.ysbeijAmt = ysbeijAmt;
            ViewBag.yslxAmt = yslxAmt;
            ViewBag.firstcount = firstcount;
            ViewBag.firstmoney = firstmoney;
            ViewBag.dayregistercount = dayregistercount;
            ViewBag.dayregistermoney = dayregistermoney;
            ViewBag.complexcount = complexcount;
            ViewBag.complexMoney = complexmoney;
            ViewBag.start = start;
            ViewBag.end = end;

            return View();
        }

        /// <summary>
        /// 返现统计
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult CashbackStatistics(string userName = "", string investTotalAmountFrom = "", string investTotalAmountTo = "", string projectTerm = "", string cashbackTotalAmountFrom = "", string cashbackTotalAmountTo = "", string cashbackStatus = "", int Page = 1, int pageSize = 20)
        {
            IPagedList<Statistics_ActiveSepteberCashback> list = null;
            ViewBag.CashbackStatusList = new List<SelectListItem> { new SelectListItem {Text="未返现",Value="未返现" },
                new SelectListItem {Text="已返现",Value="已返现" }};
            ViewBag.ProjectTermList = new List<SelectListItem> { new SelectListItem {Text="1个月",Value="1" },
                new SelectListItem {Text="3个月",Value="3" },new SelectListItem {Text="6个月",Value="6" }};
            var query = from item in ef.Statistics_ActiveSepteberCashback
                        orderby item.InvestTerm, item.InvestTotalAmount descending
                        select item;
            IQueryable<Statistics_ActiveSepteberCashback> queryOrdered = query.Where(p => 1 == 1);
            if (!string.IsNullOrWhiteSpace(userName))
            {
                queryOrdered = queryOrdered.Where(p => p.UserName == userName);
            }
            if (!string.IsNullOrWhiteSpace(projectTerm))
            {
                queryOrdered = queryOrdered.Where(p => p.InvestTerm.ToString() == projectTerm);
            }
            if (!string.IsNullOrWhiteSpace(cashbackStatus))
            {
                int hasCashback = cashbackStatus == "未返现" ? 0 : 1;
                queryOrdered = queryOrdered.Where(p => p.HasCashback == hasCashback);
            }
            decimal tmpValue1 = 0.00M;
            decimal tmpValue2 = 0.00M;
            if (!string.IsNullOrEmpty(investTotalAmountFrom) && !string.IsNullOrWhiteSpace(investTotalAmountTo))
            {
                if (decimal.TryParse(investTotalAmountFrom, out tmpValue1) && decimal.TryParse(investTotalAmountTo, out tmpValue2))
                    queryOrdered = queryOrdered.Where(p => p.InvestTotalAmount >= tmpValue1 && p.InvestTotalAmount <= tmpValue2);
                else
                    investTotalAmountTo = investTotalAmountFrom = string.Empty;
            }
            else if (!string.IsNullOrEmpty(investTotalAmountFrom) && string.IsNullOrWhiteSpace(investTotalAmountTo))
            {
                if (decimal.TryParse(investTotalAmountFrom, out tmpValue1))
                    queryOrdered = queryOrdered.Where(p => p.InvestTotalAmount >= tmpValue1);
                else
                    investTotalAmountTo = investTotalAmountFrom = string.Empty;
            }
            else if (string.IsNullOrEmpty(investTotalAmountFrom) && !string.IsNullOrWhiteSpace(investTotalAmountTo))
            {
                if (decimal.TryParse(investTotalAmountTo, out tmpValue2))
                    queryOrdered = queryOrdered.Where(p => p.InvestTotalAmount <= tmpValue2);
                else
                    investTotalAmountTo = investTotalAmountFrom = string.Empty;
            }
            decimal tmpValue3 = 0.00M;
            decimal tmpValue4 = 0.00M;
            if (!string.IsNullOrWhiteSpace(cashbackTotalAmountFrom) && !string.IsNullOrWhiteSpace(cashbackTotalAmountTo))
            {
                if (decimal.TryParse(cashbackTotalAmountFrom, out tmpValue3) && decimal.TryParse(cashbackTotalAmountTo, out tmpValue4))
                    queryOrdered = queryOrdered.Where(p => p.CashbackAmount >= tmpValue3 && p.CashbackAmount <= tmpValue4);
                else
                    cashbackTotalAmountFrom = cashbackTotalAmountTo = string.Empty;
            }
            else if (!string.IsNullOrWhiteSpace(cashbackTotalAmountFrom) && string.IsNullOrWhiteSpace(cashbackTotalAmountTo))
            {
                if (decimal.TryParse(cashbackTotalAmountFrom, out tmpValue3))
                    queryOrdered = queryOrdered.Where(p => p.CashbackAmount >= tmpValue3);
                else
                    cashbackTotalAmountFrom = cashbackTotalAmountTo = string.Empty;
            }
            else if (string.IsNullOrWhiteSpace(cashbackTotalAmountFrom) && !string.IsNullOrWhiteSpace(cashbackTotalAmountTo))
            {
                if (decimal.TryParse(cashbackTotalAmountTo, out tmpValue4))
                    queryOrdered = queryOrdered.Where(p => p.CashbackAmount <= tmpValue4);
                else
                    cashbackTotalAmountFrom = cashbackTotalAmountTo = string.Empty;
            }
            list = queryOrdered.ToPagedList(Page, pageSize);

            ViewBag.JoinMemberCount = (from item in ef.Statistics_ActiveSepteberCashback select item.UserName).Distinct().Count();
            ViewBag.InvestTotalAmount = 0;
            try
            {
                ViewBag.InvestTotalAmount = (from item in ef.Statistics_ActiveSepteberCashback select item.InvestTotalAmount).Sum();
            }
            catch { }
            ViewBag.HasCalculate = (from item in ef.Statistics_ActiveSepteberCashback select item.UserName).Count() > 0 ? 1 : 0;
            ViewBag.username = userName;
            ViewBag.investTotalAmountFrom = investTotalAmountFrom;
            ViewBag.investTotalAmountTo = investTotalAmountTo;
            ViewBag.projectTerm = projectTerm;
            ViewBag.cashbackTotalAmountFrom = cashbackTotalAmountFrom;
            ViewBag.cashbackTotalAmountTo = cashbackTotalAmountTo;
            ViewBag.cashbackStatus = cashbackStatus;
            queryOrdered = null;
            query = null;
            GC.Collect();
            return View(list);
        }
        [AdminVaildate()]
        public ActionResult CashbackStatisticsToExcel()
        {
            DataTable dt = DataTableHelper.CreateDataTableSimple(new List<string> { "Id", "UserName", "InvestTerm", "InvestTotalAmount", "InvestTimes", "CashbackAmount", "HasCashback" });
            var query = from table in ef.Statistics_ActiveSepteberCashback
                        select new
                        {
                            table.Id,
                            table.UserName,
                            table.InvestTerm,
                            table.InvestTimes,
                            table.InvestTotalAmount,
                            table.CashbackAmount,
                            HasCashback = table.HasCashback == 0 ? "未返现" : "已返现"
                        };
            foreach (var item in query)
            {
                DataRow dr = dt.NewRow();
                dr["Id"] = item.Id;
                dr["UserName"] = item.UserName;
                dr["InvestTerm"] = item.InvestTerm;
                dr["InvestTimes"] = item.InvestTimes;
                dr["InvestTotalAmount"] = item.InvestTotalAmount;
                dr["CashbackAmount"] = item.CashbackAmount;
                dr["HasCashback"] = item.HasCashback;
                dt.Rows.Add(dr);
            }
            dt.Columns["Id"].ColumnName = "编号";
            dt.Columns["UserName"].ColumnName = "用户名";
            dt.Columns["InvestTerm"].ColumnName = "项目期限";
            dt.Columns["InvestTotalAmount"].ColumnName = "累计投资金额(元)";
            dt.Columns["InvestTimes"].ColumnName = "累计投资笔数";
            dt.Columns["CashbackAmount"].ColumnName = "累计应返现(元)";
            dt.Columns["HasCashback"].ColumnName = "状态";

            var path = Extensions.ExportExcel(dt);
            return Content(path);
        }
        [AdminVaildate()]
        public ActionResult CashbackStatisticsCalculate()
        {
            var userid = Utils.GetAdmUserID();
            var controllerName = this.RouteData.Values["controller"].ToString();
            var actionName = this.RouteData.Values["action"].ToString();
            if (!new UserLimitByEF().CheckAdminLimit(userid, controllerName, actionName))
            {   //无权限
                return Content(StringAlert.Alert("您没有操作权限"));
            }
            string sql = "select id from [dbo].[Statistics_ActiveSepteberCashback]";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count < 1)
            {
                sql = " insert into [dbo].[Statistics_ActiveSepteberCashback](UserId,UserName,InvestTerm,InvestTimes,InvestTotalAmount,CashbackAmount,HasCashback) select b.registerid,b.username,c.life_of_loan,InvestTimes = count(a.investment_amount),sum(a.investment_amount) as investTotalAmount, CashbackAmount = case when life_of_loan = 1 and sum(a.investment_amount - BonusAmt) < 10000 then 0.00 when life_of_loan = 1 and sum(a.investment_amount - BonusAmt) < 30000 and sum(a.investment_amount - BonusAmt) >= 10000 then 49.00 when life_of_loan = 1 and sum(a.investment_amount - BonusAmt) < 50000 and sum(a.investment_amount - BonusAmt) >= 30000 then 149.00 when life_of_loan = 1 and sum(a.investment_amount - BonusAmt) < 100000 and sum(a.investment_amount - BonusAmt) >= 50000 then 249.00 when life_of_loan = 1 and sum(a.investment_amount - BonusAmt) >= 100000 then sum(a.investment_amount - BonusAmt) * 0.005 - 1 when life_of_loan = 3 and sum(a.investment_amount - BonusAmt) < 10000 then 0.00 when life_of_loan = 3 and sum(a.investment_amount - BonusAmt) < 30000 and sum(a.investment_amount - BonusAmt) >= 10000 then 99.00 when life_of_loan = 3 and sum(a.investment_amount - BonusAmt) < 50000 and sum(a.investment_amount - BonusAmt) >= 30000 then 299.00 when life_of_loan = 3 and sum(a.investment_amount - BonusAmt) < 100000 and sum(a.investment_amount - BonusAmt) >= 50000 then 499.00 when life_of_loan = 3 and sum(a.investment_amount - BonusAmt) >= 100000 then sum(a.investment_amount - BonusAmt) * 0.01 - 1 when life_of_loan = 6 and sum(a.investment_amount - BonusAmt) < 10000 then 0.00 when life_of_loan = 6 and sum(a.investment_amount - BonusAmt) < 30000 and sum(a.investment_amount - BonusAmt) >= 10000 then 199.00 when life_of_loan = 6 and sum(a.investment_amount - BonusAmt) < 50000 and sum(a.investment_amount - BonusAmt) >= 30000 then 599.00 when life_of_loan = 6 and sum(a.investment_amount - BonusAmt) < 100000 and sum(a.investment_amount - BonusAmt) >= 50000 then 999.00 when life_of_loan = 6 and sum(a.investment_amount - BonusAmt) >= 100000 then sum(a.investment_amount - BonusAmt) * 0.02 - 1 end, 0 as HasCashback from[hx_Bid_records] a left  join[hx_member_table] b on registerid = a.investor_registerid left  join[hx_borrowing_target] c on c.targetid = a.targetid where a.invest_state = 1 and a.invest_time >= '2016-9-1 00:00:00' and a.invest_time < '2016-10-01 00:00:00'and b.registration_time < '2016-09-01 00:00:00' and c.life_of_loan in (1, 3, 6) and c.unit_day = 1 and a.[ordstate]=1 group by username,life_of_loan,registerid order by life_of_loan, investTotalAmount  desc ";

                //sql = " insert into [dbo].[Statistics_ActiveSepteberCashback](UserId,UserName,InvestTerm,InvestTimes,InvestTotalAmount,CashbackAmount,HasCashback) select b.registerid,b.username,c.life_of_loan,InvestTimes = count(a.investment_amount),sum(a.investment_amount) as investTotalAmount, CashbackAmount = case when life_of_loan = 1 and sum(a.investment_amount - BonusAmt) < 10000 then 0.00 when life_of_loan = 1 and sum(a.investment_amount - BonusAmt) < 30000 and sum(a.investment_amount - BonusAmt) >= 10000 then 49.00 when life_of_loan = 1 and sum(a.investment_amount - BonusAmt) < 50000 and sum(a.investment_amount - BonusAmt) >= 30000 then 149.00 when life_of_loan = 1 and sum(a.investment_amount - BonusAmt) < 100000 and sum(a.investment_amount - BonusAmt) >= 50000 then 249.00 when life_of_loan = 1 and sum(a.investment_amount - BonusAmt) >= 100000 then sum(a.investment_amount - BonusAmt) * 0.005 - 1 when life_of_loan = 3 and sum(a.investment_amount - BonusAmt) < 10000 then 0.00 when life_of_loan = 3 and sum(a.investment_amount - BonusAmt) < 30000 and sum(a.investment_amount - BonusAmt) >= 10000 then 99.00 when life_of_loan = 3 and sum(a.investment_amount - BonusAmt) < 50000 and sum(a.investment_amount - BonusAmt) >= 30000 then 299.00 when life_of_loan = 3 and sum(a.investment_amount - BonusAmt) < 100000 and sum(a.investment_amount - BonusAmt) >= 50000 then 499.00 when life_of_loan = 3 and sum(a.investment_amount - BonusAmt) >= 100000 then sum(a.investment_amount - BonusAmt) * 0.01 - 1 when life_of_loan = 6 and sum(a.investment_amount - BonusAmt) < 10000 then 0.00 when life_of_loan = 6 and sum(a.investment_amount - BonusAmt) < 30000 and sum(a.investment_amount - BonusAmt) >= 10000 then 199.00 when life_of_loan = 6 and sum(a.investment_amount - BonusAmt) < 50000 and sum(a.investment_amount - BonusAmt) >= 30000 then 599.00 when life_of_loan = 6 and sum(a.investment_amount - BonusAmt) < 100000 and sum(a.investment_amount - BonusAmt) >= 50000 then 999.00 when life_of_loan = 6 and sum(a.investment_amount - BonusAmt) >= 100000 then sum(a.investment_amount - BonusAmt) * 0.02 - 1 end, 0 as HasCashback from[hx_Bid_records] a left  join[hx_member_table] b on registerid = a.investor_registerid left  join[hx_borrowing_target] c on c.targetid = a.targetid where a.invest_state = 1 and a.invest_time >= '2016-9-1 00:00:00' and a.invest_time < '2016-10-10 00:00:00'and b.registration_time < '2016-09-01 00:00:00' and c.life_of_loan in (1, 3, 6) and c.unit_day = 1 and a.[ordstate]=1 group by username,life_of_loan,registerid order by life_of_loan, investTotalAmount  desc ";// for test only
                DbHelperSQL.ExecuteSql(sql);
            }
            else
            {
                return Content("已经统计过,请勿重复统计,此次统计操作无效!", "text/html");
            }
            ViewBag.HasCalculate = 1;
            return Content("统计成功,以后可以发放奖励了!", "text/html");
        }
        [AdminVaildate()]
        public ActionResult Cashback()
        {
            //insert into[onchuangtou].[dbo].[hx_ActivityTable]([ActTypeId]      ,[RewTypeID]      ,[ActName]      ,[ActUser]      ,[ActStarttime]      ,[ActEndtime]      ,[ActRule]      ,[ActState])values(4,1,'复投返现金','2','2016-09-01 00:00:00.000','2016-09-30 23:59:59.999','{"rule":2,"cash":0,"ISsplit":2,"Uses":2,"Msplitarr":[{"cashAmt":49,"startAmt":0,"endAmt":0},{"cashAmt":99,"startAmt":0,"endAmt":0},{"cashAmt":199,"startAmt":0,"endAmt":0},{"cashAmt":149,"startAmt":0,"endAmt":0},{ "cashAmt":299,"startAmt":0,"endAmt":0},{"cashAmt":599,"startAmt":0,"endAmt":0},{"cashAmt":249,"startAmt":0,"endAmt":0},{"cashAmt":499,"startAmt":0,"endAmt":0},{"cashAmt":999,"startAmt":0,"endAmt":0},{"cashAmt":0.005,"startAmt":0,"endAmt":0},{"cashAmt":0.01,"startAmt":0,"endAmt":0},{"cashAmt":0.02,"startAmt":0,"endAmt":0},],"MAmtList":null}',2)
            //ID:323

            var query = from table in ef.Statistics_ActiveSepteberCashback select table;
            var items = query.ToList();
            foreach (var item in items)
            {
                if (item.HasCashback == 1) continue;
                int Registerid = item.UserId;
                decimal actamt = item.CashbackAmount;
                hx_ActivityTable hat = new ActFacade().GetActivityModel(323);
                hx_UserAct hua = new hx_UserAct();
                hua.ActTypeId = hat.ActTypeId;
                hua.registerid = Registerid;
                hua.RewTypeID = hat.RewTypeID;
                hua.ActID = hat.ActID;
                hua.Amt = actamt;
                hua.Uselower = 0.00M;
                hua.Usehight = 0.00M;
                hua.AmtEndtime = DateTime.Parse(hat.ActEndtime.ToString()).AddMonths(1);
                hua.AmtUses = 1; //没指定情况下默认为单独使用
                hua.UseState = 5;  //现金未转账
                hua.UseTime = DateTime.Now;
                hua.AmtProid = 0; //未使用默认为0
                hua.ISSmsOne = 0;
                hua.IsSmsThree = 0;
                hua.isSmsFifteen = 0;
                hua.IsSmsSeven = 0;
                hua.isSmsSixteen = 0;
                hua.OrderID = decimal.Parse(Utils.Createcode());
                hua.Createtime = DateTime.Now;
                hua.Title = hat.ActName;
                hua.UseLifeLoan = "";
                #region hand out cash function
                ef.hx_UserAct.Add(hua);
                int i = ef.SaveChanges();
                if (i > 0)
                {
                    //录入成功，后进行转账操作
                    //1.获取用户对向
                    M_member_table p = new M_member_table();
                    B_member_table o = new B_member_table();
                    p = o.GetModel(Registerid);

                    if (p != null)
                    {
                        //2.调用商户向用户转账接口
                        Transfer tf = new Transfer();
                        ReTransfer retf = tf.ToUserTransfer(p.UsrCustId, actamt, hua.OrderID.ToString(), hua.ActID.ToString(), "/Thirdparty/ToUserTransfer");
                        if (retf != null)
                        {
                            if (retf.RespCode == "000")
                            {
                                //3.事务处理操作账户及插入流水

                                #region 验签缓存处理
                                string cachename = retf.OrdId + "ToUserTransfer" + retf.InCustId;

                                if (Utils.GeTThirdCache(cachename) == 0)
                                {
                                    Utils.SetThirdCache(cachename);
                                    B_usercenter BUC = new B_usercenter();
                                    int ic = BUC.UpateActToUserTransfer(retf, 0);  //用户余更新
                                    if (ic > 0)
                                    {
                                        string sql = "SELECT registerid,username,mobile  from hx_member_table where UsrCustId='" + retf.InCustId + "'";
                                        DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                                        if (dt.Rows.Count > 0)
                                        {
                                            /*短信接口*/


                                            #region 流水信息
                                            B_usercenter ors = new B_usercenter();
                                            decimal di = ors.GetUsridAvailable_balance(int.Parse(dt.Rows[0]["registerid"].ToString()));
                                            // di = di + decimal.Parse(hua.Amt.ToString());
                                            StringBuilder strSql = new StringBuilder();
                                            strSql.Append("insert into hx_Capital_account_water(");
                                            strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                                            strSql.Append(" values (");
                                            strSql.Append("" + int.Parse(dt.Rows[0]["registerid"].ToString()) + "," + decimal.Parse(hua.Amt.ToString()) + ",0,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + di + "," + (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.现金奖励.ToString()) + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'" + "现金奖励" + "')");

                                            DbHelperSQL.RunSql(strSql.ToString());

                                            strSql.Clear();
                                            #endregion

                                            #region 奖励流水
                                            M_bonus_account_water mbaw = new M_bonus_account_water();
                                            B_bonus_account_water bbaw = new B_bonus_account_water();
                                            DateTime dte = DateTime.Now;
                                            mbaw.bonus_account_id = int.Parse(hua.ActID.ToString());
                                            mbaw.membertable_registerid = int.Parse(dt.Rows[0]["registerid"].ToString());
                                            mbaw.income = decimal.Parse(retf.TransAmt);
                                            mbaw.expenditure = 0.00M;
                                            mbaw.time_of_occurrence = DateTime.Now;

                                            mbaw.award_description = hat.ActName + "奖励已汇入个人账户";
                                            mbaw.water_type = 0;
                                            bbaw.Add(mbaw);

                                            #endregion


                                            #region MyRegion  系统消息
                                            DateTime dti = DateTime.Now;
                                            M_td_System_message pm = new M_td_System_message();
                                            pm.MReg = int.Parse(dt.Rows[0]["registerid"].ToString());
                                            pm.Mstate = 0;
                                            pm.MTitle = hat.ActName;
                                            pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功参与" + hat.ActName + "活动，并获得现金奖励 " + retf.TransAmt + "元。如有任何问题或疑惑请咨询创利投的客服！";
                                            pm.PubTime = dti;
                                            B_usercenter.AddMessage(pm);
                                            #endregion
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
                #endregion
                var data = ef.Statistics_ActiveSepteberCashback.Where(q => q.UserId == item.UserId && q.InvestTerm == item.InvestTerm && q.CashbackAmount == item.CashbackAmount).First();
                if (data != null)
                {
                    data.HasCashback = 1;
                    ef.SaveChanges();
                }
            }
            return Content("发放成功!", "text/html");
        }
        [AdminVaildate()]
        public ActionResult CashbackStatisticsClear()
        {
            DbHelperSQL.ExecuteSql("delete from [dbo].[Statistics_ActiveSepteberCashback]");
            return Content("数据清理完成,请重新计算!", "text/html");
        }
    }
}