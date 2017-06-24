using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model
{
    public class ImportErrorData
    {
        public string success { get; set; }

        public Error errors { get; set; }

        public Errorparamslist ErrorParams { get;set;}

        public class Error
        {
            
            public string mobile { get; set; }
        }


        public class Errorparamslist
        {

            public string moduleId { get; set; }

            public string name { get; set; }

            public string extend17 { get; set; }

            public string account { get; set; }

            public string extend5 { get; set; }

            public string extend45 { get; set; }

            public string password { get; set; }

            public string mobile { get; set; }

            public string requestSource { get; set; }

            public string action { get; set; }

            public string format { get; set; }
            
            public string controller { get; set; }

            public string u { get; set; }

            public string extend1 { get; set; }

            public string extend34 { get; set; }

            public string extend16 { get; set; }

            
        }
    }
}
