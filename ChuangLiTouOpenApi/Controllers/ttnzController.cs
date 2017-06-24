using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ChuangLiTou.Core.Entities.P2Peye;
using ChuangLiTou.Core.Entities.ttnz;
using ChuangLiTou.Core.Helpers;
using ChuangLiTouOpenApi.Factory;
using ChuanglitouP2P.BLL.Api;
using ChuanglitouP2P.Common;
namespace ChuangLiTouOpenApi.Controllers
{
    /// <summary>
    ///     网贷天眼接口--解志辉
    /// </summary>
    public class ttnzController : BaseThirdApi
    {
        private readonly P2PeyeLogic _logic;

        public ttnzController(P2PeyeLogic logic)
        {
            _logic = logic;
        }

        /// <summary>
        ///     借款数据
        /// </summary> 
        /// <param name="token">请求 token 链接平台返回的秘钥或签名.</param>
        /// <returns></returns>
        [HttpGet]
        public List<BorrowingEntity> SelectLoanList(string token = "")
        {
            var res = new  List<BorrowingEntity>();

            try
            {
               res = _logic.SelectLoanList("0");
                return res;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                return null;

            } 
        }         
    }
}