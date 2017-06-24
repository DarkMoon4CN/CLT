using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model
{
    /// <summary>
    /// 网贷之家模型
    /// </summary>
    public class WDZJModel
    {
        public class JsonProject
        {
            /// <summary>
            /// 总页数（每页显示20条借款标信息）
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
            public Double totalAmount { get; set; }

            /// <summary>
            /// 借款标信息
            /// </summary>
            public List<Project> borrowList { get; set; }
        }

        /// <summary>
        /// 借款标信息
        /// </summary>
        public class Project
        {
            /// <summary>
            /// 项目主键(唯一)
            /// </summary>
            public string projectId { get; set; }
            /// <summary>
            /// 借款标题
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// 借款金额(若标未满截标，以投标总额为准)
            /// </summary>
            public double amount { get; set; }
            /// <summary>
            /// 进度*例如：100（只传满标数据，进度均为100）
            /// </summary>
            public string schedule { get; set; }
            /// <summary>
            /// 利率* 百分比 例如：24.5% 统一转化为年化利率传过来。
            /// </summary>
            public string interestRate { get; set; }
            /// <summary>
            /// 借款期限
            /// </summary>
            public int deadline { get; set; }
            /// <summary>
            /// 期限单位* 仅限 ‘天’ 或 ‘月’
            /// </summary>
            public string deadlineUnit { get; set; }
            /// <summary>
            /// 奖励* 奖励统一返回比例,而不是奖励金额。例如：奖励50元，借款金额1000元； 奖励=50/1000=5% 即返回’5’） 奖励比例统一去掉’%’，比如奖励比例1.2%则返回’1.2’即可年化奖励直接加到利率字段中
            /// </summary>
            public double reward { get; set; }
            /// <summary>
            /// 例如： 抵押标 ，质押标，信用标，债权转让标，净值标，秒标等。移动端数据需注明移动端。(对于不参与计算平均利率的秒标（天标）、活动标（体验标），就传“秒标”或是“活动标”) 借款类型可根据平台的情况修改，不限于上述类型。若一个标有多个类型，则在每个类型中间加半角分号“;”（如实地认证+担保，就传“实地认证;担保”）
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 还款方式 1：到期还本息(到期还本付息，一次性还本付息，按日计息到期还本, 一次性付款、秒还);2：每月等额本息(按月分期，按月等额本息);3：每季分期（按季分期，按季等额本息）;5：每月付息到期还本(先息后本);6：等额本金(按月等额本金);7：每季付息到期还本（按季付息到期还本）;8：每月付息分期还本;9：先付息到期还本
            /// </summary>
            public int repaymentType { get; set; }
            /// <summary>
            /// 投资人数据（具体字段看类的属性信息）
            /// </summary>
            public List<target> subscribes { get; set; }
            ///// <summary>
            ///// 借款人所在省份*比如“广东”，“浙江”等，去掉“省”
            ///// </summary>
            //public string province { get; set; }
            ///// <summary>
            ///// 借款人所在城市。
            ///// </summary>
            //public string city { get; set; }
            /// <summary>
            /// 借款人ID(多借款人使用逗号[,]分割) *不能将ID加* 隐藏部分字符，否则会导致多个借款人使用同一个ID，导致借款集中度高。 如果借款人id包含[,] 请替换为其他符号 *如果需要加密,请用借款人手机号MD5加密(32位大写)
            /// </summary>
            public string userName { get; set; }
            ///// <summary>
            ///// 发标人头像的URL
            ///// </summary>
            //public string userAvatarUrl { get; set; }
            ///// <summary>
            ///// 借款用途
            ///// </summary>
            //public string amountUsedDesc { get; set;}
            ///// <summary>
            ///// 营收*即该笔借款平台收取的服务费、管理费等。
            ///// </summary>
            //public double revenue { get; set; }
            /// <summary>
            /// 标的详细页面地址链接
            /// </summary>
            public string loanUrl { get; set; }
            /// <summary>
            /// 标的成功时间。（满标的时间） *注意：是标被投满的时间（此标最后一个投标人投标的时间），而不是发标时间。 *格式为标准时间格式：’2014-07-23 12:23:22’ 注意：getProjectsByDate调用时必须有该字段。
            /// </summary>
            public string successTime { get; set; }
            ///// <summary>
            ///// 发标时间 *格式为标准时间格式：’2014-07-23 12:23:22’
            ///// </summary>
            //public string publishTime { get; set; }
            ///// <summary>
            ///// 标所属平台频道板块 如：爱投资频道下的融资租赁、保理等
            ///// </summary>
            //public string plateType { get; set; }
            ///// <summary>
            ///// 保障担保机构名称 如：爱投资标保障机构
            ///// </summary>
            //public string guarantorsType { get; set; }
            ///// <summary>
            ///// 是否机构借款 *0:否,1:是
            ///// </summary>
            //public int isAgency { get; set; }
        }
        /// <summary>
        /// 单个标的投标列表信息
        /// </summary>
        public class target
        {
            /// <summary>
            /// 投标人ID *不能将ID加* 隐藏部分字符，否则会导致多个投资人使用同一个ID，导致投资集中度高。 *如果需要加密,请用投资人手机号MD5加密(32位大写)
            /// </summary>
            public string subscribeUserName { get; set; }
            /// <summary>
            /// 投标金额 * 用户初始投标的金额。对于平台优惠奖励政策的情况（如投标人投**** 元自动返还**元，或是某个人获得满标奖励**）只计投标金额。
            /// </summary>
            public double amount { get; set; }
            /// <summary>
            /// 有效金额 * 实际中标金额。 如平台无’投标金额’和’有效金额’之分，则’投标金额’和’有效金额’传一样的即可。
            /// </summary>
            public double validAmount { get; set; }
            /// <summary>
            /// 投标时间 *格式为标准时间格式：’2014-07-23 12:23:22’
            /// </summary>
            public string addDate { get; set; }
            /// <summary>
            /// 投标状态 *1：全部通过 2：部分通过 注意：平台没有这个字段的默认为1
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 标识手动或自动投标 0：手动 1：自动 注意:平台没有这个字段的默认为0
            /// </summary>
            public int type { get; set; }
            ///// <summary>
            ///// 投标来源： 1 ：PC端 2 ：WAP端 3 ：平台APP客户端 4 ：微信
            ///// </summary>
            //public int sourceType { get; set; }
        }
    }
}
