using ChuangLiTou.Core.Entities.Response.AdNews;
using ChuangLiTou.Core.Entities.Response.Bank;
using System.Collections.Generic;
using System.Web.Http;

namespace ChuangLiTouOpenApi.Factory
{
    /// <summary>
    ///     创利投 接口基类
    /// </summary>
    [ApiAuth]
    public class BaseApi : ApiController
    {
        /// <summary>
        /// 删除特定活动实体-针对IOS移动端审核时存在资源侵权嫌疑
        /// </summary>
        protected virtual List<AdEntity> KickOutAppStoreRejectedAd(List<AdEntity> source, string appCode)
        {
            //return source;
            List<AdEntity> result = new List<AdEntity>();
            if (appCode == "123456")
            {
                List<string> rejectAdName = new List<string> { "app推广--微信端", "邀请好友", "1月邀请投资活动", "新年盛惠 新朋老友 17相投", "老友季大“赚”在即 “友”福同享" };
                foreach (var item in source)
                {
                    if (rejectAdName.Contains(item.AdName.Trim()))
                        continue;
                    result.Add(item);
                }
                return result;
            }
            return source;
        }

        /// <summary>
        /// 追加银行交易限额逻辑
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected virtual List<BankEntity> AppendBankEntityAmtLimit(List<BankEntity> source)
        {
            List<BankEntity> result = new List<BankEntity>();
            foreach (var item in source)
            {
                switch (item.OpenBankId)
                {
                    case "ICBC"://工商银行
                    case "ABC"://农业银行
                    case "CIB"://兴业银行
                        item.AmountLimitPerTrade = item.AmountLimitPerDay = 50000;
                        item.AmountLimitPerMonth = 0;
                        break;
                    case "CEB"://光大银行
                    case "CCB"://建设银行
                    case "PINGAN"://平安银行
                    case "SDB"://深发银行
                    case "CITIC"://中信银行
                    case "BOC"://中国银行
                    case "CBHB"://渤海银行
                    case "CMB"://招商银行
                        item.AmountLimitPerTrade = 50000;
                        item.AmountLimitPerDay = 100000;
                        item.AmountLimitPerMonth = 0;
                        break;
                    case "SPDB"://浦发银行
                    case "BOS"://上海银行
                        item.AmountLimitPerTrade = 5000;
                        item.AmountLimitPerDay = 50000;
                        item.AmountLimitPerMonth = 0;
                        break;
                    case "PSBC"://邮储银行
                        item.AmountLimitPerTrade = item.AmountLimitPerDay = 5000;
                        item.AmountLimitPerMonth = 0;
                        break;
                    default:
                        item.AmountLimitPerTrade = item.AmountLimitPerDay = item.AmountLimitPerMonth = 0;
                        break;
                }
                result.Add(item);
            }
            return result;
        }
    }
}