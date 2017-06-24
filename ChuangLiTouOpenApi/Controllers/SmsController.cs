using System;
using System.Text;
using System.Web.Http;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Sms;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.SmsEmail;
using ChuanglitouP2P.Common.Util;
using ChuanglitouP2P.BLL.Api;
using ChuanglitouP2P.Common;
namespace ChuangLiTouOpenApi.Controllers
{
    /// <summary>
    /// 短信相关接口
    /// </summary>
    public class SmsController : ApiController
    {
        private readonly MemberLogic _logic;

        private readonly SmsRecordLogic _recordLogic;

        public SmsController(MemberLogic logic, SmsRecordLogic recordLogic)
        {
            _logic = logic;
            _recordLogic = recordLogic;
        }

        /// <summary>
        /// 获取注册验证码短信--解志辉
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<string> GetRegistCode(RequestParam<SmsEntity> reqst)
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
                    // 手机号可以注册用户 
                    var res = SendSms(mobile, 1, 8);
                    return res;
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
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }

        /// <summary>
        ///     获取修改密码验证码短信--解志辉
        /// </summary>
        /// <returns>ResultInfo&lt;System.String&gt;.</returns>
        [HttpPost]
        public ResultInfo<string> GetModifyPwdCode(RequestParam<SmsEntity> reqst)
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
                    ri.code = "1000000021"; //未找到相关记录 
                }
                else
                {
                    #region 发送修改密码短信

                    var res = SendSms(mobile, 1, 16);
                    return res;

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
        ///// 获取绑定手机号验证码短信接口--解志辉
        ///// </summary>
        ///// <param name="reqst">The reqst.</param>
        ///// <returns>ResultInfo&lt;System.String&gt;.</returns>
        /////  创 建 者：解志辉
        /////  创建日期：2016-05-25 16:25:37
        //[HttpPost]
        //public ResultInfo<string> GetBindMobileCode(RequestParam<SmsEntity> reqst)
        //{
        //    var ri = new ResultInfo<string>("99999");
        //    try
        //    {
        //        var mobile = reqst.body.mobile;

        //        if (string.IsNullOrEmpty(mobile))
        //        {
        //            ri.code = "1000000000";
        //        }
        //        else if (!ValidateHelper.IsHandset(mobile))
        //        {
        //            ri.code = "1000000001";
        //        }
        //        else if (!_logic.CheckMobile(mobile))
        //        {
        //            ri.code = "1000000010"; //未找到相关记录 
        //        }
        //        else
        //        {
        //            #region 发送修改密码短信

        //            var res = SendSms(mobile, 1, 25);
        //            return res;

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
        ///// <summary>
        ///// 获取修改手机号验证码短信接口--解志辉
        ///// </summary>
        ///// <param name="reqst">The reqst.</param>
        ///// <returns>ResultInfo&lt;System.String&gt;.</returns>
        /////  创 建 者：解志辉
        /////  创建日期：2016-05-25 16:20:03
        //[HttpPost]
        //public ResultInfo<string> GetModifyMobileCode(RequestParam<RequestBindMobileEntity> reqst)
        //{
        //    var ri = new ResultInfo<string>("99999");
        //    try
        //    {
        //        var userId = ConvertHelper.ParseValue(reqst.body.userId, 0);
        //        var mobile = _logic.SelectUserMobileByUserId(userId);//当前用户手机号

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
        //            #region 发送短信

        //            var res = SendSms(mobile, 1,35);
        //            return res;

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
        ///  发送短信--解志辉
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="type"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        protected ResultInfo<string> SendSms(string mobile, int type, int messageId)
        {
            var ri = new ResultInfo<string>("99999");
            try
            {
                #region 防止短信过度频繁发送

                #region First 60之内不能再次发送短信

                var cacheKey = "reg_member_get_code_time_" + mobile;
                var cv = CacheHelper.GetSystemCache(cacheKey);

                if (cv == null)
                {
                    CacheHelper.SetCache(cacheKey, DateTime.Now, 1);
                }
                else
                {
                    var dte = ConvertHelper.ParseValue(cv, DateTime.MinValue);
                    if (dte != DateTime.MinValue)
                    {
                        var sec = Settings.Instance.DateDiff("Second", dte, DateTime.Now);
                        if (sec > 60)
                        {
                            CacheHelper.ClearCache(cacheKey);
                        }
                        else
                        {
                            ri.code = "1000000005"; //短信发送太频繁!请稍后再试
                        }
                    }
                }

                #endregion

                #region Second 同IP间隔60S之后才能再次发送 同时一个IP最多发送8条注册短信

                var t1 = (int)Enum.Parse(typeof(SmsType), SmsType.短信验证码.ToString());

                var t2 = (int)Enum.Parse(typeof(SmsType), SmsType.语音短信验证码.ToString());
                if (!_recordLogic.CheckInOneMinute(Settings.Instance.ClientIp, t1, t2))
                {
                    ri.code = "1000000005"; //短信发送太频繁!请稍后再试
                }

                #endregion

                #region Third 同一IP最多发送8条同类型短信

                if (_recordLogic.CheckIpSendTimes(Settings.Instance.ClientIp, t1, t2) >= 8)
                {
                    ri.code = "1000000006"; //短信发送太频繁!请与客服联系
                }

                #endregion

                #endregion

                #region 验证3分钟之内有没有验证码记录，有则发送

                var ent = _recordLogic.SelectHistory(mobile, t1, t2);
                if (ent != null)
                {
                    if (ent.hits < 4)
                    {
                        ent.orderid = SendSMS.Send(mobile, ent.smscontext);
                        _recordLogic.UpdateRecord(ent);
                    }
                    else
                    {
                        ri.code = "1000000006"; //短信发送太频繁!请与客服联系
                    }
                }
                else //不存在记录，生成新验证码
                {
                    #region 生成验证码 发送短信并记录发送日志

                    var vcode = Utils.RndNum(6);
                    var ebt = _logic.GetSmsEmailEntity(type, messageId); // 获取验证码邮件内容
                    var sbsms = new StringBuilder(ebt.SEContext);

                    sbsms = sbsms.Replace("#CODE#", vcode);

                    var pm = new SmsRecordEntity
                    {
                        phone_number = mobile,
                        sendtime = DateTime.Now,
                        senduserid = 0,
                        smstype = t1,
                        smscontext = sbsms.ToString(),
                        orderid = SendSMS.Send(mobile, sbsms.ToString()),
                        vcode = vcode,
                        ip = Settings.Instance.ClientIp
                    };
                    _recordLogic.AddRecord(pm);

                    #endregion

                    ri.code = "1000000099";
                    ri.body = vcode;
                }

                #endregion

                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error("send phone:" + mobile + "type:" + type + " messageId:" + messageId); ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }
    }
}