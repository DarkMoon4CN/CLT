using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Response.Capital
{
    /// <summary>
    /// 提现列表.
    /// </summary>
   public class ResponsePresentEntity
    {
        /// <summary>
        /// 提现金额
        /// </summary>
        /// <value>The present amount.</value>
       public string presentAmount { get; set; }
       /// <summary>
       /// 申请时间
       /// </summary>
       /// <value>The present amount.</value>
       public string appliedOn { get; set; }

       /// <summary>
       /// 提现状态
       /// </summary>
       /// <value>The present amount.</value>
       public string presentStatu { get; set; }
    }
}
