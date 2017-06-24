using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model
{
    public class StatisticsReport
    {
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 员工工号
        /// </summary>
        public string EmployeeNumber { get; set; }

        /// <summary>
        /// 首投人数
        /// </summary>
        public int FirstCount { get; set; }

        /// <summary>
        /// 首投金额
        /// </summary>
        public decimal FirstMoney { get; set; }

        /// <summary>
        /// 复投人数
        /// </summary>
        public int complexCount { get; set; }

        /// <summary>
        /// 复投金额
        /// </summary>
        public decimal complexMoney { get; set; }

        /// <summary>
        /// 投资总金额
        /// </summary>
        public decimal InvestAllMoney { get; set; }

        /// <summary>
        /// 折标总金额
        /// </summary>
        public decimal FoldAllMoney { get; set; }

        /// <summary>
        /// 充值总金额
        /// </summary>
        public decimal RechargeAllMoney { get; set; }

        /// <summary>
        /// 一月期总额
        /// </summary>
        public decimal JanMoney { get; set; }

        /// <summary>
        /// 三月期总额
        /// </summary>
        public decimal MarMoney { get; set; }

        /// <summary>
        /// 六月期总额
        /// </summary>
        public decimal JunMoney { get; set; }


    }
}
