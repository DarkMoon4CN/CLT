using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuangLiTou.Core.Entities.Response.Bonus;

namespace ChuangLiTou.Core.Entities.Response.Invest
{
    /// <summary>
    /// Class MemberInvestEntity.
    /// </summary>
    public class MemberInvestEntity
    {
        /// <summary>
        /// 会员ID.
        /// </summary>
        /// <value>The member identifier.</value>
        public int userId { get; set; }
        /// <summary>
        /// 账户余额.
        /// </summary>
        /// <value>The member balance.</value>
        public decimal? userBalance { get; set; }
    
    }

    public class ActivityBonusEntity {
        /// <summary>
        /// 账号代金券列表.
        /// </summary>
        /// <value>The bonus.</value>
        public List<BonusEntity> bonus { get; set; }
        /// <summary>
        /// 加息券列表.
        /// </summary>
        /// <value>The bonus.</value>
        public List<RateBonusEntity> addRate { get; set; }

    }

}
