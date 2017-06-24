using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model
{
    public class EmployeeDetails
    {
        public string calledNumber { get; set; }//被叫号码

        public string dateCreated { get; set; }//创建时间

        public string endTime { get; set; }//呼叫结束时间

        public string lastUpdated { get; set; }//修改时间

        public string seatNumber { get; set; }//坐席号码

        public string startTime { get; set; }//呼叫接通开始时间

        public string timeSpan { get; set; }//通话时长

        public string callCity { get; set; }//被叫城市

        public string calledProvince { get; set; }//被叫省份

        public Employee data { get; set; }//所有者（关联员工）
    }
}
