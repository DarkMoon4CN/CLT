using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Cash;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuanglitouP2P.Common;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.BLL.Api;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Model;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTouOpenApi.Factory;
using ChuanglitouP2P.Model.chinapnr.Cash;

namespace ChuangLiTouOpenApi.Areas.Cash.Controllers
{
    /// <summary>
    /// 提现相关接口
    /// </summary>
    public class IndexController : BaseController
    {
        // GET: Cash/Index

        [HttpPost]
        public ActionResult Index(RequestParam<RequestCash> reqst)
        {
            LoggerHelper.Info(reqst);
            ResultInfo<string> res = new ResultInfo<string>("99999");

            int userId = ConvertHelper.ParseValue(reqst.body.userId.ToString(), 0);//用户id 
            decimal Amt = reqst.body.transAmt;//提现金额
            int usrBindCardId = ConvertHelper.ParseValue(reqst.body.usrBindCardId.ToString(), 0);//提现卡id
            decimal servf = 0.00M;
            string orderid = Settings.Instance.OrderCode;
            B_member_table b = new B_member_table();
            M_member_table p = new M_member_table();
            M_Cash mc = new M_Cash();
            M_ReqExt mr = new M_ReqExt();
            p = b.GetModel(userId);
            MemberLogic _logic = new MemberLogic();


            var result = _logic.SelectUserBindCards(usrBindCardId);

            if (result == null || result.Count < 0)
            {
                LoggerHelper.Info("提现失败,用户关联的银行卡不存在:" + JsonHelper.Entity2Json(reqst));
                return Content("提现失败,用户关联的银行卡不存在!");
            }
            if (!_logic.IsAllowWithdrawalCash(usrBindCardId, reqst.body.withdrawalType))
            {
                LoggerHelper.Info("提现失败,用户关联的银行卡不支持此类型的取现请求:" + JsonHelper.Entity2Json(reqst));
                return Content("提现失败,用户关联的银行卡不支持此类型的取现请求!");
            }

            MemberBankEntity ubc = result.FirstOrDefault();
            if (ubc.UsrCustId == p.UsrCustId)
            {
                mc.Version = "20";
                mc.CmdId = "Cash";
                mc.MerCustId = Settings.Instance.MerCustId;
                mc.OrdId = orderid;
                mc.UsrCustId = p.UsrCustId;
                mc.TransAmt = Amt.ToString("0.00");

                /*普通取现不收会员手续费*/
                mc.ServFee = "";
                mc.ServFeeAcctId = Settings.Instance.MerDt;
                mc.OpenAcctId = ubc.OpenAcctId;
                mc.RetUrl = Settings.Instance.GetCallbackUrl("/Cash/Index/CashCallback");

                mc.BgRetUrl = Settings.Instance.GetCallbackUrl("/Cash/Index/CashBgCallback");
                mc.Remark = "";
                mc.CharSet = "UTF-8";
                mc.MerPriv = "chuanglitou";
                //if (reqst.body.withdrawalType == 2)
                //    mr.FeeObjFlag = "U";  //客户承担提现手费续
                //else
                mr.FeeObjFlag = "M";  //商家承担提现手费续
                mr.FeeAcctId = Settings.Instance.MerDt;
                //reqst.body.withdrawalType = 1;
                switch (reqst.body.withdrawalType)
                {
                    case 2:
                        mr.CashChl = "IMMEDIATE";
                        break;
                    case 1:
                        mr.CashChl = "FAST";
                        break;
                    case 0:
                    default:
                        mr.CashChl = "GENERAL"; //一般取现
                        break;
                }
                //mr.CashChl = "GENERAL"; //一般取现
                // mr.CashChl = "FAST";  //快速取现
                // mr.CashChl = "IMMEDIATE";  // 即时取现
                mc.ReqExt = "[" + FastJSON.toJOSN(mr) + "]";

                StringBuilder chkVal = new StringBuilder();
                chkVal.Append(mc.Version);
                chkVal.Append(mc.CmdId);
                chkVal.Append(mc.MerCustId);
                chkVal.Append(mc.OrdId);
                chkVal.Append(mc.UsrCustId);
                chkVal.Append(mc.TransAmt);
                chkVal.Append(mc.ServFee);
                chkVal.Append(mc.ServFeeAcctId);
                chkVal.Append(mc.OpenAcctId);
                chkVal.Append(mc.RetUrl);
                chkVal.Append(mc.BgRetUrl);
                chkVal.Append(mc.Remark);
                chkVal.Append(mc.MerPriv);
                chkVal.Append(mc.ReqExt);
                string chkv = chkVal.ToString();

                LoggerHelper.Info("取现chkv字符:" + chkv);

                //私钥文件的位置(这里是放在了站点的根目录下)
                string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.MerPr;
                //需要指定提交字符串的长度
                int len = Encoding.UTF8.GetBytes(chkv).Length;
                StringBuilder sbChkValue = new StringBuilder(256);
                //加签
                int str = DllInterop.SignMsg(Settings.Instance.MerId, merKeyFile, chkv, len, sbChkValue);
                mc.ChkValue = sbChkValue.ToString();
                if (str == 0)
                {
                    M_td_UserCash mu = new M_td_UserCash();
                    B_td_UserCash mo = new B_td_UserCash();
                    mu.registerid = p.registerid;
                    mu.UsrCustId = p.UsrCustId;
                    mu.TransAmt = decimal.Parse(mc.TransAmt);
                    mu.FeeAmt = servf;
                    mu.OrdId = mc.OrdId;
                    mu.OrdIdTime = DateTime.Now;
                    mu.OrdIdState = 0;
                    mu.FeeObjFlag = mr.FeeObjFlag;
                    mo.Add(mu);
                }
                LoggerHelper.Info("加签字符:" + str);
                LoggerHelper.Info("提现提交表单:" + JsonHelper.Entity2Json(mc));
                return View(mc);
            }
            else
            {
                LoggerHelper.Info("提现失败,用户提现的银行卡的关联帐户和当前用户关联的银行卡帐户不一致:" + JsonHelper.Entity2Json(reqst));
                return Content("提现失败,用户提现的银行卡的关联帐户和当前用户关联的银行卡帐户不一致!");
            }
        }

