using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.wdzj
{
   public class BorrowEntity
    {
       /// <summary>
        /// 项目主键(唯一)
       /// </summary>
       public string projectId { get; set; }
       /// <summary>
       /// title	String	借款标题	是
       /// </summary>
       public string title { get; set; }
       /// <summary>
       /// amount	double	借款金额(若标未满截标，以投标总额为准)	是
       /// </summary>
       public double amount { get; set; }
       /// <summary>
       /// schedule	String	进度 例如：100（只传满标数据，进度均为100）	是
       /// </summary>
       public string schedule { get; set; }
       /// <summary>
       /// interestRate	String	利率 百分比 例如：24.5% 统一转化为年化利率传过来。	是
       /// </summary>
       public string interestRate { get; set; }
       /// <summary>
       /// deadline	int	借款期限	是
       /// </summary>
       public int deadline { get; set; }
       /// <summary>
       /// deadlineUnit	String	期限单位 * 仅限 ‘天’ 或 ‘月’	是
       /// </summary>
       public string deadlineUnit { get; set; }
       /// <summary>
       /// reward	double	奖励统一返回比例,而不是奖励金额。例如：奖励50元，借款金额1000元； 奖励=50/1000=5% 即返回’5’）
       /// 奖励比例统一去掉’%’，比如奖励比例1.2%则返回’1.2’即可
       /// 年化奖励直接加到利率字段中	是（如果平台系统无奖励字段，则统一返回0）
       /// </summary>
       public double reward { get; set; }
       /// <summary>
       /// 例如： 抵押标 ，质押标，信用标，债权转让标，净值标，秒标等。移动端数据需注明移动端。
       /// (对于不参与计算平均利率的秒标（天标）、活动标（体验标），就传“秒标”或是“活动标”)
       /// 借款类型可根据平台的情况修改，不限于上述类型。若一个标有多个类型，则在每个类型中间加半角分号“;”（如实地认证+担保，就传“实地认证;担保”）	是
       /// </summary>
       public string type { get; set; }
       /// <summary>
       /// 还款方式
       ///1：到期还本息(到期还本付息，一次性还本付息，按日计息到期还本,一次性付款、秒还)
       ///2：每月等额本息(按月分期，按月等额本息)
       ///3：每季分期（按季分期，按季等额本息）
       ///5：每月付息到期还本(先息后本)
       ///6：等额本金(按月等额本金)
       ///7：每季付息到期还本（按季付息到期还本）
       ///8：每月付息分期还本
       ///9：先付息到期还本	是
       /// </summary>
       public int repaymentType { get; set; }
       /// <summary>
       /// 标所属平台频道板块
       /// 如：爱投资频道下的融资租赁、保理等	否
       /// </summary>
       public string plateType { get; set; }
       /// <summary>
       /// 保障担保机构名称
       /// 如：爱投资标保障机构	否
       /// </summary>
       public string guarantorsType { get; set; }
       /// <summary>
       /// 投资人数据（具体字段看下面的投标列表信息）	是
       /// </summary>
      // public List<Investors> subscribes { get; set; }
       /// <summary>
       /// 借款人所在省份。比如“广东”，“浙江”等，去掉“省”	否
       /// </summary>
       public string province { get; set; }
       /// <summary>
       /// 借款人所在城市。	否
       /// </summary>
       public string city { get; set; }
       /// <summary>
       /// 发标人ID 不能将ID加*隐藏部分字符，否则会导致多个借款人使用同一个ID，导致借款集中度高。
       /// 是
       /// </summary> 
       public string userName { get; set; }
       /// <summary>
       /// 发标人头像的URL	否
       /// </summary>
       public string userAvatarUrl { get; set; }
       /// <summary>
       /// 借款用途	否
       /// </summary>
       public string amountUsedDesc { get; set; }
       /// <summary>
       /// 营收。即该笔借款平台收取的服务费、管理费等。	否
       /// </summary>
       public double revenue { get; set; }
       /// <summary>
       /// 标的详细页面地址链接	是
       /// </summary>
       public string loanUrl { get; set; }
       /// <summary>
       /// 标的成功时间。（满标的时间）
       /// 注意：是标被投满的时间（此标最后一个投标人投标的时间），而不是发标时间。
       /// 格式为标准时间格式：’2014-07-23 12:23:22’
       /// 注意：getProjectsByDate调用时必须有该字段。	是
       /// </summary>
       public string successTime { get; set; }
       /// <summary>
       /// 发标时间
       /// 格式为标准时间格式：’2014-07-23 12:23:22’	否
       /// </summary>
       public string publishTime { get; set; }
       
    }
}
