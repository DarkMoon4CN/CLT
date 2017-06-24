using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model
{
    public class Return_Employee
    {
        public string success { get; set; }
        /// 
        /// </summary>
        public List<Employee> data { get; set; }

        public int total { get; set; }
        public List<string> summary { get; set; }
    }
}
