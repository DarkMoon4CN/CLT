using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.Bonus
{
   public class RequestBonus:RequestPage
    {
        /// <summary>
        ///用户id
        /// </summary> 
       public int userId { get; set; }

       /// <summary>
       /// 奖励状态 0 未使用   1已使用  2 已过期 3 锁定中
       /// </summary>
       /// <value>The state of the reward.</value>
       public int rewardState { get; set; }

    }

 

}
