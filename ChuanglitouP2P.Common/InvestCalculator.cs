using System;
using System.Collections.Generic;
using System.Text;
using ChuanglitouP2P.Model.Invest;

namespace ChuanglitouP2P.Common
{

    /// <summary>
    /// 投资计算器
    /// </summary>
    public class InvestCalculator
    {
        public static List<InvestmentReceiveRecordInfo> CalculateReceiveRecord(InvestmentParameters inverstParas)
        {
            List<InvestmentReceiveRecordInfo> receiveRecords = new List<InvestmentReceiveRecordInfo>();
            switch (inverstParas.RepaymentMode)
            {
                case 1:
                    receiveRecords = CalculateReceiveRecordWithEqualPrincipalAndInterest(inverstParas);
                    break;

                case 2:
                    receiveRecords = CalculateReceiveRecordWithEqualPrincipalByQuart(inverstParas);
                    break;

                case 3:
                    receiveRecords = CalculateReceiveRecordWithInterestByMonth(inverstParas);
                    break;

                case 4:
                    receiveRecords = CalculateReceiveRecordWithLumpSumPrincipalAndInterest(inverstParas);
                    break;
            }
            return FormatReceiveRecords(receiveRecords);
        }


        /// <summary>
        /// 按月等额本息
        /// </summary>
        /// <param name="inverstParas"></param>
        /// <returns></returns>
        private static List<InvestmentReceiveRecordInfo> CalculateReceiveRecordWithEqualPrincipalAndInterest(InvestmentParameters inverstParas)
        {
            List<InvestmentReceiveRecordInfo> list = new List<InvestmentReceiveRecordInfo>();
            if (inverstParas.CircleType == 1)
            {

                double num = (inverstParas.NominalYearRate / 12.0) / 100.0;//月利率
                double num2 = Math.Pow(1.0 + num, (double)inverstParas.Circle);
                decimal d = 0M;
                if (num > 0.0)
                {
                    d = inverstParas.Amount * ((decimal)((num * num2) / (num2 - 1.0)));
                }
                else
                {
                    d = inverstParas.Amount / inverstParas.Circle;
                }
                d = Math.Round(d, 2);
                for (int i = 1; i <= inverstParas.Circle; i++)
                {
                    InvestmentReceiveRecordInfo item = new InvestmentReceiveRecordInfo();
                    decimal num5 = (decimal)Math.Pow(1.0 + num, (double)((i - 1) - inverstParas.Circle));
                    item.Interest = Math.Round((decimal)(d * (1M - num5)), 2);
                    item.Principal = d - item.Interest;
                    item.Amount = item.Principal + item.Interest;
                    item.Overheads = item.Interest * ((decimal)inverstParas.OverheadsRate);
                    if (inverstParas.IsThirtyDayMonth)
                    {
                        item.NominalReceiveDate = inverstParas.InvestDate.AddDays((double)(i * 30));
                    }
                    else
                    {
                        item.NominalReceiveDate = inverstParas.InvestDate.AddMonths(i);
                    }
                    item.TotalInstallments = inverstParas.Circle;
                    item.CurrentInstallments = i;
                    item.AddTime = DateTime.Now;
                    item.UpdateTime = DateTime.Now;
                    item.PrincipalStatus = 0;
                    item.InterestStatus = 0;
                    item.Balance = (inverstParas.Circle - i) * d;
                    list.Add(item);
                }
                return list;
            }
            if (inverstParas.CircleType == 3)
            {
                list = CalculateReceiveRecordWithLumpSumPrincipalAndInterest(inverstParas);
            }
            return list;
        }

