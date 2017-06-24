using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.Recharge
{
    /// <summary>
    /// 充值接口
    /// </summary>
    public class RequestRecharge
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 银行类型
        /// </summary>
        public string bankType { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string bankNo { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal amountOfCharge { get; set; }
    }
}
