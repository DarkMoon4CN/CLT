//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChuangLitouP2P.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class V_incomeborr_count
    {
        public int income_statement_id { get; set; }
        public Nullable<int> targetid { get; set; }
        public int bid_records_id { get; set; }
        public Nullable<decimal> loan_number { get; set; }
        public Nullable<int> borrower_registerid { get; set; }
        public string OutCustId { get; set; }
        public Nullable<decimal> annual_revenue { get; set; }
        public Nullable<decimal> investment_amount { get; set; }
        public Nullable<System.DateTime> invest_time { get; set; }
        public Nullable<int> current_investment_period { get; set; }
        public Nullable<System.DateTime> value_date { get; set; }
        public Nullable<System.DateTime> interest_payment_date { get; set; }
        public Nullable<decimal> repayment_amount { get; set; }
        public Nullable<System.DateTime> repayment_period { get; set; }
        public Nullable<int> investor_registerid { get; set; }
        public string InCustId { get; set; }
        public Nullable<int> payment_status { get; set; }
        public Nullable<decimal> Principal { get; set; }
        public Nullable<decimal> interestpayment { get; set; }
        public string orderid { get; set; }
        public Nullable<decimal> BidOrderid { get; set; }
        public Nullable<int> interestDay { get; set; }
        public Nullable<int> TotalInstallments { get; set; }
        public Nullable<decimal> BorrFees { get; set; }
        public Nullable<decimal> InveFess { get; set; }
        public Nullable<decimal> loan_management_fee { get; set; }
        public Nullable<decimal> service_charge { get; set; }
    }
}