        private static List<InvestmentReceiveRecordInfo> CalculateReceiveRecordWithEqualPrincipalByQuart(InvestmentParameters inverstParas)
        {
            List<InvestmentReceiveRecordInfo> list = new List<InvestmentReceiveRecordInfo>();
            if (inverstParas.CircleType == 1)
            {
                int n = (int)Math.Ceiling((double)(((double)inverstParas.Circle) / 3.0));
                decimal num2 = inverstParas.Amount / n;
                decimal num3 = (((decimal)inverstParas.NominalYearRate) / 4M) / 100M;
                decimal num4 = (inverstParas.Amount * (Sum(n) / n)) * num3;
                decimal decimal1 = inverstParas.Amount + num4;
                int num5 = 0;
                for (int i = 1; i <= n; i++)
                {
                    decimal num7 = (inverstParas.Amount * (((n - i) + 1) / n)) * num3;
                    decimal num8 = (inverstParas.Amount * (Sum(n - i) / n)) * num3;
                    for (int j = 1; j <= 3; j++)
                    {
                        num5++;
                        InvestmentReceiveRecordInfo item = new InvestmentReceiveRecordInfo();
                        if (j == 3)
                        {
                            item.Principal = num2;
                        }
                        else
                        {
                            item.Principal = 0M;
                        }
                        item.Interest = num7 / 3M;
                        item.Amount = item.Principal + item.Interest;
                        item.Overheads = item.Interest * ((decimal)inverstParas.OverheadsRate);
                        if (inverstParas.IsThirtyDayMonth)
                        {
                            item.NominalReceiveDate = inverstParas.InvestDate.AddDays((double)((j * i) * 30));
                        }
                        else
                        {
                            item.NominalReceiveDate = inverstParas.InvestDate.AddMonths(j * i);
                        }
                        item.TotalInstallments = inverstParas.Circle;
                        item.CurrentInstallments = num5;
                        item.AddTime = DateTime.Now;
                        item.UpdateTime = DateTime.Now;
                        item.PrincipalStatus = 0;
                        item.InterestStatus = 0;
                        if (j == 3)
                        {
                            item.Balance = (((n - i) * num2) + ((3 - j) * item.Interest)) + num8;
                        }
                        else
                        {
                            item.Balance = ((((n - i) + 1) * num2) + ((3 - j) * item.Interest)) + num8;
                        }
                        list.Add(item);
                    }
                }
                return list;
            }
            if (inverstParas.CircleType == 3)
            {
                list = CalculateReceiveRecordWithLumpSumPrincipalAndInterest(inverstParas);
            }
            return list;
        }



        /// <summary>
        /// 按日计息,每月还息，到期还本
        /// </summary>
        /// <param name="inverstParas"></param>
        /// <returns></returns>

