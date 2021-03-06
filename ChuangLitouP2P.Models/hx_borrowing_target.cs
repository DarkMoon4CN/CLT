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
    
    public partial class hx_borrowing_target
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public hx_borrowing_target()
        {
            this.Contract_management = new HashSet<Contract_management>();
            this.hx_borrowing_target_detailed = new HashSet<hx_borrowing_target_detailed>();
        }
    
        public int targetid { get; set; }
        public Nullable<int> borrower_registerid { get; set; }
        public decimal loan_number { get; set; }
        public string borrowing_title { get; set; }
        public string borrowing_thumbnail { get; set; }
        public int project_type_id { get; set; }
        public Nullable<decimal> annual_interest_rate { get; set; }
        public Nullable<decimal> borrowing_balance { get; set; }
        public Nullable<int> life_of_loan { get; set; }
        public Nullable<int> unit_day { get; set; }
        public Nullable<System.DateTime> release_date { get; set; }
        public Nullable<System.DateTime> value_date { get; set; }
        public Nullable<int> month_payment_date { get; set; }
        public Nullable<System.DateTime> repayment_date { get; set; }
        public Nullable<decimal> minimum { get; set; }
        public Nullable<decimal> maxmum { get; set; }
        public Nullable<int> companyid { get; set; }
        public int guarantee_way_id { get; set; }
        public Nullable<int> payment_options { get; set; }
        public Nullable<System.DateTime> start_time { get; set; }
        public Nullable<System.DateTime> end_time { get; set; }
        public Nullable<decimal> service_charge { get; set; }
        public Nullable<decimal> loan_management_fee { get; set; }
        public Nullable<decimal> investors_management_fee { get; set; }
        public Nullable<decimal> ordinary_overdue_management_fees { get; set; }
        public Nullable<decimal> seriously_overdue_management_fees { get; set; }
        public Nullable<decimal> ordinary_overdue_penalty { get; set; }
        public Nullable<decimal> seriously_overdue_penalty { get; set; }
        public Nullable<decimal> transfer_Expenses { get; set; }
        public Nullable<decimal> fundraising_amount { get; set; }
        public Nullable<int> tender_state { get; set; }
        public Nullable<int> full_scale_loan { get; set; }
        public Nullable<int> flow_return { get; set; }
        public Nullable<int> recommend { get; set; }
        public Nullable<System.DateTime> sys_time { get; set; }
        public Nullable<int> repaymentperiods { get; set; }
        public string reviewremarks { get; set; }
        public string recheckremarks { get; set; }
        public Nullable<decimal> guarantee_fee { get; set; }
        public Nullable<decimal> consultingAMT { get; set; }
        public Nullable<decimal> guaranteeAMT { get; set; }
        public Nullable<decimal> B_Rates { get; set; }
        public Nullable<decimal> H_Repayment_Amt { get; set; }
        public Nullable<System.DateTime> Repay_Time { get; set; }
        public string G_contract_Path { get; set; }
        public Nullable<int> IsUse { get; set; }
        public Nullable<int> indexorder { get; set; }
        public Nullable<int> isDel { get; set; }
        public Nullable<int> IsIRC { get; set; }
        public string Purpose { get; set; }
        public string PaySource { get; set; }
        public string Collateral { get; set; }
        public Nullable<int> Isinterest { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contract_management> Contract_management { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<hx_borrowing_target_detailed> hx_borrowing_target_detailed { get; set; }
    }
}
