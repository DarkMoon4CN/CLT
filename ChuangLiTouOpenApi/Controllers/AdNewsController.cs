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
    /// <summary>
    /// 
    /// </summary>
    public class AdNewsController : BaseApi
    {
        // GET: AdNews
        private readonly AdNewsLogic adnewsLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdNewsController"/> class.
        /// </summary>
        /// <param name="adnewsLogic">The adnews logic.</param>
        public AdNewsController(AdNewsLogic adnewsLogic)
        {
            this.adnewsLogic = adnewsLogic;
        }

        /// <summary>
        /// 获取Banner轮播图数据列表--解志辉
        /// </summary>
        /// <param name="reqst">adtypeId 广告类型编号，top 显示条数</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ResultInfo<List<AdEntity>> SelectWebAd(RequestParam<RequestAd> reqst)
        {
            var ri = new ResultInfo<List<AdEntity>>("99999");
            try
            {
                int top = ConvertHelper.ParseValue(reqst.body.top.ToString(), 0);
                int adtypeId = ConvertHelper.ParseValue(reqst.body.adtypeId.ToString(), 0);


                ri.body = adnewsLogic.GetWebAd(adtypeId, top);

                if (ri.body == null)
                {
                    ri.code = "1000000010";
                }
                else
                {
                    ri.code = "1";
                }

                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                ri.body = KickOutAppStoreRejectedAd(ri.body, reqst.header.appId.Value.ToString());
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


        /// <summary>
        /// 获取新闻列表信息--解志辉
        /// </summary>
        /// <param name="reqst">newType 新闻类型编号，pageindex页码，pagesize一页显示条数</param>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<BasePage<List<NewsEntity>>> SelectWebNews(RequestParam<RequestNews> reqst)
        {

            var ri = new ResultInfo<BasePage<List<NewsEntity>>>("99999");
            try
            {
                int newType = ConvertHelper.ParseValue(reqst.body.newType.ToString(), 0);
                int pageIndex = ConvertHelper.ParseValue(reqst.body.pageIndex.ToString(), 0);
                int pageSize = ConvertHelper.ParseValue(reqst.body.pageSize.ToString(), 0);

                ri.body = adnewsLogic.SelectWebNews(newType, pageIndex, pageSize);

                if (ri.body == null)
                {
                    ri.code = "1000000010";
                }
                else
                {
                    ri.code = "1";
                }

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

        ///// <summary>
        ///// 根据新闻ID获取详细--解志辉
        ///// </summary>
        ///// <param name="reqst">ID 新闻编号</param>
        ///// <returns></returns>
        //[HttpPost]
        //public ResultInfo<NewsEntity> SelectNewsContent(RequestParam<RequestID> reqst)
        //{
        //    var ri = new ResultInfo<NewsEntity>("99999");
        //    try
        //    {
        //        int newId = ConvertHelper.ParseValue(reqst.body.ID.ToString(), 0);

        //        ri.body = adnewsLogic.SelectNewsContent(newId);

        //        if (ri.body == null)
        //        {
        //            ri.code = "1000000010";
        //        }
        //        else
        //        {
        //            ri.code = "1";
        //        }

        //        ri.message = Settings.Instance.GetErrorMsg(ri.code);
        //        return ri;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.Error(ex.ToString());
        //        LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
        //        ri.message = Settings.Instance.GetErrorMsg(ri.code);
        //        return ri;
        //    }
        //}        
    }
}