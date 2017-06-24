using ChuanglitouP2P.Common;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Borrow;
using ChuangLiTou.Core.Entities.Response;
using ChuanglitouP2P.Model.Invest;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.BLL.Api;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.BLL.EF;
using ChuangLiTouOpenApi.Factory;
using ChuanglitouP2P.Model.chinapnr.InitiativeTender;

namespace ChuangLiTouOpenApi.Areas.MemberCenter.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class InvestSubmitController : BaseController
    {
        // GET: MemberCenter/InvestSubmit
        /// <summary>
        /// 确认投资页面
        /// </summary>
        /// <param name="reqst"></param>
        /// <param name="projectTypeId">标的类型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(RequestParam<RequestTender> reqst)
        {
            LoggerHelper.Info(JsonHelper.Entity2Json(reqst));
            ResultInfo<string> res = new ResultInfo<string>("99999");
            var usrId = ConvertHelper.ParseValue(reqst.body.userId, 0);
            var targetId = ConvertHelper.ParseValue(reqst.body.targetId, 0);
            var bds = string.Empty;

            B_borrowing_target b_borrowing_target = new B_borrowing_target();
            var m_borrowing_target = b_borrowing_target.GetModel(targetId);
            B_member_table b_member_table = new B_member_table();
            var m_member_table = b_member_table.GetModel(usrId);


            if (m_borrowing_target == null || m_member_table == null)
            {
                res.code = "1000000010";
                res.message = Settings.Instance.GetErrorMsg(res.code);
                return View(res);
            }
            if (!string.IsNullOrWhiteSpace(reqst.body.bonusIds))
            {
                bds = reqst.body.bonusIds;
            }
            if (!string.IsNullOrWhiteSpace(reqst.body.addRateIds))
            {
                if (!string.IsNullOrWhiteSpace(bds))
                    bds += ",";
                bds = reqst.body.addRateIds;
            }
            var investAmount = ConvertHelper.ParseValue(reqst.body.investAmount, 0M);
            BorrowLogic _logic = new BorrowLogic();
            var ent = _logic.SelectBorrowDetail(targetId);
            if (usrId <= 0)
            {
                res.code = "1000000015";
                res.message = Settings.Instance.GetErrorMsg(res.code);
                return View(res);
            }
            if (targetId <= 0)
            {
                res.code = "1000000014";
                res.message = Settings.Instance.GetErrorMsg(res.code);
                return View(res);
            }
            if (investAmount < 100)
            {
                res.code = "2000000000";
                res.message = Settings.Instance.GetErrorMsg(res.code);
                return View(res);
            }
            if (m_borrowing_target.minimum > 0 && investAmount < m_borrowing_target.minimum)
            {
                res.code = "2000000008";
                res.message = Settings.Instance.GetErrorMsg(res.code);
                return View(res);
            }
            if (m_borrowing_target.maxmum > 0 && investAmount > m_borrowing_target.maxmum)
            {
                res.code = "2000000009";
                res.message = Settings.Instance.GetErrorMsg(res.code);
                return View(res);
            }

            if (reqst.body.typeId == 6)//新手标判定
            {
                if (!string.IsNullOrWhiteSpace(bds))
                {
                    res.code = "2000000010";
                    res.message = Settings.Instance.GetErrorMsg(res.code);
                    return View(res);
                }
                if (m_member_table.useridentity != 5)
                {
                    if (Convert.ToDateTime(m_member_table.Registration_time.ToString("yyyy-MM-dd")).AddDays(30) < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                        res.code = "2000000011";
                        res.message = Settings.Instance.GetErrorMsg(res.code);
                        return View(res);
                    }
                    B_Bid_records b_Bid_records = new B_Bid_records();
                    int investedCount = b_Bid_records.GetInvestCount(reqst.body.userId, reqst.body.targetId);
                    if (investedCount >= 3)
                    {
                        res.code = "2000000012";
                        res.message = Settings.Instance.GetErrorMsg(res.code);
                        return View(res);
                    }
                    if (ent.start_time != null && ent.start_time.Value < Convert.ToDateTime("2016-12-01 00:00:00"))
                    {
                        res.code = "2999999999";
                        res.message = Settings.Instance.GetErrorMsg(res.code);
                        return View(res);
                    }
                    if (ent.project_type_id != null && ent.project_type_id.Value != 6)
                    {
                        res.code = "2999999999";
                        res.message = Settings.Instance.GetErrorMsg(res.code);
                        return View(res);
                    }
                }
            }
            try
            {

                decimal vocheramttemp = GetUseRewards(bds, usrId);
                B_member_table b = new B_member_table();
                M_member_table user = new M_member_table();
                user = b.GetModel(reqst.body.userId);
                InvestmentParameters mp = new InvestmentParameters
                {
                    Amount = investAmount,
                    Circle = ConvertHelper.ParseValue(ent.life_of_loan, 0),
                    CircleType = ConvertHelper.ParseValue(ent.unit_day, 0),
                    NominalYearRate = ConvertHelper.ParseValue(ent.annual_interest_rate, 0D),
                    OverheadsRate = 0f,
                    RepaymentMode = ConvertHelper.ParseValue(ent.payment_options, 0),
                    RewardRate = 0f,
                    IsThirtyDayMonth = false,
                    InvestDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
                    ReleaseDate = DateTime.Parse(ent.release_date.ToString()).ToString("yyyy-MM-dd"),
                    Investmentenddate = DateTime.Parse(DateTime.Parse(ent.repayment_date.ToString()).ToString("yyyy-MM-dd")),
                    Payinterest = ConvertHelper.ParseValue(ent.month_payment_date, 0),
                    InvestObject = 1
                };
                //追加  加息券
                if (!string.IsNullOrWhiteSpace(bds))
                {
                    B_UserAct bUserAct = new B_UserAct();
                    var addRateModel = bUserAct.GetAddRateModel(bds);
                    if (addRateModel != null)// add rate of year
                        mp.NominalYearRate = mp.NominalYearRate + Convert.ToDouble(addRateModel.Amt.Value);
                }
                List<InvestmentReceiveRecordInfo> records = InvestCalculator.CalculateReceiveRecord(mp);
                StringBuilder sb = new StringBuilder("");
                if (records != null && records.Any())
                {
                    int i = 1;
                    foreach (var item in records)
                    {
                        sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7}|", i, item.interestvalue_date, item.NominalReceiveDate, item.Balance, item.Interest, item.Principal, item.TotalInstallments, item.TotalDays);
                        i = i + 1;
                    }
                }
                var orderId = Settings.Instance.OrderCode;
                var frozenidNo = Settings.Instance.OrderCode;
                decimal frozenAmount = 0.00M;

                var xf = ((records != null && records.Any()) ? records.Sum(t => t.Interest) : 0M);
                //LoggerHelper.Error("利息：" + xf + " ------- " + JsonHelper.Entity2Json(records));
                string invitationCode = string.Empty;
                using (InvitationLogic invitationLogic = new InvitationLogic())
                {
                    var invitationModel = invitationLogic.GetUserInvited(usrId);
                    if (invitationModel != null)
                        invitationCode = invitationModel.invcode;
                }
                var resVal = _logic.SubmitTender(usrId, targetId, investAmount, bds, invitationCode, orderId
                    , xf, frozenidNo
                    , investAmount - frozenAmount
                    , ((records != null && records.Any()) ? records.Count : 0), sb.ToString());
                if (resVal < 200)
                {
                    switch (resVal)
                    {
                        case -100:
                            {
                                res.code = "2000000001";
                            }
                            break;
                        case -101:
                            {
                                res.code = "2000000008";
                            }
                            break;
                        case -200:
                            {
                                res.code = "2000000002";
                            }
                            break;
                        case -300:
                            {
                                res.code = "2000000003";
                            }
                            break;
                        case -400:
                            {
                                res.code = "2000000004";
                            }
                            break;
                        case -500:
                            {
                                res.code = "2000000007";
                            }
                            break;
                        case -600:
                            {
                                res.code = "2000000006";
                            }
                            break;
                    }
                }
                else
                {
                    LoggerHelper.Error("SubmitTender！！！！！！！！！！！！！！！投资开始");
                    LoggerHelper.Info("投资开始");
                    #region 投资成功
                    M_InitiativeTender Mt = new M_InitiativeTender();
                    Mt.Version = "20";
                    Mt.CmdId = "InitiativeTender";
                    Mt.MerCustId = Settings.Instance.MerCustId;
                    Mt.OrdId = orderId;
                    Mt.OrdDate = mp.InvestDate.ToString("yyyyMMdd");
                    Mt.TransAmt = investAmount.ToString("0.00");
                    Mt.UsrCustId = user.UsrCustId;
                    Mt.MaxTenderRate = "0.20";

                    TenderJosnPro mtp = new TenderJosnPro();
                    mtp.BorrowerCustId = b.GetModel(ConvertHelper.ParseValue(ent.borrower_registerid, 0)).UsrCustId;
                    mtp.BorrowerAmt = investAmount.ToString("0.00");

                    // mtp.BorrowerRate = decimal.Parse( dt.Rows[0]["loan_management_fee"].ToString()).ToString("0.00");

                    mtp.BorrowerRate = "1.00"; //风控范围

                    mtp.ProId = targetId.ToString();

                    Mt.BorrowerDetails = "[" + FastJSON.toJOSN(mtp) + "]";

                    #region 此处判断优惠类型

                    #endregion
                    Mt.IsFreeze = "Y";

                    Mt.FreezeOrdId = frozenidNo;

                    Mt.RetUrl = Settings.Instance.GetCallbackUrl("/MemberCenter/InvestSubmit/CallbackRetUrl");

                    Mt.BgRetUrl = Settings.Instance.GetCallbackUrl("/MemberCenter/InvestSubmit/CallbackBgRetUrl");

                    if (!string.IsNullOrWhiteSpace(reqst.body.bonusIds))
                    {
                        Mt.MerPriv = bds;
                        TenderAccPro ret = new TenderAccPro();
                        ret.AcctId = ChuanglitouP2P.Common.Utils.GetMERDT();
                        ret.VocherAmt = vocheramttemp.ToString("0.00");
                        Mt.ReqExt = "{" + "\"Vocher\":" + FastJSON.toJOSN(ret) + "}";

                    }
                    else
                    {
                        Mt.MerPriv = reqst.body.addRateIds;
                    }

                    //append device code to comment fields for make sure it be transfered back
                    string temp = Mt.MerPriv;
                    AppendDeviceFlag(reqst.header.appId.ToString(), ref temp);
                    Mt.MerPriv = temp;

                    LoggerHelper.Info("优惠券使用的id:" + bds);
                    StringBuilder chkVal = new StringBuilder();
                    chkVal.Append(Mt.Version);
                    chkVal.Append(Mt.CmdId);
                    chkVal.Append(Mt.MerCustId);
                    chkVal.Append(Mt.OrdId);
                    chkVal.Append(Mt.OrdDate);
                    chkVal.Append(Mt.TransAmt);
                    chkVal.Append(Mt.UsrCustId);
                    chkVal.Append(Mt.MaxTenderRate);
                    chkVal.Append(Mt.BorrowerDetails);
                    chkVal.Append(Mt.IsFreeze);
                    chkVal.Append(Mt.FreezeOrdId);
                    chkVal.Append(Mt.RetUrl);
                    chkVal.Append(Mt.BgRetUrl);
                    chkVal.Append(Mt.MerPriv);
                    chkVal.Append(Mt.ReqExt);

                    string chkv = chkVal.ToString();
                    LoggerHelper.Info("投资：" + chkv);
                    //私钥文件的位置(这里是放在了站点的根目录下)
                    string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.MerPr;
                    //需要指定提交字符串的长度
                    int len = Encoding.UTF8.GetBytes(chkv).Length;
                    StringBuilder sbChkValue = new StringBuilder(256);
                    //加签
                    int str = DllInterop.SignMsg(Settings.Instance.MerId, merKeyFile, chkv, len, sbChkValue);

                    LoggerHelper.Info(str);

                    Mt.ChkValue = sbChkValue.ToString();
                    if (str == 0)
                    {
                        var strz = new StringBuilder();

                        strz.Append(" <form id=\"formauto\" name=\"formauto\"  action=\"" + Settings.Instance.ChinapnrUrl + "\" method=\"post\">");

                        strz.Append("<input id=\"Version\"  name=\"Version\"  type=\"hidden\"  value=\"" + Mt.Version + "\" />");

                        strz.Append("<input id=\"CmdId\"  name=\"CmdId\"    type=\"hidden\"  value=\"" + Mt.CmdId + "\" />");

                        strz.Append("<input id=\"MerCustId\" name=\"MerCustId\"   type=\"hidden\"  value=\"" + Mt.MerCustId + "\" />");

                        strz.Append("<input id=\"OrdId\" name=\"OrdId\" type=\"hidden\"  value=\"" + Mt.OrdId + "\" />");

                        strz.Append("<input id=\"OrdDate\" name=\"OrdDate\" type=\"hidden\"  value=\"" + Mt.OrdDate + "\" />");

                        strz.Append("<input id=\"TransAmt\" name=\"TransAmt\" type=\"hidden\"  value=\"" + Mt.TransAmt + "\" />");

                        strz.Append("<input id=\"UsrCustId\"  name=\"UsrCustId\" type=\"hidden\"  value=\"" + Mt.UsrCustId + "\" />");

                        strz.Append("<input id=\"MaxTenderRate\"   name=\"MaxTenderRate\" type=\"hidden\"  value=\"" + Mt.MaxTenderRate + "\" />");

                        strz.Append("<input id=\"BorrowerDetails\" name=\"BorrowerDetails\" type=\"hidden\"  value=" + Mt.BorrowerDetails + " />");

                        strz.Append("<input id=\"IsFreeze\" name=\"IsFreeze\" type=\"hidden\"  value=\"" + Mt.IsFreeze + "\" />");
                        strz.Append("<input id=\"FreezeOrdId\" name=\"FreezeOrdId\" type=\"hidden\"  value=\"" + Mt.FreezeOrdId + "\" />");
                        strz.Append("<input id=\"RetUrl\" name=\"RetUrl\" type=\"hidden\"  value=\"" + Mt.RetUrl + "\" />");
                        strz.Append("<input id=\"BgRetUrl\" name=\"BgRetUrl\" type=\"hidden\"  value=\"" + Mt.BgRetUrl + "\" />");
                        strz.Append("<input id=\"MerPriv\" name=\"MerPriv\" type=\"hidden\"  value=\"" + Mt.MerPriv + "\" />");
                        strz.Append("<input id=\"ReqExt\" name=\"ReqExt\" type=\"hidden\"  value=" + Mt.ReqExt + " >");
                        strz.Append("<input id=\"ChkValue\" name=\"ChkValue\" type=\"hidden\"  value=\"" + Mt.ChkValue + "\" />");
                        strz.Append(" </form>");
                        strz.Append("<script type=\"text/javascript\">document.getElementById('formauto').submit();</script>");

                        LoggerHelper.Info("投资表单：" + strz.ToString());
                        res.code = "1";
                        res.body = strz.ToString();
                    }
                    else
                    {
                        res.code = "5000000000";
                    }
                    #endregion
                }
                res.message = Settings.Instance.GetErrorMsg(res.code);
                return View(res);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst));
                res.code = "500";
                res.message = Settings.Instance.GetErrorMsg(res.code);
                return View(res);
            }
        }

        /// <summary>
        /// 投资成功，汇付回调页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CallbackRetUrl()
        {
            ViewBag.UserId = 0;
            ReInitiativeTender p = new ReInitiativeTender();
            B_usercenter BUC = new B_usercenter();
            string OrdId = "", sql = "";
            int useridc = 0;
            string targetid = "0";
            p.CmdId = DNTRequest.GetString("CmdId");
            p.RespCode = DNTRequest.GetString("RespCode");
            p.RespDesc = HttpUtility.UrlDecode(DNTRequest.GetString("RespDesc"));
            p.MerCustId = DNTRequest.GetString("MerCustId");
            p.OrdId = DNTRequest.GetString("OrdId");
            p.OrdDate = DNTRequest.GetString("OrdDate");
            p.TransAmt = DNTRequest.GetString("TransAmt");
            p.UsrCustId = DNTRequest.GetString("UsrCustId");
            p.TrxId = DNTRequest.GetString("TrxId");
            p.IsFreeze = DNTRequest.GetString("IsFreeze");
            p.FreezeOrdId = DNTRequest.GetString("FreezeOrdId");
            p.FreezeTrxId = DNTRequest.GetString("FreezeTrxId");
            p.RetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("RetUrl"));
            p.BgRetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("BgRetUrl"));
            string merp = DNTRequest.GetString("MerPriv");
            if (merp.Length > 0)
            {
                p.MerPriv = HttpUtility.UrlDecode(merp);
            }
            else
            {
                p.MerPriv = merp;
            }
            p.RespExt = HttpUtility.UrlDecode(DNTRequest.GetString("RespExt"));
            p.ChkValue = DNTRequest.GetString("ChkValue");
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(p.CmdId);
            chkVal.Append(p.RespCode);
            chkVal.Append(p.MerCustId);
            chkVal.Append(p.OrdId);
            chkVal.Append(p.OrdDate);
            chkVal.Append(p.TransAmt);
            chkVal.Append(p.UsrCustId);
            chkVal.Append(p.TrxId);
            chkVal.Append(p.IsFreeze);
            chkVal.Append(p.FreezeOrdId);
            chkVal.Append(p.FreezeTrxId);
            chkVal.Append(p.RetUrl);
            chkVal.Append(p.BgRetUrl);
            chkVal.Append(p.MerPriv);
            chkVal.Append(p.RespExt);

            //   LoggerHelper.Info("投标RespCode" + p.RespCode + "RespExt:" + p.RespExt);
            string chkv = chkVal.ToString();

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.PgPubk;

            int ret = DllInterop.VeriSignMsg(merKeyFile, chkv, chkv.Length, p.ChkValue);

            // LoggerHelper.Info("验签：" + ret.ToString());

            LoggerHelper.Info("前台主动投标返回报文:" + JsonHelper.Entity2Json(p));
            int invcount = 0; //记录用户是否为首次投资,汲及到邀请人的操作
            merp = p.MerPriv;
            string deviceKey = PickoutDeviceFlag(ref merp);//pick out device code from comment field
            p.MerPriv = merp;
            int registerid = 0;
            #region 验签
            if (ret == 0)
            {
                if (p.RespCode == "000" || p.RespCode == "534" || p.RespCode == "360" || p.RespCode == "099")//p.RespCode == "322" ||
                {
                    string cachename = p.OrdId + "Invest" + p.UsrCustId;

                    //LoggerHelper.Info("cachename:" + cachename);
                    if (Settings.Instance.GeTThirdCache(cachename) == 0)
                    {
                        Settings.Instance.SetThirdCache(cachename);
                        if (p.FreezeTrxId != "")
                        {
                            sql = "select ordstate  from hx_Bid_records  where ordstate =0 and  OrdId='" + p.OrdId + "'";
                            LoggerHelper.Info("CallbackRetUrl===p.OrdId:" + p.OrdId);
                            DataTable dts = DbHelper.Query(sql).Tables[0];
                            if (dts.Rows.Count > 0)
                            {
                                //同步处理用户金额
                                int d = BUC.ReInvest_success(p.UsrCustId, p.FreezeOrdId, p.TransAmt, p.FreezeTrxId, p.OrdId, p.MerPriv);
                                LoggerHelper.Info("返回唯一冻结标识:" + p.FreezeTrxId + "事务执行结果:" + d);
                                if (d > 0)
                                {
                                    sql = "select targetid,bid_records_id, borrowing_title,investor_registerid ,username,mobile,invitationcode,investment_amount,life_of_loan,unit_day,borrowing_balance,bonusAmt,registerid  from  V_hx_Bid_records_borrowing_target where OrdId='" + p.OrdId + "'";
                                    DataTable dt = DbHelper.Query(sql).Tables[0];
                                    if (dt.Rows.Count > 0)
                                    {
                                        decimal invdesc = decimal.Parse(dt.Rows[0]["investment_amount"].ToString());
                                        OrdId = p.OrdId;
                                        useridc = int.Parse(dt.Rows[0]["investor_registerid"].ToString());
                                        targetid = dt.Rows[0]["targetid"].ToString();
                                        registerid = Convert.ToInt32(dt.Rows[0]["registerid"]);
                                        LoggerHelper.Info(" /*此处加入活动*/:" + targetid + " DeviceKey:" + deviceKey);
                                        //参与所有活动规则
                                        if (!string.IsNullOrWhiteSpace(deviceKey))
                                        {
                                            using (ActFacade actFacade = new ActFacade())
                                            {
                                                actFacade.SendBonusAfterInvest(dt, Utils.GetDevicePlatformCode(deviceKey));
                                            }
                                        }
                                        ViewBag.UserId = Convert.ToInt32(dt.Rows[0]["registerid"]);
                                        #region MyRegion  系统消息
                                        DateTime dti = DateTime.Now;
                                        M_td_System_message pm = new M_td_System_message();
                                        pm.MReg = int.Parse(dt.Rows[0]["investor_registerid"].ToString());
                                        pm.Mstate = 0;
                                        pm.MTitle = "投资成功";
                                        pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功投资了项目【" + dt.Rows[0]["borrowing_title"].ToString() + "】，投资金额是：" + dt.Rows[0]["investment_amount"].ToString() + "。如有问题可咨询创利投的客服！谢谢！";
                                        pm.PubTime = dti;
                                        pm.Mtype = 1;
                                        B_usercenter.AddMessage(pm);
                                        #endregion

                                        #region MyRegion//短信通知

                                        MemberLogic _logic = new MemberLogic();
                                        var smsEntity = _logic.GetSmsEmailEntity(1, 15); // 短信通知



                                        StringBuilder sbsms = new StringBuilder(smsEntity.SEContext);
                                        sbsms = sbsms.Replace("#USERANEM#", dt.Rows[0]["username"].ToString());
                                        sbsms = sbsms.Replace("#PID#", dt.Rows[0]["targetid"].ToString());
                                        sbsms = sbsms.Replace("#MONEY#", dt.Rows[0]["investment_amount"].ToString());
                                        string mobile = dt.Rows[0]["mobile"].ToString();
                                        M_td_SMS_record psms = new M_td_SMS_record();
                                        B_td_SMS_record osms = new B_td_SMS_record();
                                        int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.投资成功.ToString());
                                        psms.phone_number = mobile;
                                        psms.sendtime = DateTime.Now;
                                        psms.senduserid = int.Parse(dt.Rows[0]["investor_registerid"].ToString());
                                        psms.smstype = smstype;
                                        psms.smscontext = sbsms.ToString();
                                        psms.orderid = SendSMS.Send(mobile, sbsms.ToString());
                                        psms.vcode = "";

                                        osms.Add(psms);
                                        #endregion
                                    }
                                }
                            }
                        }
                    }
                }/*缓存检查结束位置*/
            }
            #endregion
            return View(p);
        }
        #region 投资成功,汇付后台主动通知
        /// <summary>
        /// 投资成功,汇付后台主动通知
        /// </summary>
        /// <returns></returns>
        public ActionResult CallbackBgRetUrl()
        {
            int id = 0;
            string srt = "";
            ReInitiativeTender p = new ReInitiativeTender();
            B_usercenter BUC = new B_usercenter();
            string OrdId = "";
            int useridc = 0;
            string targetid = "0";

            id = DNTRequest.GetInt("id", 0);
            LoggerHelper.Info("主动通知后台有响应成功!");
            p.CmdId = DNTRequest.GetString("CmdId");
            p.RespCode = DNTRequest.GetString("RespCode");
            p.RespDesc = HttpUtility.UrlDecode(DNTRequest.GetString("RespDesc"));
            p.MerCustId = DNTRequest.GetString("MerCustId");
            p.OrdId = DNTRequest.GetString("OrdId");
            p.OrdDate = DNTRequest.GetString("OrdDate");
            p.TransAmt = DNTRequest.GetString("TransAmt");
            p.UsrCustId = DNTRequest.GetString("UsrCustId");
            p.TrxId = DNTRequest.GetString("TrxId");
            p.IsFreeze = DNTRequest.GetString("IsFreeze");
            p.FreezeOrdId = DNTRequest.GetString("FreezeOrdId");
            p.FreezeTrxId = DNTRequest.GetString("FreezeTrxId");
            p.RetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("RetUrl"));
            p.BgRetUrl = HttpUtility.UrlDecode(DNTRequest.GetString("BgRetUrl"));
            string merp = DNTRequest.GetString("MerPriv");
            if (merp.Length > 0)
            {
                p.MerPriv = HttpUtility.UrlDecode(merp);
            }
            else
            {
                p.MerPriv = merp;
            }

            p.RespExt = HttpUtility.UrlDecode(DNTRequest.GetString("RespExt"));
            p.ChkValue = DNTRequest.GetString("ChkValue");
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(p.CmdId);
            chkVal.Append(p.RespCode);
            chkVal.Append(p.MerCustId);
            chkVal.Append(p.OrdId);
            chkVal.Append(p.OrdDate);
            chkVal.Append(p.TransAmt);
            chkVal.Append(p.UsrCustId);
            chkVal.Append(p.TrxId);
            chkVal.Append(p.IsFreeze);
            chkVal.Append(p.FreezeOrdId);
            chkVal.Append(p.FreezeTrxId);
            chkVal.Append(p.RetUrl);
            chkVal.Append(p.BgRetUrl);
            chkVal.Append(p.MerPriv);
            chkVal.Append(p.RespExt);
            string chkv = chkVal.ToString();

            //私钥文件的位置(这里是放在了站点的根目录下)

            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.PgPubk;

            int ret = DllInterop.VeriSignMsg(merKeyFile, chkv, chkv.Length, p.ChkValue);

            LoggerHelper.Info("投标后台主动投标返回报文:" + JsonHelper.Entity2Json(p));

            string sql = "";
            merp = p.MerPriv;
            string deviceKey = PickoutDeviceFlag(ref merp);//pick out device code from comment field
            p.MerPriv = merp;
            int invcount = 0; //记录用户是否是首次投资
            #region 验签
            if (ret == 0)
            {
                if (p.RespCode == "000" || p.RespCode == "534" || p.RespCode == "360" || p.RespCode == "099")//p.RespCode == "322" ||
                {
                    string cachename = p.OrdId + "Invest" + p.UsrCustId;

                    if (Settings.Instance.GeTThirdCache(cachename) == 0)
                    {
                        Settings.Instance.SetThirdCache(cachename);
                        if (p.FreezeTrxId != "")
                        {
                            sql = "select ordstate  from hx_Bid_records  where ordstate =0 and  OrdId='" + p.OrdId + "'";
                            LoggerHelper.Info("CallbackBgRetUrl===p.OrdId:" + p.OrdId);
                            LoggerHelper.Info("sql :" + sql);
                            DataTable dts = DbHelper.Query(sql).Tables[0];

                            if (dts.Rows.Count > 0)
                            {
                                //同步处理用户金额
                                int d = BUC.ReInvest_success(p.UsrCustId, p.FreezeOrdId, p.TransAmt, p.FreezeTrxId, p.OrdId, p.MerPriv);
                                LoggerHelper.Info("后台投标:id" + id.ToString() + "返回唯一冻结标识:" + p.FreezeTrxId + "事务执行结果:" + d.ToString());

                                if (d > 0)
                                {
                                    sql = "select targetid,bid_records_id, borrowing_title,investor_registerid ,username,mobile,invitationcode,investment_amount,life_of_loan,unit_day,borrowing_balance,bonusAmt  from  V_hx_Bid_records_borrowing_target where OrdId='" + p.OrdId + "'";

                                    DataTable dt = DbHelper.Query(sql).Tables[0];

                                    if (dt.Rows.Count > 0)
                                    {


                                        decimal invdesc = decimal.Parse(dt.Rows[0]["investment_amount"].ToString());

                                        OrdId = p.OrdId;
                                        useridc = int.Parse(dt.Rows[0]["investor_registerid"].ToString());

                                        targetid = dt.Rows[0]["targetid"].ToString();

                                        LoggerHelper.Info(" /*此处加入活动*/:" + targetid);
                                        //参与所有活动规则
                                        if (!string.IsNullOrWhiteSpace(deviceKey))
                                        {
                                            using (ActFacade actFacade = new ActFacade())
                                            {
                                                actFacade.SendBonusAfterInvest(dt, Utils.GetDevicePlatformCode(deviceKey));
                                            }
                                        }
                                        #region MyRegion  邀请注册奖历 投资成功奖励 --已过期
                                        //invcount = B_usercenter.GetIsNews(useridc);
                                        //ActTable act = new ActTable();
                                        //#region 首次投资活动奖励
                                        //if (invcount == 1)
                                        //{
                                        //    //首次投资活动奖励
                                        //    act.UsrFirstInvest(useridc, decimal.Parse(p.TransAmt), 3, 1);

                                        //}

                                        /////续投用户
                                        //if (invcount >= 2)
                                        //{
                                        //    act.UsrFirstInvest(useridc, decimal.Parse(p.TransAmt), 3, 6);
                                        //}

                                        //#region 所有用户
                                        ////所有用户
                                        //act.UsrFirstInvest(useridc, decimal.Parse(p.TransAmt), 3, 5);

                                        //#endregion


                                        //#region 投标最大的用户
                                        ////投标最大的用户
                                        //decimal borrowing_balance = decimal.Parse(dt.Rows[0]["borrowing_balance"].ToString());
                                        //DataTable dmax = B_usercenter.Topinvestor(int.Parse(targetid));
                                        //if (dmax.Rows.Count > 0)
                                        //{
                                        //    decimal amtc = decimal.Parse(dmax.Rows[0]["InvCount_Amt"].ToString());
                                        //    if (borrowing_balance == amtc)
                                        //    {
                                        //        act.UsrFirstInvest(int.Parse(dmax.Rows[0]["investor_registerid"].ToString()), decimal.Parse(dmax.Rows[0]["maxamt"].ToString()), 3, 4);
                                        //    }
                                        //}
                                        //#endregion

                                        //#region 每标首投用户
                                        ////每标首投用户
                                        //if (B_usercenter.TopNum(targetid) == 1)
                                        //{
                                        //    act.UsrFirstInvest(useridc, decimal.Parse(p.TransAmt), 3, 3);
                                        //}
                                        //#endregion

                                        //#endregion


                                        //#region 渠道合作 第一投标调用接口

                                        //B_member_table bmt = new B_member_table();
                                        //M_member_table mmt = new M_member_table();

                                        //mmt = bmt.GetModel(int.Parse(dt.Rows[0]["investor_registerid"].ToString()));


                                        //if (mmt.Tid != null && mmt.Channelsource == 1)
                                        //{
                                        //    if (B_usercenter.GetIsNews(mmt.registerid) == 1)
                                        //    {
                                        //        string ret3 = Utils.GetCoopAPI(mmt.Tid, invdesc.ToString("0.00"), 2);

                                        //        LoggerHelper.Info("渠道合作第一次返回结果:" + ret3 + "  用户id:" + mmt.registerid + " 订单id " + p.OrdId);
                                        //    }

                                        //}
                                        //#endregion





                                        /*此处注册奖励业务*/

                                        ////注册奖历

                                        //B_bonus_account bb = new B_bonus_account();
                                        //M_bonus_account mb = new M_bonus_account();

                                        //M_bonus_account_water mbaw = new M_bonus_account_water();
                                        //B_bonus_account_water bbaw = new B_bonus_account_water();

                                        //string invcode = dt.Rows[0]["invitationcode"].ToString();
                                        //string uid = dt.Rows[0]["investor_registerid"].ToString();



                                        //LoggerHelper.Info(" /*验证码*/:" + invcode);
                                        //if (invcode != null && invcode != "")
                                        //{

                                        //    DateTime dte = DateTime.Now;

                                        //    string codesql = "SELECT invcode,Invpeopleid,invpersonid from  hx_td_Userinvitation where invcode='" + invcode + "' and invpersonid=" + uid + " ";//查询本人是否已经被邀请注册过

                                        //    LoggerHelper.Info("codesql2:" + codesql);
                                        //    DataTable dtcode = DbHelperSQL.GET_DataTable_List(codesql);
                                        //    if (dtcode.Rows.Count > 0)
                                        //    {
                                        //        int uuid = int.Parse(dtcode.Rows[0]["Invpeopleid"].ToString()); //邀请用户id
                                        //        B_member_table oy = new B_member_table();
                                        //        M_member_table py = new M_member_table();
                                        //        //获取 邀请用户身份渠道用户不执行
                                        //        py = oy.GetModel(uuid);

                                        //        if (py.useridentity != 4)
                                        //        {

                                        //            //受邀好友首次成功投资
                                        //            #region 首次投资活动奖励

                                        //            ACTInvitation aci = new ACTInvitation();

                                        //            #region 受邀好友首次成功投资给予活动奖励
                                        //            if (invcount == 1) //受邀好友首次成功投资给予活动奖励
                                        //            {
                                        //                hx_UserAct hut = aci.YaoAmtAct(uuid, int.Parse(dtcode.Rows[0]["invpersonid"].ToString()), -1, 4, 1, 1, 0, 5);
                                        //                if (hut != null)
                                        //                {
                                        //                    mbaw.bonus_account_id = hut.UserAct;
                                        //                    mbaw.membertable_registerid = uuid;
                                        //                    mbaw.income = decimal.Parse(hut.Amt.ToString());
                                        //                    mbaw.expenditure = 0.00M;
                                        //                    mbaw.time_of_occurrence = DateTime.Now;
                                        //                    mbaw.award_description = "邀请好友首次投资成功" + hut.Amt.ToString() + "元奖励";
                                        //                    mbaw.water_type = 0;
                                        //                    bbaw.Add(mbaw);
                                        //                }

                                        //            }


                                        //            #endregion


                                        //            #region 受邀用户续投奖励
                                        //            ///受邀用户续投奖励
                                        //            if (invcount >= 2)
                                        //            {
                                        //                if (py.LostInvitation == 0)
                                        //                {
                                        //                    //这里需要取出受邀用户奖总数

                                        //                    int biyaoUsrid = int.Parse(dt.Rows[0]["invpersonid"].ToString());

                                        //                    decimal totAmt = B_usercenter.GetInviUserTotalAmt(uuid, biyaoUsrid);

                                        //                    hx_UserAct hut = aci.YaoAmtAct(uuid, biyaoUsrid, decimal.Parse(p.TransAmt), 4, 2, 1, totAmt, 5);

                                        //                    if (hut != null)
                                        //                    {
                                        //                        mbaw.bonus_account_id = hut.UserAct;
                                        //                        mbaw.membertable_registerid = uuid;
                                        //                        mbaw.income = decimal.Parse(hut.Amt.ToString());
                                        //                        mbaw.expenditure = 0.00M;
                                        //                        mbaw.time_of_occurrence = DateTime.Now;
                                        //                        mbaw.award_description = "受邀用户续投奖励" + hut.Amt.ToString() + "元奖励";
                                        //                        mbaw.water_type = 0;
                                        //                        bbaw.Add(mbaw);
                                        //                    }

                                        //                }

                                        //            }
                                        //            #endregion

                                        //            /*
                                        //            int bbid = bb.Add(mb);
                                        //            if (bbid > 0) //奖励记录成功后插入明细记录
                                        //            {
                                        //                mbaw.bonus_account_id = bbid;
                                        //                mbaw.membertable_registerid = uuid;
                                        //                mbaw.income = mb.amount_of_reward;
                                        //                mbaw.expenditure = 0.00M;
                                        //                mbaw.time_of_occurrence = mb.entry_time;
                                        //                // mbaw.
                                        //                mbaw.award_description = "邀请投资成功10元奖励";
                                        //                mbaw.water_type = 0;
                                        //                bbaw.Add(mbaw);

                                        //                DbHelperSQL.RunSql(" update hx_td_Userinvitation  set InvitesStates=1  where invcode='" + invcode + "' and invpersonid=" + uid + " and  InvitesStates=2 ");
                                        //                LoggerHelper.Info(" 后台更新数据邀请状态 update hx_td_Userinvitation  set InvitesStates=1  where invcode='" + invcode + "' and invpersonid=" + uid + " and  InvitesStates=2 ");
                                        //            }*/




                                        //        }
                                        //    }
                                        //}


                                        //#endregion
                                        #endregion


                                        #region MyRegion  系统消息
                                        DateTime dti = DateTime.Now;

                                        M_td_System_message pm = new M_td_System_message();
                                        pm.MReg = int.Parse(dt.Rows[0]["investor_registerid"].ToString());
                                        pm.Mstate = 0;
                                        pm.MTitle = "投资成功";
                                        pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功投资了项目【" + dt.Rows[0]["borrowing_title"].ToString() + "】，投资金额是：" + dt.Rows[0]["investment_amount"].ToString() + "。如有问题可咨询创利投的客服！谢谢！";
                                        pm.PubTime = dti;
                                        pm.Mtype = 1;
                                        B_usercenter.AddMessage(pm);
                                        #endregion



                                        #region MyRegion//短信通知

                                        MemberLogic _logic = new MemberLogic();
                                        var smsEntity = _logic.GetSmsEmailEntity(1, 15); // 短信通知



                                        StringBuilder sbsms = new StringBuilder(smsEntity.SEContext);

                                        sbsms = sbsms.Replace("#USERANEM#", dt.Rows[0]["username"].ToString());

                                        sbsms = sbsms.Replace("#PID#", dt.Rows[0]["targetid"].ToString());

                                        sbsms = sbsms.Replace("#MONEY#", dt.Rows[0]["investment_amount"].ToString());


                                        string mobile = dt.Rows[0]["mobile"].ToString();

                                        M_td_SMS_record psms = new M_td_SMS_record();
                                        B_td_SMS_record osms = new B_td_SMS_record();
                                        int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.投资成功.ToString());
                                        psms.phone_number = mobile;
                                        psms.sendtime = DateTime.Now;
                                        psms.senduserid = int.Parse(dt.Rows[0]["investor_registerid"].ToString());
                                        psms.smstype = smstype;
                                        psms.smscontext = sbsms.ToString();
                                        psms.orderid = SendSMS.Send(mobile, sbsms.ToString());
                                        psms.vcode = "";

                                        osms.Add(psms);
                                        #endregion

                                    }
                                    //远程调用生成合同                             

                                    //var values = new NameValueCollection
                                    //{
                                    //    {"action","MUserPDF"},
                                    //    {"data",targetid},
                                    //    {"uc",useridc.ToString()},
                                    //    {"OrdId",OrdId}
                                    //};
                                    //HttpHelper.Post(Settings.Instance.SiteDomain + "/pdf", values);



                                }
                            }
                            else
                            {


                            }
                            srt = "RECV_ORD_ID_" + p.OrdId;

                        }
                    }/*缓存检查结束位置*/
                }
                else
                {

                }

            }

            #endregion
            return Content(srt);
        }
        #endregion
        #region 获取本次使用奖励金额 +decimal GetUseRewards(string strid, int userid)
        /// <summary>
        /// 获取本次使用奖励金额
        /// </summary>
        /// <param name="strid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        private decimal GetUseRewards(string strid, int userid)
        {
            decimal dec = 0.00M;

            if (strid != null && strid.Length > 0)
            {
                string sql = " select COALESCE(SUM(amt),0.00) as amount_of_reward from hx_useract where UseState=0 and registerid=" + userid.ToString() + " and UserAct in ( " + strid + ") ";
                var dt = DbHelper.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                    dec = decimal.Parse(dt.Rows[0][0].ToString());
            }
            return dec;
        }
        private string GetIntegerString(decimal data)
        {
            string temp = data.ToString();
            return temp.Split('.')[0];
        }
        #endregion      
    }
}