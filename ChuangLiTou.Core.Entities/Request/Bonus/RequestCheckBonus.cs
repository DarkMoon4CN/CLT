using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.Bonus
{
    /// <summary>
    /// 验证优惠券是否可用实体
    /// </summary>
   public class RequestCheckBonus 
    {
        /// <summary>
        /// 优惠券id 集合 用逗号隔开 1,2,3,4
        /// </summary>
        /// <value>The bonus identifier.</value>
       public string bonusId { get; set; }

       /// <summary>
       /// 当前用户Id
       /// </summary>
       /// <value>The user identifier.</value>
       public int userId { get; set; }


       /// <summary>
       /// 投资金额
       /// </summary>
       /// <value>The invest amount.</value>
       public decimal investAmount { get; set; }
    }

 

}
