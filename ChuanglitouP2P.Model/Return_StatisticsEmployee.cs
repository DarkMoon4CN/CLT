using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model
{
    public class Return_StatisticsEmployee
    {
        public string success { get; set; }

        public List<StatisticsEmployee> data { get; set; }

        public int total { get; set; }
    }
}
