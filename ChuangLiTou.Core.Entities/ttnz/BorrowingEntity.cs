using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.ttnz
{
    /// <summary>
    /// Class BorrowingEntity.
    /// </summary>
    public class BorrowingEntity
    {
        
        /// <summary>
        /// 还款方式
        /// </summary>
        /// <value>The HKFS.</value>
        public string hkfs { get; set; }
        /// <summary>
        /// 贷款期限
        /// </summary>
        /// <value>The DKQX.</value>
        public string dkqx { get; set; }
        /// <summary>
        /// 标名称
        /// </summary>
        /// <value>The bname.</value>
        public string bname { get; set; }
        /// <summary>
        /// 投资进度.
        /// </summary>
        /// <value>The TZJD.</value>
        public string tzjd { get; set; }
        /// <summary>
        /// 起投金额
        /// </summary>
        /// <value>The qtje.</value>
        public string qtje { get; set; }
        /// <summary>
        /// 可投金额
        /// </summary>
        /// <value>The ktje.</value>
        public string ktje { get; set; }
        /// <summary>
        /// 标的投资总金额
        /// </summary>
        /// <value>The jkje.</value>
        public string jkje { get; set; }
        /// <summary>
        /// 渠道给平台的唯一key.
        /// </summary>
        /// <value>The key.</value>
        public string key { get; set; }
        /// <summary>
        /// 年化收益
        /// </summary>
        /// <value>The nhsy.</value>
        public string nhsy { get; set; }


    }
}
