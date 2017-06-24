using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ChuangLiTou.Core.Entities;
using ChuangLiTou.Core.Entities.P2Peye;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Bonus;
using ChuangLiTou.Core.Entities.Response.Invest;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuangLiTou.Core.Entities.Response.NormalArea;
using ChuangLiTou.Core.Entities.Response.SmsEmail;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model.Invest;
using ChuanglitouP2P.Common.Util;

namespace ChuanglitouP2P.DAL.Api
{
    /// <summary>
    /// Class RegionDal.
    /// </summary>
    public class RegionDal : ImplBase
    {
        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="areaLevel">The area level.</param>
        /// <returns>List&lt;NormalAreaEntity&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-26 11:07:00
        public List<NormalAreaEntity> SelectAreas(int parentId ,int areaLevel)
        {
            var sql ="SELECT AreaId,AreaName FROM  NormalArea WHERE ParentId=@ParentId AND AreaLevel=@AreaLevel ORDER BY AreaId";
            SqlParameter[] parameters = {
					new SqlParameter("@ParentId", SqlDbType.Int,4),
					new SqlParameter("@AreaLevel", SqlDbType.Int,4)
			};
            parameters[0].Value = parentId;
            parameters[1].Value = areaLevel;

            var ds = DbHelper.Query(sql, parameters);
            if (DataSetIsNotNull(ds))
            {
                return InitNormalAreaEntityList(ds.Tables[0]);
            }
            return null;
        }
    }
}
