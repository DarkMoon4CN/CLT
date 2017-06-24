using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Contract;
using ChuanglitouP2P.BLL.Api;
using ChuanglitouP2P.Model;
using System.Text;
using ChuanglitouP2P.Common;
using ChuangLiTou.Core.Entities.Response.Borrow;
using ChuangLitouP2P.Models;

namespace ChuangLiTouOpenApi.Areas.H5.Controllers
{
    public class IndexController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();
        // GET: H5/Index
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 投资合同
        /// </summary>
        /// <returns></returns>
        public ActionResult InvestmentContract(int targetId)
        {
            ContractLogic _logic = new ContractLogic();
            M_Contract_management model = _logic.GetContractListForApp(targetId).FirstOrDefault();

            StringBuilder sb = new StringBuilder(model.contract_money);
            sb = sb.Replace("#loan_number#", model.loan_number.ToString());
            //手机号*号处理
            if (!string.IsNullOrEmpty(model.borrower_username) && model.borrower_username.Length == 11)
            {
                model.borrower_username = model.borrower_username.Substring(0, 3) + "****" + model.borrower_username.Substring(7);
            }
            sb = sb.Replace("#borrower_username#", model.borrower_username);
            //姓名  名字*号处理
            if (!string.IsNullOrEmpty(model.borrower_name))
            {
                model.borrower_name = model.borrower_name.Substring(0, 1) + "**";
            }
            sb = sb.Replace("#borrower_name#", model.borrower_name);
            sb = sb.Replace("#borrower_id_card#", model.borrower_id_card);
            sb = sb.Replace("#lender_username#", model.lender_username);
            sb = sb.Replace("#lender_name#", model.lender_name);
            sb = sb.Replace("#lender_id_card#", model.lender_id_card);
            sb = sb.Replace("#surety_company_name#", model.surety_company_name);
            sb = sb.Replace("#guarantor_agent_usernqme#", model.guarantor_agent_usernqme);
            sb = sb.Replace("#contract_amount#", RMB.GetDecimal(model.contract_amount, 2, true).ToString());

            BorrowLogic _borrowLogic = new BorrowLogic();
            BorrowEntity borrowEntity = _borrowLogic.SelectBorrowDetail(targetId);
            sb = sb.Replace("#annual_interest_rate#", decimal.Parse(borrowEntity.annual_interest_rate.ToString()).ToString("0.00"));
            DateTime date1 = DateTime.Parse(borrowEntity.release_date.ToString());
            DateTime date2 = DateTime.Parse(borrowEntity.repayment_date.ToString());
            sb = sb.Replace("#release_date#", date1.ToString("yyyy-MM-dd"));
            sb = sb.Replace("#repayment_date#", date2.ToString("yyyy-MM-dd"));
            sb = sb.Replace("#days#", Utils.DateDiff("Day", date1, date2).ToString());
            model.contract_money = sb.ToString();

            //ViewBag.loan_number = model.loan_number;
            //ViewBag.borrower_username = model.borrower_username;
            //ViewBag.borrower_name = model.borrower_name;
            //ViewBag.guarantee_legal_representative = model.guarantee_legal_representative;
            //ViewBag.surety_company_name = model.surety_company_name;
            //ViewBag.StartTime = model.Start_Time;
            //ViewBag.EndTime = model.End_Time;
            //ViewBag.DurationDays = model.DurationTime;
            return View(model);
        }
        /// <summary>
        /// 注册协议
        /// </summary>
        /// <returns></returns>
        public ActionResult RegistContract()
        {
            return View();
        }
        /// <summary>
        /// 安全保障
        /// </summary>
        /// <returns></returns>
        public ActionResult SecurityGuarantee()
        {
            return View();
        }

        /// <summary>
        ///  关于我们
        /// </summary>
        /// <returns></returns>
        public ActionResult AboutUs()
        {
            return View();
        }
        /// <summary>
        /// 帮助中心
        /// </summary>
        /// <returns></returns>
        public ActionResult HelpCenter()
        {
            return View();
        }

        /// <summary>
        /// 帮助中心 注册登录
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOrRegist()
        {
            return View();
        }

        /// <summary>
        /// 帮助中心 认证安全
        /// </summary>
        /// <returns></returns>
        public ActionResult AuthSafe()
        {
            return View();
        }
        /// <summary>
        /// 帮助中心 充值提现
        /// </summary>
        /// <returns></returns>
        public ActionResult Recharge()
        {
            return View();
        }
        /// <summary>
        /// 帮助中心 投资还款
        /// </summary>
        /// <returns></returns>
        public ActionResult Invest()
        {
            return View();
        }

        /// <summary>
        /// 提现手续费说明
        /// </summary>
        /// <returns></returns>
        public ActionResult CashServiceFeesNotice()
        {
            return View();
        }

        /// <summary>
        /// 充值限制说明
        /// </summary>
        /// <returns></returns>
        public ActionResult RechargeLimitNotice()
        {
            List<hx_td_Bank> listbank = ef.hx_td_Bank.Where(x => x.Isquick == 1).ToList();
            foreach (var item in listbank)
            {
                item.CardImageNew = Settings.Instance.ImagesDomain + item.CardImageNew;
                item.SingleTransQuota = Math.Round(item.SingleTransQuota.Value / 10000, 2, MidpointRounding.AwayFromZero);
                item.CardDailyTransQuota = Math.Round(item.CardDailyTransQuota.Value / 10000, 2, MidpointRounding.AwayFromZero);
            }
            return View(listbank);
        }
    }
}