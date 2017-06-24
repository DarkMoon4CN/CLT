using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Response.SmsEmail
{
    public class SmsRecordEntity
    {
        #region Model
        private int _sms_record_id;
        private int? _senduserid;
        private string _phone_number;
        private string _smscontext;
        private int? _smstype = 0;
        private DateTime? _sendtime = DateTime.Now;
        private decimal? _orderid;
        private string _vcode;
        private int? _hits = 1;
        private string _ip;
        /// <summary>
        /// 
        /// </summary>
        public int sms_record_id
        {
            set { _sms_record_id = value; }
            get { return _sms_record_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? senduserid
        {
            set { _senduserid = value; }
            get { return _senduserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string phone_number
        {
            set { _phone_number = value; }
            get { return _phone_number; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string smscontext
        {
            set { _smscontext = value; }
            get { return _smscontext; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? smstype
        {
            set { _smstype = value; }
            get { return _smstype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? sendtime
        {
            set { _sendtime = value; }
            get { return _sendtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? orderid
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string vcode
        {
            set { _vcode = value; }
            get { return _vcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? hits
        {
            set { _hits = value; }
            get { return _hits; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ip
        {
            set { _ip = value; }
            get { return _ip; }
        }
        #endregion Model
    }
}
