using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace ChuangLitouP2P.Models
{

    #region 用户全部信息对象 +UsrAllInfoMode
    /// <summary>
    /// 用户全部信息对象
    /// </summary>
    public class UsrAllInfoMode
    {
        /// <summary>
        /// 资金明细
        /// </summary>
        public PagedList<hx_Capital_account_water> Capitcal_water { get; set; }


        /// <summary>
        /// 用户充值列表
        /// </summary>
        public PagedList<V_Recharge_user_bank> Recharge { get; set; }

        /// <summary>
        /// 连连用户充值列表
        /// </summary>
        public PagedList<V_td_LLpay_bank> LLRecharge { get; set; }

        /// <summary>
        /// 用户提现列表
        /// </summary>
        public PagedList<hx_td_UserCash> UserCash { get; set; }

        /// <summary>
        /// 用户连连提现列表
        /// </summary>
        public PagedList<hx_td_LL_cash> UserLLCash { get; set; }
        /// <summary>
        /// 用户投资记录
        /// </summary>
        public PagedList<V_hx_Bid_records_borrowing_target> Bid_Records { get; set; }

        /// <summary>
        /// 回款记录
        /// </summary>
        public PagedList<V_borrowing_Bid_records_income_statement> Bid_Records_income { get; set; }

        /// <summary>
        /// 现金奖励
        /// </summary>
        public PagedList<hx_UserAct> cash { get; set; }

        /// <summary>
        /// 抵扣券奖励
        /// </summary>
        public PagedList<hx_UserAct> xianjin { get; set; }

        /// <summary>
        /// 加息券奖励
        /// </summary>
        public PagedList<hx_UserAct> jiaxi { get; set; }



        /// <summary>
        /// 邀请奖励
        /// </summary>
        public PagedList<V_YaoQinList> yaoqin { get; set; }

        /// <summary>
        /// 用户对象
        /// </summary>
        public hx_member_table UserMode { get; set; }

        /// <summary>
        /// 用户登录信息
        /// </summary>
        public PagedList<hx_td_usrlogininfo> usrlogin { get; set; }



    }
    #endregion
}
