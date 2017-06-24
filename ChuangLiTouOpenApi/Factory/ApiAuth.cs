using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.Filters;
using ChuanglitouP2P.BLL.Api;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Response;
using System.Reflection;
using System.Web.Script.Serialization;
using ChuanglitouP2P.Common;
using System.Data;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

namespace ChuangLiTouOpenApi.Factory
{
    /// <summary>
    ///     创利投 API 属性过滤器 验证请求权限
    /// </summary>
    public class ApiAuth : ActionFilterAttribute
    {
        private readonly OpenApiAuthorizationLogic _logic = new OpenApiAuthorizationLogic();

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                HttpContextBase context = (HttpContextBase)actionContext.Request.Properties["MS_HttpContext"];
                var request = context.Request;
                var tk = actionContext.ActionArguments["reqst"];
                var js = JsonHelper.Entity2Json(tk);
                var reqt = JsonHelper.JsonToObject<RequestParam<object>>(js);
                var bodyStr = DNTRequest.InputStream(request.InputStream);

                if (reqt.body != null)
                {
                    bodyStr = GetBody(bodyStr);
                    var ckr = CheckRequestParams(reqt, bodyStr);
                    if (ckr.code != "200")
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, ckr);
                        return;
                    }
                }
                base.OnActionExecuting(actionContext);
            }
            catch (Exception ex)
            {
                LoggerHelper.Info(ex.ToString());
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }

        /// <summary>
        ///     验证公共参数是否合法
        /// </summary>
        /// <param name="reqt">参数.</param>
        /// <param name="bodyStr">bodyjson字符串.</param>
        /// <returns></returns>
        private ResultInfo<string> CheckRequestParams(RequestParam<object> reqt, string bodyStr)
        {
            var res = new ResultInfo<string>("200");
            if (reqt == null)
            {
                res = new ResultInfo<string>("5000000086");
            }
            else
            {
                if (reqt.header == null)
                {
                    res = new ResultInfo<string>("5000000080");
                }
                if (reqt.header != null)
                {
                    if (reqt.header.appId <= 1)
                    {
                        res = new ResultInfo<string>("5000000081");
                    }
                    if (string.IsNullOrEmpty(reqt.header.appSecret))
                    {
                        res = new ResultInfo<string>("5000000082");
                    }
                    if (string.IsNullOrEmpty(reqt.header.accessToken))
                    {
                        res = new ResultInfo<string>("5000000083");
                    }
                    if (string.IsNullOrEmpty(reqt.header.timeStamp))
                    {
                        res = new ResultInfo<string>("5000000085");
                    }
                    DateTime dt;
                    try
                    {
                        dt = reqt.header.timeStamp.TimeStampToTime();
                    }
                    catch
                    {
                        //res = new ResultInfo<string>("5000000085.1");
                        //return res;
                    }
                    var ent = _logic.SelectAppAuthorInforById(ConvertHelper.ParseValue(reqt.header.appId, 0));
                    if (ent == null)
                    {
                        res = new ResultInfo<string>("5000000088");
                    }
                    else
                    {
                        if (!ent.appSecret.Equals(reqt.header.appSecret))
                        {
                            res = new ResultInfo<string>("5000000088");
                        }
                        var dic = new Dictionary<string, string>
                        {
                            {"appId", reqt.header.appId.ToString()},
                            {"appSecret", reqt.header.appSecret},
                            {"timeStamp", reqt.header.timeStamp}
                        };
                        var sign = HttpHelper.GetAccessToken(dic, bodyStr, ent.appSafeCode);
                        if (!sign.ToLowerInvariant().Equals(reqt.header.accessToken.ToLower()))
                        {
                            res = new ResultInfo<string>("5000000088.1");
                        }

                        #region 验证ip

                        //var reqIp = Settings.Instance.ClientIp;
                        //if (ent.AppServerIps.IndexOf(reqIp, StringComparison.Ordinal) == -1)
                        //{
                        //    res = new ResponseObject("5000000088.2");
                        //    return res;
                        //}

                        #endregion
                    }
                }
            }
            res.message = Settings.Instance.GetErrorMsg(res.code);
            if (res.code != "200")
            {
                LoggerHelper.Error(JsonHelper.Entity2Json(res));
            }
            return res;
        }

        /// <summary>
        ///  获取body
        /// </summary>
        /// <param name="bodyStr"></param>
        /// <returns></returns>
        private string GetBody(string bodyStr)
        {
            int index = bodyStr.IndexOf("body\":");
            if (index > 0)
            {
                string[] sArray = Regex.Split(bodyStr, "body\":", RegexOptions.IgnoreCase);
                if (index < 100) //body放在第一个元素
                {
                    sArray = Regex.Split(bodyStr, "header\":", RegexOptions.IgnoreCase);
                    bodyStr = sArray[0];
                    sArray = Regex.Split(bodyStr, "body\":", RegexOptions.IgnoreCase);
                    bodyStr = sArray[1];
                    bodyStr = bodyStr.Substring(0, bodyStr.Length - 2);
                }
                else
                {
                    bodyStr = sArray[1];
                    bodyStr = bodyStr.Substring(0, bodyStr.Length - 1);
                }
            }
            return bodyStr;
        }
    }
}