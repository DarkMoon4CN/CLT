using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.wdzj
{
    public class Investors
    {
        /// <summary>
        /// 投标人ID
        /// 不能将ID加*隐藏部分字符，否则会导致多个投资人使用同一个ID，导致投资集中度高。
        /// 是
        /// </summary>
        public string subscribeUserName { get; set; }
        /// <summary>
        /// 投标金额
        /// 用户初始投标的金额。对于平台优惠奖励政策的情况（如投标人投****元自动返还**元，或是某个人获得满标奖励**）只计投标金额。
        /// 是
        /// </summary>
        public double amount { get; set; }
        /// <summary>
        /// 有效金额
        /// 实际中标金额。如平台无’投标金额’和’有效金额’之分，则’投标金额’和’有效金额’传一样的即可。
        /// 是
        /// </summary>
        public double validAmount { get; set; }
        /// <summary>
        /// 投标时间
        /// 格式为标准时间格式：’2014-07-23 12:23:22’
        /// 是
        /// </summary>
        public string addDate { get; set; }
        /// <summary>
        /// 投标状态
        /// 1：全部通过 2：部分通过 注意：平台没有这个字段的默认为1
        /// 是
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 标识手动或自动投标
        /// 0：手动 1：自动 注意:平台没有这个字段的默认为0
        /// 是
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 投标来源
        /// 1 ：PC端 2 ：WAP端 3 ：平台APP客户端 4 ：微信
        /// 否
        /// </summary>
        public int sourceType { get; set; }
    }
}
