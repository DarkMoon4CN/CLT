using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ChuangLiTou.Core.Entities.P2Peye;
using ChuangLiTou.Core.Helpers;
using ChuangLiTouOpenApi.Factory;
using ChuanglitouP2P.BLL.Api;
using ChuanglitouP2P.Common;
namespace ChuangLiTouOpenApi.Controllers
{
    /// <summary>
    ///     网贷天眼接口--解志辉
    /// </summary>
    public class P2PeyeController : BaseThirdApi
    {
        private readonly P2PeyeLogic _logic;

        public P2PeyeController(P2PeyeLogic logic)
        {
            _logic = logic;
        }


        /// <summary>
        ///     借款数据
        /// </summary>
        /// <param name="status">
        ///     标的状态:0.正在投标中的借款标;1.已完成(包括还款中和已完成的借款标).
        ///     状态为1是对应平台满标字段的值检索,状态为0就以平台发标时间字段检索.
        /// </param>
        /// <param name="time_from">始时间如:2014-05-09 06:10:00,</param>
        /// <param name="time_to">截止时间如:2014-05-09 06:10:00,</param>
        /// <param name="page_size">每页记录条数.</param>
        /// <param name="page_index">请求的页码.</param>
        /// <param name="token">请求 token 链接平台返回的秘钥或签名.</param>
        /// <returns></returns>
        [HttpGet]
        public ResponseP2PeyeLoan SelectLoanList(string status = "", string time_from = "", string time_to = "",
            string page_size = "", string page_index = "", string token = "")
        {
            var res = new ResponseP2PeyeLoan();

            try
            {
                var items = _logic.SelectLoanList(status, time_from, time_to, page_size, page_index);
                if (items != null)
                {
                    res.loans = items;
                    res.result_code = "1";
                    res.result_msg = "获取数据成功";
                    res.page_index = items.CurrentPage;
                    res.page_count = items.PageCount;
                }
                else
                {
                    res.result_code = "-1";
                    res.result_msg = "未授权的访问!";
                    res.page_count = 0;
                    res.page_index = 0;
                    res.loans = null;
                }
            }
            catch (Exception ex)
            {
                res.result_code = "-1";
                res.result_msg = "未授权的访问!";
                res.page_count = 0;
                res.page_index = 0;
                res.loans = null;

                LoggerHelper.Error(ex.ToString());
            }
            return res;
        }

        /// <summary>
        ///     获取投资数据
        /// </summary>
        /// <param name="id">标的编号</param>
        /// <param name="page_size">每页记录条数.</param>
        /// <param name="page_index">请求的页码.</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> SelectInvestmentList(string id = "", string page_size = "",
            string page_index = "", string token = "")
        {
            var res = new ResponseP2PeyeInvestment();

            try
            {
                var items = _logic.SelectInvestmentList(id, page_size, page_index);
                if (items != null)
                {
                    res.loans = items.ToList();
                    res.result_code = "1";
                    res.result_msg = "获取数据成功";
                    res.page_index = items.CurrentPage;
                    res.page_count = items.PageCount;
                }
                else
                {
                    res.result_code = "-1";
                    res.result_msg = "未授权的访问!";
                    res.page_count = 0;
                    res.page_index = 0;
                    res.loans = null;
                }
            }
            catch (Exception ex)
            {
                res.result_code = "-1";
                res.result_msg = "未授权的访问!";
                res.page_count = 0;
                res.page_index = 0;
                res.loans = null;

                LoggerHelper.Error(ex.ToString());
            }
            var returnObj = await Task.Factory.StartNew(() => res);
            return Request.CreateResponse(HttpStatusCode.OK, returnObj);
        }

        /// <summary>
        ///     测试Token验证
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> Demo(string token = "")
        {
            var returnObj = await Task.Factory.StartNew(() => "test");
            return Request.CreateResponse(HttpStatusCode.OK, returnObj);
        }
    }
}