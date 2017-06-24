using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model
{
    /// <summary>
    /// 推广着陆页用到的模型
    /// </summary>
    public class M_borrowing_target_ZhuoLu
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int targetid { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string borrowing_title { get; set; }
        /// <summary>
        /// 年化利率
        /// </summary>
        public decimal annual_interest_rate { get; set; }
        /// <summary>
        /// 借款期限
        /// </summary>
        public int life_of_loan { get; set; }
        /// <summary>
        /// 需借总金额
        /// </summary>
        public decimal borrowing_balance { get; set; }
        /// <summary>
        /// 已借到金额
        /// </summary>
        public decimal fundraising_amount { get; set; }
    }

    public class M_borrowTargetZhuolu
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int targetid { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string project_type_name { get; set; }
        /// <summary>
        /// 年化利率
        /// </summary>
        public decimal annual_interest_rate { get; set; }
        /// <summary>
        /// 借款期限
        /// </summary>
        public int life_of_loan { get; set; }
        /// <summary>
        /// 期限 单位 （1-月，3-天）
        /// </summary>
        public int unit_day { get; set; }
    }
}
