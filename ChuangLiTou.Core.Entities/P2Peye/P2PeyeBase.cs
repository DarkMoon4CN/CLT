using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.P2Peye
{
    public class P2PeyeBase
    {
        public string result_code { get; set; }
        public string result_msg { get; set; }
        public int page_count { get; set; }
        public int page_index { get; set; }
    }
}
