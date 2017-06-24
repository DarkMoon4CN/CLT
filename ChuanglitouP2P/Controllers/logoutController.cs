using ChuanglitouP2P.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Controllers
{
    public class logoutController : Controller
    {
        // GET: logout
        public ActionResult Index()
        {
            Utils.logout(Utils.GetUserIDCookieslocahost());
            return View();
        }
    }
}