        private static List<InvestmentReceiveRecordInfo> CalculateReceiveRecordWithInterestByMonth(InvestmentParameters inverstParas)
        {
            List<InvestmentReceiveRecordInfo> list = new List<InvestmentReceiveRecordInfo>();

            DateTime Startdate_M = new DateTime();  //每月起息日

            if (inverstParas.CircleType == 1)
            {
                double num = (inverstParas.NominalYearRate / 12.0) / 100.0;
                decimal num2 = inverstParas.Amount * ((decimal)num);
                for (int i = 1; i <= inverstParas.Circle; i++)
                {
                    InvestmentReceiveRecordInfo item = new InvestmentReceiveRecordInfo();
                    DateTime CMTH = new DateTime();  //当月付息日
                    int days = 0; //天数
                    long diffdays = 0; //计算相差天数

                    DateTime startdate = new DateTime();
                    if (i == 1)  //第一期
                    {
                        item.Principal = 0M;

                        
                            CMTH = new DateTime(inverstParas.InvestDate.Year, inverstParas.InvestDate.Month, inverstParas.Payinterest);//当月付息日的日期显示
                        

                        //需要处理计算是投资日之前还是之后

                      
                        int dtt = 0;

                        if (inverstParas.ReleaseDate != null)
                        {
                            dtt = DateTime.Compare(CMTH, DateTime.Parse(inverstParas.ReleaseDate));
                        }
                        else
                        {
                            dtt = DateTime.Compare(CMTH, inverstParas.InvestDate);
                        }

                        if (dtt > 0)   //之后 如 投资 14 付息 25
                        {
                            item.interestvalue_date = Startdate_M = startdate = inverstParas.InvestDate;
                            item.Principal = 0M;
                            diffdays = Settings.Instance.DateDiff("Day", DateTime.Parse(inverstParas.InvestDate.ToString("yyyy-MM-dd")), DateTime.Parse(CMTH.ToString("yyyy-MM-dd")));
                            days = Math.Abs(int.Parse(diffdays.ToString()));
                            item.NominalReceiveDate = startdate.AddDays(days);
                            item.TotalDays = days;
                            item.Interest = inverstParas.Amount * ((decimal)(((inverstParas.NominalYearRate / 100.0) * days) / 365.0));
                        }
                        else //之前 如投资 14 付息 1 得计算到下个月的1号
                        {
                            item.interestvalue_date = Startdate_M = startdate = inverstParas.InvestDate;
                            item.Principal = 0M;
                            DateTime dd = DateTime.Parse(CMTH.AddMonths(1).ToString("yyyy-MM-dd"));
                            diffdays = Settings.Instance.DateDiff("Day", DateTime.Parse(inverstParas.InvestDate.ToString("yyyy-MM-dd")), dd);
                            days = Math.Abs(int.Parse(diffdays.ToString()));
                            item.NominalReceiveDate = startdate.AddDays(days);
                            item.TotalDays = days;
                            item.Interest = inverstParas.Amount * ((decimal)(((inverstParas.NominalYearRate / 100.0) * days) / 365.0));
                        }

                        Startdate_M = item.NominalReceiveDate;

                    }
                    else if (i == inverstParas.Circle) //最后一期
                    {
                        //item.interestvalue_date = Startdate_M = startdate = inverstParas.InvestDate.AddMonths(1);

                        item.interestvalue_date = Startdate_M = startdate = Startdate_M;
                        item.Principal = inverstParas.Amount;
                        CMTH = DateTime.Parse(inverstParas.Investmentenddate.ToString("yyyy-MM-dd"));
                        diffdays = Settings.Instance.DateDiff("Day", DateTime.Parse(item.interestvalue_date.ToString("yyyy-MM-dd")), CMTH);
                        days = Math.Abs(int.Parse(diffdays.ToString()));
                        item.NominalReceiveDate = CMTH;
                        item.TotalDays = days;
                        item.Interest = inverstParas.Amount * ((decimal)(((inverstParas.NominalYearRate / 100.0) * days) / 365.0));
                        // Startdate_M = CMTH;

                    }
                    else
                    {
                        item.Principal = 0M;
                        item.interestvalue_date = inverstParas.InvestDate = startdate = Startdate_M;
                        Startdate_M = Startdate_M.AddMonths(1);
                        diffdays = Settings.Instance.DateDiff("Day", DateTime.Parse(inverstParas.InvestDate.ToString("yyyy-MM-dd")), DateTime.Parse(Startdate_M.ToString("yyyy-MM-dd")));
                        days = Math.Abs(int.Parse(diffdays.ToString()));
                        item.NominalReceiveDate = startdate.AddDays(days);
                        item.TotalDays = days;
                        item.Interest = inverstParas.Amount * ((decimal)(((inverstParas.NominalYearRate / 100.0) * days) / 365.0));

                        Startdate_M = item.NominalReceiveDate;

                    }
                    // item.Interest = num2;
                    item.Amount = item.Principal + item.Interest;
                    item.Overheads = item.Interest * ((decimal)inverstParas.OverheadsRate);

                    /* 作废
                    if (inverstParas.IsThirtyDayMonth)
                    {
                        item.NominalReceiveDate = inverstParas.InvestDate.AddDays((double)(i * 30));
                    }
                    else
                    {
                        item.NominalReceiveDate = inverstParas.InvestDate.AddMonths(i);
                    }
                    */

                    item.TotalInstallments = inverstParas.Circle;
                    item.CurrentInstallments = i;
                    item.AddTime = DateTime.Now;
                    item.UpdateTime = DateTime.Now;
                    item.PrincipalStatus = 0;
                    item.InterestStatus = 0;
                    if (i == inverstParas.Circle)
                    {
                        item.Balance = inverstParas.Amount + item.Interest;

                    }
                    else
                    {
                        item.Balance = item.Interest;
                    }
                    list.Add(item);
                }
                return list;
            }
            if (inverstParas.CircleType == 3)
            {
                list = CalculateReceiveRecordWithLumpSumPrincipalAndInterest(inverstParas);
            }
            return list;
        }




