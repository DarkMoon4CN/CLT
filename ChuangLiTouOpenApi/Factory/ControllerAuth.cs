using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

using ChuanglitouP2P.BLL.Api;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Response;
using System.Reflection;
using System.Web.Script.Serialization;
using ChuanglitouP2P.Common;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ChuangLiTouOpenApi.Factory
{
    /// <summary>
    ///     创利投 控制器 属性过滤器 验证请求权限
    ///     验证签名分2个步骤1.base64获取原有body;2.body和实际数据列转换成dataTable进行比对
    /// </summary>
    public class ControllerAuth : ActionFilterAttribute 
    {
        private readonly OpenApiAuthorizationLogic _logic = new OpenApiAuthorizationLogic();


        /// <summary>
        ///  汇付相关的过滤器
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            string bodyStr = string.Empty;
            try
            {
                var tk = actionContext.ActionParameters["reqst"];
                var js = JsonHelper.Entity2Json(tk);
                var reqt = JsonHelper.JsonToObject<RequestParam<object>>(js);
                
                bool tempValidate = true;
                try
                {
                    bodyStr = ChuanglitouP2P.Common.Utils.Base64Decoder(reqt.header.specialStamp);
                }
                catch
                {
                    tempValidate = false;
                    var res = new ResultInfo<string>("5000000085.1");
                    res.message = "签名解析错误";
                    actionContext.HttpContext.Response.Write(res.SerializeJSON());
                    actionContext.HttpContext.Response.End();
                    actionContext.Result = new EmptyResult();
                }
                if (reqt.body !=null && tempValidate==true)
                {
                    var oldTable = JsonConvert.DeserializeObject<DataTable>("["+reqt.body.SerializeJSON()+"]");
                    var newTable = JsonConvert.DeserializeObject<DataTable>("["+bodyStr+ "]");
                    for (int i = 0; i < oldTable.Columns.Count; i++)
                    {
                        //获取oldTable当前列名,当前数,数据类型
                        var dcName = oldTable.Columns[i].ColumnName;
                        Type dcType = oldTable.Columns[i].DataType;
                        
                        var dcValue = MappingTbData(dcType,oldTable.Rows[0][i]);
                        var tempValue = MappingTbData(dcType, newTable.Rows[0][dcName]);
                        
                        if (dcValue != tempValue)//对比失败
                        {
                            tempValidate = false;
                            LoggerHelper.Error("~~~签名对比错误列~~~" + dcValue+":"+ tempValue);
                            LoggerHelper.Error("~~~签名对比JSON~~~" + reqt.body.SerializeJSON() + ":" + bodyStr);
                            var res = new ResultInfo<string>("5000000085.99");
                            res.message = "数据核对错误";
                            actionContext.HttpContext.Response.Write(res.SerializeJSON());
                            actionContext.HttpContext.Response.End();
                            actionContext.Result = new EmptyResult();
                        }
                    }
                    if (tempValidate==true)
                    {
                        var ckr = CheckRequestParams(reqt, bodyStr);
                        if (ckr.code != "200")
                        {
                            actionContext.HttpContext.Response.Write(ckr.SerializeJSON());
                            actionContext.HttpContext.Response.End();
                            actionContext.Result = new EmptyResult();
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
            base.OnActionExecuting(actionContext);
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

            // LoggerHelper.Info(JsonHelper.Entity2Json(res));
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
                    #region 参数验证

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
                      dt =reqt.header.timeStamp.TimeStampToTime();
                    }
                    catch
                    {
                        res = new ResultInfo<string>("5000000085.1");
                        return res;
                    }
                    #endregion

                    #region 验签

                    #region appId appsecret safeCode

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
                        LoggerHelper.Info("请求body:" + reqt.header.accessToken.ToLowerInvariant());
                        LoggerHelper.Info("系统生成body:" + sign.ToLowerInvariant());

                        if (!sign.ToLowerInvariant().Equals(reqt.header.accessToken.ToLower()))
                        {
                            LoggerHelper.Error(sign);
                            LoggerHelper.Error(reqt.header.accessToken);
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

                    #endregion

                    #endregion
                }
            }
            res.message = Settings.Instance.GetErrorMsg(res.code);
            if (res.code != "200")
            {
                LoggerHelper.Error(JsonHelper.Entity2Json(res));
            }
            return res;
        }


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


        /// <summary>
        /// 根据Sytem.Type转换类型
        /// </summary>
        /// <param name="type">Sytem.Type</param>
        /// <param name="data">数据对象</param>
        /// <returns></returns>
        public dynamic MappingTbData(Type type, object data)
        {
            if (type.Equals(typeof(System.String)))
            {
                return data.ToString();
            }
            else if (type.Equals(typeof(System.Int32)))
            {
                return data.ToInt();
            }
            else if (type.Equals(typeof(System.DateTime)))
            {
                return data.ToDateTime();
            }
            else if (type.Equals(typeof(System.Decimal)))
            {
                return data.ToDecimal();
            }
            else if (type.Equals(typeof(System.Byte)))
            {
                return Convert.ToInt16(data);
            }
            else if (type.Equals(typeof(System.Double)))
            {
                return Convert.ToDouble(data);
            }
            else if (type.Equals(typeof(System.Int64)))
            {
                return Convert.ToInt64(data);
            }
            return data.ToString();
        }

    }
}