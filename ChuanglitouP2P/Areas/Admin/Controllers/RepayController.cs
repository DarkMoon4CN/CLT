using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    /// <summary>
    /// 还款
    /// </summary>
    public class RepayController : Controller
    {

        chuangtouEntities ef = new chuangtouEntities();

        /// <summary>
        /// 三日内还款的
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int days = 3)
        {
            //            IEnumerable<V_borrow_repayment_plan> list = ef.V_borrow_repayment_plan.Where(a => ((DateTime)a.repayment_period).CompareTo(DateTime.Now.AddDays(days)) < 0 && ((DateTime)a.repayment_period).CompareTo(DateTime.Now) >= 0 && a.tender_state == 4);
            //            (from a in ef.V_borrow_repayment_plan where ((DateTime)a.repayment_period).CompareTo(DateTime.Now.AddDays(days)) < 0 && ((DateTime)a.repayment_period).CompareTo(DateTime.Now) >= 0 && a.tender_state == 4 group a by a.loan_number a.targetid ,borrowing_title,payment_options,life_of_loan,unit_day,realname,borrowing_balance,H_Repayment_Amt,
            //repaymentperiods,tender_state,repayment_period )


            //V_borrow_repayment_plan

            ///TODO 需要分组统计
            /// 
            /*
            select  loan_number, targetid ,count(targetid) as num ,
            borrowing_title,payment_options,life_of_loan,unit_day,realname,borrowing_balance,H_Repayment_Amt,
            repaymentperiods,tender_state,repayment_period 
            from  V_borrow_repayment_plan   
            where convert(varchar(10),repayment_period,120) <  convert(varchar(10), dateadd(day,3,GETDATE()),120)  
            and convert(varchar(10),repayment_period,120)>= convert(varchar(10),GETDATE(),120) 
            and tender_state=4 
            group by  loan_number,targetid ,borrowing_title,realname,payment_options,life_of_loan,unit_day,borrowing_balance,H_Repayment_Amt,repaymentperiods,tender_state,repayment_period

                */

            return View();
        }


}
}
 