using ChuangLiTou.Core.Entities.ChinaPnr;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Member;
using ChuangLiTou.Core.Entities.Response.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.BLL.Api;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.BLL.EF;
using ChuangLiTouOpenApi.Factory;

namespace ChuangLiTouOpenApi.Areas.UserAuthentication.Controllers
{
    /// <summary>
    /// 用户实名认证
    /// </summary>
    public class IndexController : BaseController
    {
        //        public ActionResult RequestRealName(string userId)
        //        {
        //            MemberLogic _logic = new MemberLogic();
        //            var uid = ConvertHelper.ParseValue(userId, 0);
        //            var p = _logic.SelectMemberByUserId(uid);

        //            var ckd = Settings.Instance.SiteDomain;
        //            UserEntity m = new UserEntity
        //            {
        //                MerId = Settings.Instance.MerId,
        //                Version = "10",
        //                CmdId = "UserRegister",
        //                MerCustId = Settings.Instance.MerCustId,
        //                BgRetUrl = ckd + ("/UserAuthentication/Index/BgRetUrlForUserRegister"),
        //                RetUrl = ckd + ("/UserAuthentication/Index/CallbackForUserRegister"),
        //                UsrMp = p.mobile,
        //                UsrEmail = p.email,
        //                UsrId = p.username,
        //                IdNo = "",
        //                IdType = "00"
        //            };
        //            LoggerHelper.Info("身份证:" + m.IdNo);
        //#pragma warning disable 1587
        //            /// 签名规则
        //            /// Version
        //            /// CmdId
        //            /// MerCustId
        //            /// BgRetUrl
        //            /// RetUrl
        //            /// UsrId
        //            /// UsrName
        //            /// IdType 
        //            /// IdNo
        //            /// UsrMp
        //            /// UsrEmail
        //            /// MerPriv 
        //            /// Version + CmdId + MerCustId + BgRetUrl + RetUrl + UsrId + UsrName + IdType + IdNo + UsrMp + UsrEmail + MerPriv
        //#pragma warning restore 1587
        //            StringBuilder chkVal = new StringBuilder();
        //            string temp = m.MerPriv;
        //            AppendDeviceFlag(reqst.header.appId.ToString(), ref temp);
        //            Mt.MerPriv = temp;
        //            chkVal.Append(m.Version);
        //            chkVal.Append(m.CmdId);
        //            chkVal.Append(m.MerCustId);
        //            chkVal.Append(m.BgRetUrl);
        //            chkVal.Append(m.RetUrl);
        //            chkVal.Append(m.UsrId);
        //            chkVal.Append(m.IdType);
        //            chkVal.Append(m.IdNo);
        //            chkVal.Append(m.UsrMp);
        //            chkVal.Append(m.UsrEmail);
        //            string chkv = chkVal.ToString();
        //            //私钥文件的位置(这里是放在了站点的根目录下)
        //            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.MerPr;
        //            //需要指定提交字符串的长度
        //            int len = Encoding.UTF8.GetBytes(chkv).Length;
        //            StringBuilder sbChkValue = new StringBuilder(256);            //加签
        //            DllInterop.SignMsg(m.MerId, merKeyFile, chkv, len, sbChkValue);
        //            m.ChkValue = sbChkValue.ToString();
        //            LoggerHelper.Info(chkVal.ToString());
        //            LoggerHelper.Info("加签结果：" + m.ChkValue);
        //            return View(m);
        //        }

