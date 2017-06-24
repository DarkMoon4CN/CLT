using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLitouP2P.Models
{
    public class MAmtList
    {

        /// <summary>
        /// 投资金额起
        /// </summary>
        public decimal startAmt { get; set; }


        /// <summary>
        /// 投资金额终
        /// </summary>
        public decimal endAmt { get; set; }

        /// <summary>
        /// 赠送百分比
        /// </summary>
        public decimal percent { get; set; }


        /// <summary>
        /// 赠送抵扣券张数
        /// </summary>
        public int num { get; set; }


        /// <summary>
        /// 赠送抵扣券用,隔开存储数据
        /// </summary>
        public string Amtstr { get; set; }

        /// <summary>
        /// 投资期限（0-不限，1-一个月，3-三个月，6-六个月；单位为天的需转换为月，小于90天（不含）为1-一个月，大于等于90天（含）小于180天（不含）为3-三个月，大于等于180天（含）为6-六个月）
        /// </summary>
        public int LifeLoan { get; set; }

    }
    
}
