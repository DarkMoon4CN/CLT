using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Model.chinapnr.AddBidInfo
{
    public class M_AddBidInfo
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { set; get; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string CmdId { set; get; }

        /// <summary>
        /// 商户客户号
        /// </summary>
        public string MerCustId { set; get; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public string ProId { set; get; }


        /// <summary>
        /// 借款人ID
        /// </summary>
        public string BorrCustId { set; get; }

        /// <summary>
        /// 借款总金额
        /// </summary>
        public string BorrTotAmt { set; get; }

        /// <summary>
        /// 年利率
        /// </summary>
        public string YearRate { set; get; }

        /// <summary>
        /// 还款方式
        /// 01 等额本息
        /// 02 等额本金
        /// 03 按期付息
        /// 04 一次性还款
        /// 99 其它
        /// </summary>
        public string RetType { set; get; }
        /// <summary>
        /// 投标开始时间 格式 yyyMMddhhmmss
        /// </summary>
        public string BidStartDate { set; get; }
        /// <summary>
        /// 投标结束时间  格式 yyyMMddhhmmss
        /// </summary>
        public string BidEndDate { set; get; }
        /// <summary>
        ///  应还款总金额
        /// </summary>
        public string RetAmt { set; get; }
        /// <summary>
        /// 应还款日期 格式 yyyyMMdd
        /// </summary>
        public string RetDate { set; get; }

        /// <summary>
        /// 担保公司ID
        /// </summary>
        public string GuarCompId { set; get; }


        /// <summary>
        /// 担保金额
        /// </summary>
        public string GuarAmt { set; get; }


        /// <summary>
        /// 项目所在地
        /// </summary>
        public string ProArea { set; get; }

        /// <summary>
        /// 商户后台应答地址
        /// </summary>
        public string BgRetUrl { set; get; }

        /// <summary>
        /// 商户私有域
        /// </summary>
        public string MerPriv { set; get; }

        /// <summary>
        /// 入参扩展域
        /// </summary>
        public string ReqExt { set; get; }

        /// <summary>
        /// 签名
        /// </summary>
        public string ChkValue { set; get; }







    }
}