        /// <summary>
        /// 实名认证接口
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RealNameAuth(RequestParam<RequestValidate> reqst)
        {
            LoggerHelper.Info(JsonHelper.Entity2Json(reqst));
            MemberLogic _logic = new MemberLogic();
            var uid = ConvertHelper.ParseValue(reqst.body.userId, 0);
            var p = _logic.SelectMemberByUserId(uid);

            var ckd = Settings.Instance.SiteDomain;
            UserEntity m = new UserEntity
            {
                MerId = Settings.Instance.MerId,
                Version = "10",
                CmdId = "UserRegister",
                MerCustId = Settings.Instance.MerCustId,
                BgRetUrl = ckd + ("/UserAuthentication/Index/BgRetUrlForUserRegister"),
                RetUrl = ckd + ("/UserAuthentication/Index/CallbackForUserRegister"),
                UsrMp = p.mobile,
                UsrEmail = p.email,
                UsrId = p.username,
                IdNo = reqst.body.userIdNo,
                IdType = "00"
            };
            LoggerHelper.Info("身份证:" + m.IdNo);
#pragma warning disable 1587
            /// 签名规则
            /// Version
            /// CmdId
            /// MerCustId
            /// BgRetUrl
            /// RetUrl
            /// UsrId
            /// UsrName
            /// IdType 
            /// IdNo
            /// UsrMp
            /// UsrEmail
            /// MerPriv 
            /// Version + CmdId + MerCustId + BgRetUrl + RetUrl + UsrId + UsrName + IdType + IdNo + UsrMp + UsrEmail + MerPriv
#pragma warning restore 1587
            StringBuilder chkVal = new StringBuilder();
            string temp = m.MerPriv;
            AppendDeviceFlag(reqst.header.appId.ToString(), ref temp);
            m.MerPriv = temp;
            chkVal.Append(m.Version);
            chkVal.Append(m.CmdId);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.UsrId);
            chkVal.Append(m.IdType);
            chkVal.Append(m.IdNo);
            chkVal.Append(m.UsrMp);
            chkVal.Append(m.UsrEmail);
            chkVal.Append(m.MerPriv);
            string chkv = chkVal.ToString();
            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.MerPr;
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);            //加签
            DllInterop.SignMsg(m.MerId, merKeyFile, chkv, len, sbChkValue);
            m.ChkValue = sbChkValue.ToString();
            LoggerHelper.Info(chkVal.ToString());
            LoggerHelper.Info("加签结果：" + m.ChkValue);
            return View(m);
        }

        /// <summary>
        /// 实名认证 后台异步通知
        /// </summary>
        /// <returns></returns>
        public ActionResult BgRetUrlForUserRegister()
        {
            return CallbackForUserRegister(true);
        }

        /// <summary>
        /// 实名注册回调地址
        /// </summary>
        /// <returns></returns>
        public ActionResult CallbackForUserRegister(bool isSyncCallback = false)
        {
            var mEntity = new MemberEntity();
            string username = "";
            string useremail = "";
            string merp = "";
            UserEntity m = new UserEntity();
            m.CmdId = DNTRequest.GetString("CmdId");
            m.RespCode = DNTRequest.GetString("RespCode");
            m.RespDesc = DNTRequest.GetString("RespDesc");
            m.MerCustId = DNTRequest.GetString("MerCustId");
            m.UsrId = DNTRequest.GetString("UsrId");
            m.UsrCustId = DNTRequest.GetString("UsrCustId");
            m.BgRetUrl = DNTRequest.GetString("BgRetUrl");
            m.TrxId = DNTRequest.GetString("TrxId");
            m.RetUrl = DNTRequest.GetString("RetUrl");
            merp = m.MerPriv = DNTRequest.GetString("MerPriv");
            m.IdType = DNTRequest.GetString("IdType");
            m.IdNo = DNTRequest.GetString("IdNo");
            m.UsrMp = DNTRequest.GetString("UsrMp");
            useremail = DNTRequest.GetString("UsrEmail");
            m.UsrEmail = useremail;
            username = HttpUtility.UrlDecode(DNTRequest.GetString("UsrName"));
            m.UsrName = username;
            m.ChkValue = DNTRequest.GetString("ChkValue");
            LoggerHelper.Info("注册开户返回报文:" + JsonHelper.Entity2Json(m));
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.CmdId);
            chkVal.Append(m.RespCode);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.UsrId);
            chkVal.Append(m.UsrCustId);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.TrxId);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.MerPriv);
            string chkv = chkVal.ToString();
            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.PgPubk;
            int ret = DllInterop.VeriSignMsg(merKeyFile, chkv, chkv.Length, m.ChkValue);

            string deviceKey = PickoutDeviceFlag(ref merp);//pick out device code from comment field
            m.MerPriv = merp;
            if (ret == 0)
            {
                if (m.RespCode == "000")
                {
                    string cachename = m.UsrId + "Register" + m.MerCustId;
                    LoggerHelper.Info("开户验签成功");
                    object lockObj = new object();
                    lock (lockObj)
                    {
                        lock (this)
                        {
                            if (Settings.Instance.GeTThirdCache(cachename) == 0)
                            {
                                Utils.SetThirdCache(cachename);
                                MemberLogic mLogic = new MemberLogic();
                                mEntity.realname = m.UsrName;
                                mEntity.UsrCustId = m.UsrCustId;
                                mEntity.UsrId = m.UsrId;
                                mEntity.iD_number = m.IdNo;
                                if (mLogic.UpdateUserRealAuth(mEntity))
                                {
                                    LoggerHelper.Info("数据库操作成功");
                                    var tempUsername = PageHelper.GetUserSplit(m.UsrId);
                                    var tempEntity = mLogic.SelectMemberEntityByName(tempUsername);
                                    //TODO: 以后添加实名奖励 
                                    if (tempEntity != null)
                                    {
                                        using (ActFacade actFacade = new ActFacade())
                                        {
                                            actFacade.SendBonusAfterRegister(tempEntity.registerid.Value, Utils.GetDevicePlatformCode(deviceKey));
                                        }
                                    }
                                    else
                                    {
                                        LoggerHelper.Info("Line to 276:查找不到用户，无法写入注册奖励！");
                                    }
                                    LoggerHelper.Info("开户成功，数据库操作成功,开户返回报文:" + JsonHelper.Entity2Json(m));
                                }
                                else
                                {
                                    /*第三方成功，本地服务器操作失败*/
                                    LoggerHelper.Info("开户成功，数据库操作失败,开户返回报文:" + JsonHelper.Entity2Json(m));
                                }
                            }
                        }
                    }
                }
                if (isSyncCallback)
                    return Content("RECV_ORD_ID_" + m.TrxId);
            }
            else
            {
                /*验签不成功*/
                LoggerHelper.Info("开户验签失败");
                if (isSyncCallback)
                    return Content("-1");
            }
            return View(m);
        }
    }
}