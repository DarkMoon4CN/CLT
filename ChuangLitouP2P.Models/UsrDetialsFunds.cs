using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;
namespace ChuangLitouP2P.Models
{
    public class UsrDetialsFunds
    {

        /// <summary>
        /// 用户资金流水
        /// </summary>
        public PagedList<hx_Capital_account_water> Capital { get; set; }



        /// <summary>
        /// 用户奖励流水
        /// </summary>
        public PagedList<hx_UserAct> V_ACT { get; set; }



    }
}