        /// <summary>
        /// 按日计息,每月还息，到期还本
        /// </summary>
        /// <param name="inverstParas"></param>
        /// <returns></returns>
        private static List<InvestmentReceiveRecordInfo> CalculateReceiveRecordWithInterestByMonth1(InvestmentParameters inverstParas)
        {
            List<InvestmentReceiveRecordInfo> list = new List<InvestmentReceiveRecordInfo>();
            if (inverstParas.CircleType == 1)
            {
                double num = (inverstParas.NominalYearRate / 12.0) / 100.0; //月利率
                decimal num2 = inverstParas.Amount * ((decimal)num);

                DateTime CMTH1 = new DateTime();  //每月结息日
                for (int i = 1; i <= inverstParas.Circle; i++)
                {
                    InvestmentReceiveRecordInfo item = new InvestmentReceiveRecordInfo();

                    DateTime CMTH = new DateTime();  //当月计息日



                    int days = 0;//投次天数







                    if (i == 1)
                    {
                        item.Principal = 0M;

                        CMTH = new DateTime(inverstParas.InvestDate.Year, inverstParas.InvestDate.Month, inverstParas.Payinterest);//当月付息日的日期显示
                        //item.interestvalue_date = CMTH;

                        item.interestvalue_date = CMTH1 = inverstParas.InvestDate;


                    }
                    else if (i == inverstParas.Circle)
                    {
                        item.Principal = inverstParas.Amount;

                        CMTH = DateTime.Parse(inverstParas.Investmentenddate.ToString("yyyy-MM-dd"));

                        item.interestvalue_date = CMTH1 = new DateTime(inverstParas.InvestDate.Year, inverstParas.InvestDate.Month, inverstParas.Payinterest);//当月付息日的日期显示

                    }
                    else
                    {
                        item.Principal = 0M;



                        CMTH = new DateTime(inverstParas.InvestDate.Year, inverstParas.InvestDate.Month, inverstParas.Payinterest);//当月付息日的日期显示
                        item.interestvalue_date = CMTH;
                    }

                    //应收利息需要按日来计算 特殊处理


                    long diffdays = Settings.Instance.DateDiff("Day", DateTime.Parse(inverstParas.InvestDate.ToString("yyyy-MM-dd")), DateTime.Parse(CMTH.ToString("yyyy-MM-dd")));


                    if (diffdays > 0)
                    {
                        days = int.Parse(diffdays.ToString());
                        //item.Interest = num2;
                        ///日利息计算
                        //inverstParas.Amount * ((decimal)(((inverstParas.NominalYearRate / 100.0) * inverstParas.Circle) / 365.0));
                        item.Interest = inverstParas.Amount * ((decimal)(((inverstParas.NominalYearRate / 100.0) * days) / 365.0));



                    }
                    else if (diffdays == 0)
                    {
                        days = DateTime.DaysInMonth(inverstParas.InvestDate.Year, inverstParas.InvestDate.Month);
                        item.Interest = inverstParas.Amount * ((decimal)(((inverstParas.NominalYearRate / 100.0) * days) / 365.0));

                    }
                    else
                    {

                        DateTime dd = DateTime.Parse(CMTH.AddMonths(1).ToString("yyyy-MM-dd"));
                        diffdays = Settings.Instance.DateDiff("Day", DateTime.Parse(inverstParas.InvestDate.ToString("yyyy-MM-dd")), dd);
                        days = int.Parse(diffdays.ToString());
                        item.Interest = inverstParas.Amount * ((decimal)(((inverstParas.NominalYearRate / 100.0) * days) / 365.0));
                        //超过日期后按投资当天计息
                        if (i == 1)
                        {
                            item.interestvalue_date = DateTime.Parse(inverstParas.InvestDate.ToString("yyyy-MM-dd"));

                        }
                        else
                        {
                            item.interestvalue_date = dd;

                        }
                        // i = i + 1;
                    }

                    if (i == 1)
                    {
                        inverstParas.InvestDate = CMTH;
                    }
                    else if (i == inverstParas.Circle)
                    {
                        inverstParas.InvestDate = CMTH;

                    }

                    else
                    {

                        inverstParas.InvestDate = CMTH.AddMonths(1);

                    }
                    item.Amount = item.Principal + item.Interest;
                    item.Overheads = item.Interest * ((decimal)inverstParas.OverheadsRate);
                    if (inverstParas.IsThirtyDayMonth)
                    {
                        item.NominalReceiveDate = inverstParas.InvestDate.AddDays((double)(i * 30));
                    }
                    else
                    {
                        //item.NominalReceiveDate = inverstParas.InvestDate.AddMonths(i);
                        item.NominalReceiveDate = inverstParas.InvestDate;

                    }
                    // item.TotalInstallments = inverstParas.Circle;

                    item.TotalInstallments = days;
                    item.CurrentInstallments = i;
                    item.AddTime = DateTime.Now;
                    item.UpdateTime = DateTime.Now;
                    item.PrincipalStatus = 0;
                    item.InterestStatus = 0;
                    if (i == inverstParas.Circle)
                    {

                        item.Balance = inverstParas.Amount + item.Interest;

                    }
                    else
                    {
                        item.Balance = item.Interest;
                    }
                    list.Add(item);
                }
                return list;
            }
            if (inverstParas.CircleType == 3)
            {
                list = CalculateReceiveRecordWithLumpSumPrincipalAndInterest(inverstParas);
            }
            return list;
        }

