using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLitouP2P.Models
{
    public class Mcoupon
    {
        /// <summary>
        /// 规则 1 统一赠送   2 按投资赠送  3 随机赠送 
        /// </summary>
        public int rule { get; set; }

        /// <summary>
        /// 抵扣券
        /// </summary>
        public decimal cash { get; set; }

        /// <summary>
        /// 是否拆分 1拆分 2不拆分
        /// </summary>
        public int ISsplit { get; set; }

        /// <summary>
        /// 使用数量要求 (1 仅单独使用  2 可组合使用)
        /// </summary>
        public int Uses { get; set; }

        /// <summary>
        /// 折分字串
        /// </summary>
        public List<Msplitarr> Msplitarr { get; set; }



        /// <summary>
        /// 赠送金额阶段列表
        /// </summary>
        public List<MAmtList> MAmtList { get; set; }


    }
}
