using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuangLitouP2P.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class AdController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/Ad
        [AdminVaildate()]
        public ActionResult Index(string adname = "", int AdTypeState=-1,int ddlType=8, int adstate = 0, int Page = 1, int pageSize = 10)
        {

            int pageNumber = Page / 1;
            Expression<Func<V_AD_type, bool>> where = PredicateExtensionses.True<V_AD_type>();
            where = where.And(p => p.Adid > 0);



            if (!string.IsNullOrEmpty(adname))
            {
                where = where.And(p => p.AdName.Contains(adname));
            }
            
            if (adstate == 1)
            {
                where = where.And(p => p.AdState == 0);
            }
            else if (adstate == 2)
            {
                where = where.And(p => p.AdState == 1);
            }
            if (AdTypeState!=-1)
            {
                where = where.And(p => p.AdTypeId == AdTypeState);
            }
            IPagedList<V_AD_type> list = ef.V_AD_type.Where(where).OrderByDescending(p => p.Adid).ToPagedList(pageNumber, pageSize);

            ViewBag.adname = adname;
            ViewBag.adstate = adstate;
            ViewBag.AdTypeState = AdTypeState;
            ViewBag.ddlType = ddlType;
            return View(list);
        }

        [AdminVaildate(false,true)]
        public ActionResult AddAd()
        {
            var list_Adtype = (from a in ef.hx_td_web_Ad_type select a).ToList<hx_td_web_Ad_type>();
            ViewBag.Adtypes = list_Adtype;

            return View();
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate()]
        public ActionResult AddAdpost(hx_td_Ad p)
        {

            p = (hx_td_Ad)Utils.ValidateModelClass(p);
            p.AdPath = p.AdPath.Replace("//", "/");
            p.Adcreatetime = DateTime.Now;
            ef.hx_td_Ad.Add(p);
            ef.SaveChanges();

            CacheRemove.RemoveWebAdtype(); //请除广告位缓存
            return RedirectToAction("index", "Ad");
        }

        [HttpGet]
        [AdminVaildate()]
        public ActionResult EditAd(int id)
        {
            if (id < 1)
            {
                return Content(StringAlert.Alert("参数错误", "/admin/Ad/Index"), "text/html");
            }
            var model = ef.hx_td_Ad.Where(p => p.Adid == id).SingleOrDefault();
            if (model == null || model.Adid < 1)
            {
                return Content(StringAlert.Alert("记录不存在", "/admin/Ad/Index"), "text/html");
            }
            //IEnumerable<SelectListItem> ddlList = Utils.GetEnumToList(typeof(EnumOrdIdState)).Select(c => new SelectListItem { Value = c.key.ToString(), Text = c.value.ToString() });
            //ViewBag.ddlList = ddlList;
            var list_Adtype = (from a in ef.hx_td_web_Ad_type select a).ToList<hx_td_web_Ad_type>();
            ViewBag.Adtypes = list_Adtype;
            ViewBag.AdTypeId = model.AdTypeId ;
            return View(model);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult EditAdpost(hx_td_Ad p)
        {
            string str = "";
            
            string[] proNames;

            proNames = new string[] { "AdName", "AdTypeId", "AdPath", "AdLink", "AdState" };
            p = (hx_td_Ad)Utils.ValidateModelClass(p);

            p.AdPath = p.AdPath.Replace("//", "/");
            DbEntityEntry entry = ef.Entry<hx_td_Ad>(p);
            entry.State = EntityState.Unchanged;

            foreach (string ProName in proNames)
            {
                entry.Property(ProName).IsModified = true;
            }
            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("广告修改成功!", "/admin/Ad/Index");
            }
            else
            {
                str = StringAlert.Alert("广告修改失败!", "/admin/Ad/EditAd/" + p.Adid + "/");
            }



            CacheRemove.RemoveWebAdtype(); //请除广告位缓存


            return Content(str, "text/html");
        }



        [AdminVaildate()]
        public ActionResult DelById(int id, int Page = 1)
        {
            string str = "";

            string adname = DNTRequest.GetString("adname");
            hx_td_Ad pDel = new hx_td_Ad() { Adid = id };
            ef.hx_td_Ad.Attach(pDel);
            ef.hx_td_Ad.Remove(pDel);
            int i = ef.SaveChanges();
            if (i > 0)
            {

                str = StringAlert.Alert("广告删除成功!", "/admin/Ad/Index?page=" + Page.ToString() + "&adname=" + adname);

                CacheRemove.RemoveWebAdtype(); //请除广告位缓存
            }
            else
            {
                str = StringAlert.Alert("广告删除失败!", "/admin/Ad/Index?page=" + Page.ToString() + "&adname=" + adname);
            }

            return Content(str, "text/html");

        }
    }

   
}