        private static List<InvestmentReceiveRecordInfo> CalculateReceiveRecordWithLumpSumPrincipalAndInterest(InvestmentParameters inverstParas)
        {
            int days = 0;

            List<InvestmentReceiveRecordInfo> list = new List<InvestmentReceiveRecordInfo>();
            InvestmentReceiveRecordInfo item = new InvestmentReceiveRecordInfo();
            if (inverstParas.CircleType == 1)
            {
                if (inverstParas.IsThirtyDayMonth)
                {
                    item.NominalReceiveDate = inverstParas.InvestDate.AddDays((double)(inverstParas.Circle * 30));
                    inverstParas.Circle *= 30;
                }
                else
                {
                    //item.NominalReceiveDate = inverstParas.InvestDate.AddMonths(inverstParas.Circle);
                    // TimeSpan span = (TimeSpan)(DateTime.Now.AddMonths(inverstParas.Circle) - DateTime.Now);

                    item.NominalReceiveDate = inverstParas.Investmentenddate;
                    TimeSpan span = (TimeSpan)(inverstParas.Investmentenddate - inverstParas.InvestDate);
                    inverstParas.Circle = (int)Math.Ceiling(span.TotalDays);
                }
            }
            else
            {
                //天标这里需要调整



                // item.NominalReceiveDate = inverstParas.InvestDate.AddDays((double)inverstParas.Circle);

                item.NominalReceiveDate = inverstParas.Investmentenddate;
            }


            long diffdays = Settings.Instance.DateDiff("Day", DateTime.Parse(inverstParas.InvestDate.ToString("yyyy-MM-dd")), DateTime.Parse(item.NominalReceiveDate.ToString("yyyy-MM-dd")));


            days = int.Parse(diffdays.ToString());
            item.interestvalue_date = inverstParas.InvestDate;
            item.AddTime = DateTime.Now;
            item.UpdateTime = DateTime.Now;

            if (days == 0)
            {
                days = 1;
            }


            item.TotalInstallments = days;
            item.TotalDays = days;
            item.CurrentInstallments = 1;
            //   item.Interest = inverstParas.Amount * ((decimal)(((inverstParas.NominalYearRate / 100.0) * inverstParas.Circle) / 365.0));

            item.Interest = inverstParas.Amount * ((decimal)(((inverstParas.NominalYearRate / 100.0) * days) / 365.0));
            item.Principal = inverstParas.Amount;
            item.PrincipalStatus = 0;
            item.InterestStatus = 0;
            item.Amount = item.Principal + item.Interest;
            item.Balance = item.Principal + item.Interest;
            item.TotalInstallments = 1;
            item.Overheads = item.Interest * ((decimal)inverstParas.OverheadsRate);
            list.Add(item);
            return list;
        }




