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
    
    public partial class Overdue_repayment
    {
        public int overdue_repayment_id { get; set; }
        public Nullable<int> repayment_plan_id { get; set; }
        public Nullable<int> membertable_registerid { get; set; }
        public Nullable<int> targetid { get; set; }
        public Nullable<decimal> loan_management_fee { get; set; }
        public Nullable<decimal> ordinary_overdue_management_fees { get; set; }
        public Nullable<decimal> seriously_overdue_management_fees { get; set; }
        public Nullable<decimal> ordinary_overdue_penalty { get; set; }
        public Nullable<decimal> seriously_overdue_penalty { get; set; }
        public Nullable<System.DateTime> actual_repayment_time { get; set; }
        public Nullable<int> overdue_days { get; set; }
        public Nullable<int> interest_state { get; set; }
    }
}