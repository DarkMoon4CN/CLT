using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ChuangLiTou.Core.Entities.wdzg;
using static ChuanglitouP2P.Common.Settings;
using ChuanglitouP2P.Common;
namespace ChuangLiTouOpenApi.Controllers
{
    /// <summary>
    ///     获取第三方请求token
    /// </summary>


    [ApiExplorerSettings(IgnoreApi = true)]
    public class TokenController : ApiController
    {
        /// <summary>
        ///     获取Token授权码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetOAuthToken(string userName = "", string password = "")
        {
            #region 验证用户名和密码

            var ps = Settings.Instance.GetConfig(Settings.Instance.ThirdUserXmlPath, "User", "UserName", userName,
                "Password");
            var secret = Settings.Instance.GetConfig(Settings.Instance.ThirdUserXmlPath, "User", "UserName",
                "chuanglitou",
                "Password");

            if (string.IsNullOrEmpty(ps))
            {
                var returnObj = await Task.Factory.StartNew(() => new { Result = -1, Data = "用户名或密码错误" });
                return Request.CreateResponse(HttpStatusCode.OK, returnObj);
            }

            if (ps != password)
            {
                var returnObj = await Task.Factory.StartNew(() => new { Result = -1, Data = "用户名或密码错误" });
                return Request.CreateResponse(HttpStatusCode.OK, returnObj);
            }

            #endregion

            #region 数据加密

            var expireTime = DateTime.Now.AddHours(1);
            var str = string.Format("{0},{1},{2},{3}", userName, password, expireTime, secret);
            var token = EncryptHelper.GetMd5Str32(str, "x");

            #endregion

            #region  记录加密串与过期时间

            var alst = new List<KeyValue>
            {
                new KeyValue {Key = "Md5Value", Value = token},
                new KeyValue {Key = "Expire", Value = expireTime.ToString(CultureInfo.InvariantCulture)}
            };

            Settings.Instance.ModifyConfig(Settings.Instance.ThirdUserXmlPath, "/Users/User", "UserName", userName, alst);

            #endregion

            #region 根据不同的平台，返回不同的JSON串

            switch (userName)
            {
                case "clt_wdty": //网贷天眼
                    {
                        var lst = await Task.Factory.StartNew(() => new { result = 1, data = new { token } });
                        return Request.CreateResponse(HttpStatusCode.OK, lst);
                    }
                case "clt_wdzj": //网贷之家
                    {
                        var lst = await Task.Factory.StartNew(() => new { data = new { token } });
                        return Request.CreateResponse(HttpStatusCode.OK, lst);
                    }
                case "clt_wdzg": //网贷中国
                    {
                        var lst = await Task.Factory.StartNew(() => new ResponseWdzgBase { code = -1, data = token });
                        return Request.CreateResponse(HttpStatusCode.OK, lst);
                    }
            }

            //其他用户
            var lstx = await Task.Factory.StartNew(() => new { result = 1, data = new { token } });
            return Request.CreateResponse(HttpStatusCode.OK, lstx);

            #endregion
        }
    }
}