using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model
{
    public class HolidayEntity
    {
        public string Name { get; set; }
        public string Years { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// 1:正常节假日；2：要上班的休息日
        /// </summary>
        public int Status { get; set; }
    }
}
