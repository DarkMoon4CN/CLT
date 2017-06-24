using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace ChuangLitouP2P.Models
{
    public class xianjinModelList
    {
        /// <summary>
        /// 正常抵扣券
        /// </summary>
        public PagedList<hx_UserAct> xianjilist { get; set; }

        /// <summary>
        ///  已用抵扣券
        /// </summary>
        public PagedList<hx_UserAct> xianjiuses { get; set; }

        /// <summary>
        ///  过期抵扣券
        /// </summary>
        public PagedList<hx_UserAct> xianjilost { get; set; }

    }
}
