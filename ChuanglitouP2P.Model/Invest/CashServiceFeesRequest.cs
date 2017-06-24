using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model
{
    public class CashServiceFeesRequest
    {
        /// <summary>
        /// 提现方式.0:普通提现;1:快速提现；2:即时提现.默认值为0
        /// </summary>
        public int WithdrawalType { get; set; }
        /// <summary>
        /// 提取金额
        /// </summary>
        public decimal WithdrawalAmount { get; set; }
    }
}
