using ChuanglitouP2P.BLL.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChuangLiTouOpenApi.Areas.H5.Controllers
{
    public class NewsController : Controller
    {
        // GET: H5/News
        public ActionResult Index(int id)
        {
            AdNewsLogic adnewsLogic = new AdNewsLogic();
            var ent = adnewsLogic.SelectNewsContent(id);
            return View(ent);
        }
    }
}