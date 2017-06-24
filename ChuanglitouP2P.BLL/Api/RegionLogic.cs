using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Bonus;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuangLiTou.Core.Entities.Response.NormalArea;
using ChuangLiTou.Core.Entities.Response.SmsEmail;

using ChuanglitouP2P.DAL.Api;
namespace ChuanglitouP2P.BLL.Api
{
    public class RegionLogic
    {
        private readonly RegionDal _dal = new RegionDal();


        /// <summary>
        /// 省
        /// </summary>
        /// <returns>List&lt;NormalAreaEntity&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-26 11:12:21
        public List<NormalAreaEntity> SelectProvinces()
        {
            return _dal.SelectAreas(1, 1);
        }
        /// <summary>
        /// 市
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>List&lt;NormalAreaEntity&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-26 11:12:20
        public List<NormalAreaEntity> SelectCities(int parentId)
        {
            return _dal.SelectAreas(parentId, 2);
        }
        /// <summary>
        /// 县
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>List&lt;NormalAreaEntity&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-26 11:12:18
        public List<NormalAreaEntity> SelectCounty(int parentId)
        {
            return _dal.SelectAreas(parentId, 3);
        }
    }
}
