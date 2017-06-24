using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Model;
using ChuangLitouP2P.Models;

namespace ChuanglitouP2P.Controllers
{
    public class NoticeController : Controller
    {

        chuangtouEntities ef = new chuangtouEntities();
        //
        // GET: /Notice/
        public ActionResult Index(int id)
        {

            var ent = ef.hx_td_about_news.FirstOrDefault(t => t.newid == id);

            return View(ent);
        }


        public PartialViewResult LeftNav()
        {
            var lst = ef.hx_td_web_type.Where(t => t.parentid == 3).OrderBy(t => t.orderid).ToList();

            return PartialView(lst);
        }


        /// <summary>
        /// 网站公告
        /// </summary>
        /// <returns></returns>
        public ActionResult NoticeList(int p = 1, int pageSize =10)
        {
            StringBuilder str = new StringBuilder();
            string TableName = "V_type_news";
            string strFields = "newid,web_Type_menu_id,News_title,News_Key,news_Des,context,createtime,menu_name,path1,topmenuname,listcomm ";
            string fldName = " newid desc";
            string Sort = "desc";
            string strWhere = "web_Type_menu_id=17";

            DataTable dt = new DataTable();

            B_PublicPageList o = new B_PublicPageList();
            var RecordCount = 0;
            dt = o.GetListByPage(TableName, strFields, fldName, pageSize, p, strWhere, out RecordCount);

            ResponsePage rp = new ResponsePage();
            rp.dataBody = dt;
            rp.pageSize = pageSize;
            rp.recordCount = RecordCount;
            rp.pageCount = (RecordCount + pageSize - 1) / pageSize;
            rp.currentCount = p;

            return View(rp);
        }

        /// <summary>
        /// 招贤纳士
        /// </summary>
        /// <returns>ActionResult.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-06-20 16:53:46
        public ActionResult Recruitment()
        {
            return View();

        }


    }
}