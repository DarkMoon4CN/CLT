using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.P2Peye
{
    public class LoanEntity
    {

  /// <summary>
  /// 标的唯一编号(不为空,很重要)
  /// </summary>
	
        public string id { get; set; }
        /// <summary>
        /// 平台名称	平台中文名称.
        /// </summary>
        public string platform_name { get; set; }
        /// <summary>
        /// 标的链接	借款标的URL链接.
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 标题	标的标题信息.
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 用户名	借款人(发标人)的用户名称,如果没有借款人用户名,一定要返回下面的 userid,尽量不为空.
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 用户编号	发标人的用户编号/ID.
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 标的状态	0,正在投标中的借款标;1,已完成(包括还款中和已完成的借款标).
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 借款类型	0 代表信用标,1 担保标;2 抵押,质押标, 3 秒标;
        /// 4 债权转让标(流转标,二级市场标的);5 理财计划(宝类业务_活期);
        ///6 其它;7 净值标;8 活动标(体验标).9 理财计划(宝类业务_定期).
        ///3，4，5标类型不参与贷款余额计算；请注意5【理财计划(宝类业务_活期)】和【9理财计划(宝类业务_定期)】的区分；4债权转让标指的是不会产生新待还的转让
        /// 如果会产生新待还，请返回其他标类型.
        /// </summary>
        public string c_type { get; set; }
        /// <summary>
        /// 借款金额	以元为单位,精度2位(1000.00),如万元请转换为元,请过滤掉借款金额小于50块的标.
        /// </summary>
        public string amount { get; set; }
        /// <summary>
        /// 借款年利率	如果为月利率或天利率,统一转换为年利率并使用小数表示;精度4位,如:0.0910.
        /// </summary>
        public string rate { get; set; }
        /// <summary>
        /// 借款期限	借款期限的数字。如3月这里只返回3若借款标的为流转标,对应的要有流转期限.
        /// </summary>
        public string period { get; set; }
        /// <summary>
        /// 期限类型	0 代表天,1 代表月.
        /// </summary>
        public string p_type { get; set; }
        /// <summary>
        /// 还款方式	
        /// 0 代表其他;
        /// 1 按月等额本息还款;
        /// 2按月付息,到期还本;
        /// 3 按天计息,一次性还本付息;
        /// 4,按月计息,一次性还本付息;
        /// 5 按季分期还款;
        /// 6 为等额本金,按月还本金;
        /// 7 先息期本.
        /// </summary>
        public string pay_way { get; set; }
        /// <summary>
        /// 完成百分比	转换成小数表示.
        /// </summary>
        public string process { get; set; }
        /// <summary>
        /// 投标奖励	如奖励为百分比,使用小数表示. 非必须
        /// </summary>
        public string reward { get; set; }
        /// <summary>
        /// 担保奖励	如奖励为百分比,使用小数表示. 非必须
        /// </summary>
        public string guarantee { get; set; }
        /// <summary>
        /// 标的创建时间	格式如:2013-08-10 14:24:01(24小时制).
        /// </summary>
        public string start_time { get; set; }
        /// <summary>
        /// 满标时间(最后一笔投标时间)	格式如:2013-08-10 13:10:00我们要的投资记录最后一笔的时间,请不要理解为标最后的的还款完成日期.
        /// </summary>
        public string end_time { get; set; }
        /// <summary>
        /// 投资次数	这笔借款标有多少个投标记录.
        /// </summary>
        public string invest_num { get; set; }
        /// <summary>
        /// 续投奖励	继续投标的奖励. 非必须
        /// </summary>
        public string c_reward { get; set; }


    }
}
