using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model.chinapnr.ChinapnrAttribute
{
    /// <summary>
    ///  设置签名顺序
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ChkValueSort : System.Attribute
    {
        public int Index { get; set; }

        public ChkValueSort(int index)
        {
            this.Index = index;
        }
    }
}
