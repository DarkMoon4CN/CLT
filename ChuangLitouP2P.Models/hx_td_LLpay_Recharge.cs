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
    
    public partial class hx_td_LLpay_Recharge
    {
        public int Rechargeid { get; set; }
        public Nullable<int> UsrId { get; set; }
        public string no_order { get; set; }
        public string ordertime { get; set; }
        public string info_order { get; set; }
        public Nullable<decimal> money_order { get; set; }
        public string BankCode { get; set; }
        public string card_no { get; set; }
        public Nullable<int> ReState { get; set; }
        public string oid_paybill { get; set; }
    }
}
