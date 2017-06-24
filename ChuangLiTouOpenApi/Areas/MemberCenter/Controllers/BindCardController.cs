//using ChuangLiTou.Core.BusinessLogic.OldVersion;
using ChuangLiTou.Core.Entities.ChinaPnr;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Member;
using ChuanglitouP2P.BLL;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.UserBindCard;

namespace ChuangLiTouOpenApi.Areas.MemberCenter.Controllers
{
    /// <summary>
    /// 绑卡相关
    /// </summary>
    public class BindCardController : Controller
    {
        /// <summary>
        /// 绑卡接口
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(RequestParam<RequestMemberDetail> reqst)
        {
            B_member_table b = new B_member_table();
            M_member_table p = new M_member_table();
            p = b.GetModel(reqst.body.userId);
            string Version = "10";
            string CmdId = "UserBindCard";
            string MerCustId = Settings.Instance.MerCustId;
            string UsrCustId = p.UsrCustId;
            string BgRetUrl = Settings.Instance.GetCallbackUrl("/MemberCenter/BindCard/BindCardCallback");
            string MerPriv = Settings.Instance.Base64Encoder("chuanglitou");

            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(Version);
            chkVal.Append(CmdId);
            chkVal.Append(MerCustId);
            chkVal.Append(UsrCustId);
            chkVal.Append(BgRetUrl);
            chkVal.Append(MerPriv);

            string chkv = chkVal.ToString();

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.MerPr;
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int str = DllInterop.SignMsg(Settings.Instance.MerId, merKeyFile, chkv, len, sbChkValue);

            string ChkValue = sbChkValue.ToString();

            UserEntity br = new UserEntity();

            br.Version = Version;
            br.CmdId = CmdId;
            br.MerCustId = MerCustId;
            br.UsrCustId = UsrCustId;
            br.BgRetUrl = BgRetUrl;
            br.MerPriv = MerPriv;
            br.ChkValue = ChkValue;

            return View(br);
        }

        /// <summary>
        /// 绑卡回调页
        /// </summary>
        /// <returns></returns>
        public ActionResult BindCardCallback()
        {
            ReUserBindCard m = new ReUserBindCard();

            m.CmdId = DNTRequest.GetString("CmdId");
            m.RespCode = DNTRequest.GetString("RespCode");
            m.RespDesc = HttpUtility.UrlDecode(DNTRequest.GetString("RespDesc"));
            m.MerCustId = DNTRequest.GetString("MerCustId");
            m.OpenAcctId = DNTRequest.GetString("OpenAcctId");
            m.OpenBankId = DNTRequest.GetString("OpenBankId");
            m.UsrCustId = DNTRequest.GetString("UsrCustId");
            m.TrxId = DNTRequest.GetString("TrxId");
            m.BgRetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("BgRetUrl"));
            m.MerPriv = HttpUtility.UrlDecode(DNTRequest.GetString("MerPriv"));
            m.ChkValue = DNTRequest.GetString("ChkValue");

            LoggerHelper.Info("消息类型:" + m.CmdId + "<br/>" +
                "应答返回码:" + m.RespCode + "<br/>" +
                "应答描述:" + m.RespDesc + "<br/>" +
                "商户客户号:" + m.MerCustId + "<br/>" +
                "开户银行账号:" + m.OpenAcctId + "<br/>" +
                "开户银行代码:" + m.OpenBankId + "<br/>" +
                "用户客户号:" + m.UsrCustId + "<br/>" +
                "交易唯一标识:" + m.TrxId + "<br/>" +
                "后台应答地址:" + m.BgRetUrl + "<br/>" +
                "私域:" + m.MerPriv + "<br/>" +
                "签名:" + m.ChkValue);

            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.CmdId);
            chkVal.Append(m.RespCode);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.OpenAcctId);
            chkVal.Append(m.OpenBankId);
            chkVal.Append(m.UsrCustId);
            chkVal.Append(m.TrxId);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.MerPriv);

            string chkv = chkVal.ToString();

            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.PgPubk;

            int ret = DllInterop.VeriSignMsg(merKeyFile, chkv, chkv.Length, m.ChkValue);

            LoggerHelper.Info("签名返回结果:" + ret + "<br/>" +
                "绑卡报文:" + JsonHelper.Entity2Json(m));

            if (ret == 0)
            {
                if (m.RespCode == "000")
                {
                    LoggerHelper.Info("进入处理:UsrCustId" + m.UsrCustId);
                    M_UsrBindCard p = new M_UsrBindCard();
                    B_UsrBindCard o = new B_UsrBindCard();
                    p.registerid = 0;
                    p.UsrCustId = m.UsrCustId;
                    p.OpenAcctId = m.OpenAcctId;
                    p.OpenBankId = m.OpenBankId;

                    if (o.Exists(p.UsrCustId))
                    {
                        p.defCard = 0;
                    }
                    else
                    {
                        p.defCard = 1;
                    }

                    LoggerHelper.Info("卡状态：" + p.defCard);

                    o.Add(p);

                    string sql = "update hx_member_table set isbankcard=1 where UsrCustId='" + m.UsrCustId + "'";

                    DbHelper.ExecuteSql(sql);

                    var str1 = "RECV_ORD_ID_" + m.TrxId;

             


                    LoggerHelper.Info("卡绑定操作签名成功" + str1);

                    #region 天天钻渠道判断和完成步骤

                    B_member_table bm = new B_member_table();
                    M_member_table mm = bm.GetModel(m.UsrCustId);
                    if (mm != null)
                    {
                        //B_hx_member_ChannelKeys bck = new B_hx_member_ChannelKeys();
                        //hx_member_ChannelKeys mck = bck.GetModel(mm.registerid);
                        /////天天钻登录用户判断///第一次投资，按照投资金额反馈天天钻 该用户到达 step 值
                        //if (mck != null && mck.ChannelId == "2" && (mck.ChannelStep == "0" || string.IsNullOrEmpty(mck.ChannelStep)))
                        //{
                        //    string ret4 = Settings.Instance.GetTTzAPI(mck.ChannelRegid, mm.mobile, 1);//
                        //    string sqlTTZ = string.Format("UPDATE dbo.hx_member_ChannelKeys SET ChannelStep='1' WHERE Mregisterid={0}", mck.Mregisterid);
                        //    DbHelper.ExecuteSql(sqlTTZ);
                        //    LoggerHelper.Info("天天钻渠道绑定银行卡返回结果:" + ret4 + "  用户id:" + mck.Mregisterid + "");
                        //}
                    }

                    #endregion 天天钻渠道判断和完成步骤

                    
                }
                else
                {
                    LoggerHelper.Info("卡绑定操作签名失败");
                }
            }
            else
            {
                LoggerHelper.Info("签名失败");
            }

            return View(m);
        }
    }
}