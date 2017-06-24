//using ChuangLiTou.Core.BusinessLogic;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Member;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Bank;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuangLiTouOpenApi.Factory;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.BLL.Api;
using ChuanglitouP2P.Model.chinapnr.QueryCardInfo;

namespace ChuangLiTouOpenApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class BankController : BaseApi
    {
        // GET: AdNews
        private readonly MemberLogic logic;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdNewsController"/> class.
        /// </summary>
        /// <param name="adnewsLogic">The adnews logic.</param>
        public BankController(MemberLogic bankLogic)
        {
            this.logic = bankLogic;
        }

        /// <summary>
        /// 获取银行列表--解志辉
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<List<BankEntity>> SelectBankList(RequestParam<RequestMemberDetail> reqst)
        {
            var ri = new ResultInfo<List<BankEntity>>("99999");
            try
            {
                if (reqst.body == null || reqst.body.userId == 0)
                {
                    ri.body = logic.SelectBankList();
                }
                else
                {
                    ri.body = logic.SelectBankListByUserID(reqst.body.userId);

                    //查询出结果为空的时候，返回所有用户的数据
                    if (ri.body == null || ri.body.Count == 0)
                    {
                        ri.body = logic.SelectBankList();
                    }
                }

                if (ri.body == null)
                {
                    ri.code = "1000000010";
                    ri.message = Settings.Instance.GetErrorMsg(ri.code);
                    return ri;
                }
                //ri.body = AppendBankEntityAmtLimit(ri.body);
                ri.code = "1";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }

        /// <summary>
        /// 获取用户关联的银行列表--解志辉
        /// </summary>
        /// <returns></returns>
        public ResultInfo<List<MemberBankEntity>> SelectUserBankList(RequestParam<RequestMemberDetail> reqst)
        {

            QueryCardInfoByHuiFu(logic.SelectMemberByUserId(reqst.body.userId).UsrCustId);

            var ri = new ResultInfo<List<MemberBankEntity>>("99999");
            try
            {
                ri.code = "1";
                ri.body = logic.SelectUserBankList(reqst.body.userId);
                if (ri.body == null)
                {
                    ri.code = "4000000000";
                }
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                LoggerHelper.Error(JsonHelper.Entity2Json(ri));
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }

        private void QueryCardInfoByHuiFu(string usrCustId)
        {

            M_QueryCardInfo m = new M_QueryCardInfo();

            m.Version = "10";
            m.CmdId = "QueryCardInfo";
            m.MerCustId = Settings.Instance.MerCustId;
            m.UsrCustId = usrCustId;
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.Version);
            chkVal.Append(m.CmdId);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.UsrCustId);

            string chkv = chkVal.ToString();


            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.MerPr;
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int str = DllInterop.SignMsg(Settings.Instance.MerId, merKeyFile, chkv, len, sbChkValue);

            //  Response.Write("加签字符:" + str.ToString() + "<br>");

            m.ChkValue = sbChkValue.ToString();

            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values.Add("Version", m.Version);
                values.Add("CmdId", m.CmdId);
                values.Add("MerCustId", m.MerCustId);
                values.Add("UsrCustId", m.UsrCustId);
                values.Add("ChkValue", m.ChkValue);

                string url = Settings.Instance.ChinapnrUrl;
                //同步发送form表单请求
                byte[] result = client.UploadValues(url, "POST", values);
                var retStr = Encoding.UTF8.GetString(result);

                ReQueryCardInfo reg = new ReQueryCardInfo();

                JavaScriptSerializer js = new JavaScriptSerializer();   //实例化一个能够序列化数据的类
                ReQueryCardInfo list = js.Deserialize<ReQueryCardInfo>(retStr);    //将json数据转化为对象类型并赋值给list

                string RespCode = list.RespCode;

                List<M_UsrCardInfolist> orderdetail = list.UsrCardInfolist;

                StringBuilder builder = new StringBuilder();
                builder.Append(list.CmdId);
                builder.Append(list.RespCode);
                builder.Append(list.MerCustId);
                builder.Append(list.UsrCustId);
                var msg = builder.ToString();
                string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.PgPubk;
                int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, list.ChkValue);

                if (ret == 0)
                {
                    var delSql = " delete hx_UsrBindCardC  where UsrCustId = '" + usrCustId + "' ";

                    DbHelper.ExecuteSql(delSql);


                    if (list.RespCode == "000")
                    {
                        string sql = "";
                        if (orderdetail.Count > 0)
                        {
                            sql = "  update hx_member_table set isbankcard=1  where  UsrCustId='" + usrCustId + "' and  isbankcard=0 ";
                            DbHelper.ExecuteSql(sql);
                        }
                        for (int i = 0; i < orderdetail.Count; i++)
                        {
                            int iy = 0;

                            if (orderdetail[i].IsDefault == "Y")
                            {

                                iy = 1;
                            }

                            int bindcartype = 0;

                            if (orderdetail[i].ExpressFlag == "Y")
                            {
                                bindcartype = 1;
                            }

                            sql = " insert into hx_UsrBindCardC (UsrCustId,OpenAcctId,OpenBankId,defCard,BindCardType)values('" + orderdetail[i].UsrCustId + "','" + orderdetail[i].CardId + "','" + orderdetail[i].BankId + "'," + iy + "," + bindcartype + ")";

                            DbHelper.ExecuteSql(sql);
                        }
                    }
                }
            }
        }
    }
}