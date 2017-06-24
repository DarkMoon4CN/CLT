using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLitouP2P.Models
{
    public  class Msplitarr
    {
        /// <summary>
        /// 抵扣券金额
        /// </summary>
        public decimal cashAmt { get; set; }

        /// <summary>
        /// 开始金额
        /// </summary>
        public decimal startAmt { get; set; }

        /// <summary>
        /// 结束金额
        /// </summary>
        public decimal endAmt { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime endTime { get; set; }
        /// <summary>
        /// 使用借款期限（0-不限，1-一个月，3-三个月，6-六个月；单位为天的需转换为月，小于90天（不含）为1-一个月，大于等于90天（含）小于180天（不含）为3-三个月，大于等于180天（含）为6-六个月）
        /// </summary>
        public string UseLifeLoan { get; set; }

        /// <summary>
        /// 有效期。获取券时间加上相应天数（天）
        /// </summary>
        public int validity { get; set; }
        
    }
}
