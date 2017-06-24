using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuangLiTou.Core.Entities.ChinaPnr;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Bonus;
using ChuangLiTou.Core.Entities.Request.Member;
using ChuangLiTou.Core.Entities.Request.Region;
using ChuangLiTou.Core.Entities.Request.Sms;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Bonus;
using ChuangLiTou.Core.Entities.Response.Invest;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuangLiTou.Core.Entities.Response.NormalArea;
using ChuangLiTou.Core.Entities.Response.SmsEmail;
using ChuangLiTou.Core.Entities.Response.UserAddress;
using ChuanglitouP2P.Common.Util;
using ChuangLiTouOpenApi.Factory;
using ChuanglitouP2P.Common;
using System.Data;
using ChuanglitouP2P.Model.Invest;
using ChuanglitouP2P.BLL.Api;
using ChuanglitouP2P.Model;

namespace ChuangLiTouOpenApi.Controllers
{
    /// <summary>
    /// 全局环境接口
    /// </summary>
    public class GlobalController : BaseApi
    {
        ///// <summary>
        ///// 获取服务器当前时间
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public ResultInfo<string> ServerTime(RequestParam reqst)
        //{
        //    return new ResultInfo<string>
        //    {
        //        body = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        //    };
        //}

        /// <summary>
        /// 获取流水的所有枚举状态
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<List<dynamic>> SelectTypeFinance(RequestParam reqst)
        {
            var ri = new ResultInfo<List<dynamic>>("99999");
            List<dynamic> list = new List<dynamic>();
            foreach (int i in Enum.GetValues(typeof(EnumTypesFinance)))
            {
                string strName = Enum.GetName(typeof(EnumTypesFinance), i);//获取名称
                list.Add(new { Key = i, Value = strName });
            }
            ri.code = "1";
            ri.message = Settings.Instance.GetErrorMsg(ri.code);
            ri.body = list;
            return ri;
        }

        /// <summary>
        /// 获取客户服务电话号码
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<string> CustomerServiceNumber(RequestParam reqst)
        {
            var ri = new ResultInfo<string>("99999");
            ri.body = Settings.Instance.GetCustomerServiceNumber;
            ri.code = "1";
            ri.message = Settings.Instance.GetErrorMsg(ri.code);
            return ri;
        }

        /// <summary>
        /// 获取平台统计信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<GlobelInvestStatistics> PlatformStatistics(RequestParam reqst)
        {
            var ri = new ResultInfo<GlobelInvestStatistics>("99999");
            ri.body = new GlobelLogic().PlatformStatistics(reqst);
            ri.code = "1";
            ri.message = Settings.Instance.GetErrorMsg(ri.code);
            return ri;
        }

        /// <summary>
        /// 获取平台运维报告的URL地址
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<string> PlatformStatisticsReportUrl(RequestParam reqst)
        {
            var ri = new ResultInfo<string>("99999");
            ri.body = Settings.Instance.GetPlatformStatisticsReportUrl;
            ri.code = "1";
            ri.message = Settings.Instance.GetErrorMsg(ri.code);
            return ri;
        }

        /// <summary>
        /// 获取平台充值规则说明的URL地址
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<string> RechargeRuleDescriptionUrl(RequestParam reqst)
        {
            var ri = new ResultInfo<string>("99999");
            ri.body = Settings.Instance.GetRechargeRuleDescriptionUrl;
            ri.code = "1";
            ri.message = Settings.Instance.GetErrorMsg(ri.code);
            return ri;
        }

        /// <summary>
        /// 获取用户相关数据是否变更的标记状态.包含：代金卷
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<GlobalNotificationMarks> NotificationMarks(RequestParam<GlobalNotificationMarksRequest> reqst)
        {
            var ri = new ResultInfo<GlobalNotificationMarks>("99999");
            using (GlobalLogic global = new GlobalLogic())
            {
                ri.body = global.GetGlobalNotificationMarks(reqst.body);
            }
            ri.code = "1";
            ri.message = Settings.Instance.GetErrorMsg(ri.code);
            return ri;
        }

        /// <summary>
        /// 获取特定时间内的主要推广活动的URL地址
        /// </summary>
        [HttpPost]
        public ResultInfo<string> SpecialActivityUrl(RequestParam reqst)
        {
            var ri = new ResultInfo<string>("99999");
            AdNewsLogic logic = new AdNewsLogic();
            ri.body = logic.GetWebAdModel(11, "双十二").AdLink;
            ri.code = "1";
            ri.message = Settings.Instance.GetErrorMsg(ri.code);
            return ri;
        }

        /// <summary>
        /// 获取此次提现需要花费的手续费金额
        /// </summary>
        [HttpPost]
        public ResultInfo<string> CashServiceFees(RequestParam<CashServiceFeesRequest> reqst)
        {
            var ri = new ResultInfo<string>("99999");
            ri.body = "0.00";
            using (ServiceFeesLogic logic = new ServiceFeesLogic())
            {
                var item = logic.GetWithdrawalCashFees(reqst.body.WithdrawalType, reqst.body.WithdrawalAmount);
                ri.body = item.ServiceFees.ToString();
            }
            ri.code = "1";
            ri.message = Settings.Instance.GetErrorMsg(ri.code);
            return ri;
        }
    }
}