        /// <summary>
        /// 提现回调页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CashCallback()
        {
            ReCash m = new ReCash();
            m.CmdId = DNTRequest.GetString("CmdId");
            m.RespCode = DNTRequest.GetString("RespCode");
            m.RespDesc = HttpUtility.UrlDecode(DNTRequest.GetString("RespDesc"));
            m.MerCustId = DNTRequest.GetString("MerCustId");
            m.OrdId = DNTRequest.GetString("OrdId");
            m.UsrCustId = DNTRequest.GetString("UsrCustId");
            m.TransAmt = DNTRequest.GetString("TransAmt");
            m.OpenAcctId = DNTRequest.GetString("OpenAcctId");
            m.OpenBankId = DNTRequest.GetString("OpenBankId");
            m.FeeAmt = DNTRequest.GetString("FeeAmt");
            m.FeeCustId = DNTRequest.GetString("FeeCustId");
            m.FeeAcctId = DNTRequest.GetString("FeeAcctId");
            m.ServFee = DNTRequest.GetString("ServFee");
            m.ServFeeAcctId = DNTRequest.GetString("ServFeeAcctId");
            m.RetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("RetUrl"));
            m.BgRetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("BgRetUrl"));
            m.MerPriv = DNTRequest.GetString("MerPriv");
            m.RespExt = HttpUtility.UrlDecode(DNTRequest.GetString("RespExt"));
            m.ChkValue = DNTRequest.GetString("ChkValue");
            m.RealTransAmt = DNTRequest.GetString("RealTransAmt");
            LoggerHelper.Info("取现返回报文:" + JsonHelper.Entity2Json(m));

