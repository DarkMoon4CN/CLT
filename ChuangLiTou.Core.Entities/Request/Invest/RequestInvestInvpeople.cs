using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.Invest
{
    /// <summary>
    /// Class RequestInvestInvpeople
    /// </summary>
    public class RequestInvestInvpeople
    {
        /// <summary>
        /// 邀请人ID
        /// </summary>
        public int userId { get; set; }
    }

    public class RequestInvestInvpeoplePage : RequestPage
    {
        /// <summary>
        /// 邀请人ID
        /// </summary>
        public int userId { get; set; }
    }
}
