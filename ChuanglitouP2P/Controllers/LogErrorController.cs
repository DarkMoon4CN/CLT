using ChuanglitouP2P.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Controllers
{
    public class LogErrorController : Controller
    {
        // GET: LogError
        public ActionResult Index(string errorDate = "")
        {
            string ip = Utils.GetRealIP();
            if (ip != "58.132.211.147")
            {
                return Content("抱歉，您没有访问权限");
            }
            string fileName = "";
            if (string.IsNullOrWhiteSpace(errorDate))
            {
                errorDate = DateTime.Now.ToString("yyyyMMdd");
            }
            fileName = Server.MapPath("/Log/error." + errorDate + ".htm");
            if (!System.IO.File.Exists(fileName))
            {
                return Content("日志不存在");
            }
            return File(fileName, "text/html");
            //return View();
        }
    }
}