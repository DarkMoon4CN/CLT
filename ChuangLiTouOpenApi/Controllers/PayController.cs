using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Member;
using ChuangLiTou.Core.Entities.Response;
using LLYTPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ChuanglitouP2P.Common;
namespace ChuangLiTouOpenApi.Controllers
{
    public class PayController : ApiController
    {
        /// <summary>
        /// 解绑LL银行卡
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public ResultInfo<string> UnBind(RequestParam<RequestPayEntity> reqst)
        {

            var ri = new ResultInfo<string>("99999");

            var sql=string.Format("SELECT no_agree FROM dbo.hx_td_LLPay_bindCard WHERE Usrid={0} AND BankCard='{1}'",reqst.body.userId,reqst.body.bankCard);

            LoggerHelper.Info(sql);
            var res=DbHelper.Query(sql);

            if (res!=null&&res.Tables[0].Rows.Count>0)
            {
            

                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("oid_partner", PartnerConfig.OID_PARTNER);
                sParaTemp.Add("user_id", reqst.body.userId);
                sParaTemp.Add("pay_type", "D");
                sParaTemp.Add("no_agree", res.Tables[0].Rows[0]["no_agree"].ToString());
                sParaTemp.Add("sign_type", PartnerConfig.SIGN_TYPE);
                string sign = YinTongUtil.addSign(sParaTemp, PartnerConfig.TRADER_PRI_KEY, PartnerConfig.MD5_KEY);
                sParaTemp.Add("sign", sign);


                string reqJson = YinTongUtil.dictToJson(sParaTemp);
                LoggerHelper.Info("解绑-请求报文[" + reqJson + "]");

                string responseJSON = HttpHelper.Post(ServerURLConfig.BANK_CARD_UNBIND_URL, reqJson);
                LoggerHelper.Info(responseJSON);

                ri.code = "1";
                ri.message = "";
                ri.body = responseJSON;
            }



            return ri;
        }
    }
}