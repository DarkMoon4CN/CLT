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
using ChuangLiTou.Core.Entities.Response.Record;
using ChuangLiTou.Core.Entities.Response.SmsEmail;
using ChuanglitouP2P.Model.Invest;
using ChuanglitouP2P.Common.Util;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.DAL.Api
{
    /// <summary>
    /// Class RecordDal.
    /// </summary>
    public class RecordDal : ImplBase
    {
        /// <summary>
        /// Selects the record identifier.
        /// </summary>
        /// <param name="recordId">The record identifier.</param>
        /// <returns>RecordEntity.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-06-03 17:23:27
        public RecordEntity SelectRecordId(int recordId)
        {
            var sql = string.Format("select * from hx_Bid_records where bid_records_id={0}", recordId);
            var ds = DbHelper.Query(sql);
            if (DataSetIsNotNull(ds))
            {
                return InitRecordEntity(ds.Tables[0]);
            }
            return null;
        }

        public string SelectFreezeOrdId(int recordId)
        {
            try
            {
                var sql = string.Format("SELECT FrozenidNo FROM dbo.hx_td_frozen WHERE bid_records_id={0}", recordId);
                return DbHelper.GetSingle(sql).ToString();
            }
            catch (SqlException e)
            {

                LoggerHelper.Error("SqlException：" + e);
                return null;
            }
        }
    }
}
