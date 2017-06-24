using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model
{
    public class Return_EmployeeDetails
    {
        public string success { get; set; }

        public List<EmployeeDetails> data { get; set; }
        public int total { get; set; }
    }
}
