using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuangLitouP2P.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Text;
using ChuanglitouP2P.DBUtility;
using System.Data;
using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using EntityFramework.Extensions;
using System.Web.UI;
using ChuanglitouP2P.Models;
using System.Xml.Linq;
using ChuanglitouP2P.Model.chinapnr.UsrUnFreeze;
using System.Collections.Specialized;
using System.Net;
using ChuanglitouP2P.Model.chinapnr.QueryBalanceBg;
using System.Threading.Tasks;
using ChuanglitouP2P.Common.chinapnr;
using ChuanglitouP2P.BLL;
using System.Data.SqlClient;
using ChuanglitouP2P.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Configuration;
using System.Globalization;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UsersController : Controller
    {
        IsoDateTimeConverter timeFormat = new IsoDateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
        };
        chuangtouEntities ef = new chuangtouEntities();

        #region 会员

        // GET: Admin/Users
        [AdminVaildate()]
        public ActionResult Index(string username = "", string realname = "", string mobile = "", int useridentity = -1, int Channelsource = -1, int usertypes = -1, string time1 = "", string time2 = "", int Page = 1, int pageSize = 10, string hid_OrderByFiled = "")
        {
            int pageNumber = Page / 1;
            Expression<Func<hx_member_table, bool>> where = PredicateExtensionses.True<hx_member_table>();
            where = where.And(p => p.registerid > 0);
            #region 查询条件
            if (!string.IsNullOrEmpty(username))
            {
                where = where.And(p => p.username.Contains(username));
            }
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(p => p.realname.Contains(realname));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                where = where.And(p => p.mobile.Contains(mobile));
            }
            if (useridentity >= 0)
            {
                where = where.And(p => p.useridentity == useridentity);
            }
            if (Channelsource >= 0)
            {
                where = where.And(p => p.Channelsource == Channelsource);
            }
            if (usertypes >= 0)
            {
                where = where.And(p => p.usertypes == usertypes);
            }
            if (!string.IsNullOrEmpty(time1))
            {
                DateTime dt1 = Convert.ToDateTime(time1);
                where = where.And(a => ((DateTime)a.registration_time).CompareTo(dt1) >= 0);
            }
            if (!string.IsNullOrEmpty(time2))
            {
                DateTime dt2 = Convert.ToDateTime(time2);
                dt2 = Convert.ToDateTime(dt2.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(a => ((DateTime)a.registration_time).CompareTo(dt2) <= 0);
            }
            #endregion
            var query = ef.hx_member_table.Where(where);
            if (!string.IsNullOrWhiteSpace(hid_OrderByFiled))
            {
                string[] vals = hid_OrderByFiled.Split(',');
                string filedFilter = vals[1];
                if (vals[0] == "asc")
                {
                    if (filedFilter == "zhuceshijian")
                    {
                        query = query.OrderBy(c => c.registration_time);
                    }
                    else
                    {
                        query = query.OrderBy(c => filedFilter == "zongzichan" ? c.account_total_assets : (filedFilter == "keyong" ? c.available_balance : c.frozen_sum));
                    }
                }
                else
                {
                    if (filedFilter == "zhuceshijian")
                    {
                        query = query.OrderByDescending(c => c.registration_time);
                    }
                    else
                    {
                        query = query.OrderByDescending(c => filedFilter == "zongzichan" ? c.account_total_assets : (filedFilter == "keyong" ? c.available_balance : c.frozen_sum));
                    }
                }
                ViewBag.OrderB = vals[0];
                ViewBag.HidValOrderB = vals[0] + "," + filedFilter;
            }
            else
            {
                query = query.OrderByDescending(c => c.registerid);
            }
            IPagedList<hx_member_table> list = query.ToPagedList(pageNumber, pageSize);

            ViewBag.username = username;
            ViewBag.realname = realname;
            ViewBag.mobile = mobile;
            ViewBag.useridentity = useridentity;
            ViewBag.Channelsource = Channelsource;
            ViewBag.UserTypes = usertypes;
            ViewBag.time1 = time1;
            ViewBag.time2 = time2;
            ViewBag.TotalItemCount = list.TotalItemCount;
            ViewBag.TotalPageCount = (list.TotalItemCount - 1) / pageSize + 1;


            string sql = "select SUM(investment_amount)investment_amount, COUNT(distinct investor_registerid) investor_registerid  from hx_Bid_records where ordstate = 1 and payment_status = 0 and IsLoans = 1";//获取在投金额与再投用户数
            DataTable dtcode = DbHelperSQL.GET_DataTable_List(sql);
            if (dtcode.Rows.Count > 0)
            {
                ViewBag.InvestmentAmount = dtcode.Rows[0]["investment_amount"].ToString(); //在投金额
                ViewBag.InvestorRegisterid = dtcode.Rows[0]["investor_registerid"].ToString(); //再投用户数
            }
            List<int> registerIDs = list.Select(c => c.registerid).ToList();
            var investAmounts = (from item in ef.hx_Bid_records
                                 where registerIDs.Contains(item.investor_registerid ?? 0) && item.payment_status == 0 && item.ordstate == 1 && item.IsLoans == 1
                                 group item by item.investor_registerid into g
                                 select new
                                 {
                                     registerID = g.Key,
                                     investAmount = g.Sum(c => c.investment_amount ?? 0)
                                 }).ToList();
            foreach (var item in list)
            {
                var tmpIA = investAmounts.Where(c => c.registerID == item.registerid).FirstOrDefault();
                decimal ia = tmpIA == null ? 0 : tmpIA.investAmount;
                item.account_total_assets = item.available_balance + item.frozen_sum + ia;
            }
            //string sql = @"select COALESCE(sum(investment_amount), 0)  as investment_amount from hx_Bid_records where investor_registerid = " + register.ToString() + " and payment_status = 0 and ordstate = 1 and IsLoans = 1";
            return View(list);
        }

        public ActionResult CapitalDetail(int membertable_registerid, int Page = 1, int pageSize = 20)
        {
            int pageNumber = Page / 1;
            Expression<Func<hx_Capital_account_water, bool>> where = PredicateExtensionses.True<hx_Capital_account_water>();
            where = where.And(p => p.membertable_registerid == membertable_registerid);


            IPagedList<hx_Capital_account_water> list = ef.hx_Capital_account_water.Where(where).OrderByDescending(p => p.account_water_id).ToPagedList(pageNumber, pageSize);

            ViewBag.MembertableRegisterid = membertable_registerid;
            ViewBag.TotalItemCount = list.TotalItemCount;
            return View(list);
        }
        public ActionResult IndexToExcel(string username = "", string realname = "", string mobile = "", int useridentity = -1, int Channelsource = -1, int usertypes = -1, string time1 = "", string time2 = "")
        {
            var userid = Utils.GetAdmUserID();
            var controllerName = this.RouteData.Values["controller"].ToString();
            var actionName = this.RouteData.Values["action"].ToString();
            if (!new UserLimitByEF().CheckAdminLimit(userid, controllerName, actionName))
            {   //无权限
                return Content("您没有操作权限");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT registerid as '用户ID',realname as '姓名'");
            sql.Append(",case when iD_number is null then '' when len(iD_number) = 15 then '19' + substring(iD_number, 7, 6) when len(iD_number) = 18 then substring(iD_number,7,4)+'年'+ substring(iD_number,11,2)+'月' end as '出生年月' ");
            sql.Append(",case when ISNULL(open_tonto_account,0)=0 then '否' else '是' end as 托管 ");
            sql.Append(",case ISNULL(useridentity,0) when 6 then '钻石' when 1 then 'VIP' when 2 then '黄金' when 3 then '虚假' when 4 then '渠道' when 5 then '白金' else '普通' end as '等级' ");
            sql.Append(",account_total_assets as '总资产',available_balance as '可用',frozen_sum as '冻结',convert(varchar(10),registration_time,120) as '注册时间' ");
            sql.Append(",lastlogintime as '最后登录时间',case ISNULL(Channelsource,0) when 0 then 'pc注册' when 1 then 'pc端好友邀请' when 2 then '微信注册' when 3 then '微信端好友邀请' else '其它' end  as '来源',ISNULL(CommNum,0) as '沟通次数' ");//username as '用户名',mobile as '手机', 
            sql.Append("FROM hx_member_table ");
            sql.Append("Where registerid>0 ");
            #region 查询条件
            if (!string.IsNullOrEmpty(username))
            {
                sql.AppendFormat(" AND username like '%{0}%' ", username);
            }
            if (!string.IsNullOrEmpty(realname))
            {
                sql.AppendFormat(" AND realname like '%{0}%' ", realname);
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                sql.AppendFormat(" AND mobile like '%{0}%' ", mobile);
            }
            if (useridentity >= 0)
            {
                sql.AppendFormat(" AND useridentity ='{0}' ", useridentity);
            }
            if (Channelsource >= 0)
            {
                sql.AppendFormat(" AND Channelsource ='{0}' ", Channelsource);
            }
            if (usertypes >= 0)
            {
                sql.AppendFormat(" AND usertypes ='{0}' ", usertypes);
            }
            if (!string.IsNullOrEmpty(time1))
            {
                DateTime df = DateTime.Parse(time1);

                sql.AppendFormat(" AND convert(varchar(10),registration_time,120)>='{0}' ", df.ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(time2))
            {
                DateTime df = DateTime.Parse(time2);
                sql.AppendFormat(" AND convert(varchar(10),registration_time,120)<='{0}' ", df.ToString("yyyy-MM-dd"));
            }
            #endregion
            sql.Append(" order by  registerid desc; ");

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql.ToString());

            var path = Extensions.ExportExcel(dt);

            return Content(path);
        }

        #region 用户详情
        /// <summary>
        /// 用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 120, Location = OutputCacheLocation.ServerAndClient)]
        [AdminVaildate(false, true)]
        public ActionResult Detail(int id, string startdatetime, string enddatetime, int? pageIndex = 1, int? acttype = 1, int? capitalindex = 1, int? Cashpageindex = 1, int? Citypageindex = 1, int? regpageindex = 1
           , int? llregpageindex = 1, int? cashpageindex = 1, int? llcashpageindex = 1, int? huipageindex = 1, int? invpageindex = 1, int page = 1, int pgaesize = 5)
        {

            UsrAllInfoMode model = new UsrAllInfoMode();

            #region  资金明细统计
            var listcount = 0;
            var CapticalDetailList = ef.hx_Capital_account_water.Where(c => c.membertable_registerid == id).GroupBy(c => c.membertable_registerid)
                .Select(c => new
                {
                    Owner = c.Key,
                    Counts = c.Count()
                });
            foreach (var itc in CapticalDetailList)
            {
                if (itc.Counts > 0)
                {
                    int.TryParse(itc.Counts.ToString(), out listcount);
                }
            }
            ViewBag.CapticalCount = listcount;
            #endregion
            #region 统计充值数据

            int succCount = 0;
            decimal succTotal = 0.00M;
            var ListByOwner = ef.hx_Recharge_history.Where(l => l.membertable_registerid == id && l.recharge_condition == 1).GroupBy(l => l.membertable_registerid)
                .Select(lg => new
                {
                    Owner = lg.Key,
                    Counts = lg.Count(),
                    lostCount = lg.Where(lo => lo.recharge_condition == 0).Count(),
                    succCount = lg.Where(su => su.recharge_condition == 1).Count(),
                    Totals = lg.Sum(w => w.recharge_amount),
                    lostTotal = lg.Where(w => w.recharge_condition == 0).Sum(w => w.recharge_amount),
                    succTotal = lg.Where(w => w.recharge_condition == 1).Sum(w => w.recharge_amount),
                });
            foreach (var itc in ListByOwner)
            {
                if (itc.Counts > 0)
                {
                    int.TryParse(itc.succCount.ToString(), out succCount);
                    decimal.TryParse(itc.succTotal.ToString(), out succTotal);
                }
            }
            ViewBag.succCount = succCount;
            ViewBag.succTotal = succTotal;
            #endregion
            #region 连连充值统计数据

            int llsuccCount = 0;
            decimal llsuccTotal = 0.00M;
            var llListByOwer = ef.V_LLPay_Re.Where(a => a.registerid == id).GroupBy(a => a.registerid)
                .Select(b => new
                {
                    Owner = b.Key,
                    Counts = b.Count(),
                    lostCount = b.Where(c => c.ReState != 1).Count(),
                    succCount = b.Where(c => c.ReState == 1).Count(),
                    Totals = b.Sum(c => c.money_order),
                    lostTotal = b.Where(c => c.ReState != 1).Sum(c => c.money_order),
                    succTotal = b.Where(c => c.ReState == 1).Sum(c => c.money_order)
                });
            foreach (var itc in llListByOwer)
            {
                if (itc.Counts > 0)
                {
                    int.TryParse(itc.succCount.ToString(), out llsuccCount);
                    decimal.TryParse(itc.succTotal.ToString(), out llsuccTotal);
                }
            }
            ViewBag.llsuccCount = llsuccCount;
            ViewBag.llsuccTotal = llsuccTotal;

            #endregion
            #region 统计取现数据
            int succCountCash = 0;
            decimal succTotalcash = 0.00M;
            ListByOwner = ef.hx_td_UserCash.Where(l => l.registerid == id).GroupBy(l => l.registerid)
           .Select(lg => new
           {
               Owner = lg.Key,
               Counts = lg.Count(),
               lostCount = lg.Where(lo => lo.OrdIdState == 0).Count(),
               succCount = lg.Where(su => su.OrdIdState == 3).Count(),
               Totals = lg.Where(w => w.OrdIdState == 3).Sum(w => w.TransAmt),
               lostTotal = lg.Where(w => w.OrdIdState == 0).Sum(w => w.TransAmt),
               succTotal = lg.Where(w => w.OrdIdState == 3).Sum(w => w.TransAmt),
           });

            foreach (var itc in ListByOwner)
            {
                if (itc.Counts > 0)
                {
                    int.TryParse(itc.succCount.ToString(), out succCountCash);
                    decimal.TryParse(itc.succTotal.ToString(), out succTotalcash);
                }
            }
            ViewBag.succCountCash = succCountCash;
            ViewBag.succTotalcash = succTotalcash;
            #endregion
            #region 连连取现数据
            int succCountLLCash = 0;
            decimal succTotalLLcash = 0.00M;
            var LLCashListByOwner = ef.hx_td_LL_cash.Where(c => c.Usrid == id).GroupBy(c => c.Usrid)
                .Select(a => new
                {
                    Owner = a.Key,
                    Counts = a.Count(),
                    lostCount = a.Where(c => c.paystate != 1).Count(),
                    succCount = a.Where(c => c.paystate == 1).Count(),
                    Totals = a.Where(c => c.paystate == 1).Sum(c => c.money_order),
                    lostTotal = a.Where(c => c.paystate != 1).Sum(c => c.money_order),
                    succTotal = a.Where(c => c.paystate == 1).Sum(c => c.money_order)
                });

            foreach (var itc in LLCashListByOwner)
            {
                if (itc.Counts > 0)
                {
                    int.TryParse(itc.succCount.ToString(), out succCountLLCash);
                    decimal.TryParse(itc.succTotal.ToString(), out succTotalLLcash);
                }
            }
            ViewBag.succCountLLCash = succCountLLCash;
            ViewBag.succTotalLLCash = succTotalLLcash;
            #endregion
            #region 投资信息统计
            var intListByOwner = ef.hx_Bid_records.Where(c => c.investor_registerid == id).GroupBy(c => c.investor_registerid)
               .Select(lg => new
               {
                   Owner = lg.Key,
                   Counts = lg.Count(),
                   succCount = lg.Where(su => su.invest_state == 1).Count(),
                   Totals = lg.Sum(w => w.investment_amount),
                   succTotal = lg.Where(w => w.invest_state == 1).Sum(w => w.investment_amount)
               });

            foreach (var itc in intListByOwner)
            {
                if (itc.Counts > 0)
                {
                    int.TryParse(itc.succCount.ToString(), out succCountCash);
                    decimal.TryParse(itc.succTotal.ToString(), out succTotalcash);
                }
            }

            ViewBag.invcount = succCountCash;
            ViewBag.invtotal = succTotalcash;

            #endregion



            List<V_UsrBindCardBank> Ucard = new List<V_UsrBindCardBank>();
            Ucard = ef.V_UsrBindCardBank.Where(p => p.registerid == id).OrderByDescending(p => p.defCard).ToList();

            model.UserMode = (from a in ef.hx_member_table where a.registerid == id select a).SingleOrDefault();

            UserInfoData pauser = new UserInfoData();
            model.Capitcal_water = pauser.Capital_water(id, startdatetime, enddatetime, capitalindex, page, pgaesize);
            model.Recharge = pauser.Recharge(id, startdatetime, enddatetime, regpageindex, page, pgaesize);
            model.LLRecharge = pauser.LLRecharge(id, startdatetime, enddatetime, llregpageindex, page, pgaesize);
            model.UserCash = pauser.UserCash(id, startdatetime, enddatetime, cashpageindex, page, pgaesize);
            model.UserLLCash = pauser.UserLLCash(id, startdatetime, enddatetime, llcashpageindex, page, pgaesize);
            model.Bid_Records = pauser.Bid_RecordsList(id, startdatetime, enddatetime, invpageindex, page, 3);
            model.Bid_Records_income = pauser.Bid_Records_income(id, startdatetime, enddatetime, huipageindex, page, pgaesize);
            model.cash = pauser.Actcash(id, startdatetime, enddatetime, acttype, Cashpageindex, page, pgaesize);
            model.yaoqin = pauser.yaoqin(id, startdatetime, enddatetime, acttype, Cashpageindex, page, pgaesize);
            model.usrlogin = pauser.usrlogin(id, startdatetime, enddatetime, acttype, Citypageindex, page, pgaesize);


            if (Request.IsAjaxRequest())
            {
                var target = Request.QueryString["target"];
                if (target == "capitallist")//资金明细列表
                {
                    return PartialView("_CapitcalWater", model.Capitcal_water);
                }
                else if (target == "rechargelist")  //充值列表
                {
                    return PartialView("_rechargelist", model.Recharge);
                }
                else if (target == "llrechargelist")//连连充值列表
                {
                    return PartialView("_LLrechargelist", model.LLRecharge);
                }
                else if (target == "cashlistd")  //取现列表
                {
                    return PartialView("_cashlist", model.UserCash);
                }
                else if (target == "llcashlistd")//连连取现列表
                {
                    return PartialView("_LLcashlist", model.UserLLCash);
                }
                else if (target == "invsList")  //投资列表
                {
                    return PartialView("_invsList", model.Bid_Records);
                }
                else if (target == "invhuilist")  //回款列表
                {
                    return PartialView("_invhuilist", model.Bid_Records_income);
                }
                else if (target == "jiangli")  //奖励
                {
                    if (acttype == 4)
                    {

                        return PartialView("_yaoqinlist", model.yaoqin);
                    }
                    else
                    {
                        return PartialView("_jianglilist", model.cash);
                    }


                }
                else if (target == "cityinfo")  //用户登录来源
                {

                    return PartialView("_cityinfrolist", model.usrlogin);
                }
            }


            ViewBag.acttype = acttype.ToString();
            ViewBag.Ucard = Ucard;

            return View(model);
        }

        #endregion

        public ActionResult CashDetail(int id, string startdatetime, string enddatetime, int? pageIndex = 1, int pgaesize = 5)
        {
            UsrAllInfoMode model = new UsrAllInfoMode();
            UserInfoData pauser = new UserInfoData();
            model.UserCash = pauser.UserCash(id, startdatetime, enddatetime, pageIndex, 0, pgaesize);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_cashlist", model.UserCash);

            }


            return View(model);
        }



        /// <summary>
        /// 用户沟通记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult ContactRecords(int id, string problemType1 = "", string gtType1 = "", int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            hx_member_table member = (from a in ef.hx_member_table where a.registerid == id select a).SingleOrDefault();

            Expression<Func<V_Phone_records, bool>> where = PredicateExtensionses.True<V_Phone_records>();
            where = where.And(p => p.registerid == id);
            // 查询条件
            if (!string.IsNullOrEmpty(problemType1))
            {
                where = where.And(p => p.problemType.Contains(problemType1));
            }
            if (!string.IsNullOrEmpty(gtType1))
            {
                where = where.And(p => p.gtType.Contains(gtType1));
            }
            IPagedList<V_Phone_records> list = ef.V_Phone_records.Where(where).OrderByDescending(p => p.recordsid).ToPagedList(pageNumber, pageSize);

            ViewBag.member = member;
            ViewBag.TotalNum = list == null ? 0 : list.PageCount;
            ViewBag.id = id;
            ViewBag.problemType = problemType1;
            ViewBag.gtType = gtType1;

            return View(list);
        }

        /// <summary>
        /// 保存沟通记录
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns> [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate(false, true)]
        public ActionResult PostUserRecords(hx_td_Phone_records records)
        {
            records = (hx_td_Phone_records)Utils.ValidateModelClass(records);
            records.adminid = Utils.GetAdmUserID();
            records.recordtime = DateTime.Now;
            ef.hx_td_Phone_records.Add(records);
            int id = ef.SaveChanges();
            if (id > 0)
            {
                ef.hx_member_table.Where(c => c.registerid == records.registerid).Update(c => new hx_member_table { CommNum = c.CommNum + 1, calltime = DateTime.Now });


                return Content(StringAlert.Alert("操作成功", "/admin/Users/ContactRecords?id=" + records.registerid), "text/html");
            }
            else
            {
                return Content(StringAlert.Alert("操作失败"), "text/html");
            }
        }

        /// <summary>
        /// 更新用户手机号
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult UpdateUserAddress(int id, string address)
        {
            address = Utils.CheckSQLHtml(address);
            string json = "{\"ret\":0,\"msg\":\"操作失败!\"}";
            var result = ef.hx_member_table.Where(a => a.registerid == id).Update(p => new hx_member_table { contactaddress = address });
            if (result > 0)
            {
                json = "{\"ret\":1,\"msg\":\"操作成功！\"}";
            }
            return Content(json, "text/json");
        }


        #endregion

        #region 提现

        /// <summary>
        /// 提现列表
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult UserCash(string realname = "", string mobile = "", string orderNO = "", string time1 = "", string time2 = "", int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<V_UserCash_Bank, bool>> where = PredicateExtensionses.True<V_UserCash_Bank>();
            where = where.And(p => p.UserCashId > 0);
            #region 查询条件
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(p => p.realname.Contains(realname));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                where = where.And(p => p.mobile.Contains(mobile));
            }
            if (!string.IsNullOrEmpty(orderNO))
            {
                where = where.And(p => p.OrdId.Contains(orderNO));
            }
            if (!string.IsNullOrEmpty(time1))
            {
                DateTime dt1 = Convert.ToDateTime(time1);
                where = where.And(a => ((DateTime)a.OrdIdTime).CompareTo(dt1) >= 0);
            }
            if (!string.IsNullOrEmpty(time2))
            {
                DateTime dt2 = Convert.ToDateTime(time2);
                dt2 = Convert.ToDateTime(dt2.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(a => ((DateTime)a.OrdIdTime).CompareTo(dt2) <= 0);
            }
            #endregion

            IPagedList<V_UserCash_Bank> list = ef.V_UserCash_Bank.Where(where).OrderByDescending(p => p.UserCashId).ToPagedList(pageNumber, pageSize);

            ViewBag.realname = realname;
            ViewBag.mobile = mobile;
            ViewBag.orderNO = orderNO;
            ViewBag.time1 = time1;
            ViewBag.time2 = time2;

            return View(list);
        }


        public ActionResult UserCashToExcel(string realname = "", string mobile = "", string orderNO = "", string time1 = "", string time2 = "")
        {
            var userid = Utils.GetAdmUserID();
            var controllerName = this.RouteData.Values["controller"].ToString();
            var actionName = this.RouteData.Values["action"].ToString();
            if (!new UserLimitByEF().CheckAdminLimit(userid, controllerName, actionName))
            {   //无权限
                return Content("您没有操作权限");
            }

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT UserCashId as ID,realname as '姓名',membertable_registerid as '用户id',TransAmt as '提现金额'");
            sql.Append(",OrdId as '订单号',FeeAmt as '手续',OrdIdTime as '申请时间',OperTime as '审核时间'");
            sql.Append(",case ISNULL(OrdIdState,0) when 1 then '待付款' when 3 then '已付款' when 4 then '未通过' else '未审核'end as '状态' ");//membertable_registerid

            sql.Append("FROM V_UserCash_Bank ");
            sql.Append("Where UserCashId>0 ");

            #region 查询条件
            if (!string.IsNullOrEmpty(realname))
            {
                sql.AppendFormat("AND realname like '%{0}%' ", realname);
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                sql.AppendFormat("AND mobile like '%{0}%' ", mobile);
            }
            if (!string.IsNullOrEmpty(orderNO))
            {
                sql.AppendFormat("AND OrdId like '%{0}%' ", orderNO);
            }
            if (!string.IsNullOrEmpty(time1))
            {
                DateTime df = DateTime.Parse(time1);
                sql.AppendFormat(" AND convert(varchar(10),OrdIdTime,120)>='{0}' ", df.ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(time2))
            {
                DateTime df = DateTime.Parse(time2);
                sql.AppendFormat(" AND convert(varchar(10),OrdIdTime,120)<='{0}' ", df.ToString("yyyy-MM-dd"));
            }
            #endregion
            sql.Append(" Order By UserCashId DESC;");

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql.ToString());

            var path = Extensions.ExportExcel(dt);

            return Content(path);
        }

        #endregion

        #region 充值

        /// <summary>
        /// 充值列表
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Recharge(string realname = "", string mobile = "", string orderNO = "", string bankCode = "", string time1 = "", string time2 = "", int Page = 1, int pageSize = 10)
        {
            ///V_Recharge_user_bank
            int pageNumber = Page / 1;
            Expression<Func<V_Recharge_user_bank, bool>> where = PredicateExtensionses.True<V_Recharge_user_bank>();
            where = where.And(p => p.recharge_history_id > 0);
            #region 查询条件
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(p => p.realname.Contains(realname));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                where = where.And(p => p.mobile.Contains(mobile));
            }
            if (!string.IsNullOrEmpty(orderNO))
            {
                where = where.And(p => p.order_No.Contains(orderNO));
            }
            if (!string.IsNullOrEmpty(bankCode))
            {
                // where = where.And(p => p.recharge_bank.Contains(bankCode));

                where = where.And(p => p.recharge_bank == bankCode);
            }
            if (!string.IsNullOrEmpty(time1))
            {
                DateTime dt1 = Convert.ToDateTime(time1);
                where = where.And(a => ((DateTime)a.recharge_time).CompareTo(dt1) >= 0);
            }
            if (!string.IsNullOrEmpty(time2))
            {
                DateTime dt2 = Convert.ToDateTime(time2);
                dt2 = Convert.ToDateTime(dt2.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(a => ((DateTime)a.recharge_time).CompareTo(dt2) <= 0);
            }
            #endregion

            IPagedList<V_Recharge_user_bank> list = ef.V_Recharge_user_bank.Where(where).OrderByDescending(p => p.recharge_history_id).ToPagedList(pageNumber, pageSize);

            ViewBag.realname = realname;
            ViewBag.mobile = mobile;
            ViewBag.orderNO = orderNO;
            ViewBag.bankCode = bankCode;
            ViewBag.time1 = time1;
            ViewBag.time2 = time2;
            ViewBag.dropdownBank = new SelectListByEF().GetBandDropDownList("", "请选择");

            return View(list);
        }


        public ActionResult RechargeToExcel(string realname = "", string mobile = "", string orderNO = "", string bankCode = "", string time1 = "", string time2 = "")
        {
            var userid = Utils.GetAdmUserID();
            var controllerName = this.RouteData.Values["controller"].ToString();
            var actionName = this.RouteData.Values["action"].ToString();
            if (!new UserLimitByEF().CheckAdminLimit(userid, controllerName, actionName))
            {   //无权限
                return Content("您没有操作权限");
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT recharge_history_id as ID,realname as '姓名',membertable_registerid as '用户id'");
            sql.Append(",recharge_amount as '充值金额',recharge_time as '充值时间',BankName as '充值银行'");
            sql.Append(",order_No as '订单号',case when ISNULL(recharge_condition,0)=0 then '失败' else '成功' end as '状态' ");//username as '用户名',mobile as '手机'
            sql.Append(",'' as '用于投资' ");
            sql.Append("FROM V_Recharge_user_bank ");
            sql.Append("WHERE recharge_history_id>0 ");

            #region 查询条件
            if (!string.IsNullOrEmpty(realname))
            {
                sql.AppendFormat("AND realname like '%{0}%'", realname);
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                sql.AppendFormat("AND mobile like '%{0}%'", mobile);
            }
            if (!string.IsNullOrEmpty(orderNO))
            {
                sql.AppendFormat("AND order_No like '%{0}%'", orderNO);
            }
            if (!string.IsNullOrEmpty(bankCode))
            {
                sql.AppendFormat("AND recharge_bank like '%{0}%'", bankCode);
            }
            if (!string.IsNullOrEmpty(time1))
            {
                DateTime df = DateTime.Parse(time1);

                sql.AppendFormat(" AND convert(varchar(10),recharge_time,120)>='{0}' ", df.ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(time2))
            {
                DateTime df = DateTime.Parse(time2);
                sql.AppendFormat(" AND convert(varchar(10),recharge_time,120)<='{0}' ", df.ToString("yyyy-MM-dd"));
            }
            #endregion

            sql.Append(" order by recharge_history_id desc;");

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql.ToString());

            var path = Extensions.ExportExcel(dt);

            return Content(path);
        }

        #endregion

        #region 投资

        /// <summary>
        /// 投资列表
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Investment(string realname = "", string mobile = "", string orderid = "", string minMoney = "", string maxMoney = "", int rewardType = -1, int timeType = 0, string time1 = "", string time2 = "", int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            IOrderedQueryable<V_hx_Bid_records_borrowing_target> finalData = GetBaseData(realname, mobile, orderid, minMoney, maxMoney, rewardType, timeType, time1, time2);
            IPagedList<V_hx_Bid_records_borrowing_target> list = finalData.ToPagedList(pageNumber, pageSize);


            ViewBag.realname = realname;
            ViewBag.mobile = mobile;
            ViewBag.minMoney = minMoney;
            ViewBag.maxMoney = maxMoney;
            ViewBag.rewardType = rewardType;
            ViewBag.time1 = time1;
            ViewBag.time2 = time2;
            ViewBag.timeType = timeType;

            ViewBag.orderid = orderid;
            return View(list);
        }

        private IOrderedQueryable<V_hx_Bid_records_borrowing_target> GetBaseData(string realname, string mobile, string orderid, string minMoney, string maxMoney, int rewardType, int timeType, string time1, string time2)
        {
            Expression<Func<V_hx_Bid_records_borrowing_target, bool>> where = PredicateExtensionses.True<V_hx_Bid_records_borrowing_target>();
            where = where.And(p => p.bid_records_id > 0);
            #region 查询条件
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(p => p.realname.Contains(realname));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                where = where.And(p => p.mobile.Contains(mobile));
            }
            if (!string.IsNullOrEmpty(orderid))
            {
                where = where.And(p => p.OrdId.ToString().Contains(orderid));
            }
            if (!string.IsNullOrEmpty(minMoney))
            {
                decimal min = Convert.ToDecimal(minMoney);
                where = where.And(p => p.investment_amount >= min);
            }
            if (!string.IsNullOrEmpty(maxMoney))
            {
                decimal max = Convert.ToDecimal(maxMoney);
                where = where.And(p => p.investment_amount <= max);
            }
            //奖励类型
            if (rewardType > -1)
            {

                if (rewardType == 2)
                {
                    where = where.And(p => p.BonusAmt > 0);
                }
                else if (rewardType == 3)
                {
                    where = where.And(p => p.JiaxiNum > 0);

                }

                ///TODO 
            }
            if (!string.IsNullOrEmpty(time1))
            {
                DateTime dt1 = Convert.ToDateTime(time1);
                if (timeType == 0)
                {   //投资时间
                    where = where.And(a => ((DateTime)a.invest_time).CompareTo(dt1) >= 0);
                }
                else
                {   //结束时间
                    where = where.And(a => ((DateTime)a.investment_maturity).CompareTo(dt1) >= 0);
                }
            }

            if (!string.IsNullOrEmpty(time2))
            {
                DateTime dt2 = Convert.ToDateTime(time2);
                dt2 = Convert.ToDateTime(dt2.ToString("yyyy-MM-dd") + " 23:59:59");
                if (timeType == 0)
                {   //投资时间
                    where = where.And(a => ((DateTime)a.invest_time).CompareTo(dt2) <= 0);
                }
                else
                {   //结束时间
                    where = where.And(a => ((DateTime)a.investment_maturity).CompareTo(dt2) <= 0);
                }
            }
            #endregion
            var finalData = ef.V_hx_Bid_records_borrowing_target.Where(where).OrderByDescending(p => p.bid_records_id);
            return finalData;
        }

        /// <summary>
        /// 投资列表导出
        /// </summary>
        /// <returns></returns>
        public ActionResult InvestmentToExcel(string realname = "", string mobile = "", string orderid = "", string minMoney = "", string maxMoney = "", int rewardType = -1, int timeType = 0, string time1 = "", string time2 = "")
        {
            var userid = Utils.GetAdmUserID();
            var controllerName = this.RouteData.Values["controller"].ToString();
            var actionName = this.RouteData.Values["action"].ToString();
            if (!new UserLimitByEF().CheckAdminLimit(userid, controllerName, actionName))
            {   //无权限
                return Content("您没有操作权限");
            }
            List<dynamic> baseData = (from item in GetBaseData(realname, mobile, orderid, minMoney, maxMoney, rewardType, timeType, time1, time2).ToList()
                                      select new
                                      {
                                          ID = item.bid_records_id,
                                          订单号 = item.OrdId != null ? item.OrdId.ToString() : "",
                                          姓名 = item.realname,
                                          性别 = Convert.ToInt32((item.iD_number.Length == 18 ? item.iD_number.Substring(16, 1) : item.iD_number.Substring(14, 1))) % 2 == 1 ? "男" : "女",
                                          年龄 = DateTime.Now.Year - Convert.ToInt32(item.iD_number.Length == 18 ? (item.iD_number.Substring(6, 4)) : ("19" + item.iD_number.Substring(6, 2))),
                                          出生日期 = item.iD_number.Length == 18 ? (item.iD_number.Substring(6, 4) + "-" + item.iD_number.Substring(10, 2) + "-" + item.iD_number.Substring(12, 2)) : ("19" + item.iD_number.Substring(6, 2) + "-" + item.iD_number.Substring(8, 2) + "-" + item.iD_number.Substring(10, 2)),
                                          用户id = item.registerid,
                                          投资金额 = item.investment_amount ?? 0,
                                          加息券 = item.JiaxiNum.ToString("0.00") + "%",
                                          抵扣券 = item.BonusAmt.ToString("0.00"),
                                          投资项目 = item.borrowing_title,
                                          年化收益 = (item.annual_interest_rate != null ? ((decimal)item.annual_interest_rate).ToString("0.00") : "") + "%",
                                          投资时间段 = (item.value_date != null ? ((DateTime)item.value_date).ToString("yyyy-MM-dd") : "") + "--" + (item.investment_maturity != null ? ((DateTime)item.investment_maturity).ToString("yyyy-MM-dd") : ""),
                                          投资期限 = Utils.GetLife_of_loan(item.life_of_loan.ToString(), item.unit_day.ToString()),
                                          投资时间 = item.invest_time != null ? ((DateTime)item.invest_time).ToString("yyyy-MM-dd hh:mm:ss") : "",
                                          状态 = Utils.GetPayment_Status(item.payment_status.ToString()),
                                          总收益 = (item.investment_amount + item.withoutinterest + item.haveinterest) ?? 0
                                      }).ToList<dynamic>();
            DataTable dt = ConvertDataTable<dynamic>.ListToDataTable(baseData);

            var path = Extensions.ExportExcel(dt);
            return Content(path);
        }
        #endregion

        /// <summary>
        /// 选择用户
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectUser(string username = "", string realname = "", int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<hx_member_table, bool>> where = PredicateExtensionses.True<hx_member_table>();
            where = where.And(p => p.registerid > 0 && (p.usertypes == 1 || p.usertypes == 2));

            if (!string.IsNullOrEmpty(username))
            {
                where = where.And(p => p.username.Contains(username));
            }
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(p => p.realname.Contains(realname));
            }

            IPagedList<hx_member_table> list = ef.hx_member_table.Where(where).OrderByDescending(p => p.registerid).ToPagedList(pageNumber, pageSize);

            ViewBag.username = username;
            ViewBag.realname = realname;

            return View(list);


        }

        #region 发送短信

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="uids"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult SMSItem(string uids = "")
        {
            ViewBag.uids = uids;

            return View();
        }

        /// <summary>
        /// 手机号发短信
        /// </summary>
        /// <param name="uids"></param>
        /// <param name="txt"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult SendSMSTest(string uids = "", string txt = "")
        {
            string json = "{\"ret\":-1,\"msg\":\"参数错误\"}";
            if (!string.IsNullOrEmpty(uids) && !string.IsNullOrEmpty(txt))
            {
                //参数OK
                int fhState = SendSMS(uids, txt);
                if (fhState == 0)
                {
                    json = "{\"ret\":1,\"msg\":\"发送成功\"}";
                }
                else
                {
                    json = "{\"ret\":0,\"msg\":\"发送失败\"}";
                }
            }
            return Content(json, "text/json");
        }

        /// <summary>
        /// 手机号发短信
        /// </summary>
        /// <param name="uids"></param>
        /// <param name="txt"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult SendSMSByCon(string condition = "", string txt = "")
        {
            string json = "{\"ret\":-1,\"msg\":\"参数错误\"}";
            if (!string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(txt))
            {
                int sendCount = 0;
                if (condition == "1000")
                {
                    sendCount = oldUsers(txt);
                }
                else if (condition == "1001")
                {
                    sendCount = SetAllInvUser(txt);
                }
                else if (condition == "1002")
                {
                    sendCount = invone(txt);
                }
                else if (condition == "1003")
                {
                    sendCount = invtwo(txt);
                }

                else if (condition == "1004")
                {
                    sendCount = notinv(txt);
                }

                if (sendCount > 0)
                {
                    json = "{\"ret\":1,\"msg\":\"发送成功" + sendCount + "条\"}";
                }
                else
                {
                    json = "{\"ret\":0,\"msg\":\"发送失败\"}";
                }//string Text =" 发送短信总数为:" + RecordCount.ToString();
            }
            return Content(json, "text/json");
        }

        /// <summary>
        /// 用户id发短信
        /// </summary>
        /// <param name="uids"></param>
        /// <param name="txt"></param>
        /// <returns></returns>
        public ActionResult SendSMSByIds(string uids = "", string txt = "")
        {
            string json = "{\"ret\":-1,\"msg\":\"参数错误\"}";
            if (!string.IsNullOrEmpty(uids) && !string.IsNullOrEmpty(txt))
            {   //参数OK
                List<string> listUid = uids.Split(',').ToList();
                var list = ef.hx_member_table.Where(a => listUid.Contains(a.registerid.ToString())).ToList();
                string strTels = "";
                if (list != null && list.Count > 0)
                {
                    foreach (hx_member_table item in list)
                    {
                        if (!string.IsNullOrEmpty(strTels))
                        {
                            strTels = strTels + ",";
                        }
                        if (!string.IsNullOrEmpty(item.mobile))
                        {
                            strTels = strTels + item.mobile;
                        }
                    }
                    int fhState = SendSMS(strTels, txt);
                    if (fhState == 0)
                    {
                        json = "{\"ret\":1,\"msg\":\"发送成功\"}";
                    }
                    else
                    {
                        json = "{\"ret\":0,\"msg\":\"发送失败\"}";
                    }
                }
                else
                {
                    json = "{\"ret\":-1,\"msg\":\"请选择正确的用户\"}";
                }
            }
            return Content(json, "text/json");
        }

        private int SendSMS(string tels, string txt)
        {
            return YMSendSMS.Send_SMS(tels, txt);
        }
        #endregion


        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="borrowing_title"></param>
        /// <param name="realname"></param>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <param name="Page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [AdminVaildate(false)]
        public ActionResult UserInvestmentlist(string borrowing_title = "", string realname = "", string time1 = "", string time2 = "", int Page = 1, int pageSize = 10)
        {
            ///TODO
            int pageNumber = Page / 1;
            Expression<Func<V_hx_Bid_records_borrowing_target, bool>> where = PredicateExtensionses.True<V_hx_Bid_records_borrowing_target>();
            where = where.And(p => p.bid_records_id > 0);

            if (!string.IsNullOrEmpty(borrowing_title))
            {
                where = where.And(p => p.username.Contains(borrowing_title));
            }
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(p => p.realname.Contains(realname));
            }

            IPagedList<V_hx_Bid_records_borrowing_target> list = ef.V_hx_Bid_records_borrowing_target.Where(where).OrderByDescending(p => p.registerid).ToPagedList(pageNumber, pageSize);

            ViewBag.borrowing_title = borrowing_title;
            ViewBag.realname = realname;

            return View(list);
        }


        #region CRM
        /// <summary>
        /// CRM
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult CRM(string username = "", string realname = "", string mobile = "", int problemType = 0, int num1 = 0, int num2 = 0, int useridentity = 0, int calltime = 0, string time1 = "", string time2 = "", int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<V_Usr_Phone_adminUsr, bool>> where = PredicateExtensionses.True<V_Usr_Phone_adminUsr>();
            where = where.And(p => p.registerid > 0);
            #region 查询条件
            if (!string.IsNullOrEmpty(username))
            {
                where = where.And(p => p.username.Contains(username));
            }
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(p => p.realname.Contains(realname));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                where = where.And(p => p.mobile.Contains(mobile));
            }
            if (useridentity >= 0)
            {
                where = where.And(p => p.useridentity == useridentity);
            }

            #region 问题类型
            if (problemType > 0)
            {
                where = where.And(p => p.problemType == problemType.ToString());

            }
            #endregion
            #region 时间条件
            if (calltime > 0)
            {
                if (calltime == 1) //沟通时间
                {
                    if (!string.IsNullOrEmpty(time1))
                    {
                        DateTime dt1 = Convert.ToDateTime(time1);
                        where = where.And(a => ((DateTime)a.calltime).CompareTo(dt1) >= 0);
                    }
                    if (!string.IsNullOrEmpty(time2))
                    {
                        DateTime dt2 = Convert.ToDateTime(time2);
                        dt2 = Convert.ToDateTime(dt2.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(a => ((DateTime)a.calltime).CompareTo(dt2) <= 0);
                    }
                }
                else if (calltime == 2)//登录时间
                {
                    if (!string.IsNullOrEmpty(time1))
                    {
                        DateTime dt1 = Convert.ToDateTime(time1);
                        where = where.And(a => ((DateTime)a.lastlogintime).CompareTo(dt1) >= 0);
                    }
                    if (!string.IsNullOrEmpty(time2))
                    {
                        DateTime dt2 = Convert.ToDateTime(time2);
                        dt2 = Convert.ToDateTime(dt2.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(a => ((DateTime)a.lastlogintime).CompareTo(dt2) <= 0);
                    }
                }
                else if (calltime == 3)//注册时间
                {
                    if (!string.IsNullOrEmpty(time1))
                    {
                        DateTime dt1 = Convert.ToDateTime(time1);
                        where = where.And(a => ((DateTime)a.registration_time).CompareTo(dt1) >= 0);
                    }
                    if (!string.IsNullOrEmpty(time2))
                    {
                        DateTime dt2 = Convert.ToDateTime(time2);
                        dt2 = Convert.ToDateTime(dt2.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(a => ((DateTime)a.registration_time).CompareTo(dt2) <= 0);
                    }

                }

            }

            #endregion

            #endregion

            IPagedList<V_Usr_Phone_adminUsr> list = ef.V_Usr_Phone_adminUsr.Where(where).OrderByDescending(p => p.registerid).ToPagedList(pageNumber, pageSize);

            ViewBag.username = username;
            ViewBag.realname = realname;
            ViewBag.mobile = mobile;
            ViewBag.useridentity = useridentity;
            ViewBag.time1 = time1;
            ViewBag.time2 = time2;
            ViewBag.calltime = calltime;
            ViewBag.problemType = problemType;


            return View(list);
        }
        #endregion


        #region 忠诚度
        /// <summary>
        /// 忠诚度
        /// </summary>
        /// <returns></returns>

        [AdminVaildate()]
        public ActionResult loyalty(string username = "", string realname = "", string mobile = "", int datawhere = -1, int num1 = 0, int num2 = 0, int useridentity = -1, int timewhere = -1, string time1 = "", string time2 = "", int Page = 1, int pageSize = 10)
        {

            int pageNumber = Page / 1;
            Expression<Func<hx_member_table, bool>> where = PredicateExtensionses.True<hx_member_table>();
            where = where.And(p => p.registerid > 0);
            #region 查询条件
            if (!string.IsNullOrEmpty(username))
            {
                where = where.And(p => p.username.Contains(username));
            }
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(p => p.realname.Contains(realname));
            }
            if (!string.IsNullOrEmpty(mobile))
            {
                where = where.And(p => p.mobile.Contains(mobile));
            }
            if (useridentity >= 0)
            {
                where = where.And(p => p.useridentity == useridentity);
            }

            #region 充值 登录  ...条件
            if (datawhere > 0)
            {

                if (datawhere == 1) //充值次数
                {

                    if (num1 > 0)
                    {
                        where = where.And(p => p.RechargeNum >= num1);

                    }
                    if (num2 > 0)
                    {
                        where = where.And(p => p.RechargeNum <= num2);
                    }

                }
                else if (datawhere == 2)//提现次数
                {
                    if (num1 > 0)
                    {
                        where = where.And(p => p.CashNum >= num1);

                    }
                    if (num2 > 0)
                    {
                        where = where.And(p => p.CashNum <= num2);
                    }

                }
                else if (datawhere == 3)//登录次数
                {
                    if (num1 > 0)
                    {
                        where = where.And(p => p.LoginNum >= num1);

                    }
                    if (num2 > 0)
                    {
                        where = where.And(p => p.LoginNum <= num2);
                    }
                }
                else if (datawhere == 4)//投资次数
                {
                    if (num1 > 0)
                    {
                        where = where.And(p => p.InvestNum >= num1);

                    }
                    if (num2 > 0)
                    {
                        where = where.And(p => p.InvestNum <= num2);
                    }
                }

            }
            #endregion




            #region 时间条件
            if (timewhere > 0)
            {
                if (timewhere == 1) //沟通时间
                {
                    if (!string.IsNullOrEmpty(time1))
                    {
                        DateTime dt1 = Convert.ToDateTime(time1);
                        where = where.And(a => ((DateTime)a.calltime).CompareTo(dt1) >= 0);
                    }
                    if (!string.IsNullOrEmpty(time2))
                    {
                        DateTime dt2 = Convert.ToDateTime(time2);
                        dt2 = Convert.ToDateTime(dt2.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(a => ((DateTime)a.calltime).CompareTo(dt2) <= 0);
                    }
                }
                else if (timewhere == 2)//登录时间
                {
                    if (!string.IsNullOrEmpty(time1))
                    {
                        DateTime dt1 = Convert.ToDateTime(time1);
                        where = where.And(a => ((DateTime)a.lastlogintime).CompareTo(dt1) >= 0);
                    }
                    if (!string.IsNullOrEmpty(time2))
                    {
                        DateTime dt2 = Convert.ToDateTime(time2);
                        dt2 = Convert.ToDateTime(dt2.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(a => ((DateTime)a.lastlogintime).CompareTo(dt2) <= 0);
                    }
                }
                else if (timewhere == 3)//注册时间
                {
                    if (!string.IsNullOrEmpty(time1))
                    {
                        DateTime dt1 = Convert.ToDateTime(time1);
                        where = where.And(a => ((DateTime)a.registration_time).CompareTo(dt1) >= 0);
                    }
                    if (!string.IsNullOrEmpty(time2))
                    {
                        DateTime dt2 = Convert.ToDateTime(time2);
                        dt2 = Convert.ToDateTime(dt2.ToString("yyyy-MM-dd") + " 23:59:59");
                        where = where.And(a => ((DateTime)a.registration_time).CompareTo(dt2) <= 0);
                    }

                }

            }

            #endregion


            #endregion

            // var list = ef.hx_member_table.Where(where).OrderByDescending(p => p.registerid).ToPagedList(pageIndex ?? 1, pgaesize);

            IPagedList<hx_member_table> list = ef.hx_member_table.Where(where).OrderByDescending(p => p.registerid).ToPagedList(pageNumber, pageSize);
            //   Webdiyer.WebControls.Mvc.IPagedList<hx_member_table> list  =(Webdiyer.WebControls.Mvc.IPagedList<hx_member_table>) ef.hx_member_table.Where(where).OrderByDescending(p => p.registerid).ToPagedList(pageIndex ?? 1, pgaesize);
            ViewBag.username = username;
            ViewBag.realname = realname;
            ViewBag.mobile = mobile;
            ViewBag.useridentity = useridentity;
            ViewBag.time1 = time1;
            ViewBag.time2 = time2;
            ViewBag.datawhere = datawhere;
            ViewBag.timewhere = timewhere;

            //if (Request.IsAjaxRequest())
            //{

            // return PartialView("_loyaltyList", list);


            //}



            return View(list);




        }
        #endregion


        /// <summary>
        /// 新增用户
        /// </summary>
        /// <returns></returns>
        public ActionResult AddUser()
        {

            return View();
        }


        public ActionResult PostUsers(hx_member_table p)
        {
            p = (hx_member_table)Utils.ValidateModelClass(p);

            p.username = p.mobile;
            p.transactionpassword = "";
            p.istransactionpassword = 0;
            p.ismobile = 0;
            p.isrealname = 0;
            p.isbankcard = 0;
            p.isemail = 0;
            p.userstate = 0;
            p.account_total_assets = 0.00M;
            p.available_balance = 0.00M;
            p.collect_total_amount = 0.00M;
            p.frozen_sum = 0.00M;
            p.open_tonto_account = 0;
            p.tonto_account_user = "";
            p.usertypes = 1;
            p.invitedcode = Calculator.Getinvitedcode();
            p.useridentity = 0;
            p.CommNum = 0;
            p.RechargeNum = 0;
            p.CashNum = 0;
            p.InvestNum = 0;
            p.registration_time = DateTime.Now;

            ef.hx_member_table.Add(p);
            int i = ef.SaveChanges();
            string str = "";
            if (i > 0)
            {
                str = @" {""rs""    : ""y"", ""info""      :  ""注册成功!""}";
            }
            else
            {
                str = @" {""rs""    : ""n"", ""error""      :  ""注册失败!""}";
            }
            return Content(str);
        }



        #region 更新类型

        /// <summary>
        /// 更新类型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult UpdateTypes(int id, int values1, string comname = "", int ty = 0)
        {
            // address = Utils.CheckSQLHtml(address);
            string json = "{\"ret\":0,\"msg\":\"操作失败!\"}";
            int result = 0;
            if (ty == 0)
            {

                result = ef.hx_member_table.Where(a => a.registerid == id).Update(p => new hx_member_table { useridentity = values1 });

            }
            else
            {
                result = ef.hx_member_table.Where(a => a.registerid == id).Update(p => new hx_member_table { usertypes = values1, CopName = comname });
            }



            if (result > 0)
            {
                json = "{\"ret\":1,\"msg\":\"操作成功！\"}";
            }
            return Content(json, "text/json");
        }


        #endregion


        #region 发送活动优惠券

        /// <summary>
        /// 发送活动券
        /// </summary>
        /// <returns></returns>
        public ActionResult SendOut()
        {
            var userid = Utils.GetAdmUserID();
            var controllerName = this.RouteData.Values["controller"].ToString();
            var actionName = this.RouteData.Values["action"].ToString();
            if (!new UserLimitByEF().CheckAdminLimit(userid, controllerName, actionName))
            {   //无权限
                return Content(StringAlert.Alert("您没有操作权限"));
            }
            int typex = DNTRequest.GetInt("typex", 2);

            int ActID = 182;


            decimal amt = 0.00M;



            if (typex == 2)
            {
                amt = DNTRequest.GetDecimal("bonusType", 10M);
                ActID = 182;
            }
            else
            {
                amt = DNTRequest.GetDecimal("ddlRateList", 0.2M);
                ActID = 183;
            }

            string registerid = DNTRequest.GetString("registerid");

            if (registerid == "")
            {
                return Content(StringAlert.Alert("请选择用户"));
            }
            string txtTitle = DNTRequest.GetString("txtTitle");
            string time1 = DNTRequest.GetString("time1");
            string time2 = DNTRequest.GetString("time2");

            DateTime datetime1;

            DateTime datetime2;
            if (Utils.IsDate(time1))
            {
                datetime1 = DateTime.Parse(time1);
            }
            else
            {
                return Content(StringAlert.Alert("开始日期格式不对"));
            }

            if (Utils.IsDate(time2))
            {
                datetime2 = DateTime.Parse(time2);
            }
            else
            {
                return Content(StringAlert.Alert("结束日期格式不对"));
            }

            if (datetime1 > datetime2)
            {
                return Content(StringAlert.Alert("开始时间不可大于结束时间"));
            }


            int bonus_k = DNTRequest.GetInt("bonus_k", 1);


            decimal Uselower = DNTRequest.GetDecimal("Uselower", 100M);

            decimal Usehight = DNTRequest.GetDecimal("Usehight", 200M);
            string UseLifeLoanStart = DNTRequest.GetString("UseLifeLoanStart");
            string UseLifeLoanEnd = DNTRequest.GetString("UseLifeLoanEnd");



            string[] usrid = registerid.Split(',');
            var uids = usrid.Select(d => Convert.ToInt32(d)).ToList();
            List<hx_member_table> users = ef.hx_member_table.Where(c => uids.Contains(c.registerid)).ToList();
            foreach (var item in users)
            {
                sendNum(typex, amt, ActID, txtTitle, item.registerid, datetime1, datetime2, bonus_k, Uselower, Usehight, UseLifeLoanStart, UseLifeLoanEnd);
                if (!string.IsNullOrWhiteSpace(item.mobile))
                {
                    string content = string.Format("恭喜您获得{0}张{1}奖励，如有问题可咨询创利投客服！", bonus_k.ToString(), (typex == 2 ? amt + "元抵扣券" : (typex == 3 ? amt + "%加息券" : "")));
                    //参数OK
                    int fhState = SendSMS(item.mobile, content);
                    LogInfo.WriteLog(string.Format("后台发放奖励短信通知：发放状态：{0}手机号：{1}，内容：{2}", fhState, item.mobile, content));
                }
            }

            return Content(StringAlert.Alert("发放成功!"), "text/html");

            //return View();
        }

        #endregion

        /// <summary>
        /// 发送方法
        /// </summary>
        /// <param name="typex"></param>
        /// <param name="amt"></param>
        /// <param name="txtTitle"></param>
        /// <param name="datetime1"></param>
        /// <param name="datetime2"></param>
        /// <param name="bonus_k"></param>
        public void sendNum(int typex, decimal amt, int ActID, string txtTitle, int registerid, DateTime datetime1, DateTime datetime2, int bonus_k = 1, decimal Uselower = 100M, decimal Usehight = 200M, string useLifeLoanStart = "", string useLifeLoanEnd = "")
        {
            int min = 0, max = 0;
            for (int i = 0; i < bonus_k; i++)
            {
                hx_UserAct ua = new hx_UserAct();
                ua.ActTypeId = 5;
                ua.ActID = ActID;
                ua.Amt = amt;
                ua.Title = txtTitle;
                ua.RewTypeID = typex;
                ua.registerid = registerid;
                ua.Uselower = Uselower;
                ua.Usehight = Usehight;
                ua.AmtEndtime = datetime2;
                ua.Createtime = DateTime.Now;
                ua.isSmsFifteen = 0;
                ua.ISSmsOne = 0;
                ua.IsSmsSeven = 0;
                ua.isSmsSixteen = 0;
                ua.IsSmsThree = 0;
                ua.UseState = 0;
                ua.AmtUses = 1;
                ua.UseLifeLoan = (int.TryParse(useLifeLoanStart, out min) ? min : 0) + "-" + (int.TryParse(useLifeLoanEnd, out max) ? max : 0);
                ef.hx_UserAct.Add(ua);
                ef.SaveChanges();
            }
        }

        #region 用户管理>推荐人列表
        [AdminVaildate(false)]
        public ActionResult RecommendedList(int Page = 1, int pageSize = 10)
        {
            IPagedList<RecommendedListViewModel> list = null;
            var query = (from a in (
                        from a in ef.V_yaoqin_count
                        group a by new { yaouid = a.yaouid, yaousername = a.yaousername } into g
                        select new { g.Key.yaouid, g.Key.yaousername }
                        )
                         orderby a.yaouid descending
                         select new RecommendedListViewModel
                         {
                             Referee = a.yaousername,// = (from a1 in ef.V_yaoqin_count where a1.yaouid == a.yaouid select new { a1.yaousername }).Distinct(),
                             InvestedTimes = (from a1 in ef.V_yaoqin_count where a1.yaouid == a.yaouid select new { a1.yaouid }).Count(),
                             RecommendCount = (from a2 in ef.hx_td_Userinvitation where a2.Invpeopleid == a.yaouid select new { a2.Invpeopleid }).Count(),
                             InvestTotalAmount = (from a3 in ef.V_yaoqin_count where a3.yaouid == a.yaouid select a3.investment_amount).Sum().ToString()
                         }).ToPagedList(Page, pageSize);
            //list = query.ToList().Select(a => new RecommendedListViewModel
            //{
            //    Referee = a.yaousername,
            //    InvestedTimes = a.InvestTimes,
            //    InvestTotalAmount = a.InvestAmount.ToString(),
            //    RecommendCount = a.InvitedCount
            //}).ToPagedList(Page, pageSize);
            return View(query);
        }
        #endregion

        #region 用户管理>邀请记录
        [AdminVaildate(false)]
        public ActionResult InvitedRecord(string userName = "", string realName = "", DateTime? investTimeFrom = null, DateTime? investTimeTo = null, int Page = 1, int pageSize = 10)
        {
            IPagedList<InvitedRecordViewModel> list = null;
            var query = from a in ef.V_yaoqin_count
                            //where a.bid_records_id > 0
                        orderby a.bid_records_id descending
                        select new
                        {
                            a.bid_records_id,
                            a.investment_amount,
                            a.OrdId,
                            a.ordstate,
                            a.invitationcode,
                            a.yaousername,
                            a.invusername,
                            a.realname,
                            a.invest_time,
                            a.yaorealname,
                            a.yaouid,
                            a.invuid
                        };
            if (!string.IsNullOrWhiteSpace(userName))
            {
                query = query.Where(p => p.yaousername.Contains(userName));
            }
            if (!string.IsNullOrWhiteSpace(realName))
            {
                query = query.Where(p => p.yaorealname.Contains(realName));
            }
            if (investTimeFrom != null)
            {


                query = query.Where(p => p.invest_time >= investTimeFrom.Value);
            }
            if (investTimeTo != null)
            {

                var investTimeTo2 = investTimeTo.Value.AddDays(1).AddSeconds(-1); //23:59:59
                query = query.Where(p => p.invest_time <= investTimeTo2);
            }
            query = query.Where(p => p.bid_records_id > 0);
            list = query.ToList().Select(a => new InvitedRecordViewModel
            {
                InvestRecordId = a.bid_records_id.ToString(),
                InviterAccount = a.yaousername,
                InviterRealName = a.yaorealname,
                BeInvitedAccount = a.invusername,
                BeInvitedRealName = a.realname,
                InvestDatetime = a.invest_time == null ? "----" : a.invest_time.Value.ToString("yyyy-MM-dd hh24:mi:ss"),
                InvestAmount = a.investment_amount == null ? "0.00" : a.investment_amount.ToString(),
                InvestOrderNo = a.OrdId == null ? "----" : a.OrdId.Value.ToString(),
                YaoUid = a.yaouid == null ? "" : a.yaouid.ToString(),
                InvUid = Convert.ToString(a.invuid) == null ? "" : a.invuid.ToString()
            }).ToPagedList(Page, pageSize);

            ViewBag.username = userName;
            ViewBag.realname = realName;
            ViewBag.investTimeTo = investTimeTo;
            ViewBag.investTimeFrom = investTimeFrom;

            return View(list);
        }
        #endregion

        #region 用户管理>邀请投资记录
        [AdminVaildate(false)]
        public ActionResult UserInviteInfor(string inviterAccount = "", string invitedAccount = "",
            DateTime? registeTimeFrom = null, DateTime? registeTimeTo = null, DateTime? investTimeFrom = null, DateTime? investTimeTo = null,
            string investTerm = "", string investState = "", string isBindedCard = "", int Page = 1, int pageSize = 10)
        {
            IPagedList<UserInviteInforViewModel> list = null;
            var query = from a in ef.ViewUserInviteInfors
                        orderby a.invitationid descending
                        let totalAmount = (from c in ef.hx_Bid_records
                                           where c.ordstate == 1 && c.investor_registerid == a.InvitedId.Value
                                           select c.investment_amount).Sum()
                        select new
                        {
                            a.invitationid,
                            a.InviteId,
                            a.InviteName,
                            a.InvitedId,
                            a.InvitedName,
                            a.InvitedRealName,
                            a.InviteCode,
                            a.CreatedOn,
                            InvestState = a.InvestedOn == null ? "未投资" : "已投资",
                            a.InvestedOn,
                            a.InvestAmount,
                            InvestTotalAmount = totalAmount,
                            a.OrderId,
                            a.BorrowTitle,
                            a.DeadLine,
                            a.DeadLineNumber,
                            IsBindedCard = a.IsBindedCard == 0 ? "未绑定" : "已绑定"
                        };
            if (!string.IsNullOrWhiteSpace(inviterAccount))
            {
                query = query.Where(p => p.InviteName == inviterAccount);
            }
            if (!string.IsNullOrWhiteSpace(invitedAccount))
            {
                query = query.Where(p => p.InvitedName == invitedAccount);
            }
            if (registeTimeFrom != null)
            {
                query = query.Where(p => p.CreatedOn >= registeTimeFrom);
            }
            if (registeTimeTo != null)
            {
                var registeTimeTo2 = registeTimeTo.Value.AddDays(1).AddSeconds(-1); //23:59:59
                query = query.Where(p => p.CreatedOn <= registeTimeTo2);
            }
            if (investTimeFrom != null)
            {
                query = query.Where(p => p.InvestedOn >= investTimeFrom);
            }
            if (investTimeTo != null)
            {
                var investTimeTo2 = investTimeTo.Value.AddDays(1).AddSeconds(-1); //23:59:59
                query = query.Where(p => p.InvestedOn <= investTimeTo2);
            }
            if (!string.IsNullOrWhiteSpace(investTerm) && investTerm != "请选择")
            {
                query = query.Where(p => p.DeadLine == investTerm);
            }
            if (!string.IsNullOrWhiteSpace(investState) && investState != "请选择")
            {
                query = query.Where(p => p.InvestState.Equals(investState));
            }
            if (!string.IsNullOrWhiteSpace(isBindedCard) && isBindedCard != "请选择")
            {
                query = query.Where(p => p.IsBindedCard == isBindedCard);
            }
            list = query.ToList().Select(a => new UserInviteInforViewModel
            {
                InviterId = a.InviteId == null ? "" : a.InviteId.ToString(),
                InviterAccount = a.InviteName,
                BeInvitedId = a.InvitedId == null ? "" : a.InvitedId.ToString(),
                BeInvitedAccount = a.InvitedName == null ? "----" : a.InvitedRealName == null ? a.InvitedName + "(未实名)" : a.InvitedName + "(" + a.InvitedRealName + ")",
                RegistedDateTime = a.CreatedOn == null ? "----" : a.CreatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                InvestState = a.InvestState == null ? "----" : a.InvestState,
                InvestDateTime = a.InvestedOn == null ? "----" : a.InvestedOn.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                InvestAmount = a.InvestAmount == null ? "----" : a.InvestAmount.Value.ToString(),
                InvestTotalAmount = a.InvestTotalAmount == null ? "----" : a.InvestTotalAmount.ToString(),
                InvestOrderNo = a.OrderId == null ? "----" : a.OrderId.Value.ToString(),
                InvestInformation = a.BorrowTitle == null ? "----" : a.BorrowTitle,
                InvestTerm = a.DeadLine == null ? "----" : a.DeadLine,
                IsBindedCard = a.IsBindedCard
            }).ToPagedList(Page, pageSize);


            var query1 = (from a in ef.ViewUserInviteInfors
                          orderby a.DeadLine
                          select new { a.DeadLine }).Distinct();
            ViewBag.InvestTermList = new List<SelectListItem> { };
            var investTermList = new List<SelectListItem>();
            foreach (var item in query1.ToList())
            {
                if (!string.IsNullOrWhiteSpace(item.DeadLine))
                    investTermList.Add(new SelectListItem()
                    {
                        Text = item.DeadLine,
                        Value = item.DeadLine
                    });
            }
            ViewBag.InvestTermList = investTermList;

            ViewBag.InvestStatusList = new List<SelectListItem> {
                new SelectListItem() { Text = "未投资", Value = "未投资" },
                new SelectListItem() { Text = "已投资", Value = "已投资" }};

            ViewBag.BindCardStatusList = new List<SelectListItem> {
                new SelectListItem() { Text = "未绑定", Value = "未绑定" },
                new SelectListItem() { Text = "已绑定", Value = "已绑定" }};

            ViewBag.inviterAccount = inviterAccount;
            ViewBag.invitedAccount = invitedAccount;
            ViewBag.registeTimeFrom = registeTimeFrom;
            ViewBag.registeTimeTo = registeTimeTo;
            ViewBag.investTimeFrom = investTimeFrom;
            ViewBag.investTimeTo = investTimeTo;
            ViewBag.investTerm = investTerm;
            ViewBag.investState = investState;
            ViewBag.isBindedCard = isBindedCard;
            ViewBag.TotalItemCount = list.TotalItemCount;
            return View(list);
        }

        [AdminVaildate(false)]
        public ActionResult UserInviteInforToExcel(string inviterAccount = "", string invitedAccount = "",
            DateTime? registeTimeFrom = null, DateTime? registeTimeTo = null, DateTime? investTimeFrom = null, DateTime? investTimeTo = null,
            string investTerm = "", string investState = "", string isBindedCard = "")//, int Page = 1, int pageSize = 10)
        {
            IPagedList<UserInviteInforViewModel> list = null;
            //select InvitationId, InviteId, InviteName, InvitedId, InvitedName, InvitedRealName, InviteCode, CreatedOn, InvestedOn, InvestAmount, OrderId, BorrowTitle, DeadLine, DeadLineNumber, IsBindedCard
            //from ViewUserInviteInfor
            //order by InvitationId desc
            //case when len(a.InviteName)=11 then STUFF(a.InviteName,4,4,'****') 
            var query = from a in ef.ViewUserInviteInfors
                        orderby a.invitationid descending
                        let totalAmount = (from c in ef.hx_Bid_records
                                           where c.ordstate == 1 && c.investor_registerid == a.InvitedId.Value
                                           select c.investment_amount).Sum()
                        select new
                        {
                            a.invitationid,
                            a.InviteId,
                            a.InviteName, //InviteName=(!string.IsNullOrEmpty(a.InviteName) &&a.InviteName.Length==11)? a.InviteName.Substring(0,3)+"****"+a.InviteName.Substring(7): a.InviteName,
                            a.InvitedId,
                            a.InvitedName,
                            a.InvitedRealName,
                            a.InviteCode,
                            a.CreatedOn,
                            InvestState = a.InvestedOn == null ? "未投资" : "已投资",
                            a.InvestedOn,
                            a.InvestAmount,
                            InvestTotalAmount = totalAmount,
                            a.OrderId,
                            a.BorrowTitle,
                            a.DeadLine,
                            a.DeadLineNumber,
                            IsBindedCard = a.IsBindedCard == 0 ? "未绑定" : "已绑定"
                        };
            if (!string.IsNullOrWhiteSpace(inviterAccount))
            {
                query = query.Where(p => p.InviteName == inviterAccount);
            }
            if (!string.IsNullOrWhiteSpace(invitedAccount))
            {
                query = query.Where(p => p.InvitedName == invitedAccount);
            }
            if (registeTimeFrom != null)
            {
                query = query.Where(p => p.CreatedOn >= registeTimeFrom);
            }
            if (registeTimeTo != null)
            {
                var registeTimeTo2 = registeTimeTo.Value.AddDays(1).AddSeconds(-1); //23:59:59
                query = query.Where(p => p.CreatedOn <= registeTimeTo2);
            }
            if (investTimeFrom != null)
            {
                query = query.Where(p => p.InvestedOn >= investTimeFrom);
            }
            if (investTimeTo != null)
            {
                var investTimeTo2 = investTimeTo.Value.AddDays(1).AddSeconds(-1); //23:59:59
                query = query.Where(p => p.InvestedOn <= investTimeTo2);
            }
            if (!string.IsNullOrWhiteSpace(investTerm) && investTerm != "请选择")
            {
                query = query.Where(p => p.DeadLine == investTerm);
            }
            if (!string.IsNullOrWhiteSpace(investState) && investState != "请选择")
            {
                query = query.Where(p => p.InvestState.Equals(investState));
            }
            if (!string.IsNullOrWhiteSpace(isBindedCard) && isBindedCard != "请选择")
            {
                query = query.Where(p => p.IsBindedCard == isBindedCard);
            }
            DataTable dt = DataTableHelper.CreateDataTableSimple(new List<string> { "InviterId", "InviterAccount", "BeInvitedId", "BeInvitedAccount", "RegistedDateTime", "InvestState", "InvestDateTime", "InvestAmount", "InvestTotalAmount", "InvestOrderNo", "InvestInformation", "InvestTerm", "IsBindedCard" });
            foreach (var item in query)
            {
                DataRow dr = dt.NewRow();
                dr["InviterId"] = item.InviteId;
                dr["InviterAccount"] = item.InviteName; //!(string.IsNullOrEmpty(item.InviteName) && item.InviteName.Length == 11)? item.InviteName.Substring(0, 3) + "****" + item.InviteName.Substring(7):item.InviteName;//

                dr["BeInvitedId"] = item.InvitedId;
                dr["InviterAccount"] = item.InviteName; //!(string.IsNullOrEmpty(item.InviteName) && item.InviteName.Length == 11) ? item.InviteName.Substring(0, 3) + "****" + item.InviteName.Substring(7) : item.InviteName;//
                dr["BeInvitedAccount"] = item.InvitedName; //!(string.IsNullOrEmpty(item.InvitedName) && item.InvitedName.Length == 11) ? item.InvitedName.Substring(0, 3) + "****" + item.InvitedName.Substring(7) : item.InvitedName;//
                dr["RegistedDateTime"] = item.InvestedOn;
                dr["InvestState"] = item.InvestState;
                dr["InvestDateTime"] = item.InvestedOn;
                dr["InvestAmount"] = item.InvestAmount;
                dr["InvestTotalAmount"] = item.InvestTotalAmount;
                dr["InvestOrderNo"] = item.OrderId;
                dr["InvestInformation"] = item.BorrowTitle;
                dr["InvestTerm"] = item.DeadLine;
                dr["IsBindedCard"] = item.IsBindedCard;
                dt.Rows.Add(dr);
            }
            var path = Extensions.ExportExcel(dt);
            return Content(path);
        }
        #endregion

        public ActionResult UsrUnFreeZeOP(string ordDate1, string freezeTrxId1, string valSubmit)
        {
            string resContent = "";
            if (valSubmit == "解冻")
            {
                Response.Write("日期：" + ordDate1 + " TrxId: " + freezeTrxId1);
                M_UsrUnFreeze m = ChinapnrFacade.UsrUnFreeze(ordDate1, freezeTrxId1);
                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values.Add("Version", m.Version);
                    values.Add("CmdId", m.CmdId);
                    values.Add("MerCustId", m.MerCustId);
                    values.Add("OrdId", m.OrdId);
                    values.Add("OrdDate", m.OrdDate);
                    values.Add("TrxId", m.TrxId);
                    values.Add("BgRetUrl", m.BgRetUrl);
                    values.Add("MerPriv", m.MerPriv);
                    values.Add("ChkValue", m.ChkValue);

                    string url = Utils.GetChinapnrUrl();
                    //同步发送form表单请求
                    byte[] result = client.UploadValues(url, "POST", values);
                    var retStr = Encoding.UTF8.GetString(result);
                    // Response.Write(retStr);

                    LogInfo.WriteLog("解冻:" + retStr);
                    //ReUsrUnFreeze reg = new ReUsrUnFreeze();
                    ReUsrUnFreeze retLoan = new ReUsrUnFreeze();
                    var retloan = (ReUsrUnFreeze)FastJSON.ToObject(retStr, retLoan);
                    LogInfo.WriteLog("解冻返回报文：" + FastJSON.toJOSN(retloan));
                    StringBuilder builder = new StringBuilder();
                    builder.Append(retloan.CmdId);
                    builder.Append(retloan.RespCode);
                    builder.Append(retloan.MerCustId);
                    builder.Append(retloan.OrdId);
                    builder.Append(retloan.OrdDate);
                    builder.Append(retloan.TrxId);
                    builder.Append(retloan.RetUrl);
                    builder.Append(HttpUtility.UrlDecode(retloan.BgRetUrl));
                    builder.Append(retloan.MerPriv);
                    var msg = builder.ToString();
                    LogInfo.WriteLog("解冻返回参数:" + msg);
                    //验签
                    string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
                    int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, retloan.ChkValue);
                    LogInfo.WriteLog("解冻验签ret:" + ret.ToString());
                    if (ret == 0)
                    {
                        if (retloan.RespCode == "000")
                        {
                            //Response.Write(HttpUtility.UrlDecode(retloan.RespDesc));
                        }
                    }
                    resContent = "; RespCode=" + retloan.RespCode + "; RespDesc=" + retloan.RespDesc;
                }
                Response.Write(HttpUtility.UrlDecode(resContent));
            }
            return View();
        }

        public ActionResult QueryBalanceBg(string id)
        {
            ViewBag.UsrCustId = id;
            UserInfoData uInfoData = new UserInfoData();
            var user = ef.hx_member_table.Where(c => c.UsrCustId == id).FirstOrDefault();
            if (user != null)
            {
                ReQueryBalanceBg retloan = uInfoData.Querybalance(user.registerid);
                if (retloan.RespCode == "000")
                {

                    B_usercenter bu = new B_usercenter();
                    bu.DataSync(retloan, user.registerid.ToString());
                    ViewBag.fhContent += "账户余额:  " + retloan.AcctBal;
                    ViewBag.fhContent += "；可用余额:  " + retloan.AvlBal;
                    ViewBag.fhContent += "；冻结余额:  " + retloan.FrzBal;
                    ViewBag.AvlBal = retloan.AvlBal;
                    ViewBag.FrzBal = retloan.FrzBal;
                }
                else
                {
                    ViewBag.fhContent += HttpUtility.UrlDecode(retloan.RespDesc);
                }
            }
            //ReQueryBalanceBg retloan = ChinapnrFacade.QueryBalance(id);
            //if (retloan != null)
            //{
            //    if (retloan.RespCode == "000")
            //    {
            //        ViewBag.fhContent += "账户余额:  " + retloan.AcctBal;
            //        ViewBag.fhContent += "；可用余额:  " + retloan.AvlBal;
            //        ViewBag.fhContent += "；冻结余额:  " + retloan.FrzBal;
            //        ViewBag.AvlBal = retloan.AvlBal;
            //        ViewBag.FrzBal = retloan.FrzBal;
            //    }
            //    else
            //    {
            //        ViewBag.fhContent += HttpUtility.UrlDecode(retloan.RespDesc);
            //    }
            //}
            return View();
        }

        public int GetTBXX(string AvlBal, string FrzBal, string UsrCustId)
        {
            try
            {
                //available_balance 可用余额 ; frozen_sum 冻结金额
                if (UsrCustId == "")
                {
                    return 2;
                }
                else
                {
                    string sql = "update  hx_member_table  set  available_balance=" + decimal.Parse(AvlBal) + " ,frozen_sum=" + decimal.Parse(FrzBal) + " where  UsrCustId='" + UsrCustId + "'";

                    string reCounten = DbHelperSQL.RunSql(sql);
                    return 1;

                }
            }
            catch (Exception)
            {
                return 0;
            }
            //Response.Redirect("/admin/users/Userlist.aspx");

            // Response.Write("已同步");

            ///admin/users/Userlist.aspx
        }


        [AdminVaildate(true, true)]
        public async Task<ActionResult> AsyncAllUserMoney()
        {
            return await Task.Run(() => AllUserMoney());
        }
        /// <summary>
        /// 同步用户余额
        /// </summary>
        /// <returns></returns>
        [AdminVaildate(true, true)]
        public ActionResult AllUserMoney()
        {
            UserInfoData uInfoData = new UserInfoData();
            List<int> registerIDs = ef.hx_member_table.Where(c => c.UsrCustId != null && c.UsrCustId.Length > 0).Select(c => c.registerid).ToList();
            foreach (int registerID in registerIDs)
            {
                //ReQueryBalanceBg retloan = ChinapnrFacade.QueryBalance(UsrCustId);
                //if (retloan != null)
                //{
                //    if (retloan.RespCode == "000")
                //    {
                //        string sql = "update  hx_member_table  set  available_balance=" + decimal.Parse(retloan.AvlBal) + " ,frozen_sum=" + decimal.Parse(retloan.FrzBal) + " where  UsrCustId='" + UsrCustId + "'";
                //        string reCounten = DbHelperSQL.RunSql(sql);
                //    }
                //}
                ReQueryBalanceBg retloan = uInfoData.Querybalance(registerID);
                if (retloan.RespCode == "000")
                {

                    B_usercenter bu = new B_usercenter();
                    bu.DataSync(retloan, registerID.ToString());
                }
            }
            return Content("操作完成");
        }


        /// <summary>
        /// 只投资一次的客户
        /// </summary>
        private int invone(string Message)
        {
            int RecordCount = 0;
            DataTable dt = new DataTable();
            string sql = "select count(distinct(b.mobile)) from ( select mobile from V_hx_Bid_records_borrowing_target where  ordstate=1 and IsLoans=1  group by  investor_registerid,username,mobile,realname   HAVING count(investor_registerid)=1) as b";
            dt = DBUtility.DbHelperSQL.GET_DataTable_List(sql);
            if (dt != null && dt.Rows.Count > 0) { RecordCount = dt.Rows[0][0].ToInt(); }

            string TableNames = "V_hx_Bid_records_borrowing_target";
            string PrimaryKey = "investor_registerid";
            string Fields = "mobile";
            int PageSize = 200;
            int CurrentPage = 0;
            string FilterWhere = "ordstate=1 and IsLoans=1";
            string GroupBy = "investor_registerid,username,mobile,realname   HAVING count(investor_registerid)=1";
            string Order = "investor_registerid desc";

            //计算总页数
            int pagecount = RecordCount / PageSize;
            if ((RecordCount % PageSize) > 0)
            {
                pagecount++;
            }
            int SendCount = 0;
            for (int i = 0; i < pagecount; i++)
            {
                dt = o.GetListByPageGroupBy(TableNames, PrimaryKey, Fields, PageSize, CurrentPage, FilterWhere, GroupBy, Order);

                int state = SendSMSDateTable(dt, Message);
                if (state == 0)
                {
                    SendCount += PageSize;
                }
                CurrentPage = CurrentPage + 1;
            }
            if (SendCount > RecordCount)
            {
                SendCount = RecordCount;
            }
            return SendCount;
        }



        /// <summary>
        /// 只投资二次以上的客户
        /// </summary>
        private int invtwo(string Message)
        {
            int RecordCount = 0;
            DataTable dt = new DataTable();
            string sql = "select count(distinct(b.mobile)) from ( select mobile from V_hx_Bid_records_borrowing_target where  ordstate=1 and IsLoans=1  group by  investor_registerid,username,mobile,realname   HAVING count(investor_registerid)>=2) as b";
            dt = DBUtility.DbHelperSQL.GET_DataTable_List(sql);
            if (dt != null && dt.Rows.Count > 0) { RecordCount = dt.Rows[0][0].ToInt(); }

            string TableNames = "V_hx_Bid_records_borrowing_target";
            string PrimaryKey = "investor_registerid";
            string Fields = "mobile";
            int PageSize = 200;
            int CurrentPage = 0;
            string FilterWhere = "ordstate=1 and IsLoans=1";
            string GroupBy = "investor_registerid,username,mobile,realname  HAVING count(investor_registerid)>=2";
            string Order = "investor_registerid desc";

            //计算总页数
            int pagecount = RecordCount / PageSize;
            if ((RecordCount % PageSize) > 0)
            {
                pagecount++;
            }
            int SendCount = 0;
            for (int i = 0; i < pagecount; i++)
            {
                dt = o.GetListByPageGroupBy(TableNames, PrimaryKey, Fields, PageSize, CurrentPage, FilterWhere, GroupBy, Order);

                int state = SendSMSDateTable(dt, Message);
                if (state == 0)
                {
                    SendCount += PageSize;
                }
                CurrentPage = CurrentPage + 1;
            }
            if (SendCount > RecordCount)
            {
                SendCount = RecordCount;
            }
            return SendCount;
        }



        /// <summary>
        /// 所有投资客户
        /// </summary>
        private int SetAllInvUser(string Message)
        {
            DataTable dt = new DataTable();
            int RecordCount = 0;
            dt = DBUtility.DbHelperSQL.GET_DataTable_List("select count(distinct(a.mobile)) from hx_member_table a, hx_Bid_records b where a.registerid = b.investor_registerid");
            if (dt != null && dt.Rows.Count > 0) { RecordCount = dt.Rows[0][0].ToInt(); }
            string TableNames = "hx_member_table , hx_Bid_records";
            string PrimaryKey = "hx_member_table.registerid";
            string Fields = "hx_member_table.mobile";
            int PageSize = 200;
            int CurrentPage = 0;
            string FilterWhere = "hx_member_table.registerid = hx_Bid_records.investor_registerid";
            string GroupBy = "hx_member_table.registerid,hx_member_table.username,hx_member_table.mobile,hx_member_table.realname";
            string Order = "hx_member_table.registerid desc";

            //计算总页数
            int pagecount = RecordCount / PageSize;
            if ((RecordCount % PageSize) > 0)
            {
                pagecount++;
            }
            //Response.Write("总页数:"+pagecount.ToString());
            int SendCount = 0;
            for (int i = 0; i < pagecount; i++)
            {
                dt = o.GetListByPageGroupBy(TableNames, PrimaryKey, Fields, PageSize, CurrentPage, FilterWhere, GroupBy, Order);
                int state = SendSMSDateTable(dt, Message);
                if (state == 0)
                {
                    SendCount += PageSize;
                }
                CurrentPage = CurrentPage + 1;
            }
            if (SendCount > RecordCount)
            {
                SendCount = RecordCount;
            }
            return SendCount;
        }

        private int notinv(string Message)
        {
            string sql = "  select count(mobile) from  hx_member_table h where not exists(select 1 from hx_Bid_records a where a.investor_registerid=h.registerid)";
            DataTable dt = new DataTable();
            int RecordCount = 0;

            dt = DBUtility.DbHelperSQL.GET_DataTable_List(sql);
            if (dt != null && dt.Rows.Count > 0) { RecordCount = dt.Rows[0][0].ToInt(); }
            string TableNames = "hx_member_table";
            string PrimaryKey = "hx_member_table.registerid";
            string Fields = "hx_member_table.mobile";
            int PageSize = 200;
            int CurrentPage = 0;
            string FilterWhere = "not exists(select 1 from hx_Bid_records  where hx_Bid_records.investor_registerid=hx_member_table.registerid)";
            string GroupBy = "";
            string Order = "hx_member_table.registerid desc";

            //计算总页数
            int pagecount = RecordCount / PageSize;
            if ((RecordCount % PageSize) > 0)
            {
                pagecount++;
            }

            int SendCount = 0;
            //Response.Write("总页数:" + pagecount.ToString());
            for (int i = 0; i < pagecount; i++)
            {
                dt = o.GetListByPageGroupBy(TableNames, PrimaryKey, Fields, PageSize, CurrentPage, FilterWhere, GroupBy, Order);
                int state = SendSMSDateTable(dt, Message);
                if (state == 0)
                {
                    SendCount += PageSize;
                }
                CurrentPage = CurrentPage + 1;
            }
            if (SendCount > RecordCount)
            {
                SendCount = RecordCount;
            }
            return SendCount;
        }


        B_PublicPageList o = new B_PublicPageList();
        public int oldUsers(string Message)
        {
            string sql = "  select count(registerid) from [dbo].[hx_member_table] where userstate=0";
            DataTable dt = new DataTable(); ;
            int RecordCount = 0;
            dt = DBUtility.DbHelperSQL.GET_DataTable_List(sql);
            if (dt != null && dt.Rows.Count > 0) { RecordCount = dt.Rows[0][0].ToInt(); }
            string TableNames = "hx_member_table";
            string PrimaryKey = "hx_member_table.registerid";
            string Fields = "hx_member_table.mobile";
            int PageSize = 200;
            int CurrentPage = 0;
            string FilterWhere = " userstate=0 and hx_member_table.mobile<>'' ";
            string GroupBy = "";
            string Order = "hx_member_table.registerid desc";

            //计算总页数
            int pagecount = RecordCount / PageSize;
            if ((RecordCount % PageSize) > 0)
            {
                pagecount++;

            }
            int SendCount = 0;
            //Response.Write("总页数:" + pagecount.ToString());
            for (int i = 0; i < pagecount; i++)
            {
                dt = o.GetListByPageGroupBy(TableNames, PrimaryKey, Fields, PageSize, CurrentPage, FilterWhere, GroupBy, Order);
                int state = SendSMSDateTable(dt, Message);
                if (state == 0)
                {
                    SendCount += PageSize;
                }
                CurrentPage = CurrentPage + 1;
            }
            if (SendCount > RecordCount)
            {
                SendCount = RecordCount;
            }
            return SendCount;
        }
        public int SendSMSDateTable(DataTable dt, string Message)
        {
            int state = -1;
            try
            {
                string str = "";
                string[] ary = Array.ConvertAll<DataRow, string>(dt.Rows.Cast<DataRow>().ToArray(), r => r["mobile"].ToString());
                str = string.Join(",", ary);
                // string str2 = "";
                state = SendSMS(str, Message);
                LogInfo.WriteLog("发送短信列表:" + str + "<br>");
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog(ex.ToString());
            }
            return state;

        }

        #region  客服外呼数据导出
        [AdminVaildate()]
        public ActionResult ExternalCall()
        {
            List<ViewUserCenter> lists = new List<ViewUserCenter>();

            var list = lists.ToPagedList(1, 1);

            return View(list);
        }

        [AdminVaildate(false)]
        public ActionResult ExternalCallCondition(string starttime = "", string endtime = "", int IsInvestment = -1, string investStartTime = "", string investEndTime = "", int useridentity = -1, int istruename = -1, int iscps = -1, string beginBalance = "", string endBalance = "", string investmentExpireStartDate = "", string InvestmentExpireEndDate = "", int InvestmentCount = -1, string projectterms = "0", int Page = 1, int pageSize = 10)
        {
            Utils.SetSYSDateTimeFormat();

            string where = "where 1=1 ";

            string leftwhere = " where 1=1 ";

            StringBuilder sql = new StringBuilder();

            StringBuilder leftsql = new StringBuilder();


            #region 在查询列中判断投资时间及投资产品期限
            if (IsInvestment == 1)//查询投资用户
            {
                where += " and b.allInvestMoney is not null and b.investcounts is not null ";
                if (!String.IsNullOrEmpty(investStartTime))
                {
                    leftwhere += " and invest_time>='" + investStartTime + " 00:00:00'";
                }
                if (!String.IsNullOrEmpty(investEndTime))
                {
                    leftwhere += " and invest_time<='" + investEndTime + " 23:59:59'";
                }
            }

            if (IsInvestment == 0)//查询未投资用户
            {
                where += " and a.investcount=0 and b.investcounts is null ";
            }

            string whereInvestterm = "";

            if (projectterms != "0")//投资期限
            {

                if (projectterms.IndexOf("1") >= 0)
                {
                    whereInvestterm = "life_of_loan=1";
                }
                if (projectterms.IndexOf("3") >= 0)
                {
                    if (whereInvestterm != "")
                    {
                        whereInvestterm += " or life_of_loan=3";
                    }
                    else
                    {
                        whereInvestterm = "life_of_loan=3";
                    }
                }
                if (projectterms.IndexOf("6") >= 0)
                {
                    if (whereInvestterm != "")
                    {
                        whereInvestterm += " or life_of_loan=6";
                    }
                    else
                    {
                        whereInvestterm = "life_of_loan=6";
                    }
                }

                if (whereInvestterm != "")
                {
                    whereInvestterm = " where unit_day=1 and(" + whereInvestterm + ")";
                    leftwhere += " and targetid in(select targetid from hx_borrowing_target" + whereInvestterm + ")";
                }

                if (!string.IsNullOrEmpty(investmentExpireStartDate))//理财到期开始时间
                {
                    leftwhere += " and CAST(repayment_period as Date) >= '" + investmentExpireStartDate.ToString() + " 00:00:00'";
                }
                if (!string.IsNullOrEmpty(InvestmentExpireEndDate))//理财到期结束时间
                {
                    leftwhere += " and CAST(repayment_period as Date) <= '" + InvestmentExpireEndDate.ToString() + " 23:59:59'";
                }
            }

            #endregion

            leftwhere += " Group by investor_registerid";

            #region  判断查询条件

            if (!String.IsNullOrEmpty(starttime))
            {
                where += " and a.registration_time >= '" + starttime.Trim() + " 00:00:00'";
            }
            if (!String.IsNullOrEmpty(endtime))
            {
                where += " and a.registration_time <= '" + endtime.Trim() + " 23:59:59'";
            }

            if (useridentity > -1)//用户等级
            {
                if (useridentity == 0)
                {
                    where += " and a.usertypes=0";
                }
                if (useridentity == 1)
                {
                    where += " and (a.usertypes=0 or a.usertypes=1)";
                }

            }
            else
            {
                where += " and a.usertypes=0";
            }
            if (istruename > -1)//是否实名
            {
                where += " and a.isrealname=" + istruename.ToString() + "";
                if (istruename == 1)//是
                {
                    where += " and a.UsrCustId Is Not NULL";
                }
            }
            if (iscps > -1)//是否CPS
            {
                where += " and a.IsCPS=" + iscps.ToString() + ""; ;
            }
            if (!string.IsNullOrEmpty(beginBalance))//查询账户余额条件起始值
            {
                where += " and a.available_balance>=" + beginBalance.ToString() + "";
            }
            if (!string.IsNullOrEmpty(endBalance))//查询账户余额条件最大值
            {
                where += " and a.available_balance<=" + endBalance.ToString() + "";
            }

            if (InvestmentCount >= 0)//投资次数条件
            {
                where += " and a.investCount=" + InvestmentCount.ToString() + "";
            }

            #endregion


            leftsql.Append("SELECT investor_registerid,count(*) as investcounts,SUM(investment_amount)  AS allInvestMoney FROM dbo.hx_Bid_records" + leftwhere);

            sql.Append("Select a.registerid,a.username,a.UsrCustId,a.isrealname,a.registration_time,a.mobile,a.realname,a.available_balance,a.usertypes,a.investCount,a.IsCPS,b.allInvestMoney,b.investcounts,");
            sql.Append("(case when b.investcounts>0 then '已投资' else '未投资' end) as InvestState ");
            sql.Append(" from ViewUserCenter a Left Join (");
            sql.Append(leftsql.ToString() + ") as b on a.registerid=b.investor_registerid ");
            sql.Append(where);
            sql.Append(" order by registerid desc");

            //string sqll = "select registerid,username,UsrCustId,isrealname,registration_time,mobile,realname,available_balance,usertypes,investCount,IsCPS from ViewUserCenter" + where + " order by registerid desc";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql.ToString());

            List<ViewUserCenter> lists = new List<ViewUserCenter>();

            #region  将DataTable导入到list集合

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ViewUserCenter u = new ViewUserCenter();
                    u.registerid = Convert.ToInt32(dt.Rows[i]["registerid"]);
                    u.username = dt.Rows[i]["username"].ToString();
                    u.UsrCustId = "";
                    if (!String.IsNullOrEmpty(dt.Rows[i]["UsrCustId"].ToString()))
                    {
                        u.UsrCustId = dt.Rows[i]["UsrCustId"].ToString();
                    }
                    u.isrealname = -1;
                    if (!String.IsNullOrEmpty(dt.Rows[i]["isrealname"].ToString()))
                    {
                        u.isrealname = Convert.ToInt32(dt.Rows[i]["isrealname"]);
                    }
                    u.registration_time = Convert.ToDateTime(dt.Rows[i]["registration_time"]);
                    u.mobile = dt.Rows[i]["mobile"].ToString();
                    if (!String.IsNullOrEmpty(dt.Rows[i]["realname"].ToString()))
                    {
                        u.realname = dt.Rows[i]["realname"].ToString();
                    }
                    u.available_balance = Convert.ToDecimal(dt.Rows[i]["available_balance"]);
                    u.usertypes = Convert.ToInt32(dt.Rows[i]["usertypes"]);
                    u.investCount = Convert.ToInt32(dt.Rows[i]["investCount"]);
                    u.IsCPS = Convert.ToInt32(dt.Rows[i]["IsCPS"]);
                    u.allInvestMoney = Convert.ToDecimal(0.00);
                    if (!String.IsNullOrEmpty(dt.Rows[i]["allInvestMoney"].ToString()))
                    {
                        u.allInvestMoney = Convert.ToDecimal(dt.Rows[i]["allInvestMoney"]);
                    }
                    u.investcounts = 0;
                    if (!String.IsNullOrEmpty(dt.Rows[i]["investcounts"].ToString()))
                    {
                        u.investcounts = Convert.ToInt32(dt.Rows[i]["investcounts"]);
                    }

                    u.InvestState = dt.Rows[i]["InvestState"].ToString();
                    lists.Add(u);
                }
            }

            # endregion

            //var list = ef.Database.SqlQuery<ViewUserCenter>(sql.ToString()).ToPagedList(Page, pageSize);

            var list = lists.ToPagedList(Page, pageSize);

            #region  给页面标签赋值
            ViewBag.StartTime = starttime;
            ViewBag.EndTime = endtime;
            ViewBag.useridentity = useridentity;
            ViewBag.IsTrueName = istruename;
            ViewBag.IsCPS = iscps;
            ViewBag.BeginBalance = beginBalance;
            ViewBag.EndBalance = endBalance;
            ViewBag.InvestmentExpireStartDate = investmentExpireStartDate;//理财到期开始时间
            ViewBag.InvestmentExpireEndDate = InvestmentExpireEndDate;//理财到期结束时间
            ViewBag.IsInvestment = IsInvestment;//是否已投资
            ViewBag.InvestStartTime = investStartTime;//投资开始时间
            ViewBag.InvestEndTime = investEndTime;//投资结束时间
            if (InvestmentCount != -1)
            {
                ViewBag.InvestmentCount = InvestmentCount;
            }

            ViewBag.ProjectTerms = projectterms;
            ViewBag.TotalItemCount = list.TotalItemCount;
            ViewBag.TotalPageCount = (list.TotalItemCount - 1) / pageSize + 1;

            #endregion

            return View(list);
        }

        [AdminVaildate(false)]
        public ActionResult ExternalImportData(string starttime = "", string endtime = "", int IsInvestment = 1, string investStartTime = "", string investEndTime = "", int useridentity = -1, int istruename = -1, int iscps = -1, string beginBalance = "", string endBalance = "", string investmentExpireStartDate = "", string InvestmentExpireEndDate = "", int InvestmentCount = -1, string projectterms = "0")
        {
            #region  赋值
            ViewBag.StartTime = starttime;
            ViewBag.EndTime = endtime;
            ViewBag.IsInvestment = IsInvestment;//是否已投资
            ViewBag.useridentity = useridentity;
            ViewBag.IsTrueName = istruename;
            ViewBag.IsCPS = iscps;
            ViewBag.BeginBalance = beginBalance;
            ViewBag.EndBalance = endBalance;
            ViewBag.InvestmentExpireStartDate = investmentExpireStartDate;//理财到期开始时间
            ViewBag.InvestmentExpireEndDate = InvestmentExpireEndDate;//理财到期结束时间
            ViewBag.InvestStartTime = investStartTime;//投资开始时间
            ViewBag.InvestEndTime = investEndTime;//投资结束时间
            ViewBag.InvestmentCount = InvestmentCount;
            ViewBag.ProjectTerms = projectterms;
            #endregion
            return View();
        }

        [AdminVaildate()]
        public ActionResult ExternalCallToExcel(string starttime = "", string endtime = "", int IsInvestment = 1, string investStartTime = "", string investEndTime = "", int useridentity = -1, int istruename = -1, int iscps = -1, string beginBalance = "", string endBalance = "", string investmentExpireStartDate = "", string InvestmentExpireEndDate = "", int InvestmentCount = -1, string projectterms = "0", string projectname = "", int Page = 1, int pageSize = 5)
        {
            #region  判断权限

            var userid = Utils.GetAdmUserID();
            var controllerName = this.RouteData.Values["controller"].ToString();
            var actionName = this.RouteData.Values["action"].ToString();
            if (!new UserLimitByEF().CheckAdminLimit(userid, controllerName, actionName))
            {   //无权限
                return Content("您没有操作权限");
            }

            #endregion

            Utils.SetSYSDateTimeFormat();

            #region  根据条件判断获取所需导入到傲融系统中的数据

            string where = "where 1=1 ";

            string leftwhere = " where 1=1 ";

            StringBuilder sql = new StringBuilder();

            StringBuilder leftsql = new StringBuilder();


            #region 在查询列中判断投资时间及投资产品期限
            if (IsInvestment == 1)//查询投资用户
            {
                where += " and b.allInvestMoney is not null and b.investcounts is not null";
                if (!String.IsNullOrEmpty(investStartTime))
                {
                    leftwhere += " and invest_time>='" + investStartTime + " 00:00:00'";
                }
                if (!String.IsNullOrEmpty(investEndTime))
                {
                    leftwhere += " and invest_time<='" + investEndTime + " 23:59:59'";
                }
            }

            if (IsInvestment == 0)//查询未投资用户
            {
                where += " and a.investcount=0 and b.investcounts is null ";
            }

            string whereInvestterm = "";

            if (projectterms != "0")//投资期限
            {

                if (projectterms.IndexOf("1") >= 0)
                {
                    whereInvestterm = "life_of_loan=1";
                }
                if (projectterms.IndexOf("3") >= 0)
                {
                    if (whereInvestterm != "")
                    {
                        whereInvestterm += " or life_of_loan=3";
                    }
                    else
                    {
                        whereInvestterm = "life_of_loan=3";
                    }
                }
                if (projectterms.IndexOf("6") >= 0)
                {
                    if (whereInvestterm != "")
                    {
                        whereInvestterm += " or life_of_loan=6";
                    }
                    else
                    {
                        whereInvestterm = "life_of_loan=6";
                    }
                }

                if (whereInvestterm != "")
                {
                    whereInvestterm = " where unit_day=1 and(" + whereInvestterm + ")";
                    leftwhere += " and targetid in(select targetid from hx_borrowing_target" + whereInvestterm + ")";
                }

                if (!string.IsNullOrEmpty(investmentExpireStartDate))//理财到期开始时间
                {
                    leftwhere += " and CAST(repayment_period as Date) >= '" + investmentExpireStartDate.ToString() + " 00:00:00'";
                }
                if (!string.IsNullOrEmpty(InvestmentExpireEndDate))//理财到期结束时间
                {
                    leftwhere += " and CAST(repayment_period as Date) <= '" + InvestmentExpireEndDate.ToString() + " 23:59:59'";
                }
            }

            #endregion

            leftwhere += " Group by investor_registerid";

            #region  判断查询条件

            if (!String.IsNullOrEmpty(starttime))
            {
                where += " and a.registration_time >= '" + starttime.Trim() + " 00:00:00'";
            }
            if (!String.IsNullOrEmpty(endtime))
            {
                where += " and a.registration_time <= '" + endtime.Trim() + " 23:59:59'";
            }

            if (useridentity > -1)//用户等级
            {
                if (useridentity == 0)
                {
                    where += " and a.usertypes=0";
                }
                if (useridentity == 1)
                {
                    where += " and (a.usertypes=0 or a.usertypes=1)";
                }

            }
            if (istruename > -1)//是否实名
            {
                where += " and a.isrealname=" + istruename.ToString() + "";
                if (istruename == 1)//是
                {
                    where += " and a.UsrCustId Is Not NULL";
                }
            }
            if (iscps > -1)//是否CPS
            {
                where += " and a.IsCPS=" + iscps.ToString() + ""; ;
            }
            if (!string.IsNullOrEmpty(beginBalance))//查询账户余额条件起始值
            {
                where += " and a.available_balance>=" + beginBalance.ToString() + "";
            }
            if (!string.IsNullOrEmpty(endBalance))//查询账户余额条件最大值
            {
                where += " and a.available_balance<=" + endBalance.ToString() + "";
            }

            if (InvestmentCount >= 0)//投资次数条件
            {
                where += " and a.investCount=" + InvestmentCount.ToString() + "";
            }

            #endregion


            leftsql.Append("SELECT investor_registerid,count(*) as investcounts,SUM(investment_amount)  AS allInvestMoney FROM dbo.hx_Bid_records" + leftwhere);


            sql.Append("declare @PrjName varchar(50)");
            sql.Append(" select @PrjName='" + projectname + "'");//项目名称
            sql.Append("Select @PrjName as '项目名称',a.registerid,a.mobile,a.realname,a.username, a.registration_time,");
            sql.Append("(case when b.investcounts>0 then '已投资' else '未投资' end) as 'InvestState', b.allInvestMoney,b.investcounts");
            sql.Append(" from ViewUserCenter a Left Join (");
            sql.Append(leftsql.ToString() + ") as b on a.registerid=b.investor_registerid ");
            sql.Append(where);
            sql.Append(" order by registerid desc");

            #endregion

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql.ToString());

            int importdatesuccesscount = 0;

            List<ImportErrorDataForIn> listerrordata = new List<ImportErrorDataForIn>();

            string message = "";//导入成功与否信息

            if (dt != null && dt.Rows.Count > 0)
            {
                string url = "http://192.168.0.199/api/publicCustomer/insert";

                url = ConfigurationManager.AppSettings["AoRongInsert"].ToString();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var values = new NameValueCollection();
                    values.Add("account", "admin");
                    values.Add("password", "1");
                    values.Add("moduleId", "public_customer");//往公海里添加数据
                    values.Add("mobile", dt.Rows[i]["mobile"].ToString());//手机号
                    values.Add("name", dt.Rows[i]["realname"].ToString());//姓名
                    values.Add("extend5", dt.Rows[i]["username"].ToString());//登录名
                    values.Add("extend45", dt.Rows[i]["registration_time"].ToString());//注册时间yyyy-MM-dd HH:mm:ss
                    values.Add("extend1", dt.Rows[i]["InvestState"].ToString());//投资状态
                    values.Add("extend34", dt.Rows[i]["allInvestMoney"].ToString());//投资金额
                    values.Add("extend16", dt.Rows[i]["investcounts"].ToString());//投资笔数
                    values.Add("extend17", dt.Rows[i]["registerid"].ToString());//客服id
                    values.Add("extend3", projectname);//项目名称

                    var result = JsonHelper.JsonToObject<ImportErrorData>(HttpHelper.Post(url, values));

                    if (result.success == "True")
                    {
                        importdatesuccesscount += 1;
                    }
                    else
                    {
                        ImportErrorData.Errorparamslist errorparam = result.ErrorParams;


                        ImportErrorDataForIn vuc = new ImportErrorDataForIn();
                        vuc.registerid = Convert.ToInt32(result.ErrorParams.extend17);
                        vuc.mobile = result.ErrorParams.mobile;
                        vuc.realname = result.ErrorParams.name;
                        vuc.username = result.ErrorParams.extend5;
                        vuc.registration_time = Convert.ToDateTime(result.ErrorParams.extend45);
                        vuc.InvestState = result.ErrorParams.extend1;
                        vuc.allInvestMoney = Convert.ToDecimal(result.ErrorParams.extend34);
                        vuc.investCount = Convert.ToInt32(result.ErrorParams.extend16);
                        vuc.errorreason = result.errors.mobile;

                        listerrordata.Add(vuc);
                    }
                }

            }
            else
            {
                message = "空信息不可导入";
            }

            ViewBag.Message = message;

            ViewBag.AllImportDataCount = dt.Rows.Count;//总共需要导入的数据量
            ViewBag.ImportDataSuccessDataCount = importdatesuccesscount;//成功导入的数据量
            ViewBag.ImportDataErrorDataCount = listerrordata.Count();//导入数据失败的量

            ViewBag.TotalItemCount = listerrordata.Count();
            ViewBag.TotalPageCount = (listerrordata.Count() - 1) / pageSize + 1;

            ViewBag.ErrorViewUserCenter = listerrordata.ToPagedList(Page, pageSize);

            if (listerrordata.Count() > 0 && importdatesuccesscount > 0)
            {
                message = "数据导入成功,部分数据导入失败！";
            }
            if (listerrordata.Count() < 0 && importdatesuccesscount > 0)
            {
                message = "数据导入成功！";
            }

            var list = listerrordata.ToPagedList(1, listerrordata.Count() == 0 ? 1 : listerrordata.Count());

            return View(list);
        }



        #endregion

        #region 客服外呼数据统计报表

        //[HttpPost]
        //[ValidateAntiForgeryToken]

        [AdminVaildate()]
        public ActionResult StatisticsReport()
        {

            List<StatisticsEmployee> list = new List<StatisticsEmployee>();

            var listemployee = list.ToPagedList(1, 1);

            return View(listemployee);
        }

        [AdminVaildate(false)]
        public ActionResult StatisticsReportCondition(string starttime = "", string endtime = "", string searchterm = "0", int Page = 1, int pageSize = 10)
        {
            Utils.SetSYSDateTimeFormat();

            DataTable dt = new DataTable();

            List<StatisticsEmployee> list = null;

            List<StatisticsReport> listReport = new List<StatisticsReport>();

            #region  定义、查询、赋值变量
            var AllCallOutNumber = 0;//总的电话量

            var AllcallOutSuccessNumber = 0;//总的电话接通量

            var AllcallOutValidNumber = 0;//总的电话有效量

            var AllConnectRate = "0";//总的接通率

            var AllValidRate = "0";//总的有效率

            var AllFirstCount = 0;//总的首投次数

            var AllFirstMoney = 0.00;//总的首投金额

            var AllComplexMoney = 0.00;//总的复投金额

            var AllComplexCount = 0;//总的复投次数

            var AllAllMoney = 0.00;//总的投资总金额

            var AllFractureMoney = 0.00;//总的折标总金额

            var AllRechargeAllMoney = 0.00;//总的充值总金额

            var AllJanMoney = 0.00;//总的一月期总额

            var AllMarMoney = 0.00;//总的三月期总额

            var ALlJunMoney = 0.00;//总的六月期总额

            #endregion

            StringBuilder str = new StringBuilder();

            string url = "http://192.168.0.199/api/callOut/count";//接口地址

            url = ConfigurationManager.AppSettings["AoRongStatistics"].ToString();

            var values = new NameValueCollection();
            values.Add("account", "admin");
            values.Add("password", "1");
            values.Add("viewId", "CallCountList");

            if (!String.IsNullOrEmpty(starttime) || !String.IsNullOrEmpty(endtime))
            {
                values.Add("type", "CUSTOM");
                if (!String.IsNullOrEmpty(starttime))
                {
                    values.Add("startDate", starttime + " 00:00:00");
                }
                if (!String.IsNullOrEmpty(endtime))
                {
                    values.Add("endDate", endtime + " 23:59:59");
                }
                else
                {
                    values.Add("endDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }

            }

            try
            {
                var result = JsonHelper.JsonToObject<Return_StatisticsEmployee>(HttpHelper.Post(url, values));//调用接口地址获取结果集


                if (result.success == "True")
                {
                    list = result.data;
                }
                else
                {
                    return View();
                }


                #region 业绩统计
                if (list != null && list.Count > 0)
                {

                    foreach (var item in list)
                    {
                        string phonelist = GetCustomerPhoneByEmployeeID(item.id, starttime, endtime);

                        StatisticsReport sr = new StatisticsReport();

                        sr.EmployeeName = item.employeeName;
                        sr.EmployeeNumber = item.exten;//员工工号

                        if (!String.IsNullOrEmpty(phonelist))
                        {
                            StringBuilder sqlAchivement = new StringBuilder();
                            sqlAchivement.Append("select sum(case when IsFirstInvest='首投' Then 1 ElSE 0 End) as '首投人数',sum(case when IsFirstInvest = '首投' Then InvestAllMoney ElSE 0 End) as '首投金额',sum(case when IsFirstInvest = '复投' Then 1 ElSE 0 End) as '复投人数',sum(case when IsFirstInvest = '复投' Then InvestAllMoney ElSE 0 End) as '复投金额',sum(InvestAllMoney) as '投资总金额', sum(FoldAllMoney) as '折标总金额',sum(RechargeAllMoney) as '充值总金额', sum(JanMoney) as '一月期总额',sum(MarMoney) as '三月期总额', sum(JunMoney) as '六月期总额' from ViewDetailsReport ");

                            sqlAchivement.Append(phonelist);

                            dt = DbHelperSQL.GET_DataTable_List(sqlAchivement.ToString());


                            sr.FirstCount = dt.Rows[0]["首投人数"] != null && dt.Rows[0]["首投人数"].ToString() != "" ? Convert.ToInt32(dt.Rows[0]["首投人数"]) : 0;
                            sr.FirstMoney = dt.Rows[0]["首投金额"] != null && dt.Rows[0]["首投金额"].ToString() != "" ? Convert.ToDecimal(dt.Rows[0]["首投金额"]) : 0;
                            sr.complexCount = dt.Rows[0]["复投人数"] != null && dt.Rows[0]["复投人数"].ToString() != "" ? Convert.ToInt32(dt.Rows[0]["复投人数"]) : 0;
                            sr.complexMoney = dt.Rows[0]["复投金额"] != null && dt.Rows[0]["复投金额"].ToString() != "" ? Convert.ToDecimal(dt.Rows[0]["复投金额"]) : 0;
                            sr.InvestAllMoney = dt.Rows[0]["投资总金额"] != null && dt.Rows[0]["投资总金额"].ToString() != "" ? Convert.ToDecimal(dt.Rows[0]["投资总金额"]) : 0;
                            sr.FoldAllMoney = dt.Rows[0]["折标总金额"] != null && dt.Rows[0]["折标总金额"].ToString() != "" ? Convert.ToDecimal(dt.Rows[0]["折标总金额"]) : 0;
                            sr.RechargeAllMoney = dt.Rows[0]["充值总金额"] != null && dt.Rows[0]["充值总金额"].ToString() != "" ? Convert.ToDecimal(dt.Rows[0]["充值总金额"]) : 0;
                            sr.JanMoney = dt.Rows[0]["一月期总额"] != null && dt.Rows[0]["一月期总额"].ToString() != "" ? Convert.ToDecimal(dt.Rows[0]["一月期总额"]) : 0;
                            sr.MarMoney = dt.Rows[0]["三月期总额"] != null && dt.Rows[0]["三月期总额"].ToString() != "" ? Convert.ToDecimal(dt.Rows[0]["三月期总额"]) : 0;
                            sr.JunMoney = dt.Rows[0]["六月期总额"] != null && dt.Rows[0]["六月期总额"].ToString() != "" ? Convert.ToDecimal(dt.Rows[0]["六月期总额"]) : 0;
                        }
                        else
                        {
                            sr.FirstCount = 0;
                            sr.FirstMoney = 0;
                            sr.complexCount = 0;
                            sr.complexMoney = 0;
                            sr.InvestAllMoney = 0;
                            sr.FoldAllMoney = 0;
                            sr.RechargeAllMoney = 0;
                            sr.JanMoney = 0;
                            sr.MarMoney = 0;
                            sr.JunMoney = 0;
                        }


                        listReport.Add(sr);

                        #region  计算总的投资金额等情况
                        AllFirstCount += sr.FirstCount;
                        AllFirstMoney += Convert.ToDouble(sr.FirstMoney);
                        AllComplexCount += sr.complexCount;
                        AllComplexMoney += Convert.ToDouble(sr.complexMoney);
                        AllAllMoney += Convert.ToDouble(sr.InvestAllMoney);
                        AllFractureMoney += Convert.ToDouble(sr.FoldAllMoney);
                        AllRechargeAllMoney += Convert.ToDouble(sr.RechargeAllMoney);
                        AllJanMoney += Convert.ToDouble(sr.JanMoney);
                        AllMarMoney += Convert.ToDouble(sr.MarMoney);
                        ALlJunMoney += Convert.ToDouble(sr.JunMoney);

                        #endregion

                        #region 计算电话量情况

                        AllCallOutNumber += Convert.ToInt32(item.callOutNumber);//总的电话量
                        AllcallOutSuccessNumber += Convert.ToInt32(item.callOutSuccessNumber);//总的接通量
                        AllcallOutValidNumber += Convert.ToInt32(item.callOutValidNumber);//总的有效量

                        #endregion
                    }

                    AllConnectRate = (AllcallOutSuccessNumber * 100 / AllCallOutNumber).ToString() + "%";//总的接通率
                    AllValidRate = (AllcallOutValidNumber * 100 / AllCallOutNumber).ToString() + "%";//总的有效率
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog(ex.ToString());
            }

            var listemployee = list.ToPagedList(Page, pageSize);

            #region 赋值

            ViewBag.StartTime = starttime;
            ViewBag.EndTime = endtime;
            ViewBag.searchterm = searchterm;

            ViewBag.AllCallOutNumber = AllCallOutNumber;//电话量
            ViewBag.AllcallOutSuccessNumber = AllcallOutSuccessNumber;//接通量
            ViewBag.AllConnectRate = AllConnectRate;//接通率
            ViewBag.AllcallOutValidNumber = AllcallOutValidNumber;//有效量
            ViewBag.AllValidRate = AllValidRate;//有效率
            ViewBag.AllFirstCount = AllFirstCount;//总的首投人数
            ViewBag.AllFirstMoney = AllFirstMoney;//总的首投金额
            ViewBag.AllComplexCount = AllComplexCount;//总的复投人数
            ViewBag.AllComplexMoney = AllComplexMoney;//总的复投金额
            ViewBag.AllAllMoney = AllAllMoney;//总的投资总金额
            ViewBag.AllFractureMoney = AllFractureMoney;//总的折标总金额
            ViewBag.AllRechargeAllMoney = AllRechargeAllMoney;//总的充值总金额
            ViewBag.AllJanMoney = AllJanMoney;//总的一月期总额
            ViewBag.AllMarMoney = AllMarMoney;//总的三月期总额
            ViewBag.AllJunMoney = ALlJunMoney;//总的六月期总额

            ViewBag.ListStatisticsReport = listReport;

            #endregion

            return View(listemployee);
        }


        /// <summary>
        /// 根据员工id获取该名员工下的所有客户电话号码
        /// </summary>
        /// <param name="id"></param>
        [AdminVaildate(false)]
        private string GetCustomerPhoneByEmployeeID(string id, string startTime1, string endTime1)
        {
            List<string> phonelist = new List<string>();

            StringBuilder sbphone = new StringBuilder();

            int totalCount = 0;

            int urlPageSize = 50;

            int pageCount = 0;

            string url = "http://192.168.0.199/api/callOut/list";

            url = ConfigurationManager.AppSettings["AoRongDetails"].ToString();

            var values = new NameValueCollection();
            values.Add("account", "admin");
            values.Add("password", "1");
            values.Add("empId", id);

            if (!String.IsNullOrEmpty(startTime1) || !String.IsNullOrEmpty(endTime1))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[{");
                sb.Append("\"searchFieldName\":\"dateCreated\",\"searchFieldOperation\":\"ilike\",\"searchFieldValue\":\"CUSTOM\",\"dbType\":\"java.util.Date\"");
                if (!String.IsNullOrEmpty(startTime1))
                {
                    sb.Append(",\"startDateFieldValue\":\"" + startTime1 + " 00:00:00\"");
                }
                else
                {
                    sb.Append(",\"startDateFieldValue\":\"1970-01-01 00:00:00\"");
                }
                if (!String.IsNullOrEmpty(endTime1))
                {
                    sb.Append(",\"endDateFieldValue\":\"" + endTime1 + " 23:59:59\"");
                }
                else
                {
                    var datenow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    sb.Append(",\"endDateFieldValue\":\"" + datenow + "\"");
                }

                sb.Append("}]");
                values.Add("searchCondition", sb.ToString());
            }

            var result = JsonHelper.JsonToObject<Return_EmployeeDetails>(HttpHelper.Post(url, values));

            //StringBuilder sbphonelist = new StringBuilder();

            if (result.success == "True")
            {
                totalCount = result.total;//请求总数

                pageCount = (totalCount - 1) / urlPageSize + 1;

                if (totalCount > urlPageSize)
                {
                    values.Add("limit", urlPageSize.ToString());
                    for (int i = 0; i < pageCount; i++)
                    {
                        values.Remove("page");
                        values.Add("page", (i + 1).ToString());

                        var resultInfor = JsonHelper.JsonToObject<Return_EmployeeDetails>(HttpHelper.Post(url, values));
                        if (resultInfor.success == "True")
                        {
                            var employeedetailslist = resultInfor.data;

                            if (employeedetailslist.Count != 0)
                            {

                                for (int j = 0; j < employeedetailslist.Count(); j++)
                                {
                                    if (!String.IsNullOrEmpty(employeedetailslist[j].calledNumber))
                                    {
                                        if (employeedetailslist[j].calledNumber.Length == 12)
                                        {
                                            phonelist.Add(employeedetailslist[j].calledNumber.Substring(1, 11));
                                        }
                                        else if (employeedetailslist[j].calledNumber.Length == 11)
                                        {
                                            phonelist.Add(employeedetailslist[j].calledNumber);
                                        }
                                    }
                                }
                            }
                        }


                    }

                }
                else
                {
                    var resultInfor = JsonHelper.JsonToObject<Return_EmployeeDetails>(HttpHelper.Post(url, values));
                    if (resultInfor.success == "True")
                    {
                        var employeedetailslist = resultInfor.data;

                        if (employeedetailslist.Count != 0)
                        {

                            for (int j = 0; j < employeedetailslist.Count(); j++)
                            {
                                if (employeedetailslist[j].calledNumber.Length == 12)
                                {
                                    phonelist.Add(employeedetailslist[j].calledNumber.Substring(1, 11));
                                }
                                else if (employeedetailslist[j].calledNumber.Length == 11)
                                {
                                    phonelist.Add(employeedetailslist[j].calledNumber);
                                }

                            }
                        }
                    }
                    else
                    {
                        return "";
                    }
                }


                if (phonelist != null && phonelist.Count() > 0)
                {
                    sbphone.Append(" where 1=1 and mobile in (");

                    for (int kk = 0; kk < phonelist.Count(); kk++)
                    {
                        sbphone.Append("'" + phonelist[kk] + "'");
                        if (kk < phonelist.Count() - 1)
                        {
                            sbphone.Append(",");
                        }
                    }
                    sbphone.Append(")");
                }
                else
                {
                    return "";
                }



            }

            return sbphone.ToString();
        }

        [AdminVaildate(false)]
        public ActionResult StatisticsReportToExcel(string starttime = "", string endtime = "", string searchterm = "0")
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

            DataTable dt = new DataTable();

            DataTable dtExcel = new DataTable();

            List<StatisticsEmployee> list = null;

            List<StatisticsReport> listReport = new List<StatisticsReport>();

            #region  定义、查询、赋值变量
            var AllCallOutNumber = 0;//总的电话量

            var AllcallOutSuccessNumber = 0;//总的电话接通量

            var AllcallOutValidNumber = 0;//总的电话有效量

            var AllConnectRate = "0";//总的接通率

            var AllValidRate = "0";//总的有效率

            var AllFirstCount = 0;//总的首投次数

            var AllFirstMoney = 0.00;//总的首投金额

            var AllComplexMoney = 0.00;//总的复投金额

            var AllComplexCount = 0;//总的复投次数

            var AllAllMoney = 0.00;//总的投资总金额

            var AllFractureMoney = 0.00;//总的折标总金额

            var AllRechargeAllMoney = 0.00;//总的充值总金额

            var AllJanMoney = 0.00;//总的一月期总额

            var AllMarMoney = 0.00;//总的三月期总额

            var ALlJunMoney = 0.00;//总的六月期总额

            #endregion

            StringBuilder str = new StringBuilder();

            string url = "http://192.168.0.199/api/callOut/count";//接口地址

            url = ConfigurationManager.AppSettings["AoRongStatistics"].ToString();

            var values = new NameValueCollection();
            values.Add("account", "admin");
            values.Add("password", "1");
            values.Add("viewId", "CallCountList");
            if (!String.IsNullOrEmpty(starttime))
            {
                values.Add("startDate", starttime + " 00:00:00");
            }
            if (!String.IsNullOrEmpty(endtime))
            {
                values.Add("endDate", endtime + " 23:59:59");
            }
            else
            {
                values.Add("endDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            try
            {
                var result = JsonHelper.JsonToObject<Return_StatisticsEmployee>(HttpHelper.Post(url, values));//调用接口地址获取结果集


                if (result.success == "True")
                {
                    list = result.data;
                }

                #region   导出KPI
                if (searchterm == "0")
                {
                    dtExcel.Columns.Add("员工姓名");
                    dtExcel.Columns.Add("员工工号");
                    dtExcel.Columns.Add("电话量");
                    dtExcel.Columns.Add("接通量");
                    dtExcel.Columns.Add("接通率");
                    dtExcel.Columns.Add("有效量");
                    dtExcel.Columns.Add("有效率");

                    foreach (var item in list)
                    {
                        DataRow dr = dtExcel.NewRow();
                        dr["员工姓名"] = item.employeeName;
                        dr["员工工号"] = item.exten;
                        dr["电话量"] = item.callOutNumber;
                        dr["接通量"] = item.callOutSuccessNumber;
                        dr["接通率"] = Convert.ToInt32(item.callOutNumber) == 0 || item.callOutNumber == null ? "0%" : Convert.ToString(Convert.ToInt32(item.callOutSuccessNumber) * 100 / Convert.ToInt32(item.callOutNumber)) + "%";
                        dr["有效量"] = item.callOutValidNumber;
                        dr["有效率"] = Convert.ToInt32(item.callOutNumber) == 0 || item.callOutNumber == null ? "0%" : Convert.ToString(Convert.ToInt32(item.callOutValidNumber) * 100 / Convert.ToInt32(item.callOutNumber)) + "%";

                        dtExcel.Rows.Add(dr);

                        #region 计算总计
                        AllCallOutNumber += Convert.ToInt32(item.callOutNumber);//总的电话量
                        AllcallOutSuccessNumber += Convert.ToInt32(item.callOutSuccessNumber);//总的接通量
                        AllcallOutValidNumber += Convert.ToInt32(item.callOutValidNumber);//总的有效量
                        #endregion
                    }
                    AllConnectRate = Convert.ToString(AllcallOutSuccessNumber * 100 / AllCallOutNumber) + "%";//总的接通率
                    AllValidRate = Convert.ToString(AllcallOutValidNumber * 100 / AllCallOutNumber) + "%";//总的有效率
                    DataRow drr = dtExcel.NewRow();
                    drr["员工姓名"] = "";
                    drr["员工工号"] = "总计";
                    drr["电话量"] = AllCallOutNumber.ToString();
                    drr["接通量"] = AllcallOutSuccessNumber.ToString();
                    drr["接通率"] = AllConnectRate;
                    drr["有效量"] = AllcallOutValidNumber.ToString();
                    drr["有效率"] = AllValidRate;
                    dtExcel.Rows.Add(drr);

                }
                #endregion

                if (searchterm == "1")
                {
                    dtExcel.Columns.Add("员工姓名");
                    dtExcel.Columns.Add("员工工号");
                    dtExcel.Columns.Add("首投人数");
                    dtExcel.Columns.Add("首投金额");
                    dtExcel.Columns.Add("复投人数");
                    dtExcel.Columns.Add("复投金额");
                    dtExcel.Columns.Add("投资总金额");
                    dtExcel.Columns.Add("折标总金额");
                    dtExcel.Columns.Add("充值总金额");
                    dtExcel.Columns.Add("一月期总额");
                    dtExcel.Columns.Add("三月期总额");
                    dtExcel.Columns.Add("六月期总额");
                    #region 业绩统计
                    if (list != null && list.Count > 0)
                    {

                        foreach (var item in list)
                        {
                            string phonelist = GetCustomerPhoneByEmployeeID(item.id, starttime, endtime);

                            StatisticsReport sr = new StatisticsReport();

                            sr.EmployeeName = item.employeeName;
                            sr.EmployeeNumber = item.exten;//员工工号

                            if (!String.IsNullOrEmpty(phonelist))
                            {
                                StringBuilder sqlAchivement = new StringBuilder();
                                sqlAchivement.Append("select sum(case when IsFirstInvest='首投' Then 1 ElSE 0 End) as '首投人数',sum(case when IsFirstInvest = '首投' Then InvestAllMoney ElSE 0 End) as '首投金额',sum(case when IsFirstInvest = '复投' Then 1 ElSE 0 End) as '复投人数',sum(case when IsFirstInvest = '复投' Then InvestAllMoney ElSE 0 End) as '复投金额',sum(InvestAllMoney) as '投资总金额', sum(FoldAllMoney) as '折标总金额',sum(RechargeAllMoney) as '充值总金额', sum(JanMoney) as '一月期总额',sum(MarMoney) as '三月期总额', sum(JunMoney) as '六月期总额' from ViewDetailsReport ");

                                sqlAchivement.Append(phonelist);

                                dt = DbHelperSQL.GET_DataTable_List(sqlAchivement.ToString());


                                sr.FirstCount = dt.Rows[0]["首投人数"] != null && dt.Rows[0]["首投人数"].ToString() != "" ? Convert.ToInt32(dt.Rows[0]["首投人数"]) : 0;
                                sr.FirstMoney = dt.Rows[0]["首投金额"] != null && dt.Rows[0]["首投金额"].ToString() != "" ? Convert.ToDecimal(dt.Rows[0]["首投金额"]) : 0;
                                sr.complexCount = dt.Rows[0]["复投人数"] != null && dt.Rows[0]["复投人数"].ToString() != "" ? Convert.ToInt32(dt.Rows[0]["复投人数"]) : 0;
                                sr.complexMoney = dt.Rows[0]["复投金额"] != null && dt.Rows[0]["复投金额"].ToString() != "" ? Convert.ToDecimal(dt.Rows[0]["复投金额"]) : 0;
                                sr.InvestAllMoney = dt.Rows[0]["投资总金额"] != null && dt.Rows[0]["投资总金额"].ToString() != "" ? Convert.ToDecimal(dt.Rows[0]["投资总金额"]) : 0;
                                sr.FoldAllMoney = dt.Rows[0]["折标总金额"] != null && dt.Rows[0]["折标总金额"].ToString() != "" ? Convert.ToDecimal(dt.Rows[0]["折标总金额"]) : 0;
                                sr.RechargeAllMoney = dt.Rows[0]["充值总金额"] != null && dt.Rows[0]["充值总金额"].ToString() != "" ? Convert.ToDecimal(dt.Rows[0]["充值总金额"]) : 0;
                                sr.JanMoney = dt.Rows[0]["一月期总额"] != null && dt.Rows[0]["一月期总额"].ToString() != "" ? Convert.ToDecimal(dt.Rows[0]["一月期总额"]) : 0;
                                sr.MarMoney = dt.Rows[0]["三月期总额"] != null && dt.Rows[0]["三月期总额"].ToString() != "" ? Convert.ToDecimal(dt.Rows[0]["三月期总额"]) : 0;
                                sr.JunMoney = dt.Rows[0]["六月期总额"] != null && dt.Rows[0]["六月期总额"].ToString() != "" ? Convert.ToDecimal(dt.Rows[0]["六月期总额"]) : 0;
                            }
                            else
                            {
                                sr.FirstCount = 0;
                                sr.FirstMoney = 0;
                                sr.complexCount = 0;
                                sr.complexMoney = 0;
                                sr.InvestAllMoney = 0;
                                sr.FoldAllMoney = 0;
                                sr.RechargeAllMoney = 0;
                                sr.JanMoney = 0;
                                sr.MarMoney = 0;
                                sr.JunMoney = 0;
                            }


                            listReport.Add(sr);

                            #region  计算总的投资金额等情况
                            AllFirstCount += sr.FirstCount;
                            AllFirstMoney += Convert.ToDouble(sr.FirstMoney);
                            AllComplexCount += sr.complexCount;
                            AllComplexMoney += Convert.ToDouble(sr.complexMoney);
                            AllAllMoney += Convert.ToDouble(sr.InvestAllMoney);
                            AllFractureMoney += Convert.ToDouble(sr.FoldAllMoney);
                            AllRechargeAllMoney += Convert.ToDouble(sr.RechargeAllMoney);
                            AllJanMoney += Convert.ToDouble(sr.JanMoney);
                            AllMarMoney += Convert.ToDouble(sr.MarMoney);
                            ALlJunMoney += Convert.ToDouble(sr.JunMoney);

                            #endregion
                        }

                        foreach (var item in listReport)
                        {
                            DataRow dr = dtExcel.NewRow();
                            dr["员工姓名"] = item.EmployeeName;
                            dr["员工工号"] = item.EmployeeNumber;
                            dr["首投人数"] = item.FirstCount;
                            dr["首投金额"] = item.FirstMoney;
                            dr["复投人数"] = item.complexCount;
                            dr["复投金额"] = item.complexMoney;
                            dr["投资总金额"] = item.InvestAllMoney;
                            dr["折标总金额"] = item.FoldAllMoney;
                            dr["充值总金额"] = item.RechargeAllMoney;
                            dr["一月期总额"] = item.JanMoney;
                            dr["三月期总额"] = item.MarMoney;
                            dr["六月期总额"] = item.JunMoney;

                            dtExcel.Rows.Add(dr);
                        }
                        DataRow drr = dtExcel.NewRow();
                        drr["员工姓名"] = "";
                        drr["员工工号"] = "总计";
                        drr["首投人数"] = AllFirstCount.ToString();
                        drr["首投金额"] = AllFirstMoney.ToString();
                        drr["复投人数"] = AllComplexCount.ToString();
                        drr["复投金额"] = AllComplexMoney.ToString();
                        drr["投资总金额"] = AllAllMoney.ToString();
                        drr["折标总金额"] = AllFractureMoney.ToString();
                        drr["充值总金额"] = AllRechargeAllMoney.ToString();
                        drr["一月期总额"] = AllJanMoney.ToString();
                        drr["三月期总额"] = AllMarMoney.ToString();
                        drr["六月期总额"] = ALlJunMoney.ToString();

                        dtExcel.Rows.Add(drr);
                    }

                    #endregion
                }

            }
            catch (Exception ex)
            {
                LogInfo.WriteLog(ex.ToString());
            }

            var path = Extensions.ExportExcel(dtExcel);

            return Content(path);
        }

        #endregion

        #region  客服外呼数据明细报表
        [AdminVaildate()]
        public ActionResult DetailsReport()
        {
            List<ViewDetailsReport> lists = new List<ViewDetailsReport>();

            var list = lists.ToPagedList(1, 1);

            ViewBag.EmployeeList = GetStaffName();//绑定员工姓名列表

            return View(list);
        }

        [AdminVaildate(false)]
        public ActionResult DetailsReportCondition(string starttime = "", string endtime = "", string staffname = "", int Page = 1, int pageSize = 10)
        {
            List<ViewDetailsReport> lists = new List<ViewDetailsReport>();

            int totalCount = 0;

            int pagecount = 0;

            #region  定义接口地址、参数

            string url = "http://192.168.0.199/api/callOut/list";

            url = ConfigurationManager.AppSettings["AoRongDetails"].ToString();

            int urlPageSize = 50;

            var values = new NameValueCollection();
            values.Add("account", "admin");
            values.Add("password", "1");
            values.Add("page", Page.ToString());
            values.Add("limit", pageSize.ToString());
            if (staffname != "-1" && staffname != "")
            {
                values.Add("empId", staffname);
            }

            if (!String.IsNullOrEmpty(starttime) || !String.IsNullOrEmpty(endtime))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[{");
                sb.Append("\"searchFieldName\":\"dateCreated\",\"searchFieldOperation\":\"ilike\",\"searchFieldValue\":\"CUSTOM\",\"dbType\":\"java.util.Date\"");
                if (!String.IsNullOrEmpty(starttime))
                {
                    sb.Append(",\"startDateFieldValue\":\"" + starttime + " 00:00:00\"");
                }
                else
                {
                    sb.Append(",\"startDateFieldValue\":\"1970-01-01 00:00:00\"");
                }
                if (!String.IsNullOrEmpty(endtime))
                {
                    sb.Append(",\"endDateFieldValue\":\"" + endtime + " 23:59:59\"");
                }
                else
                {
                    var datenow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    sb.Append(",\"endDateFieldValue\":\"" + datenow + "\"");
                }
                sb.Append("}]");
                values.Add("searchCondition", sb.ToString());
            }
            #endregion

            try
            {
                var result = JsonHelper.JsonToObject<Return_EmployeeDetails>(HttpHelper.Post(url, values));

                totalCount = result.total;

                DataTable dt = new DataTable();

                if (result.success == "True")
                {
                    if (totalCount > urlPageSize)
                    {
                        pagecount = (totalCount - 1) / urlPageSize + 1;

                        values.Remove("limit");
                        values.Add("limit", urlPageSize.ToString());

                        for (int i = 0; i < pagecount; i++)
                        {
                            values.Remove("page");
                            values.Add("page", (i + 1).ToString());

                            var resultInfor = JsonHelper.JsonToObject<Return_EmployeeDetails>(HttpHelper.Post(url, values));

                            if (resultInfor.success == "True")
                            {
                                #region 获得数据
                                var employeedetailslist = resultInfor.data;
                                if (employeedetailslist.Count != 0)
                                {
                                    StringBuilder sql = new StringBuilder();

                                    sql.Append("select * from ViewDetailsReport where 1=1");

                                    //if (!String.IsNullOrEmpty(starttime))
                                    //{
                                    //    sql.Append(" and registration_time>='" + starttime + "'");
                                    //}
                                    //if (!String.IsNullOrEmpty(endtime))
                                    //{
                                    //    sql.Append(" and registration_time<='" + endtime + "'");
                                    //}

                                    StringBuilder sbphonelist = new StringBuilder();
                                    sbphonelist.Append("(");
                                    for (int k = 0; k < employeedetailslist.Count(); k++)
                                    {
                                        if (!String.IsNullOrEmpty(employeedetailslist[k].calledNumber))
                                        {
                                            if (employeedetailslist[k].calledNumber.Length == 12)
                                            {
                                                sbphonelist.Append("'" + employeedetailslist[k].calledNumber.Substring(1, 11) + "'");
                                            }
                                            else if (employeedetailslist[k].calledNumber.Length == 11)
                                            {
                                                sbphonelist.Append("'" + employeedetailslist[k].calledNumber + "'");
                                            }
                                            if (k < employeedetailslist.Count() - 1)
                                            {
                                                //sbphonelist.Append("'',''");
                                                sbphonelist.Append(",");
                                            }
                                        }

                                    }
                                    sbphonelist.Append(")");
                                    sql.Append(" and mobile in" + sbphonelist.ToString());

                                    dt = DbHelperSQL.GET_DataTable_List(sql.ToString());

                                }

                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        ViewDetailsReport u = new ViewDetailsReport();
                                        u.registerid = Convert.ToInt32(dt.Rows[j]["registerid"]);
                                        u.mobile = dt.Rows[j]["mobile"].ToString();
                                        u.username = dt.Rows[j]["username"].ToString();
                                        u.realname = dt.Rows[j]["realname"].ToString();
                                        u.registration_time = Convert.ToDateTime(dt.Rows[j]["registration_time"]);
                                        u.InvestAllMoney = Convert.ToDecimal(dt.Rows[j]["InvestAllMoney"]);
                                        u.FoldAllMoney = Convert.ToDecimal(dt.Rows[j]["FoldAllMoney"]);
                                        u.RechargeAllMoney = Convert.ToDecimal(dt.Rows[j]["RechargeAllMoney"]);
                                        u.IsFirstInvest = dt.Rows[j]["IsFirstInvest"].ToString();
                                        u.JanMoney = Convert.ToDecimal(dt.Rows[j]["JanMoney"]);
                                        u.MarMoney = Convert.ToDecimal(dt.Rows[j]["MarMoney"]);
                                        u.JunMoney = Convert.ToDecimal(dt.Rows[j]["JunMoney"]);
                                        lists.Add(u);
                                    }
                                }

                                #endregion
                            }
                        }
                    }
                    else
                    {
                        #region 获得数据
                        var employeedetailslist = result.data;
                        if (employeedetailslist.Count != 0)
                        {
                            StringBuilder sql = new StringBuilder();

                            sql.Append("select * from ViewDetailsReport where 1=1");

                            //if (!String.IsNullOrEmpty(starttime))
                            //{
                            //    sql.Append(" and registration_time>='" + starttime + "'");
                            //}
                            //if (!String.IsNullOrEmpty(endtime))
                            //{
                            //    sql.Append(" and registration_time<='" + endtime + "'");
                            //}

                            StringBuilder sbphonelist = new StringBuilder();
                            sbphonelist.Append("(");
                            for (int k = 0; k < employeedetailslist.Count(); k++)
                            {
                                if (!String.IsNullOrEmpty(employeedetailslist[k].calledNumber))
                                {
                                    if (employeedetailslist[k].calledNumber.Length == 12)
                                    {
                                        sbphonelist.Append("'" + employeedetailslist[k].calledNumber.Substring(1, 11) + "'");
                                    }
                                    else if (employeedetailslist[k].calledNumber.Length == 11)
                                    {
                                        sbphonelist.Append("'" + employeedetailslist[k].calledNumber + "'");
                                    }
                                    if (k < employeedetailslist.Count() - 1)
                                    {
                                        //sbphonelist.Append("'',''");
                                        sbphonelist.Append(",");
                                    }
                                }
                            }
                            sbphonelist.Append(")");
                            sql.Append(" and mobile in" + sbphonelist.ToString());

                            dt = DbHelperSQL.GET_DataTable_List(sql.ToString());

                        }

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                ViewDetailsReport u = new ViewDetailsReport();
                                u.registerid = Convert.ToInt32(dt.Rows[j]["registerid"]);
                                u.mobile = dt.Rows[j]["mobile"].ToString();
                                u.username = dt.Rows[j]["username"].ToString();
                                u.realname = dt.Rows[j]["realname"].ToString();
                                u.registration_time = Convert.ToDateTime(dt.Rows[j]["registration_time"]);
                                u.InvestAllMoney = Convert.ToDecimal(dt.Rows[j]["InvestAllMoney"]);
                                u.FoldAllMoney = Convert.ToDecimal(dt.Rows[j]["FoldAllMoney"]);
                                u.RechargeAllMoney = Convert.ToDecimal(dt.Rows[j]["RechargeAllMoney"]);
                                u.IsFirstInvest = dt.Rows[j]["IsFirstInvest"].ToString();
                                u.JanMoney = Convert.ToDecimal(dt.Rows[j]["JanMoney"]);
                                u.MarMoney = Convert.ToDecimal(dt.Rows[j]["MarMoney"]);
                                u.JunMoney = Convert.ToDecimal(dt.Rows[j]["JunMoney"]);
                                lists.Add(u);
                            }
                        }

                        #endregion
                    }

                }

            }
            catch (Exception ex)
            {
                LogInfo.WriteLog(ex.ToString());
            }

            //#region  手写分页

            //int pagecount = 0;

            //string pagenumbers = "";

            ////计算总页数
            //pagecount = (totalCount - 1) / pageSize + 1;
            //if (pageSize <= 0)
            //    pageSize = 1;
            ////生成分页字符串
            //pagenumbers = "<div>共 <strong>" + pagecount + "</strong> 页&nbsp;共 <strong>" + totalCount + "<strong> 条&nbsp;" + Utils.GetPageNumbers(Page, pageSize, "DetailsReport?starttime=" + starttime + "&endtime=" + endtime + "&staffname=" + staffname, 6, "page").ToString();
            //pagenumbers = pagenumbers + " 跳转到第 <input name=\"page1\" id=\"page1\" type=\"text\" style=\"width: 26px\"  /> <span class=\"button white small\" style=\"cursor:pointer;\" onclick=\"page()\">转跳</span>  </div>";

            //ViewBag.PageData = pagenumbers;

            //#endregion

            var list = lists.ToPagedList(Page, pageSize);

            #region  赋值页面标签值
            ViewBag.ViewDetailsReport = lists;
            ViewBag.EmployeeList = GetStaffName();//绑定员工姓名列表
            ViewBag.starttime = starttime;
            ViewBag.endtime = endtime;
            ViewBag.staffname = staffname;
            ViewBag.TotalItemCount = lists.Count();
            ViewBag.TotalPageCount = (lists.Count() - 1) / pageSize + 1;
            #endregion

            return View(list);
        }

        [AdminVaildate(false)]
        public ActionResult DetailsReportToExcel(string starttime = "", string endtime = "", string staffname = "")
        {

            List<ViewDetailsReport> lists = new List<ViewDetailsReport>();

            DataTable dtExcel = new DataTable();

            int totalCount = 0;

            int pagecount = 0;

            #region  定义接口地址、参数

            string url = "http://192.168.0.199/api/callOut/list";

            url = ConfigurationManager.AppSettings["AoRongDetails"].ToString();

            int urlPageSize = 50;

            var values = new NameValueCollection();
            values.Add("account", "admin");
            values.Add("password", "1");
            values.Add("page", "1");
            values.Add("limit", "10");
            if (staffname != "-1" && staffname != "")
            {
                values.Add("empId", staffname);
            }

            if (!String.IsNullOrEmpty(starttime) || !String.IsNullOrEmpty(endtime))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[{");
                sb.Append("'searchFieldName':'dateCreated','searchFieldOperation':'ilike','searchFieldValue':'CUSTOM','dbType':'java.util.Date'");
                if (!String.IsNullOrEmpty(starttime))
                {
                    sb.Append(",'startDateFieldValue':'" + starttime + "'");
                }
                else
                {
                    sb.Append(",'startDateFieldValue':'1970-01-01 00:00:00'");
                }
                if (!String.IsNullOrEmpty(endtime))
                {
                    sb.Append(",'endDateFieldValue':'" + endtime + "'");
                }
                else
                {
                    var datenow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    sb.Append(",'endDateFieldValue':'" + datenow + "'");
                }
                sb.Append("}]");
                values.Add("searchCondition", sb.ToString());
            }
            #endregion

            try
            {
                var result = JsonHelper.JsonToObject<Return_EmployeeDetails>(HttpHelper.Post(url, values));

                totalCount = result.total;

                DataTable dt = new DataTable();

                if (result.success == "True")
                {
                    if (totalCount > urlPageSize)
                    {
                        pagecount = (totalCount - 1) / urlPageSize + 1;

                        values.Remove("limit");
                        values.Add("limit", urlPageSize.ToString());

                        for (int i = 0; i < pagecount; i++)
                        {
                            values.Remove("page");
                            values.Add("page", (i + 1).ToString());

                            var resultInfor = JsonHelper.JsonToObject<Return_EmployeeDetails>(HttpHelper.Post(url, values));

                            if (resultInfor.success == "True")
                            {
                                #region 获得数据
                                var employeedetailslist = resultInfor.data;
                                if (employeedetailslist.Count != 0)
                                {
                                    StringBuilder sql = new StringBuilder();

                                    sql.Append("select * from ViewDetailsReport where 1=1");

                                    StringBuilder sbphonelist = new StringBuilder();
                                    sbphonelist.Append("(");
                                    for (int k = 0; k < employeedetailslist.Count(); k++)
                                    {
                                        if (employeedetailslist[k].calledNumber.Length == 12)
                                        {
                                            sbphonelist.Append("'" + employeedetailslist[k].calledNumber.Substring(1, 11) + "'");
                                        }
                                        else if (employeedetailslist[k].calledNumber.Length == 11)
                                        {
                                            sbphonelist.Append("'" + employeedetailslist[k].calledNumber + "'");
                                        }
                                        if (k < employeedetailslist.Count() - 1)
                                        {
                                            //sbphonelist.Append("'',''");
                                            sbphonelist.Append(",");
                                        }
                                    }
                                    sbphonelist.Append(")");
                                    sql.Append(" and mobile in" + sbphonelist.ToString());

                                    dt = DbHelperSQL.GET_DataTable_List(sql.ToString());

                                }

                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        ViewDetailsReport u = new ViewDetailsReport();
                                        u.registerid = Convert.ToInt32(dt.Rows[j]["registerid"]);
                                        u.mobile = dt.Rows[j]["mobile"].ToString();
                                        u.username = dt.Rows[j]["username"].ToString();
                                        u.realname = dt.Rows[j]["realname"].ToString();
                                        u.registration_time = Convert.ToDateTime(dt.Rows[j]["registration_time"]);
                                        u.InvestAllMoney = Convert.ToDecimal(dt.Rows[j]["InvestAllMoney"]);
                                        u.FoldAllMoney = Convert.ToDecimal(dt.Rows[j]["FoldAllMoney"]);
                                        u.RechargeAllMoney = Convert.ToDecimal(dt.Rows[j]["RechargeAllMoney"]);
                                        u.IsFirstInvest = dt.Rows[j]["IsFirstInvest"].ToString();
                                        u.JanMoney = Convert.ToDecimal(dt.Rows[j]["JanMoney"]);
                                        u.MarMoney = Convert.ToDecimal(dt.Rows[j]["MarMoney"]);
                                        u.JunMoney = Convert.ToDecimal(dt.Rows[j]["JunMoney"]);
                                        lists.Add(u);
                                    }
                                }

                                #endregion
                            }
                        }
                    }
                    else
                    {
                        #region 获得数据
                        var employeedetailslist = result.data;
                        if (employeedetailslist.Count != 0)
                        {
                            StringBuilder sql = new StringBuilder();

                            sql.Append("select * from ViewDetailsReport where 1=1");

                            StringBuilder sbphonelist = new StringBuilder();
                            sbphonelist.Append("(");
                            for (int k = 0; k < employeedetailslist.Count(); k++)
                            {
                                if (employeedetailslist[k].calledNumber.Length == 12)
                                {
                                    sbphonelist.Append("'" + employeedetailslist[k].calledNumber.Substring(1, 11) + "'");
                                }
                                else if (employeedetailslist[k].calledNumber.Length == 11)
                                {
                                    sbphonelist.Append("'" + employeedetailslist[k].calledNumber + "'");
                                }
                                if (k < employeedetailslist.Count() - 1)
                                {
                                    //sbphonelist.Append("'',''");
                                    sbphonelist.Append(",");
                                }
                            }
                            sbphonelist.Append(")");
                            sql.Append(" and mobile in" + sbphonelist.ToString());

                            dt = DbHelperSQL.GET_DataTable_List(sql.ToString());

                        }

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                ViewDetailsReport u = new ViewDetailsReport();
                                u.registerid = Convert.ToInt32(dt.Rows[j]["registerid"]);
                                u.mobile = dt.Rows[j]["mobile"].ToString();
                                u.username = dt.Rows[j]["username"].ToString();
                                u.realname = dt.Rows[j]["realname"].ToString();
                                u.registration_time = Convert.ToDateTime(dt.Rows[j]["registration_time"]);
                                u.InvestAllMoney = Convert.ToDecimal(dt.Rows[j]["InvestAllMoney"]);
                                u.FoldAllMoney = Convert.ToDecimal(dt.Rows[j]["FoldAllMoney"]);
                                u.RechargeAllMoney = Convert.ToDecimal(dt.Rows[j]["RechargeAllMoney"]);
                                u.IsFirstInvest = dt.Rows[j]["IsFirstInvest"].ToString();
                                u.JanMoney = Convert.ToDecimal(dt.Rows[j]["JanMoney"]);
                                u.MarMoney = Convert.ToDecimal(dt.Rows[j]["MarMoney"]);
                                u.JunMoney = Convert.ToDecimal(dt.Rows[j]["JunMoney"]);
                                lists.Add(u);
                            }
                        }

                        #endregion
                    }

                }

            }
            catch (Exception ex)
            {
                LogInfo.WriteLog(ex.ToString());
            }
            dtExcel.Columns.Add("用户Id");
            dtExcel.Columns.Add("客户姓名");
            dtExcel.Columns.Add("登录名");
            dtExcel.Columns.Add("注册时间");
            dtExcel.Columns.Add("投资金额");
            dtExcel.Columns.Add("折标金额");
            dtExcel.Columns.Add("充值金额");
            dtExcel.Columns.Add("首复投");
            dtExcel.Columns.Add("一月期总额");
            dtExcel.Columns.Add("三月期总额");
            dtExcel.Columns.Add("六月期总额");

            foreach (var item in lists)
            {
                DataRow dr = dtExcel.NewRow();
                dr["用户Id"] = item.registerid;
                dr["客户姓名"] = item.realname;
                dr["登录名"] = item.username;
                dr["注册时间"] = item.registration_time;
                dr["投资金额"] = item.InvestAllMoney;
                dr["折标金额"] = item.FoldAllMoney;
                dr["充值金额"] = item.RechargeAllMoney;
                dr["首复投"] = item.IsFirstInvest;
                dr["一月期总额"] = item.JanMoney;
                dr["三月期总额"] = item.MarMoney;
                dr["六月期总额"] = item.JunMoney;

                dtExcel.Rows.Add(dr);
            }

            var path = Extensions.ExportExcel(dtExcel);

            return Content(path);
        }

        /// <summary>
        /// 绑定员工姓名
        /// </summary>
        [AdminVaildate(false)]
        private List<Employee> GetStaffName()
        {
            string url = "http://192.168.0.199/api/employee/listSub";//接口地址

            url = ConfigurationManager.AppSettings["AoRongEmployeeList"].ToString();

            var values = new NameValueCollection();
            values.Add("account", "admin");
            values.Add("password", "1");

            var result = JsonHelper.JsonToObject<Return_Employee>(HttpHelper.Post(url, values));

            List<Employee> employeelist = null;

            if (result.success == "True")
            {
                employeelist = result.data;

            }
            return employeelist;
        }

        #endregion

        #region 客服外呼用户投资排名

        public ActionResult ExternalRanking()
        {
            List<ViewUserCenter> listViewUser = new List<ViewUserCenter>();

            var list = listViewUser.ToPagedList(1, 1);
            return View(list);
        }

        #endregion

    }
}