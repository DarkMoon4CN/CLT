using ChuangLitouP2P.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace WeiXin.Controllers
{
    public class InterestRatesController : BaseController
    {
        chuangtouEntities ef = new chuangtouEntities();
        // GET: InterestRates

             
        public ActionResult Index(int pageIndex = 1)
        {
            int userid =CurrentUserId;

            //新加加息券提醒
            var cookie = new HttpCookie("WXRewardTimeJiaXi" + userid);//保存至Cookie
            cookie.Value = DateTime.Now.ToString();
            cookie.Expires = DateTime.Now.Date.AddYears(1);
            Response.Cookies.Add(cookie);

            int pageSize = 5;
            int pageCount = 0;
            int count = ef.hx_UserAct.Where(p => p.registerid == userid && p.RewTypeID == 3 && p.UseState == 0).Count();
            if (count > 0)
            {
                if (count % pageSize > 0)
                {
                    pageCount = count / pageSize + 1;
                }
                else
                {
                    pageCount = count / pageSize;
                }
            }
            pageIndex = pageIndex <= 1 ? 1 : pageIndex;
            pageIndex = pageIndex >= pageCount ? pageCount : pageIndex;
            var list = ef.hx_UserAct.Where(p => p.registerid == userid && p.RewTypeID == 3 && p.UseState == 0).OrderByDescending(a => a.Createtime).ToPagedList(pageIndex, pageSize);
            ViewBag.pageCount = pageCount;
            ViewBag.pageIndex = pageIndex;
            return View(list);
            
        }

         
        public  ActionResult activityUsed(int pageIndex = 1)
        {
            int userid =CurrentUserId;
            int pageSize = 5;
            int pageCount = 0;
            int count = ef.hx_UserAct.Where(p => p.registerid == userid && p.RewTypeID == 3 && p.UseState == 1).Count();
            if (count > 0)
            {
                if (count % pageSize > 0)
                {
                    pageCount = count / pageSize + 1;
                }
                else
                {
                    pageCount = count / pageSize;
                }
            }
            pageIndex = pageIndex <= 1 ? 1 : pageIndex;
            pageIndex = pageIndex >= pageCount ? pageCount : pageIndex;
            var list = ef.hx_UserAct.Where(p => p.registerid == userid && p.RewTypeID == 3 && p.UseState == 1).OrderByDescending(a => a.ActID).ToPagedList(pageIndex, pageSize);
            ViewBag.pageCount = pageCount;
            ViewBag.pageIndex = pageIndex;
            return View(list);

           
        }
         
        public ActionResult activityExpired(int pageIndex = 1)
        {
            int userid =CurrentUserId;
            int pageSize = 5;
            int pageCount = 0;
            int count = ef.hx_UserAct.Where(p => p.registerid == userid && p.RewTypeID == 3 && p.UseState == 2).Count();
            if (count > 0)
            {
                if (count % pageSize > 0)
                {
                    pageCount = count / pageSize + 1;
                }
                else
                {
                    pageCount = count / pageSize;
                }
            }
            pageIndex = pageIndex <= 1 ? 1 : pageIndex;
            pageIndex = pageIndex >= pageCount ? pageCount : pageIndex;
            var list = ef.hx_UserAct.Where(p => p.registerid == userid && p.RewTypeID == 3 && p.UseState == 2).OrderByDescending(a => a.ActID).ToPagedList(pageIndex, pageSize);
            ViewBag.pageCount = pageCount;
            ViewBag.pageIndex = pageIndex;
            return View(list);
        }


    }
}