        /// <summary>
        /// 计算投资收入
        /// </summary>
        /// <param name="inverstParas"></param>
        /// <returns></returns>
        public static InvestmentTotalIncome CalculateTotalIncome(InvestmentParameters inverstParas)
        {
            InvestmentTotalIncome income = new InvestmentTotalIncome();
            switch (inverstParas.RepaymentMode)
            {
                case 1:

                    income = CalculateTotalIncomeWithEqualPrincipalAndInterest(inverstParas);//以平等的本金和利息的计算总收入  按月等额本息
                    break;

                case 2:
                    income = CalculateTotalIncomeWithEqualPrincipalByQuart(inverstParas);
                    break;

                case 3:
                    income = CalculateTotalIncomeWithInterestByMonth(inverstParas);
                    break;

                case 4:
                    income = CalculateTotalIncomeWithLumpSumPrincipalAndInterest(inverstParas);
                    break;
            }
            return CorrectTotalIncome(FormatTotalIncome(income), inverstParas);
        }

        /// <summary>
        /// Calculates the total income with equal principal and interest.
        /// </summary>
        /// <param name="inverstParas">The inverst paras.</param>
        /// <returns>InvestmentTotalIncome.</returns>
        ///  <remarks>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 10:43:10
        ///  </remarks>
        private static InvestmentTotalIncome CalculateTotalIncomeWithEqualPrincipalAndInterest(InvestmentParameters inverstParas)
        {
            InvestmentTotalIncome income = new InvestmentTotalIncome();
            if (inverstParas.CircleType == 1)
            {
                income.Reward = inverstParas.Amount * ((decimal)(inverstParas.RewardRate / 100.0));
                income.ActualYearRate = (inverstParas.NominalYearRate * (1.0 - (inverstParas.OverheadsRate / 100.0))) + ((inverstParas.RewardRate / ((double)inverstParas.Circle)) * 12.0);
                income.ActualMonthRate = income.ActualYearRate / 12.0;
                double num = (inverstParas.NominalYearRate / 12.0) / 100.0;
                double num2 = Math.Pow(1.0 + num, (double)inverstParas.Circle);
                decimal num3 = 0M;
                if (num > 0.0)
                {
                    num3 = inverstParas.Amount * ((decimal)((num * num2) / (num2 - 1.0)));
                }
                else
                {
                    num3 = inverstParas.Amount / inverstParas.Circle;
                }
                income.TotalIncome = (num3 * inverstParas.Circle) + income.Reward;
                income.Interest = income.TotalIncome - inverstParas.Amount;
                income.Overheads = income.Interest * (((decimal)inverstParas.OverheadsRate) / 100M);
                return income;
            }
            if (inverstParas.CircleType == 3)
            {
                income = CalculateTotalIncomeWithLumpSumPrincipalAndInterest(inverstParas);
            }
            return income;
        }

