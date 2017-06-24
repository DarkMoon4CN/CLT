using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Member;
using ChuangLiTou.Core.Entities.Request.NormalArea;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuangLiTou.Core.Entities.Response.NormalArea;
using ChuangLiTou.Core.Entities.Response.SmsEmail;
using ChuanglitouP2P.Common.Util;
using ChuangLiTouOpenApi.Factory;
using ChuanglitouP2P.BLL.Api;
using ChuanglitouP2P.Common;
namespace ChuangLiTouOpenApi.Controllers
{
    /// <summary>
    /// 省市县相关接口
    /// </summary>
    public class RegionController : BaseApi
    {

        private readonly RegionLogic logic;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionController"/> class.
        /// </summary>
        /// <param name="logic">The logic.</param>
        public RegionController(RegionLogic logic)
        {
            this.logic = logic;
        }

        /// <summary>
        /// 获取省级区域--解志辉
        /// </summary>
        /// <returns>ResultInfo&lt;NormalAreaEntity&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-26 10:15:35
        [System.Web.Http.HttpPost]
        public ResultInfo<List<NormalAreaEntity>> SelectProvince(RequestParam reqst)
        {
            var ri = new ResultInfo<List<NormalAreaEntity>>("99999");
            try
            {
                var lst = logic.SelectProvinces();
                if (lst != null)
                {
                    ri.code = "1";
                    ri.body = lst;
                }
                else
                {
                    ri.code = "1000000010";
                }
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }

        /// <summary>
        /// 获取市级区域--解志辉
        /// </summary>
        /// <param name="reqst">The parent identifier.</param>
        /// <returns>ResultInfo&lt;List&lt;NormalAreaEntity&gt;&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-26 11:15:45
        [System.Web.Http.HttpPost]
        public ResultInfo<List<NormalAreaEntity>> SelectCity(RequestParam<RequestAreaEntity> reqst)
        {
            var ri = new ResultInfo<List<NormalAreaEntity>>("99999");
            try
            {
                var lst = logic.SelectCities(reqst.body.parentId);
                if (lst != null)
                {
                    ri.code = "1";
                    ri.body = lst;
                }
                else
                {
                    ri.code = "1000000010";
                }
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }
        /// <summary>
        /// 获取县级区域--解志辉
        /// </summary>
        /// <param name="reqst">The parent identifier.</param>
        /// <returns>ResultInfo&lt;List&lt;NormalAreaEntity&gt;&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-26 11:15:42
        [System.Web.Http.HttpPost]
        public ResultInfo<List<NormalAreaEntity>> SelectCounty(RequestParam<RequestAreaEntity> reqst)
        {
            var ri = new ResultInfo<List<NormalAreaEntity>>("99999");
            try
            {
                var lst = logic.SelectCounty(reqst.body.parentId);
                if (lst != null)
                {
                    ri.code = "1";
                    ri.body = lst;
                }
                else
                {
                    ri.code = "1000000010";
                }

                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }
    }
}