            //验签
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.CmdId);
            chkVal.Append(m.RespCode);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.OrdId);
            chkVal.Append(m.UsrCustId);
            chkVal.Append(m.TransAmt);
            chkVal.Append(m.OpenAcctId);
            chkVal.Append(m.OpenBankId);
            chkVal.Append(m.FeeAmt);
            chkVal.Append(m.FeeCustId);
            chkVal.Append(m.FeeAcctId);
            chkVal.Append(m.ServFee);
            chkVal.Append(m.ServFeeAcctId);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.MerPriv);
            chkVal.Append(m.RespExt);

            string msg = chkVal.ToString();

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.PgPubk;
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(msg).Length;
            StringBuilder sbChkValue = new StringBuilder(256);

            int ret = DllInterop.VeriSignMsg(merKeyFile, msg, msg.Length, m.ChkValue);


            StringBuilder str = new StringBuilder();
            if (ret == 0)
            {
                //提现成功后，得多事务处理账户金额，流水及冻结金额等
                if (m.RespCode == "000")
                {
                    string cachename = m.OrdId + "Cash" + m.UsrCustId;
                    M_ReqExt mr = new M_ReqExt();
                    mr = FastJSON.ToObject<M_ReqExt>(m.RespExt.Replace("[", "").Replace("]", ""));
                    if (Settings.Instance.GeTThirdCache(cachename) == 0)
                    {
                        Settings.Instance.SetThirdCache(cachename);
                        B_usercenter BUC = new B_usercenter();
                        int CashOp = BUC.CashTran(m.OpenAcctId, m.OpenBankId, m.OrdId, m.RealTransAmt, m.UsrCustId, m.FeeAmt, mr.FeeObjFlag, mr.CashChl);
                        if (CashOp > 0)
                        {
                            string sql = "select registerid,username,mobile,UsrCustId,available_balance from hx_member_table where UsrCustId='" + m.UsrCustId + "'";
                            LoggerHelper.Info("审请取现成功短信sql:" + sql);
                            DataTable dt = DbHelper.Query(sql).Tables[0];
                            if (dt.Rows.Count > 0)
                            {

                                //短信通知
                                //尊敬的#USERANEM#,您已成功提现#MONEY#元,账户余额#MONEY1#.请注意查收!【创利投】

                                MemberLogic _logic = new MemberLogic();

                                var ebt = _logic.GetSmsEmailEntity(1, 12); // 获取内容
                                string contxt = ebt.SEContext;

                                StringBuilder sbsms = new StringBuilder(contxt);

                                sbsms = sbsms.Replace("#USERANEM#", dt.Rows[0]["username"].ToString());

                                sbsms = sbsms.Replace("#MONEY#", m.TransAmt);

                                decimal amt = decimal.Parse(dt.Rows[0]["available_balance"].ToString()) - decimal.Parse(m.TransAmt);

                                sbsms = sbsms.Replace("#MONEY1#", amt.ToString());

                                string mobile = dt.Rows[0]["mobile"].ToString();

                                M_td_SMS_record psms = new M_td_SMS_record();
                                B_td_SMS_record osms = new B_td_SMS_record();
                                int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.取现成功.ToString());
                                psms.phone_number = mobile;
                                psms.sendtime = DateTime.Now;
                                psms.senduserid = int.Parse(dt.Rows[0]["registerid"].ToString());
                                psms.smstype = smstype;
                                psms.smscontext = sbsms.ToString();
                                psms.orderid = SendSMS.Send(mobile, sbsms.ToString());
                                psms.vcode = "";
                                osms.Add(psms);
                                DateTime dti = DateTime.Now;
                                M_td_System_message pm = new M_td_System_message();
                                pm.MReg = int.Parse(dt.Rows[0]["registerid"].ToString());
                                pm.Mstate = 0;
                                pm.MTitle = "提现";
                                pm.MContext = sbsms.ToString();
                                pm.PubTime = dti;
                                pm.Mtype = 3;
                                B_usercenter.AddMessage(pm);

                                //即时提现，提现金额小于等于20万自动审核，高于20万人工审核
                                string cashChl = Enum.GetName(typeof(EnumCommon.E_hx_td_UserCash.EnumCashChl), (int)EnumCommon.E_hx_td_UserCash.EnumCashChl.IMMEDIATE);
                                if (mr.CashChl == cashChl && decimal.Parse(m.TransAmt) <= 200000)
                                {
                                    string retUrl = Settings.Instance.ImagesDomain + "/admin/UserCash/RePostCashProcessing";
                                    string bgRetUrl = Settings.Instance.ImagesDomain + "/admin/Thirdparty/BgCashProcessing";
                                    BusinessLogicHelper.AutoCheckCash(m.UsrCustId, retUrl, bgRetUrl);
                                }
                            }
                        }
                    }
                }
            }
            return View(m);
        }

        /// <summary>
        /// 汇付主动通知页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CashBgCallback()
        {
            string str1 = "";
            ReCash m = new ReCash();
            m.CmdId = DNTRequest.GetString("CmdId");
            m.RespCode = DNTRequest.GetString("RespCode");
            m.RespDesc = HttpUtility.UrlDecode(DNTRequest.GetString("RespDesc"));
            m.MerCustId = DNTRequest.GetString("MerCustId");
            m.OrdId = DNTRequest.GetString("OrdId");
            m.UsrCustId = DNTRequest.GetString("UsrCustId");
            m.TransAmt = DNTRequest.GetString("TransAmt");
            m.OpenAcctId = DNTRequest.GetString("OpenAcctId");
            m.OpenBankId = DNTRequest.GetString("OpenBankId");
            m.FeeAmt = DNTRequest.GetString("FeeAmt");
            m.FeeCustId = DNTRequest.GetString("FeeCustId");
            m.FeeAcctId = DNTRequest.GetString("FeeAcctId");
            m.ServFee = DNTRequest.GetString("ServFee");
            m.ServFeeAcctId = DNTRequest.GetString("ServFeeAcctId");
            m.RetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("RetUrl"));
            m.BgRetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("BgRetUrl"));
            m.MerPriv = DNTRequest.GetString("MerPriv");
            m.RespExt = HttpUtility.UrlDecode(DNTRequest.GetString("RespExt"));
            m.ChkValue = DNTRequest.GetString("ChkValue");
            m.RealTransAmt = DNTRequest.GetString("RealTransAmt");
            LoggerHelper.Info("后台取现返回报文:" + JsonHelper.Entity2Json(m));

            //验签
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.CmdId);
            chkVal.Append(m.RespCode);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.OrdId);
            chkVal.Append(m.UsrCustId);
            chkVal.Append(m.TransAmt);
            chkVal.Append(m.OpenAcctId);
            chkVal.Append(m.OpenBankId);
            chkVal.Append(m.FeeAmt);
            chkVal.Append(m.FeeCustId);
            chkVal.Append(m.FeeAcctId);
            chkVal.Append(m.ServFee);
            chkVal.Append(m.ServFeeAcctId);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.MerPriv);
            chkVal.Append(m.RespExt);

            string msg = chkVal.ToString();

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.PgPubk;
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(msg).Length;
            StringBuilder sbChkValue = new StringBuilder(256);

            int ret = DllInterop.VeriSignMsg(merKeyFile, msg, msg.Length, m.ChkValue);

            // LoggerHelper.Info("验签返回参数:" + ret.ToString());
            StringBuilder str = new StringBuilder();
            if (ret == 0)
            {
                //提现成功后，得多事务处理账户金额，流水及冻结金额等
                if (m.RespCode == "000")
                {
                    string cachename = m.OrdId + "Cash" + m.UsrCustId;

                    if (Settings.Instance.GeTThirdCache(cachename) == 0)
                    {
                        Settings.Instance.SetThirdCache(cachename);
                        B_usercenter BUC = new B_usercenter();
                        M_ReqExt mr = new M_ReqExt();
                        mr = FastJSON.ToObject<M_ReqExt>(m.RespExt.Replace("[", "").Replace("]", ""));
                        int CashOp = BUC.CashTran(m.OpenAcctId, m.OpenBankId, m.OrdId, m.RealTransAmt, m.UsrCustId, m.FeeAmt, mr.FeeObjFlag, mr.CashChl);
                        if (CashOp > 0)
                        {
                            string sql = "select registerid,username,mobile,UsrCustId,available_balance from hx_member_table where UsrCustId='" + m.UsrCustId + "'";
                            LoggerHelper.Info("后台审请取现成功短信sql:" + sql);
                            DataTable dt = DbHelper.Query(sql).Tables[0];
                            if (dt.Rows.Count > 0)
                            {

                                //短信通知
                                //尊敬的#USERANEM#,您已成功提现#MONEY#元,账户余额#MONEY1#.请注意查收!【创利投】
                                MemberLogic _logic = new MemberLogic();

                                var ebt = _logic.GetSmsEmailEntity(1, 12); // 获取内容
                                string contxt = ebt.SEContext;

                                StringBuilder sbsms = new StringBuilder(contxt);

                                sbsms = sbsms.Replace("#USERANEM#", dt.Rows[0]["username"].ToString());

                                sbsms = sbsms.Replace("#MONEY#", m.TransAmt);

                                decimal amt = decimal.Parse(dt.Rows[0]["available_balance"].ToString()) - decimal.Parse(m.TransAmt);

                                sbsms = sbsms.Replace("#MONEY1#", amt.ToString());

                                string mobile = dt.Rows[0]["mobile"].ToString();

                                M_td_SMS_record psms = new M_td_SMS_record();
                                B_td_SMS_record osms = new B_td_SMS_record();
                                int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.取现成功.ToString());
                                psms.phone_number = mobile;
                                psms.sendtime = DateTime.Now;
                                psms.senduserid = int.Parse(dt.Rows[0]["registerid"].ToString());
                                psms.smstype = smstype;
                                psms.smscontext = sbsms.ToString();
                                psms.orderid = SendSMS.Send(mobile, sbsms.ToString());
                                psms.vcode = "";
                                osms.Add(psms);
                                DateTime dti = DateTime.Now;
                                M_td_System_message pm = new M_td_System_message();
                                pm.MReg = int.Parse(dt.Rows[0]["registerid"].ToString());
                                pm.Mstate = 0;
                                pm.MTitle = "投现成功";
                                pm.MContext = sbsms.ToString();
                                pm.PubTime = dti;
                                pm.Mtype = 3;
                                B_usercenter.AddMessage(pm);

                                //即时提现，提现金额小于等于20万自动审核，高于20万人工审核
                                string cashChl = Enum.GetName(typeof(EnumCommon.E_hx_td_UserCash.EnumCashChl), (int)EnumCommon.E_hx_td_UserCash.EnumCashChl.IMMEDIATE);
                                if (mr.CashChl == cashChl && decimal.Parse(m.TransAmt) <= 200000)
                                {
                                    string retUrl = Settings.Instance.ImagesDomain + "/admin/UserCash/RePostCashProcessing";
                                    string bgRetUrl = Settings.Instance.ImagesDomain + "/admin/Thirdparty/BgCashProcessing";
                                    BusinessLogicHelper.AutoCheckCash(m.UsrCustId, retUrl, bgRetUrl);
                                }
                            }
                        }
                    }
                    str1 = "RECV_ORD_ID_" + m.OrdId;
                }
            }
            return Content(str1);
        }
    }
}