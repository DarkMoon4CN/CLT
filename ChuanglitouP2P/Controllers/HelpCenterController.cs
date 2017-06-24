using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Controllers
{
    public class HelpCenterController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();
        // GET: HelpCenter
        public ActionResult Index(string path1, int id)
        {
            var ent = ef.V_type_news.FirstOrDefault(t => t.newid == id);

            List<V_type_news> lastcount = new List<V_type_news>();
            List<V_type_news> nextcount = new List<V_type_news>();

            if (ent != null && ent.web_Type_menu_id != null)
            {
                lastcount = ef.V_type_news.Where(t => t.newid > id && t.web_Type_menu_id == ent.web_Type_menu_id.Value).ToList();
                if (lastcount.Count > 0)
                    lastcount = lastcount.OrderByDescending(t => t.newid).ToList();//看这一篇上一篇的个数

                nextcount = ef.V_type_news.Where(t => t.newid < id && t.web_Type_menu_id == ent.web_Type_menu_id.Value).ToList();
                if (nextcount.Count > 0)
                    nextcount = nextcount.OrderBy(t => t.newid).ToList();//看这一篇下一篇的个数
            }
            else
            {
                ent = new V_type_news();
            }

            if (lastcount.Count() > 0)
            {
                ViewBag.lastpath = "/clt_Detail_" + path1 + "_" + lastcount[lastcount.Count() - 1].newid + ".html";
                ViewBag.lastNewsTitle = lastcount[lastcount.Count() - 1].News_title;
            }
            if (nextcount.Count() > 0)
            {
                ViewBag.nextpath = "/clt_Detail_" + path1 + "_" + nextcount[nextcount.Count() - 1].newid + ".html";
                ViewBag.nextNewsTitle = nextcount[nextcount.Count() - 1].News_title;
            }

            ViewBag.lastCount = lastcount.Count();

            ViewBag.nextcount = nextcount.Count();

            return View(ent);
        }


        public PartialViewResult LeftNav()
        {
            var lst = ef.hx_td_web_type.Where(t => t.parentid == 2).OrderBy(t => t.orderid).ToList();

            //2016-09-20 设置隐藏债权转让
            lst = lst.Where(p => p.menu_id != 14).ToList();
            return PartialView(lst);
        }



        /// <summary>
        /// 网站公告
        /// </summary>
        /// <returns></returns>
        public ActionResult HelpList(string path1 = "Login_register", int p = 1, int pageSize = 10)
        {
            StringBuilder str = new StringBuilder();
            string TableName = "V_type_news";
            string strFields = "newid,web_Type_menu_id,News_title,News_Key,news_Des,context,createtime,menu_name,path1,topmenuname,listcomm ";
            string fldName = " newid desc";
            string Sort = "desc";
            string strWhere = " path1='" + path1 + "'";

            DataTable dt = new DataTable();

            B_PublicPageList o = new B_PublicPageList();
            var RecordCount = 0;
            dt = o.GetListByPage(TableName, strFields, fldName, pageSize, p, strWhere, out RecordCount);

            string srtitle = "注册与登录";
            if (dt.Rows.Count > 0)
            {
                srtitle = dt.Rows[0]["menu_name"].ToString();
            }

            ResponsePage rp = new ResponsePage();
            rp.dataBody = dt;
            rp.pageSize = pageSize;
            rp.recordCount = RecordCount;
            rp.pageCount = (RecordCount + pageSize - 1) / pageSize;
            rp.currentCount = p;

            ViewBag.srtitle = srtitle;
            ViewBag.path1 = path1;
            return View(rp);
        }



    }
}