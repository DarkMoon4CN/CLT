using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ChuanglitouP2P.BLL.Api;
using ChuangLiTou.Core.Entities.ChinaPnr;
using ChuangLiTou.Core.Entities.Response.Record;
using ChuanglitouP2P.Common;
namespace ChuangLiTouOpenApi.Areas.Callback.Controllers
{
    public class IndexController : Controller
    {
        public IndexController()
            : this(GlobalConfiguration.Configuration)
        {
        }
        public HttpConfiguration Configuration { get; private set; }

        private IndexController(HttpConfiguration configuration)
        {
            Configuration = configuration;
        }

        //
        // GET: /Borrow/Index/
        [ValidateInput(false)]
        public ActionResult Index(int userId)
        {

            MemberLogic _logic = new MemberLogic();
            //var uid = ConvertHelper.ParseValue(reqst.body.userId, 0);
            var p = _logic.SelectMemberByUserId(userId);

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
                ,
                UsrName = "解志辉"
                ,
                IdNo = "130324198702075115"
                ,
                IdType = "00"
            };

#pragma warning disable 1587
            ///签名规则
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
#pragma warning restore 1587
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.Version);
            chkVal.Append(m.CmdId);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.BgRetUrl);
            chkVal.Append(m.RetUrl);
            chkVal.Append(m.UsrId);
            chkVal.Append(HttpUtility.UrlEncode(m.UsrName, Encoding.UTF8));
            chkVal.Append(m.IdType);
            chkVal.Append(m.IdNo);
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

            return View(m);
        }

        /// <summary>
        /// 支付跳转页面
        /// </summary>
        /// <param name="recordId">The record identifier.</param>
        /// <returns>ActionResult.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-06-03 17:01:56
        public ActionResult SubmitTenderLoading(int recordId)
        {
            RecordLogic _logic = new RecordLogic();
            var p = _logic.SelectRecordId(recordId);
            MemberLogic memberLogic = new MemberLogic();
            //获取投资人信息
            var memEnt = memberLogic.SelectMemberByUserId(ConvertHelper.ParseValue(p.investor_registerid, 0));
            //获取借款人信息
            var borEnt = memberLogic.SelectMemberByUserId(ConvertHelper.ParseValue(p.borrower_registerid, 0));
            //封装借款人信息
            BorrowRecordEntity mtp = new BorrowRecordEntity
            {
                BorrowerCustId = borEnt.UsrCustId,
                BorrowerAmt = p.investment_amount.ToString("0.00"),
                BorrowerRate = "1.00",
                ProId = p.targetid.ToString()
            };

            //获取冻结号
            var freezeNo = _logic.SelectFreezeOrdId(recordId);

            //风控范围

            var ckd = Settings.Instance.SiteDomain;
            SubmitTenderEntity stm = new SubmitTenderEntity
            {
                MerId = Settings.Instance.MerId,
                Version = "20",
                CmdId = "InitiativeTender",
                MerCustId = Settings.Instance.MerCustId,
                BgRetUrl = ckd + ("/Test/Index/BgRetUrlForUserRegister"),
                RetUrl = ckd + ("/Test/Index/CallbackForUserRegister")

                ,
                OrdId = p.OrdId.ToString()
                ,
                OrdDate = p.invest_time.ToString("yyyyMMdd")
                ,
                TransAmt = p.investment_amount.ToString("0.00")
                ,
                UsrCustId = memEnt.UsrCustId
                ,
                MaxTenderRate = "0.20"
                ,
                BorrowerDetails = "[" + JsonHelper.ObjectToJson(mtp) + "]"
                ,
                ReqExt = ""//入参扩展域
                ,
                IsFreeze = "Y"
                ,
                FreezeOrdId = freezeNo
                ,
                MerPriv = Settings.Instance.MerPr
            };

            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(stm.Version);
            chkVal.Append(stm.CmdId);
            chkVal.Append(stm.MerCustId);
            chkVal.Append(stm.RetUrl);
            chkVal.Append(stm.BgRetUrl);

            chkVal.Append(stm.OrdId);
            chkVal.Append(stm.OrdDate);
            chkVal.Append(stm.TransAmt);
            chkVal.Append(stm.UsrCustId);
            chkVal.Append(stm.MaxTenderRate);
            chkVal.Append(stm.BorrowerDetails);
            chkVal.Append(stm.IsFreeze);
            chkVal.Append(stm.FreezeOrdId);
            chkVal.Append(stm.MerPriv);
            chkVal.Append(stm.ReqExt);

            string chkv = chkVal.ToString();


            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.MerPr;
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            DllInterop.SignMsg(stm.MerId, merKeyFile, chkv, len, sbChkValue);

            stm.ChkValue = sbChkValue.ToString();
            return View(stm);
        }


        [System.Web.Mvc.HttpPost]
        public ActionResult BgRetUrlForUserRegister(UserEntity m)
        {
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
            LoggerHelper.Info(chkv);

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Settings.Instance.PgPubk;

            int ret = DllInterop.VeriSignMsg(merKeyFile, chkv, chkv.Length, m.ChkValue);
            if (ret == 0)
            {
                string srt = "RECV_ORD_ID_" + m.TrxId;
                return View(srt);
            }

            LoggerHelper.Info(ret);


            return View("-1");

        }

        public ActionResult CallbackForUserRegister()
        {
            return View();
        }
    }
}