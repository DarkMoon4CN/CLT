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
    
    public partial class hx_td_menu
    {
        public int menu_id { get; set; }
        public string menu_name { get; set; }
        public Nullable<int> parentid { get; set; }
        public string parentpath { get; set; }
        public Nullable<int> depath { get; set; }
        public Nullable<int> rootid { get; set; }
        public Nullable<int> child { get; set; }
        public Nullable<int> previd { get; set; }
        public Nullable<int> nextid { get; set; }
        public Nullable<int> orderid { get; set; }
        public Nullable<System.DateTime> createtime { get; set; }
        public string path1 { get; set; }
        public string path2 { get; set; }
        public string path3 { get; set; }
        public string path4 { get; set; }
    }
}
