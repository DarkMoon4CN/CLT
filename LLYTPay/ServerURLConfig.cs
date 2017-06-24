using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LLYTPay
{
    public class ServerURLConfig
    {

        public static string PAY_URL = "https://yintong.com.cn/llpayh5/authpay.htm";
        
       // public static string PAY_URL = "https://yintong.com.cn/payment/authpay.htm"; // 连连支付WEB收银台支付服务地址
        public static string QUERY_USER_BANKCARD_URL = "https://yintong.com.cn/traderapi/userbankcard.htm"; // 用户已绑定银行卡列表查询
        public static string QUERY_BANKCARD_URL = "https://yintong.com.cn/traderapi/bankcardquery.htm"; //银行卡卡bin信息查询
        public static string BANK_CARD_UNBIND_URL = "https://yintong.com.cn/traderapi/bankcardunbind.htm"; //解绑卡

        /// <summary>
        /// 连连支付WEB收银台支付服务地址
        /// </summary>
        public static string CASHPAY_URL = "https://yintong.com.cn/traderapi/cardandpay.htm"; // 连连支付WEB收银台支付服务地址
    }
}
