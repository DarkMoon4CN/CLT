using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
namespace ChuanglitouP2P.Controllers
{
    public class RewardController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();



        /// <summary>
        /// 现金奖励
        /// </summary>
        /// <returns></returns>
        // GET: Reward
        public ActionResult Index(int? pageIndex, int pgaesize = 6)
        {
            int userid = Utils.checkloginsession();
            //新加抵扣券提醒
            var cookie = new HttpCookie("RewardTimeXianJin"+ userid);//保存至Cookie
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.Date.AddYears(1);
            Response.Cookies.Add(cookie);

            int Totals = 0;
            decimal succTotal = 0.00M;
            var ListByOwner = ef.hx_UserAct.Where(l => l.registerid == userid && l.RewTypeID == 1).GroupBy(l => l.registerid)
                .Select(lg => new
                {
                    Owner = lg.Key,
                    Counts = lg.Count(),
                    succTotal = lg.Sum(w => w.Amt)
                });
            foreach (var itc in ListByOwner)
            {
                if (itc.Counts > 0)
                {
                    Totals = (int)itc.Counts;
                    decimal.TryParse(itc.succTotal.ToString(), out succTotal);
                }
            }
            Expression<Func<hx_UserAct, bool>> where = PredicateExtensionses.True<hx_UserAct>();
            where = where.And(p => p.UserAct > 0);
            where = where.And(p => p.registerid == userid);
            where = where.And(p => p.RewTypeID == 1);

            var list = ef.hx_UserAct.Where(where).OrderByDescending(p => p.UserAct).ToPagedList(pageIndex ?? 1, pgaesize);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_RewardList1", list);
            }
            ViewBag.Totals = Totals;
            ViewBag.succTotal = succTotal;

