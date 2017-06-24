
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// td_System_message:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_td_System_message
	{
		public M_td_System_message()
		{}
		#region Model
		private int _messageid;
		private string _mtitle;
		private DateTime _pubtime= DateTime.Now;
		private string _mcontext;
		private int _mstate;
		private string _murl;
		private int _mreg=0;
        private int _Mtype = 0;

		/// <summary>
		/// 
		/// </summary>
		public int Messageid
		{
			set{ _messageid=value;}
			get{return _messageid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MTitle
		{
			set{ _mtitle=value;}
			get{return _mtitle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime PubTime
		{
			set{ _pubtime=value;}
			get{return _pubtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MContext
		{
			set{ _mcontext=value;}
			get{return _mcontext;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Mstate
		{
			set{ _mstate=value;}
			get{return _mstate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MUrl
		{
			set{ _murl=value;}
			get{return _murl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int MReg
		{
			set{ _mreg=value;}
			get{return _mreg;}
		}


        /// <summary>
        /// 消息类型  0系统消息 1投资通知   2收益通知   3提现 4充值 5系统通知
        /// </summary>
        public int Mtype
        {
            set { _Mtype = value; }
            get { return _Mtype; }
        }
		#endregion Model

	}
}

