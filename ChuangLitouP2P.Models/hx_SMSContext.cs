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
    
    public partial class hx_SMSContext
    {
        public int SmsID { get; set; }
        public Nullable<int> ActID { get; set; }
        public string SmsOne { get; set; }
        public string SmsThree { get; set; }
        public string SmsSeven { get; set; }
        public string SmsFifteen { get; set; }
        public string SmsSixteen { get; set; }
        public Nullable<System.DateTime> createtime { get; set; }
    
        public virtual hx_ActivityTable hx_ActivityTable { get; set; }
    }
}
