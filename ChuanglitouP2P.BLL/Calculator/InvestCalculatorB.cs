using ChuanglitouP2P.Model.VeryCodes.NetCreditAssistant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.BLL.Calculator
{
    public class InvestCalculatorB
    {
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



    }
}
