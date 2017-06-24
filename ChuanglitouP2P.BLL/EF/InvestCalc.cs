using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.VeryCodes.NetCreditAssistant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.BLL.EF
{

    /// <summary>
    /// 投资利率计算器
    /// </summary>
    public class InvestCalc
    {

        #region 项目投资利率计算+decimal InvCalc(int targetid, decimal Amount = 100.00M)
        /// <summary>
        /// 项目投资利率计算
        /// </summary>
        /// <param name="targetid">标的ID</param>
        /// <param name="Amount">投资金额</param>
        ///<param name = "Lixi" > 加息券，默认为0 </ param >
        /// <returns></returns>
        public decimal InvCalc(int targetid, decimal Amount = 100.00M, double Lixi =0)
        {
            decimal dec = 0.00M;
            B_borrowing_target o = new B_borrowing_target();
            M_borrowing_target m = new M_borrowing_target();
            m = o.GetModel(targetid);
            InvestmentParameters mp = new InvestmentParameters();
            mp.Amount = Amount;
            mp.Circle = m.life_of_loan;
            mp.CircleType = m.unit_day;
            mp.NominalYearRate = double.Parse(m.annual_interest_rate.ToString()) + Lixi;
            mp.OverheadsRate = 0f;
            mp.RepaymentMode = m.payment_options;
            mp.RewardRate = 0f;
            mp.IsThirtyDayMonth = false;
            mp.InvestDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            mp.Investmentenddate = DateTime.Parse(m.repayment_date.ToString("yyyy-MM-dd"));
            mp.Payinterest = m.month_payment_date;
            mp.InvestObject = 1;
            dec = Calculationofinterest(mp);
            return dec;
        }
        #endregion

        #region 计算利息+decimal Calculationofinterest(InvestmentParameters Minverst)
        /// <summary>
        /// 计算利息
        /// </summary>
        /// <param name="Minverst"></param>
        /// <returns></returns>
        public decimal Calculationofinterest(InvestmentParameters Minverst)
        {
            decimal dec = 0.00M;
            InvestmentParameters inverstParas = new InvestmentParameters
            {
                Amount = Minverst.Amount, //投资金额
                Circle = Minverst.Circle,//期限
                CircleType = Minverst.CircleType,//期限类型 月 / 天
                NominalYearRate = Minverst.NominalYearRate,//年利率
                OverheadsRate = Minverst.OverheadsRate,//管理费率
                RepaymentMode = Minverst.RepaymentMode,//还款方式
                RewardRate = Minverst.RewardRate,//奖励比例
                InvestDate = Minverst.InvestDate,
                IsThirtyDayMonth = false,
                Investmentenddate = Minverst.Investmentenddate,
                Payinterest = Minverst.Payinterest,
                InvestObject = Minverst.InvestObject

            };
            InvestmentTotalIncome income = ChuanglitouP2P.Bll.VeryCodes.NetCreditAssistant.BLL.InvestCalculator.CalculateTotalIncome(inverstParas);
            List<InvestmentReceiveRecordInfo> records = income.ReceiveRecords;
            foreach (InvestmentReceiveRecordInfo p in records)
            {
                dec = dec + p.Interest;
            }
            //  dec = dec + Minverst.Amount;  //投资总收益
            return dec;
        } 
        #endregion

    }



}
