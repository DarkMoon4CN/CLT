using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model
{
    /// <summary>
    /// 投资6月专享标抽IPhone7活动
    /// </summary>
    [Serializable]
    public class M_GrabIphone
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int RegrsterID { get; set; }
        /// <summary>
        /// Color
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime Addtime { get; set; }
        /// <summary>
        /// 抽奖状态：0未参与，1已参与
        /// </summary>
        public int LuckDrawState { get; set; }
        /// <summary>
        /// 中奖状态：0未中奖，1已中奖
        /// </summary>
        public int WinningState { get; set; }
        /// <summary>
        /// 中奖时间
        /// </summary>
        public DateTime WinningTime { get; set; }

        /// <summary>
        /// 标的ID
        /// </summary>
        public int TargetID { get; set; }
        /// <summary>
        /// 用户投标记录ID
        /// </summary>
        public int BidRecordsID { get; set; }
        /// <summary>
        /// 投标金额
        /// </summary>
        public string InvestmentAmount { get; set; }


    }
}
