using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Response.Record
{
    /// <summary>
    ///借款人信息
    /// </summary>
   public class BorrowRecordEntity
    {

        /// <summary>
        /// 借款人客户号
        /// </summary>
        public string BorrowerCustId { set; get; }

        /// <summary>
        /// 借款金额
        /// </summary>
        public string BorrowerAmt { set; get; }

        /// <summary>
        /// 借款手续费率
        /// </summary>
        public string BorrowerRate { set; get; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProId { set; get; }
    }
}
