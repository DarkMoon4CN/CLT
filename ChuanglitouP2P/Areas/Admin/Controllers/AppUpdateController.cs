using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using PagedList;
using ChuangLitouP2P.Models;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class AppUpdateController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();
        // GET: Admin/AppUpdate
        public ActionResult Index()
        {
            return View();
        }

        private List<SelectListItem> GetPlatformList()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text="请选择",Value="请选择",Selected=true},
                new SelectListItem() { Text="Android",Value="Android"},
                new SelectListItem() { Text="IOS",Value="IOS"}
            };
        }

        private List<SelectListItem> GetUpdateLevel()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text="请选择",Value="0",Selected=true},
                new SelectListItem() { Text="[非常严重]服务器接口变更",Value="1"},
                new SelectListItem() { Text="[较严重]增加了新业务",Value="2"},
                new SelectListItem() { Text="[严重]现有业务规则有变更",Value="3"},
                new SelectListItem() { Text="[常规]阶段性Bug修复",Value="4"},
                new SelectListItem() { Text="[一般]日常更新",Value="5"}
            };
        }
        private string GetUpdateLevel(int code)
        {
            switch (code)
            {
                case 1: return "[非常严重]服务器接口变更";
                case 2: return "[较严重]增加了新业务";
                case 3: return "[严重]现有业务规则有变更";
                case 4: return "[常规]阶段性Bug修复";
                case 5: return "[一般]日常更新";
                default: return "请选择";
            }
        }

        public ActionResult AppUpdateList(string platform = "", string version = "", DateTime? createTimeFrom = null, DateTime? createTimeTo = null, string channel = "", int Page = 1, int pageSize = 10)
        {
            List<SelectListItem> platformList = GetPlatformList();
            IPagedList<hx_AppUpdatePackage> model = null;
            var query = from data in ef.hx_AppUpdatePackage select data;
            if (!string.IsNullOrWhiteSpace(platform))
            {
                query = query.Where(q => q.Platform == platform.Trim());
                foreach (var item in platformList)
                {
                    if (item.Text == platform.Trim())
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(version))
                query = query.Where(q => q.Version == version.Trim());
            if (createTimeFrom != null && createTimeTo != null)
                query = query.Where(q => q.CreateTime >= createTimeFrom && q.CreateTime < createTimeTo);
            else if (createTimeFrom != null && createTimeTo == null)
                query = query.Where(q => q.CreateTime >= createTimeFrom);
            else if (createTimeFrom == null && createTimeTo != null)
                query = query.Where(q => q.CreateTime < createTimeTo);
            if (!string.IsNullOrWhiteSpace(channel))
                query = query.Where(q => q.Channel == channel.Trim());
            query = query.OrderByDescending(o => o.CreateTime);
            model = query.ToList().ToPagedList(Page, pageSize);

            ViewBag.PlatformList = platformList;
            ViewBag.Version = version;
            ViewBag.CreateTimeFrom = createTimeFrom;
            ViewBag.CreateTimeTo = createTimeTo;
            ViewBag.Channel = channel;
            return View(model);
        }

        public ActionResult AppUpdateAdd(FormCollection Form)
        {
            hx_AppUpdatePackage model = new hx_AppUpdatePackage();
            if (Form.Count < 1)
            {
                ViewBag.UpdateLevelList = GetUpdateLevel();
                ViewBag.PlatformList = GetPlatformList();
                return View(model);
            }

            model.Code = Guid.NewGuid().ToString().Replace("-", "");
            model.Channel = Form["channel"] ?? string.Empty;
            model.CreateTime = DateTime.Now;
            model.DependCode = Form["DependCode"] ?? string.Empty;
            model.Description = Form["Description"] ?? string.Empty;
            var canUse = Form["canUse"] ?? "否";
            if (canUse == "是")
                model.IsEnable = 1;
            else model.IsEnable = 0;
            model.DownloadCount = 0;
            model.Platform = Form["Platform"] ?? string.Empty;
            string updateLevel = Form["UpdateLevel"];
            model.UpdateLevel = Convert.ToInt32(updateLevel);
            model.ValideCode = string.Empty;
            model.Version = Form["version"];
            model.VirtualPath = "";
            if (model.Platform == "IOS")
            {
                model.Channel = "CLT";
                model.Ways = "Apple Store";
                model.ValideCode = "XxxxXxxxXxxxXxxxXxxxXxxxXxxxXxxx";
                model.VirtualPath = "";
            }
            else
            {
                model.Ways = "Http";
                var uploadFile = HttpContext.Request.Files[0];
                string directoryPath = Utils.GetPhysicalPath("Static/AppUpdate");
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                string fileName = directoryPath + "/" + model.Code;
                uploadFile.SaveAs(fileName);
                string md5 = EncryptHelper.GetMD5HashFromFile(fileName);
                model.ValideCode = md5;
                model.VirtualPath = "Static/AppUpdate/" + model.Code;
            }
            ef.hx_AppUpdatePackage.Add(model);
            ef.SaveChanges();
            return Redirect("AppUpdateList");
        }

        public ActionResult AppUpdateAddCancel()
        {
            return Redirect("AppUpdateList");
        }
    }
}