using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.Cash
{
    public class RequestCash
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 用户绑定的卡Id
        /// </summary>
        public int usrBindCardId { get; set; }

        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal transAmt { get; set; }

        /// <summary>
        /// 提现方式.0:普通提现;1:快速提现；2:即时提现.默认值为0
        /// </summary>
        public int withdrawalType { get; set; }
    }
}
