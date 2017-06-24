using System;
using System.Web.Http;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Member;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTouOpenApi.Factory;
using ChuanglitouP2P.Common;
namespace ChuangLiTouOpenApi.Controllers
{
    /// <summary>
    /// 测试接口专用方法
    /// </summary>
    public class UserController : BaseApi
    {

        /// <summary>
        /// 测试地址--解志辉
        /// </summary>
        /// <returns>ResultInfo&lt;System.String&gt;.</returns>
        [HttpPost]
        public ResultInfo<string> SelectMessages(RequestParam<RequestMember> reqst)
        {
            var ri = new ResultInfo<string>("99999");
            try
            {
                ri.code = "1";
                ri.body = reqst.ToString();
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                 
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }

   

    }
}