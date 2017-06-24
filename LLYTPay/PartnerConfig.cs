using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LLYTPay
{
    public class PartnerConfig
    {

        // RSA银通公钥
        public static string YT_PUB_KEY = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCSS/DiwdCf/aZsxxcacDnooGph3d2JOj5GXWi+q3gznZauZjkNP8SKl3J2liP0O6rU/Y/29+IUe+GTMhMOFJuZm1htAtKiu5ekW0GlBMWxf4FPkYlQkPE0FtaoMP3gYfh+OwI+fIRrpW3ySn3mScnc6Z700nU/VYrRkfcSCbSnRwIDAQAB";
        // RSA商户私钥
        //public static string TRADER_PRI_KEY = "MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBAMhKNA1Ws0H6PrZ8t1lQxhQjERj0hYf8QWBlF2DtlMajYU52WsiGIvid6iQQhJGc+aPNTf3MfWCWSHk2XRIYRpjoVPQ8Oz8sLF8j3pT3I2h2gDRNvO2xqX+x+jyFDMnAXm4uMyBYS9wabuhUchF5JkHT1A3rZZFYapPqMTj/zeEFAgMBAAECgYB+uPwwCFAIiYVOPqBe4U1CBmHV8TffLwpKLAvbptX/y/VQCHAt+Th9JqSyxsSpwLDuI4KZ9tzI1KzsDCpcvYFEMuoPNgwjZBFBsmTdXD+nxUTKVbTII6kITyzMMWDBnF8LxAicMKpYcRKaVOULCg/AHPGV32Efd4pH8cyJGcJ6TQJBAP+7+YygfcJLvxI9kk/2Se+dI//mX6WVh1V0RFgSl0cWry+xq9xTQofy0wU++TiXkA05aCJbwY0EjyodUOcpHkMCQQDIf3r3WVpW4Fx6t6B2geew4mllckFEHHDf0pXE5GWymccQHHxo6knFrzZ8F/97XwAIGTabNBXQiWd9G1DfEyMXAkEAow/84wpCpe0efEb+UDY+lqagGb+PJUne7UIhgfb4tr9kHQkxCF+egIj4vNOWndsmYwhDugS/uWc60iO3Pm4deQJAC3qA57hN27tsj/oDTcWSJiZQMmagJe4a6DV+LY+F4vu60clPthHzt0WYsPIOxllh/xSyc6A/v3ieXCM8Ngk6cQJBAJiX6nzlyLyHrHQ0jIdQ97bYtJTqh0ZC6bZ3PShCj3we/Cu+5v6L5Rmwx0s+OJ84OnWIopuuc5QwmOT53VRIntE=";
       public static string TRADER_PRI_KEY = "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAJq/O2In6TeAbkFaH4gfKu5ze5t4mpP481fvCFtw91XD90IRmU+aSTEMD4b+ycx5H86M3GmK80d/YRudz0fEqHOitU8sePC1c3DwZUg+DId/gKoxEo55DWMhn/08LZ3SYwPW+P1zZXdgw4//2TjtWjVzubNj3G5+ZBgxZbKK7ePZAgMBAAECgYAt6ow+RcVnmI4eihVNGKJueom0ygZj3bym3OBH8a8SHcGiMwKgohERopiA390U9OPBL/6+umIRVvSDc0Hr86hfoT+XmWolXRYv562AK359hYjetiCvyvV57QdqWHTGKHg0B7svYX4MGAQHhwX2PKKwurmWeFpl71Tubs9kzJ4bYQJBAMzPMCRIANeFTz95MaAcI3qBsae5SmxkpNV71MM4HE84FM7+1kpiaXsjFDI2SREJ3OmXX1AkEBP+Qbo5ZVWWCNUCQQDBbMjXxl85RcTUURA0sSke2KHSKD3d4a6ZN0FNgDoeAXVdPej2H/gl6+WUo+UBS1oFj9D77vIZr+EAKrNdsbD1AkEAuaFwePHqEW16joVCPWRDo925L9P5aJUv8W7zQJ2yyvqBobvIblMUV89LltctEoxl9jKE6RZGnFhvKmKrPg9moQJAQFZV6+fQ7rT1RoX8NPqkqdz23neCNJaHw/DsKMI5Epf7mNsp7QwvNzXi9HEbkDWnKOhwZAxTvRWSasLJTKX5LQJATjemAkH5sZBdrU+3/xI6zTtAlxDPSzb085/GEM993QxHtOwqNfZ6FmpTNGZYa6Lnk1wUt4LqRwVsaER4/1SP6Q==";

        public static string CashTRADER_PRI_KEY = "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAJq/O2In6TeAbkFaH4gfKu5ze5t4mpP481fvCFtw91XD90IRmU+aSTEMD4b+ycx5H86M3GmK80d/YRudz0fEqHOitU8sePC1c3DwZUg+DId/gKoxEo55DWMhn/08LZ3SYwPW+P1zZXdgw4//2TjtWjVzubNj3G5+ZBgxZbKK7ePZAgMBAAECgYAt6ow+RcVnmI4eihVNGKJueom0ygZj3bym3OBH8a8SHcGiMwKgohERopiA390U9OPBL/6+umIRVvSDc0Hr86hfoT+XmWolXRYv562AK359hYjetiCvyvV57QdqWHTGKHg0B7svYX4MGAQHhwX2PKKwurmWeFpl71Tubs9kzJ4bYQJBAMzPMCRIANeFTz95MaAcI3qBsae5SmxkpNV71MM4HE84FM7+1kpiaXsjFDI2SREJ3OmXX1AkEBP+Qbo5ZVWWCNUCQQDBbMjXxl85RcTUURA0sSke2KHSKD3d4a6ZN0FNgDoeAXVdPej2H/gl6+WUo+UBS1oFj9D77vIZr+EAKrNdsbD1AkEAuaFwePHqEW16joVCPWRDo925L9P5aJUv8W7zQJ2yyvqBobvIblMUV89LltctEoxl9jKE6RZGnFhvKmKrPg9moQJAQFZV6+fQ7rT1RoX8NPqkqdz23neCNJaHw/DsKMI5Epf7mNsp7QwvNzXi9HEbkDWnKOhwZAxTvRWSasLJTKX5LQJATjemAkH5sZBdrU+3/xI6zTtAlxDPSzb085/GEM993QxHtOwqNfZ6FmpTNGZYa6Lnk1wUt4LqRwVsaER4/1SP6Q==";
        
        // MD5 KEY
      //  public static string MD5_KEY = "201408071000001543test_20140812";

         public static string MD5_KEY = "20150902chuanglitou_20150902";

        //远程的 20150902chuanglitou_20150902
        // 接收异步通知地址
        public static string NOTIFY_URL = "http://m.chuanglitou.cn/LLPay/notify_url.aspx";  //请变更yourdomain为你的域名（及端口）

        // public static string NOTIFY_URL = "http://m.jiajubuy.cn/LLPay/notify_url.aspx";

        // 支付结束后返回地址
        public static string URL_RETURN = "http://m.chuanglitou.cn/index.html";    //请变更为您的返回地址

       // public static string URL_RETURN = "http://m.jiajubuy.cn/index.html";



       /// <summary>
       /// 代付接口URL
       /// </summary>
       public static string PAYNOTIFY_URL = "http://www.chuanglitou.cn/LLPay/Paynotify_url.aspx"; //正式平台
        /// <summary>
        /// 代付接口URL
        /// </summary>
     //  public static string PAYNOTIFY_URL = "http://m.jiajubuy.cn/LLPay/Paynotify_url.aspx"; //测试平台

      


        // 商户编号
       //public static string OID_PARTNER = "201408071000001543";     //请变更为您的商户号  

        public static string OID_PARTNER = "201508111000451507"; 
        // 签名方式 RSA或MD5
       public static string SIGN_TYPE = "MD5";    					//请选择签名方式

        public static string CASHSIGN_TYPE = "RSA";

        // 接口版本号，固定1.0
        public static string VERSION = "1.0";

        // 代付接口版本号，固定1.0
        public static string CashVERSION = "1.2";
        // 业务类型，连连支付根据商户业务为商户开设的业务类型； （101001：虚拟商品销售、109001：实物商品销售）
        public static string BUSI_PARTNER = "101001";   //请选择业务类型


    }
}
