using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Member;
using ChuangLiTou.Core.Entities.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.BLL.Api;
using ChuanglitouP2P.Common;
using System.Reflection;
using ChuangLiTouOpenApi.Factory;

namespace ChuangLiTouOpenApi.Areas.MemberCenter.Controllers
{
    /// <summary>
    /// 图片上传
    /// </summary>
    public class UploadImageController : BaseController
    {
        /// <summary>
        /// App上传图片
        /// </summary>
        /// <returns>返回上传图片的相对路径</returns>

        // GET: MemberCenter/UploadImage
        [HttpPost]
        public ActionResult Index(RequestParam<RequestMemberDetail> reqst)
        {
            int userId = ConvertHelper.ParseValue(reqst.body.userId.ToString(), 0);
            ResultInfo<string> rModel = new ResultInfo<string>();
            DateTime dt = DateTime.Now;
            string abtPath = Settings.Instance.GetWebsitePhysicalRootPath + "\\Avatar";
            if (!Directory.Exists(abtPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(abtPath);
                directoryInfo.Create();
            }
            string fileName = "";
            string ext = "";
            string filePath = "";
            try
            {
                HttpRequestBase request = HttpContext.Request;//定义传统request对象
                HttpFileCollectionBase imgFiles = request.Files;
                for (int i = 0; i < imgFiles.Count; i++)
                {
                    fileName = string.Format("{0}{1}", System.Guid.NewGuid().ToString(), ".png");
                    filePath = string.Format("/{0}/{1}", "Avatar", fileName);
                    imgFiles[i].SaveAs(abtPath + "\\" + fileName);
                    imgFiles[i].InputStream.Position = 0;
                    if (System.IO.File.Exists(abtPath + "\\" + fileName))
                    {
                        rModel.code = "1";
                        rModel.body = Settings.Instance.ImagesAvater + filePath;
                        rModel.message = "success";
                        #region 根据userid保存更新图片路径 
                        MemberLogic A = new MemberLogic();
                        var model = A.SelectMemberByUserId(userId);
                        var virtualPath = model.userPhotoUrl == "0" ? string.Empty : model.UserPhotoVirtualPath;
                        if (!string.IsNullOrWhiteSpace(virtualPath))
                        {
                            System.IO.File.Delete(Settings.Instance.GetWebsitePhysicalRootPath + virtualPath.Replace("/", "\\"));
                            LoggerHelper.Info("头像上传-删除原有头像图片-成功 UserID：" + userId + "；orginal virtual Path：" + virtualPath);
                        }
                        A.UpdateMemberImg(filePath, userId);
                        #endregion
                        LoggerHelper.Info(JsonHelper.Entity2Json("头像上传 UserID：" + userId + "；fileName：" + fileName + "；filePath：" + filePath + "；返回json：" + rModel));
                    }
                }
            }
            catch (Exception e)
            {
                rModel.code = "500";
                rModel.body = filePath;
                rModel.message = "网络异常";
                LoggerHelper.Error("头像上传  异常信息：" + e.ToString() + "；参数信息 UserID：" + userId + "；fileName：" + fileName + "；filePath：" + filePath);
            }
            return Json(rModel);
        }
    }
}