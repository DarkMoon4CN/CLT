using System.Web.Http;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Enc;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTouOpenApi.Factory;
using Newtonsoft.Json.Linq;
using ChuanglitouP2P.Common;
namespace ChuangLiTouOpenApi.Controllers
{
    public class EncController : BaseApi
    {
        /// <summary>
        ///  Dec系统解密--解志辉
        /// </summary>
        /// <param name="reqst">加密串</param>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<string> Decrypt(RequestParam<RequestEnc> reqst)
        {
            var ri = new ResultInfo<string>("99999");
            ri.code = "1";
            ri.message = Settings.Instance.GetErrorMsg(ri.code);
            ri.body = EncryptHelper.Decrypt(reqst.body.EncStr);
            return ri;
        }
    }
}