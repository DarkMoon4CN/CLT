using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model
{
    public class M_ActivityFlowWater
    {

        /// <summary>
        /// 流水ID
        /// </summary>		
        private int _activityflowwaterid;
        public int ActivityFlowWaterID
        {
            get { return _activityflowwaterid; }
            set { _activityflowwaterid = value; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>		
        private int _registerid;
        public int RegisterID
        {
            get { return _registerid; }
            set { _registerid = value; }
        }
        /// <summary>
        /// 流米充值订单流水号
        /// </summary>		
        private string _orderno;
        public string OrderNO
        {
            get { return _orderno; }
            set { _orderno = value; }
        }
        /// <summary>
        /// 业务类型 0注册，1投资
        /// </summary>		
        private int _watertype;
        public int WaterType
        {
            get { return _watertype; }
            set { _watertype = value; }
        }
        /// <summary>
        /// 发生时间
        /// </summary>		
        private DateTime _createtime;
        public DateTime CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }

    }
}