        /// <summary>
        /// Calculates the total income with equal principal by quart.
        /// </summary>
        /// <param name="inverstParas">The inverst paras.</param>
        /// <returns>InvestmentTotalIncome.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 10:43:07
        private static InvestmentTotalIncome CalculateTotalIncomeWithEqualPrincipalByQuart(InvestmentParameters inverstParas)
        {
            InvestmentTotalIncome income = new InvestmentTotalIncome();
            if (inverstParas.CircleType == 1)
            {
                double num = Math.Ceiling((double)(((double)inverstParas.Circle) / 3.0));
                income.Reward = inverstParas.Amount * ((decimal)(inverstParas.RewardRate / 100.0));
                income.ActualYearRate = (inverstParas.NominalYearRate * (1.0 - (inverstParas.OverheadsRate / 100.0))) + ((inverstParas.RewardRate / num) * 4.0);
                income.ActualMonthRate = income.ActualYearRate / 12.0;
                decimal num2 = (((decimal)inverstParas.NominalYearRate) / 4M) / 100M;
                income.Interest = (inverstParas.Amount * ((decimal)(((double)Sum((int)num)) / num))) * num2;
                income.TotalIncome = inverstParas.Amount + income.Interest;
                income.Overheads = income.Interest * (((decimal)inverstParas.OverheadsRate) / 100M);
                return income;
            }
            if (inverstParas.CircleType == 3)
            {
                income = CalculateTotalIncomeWithLumpSumPrincipalAndInterest(inverstParas);
            }
            return income;
        }



        /// <summary>
        /// 以月计算利息的总收入 [以月付息到期还本]
        /// </summary>
        /// <param name="inverstParas">The inverst paras.</param>
        /// <returns>InvestmentTotalIncome.</returns>
        ///  <remarks>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 10:43:05
        /// </remarks>

        private static InvestmentTotalIncome CalculateTotalIncomeWithInterestByMonth(InvestmentParameters inverstParas)
        {
            InvestmentTotalIncome income = new InvestmentTotalIncome();
            if (inverstParas.CircleType == 1) //月
            {
                income.Reward = inverstParas.Amount * ((decimal)(inverstParas.RewardRate / 100.0));
                income.ActualYearRate = (inverstParas.NominalYearRate * (1.0 - (inverstParas.OverheadsRate / 100.0))) + ((inverstParas.RewardRate / ((double)inverstParas.Circle)) * 12.0);
                income.ActualMonthRate = income.ActualYearRate / 12.0;
                double num = (inverstParas.NominalYearRate / 12.0) / 100.0;
                decimal num2 = inverstParas.Amount * ((decimal)num);
                income.Interest = num2 * inverstParas.Circle;
                income.TotalIncome = (income.Interest + inverstParas.Amount) + income.Reward;
                income.Overheads = income.Interest * (((decimal)inverstParas.OverheadsRate) / 100M);
                return income;
            }
            if (inverstParas.CircleType == 3) //天
            {
                income = CalculateTotalIncomeWithLumpSumPrincipalAndInterest(inverstParas);
            }
            return income;
        }


        /// <summary>
        /// 计算总付本金和利息收入总额 天为单位
        /// </summary>
        /// <param name="inverstParas">The inverst paras.</param>
        /// <returns>InvestmentTotalIncome.</returns>
        ///  <remarks>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 10:43:02
        /// </remarks>
        private static InvestmentTotalIncome CalculateTotalIncomeWithLumpSumPrincipalAndInterest(InvestmentParameters inverstParas)
        {
            InvestmentTotalIncome income = new InvestmentTotalIncome();
            double circle = inverstParas.Circle;
            if (inverstParas.CircleType == 3)
            {
                circle = ((double)inverstParas.Circle) / 30.0;
            }
            income.Reward = inverstParas.Amount * ((decimal)(inverstParas.RewardRate / 100.0));
            income.ActualYearRate = (inverstParas.NominalYearRate * (1.0 - (inverstParas.OverheadsRate / 100.0))) + ((inverstParas.RewardRate / circle) * 12.0);
            income.ActualMonthRate = income.ActualYearRate / 12.0;
            income.Interest = inverstParas.Amount * ((decimal)(((inverstParas.NominalYearRate / 100.0) * inverstParas.Circle) / 365.0));
            income.TotalIncome = (inverstParas.Amount + income.Interest) + income.Reward;
            income.Overheads = income.Interest * (((decimal)inverstParas.OverheadsRate) / 100M);
            return income;
        }

