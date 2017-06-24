using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web.Http;
//using ChuangLiTou.Core.BusinessLogic;
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
using System.Net.Http;
using System.Web;
using System.Collections;
using System.IO;
using ChuanglitouP2P.BLL.Api;
using ChuanglitouP2P.Model;
using ChuangLitouP2P.Models;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.BLL.EF;

namespace ChuangLiTouOpenApi.Controllers
{
    /// <summary>
    ///     用户相关接口
    /// </summary>
    public class MemberController : BaseApi
    {
        private readonly MemberLogic _logic;
        private readonly SmsRecordLogic _recordLogic;
        private readonly BonusLogic bonusLogic;
        private readonly RegionLogic regionLogic;

        /// <summary>
        ///     会员相关接口
        /// </summary>
        /// <param name="logic"></param>
        /// <param name="recordLogic"></param>
        /// <param name="bonusLogic"></param>
        public MemberController(MemberLogic logic, SmsRecordLogic recordLogic, BonusLogic bonusLogic, RegionLogic regionLogic)
        {
            _logic = logic;
            _recordLogic = recordLogic;
            this.bonusLogic = bonusLogic;
            this.regionLogic = regionLogic;
        }

        /// <summary>
        ///     会员注册接口--解志辉
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<MemberEntity> Regist(RequestParam<RequestRegistMember> reqst)
        {
            //todo 添加注册代码

            var ri = new ResultInfo<MemberEntity>("99999");
            try
            {
                var mobile = reqst.body.userMobile.ToString();

                if (string.IsNullOrEmpty(mobile))
                {
                    ri.code = "1000000000";
                }
                else if (!ValidateHelper.IsHandset(mobile))
                {
                    ri.code = "1000000001";
                }
                else if (!_logic.CheckMobile(mobile))
                {
                    // true 存在 false 不存在

                    #region 验证code

                    var code = reqst.body.code.ToString();
                    var w = 8;
                    //if (_recordLogic.CheckCode(code, w, mobile) || 1 == 1)
                    if (_recordLogic.CheckCode(code, w, mobile))
                    {
                        #region 注册方法

                        var ent = new MemberEntity
                        {
                            username = mobile,
                            password =
                                EncryptHelper.Encrypt(reqst.body.userPass.ToString(), Settings.Instance.EncryptKey),
                            mobile = mobile
                        };
                        string me = _logic.Regist(ent, reqst.body.code.ToString(), reqst.header.appId);
                        ent.registerid = ConvertHelper.ParseValue(me, 0);

                        if (ent.registerid > 0)
                        {
                            //#region 发送注册成功短信
                            //var smstype = (int)Enum.Parse(typeof(SmsType), SmsType.注册成功.ToString());
                            //var messageId = 18; //短信注册成功
                            //var smsEntity = _logic.GetSmsEmailEntity(1, messageId); // 获取注册成功邮件内容
                            //if (smsEntity != null)
                            //{
                            //    var cnt = smsEntity.SEContext.Replace("#USERANEM#", mobile);
                            //    //  var rx = SmsHelper.Send(mobile, cnt);
                            //    var pm = new SmsRecordEntity
                            //    {
                            //        phone_number = mobile,
                            //        sendtime = DateTime.Now,
                            //        senduserid = ent.registerid,
                            //        smstype = smstype,
                            //        smscontext = cnt.ToString(),
                            //        orderid = 1,
                            //        vcode = "",
                            //        ip = Settings.Instance.ClientIp
                            //    };
                            //    _recordLogic.AddRecord(pm);
                            //}
                            //#endregion
                            try
                            {
                                var sf = reqst.body.sourceFrom;
                                LoggerHelper.Info(ent.registerid + ":" + sf);
                                //参与活动
                                // using (ActFacade actFacade = new ActFacade())
                                //{
                                //    actFacade.SendBonusAfterRegister(ent.registerid.Value);
                                //}
                            }
                            catch (Exception ex)
                            {
                                LoggerHelper.Info(ent.registerid + ":3");
                            }
                            ri.code = "1000000099";
                            ri.body = ent;
                        }
                        else
                        {
                            ri.code = "99999";
                        }

                        #endregion
                    }
                    else
                    {
                        ri.code = "1000000011"; //验证码不存在或已过期
                    }

                    #endregion
                }
                else
                {
                    // 用户已存在
                    ri.code = "1000000002";
                }
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
        ///     验证手机号是否被占用--解志辉
        /// </summary>
        /// <param name="reqst">
        ///     {"mobile":"手机号"}
        /// </param>
        /// <returns>Code==1  可用</returns>
        [HttpPost]
        public ResultInfo<string> CheckMobile(RequestParam<SmsEntity> reqst)
        {
            var ri = new ResultInfo<string>("99999");
            try
            {
                var mobile = reqst.body.mobile.ToString();

                if (string.IsNullOrEmpty(mobile))
                {
                    ri.code = "1000000000";
                }
                else if (!ValidateHelper.IsHandset(mobile))
                {
                    ri.code = "1000000001";
                }
                else if (!_logic.CheckMobile(mobile))
                {
                    // true 存在 false 不存在
                    ri.code = "1";
                }
                else
                {
                    ri.code = "1000000002"; //手机号已经被注册 
                }
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst));
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }

        /// <summary>
        ///     会员登录接口--解志辉
        /// </summary>
        /// <param name="reqst">
        ///     {"userName":"用户名","userPass":"密码明文"}
        /// </param>
        /// <returns>ResultInfo&lt;dynamic&gt;.</returns>
        [HttpPost]
        public ResultInfo<MemberEntity> Login(RequestParam<RequestLoginEntity> reqst)
        {
            //todo 添加登录验证代码

            var ri = new ResultInfo<MemberEntity>("99999");
            try
            {
                var un = reqst.body.userName.ToString();
                var up = reqst.body.userPass.ToString();

                if (string.IsNullOrEmpty(un))
                {
                    ri.code = "1000000007";
                }
                else if (string.IsNullOrEmpty(up))
                {
                    ri.code = "1000000008";
                }
                else
                {
                    #region 验证用户名和密码是否正确

                    MemberEntity ent = _logic.SelectMemberEntityByName(un);
                    string pass = EncryptHelper.Encrypt(up);
                    if (ent != null && ent.password.ToLowerInvariant().Equals(pass.ToLowerInvariant()))
                    {
                        #region 登录成功
                        try
                        {
                            hx_td_usrlogininfo usrmode = new hx_td_usrlogininfo();
                            usrmode.logintime = DateTime.Now;
                            usrmode.Loginusrname = ent.username;
                            usrmode.loginusrpass = "********";
                            usrmode.registerid = ent.registerid;
                            usrmode.loginIP = Settings.Instance.ClientIp;
                            usrmode.logincity = GetIpToCity.GetAddressByIp(Settings.Instance.ClientIp);

                            if (reqst.header.appId != 123456)
                            {
                                usrmode.loginsource = 2;//安卓
                            }
                            else
                            {
                                usrmode.loginsource = 3;//ios
                            }
                            usrmode.loginstate = 0;

                            _logic.SaveLoginInfo(usrmode);
                            using (ActFacade actFacade = new ActFacade())
                            {
                                actFacade.SendBonusAfterLogin(ent.registerid.Value, Utils.GetDevicePlatformCode(reqst.header.appId.ToString()));
                            }
                        }
                        catch (Exception tx)
                        {

                            throw (tx);
                        }
                        #endregion



                        ri.code = "1";
                        ri.body = ent;
                    }
                    else
                    {
                        ri.code = "1000000009";

                    }

                    #endregion
                }
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
        ///     找回密码接口--解志辉
        /// </summary>
        /// <param name="reqst">
        ///     {"mobile":"手机号","code":"验证码","newPwd":"密码明文"}
        /// </param>
        /// <returns>ResultInfo&lt;System.String&gt;.</returns>
        [HttpPost]
        public ResultInfo<string> FindPassword(RequestParam<RequestFindPass> reqst)
        {
            //todo 添加找回密码代码
            var ri = new ResultInfo<string>("99999");
            try
            {
                var mobile = reqst.body.mobile.ToString();

                if (string.IsNullOrEmpty(mobile))
                {
                    ri.code = "1000000000";
                }
                else if (!ValidateHelper.IsHandset(mobile))
                {
                    ri.code = "1000000001";
                }
                else if (_logic.CheckMobile(mobile))
                {
                    #region 验证code 通过后修改密码

                    var code = reqst.body.code.ToString();
                    var w = 8;
                    if (_recordLogic.CheckCode(code, w, mobile))
                    {
                        #region 修改密码

                        var pwd = reqst.body.newPwd.ToString();
                        try
                        {
                            _logic.ModifyPassword(mobile, pwd);

                            SendMsg(mobile);

                            ri.code = "1000000060";
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.Error(ex.ToString());
                            ri.code = "0";
                        }

                        #endregion
                    }
                    else
                    {
                        ri.code = "1000000011"; //验证码不存在或已过期
                    }

                    #endregion
                }
                else
                {
                    ri.code = "1000000010"; //未找到相关记录
                }
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
        ///     修改密码--解志辉
        /// </summary>
        /// <param name="reqst">
        ///     {"userId":"用户ID","orgPwd":"原始密码","newPwd":"新密码明文"}
        /// </param>
        /// <returns>ResultInfo&lt;System.String&gt;.</returns>

        [HttpPost]
        public ResultInfo<string> ModifyPassword(RequestParam<RequestModifyPass> reqst)
        {
            //todo 添加修改密码代码
            var ri = new ResultInfo<string>("99999");
            try
            {
                var orgPwd = reqst.body.orgPwd.ToString();
                var newPwd = reqst.body.newPwd.ToString();
                var userId = reqst.body.userId.ToString();

                var ent = _logic.SelectMemberByUserId(ConvertHelper.ParseValue(userId, 0));
                if (ent != null)
                {
                    #region 验证原始密码是否正确

                    if (!ent.password.Equals(EncryptHelper.Encrypt(orgPwd)))
                    {
                        ri.code = "1000000012";
                    }
                    else
                    {
                        #region 验证通过，修改密码

                        try
                        {
                            _logic.ModifyPassword(ent.mobile, newPwd);

                            SendMsg(ent.mobile);

                            ri.code = "1";
                        }
                        catch (Exception ex)
                        {
                            LoggerHelper.Error(ex.ToString());
                            ri.code = "0";
                        }

                        #endregion
                    }

                    #endregion
                }
                else
                {
                    ri.code = "1000000021";
                }


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
        /// 修改手机号--解志辉
        /// </summary>
        /// <param name="reqst">The reqst.</param>
        /// <returns>ResultInfo&lt;System.String&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-25 16:27:44
        [HttpPost]
        public ResultInfo<string> ModifyMobile(RequestParam<RequestValidateOrgMobileEntity> reqst)
        {
            var ri = new ResultInfo<string>("99999");
            try
            {
                var mobile = _logic.SelectUserMobileByUserId(reqst.body.userId);//当前用户手机号

                if (string.IsNullOrEmpty(reqst.body.mobile))
                {
                    ri.code = "1000000000";
                }
                else if (!ValidateHelper.IsHandset(reqst.body.mobile))
                {
                    ri.code = "1000000001";
                }
                else if (mobile != reqst.body.mobile)
                {
                    ri.code = "1000000013";
                }

                else
                {
                    #region 验证code 通过后修改密码

                    var code = reqst.body.code;
                    var w = 8;
                    if (_recordLogic.CheckCode(code, w, mobile))
                    {
                        //数据修改
                        ri.code = "1";

                    }
                    else
                    {
                        ri.code = "1000000011"; //验证码不存在或已过期
                    }

                    #endregion
                }
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


        ///// <summary>
        ///// 验证原始手机号接口--解志辉
        ///// </summary>
        ///// <param name="reqst">The reqst.</param>
        ///// <returns>ResultInfo&lt;System.String&gt;.</returns>
        /////  创 建 者：解志辉
        /////  创建日期：2016-05-25 16:29:13
        //public ResultInfo<string> ValidateOrgMobile(RequestParam<RequestValidateOrgMobileEntity> reqst)
        //{
        //    var ri = new ResultInfo<string>("99999");
        //    try
        //    {
        //        var mobile = _logic.SelectUserMobileByUserId(reqst.body.userId);//当前用户手机号

        //        if (string.IsNullOrEmpty(reqst.body.mobile))
        //        {
        //            ri.code = "1000000000";
        //        }
        //        else if (!ValidateHelper.IsHandset(reqst.body.mobile))
        //        {
        //            ri.code = "1000000001";
        //        }
        //        else if (mobile != reqst.body.mobile)
        //        {
        //            ri.code = "1000000013";
        //        }

        //        else
        //        {
        //            #region 验证code 通过后修改密码

        //            var code = reqst.body.code;
        //            var w = 8;
        //            if (_recordLogic.CheckCode(code, w, mobile))
        //            {

        //                ri.code = "1";

        //            }
        //            else
        //            {
        //                ri.code = "1000000011"; //验证码不存在或已过期
        //            }

        //            #endregion
        //        }
        //        ri.message = Settings.Instance.GetErrorMsg(ri.code);
        //        return ri;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.Error(ex.ToString());
        //        LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
        //        ri.message = Settings.Instance.GetErrorMsg(ri.code);
        //        return ri;
        //    }

        //}

        /// <summary>
        /// 验证用户是否实名--解志辉
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        public ResultInfo<int> IsRealName(RequestParam<RequestMemberDetail> reqst)
        {
            var ri = new ResultInfo<int>("99999");
            try
            {
                var ent = _logic.SelectMemberByUserId(reqst.body.userId);
                if (string.IsNullOrEmpty(ent.UsrCustId))
                {
                    ri.body = 0;
                }
                else
                {
                    ri.body = 1;
                }
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

        ///// <summary>
        ///// 验证绑定手机号接口--解志辉
        ///// </summary>
        ///// <param name="reqst">The reqst.</param>
        ///// <returns>ResultInfo&lt;System.String&gt;.</returns>
        /////  创 建 者：解志辉
        /////  创建日期：2016-05-25 16:34:22
        //public ResultInfo<string> CheckForBindMobile(RequestParam<RequestValidateOrgMobileEntity> reqst)
        //{
        //    var ri = new ResultInfo<string>("99999");
        //    try
        //    {
        //        if (string.IsNullOrEmpty(reqst.body.mobile))
        //        {
        //            ri.code = "1000000000";
        //        }
        //        else if (!ValidateHelper.IsHandset(reqst.body.mobile))
        //        {
        //            ri.code = "1000000001";
        //        }                
        //        else
        //        {
        //            #region 验证code 通过后修改密码
        //            var f = _logic.CheckMobileForBindMobile(reqst.body.mobile, reqst.body.userId);

        //            if (f)
        //            {
        //                //验证通过 进行下一步修改   
        //                ri.code = "1";
        //            }
        //            else
        //            {
        //                ri.code = "1000000002";

        //            }
        //            #endregion
        //        }
        //        ri.message = Settings.Instance.GetErrorMsg(ri.code);
        //        return ri;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.Error(ex.ToString());
        //        LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
        //        ri.message = Settings.Instance.GetErrorMsg(ri.code);
        //        return ri;
        //    }
        //}

        /// <summary>
        /// 获取特定会员基本信息--解志辉
        /// </summary>
        /// <param name="reqst">
        ///     {"userId":"用户ID"}
        /// </param>
        /// <returns>ResultInfo&lt;MemberEntity&gt;.</returns>
        [HttpPost]
        public ResultInfo<MemberWithRedpacketEntity> SelectMemberInformation(RequestParam<RequestMemberDetail> reqst)
        {
            //todo 添加会员基本信息代码

            var ri = new ResultInfo<MemberWithRedpacketEntity>("99999");
            try
            {
                var userId = reqst.body.userId.ToString();
                if (string.IsNullOrEmpty(userId))
                {
                    ri.code = "1000000000";
                }
                else
                {
                    ri.code = "1";
                    var ent = _logic.SelectMemberWithRedpacketByUserId(ConvertHelper.ParseValue(userId, 0));
                    if (ent == null)
                    {
                        ri.code = "1000000015";
                    }
                    ri.body = ent;
                }
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }

        /// <summary>
        /// 获取用户可用余额--解志辉
        /// <remark>用户投资后,刷新账户余额</remark>
        /// </summary>
        /// <param name="reqst">The reqst.</param>
        /// <returns>ResultInfo&lt;MemberInvestEntity&gt;.</returns>
        [HttpPost]
        public ResultInfo<MemberInvestEntity> SelectBalance(RequestParam<RequestMemberDetail> reqst)
        {
            var ri = new ResultInfo<MemberInvestEntity>("99999");
            try
            {
                int userId = ConvertHelper.ParseValue(reqst.body.userId.ToString(), 0);
                ri.body = bonusLogic.SelectBalance(userId);
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
        /// 获取用户 余额+优惠券--解志辉 新
        /// </summary>
        /// <param name="reqst">The reqst.</param>
        /// <returns>ResultInfo&lt;MemberInvestEntity&gt;.</returns>
        [HttpPost]
        public ResultInfo<ActivityBonusEntity> SelectBonus(RequestParam<RequestMemberBonus> reqst)
        {
            var ri = new ResultInfo<ActivityBonusEntity>("99999");
            try
            {
                int userId = ConvertHelper.ParseValue(reqst.body.userId.ToString(), 0);
                ri.body = bonusLogic.SelectBonus(userId, reqst.body.selectedIds);
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
        /// 获取用户可用的代金券列表--刘佳
        /// <remark>APP投资页调用此接口</remark>
        /// </summary>
        /// <param name="reqst">The reqst.</param>
        /// <returns>ResultInfo&lt;MemberInvestEntity&gt;.</returns>
        [HttpPost]
        public ResultInfo<ActivityBonusEntity> SelectActivity(RequestParam<RequestMemberBonus> reqst)
        {
            var ri = new ResultInfo<ActivityBonusEntity>("99999");
            try
            {
                int userId = ConvertHelper.ParseValue(reqst.body.userId.ToString(), 0);
                ri.body = bonusLogic.SelectActivity(userId, reqst.body.selectedIds);
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
        ///     获取用户奖励(优惠券 代金券)列表--解志辉.
        /// </summary>
        /// <param name="reqst">
        ///     {"userId":"用户ID","pageIndex":"当前页数", "pageSize":"每页条数"}
        /// </param>
        /// <returns>ResultInfo&lt;BasePage&lt;List&lt;BonusEntity&gt;&gt;&gt;.</returns>

        [HttpPost]
        public ResultInfo<BasePage<List<BonusEntity>>> SelectBonuses(RequestParam<RequestBonus> reqst)
        {
            var ri = new ResultInfo<BasePage<List<BonusEntity>>>("99999");
            try
            {
                int userId = ConvertHelper.ParseValue(reqst.body.userId.ToString(), 0);
                int rewardState = 0;

                try
                {
                    rewardState = ConvertHelper.ParseValue(reqst.body.rewardState.ToString(), 0);
                }
                catch (Exception ex)
                {
                    rewardState = 0;
                    LoggerHelper.Error(ex.Message);
                }


                int pageIndex = ConvertHelper.ParseValue(reqst.body.pageIndex.ToString(), 0);
                int pageSize = ConvertHelper.ParseValue(reqst.body.pageSize.ToString(), 0);

                ri.code = "1";

                ri.body = bonusLogic.SelectBonuses(pageIndex, pageSize, userId, rewardState);


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
        /// 获取特定用户已拥有的代金券列表--王雪松
        /// <remark>个人中心-奖励券界面调用此接口</remark>
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<BasePage<List<OwnBonusEntity>>> SelectPersonalBonuses(RequestParam<RequestOwnBonus> reqst)
        {
            var ri = new ResultInfo<BasePage<List<OwnBonusEntity>>>("99999");
            try
            {
                int userId = ConvertHelper.ParseValue(reqst.body.userId.ToString(), 0);
                int pageIndex = ConvertHelper.ParseValue(reqst.body.pageIndex.ToString(), 0);
                int pageSize = ConvertHelper.ParseValue(reqst.body.pageSize.ToString(), 0);
                ri.code = "1";
                ri.body = bonusLogic.SelectOwnBonuses(pageIndex, pageSize, userId);
                if (ri.body == null || ri.body.rows == null || ri.body.rows.Count < 1)
                {
                    ri.code = "1000000010";
                    ri.body = null;
                }
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
        /// 获取现特定代金卷的详情--王雪松
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<OwnBonusDetailEntity> SelectPersonalBonusesDetail(RequestParam<RequestBonusDetail> reqst)
        {
            var ri = new ResultInfo<OwnBonusDetailEntity>("99999");
            try
            {
                int userAct = ConvertHelper.ParseValue(reqst.body.userAct.ToString(), 0);
                ri.code = "1";
                ri.body = bonusLogic.SelectOwnBonusesDetail(userAct);
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
        /// 获取用户常用地址--解志辉
        /// </summary>
        /// <param name="reqst">The reqst.</param>
        /// <returns>ResultInfo&lt;NormalAreaEntity&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-26 16:45:10
        [HttpPost]
        public ResultInfo<ResponseUserAddressEntity> SelectUserAddress(RequestParam<RequestMemberDetail> reqst)
        {
            var ri = new ResultInfo<ResponseUserAddressEntity>("99999");
            try
            {
                int userId = ConvertHelper.ParseValue(reqst.body.userId.ToString(), 0);


                ri.body = bonusLogic.SelectUserAddress(userId);
                if (ri.body == null)
                {
                    ri.code = "1000000010";
                }
                else
                {
                    ri.code = "1";
                }

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
        /// 保存用户常用地址数据--解志辉
        /// </summary>
        /// <returns>ResultInfo&lt;System.Int32&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-26 15:03:37
        public ResultInfo<int> SaveAddress(RequestParam<RequestRegionEntity> reqst)
        {
            var ri = new ResultInfo<int>("99999");
            try
            {
                var userId = ConvertHelper.ParseValue(reqst.body.userId, 0);
                var provId = ConvertHelper.ParseValue(reqst.body.provinceId, 0);
                var cityId = ConvertHelper.ParseValue(reqst.body.cityId, 0);
                var contId = ConvertHelper.ParseValue(reqst.body.countyId, 0);
                var det = reqst.body.detailAddress;
                var userName = reqst.body.userName;
                var mobile = reqst.body.mobile;
                var zipCode = reqst.body.zipCode;

                if (userId == 0)
                {
                    ri.code = "1000000015";
                }
                else if (string.IsNullOrEmpty(userName))
                {
                    ri.code = "1000000007";
                }
                else if (string.IsNullOrEmpty(mobile))
                {
                    ri.code = "1000000000";
                }
                else if (!ValidateHelper.IsHandset(mobile))
                {
                    ri.code = "1000000001";
                }
                else if (provId == 0)
                {
                    ri.code = "1000000016";
                }
                else if (cityId == 0)
                {
                    ri.code = "1000000017";
                }
                else if (contId == 0)
                {
                    ri.code = "1000000018";
                }
                else if (string.IsNullOrEmpty(det))
                {
                    ri.code = "1000000019";
                }
                else if (det.Length > 100)
                {
                    ri.code = "1000000020";
                }
                else
                {
                    #region 保存数据

                    var res = _logic.SaveAddress(userId, provId, cityId, contId, det, userName, mobile, zipCode);
                    if (res > 0)
                    {
                        ri.code = "1";
                    }
                    else
                    {
                        switch (res)
                        {
                            case -500:
                                {

                                    ri.code = "1000000016";
                                }
                                break;
                            case -400:
                                {

                                    ri.code = "1000000017";
                                }
                                break;
                            case -300:
                                {

                                    ri.code = "1000000018";
                                }
                                break;
                            case -200:
                                {
                                    ri.code = "1000000015";
                                }
                                break;
                        }
                    }
                    #endregion
                }
                LoggerHelper.Error("保存用户地址数据 message:" + ri.message + "; zipCode:" + zipCode + ";provId: " + provId + " ;cityId: " + cityId + " ; contId:" + contId + " ; det:" + det + " ;userName:" + userName + "; mobile :" + mobile + "; zipCode" + zipCode);
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



        public ResultInfo<int> Test(RequestParam<RequestMember> reqst)
        {
            try
            {

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
                    BgRetUrl = ckd + ("/Test/Index/BgRetUrlForUserRegister"),
                    RetUrl = ckd + ("/Test/Index/CallbackForUserRegister"),
                    UsrMp = p.mobile,
                    UsrEmail = p.email,
                    UsrId = p.username
                };
                StringBuilder chkVal = new StringBuilder();
                chkVal.Append(m.Version);
                chkVal.Append(m.CmdId);
                chkVal.Append(m.MerCustId);
                chkVal.Append(m.BgRetUrl);
                chkVal.Append(m.RetUrl);
                chkVal.Append(m.UsrId);
                chkVal.Append(m.UsrMp);
                chkVal.Append(m.UsrEmail);
                string chkv = chkVal.ToString();
                //私钥文件的位置(这里是放在了站点的根目录下)
                string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.MerPr;
                //需要指定提交字符串的长度
                int len = Encoding.UTF8.GetBytes(chkv).Length;
                StringBuilder sbChkValue = new StringBuilder(256);
                //加签
                DllInterop.SignMsg(m.MerId, merKeyFile, chkv, len, sbChkValue);
                m.ChkValue = sbChkValue.ToString();

                var values = new NameValueCollection
                {
                    {"Version", m.Version},
                    {"CmdId", m.CmdId},
                    {"MerCustId", m.MerCustId},
                    {"BgRetUrl", m.BgRetUrl},
                    {"RetUrl", m.RetUrl},
                    {"UsrId", m.UsrId},
                    {"UsrMp", m.UsrMp},
                    {"UsrEmail", m.UsrEmail},
                    {"MerPriv", m.MerPriv},
                    {"ChkValue", m.ChkValue}
                };
                HttpHelper.Post(Settings.Instance.ChinapnrUrl, values);

            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
            }


            return null;
        }


        /// <summary>
        ///     发送密码修改成功短信--解志辉
        /// </summary>
        /// <param name="mobile">手机号</param>
        protected void SendMsg(string mobile)
        {
            #region 发送密码修改成功短信

            var smstype = (int)Enum.Parse(typeof(SmsType), SmsType.修改密码.ToString());
            var messageId = 17; //密码修改成功
            var smsEntity = _logic.GetSmsEmailEntity(1, messageId); // 获取密码修改成功内容
            if (smsEntity != null)
            {
                var cnt = smsEntity.SEContext.Replace("#DATATIME#", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //  var rx = SmsHelper.Send(mobile, cnt);
                var pm = new SmsRecordEntity
                {
                    phone_number = mobile,
                    sendtime = DateTime.Now,
                    senduserid = 0,
                    smstype = smstype,
                    smscontext = cnt,
                    orderid = 1,
                    vcode = "",
                    ip = Settings.Instance.ClientIp
                };
                _recordLogic.AddRecord(pm);
            }

            #endregion
        }
    }
}