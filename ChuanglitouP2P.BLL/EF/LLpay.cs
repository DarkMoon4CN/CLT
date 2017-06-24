using ChuanglitouP2P.Model;
using LLYTPay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.BLL.EF
{
    public class LLpay
    {

        public string orderquery(string orderid)
        {

            string url = "https://yintong.com.cn/traderapi/orderquery.htm";
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("oid_partner", PartnerConfig.OID_PARTNER);
            sParaTemp.Add("no_order", "" + orderid + "");

            sParaTemp.Add("sign_type", PartnerConfig.CASHSIGN_TYPE);
            string sign = YinTongUtil.addSign(sParaTemp, PartnerConfig.CashTRADER_PRI_KEY, PartnerConfig.MD5_KEY);
            sParaTemp.Add("sign", sign);
            string reqJson = YinTongUtil.dictToJson(sParaTemp);
            YinTongUtil.writelog("代付查询订单信息-请求报文[" + reqJson + "]");
            string responseJSON = postJson(url, reqJson);
            return responseJSON;

        }


        #region API付款
        /// <summary>
        /// API付款
        /// </summary>
        /// 
        public string cashpay(M_LLPay p)
        {


            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("oid_partner", PartnerConfig.OID_PARTNER);
            sParaTemp.Add("api_version", PartnerConfig.CashVERSION);
            // sParaTemp.Add("acct_name", HttpUtility.UrlEncode(p.Acct_name));
            sParaTemp.Add("acct_name", p.Acct_name);
            sParaTemp.Add("card_no", p.Card_no);
            sParaTemp.Add("no_order", p.No_order);
            sParaTemp.Add("dt_order", p.Dt_order);
            sParaTemp.Add("money_order", p.Money_order);
            sParaTemp.Add("flag_card", "0");
            // sParaTemp.Add("info_order", HttpUtility.UrlEncode("P2P投资回款"));
            sParaTemp.Add("info_order", "P2P投资回款");
            sParaTemp.Add("notify_url", PartnerConfig.PAYNOTIFY_URL);
            sParaTemp.Add("city_code", p.City_code);
            sParaTemp.Add("bank_code", p.Bank_code);
            //sParaTemp.Add("brabank_name", HttpUtility.UrlEncode(p.Brabank_name));
            sParaTemp.Add("brabank_name", p.Brabank_name);
            sParaTemp.Add("sign_type", PartnerConfig.CASHSIGN_TYPE);
            string sign = YinTongUtil.addSign(sParaTemp, PartnerConfig.CashTRADER_PRI_KEY, PartnerConfig.MD5_KEY);
            sParaTemp.Add("sign", sign);
            string reqJson = YinTongUtil.dictToJson(sParaTemp);
            YinTongUtil.writelog("付款银行卡卡bin信息查询-请求报文[" + reqJson + "]");




            string responseJSON = postJson(ServerURLConfig.CASHPAY_URL, reqJson);
            return responseJSON;


            /*
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("oid_partner", PartnerConfig.OID_PARTNER);
            sParaTemp.Add("api_version", PartnerConfig.CashVERSION);
            sParaTemp.Add("acct_name", "张长江");
            sParaTemp.Add("card_no", "9559980014443530812");
            sParaTemp.Add("no_order", "20150908120120");
            sParaTemp.Add("dt_order", "20150908120120");
            sParaTemp.Add("money_order", "0.05");
            sParaTemp.Add("flag_card", "0");
            sParaTemp.Add("info_order", "p2p投资回款");
            sParaTemp.Add("notify_url", PartnerConfig.PAYNOTIFY_URL);
            sParaTemp.Add("city_code", "110000");
            sParaTemp.Add("bank_code", "01030000");
            sParaTemp.Add("brabank_name", "丽泽桥支行");
            sParaTemp.Add("sign_type", PartnerConfig.SIGN_TYPE);
            string sign = YinTongUtil.addSign(sParaTemp, PartnerConfig.TRADER_PRI_KEY, PartnerConfig.MD5_KEY);
            sParaTemp.Add("sign", sign);
            string reqJson = YinTongUtil.dictToJson(sParaTemp);
            YinTongUtil.writelog("银行卡卡bin信息查询-请求报文[" + reqJson + "]");
            string responseJSON = postJson(ServerURLConfig.CASHPAY_URL, reqJson);
            return responseJSON;
            */
        }

        #endregion



        #region pos方法
        /// <summary>
        /// pos方法
        /// </summary>
        /// 
        public string postJson(string serverUrl, string reqJson)
        {

            var http = (HttpWebRequest)WebRequest.Create(new Uri(serverUrl));
            http.Accept = "application/json";
            http.ContentType = "application/json;charset=utf-8";
            http.Method = "POST";


            //ASCIIEncoding encoding = new ASCIIEncoding();


            // Byte[] bytes = encoding.GetBytes(reqJson);

            byte[] bytes = Encoding.UTF8.GetBytes(reqJson);
            Stream newStream = http.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();

            var response = http.GetResponse();

            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();

            return content;
        } 
        #endregion


    }
}