            return View(list);
        }


        /// <summary>
        /// 抵扣券
        /// </summary>
        /// <returns></returns>
        public ActionResult xianjin(int? pageIndex, int? pageIndex1, int? pageIndex2, int pgaesize = 6)
        {
            int userid = Utils.checkloginsession();

            //新加抵扣券提醒
            var cookie = new HttpCookie("RewardTimeXianJinQuan"+ userid);//保存至Cookie
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.Date.AddYears(1);
            Response.Cookies.Add(cookie);



            xianjinModelList mode = new xianjinModelList();

            Expression<Func<hx_UserAct, bool>> where = PredicateExtensionses.True<hx_UserAct>();
            where = where.And(p => p.UserAct > 0);
            where = where.And(p => p.registerid == userid);
            where = where.And(p => p.RewTypeID == 2);
            where = where.And(p => p.UseState == 0);


            Expression<Func<hx_UserAct, bool>> where1 = PredicateExtensionses.True<hx_UserAct>();
            where1 = where1.And(p => p.UserAct > 0);
            where1 = where1.And(p => p.registerid == userid);
            where1 = where1.And(p => p.RewTypeID == 2);
            where1 = where1.And(p => p.UseState == 1);

            //过期的加息券
            Expression<Func<hx_UserAct, bool>> where2 = PredicateExtensionses.True<hx_UserAct>();
            where2 = where2.And(p => p.UserAct > 0);
            where2 = where2.And(p => p.registerid == userid);
            where2 = where2.And(p => p.RewTypeID == 2);
            where2 = where2.And(p => p.UseState == 2);


            mode.xianjilist = ef.hx_UserAct.Where(where).OrderByDescending(p => p.UserAct).ToPagedList(pageIndex ?? 1, pgaesize);
            mode.xianjiuses = ef.hx_UserAct.Where(where1).OrderByDescending(p => p.UserAct).ToPagedList(pageIndex1 ?? 1, pgaesize);
            mode.xianjilost = ef.hx_UserAct.Where(where2).OrderByDescending(p => p.UserAct).ToPagedList(pageIndex2 ?? 1, pgaesize);



            if (Request.IsAjaxRequest())
            {
                var target = Request.QueryString["target"];
                if (target == "dTable")  //充值列表
                {
                    return PartialView("_xianjinList", mode.xianjilist);
                }
                else if (target == "xianUse")  //资金流水
                {
                    return PartialView("_xianUse", mode.xianjiuses);
                }
                else if (target == "xianjinlost")  //资金流水
                {
                    return PartialView("_xianjilost", mode.xianjilost);
                }
            }

            /*
             List<hx_UserAct> IsUse = ef.hx_UserAct.Where(c => c.UseState == 1 && c.RewTypeID == 2 && c.registerid == userid).OrderByDescending(p => p.UserAct).ToList();
             List<hx_UserAct> overdue = ef.hx_UserAct.Where(c => c.UseState == 2 && c.RewTypeID == 2 && c.registerid == userid).OrderByDescending(p => p.UserAct).ToList();
             ViewBag.IsUse = IsUse;
             ViewBag.overdue = overdue;
             */

            return View(mode);

        }



        /// <summary>
        /// 加息券
        /// </summary>
        /// <returns></returns>
        public ActionResult jiaxi(int? pageIndex, int? pageIndex1, int? pageIndex2, int pgaesize = 6)
        {
            int userid = Utils.checkloginsession();
            //新加加息券提醒
            var cookie = new HttpCookie("RewardTimeJiaXi"+ userid);//保存至Cookie
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.Date.AddYears(1);
            Response.Cookies.Add(cookie);

            JiaxiModelsLsit model = new JiaxiModelsLsit();
            //正常加息券
            Expression<Func<hx_UserAct, bool>> where = PredicateExtensionses.True<hx_UserAct>();
            where = where.And(p => p.UserAct > 0);
            where = where.And(p => p.registerid == userid);
            where = where.And(p => p.RewTypeID == 3);
            where = where.And(p => p.UseState == 0);
            //已经加息券

            Expression<Func<hx_UserAct, bool>> where1 = PredicateExtensionses.True<hx_UserAct>();
            where1 = where1.And(p => p.UserAct > 0);
            where1 = where1.And(p => p.registerid == userid);
            where1 = where1.And(p => p.RewTypeID == 3);
            where1 = where1.And(p => p.UseState == 1);

            //过期的加息券
            Expression<Func<hx_UserAct, bool>> where2 = PredicateExtensionses.True<hx_UserAct>();
            where2 = where2.And(p => p.UserAct > 0);
            where2 = where2.And(p => p.registerid == userid);
            where2 = where2.And(p => p.RewTypeID == 3);
            where2 = where2.And(p => p.UseState == 2);

            model.Jiaxi = ef.hx_UserAct.Where(where).OrderByDescending(p => p.UserAct).ToPagedList(pageIndex ?? 1, pgaesize);

            model.JiaxiUses = ef.hx_UserAct.Where(where1).OrderByDescending(p => p.UserAct).ToPagedList(pageIndex1 ?? 1, pgaesize);

            model.JiaxiLost = ef.hx_UserAct.Where(where2).OrderByDescending(p => p.UserAct).ToPagedList(pageIndex2 ?? 1, pgaesize);

            if (Request.IsAjaxRequest())
            {
                var target = Request.QueryString["target"];
                if (target == "dTable")  //充值列表
                {
                    return PartialView("_JiaXiList", model.Jiaxi);
                }
                else if (target == "JiaxiUses")  //资金流水
                {
                    return PartialView("_JiaxiUses", model.JiaxiUses);
                }
                else if (target == "JiaxiLost")  //资金流水
                {
                    return PartialView("_JiaxiLost", model.JiaxiLost);
                }




            }

            /*
            List<hx_UserAct> IsUse = ef.hx_UserAct.Where(c => c.UseState == 1 && c.RewTypeID == 3 && c.registerid == userid).OrderByDescending(p => p.UserAct).ToList();
            List<hx_UserAct> overdue = ef.hx_UserAct.Where(c => c.UseState == 2 && c.RewTypeID == 3 && c.registerid == userid).OrderByDescending(p => p.UserAct).ToList();
            */


            // ViewBag.IsUse = IsUse;
            // ViewBag.overdue = overdue;


            return View(model);
        }

    }
}