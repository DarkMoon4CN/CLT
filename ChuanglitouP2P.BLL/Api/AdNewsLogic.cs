using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.AdNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuanglitouP2P.DAL.Api;

namespace ChuanglitouP2P.BLL.Api
{
    public class AdNewsLogic
    {
        private readonly AdNewsDal _dal = new AdNewsDal();

        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <param name="adtypId">广告类型编号</param>
        /// <param name="top">显示条数</param>
        /// <returns></returns>
        public List<AdEntity> GetWebAd(int adtypId, int top)
        {
            return _dal.GetWebAd(adtypId, top);
        }

        /// <summary>
        /// 获取网站新闻
        /// </summary>
        /// <param name="newType">新闻类型</param>
        /// <param name="pageIndex">页码</param>
        ///  <param name="pageSize">显示条数</param>
        /// <returns></returns>
        public BasePage<List<NewsEntity>> SelectWebNews(int newType, int pageIndex, int pageSize)
        {
            return _dal.SelectWebNews(newType, pageIndex, pageSize);
        }

        /// <summary>
        /// 根据新闻编号获取详细
        /// </summary>
        /// <param name="newId">新闻编号</param>
        /// <returns></returns>
        public NewsEntity SelectNewsContent(int newId)
        {
            return _dal.SelectNewsContent(newId);
        }

        /// <summary>
        /// 获取广告列表(分页)
        /// </summary>
        /// <param name="adtypId">广告类型编号</param>
        /// <param name="pageIndex">页码</param>
        ///  <param name="pageSize">显示条数</param>
        /// <returns></returns>
        public BasePage<List<AdEntity>> SelectWebAd(int adtypId, int pageIndex, int pageSize)
        {
            return _dal.SelectWebAd(adtypId, pageIndex, pageSize);
        }

        public AdEntity GetWebAdModel(int adtypId, string adName)
        {
            return _dal.GetWebAdModel(adtypId, adName);
        }
    }
}
