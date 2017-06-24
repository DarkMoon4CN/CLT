using ChuanglitouP2P.Common;
using ChuanglitouP2P.DAL;
using ChuanglitouP2P.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.BLL.Api
{
    public class ServiceFeesLogic : IDisposable
    {
        D_Holiday d_Holiday = new D_Holiday();
        /// <summary>
        /// 获取提现手续费
        /// </summary>
        /// <param name="withdrawalType">提现方式.0:普通提现;1:快速提现；2:即时提现.默认值为0</param>
        /// <param name="withdrawalAmount">提现金额</param>
        /// <returns></returns>
        public M_WithdrawalCash GetWithdrawalCashFees(int withdrawalType, Decimal withdrawalAmount)
        {
            DateTime withdrawalTime = DateTime.Now;
            M_WithdrawalCash result = new M_WithdrawalCash();
            result.Amount = withdrawalAmount;
            result.Type = withdrawalType;
            result.ArrivalDate = GetArrivalDate(withdrawalType, withdrawalTime);
            result.ServiceFees = GetServiceFees(withdrawalType, withdrawalAmount, withdrawalTime);
            switch (withdrawalType)
            {
                case 2:
                    result.Limit = 200000.00M;
                    result.Payer = FeesPayerEnum.Remittee;
                    break;
                default:
                    result.Limit = 0.00M;
                    result.Payer = FeesPayerEnum.Platform;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 获取提现到用户账上的预估时间
        /// </summary>
        /// <param name="withdrawalType">提现方式.0:普通提现;1:快速提现；2:即时提现.默认值为0</param>
        /// <param name="withdrawalTime">提现时间</param>
        /// <returns></returns>
        public DateTime GetArrivalDate(int withdrawalType, DateTime withdrawalTime)
        {
            var item = d_Holiday.GetHolidayBounds(withdrawalTime);
            switch (withdrawalType)
            {

                case 1://FAST
                    if (item != null)
                        return item.Endtime.AddDays(1);
                    else
                    {
                        if (withdrawalTime < Convert.ToDateTime(withdrawalTime.ToString("yyyy-MM-dd") + " 14:30:00"))
                            return withdrawalTime;
                        else
                            return withdrawalTime.AddDays(1);
                    }
                case 2://IMMEDIATE
                    return withdrawalTime.AddHours(2);
                case 0://GENERAL
                default:
                    if (item != null)
                        return item.Endtime.AddDays(1);
                    else
                        return withdrawalTime.AddDays(1);
            }
        }
        /// <summary>
        /// 获取提现服务费用
        /// </summary>
        /// <param name="withdrawalType">提现方式.0:普通提现;1:快速提现；2:即时提现.默认值为0</param>
        /// <param name="amount">提现金额</param>
        /// <param name="withdrawalTime">提现时间</param>
        /// <returns></returns>
        public Decimal GetServiceFees(int withdrawalType, Decimal amount, DateTime withdrawalTime)
        {
            var item = d_Holiday.GetHolidayBounds(withdrawalTime);
            switch (withdrawalType)
            {
                case 1://FAST
                    if (item != null)
                        return 2.00M;
                    else
                    {
                        if (withdrawalTime < Convert.ToDateTime(withdrawalTime.ToString("yyyy-MM-dd") + " 14:30:00"))
                        {
                            var temp = Math.Round(amount * 5 / 10000, 2, MidpointRounding.AwayFromZero);
                            return temp + 2.00M;
                        }
                        else
                            return 2.00M;
                    }
                case 2://IMMEDIATE
                    if (item != null)
                    {
                        var duringDays = (Convert.ToDateTime(item.Endtime.ToString("yyyy-MM-dd")) - Convert.ToDateTime(withdrawalTime.ToString("yyyy-MM-dd"))).TotalDays;
                        var temp = Math.Round(amount * 5 / 10000, 2, MidpointRounding.AwayFromZero) * Convert.ToDecimal(duringDays + 1);
                        return temp + 2.00M;
                    }
                    else
                    {
                        var temp = Math.Round(amount * 5 / 10000, 2, MidpointRounding.AwayFromZero);
                        return temp + 2.00M;
                    }
                case 0://GENERAL
                default:
                    return 2.00M;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.Collect();
            }
            else
                GC.SuppressFinalize(this);
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        ~ServiceFeesLogic()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(false);
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
