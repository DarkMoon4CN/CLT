using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ChuangLiTou.Core.Entities.Response;
using ChuanglitouP2P.Common;
namespace ChuangLiTouOpenApi.Factory
{
    public class ResponseForbidden
    {
        public int result { get; set; }
        public Tk data { get; set; }
    }

    public class Tk
    {
        public string token { get; set; }
    }


    /// <summary>
    /// </summary>
    public class ThirdApiAuth : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                #region 验证请求参数中是否包含token

                var tk = actionContext.ActionArguments["token"];
                if (tk != null && string.IsNullOrEmpty(tk.ToString()))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK,
                        new ResponseForbidden {result = -1, data = new Tk {token = null}});
                    return;
                }

                #endregion

                #region 验签 获取token相关信息

                var ps = Settings.Instance.GetConfig(Settings.Instance.ThirdUserXmlPath, "User", "Md5Value",
                    tk.ToString(), "Expire");
                if (string.IsNullOrEmpty(ps))
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK,
                        new ResultInfo<string> {code = "500.4", message = Settings.Instance.GetErrorMsg("500.4")});
                    return;
                }
                if (ConvertHelper.ParseValue(ps, DateTime.Now) < DateTime.Now)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK,
                        new ResultInfo<string> {code = "500.5", message = Settings.Instance.GetErrorMsg("500.5")});
                    return;
                }

                #endregion

                base.OnActionExecuting(actionContext);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }
    }
}