        /// <summary>
        /// Corrects the total income.
        /// </summary>
        /// <param name="income">The income.</param>
        /// <param name="inverstParas">The inverst paras.</param>
        /// <returns>InvestmentTotalIncome.</returns>
        ///  <remarks>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 10:43:00
        /// </remarks>
        private static InvestmentTotalIncome CorrectTotalIncome(InvestmentTotalIncome income, InvestmentParameters inverstParas)
        {
            List<InvestmentReceiveRecordInfo> list = CalculateReceiveRecord(inverstParas);
            income.ReceiveRecords = list;
            decimal num = 0M;
            decimal num2 = 0M;
            foreach (InvestmentReceiveRecordInfo info in list)
            {
                num += info.Interest;
                num2 += info.Principal;
            }
            income.Interest = num;
            income.TotalIncome = (num + inverstParas.Amount) + income.Reward;
            income.CleanTotalIncome = income.TotalIncome - income.Overheads;
            income.TotalEarnings = income.TotalIncome - inverstParas.Amount;
            income.CleanTotalEarnings = income.TotalEarnings - income.Overheads;
            return income;
        }

        /// <summary>
        /// Formats the receive records.
        /// </summary>
        /// <param name="receiveRecords">The receive records.</param>
        /// <returns>List&lt;InvestmentReceiveRecordInfo&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 10:42:57
        private static List<InvestmentReceiveRecordInfo> FormatReceiveRecords(List<InvestmentReceiveRecordInfo> receiveRecords)
        {
            if (receiveRecords != null)
            {
                foreach (InvestmentReceiveRecordInfo info in receiveRecords)
                {
                    info.Amount = Math.Round(info.Amount, 2);
                    info.Balance = Math.Round(info.Balance, 2);
                    info.Interest = Math.Round(info.Interest, 2);
                    info.Overheads = Math.Round(info.Overheads, 2);
                    info.Principal = Math.Round(info.Principal, 2);
                }
            }
            return receiveRecords;
        }





        /// <summary>
        /// 计算利息返回分期记录列表
        /// </summary>
        /// <param name="Minverst">业务对象</param>
        /// <returns>List&lt;InvestmentReceiveRecordInfo&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 10:42:53
        public static List<InvestmentReceiveRecordInfo> Calculationofinterest(InvestmentParameters Minverst)
        {
            StringBuilder str = new StringBuilder();

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
            InvestmentTotalIncome income = InvestCalculator.CalculateTotalIncome(inverstParas);

            List<InvestmentReceiveRecordInfo> records = income.ReceiveRecords;

            return records;
        }


        private static InvestmentTotalIncome FormatTotalIncome(InvestmentTotalIncome income)
        {
            if (income != null)
            {
                income.TotalIncome = Math.Round(income.TotalIncome, 2);
                income.Reward = Math.Round(income.Reward, 2);
                income.Interest = Math.Round(income.Interest, 2);
                income.Overheads = Math.Round(income.Overheads, 2);
                income.CleanTotalIncome = income.TotalIncome - income.Overheads;
                income.ActualYearRate = Math.Round(income.ActualYearRate, 2);
                income.ActualMonthRate = Math.Round(income.ActualMonthRate, 2);
            }
            return income;
        }

        private static int Sum(int n)
        {
            if (n < 1)
            {
                return 0;
            }
            return (n + Sum(n - 1));
        }


    }
}
