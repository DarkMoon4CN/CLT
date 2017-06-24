using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// td_SMS_record:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_td_SMS_record
	{
		public M_td_SMS_record()
		{}
		#region Model
		private int _sms_record_id;
		private int  _senduserid;
		private string _phone_number;
		private string _smscontext;
		private int  _smstype;
		private DateTime  _sendtime= DateTime.Now;



        private decimal _orderid;

        private string _vcode;

        private string _ip;

		/// <summary>
		/// 
		/// </summary>
		public int sms_record_id
		{
			set{ _sms_record_id=value;}
			get{return _sms_record_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int  senduserid
		{
			set{ _senduserid=value;}
			get{return _senduserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string phone_number
		{
			set{ _phone_number=value;}
			get{return _phone_number;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string smscontext
		{
			set{ _smscontext=value;}
			get{return _smscontext;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int  smstype
		{
			set{ _smstype=value;}
			get{return _smstype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime  sendtime
		{
			set{ _sendtime=value;}
			get{return _sendtime;}
		}

        public decimal orderid
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        public string vcode
        {
            set { _vcode = value; }
            get { return _vcode; }
        }

        public string ip 
        {
            set { _ip = value; }
            get { return _ip; }
        }

		#endregion Model

	}
}

