using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model
{
    //ActivityFlow
    public class M_ActivityFlow
    {

        /// <summary>
        /// 活动送流量ID
        /// </summary>		
        private int _activityflowid;
        public int ActivityFlowID
        {
            get { return _activityflowid; }
            set { _activityflowid = value; }
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
        /// 注册实名是否发放 0未发放，1已发放
        /// </summary>		
        private int _activityflowtype;
        public int ActivityFlowType
        {
            get { return _activityflowtype; }
            set { _activityflowtype = value; }
        }
        /// <summary>
        /// 投资是否发放 0未发放，1已发放,2未达到投资返流量标准
        /// </summary>		
        private int _isgrant;
        public int IsGrant
        {
            get { return _isgrant; }
            set { _isgrant = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>		
        private DateTime _createtime;
        public DateTime CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }
     
        
    }
}