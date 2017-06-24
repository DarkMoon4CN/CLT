//using ChuangLiTou.Core.BusinessLogic.OldVersion;
using ChuangLiTou.Core.Entities.ChinaPnr;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Model;
using ChuangLiTouOpenApi.Factory;

namespace ChuangLiTouOpenApi.Areas.MemberCenter.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ThirdManageController : BaseController
    {
        // GET: MemberCenter/ThirdManage
        [HttpPost]
        public ActionResult Index(RequestParam<RequestMemberDetail> reqst)
        {
            LoggerHelper.Info(JsonHelper.Entity2Json(reqst));
            UserEntity br = new UserEntity();
            B_member_table b = new B_member_table();
            M_member_table p = new M_member_table();
            p = b.GetModel(reqst.body.userId);

            if (p != null)
            {
                if (p.UsrCustId.Length <= 0)
                {
                    LoggerHelper.Warning("未实名，跳转至实名接口");
                    RequestParam<RequestValidate> vldParam = new RequestParam<RequestValidate>();
                    RequestValidate rv = new RequestValidate();
                    rv.userId = reqst.body.userId.ToString();
                    vldParam.body = rv;
                    return RedirectToAction("RequestRealName", "Index", new { area = "UserAuthentication", userId = reqst.body.userId.ToString() });//未实名，跳转至实名接口
                }



                br.Version = "10";
                br.CmdId = "UserLogin";
                br.MerCustId = Settings.Instance.MerCustId;
                br.UsrCustId = p.UsrCustId;
                return View(br);
            }
            else {
                return Content("异常错误，请联系管理员");
            }
           
        }
    }
}