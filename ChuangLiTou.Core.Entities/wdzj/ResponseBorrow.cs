using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.wdzj
{
    public class ResponseBorrow
    {
        /// <summary>
        /// 总页数(每页显示20条借款标信息)
        /// </summary>
        public string totalPage { get; set; }
        /// <summary>
        /// 当前页数（从1开始）
        /// </summary>
        public string currentPage { get; set; }
        /// <summary>
        /// 总标数
        /// </summary>
        public int totalCount { get; set; }
        /// <summary>
        /// 当天借款标总额
        /// </summary>
        public double totalAmount { get; set; }
        /// <summary>
        /// 借款标信息（参照下面）
        /// </summary>
        public List<BorrowEntity> borrowList { get; set; }
    }
}
