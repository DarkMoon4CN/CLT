using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.AdNews;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.AdNews;
using ChuangLiTouOpenApi.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.BLL.Api;
namespace ChuangLiTouOpenApi.Controllers
{
    public class ActivityController : BaseApi
    {
        // GET: AdNews
        private readonly AdNewsLogic adnewsLogic;
        /// <summary>
        /// Initializes a new instance of the <see cref="AdNewsController"/> class.
        /// </summary>
        /// <param name="adnewsLogic">The adnews logic.</param>
        public ActivityController(AdNewsLogic adnewsLogic)
        {
            this.adnewsLogic = adnewsLogic;
        }
        /// <summary>
        /// 获取活动专区列表--刘佳
        /// </summary>
        /// <param name="reqst">newType 新闻类型编号，pageindex页码，pagesize一页显示条数</param>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<BasePage<List<AdEntity>>> SelectWebNews(RequestParam<RequestTypeAd> reqst)
        {
            var ri = new ResultInfo<BasePage<List<AdEntity>>>("99999");
            try
            {
                int adtypeId = ConvertHelper.ParseValue(reqst.body.adtypeId.ToString(), 0);
                int pageIndex = ConvertHelper.ParseValue(reqst.body.pageIndex.ToString(), 0);
                int pageSize = ConvertHelper.ParseValue(reqst.body.pageSize.ToString(), 0);

                ri.body = adnewsLogic.SelectWebAd(adtypeId, pageIndex, pageSize);

                if (ri.body == null)
                {
                    ri.code = "1000000010";
                }
                else
                {
                    ri.code = "1";
                }

                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                ri.body.rows = KickOutAppStoreRejectedAd(ri.body.rows, reqst.header.appId.Value.ToString());
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
