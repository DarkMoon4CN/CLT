using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace ChuangLitouP2P.Models
{
    public class InvestMode
    {

        /// <summary>
        /// 投标记录
        /// </summary>
        public PagedList<V_hx_Bid_records_borrowing_target> Bid_record { get; set; }

        /// <summary>
        /// 标的数据对象
        /// </summary>
        public V_borrowing_target_addlist vbtaMode { get; set; }


    }
}
