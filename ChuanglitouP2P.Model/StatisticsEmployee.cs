using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model
{
    public class StatisticsEmployee
    {
        public string id { get; set; }//员工ID

        public string deptId { get; set; }//部门ID

        public string employeeName { get; set; }//坐席名称

        public string deptName { get; set; }//部门名称

        public string exten { get; set; }//分机号

        public string callOutTime { get; set; }//外呼总时长

        public string callOutNumber { get; set; }//外呼电话数

        public string callOutFailNumber { get; set; }//外呼失败数

        public string callOutSuccessNumber { get; set; }//外呼成功数

        public string callOutValidNumber { get; set; }//外呼有效数

        public string callInTime { get; set; }//接听总时长

        public string callInFailNumber { get; set; }//未接电话数量

        public string callInSuccessNumber { get; set; }//已接电话数
    }
}
