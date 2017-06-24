using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Data;
namespace ChuanglitouP2P.Controllers
{
    /// <summary>
    /// 大数据统计
    /// </summary>
    public class BigDataController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();
        // GET: BigData
        public ActionResult Index()
        {
            //查询交易总额,交易笔数,投资人总数，总投资额，人均投资额,借款总数，借款总额，待偿还金额，人均借款
            //逾期金额,项目逾期率,金额逾期率
            //其中交易总额 总投资额 是一个值
            var money = ef.hx_borrowing_target.Where(p => p.tender_state >= 2 && p.tender_state <= 5).Sum(s => s.borrowing_balance)+ 30000000;
            //待收本金总额
            var collectAmt = ef.V_hx_Bid_records_borrowing_target.Where(p => p.payment_status == 0).Sum(s=>s.investment_amount);
            //已收本金总额
            var receivedAmt = ef.V_hx_Bid_records_borrowing_target.Where(p => p.payment_status == 1).Sum(s => s.investment_amount);
            //总本金
            var principalAmt = money;
            //总交易笔数
            var businessCount = ef.hx_Bid_records.Where(p=>p.ordstate==1).Count();
            //投资总人数
            var totalNumber = ef.hx_Bid_records.Where(p => p.ordstate ==1).GroupBy(g => g.investor_registerid).Count();
            //总投资额
            var allAmt = money;
            //人均投资
            var avgAmt = allAmt / businessCount;
            //借款总次数
            var loanCompletedCount = ef.V_borrowing_target_addlist.Where(p => p.tender_state >=2 && p.tender_state <= 5).Count();
            //借款总额
            var loanCompletedAmt = money;//ef.V_borrowing_target_addlist.Where(p => p.tender_state >= 2 && p.tender_state <= 5).Sum(s=>s.borrowing_balance) + 30000000;
            //待偿还金额
            var collectLoanCompletedAmt = collectAmt;
            //人均借款
            var avgLoanCompletedAmt = loanCompletedAmt/loanCompletedCount;


            //2015-1-15
            DateTime startTime = DateTime.Parse("2015/1/15");
            DateTime endTime = DateTime.Now;
            int days = new TimeSpan(endTime.Ticks - startTime.Ticks).Days;

            ViewBag.PrincipalAmt = principalAmt.Value.ToString("N");
            ViewBag.BusinessCount = businessCount.ToString("#,###");
            ViewBag.TotalNumber = totalNumber.ToString("#,###");
            ViewBag.AllAmt = principalAmt.Value.ToString("N");
            ViewBag.AvgAmt = Math.Round((double)avgAmt, 2).ToString("N");
            ViewBag.LoanCompletedCount = loanCompletedCount.ToString("#,###");
            ViewBag.LoanCompletedAmt = Math.Round((double)loanCompletedAmt, 2).ToString("N"); 
            ViewBag.CollectLoanCompletedAmt = Math.Round((double)collectLoanCompletedAmt, 2).ToString("N");
            ViewBag.AvgLoanCompletedAmt = Math.Round((double)avgLoanCompletedAmt, 2).ToString("N");
            ViewBag.Days = days;
            ViewBag.ShowTime = endTime;
            return View();
        }

        /// <summary>
        ///  配合页面轮询回调
        ///  初次调用记录（时间标记）至缓存，后只调用时间标记以后的数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTotal()
        {
            var allAmt = ef.hx_borrowing_target.Where(p => p.tender_state >= 2 && p.tender_state <= 5).Sum(s => s.borrowing_balance);

            allAmt = allAmt == null ? 0 : allAmt;
            //总累计收益
            var profitAmt = ef.hx_income_statement.Sum(s => s.interestpayment);
            profitAmt = profitAmt == null ? 0 : profitAmt;
            ////总投资次数 AS 总交易笔数
            var businessCount = ef.hx_Bid_records.Where(p => p.ordstate == 1).Count();
            //投资总人数
            //var totalNumber = ef.hx_Bid_records.Where(p => p.ordstate == 1).GroupBy(g => g.investor_registerid).Count();
            //总注册人数
            var registerCount = ef.hx_member_table.Count();
            DateTime endTime = DateTime.Now;
            //缓存为空
            var result = new
            {
                state = 0
                    ,
                allAmt = Math.Round((double)allAmt + 30000000, 2)
                    ,
                profitAmt = Math.Round((double)profitAmt + 321646.58 + 700000, 2)
                    ,
                businessCount = businessCount
                    ,
                registerCount = registerCount
                    ,
                time = endTime.ToString("yyyy/MM/dd HH:mm:ss")
            };
            return Json(result);
        }
       
    }
}