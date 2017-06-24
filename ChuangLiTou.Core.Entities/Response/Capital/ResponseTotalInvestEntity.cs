using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Response.Capital
{
    /// <summary>
    /// Class ResponseTotalInvestEntity.
    /// </summary>
  public  class ResponseTotalInvestEntity
    {
        /// <summary>
        /// 标名称
        /// </summary>
        /// <value>The name of the target.</value>
      public string targetName { get; set; }

      /// <summary>
      /// 投资金额
      /// </summary>
      /// <value>The invest amount.</value>
      public double investAmount { get; set; }

    }
}
