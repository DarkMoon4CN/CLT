using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.AppUpdate;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.AppUpdate;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.BLL.Api;
using ChuangLitouP2P.Models;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
namespace ChuangLiTouOpenApi.Areas.Download.Controllers
{
    //http://localhost/download/appupdate/download?code=bd5d859848b94246975c5b18a4896f37
    public class AppUpdateController : Controller
    {
        public ActionResult Download(string code = "")
        {
            HttpResponseMessage result = null;
            if (string.IsNullOrWhiteSpace(code))
                return View();
            using (B_AppUpdatePackage bll = new B_AppUpdatePackage())
            {
                var model = bll.GetDownloadModel(code);
                if (model != null)
                {
                    string filePath = Settings.Instance.GetWebsitePhysicalRootPath + "\\" + model.VirtualPath.Replace("/", "\\");
                    return File(filePath, "application/octet-stream", model.Code + ".apk");
                }
                else
                    return View();
            }
            return View();
        }
    }
}