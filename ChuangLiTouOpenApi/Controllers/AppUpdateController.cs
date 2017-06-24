using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.AppUpdate;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.AppUpdate;
using ChuangLiTouOpenApi.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.BLL.Api;
using ChuangLitouP2P.Models;
namespace ChuangLiTouOpenApi.Controllers
{
    public class AppUpdateController : BaseApi
    {
        /// <summary>
        /// 获取最新更新包下载信息
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public ResultInfo<AppDownloadInfo> SelectPackageInformation(RequestParam<RequestVersion> reqst)
        {
            ResultInfo<AppDownloadInfo> result = new ResultInfo<AppDownloadInfo>("99999");
            using (B_AppUpdatePackage bll = new B_AppUpdatePackage())
            {
                var model = bll.GetLastModel(reqst.body.Platform, reqst.body.Channel, reqst.body.DeviceVersion.Trim());
                if (model != null)
                {
                    //if (model.Version.Trim() == reqst.body.DeviceVersion.Trim())
                    if (!Utils.IsNewAppVersion(reqst.body.DeviceVersion.Trim(), model.Version.Trim()))
                    {
                        result.code = "0";
                        result.message = "你已是最新版本,无需更新!";
                        result.body = null;
                        return result;
                    }
                    result.code = "1";
                    result.body = new AppDownloadInfo();
                    result.body.Channel = model.Channel;
                    result.body.Code = model.Code;
                    result.body.UpdateLevel = model.UpdateLevel;
                    result.body.Description = model.Description;
                    result.body.FileName = model.Code + ".apk";
                    result.body.CreateTime = model.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    result.body.IsEnable = Convert.ToBoolean(model.IsEnable);
                    result.body.ValideCode = model.ValideCode;
                    result.body.Version = model.Version;
                }
            }
            return result;
        }
    }
}