using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Bonus;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuangLiTou.Core.Entities.Response.NormalArea;
using ChuangLiTou.Core.Entities.Response.Record;
using ChuangLiTou.Core.Entities.Response.SmsEmail;

using ChuanglitouP2P.DAL.Api;
namespace ChuanglitouP2P.BLL.Api
{
    public class RecordLogic
    {
        private readonly RecordDal _dal = new RecordDal();


        /// <summary>
        /// Selects the record identifier.
        /// </summary>
        /// <param name="recordId">The record identifier.</param>
        /// <returns>RecordEntity.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-06-03 17:23:16
        public RecordEntity SelectRecordId(int recordId)
        {
            return _dal.SelectRecordId(recordId);
        }

        /// <summary>
        /// 获取冻结号
        /// </summary>
        /// <param name="recordId">The record identifier.</param>
        /// <returns>System.String.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-06-07 16:07:18
        public string SelectFreezeOrdId(int recordId)
        {
            return _dal.SelectFreezeOrdId(recordId);

        }
    }
}
