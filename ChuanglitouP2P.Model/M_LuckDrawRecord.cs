using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model
{
    /// <summary>
    /// 抽奖活动记录表
    /// </summary>
    public class M_LuckDrawRecord
    {
        /// <summary>
        /// 抽奖记录编号
        /// </summary>
        public int Ldre_ID { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public int Ldre_UserID { get; set; }
        /// <summary>
        /// 奖品编号
        /// </summary>
        public int Ldre_AwardID { get; set; }
        /// <summary>
        /// 奖品名称
        /// </summary>
        public string Ldre_AwardName { get; set; }
        /// <summary>
        /// 中奖时间
        /// </summary>
        public DateTime Ldre_CreatTime { get; set; }
        /// <summary>
        /// 奖品类型（0-现金50元；1-50元代金券；2-20元代金卷；3-10元代金卷；4-1%加息券；5-2%加息券；6-谢谢参与）
        /// </summary>
        public int Ldre_AwardType { get; set; }
        /// <summary>
        /// 汇付交易订单号（奖品类型为4时该字段有值）
        /// </summary>
        public string Ldre_OrderID { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string Ldre_ActivityName { get; set; }
    }

    public class M_LuckMan
    {
        /// <summary>
        /// 中奖人
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 中奖人手机号
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 奖品
        /// </summary>
        public string AwardName { get; set; }
    }

    public class M_LuckData
    {
        /// <summary>
        /// 记录编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 奖品名称
        /// </summary>
        public string AwardName { get; set; }
        /// <summary>
        /// 奖品类型
        /// </summary>
        public int AwardType { get; set; }
        /// <summary>
        /// 获奖会员
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 中奖时间
        /// </summary>
        public DateTime AwardTime { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }
    }

    public class M_LuckActivityNameData
    {
        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName { get; set; }
    }
}