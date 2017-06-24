using ChuanglitouP2P.BLL;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeiXin.Controllers
{
    public class zhuanxController : Controller
    {
        // GET: zhuanx
        public ActionResult Index()
        {
            B_td_Ad bad = new B_td_Ad();
            DataSet ds = bad.GetList(8, " AdState=0 and AdTypeId=11 ", " Adid Desc ");
            string str = String.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str += "<div class='common'>"
                                + " <p><i>" + 0 + (Convert.ToInt32(i) + 1) + "</i><a href='"+ ds.Tables[0].Rows[i]["AdLink"].ToString() + "'>"+ ds.Tables[0].Rows[i]["AdName"].ToString() + "</a></p>"
                                + "<a href ='"+ ds.Tables[0].Rows[i]["AdLink"].ToString() + "'><img class='link' src='http://www.chuanglitou.cn" + ds.Tables[0].Rows[i]["AdPath"].ToString() + "' alt='' /></div>";
                }
            }
            ViewBag.Blist = str;
            return View();
        }


    }
}