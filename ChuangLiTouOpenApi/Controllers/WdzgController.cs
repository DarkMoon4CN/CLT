using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ChuangLiTou.Core.Entities.wdzg;
using ChuangLiTouOpenApi.Factory;
using ChuanglitouP2P.BLL.Api;
using ChuanglitouP2P.Common;
namespace ChuangLiTouOpenApi.Controllers
{
    /// <summary>
    ///     网贷中国 数据接口
    /// </summary>
    public class WdzgController : BaseThirdApi
    {
        private readonly WdzgLogic _logic;

        public WdzgController(WdzgLogic logic)
        {
            _logic = logic;
        }

        /// <summary>
        ///     通过 【标ID】 或 【日期】 获取指定标信息，包含投资人数据--解志辉
        /// </summary>
        /// <param name="id">标的编号</param>
        /// <param name="date">日期</param>
        /// <param name="token">访问平台密钥</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> SelectTargetByIdOrDate(string id = "", string date = "",
            string token = "")
        {
            var res = new ResponseWdzgBase();

            try
            {
                var items = _logic.SelectTargetByIdOrDate(id, date);
                if (items != null)
                {
                    res.data = items;
                    res.code = 1;
                }
                else
                {
                    res.data = null;
                    res.code = -1;
                }
            }
            catch (Exception ex)
            {
                res.data = null;
                res.code = -1;

                LoggerHelper.Error(ex.ToString());
            }
            var returnObj = await Task.Factory.StartNew(() => res);
            return Request.CreateResponse(HttpStatusCode.OK, returnObj);
        }
    }
}