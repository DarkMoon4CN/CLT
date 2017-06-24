using ChuanglitouP2P.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChuangLiTouOpenApi.Areas.Console.Controllers
{
    public class GlobalConsoleController : Controller
    {
        // GET: Console/GlobalConsole
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ClearCaching()
        {
            CacheRemove.ClearAllCache();
            return Content("缓存清除成功");
        }
    }
}