
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// td_frozen:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_td_frozen
	{
		public M_td_frozen()
		{}
		#region Model
		private int _frozenid;
		private int _mbt_registerid;
		private decimal _frozenidamount;
		private string _frozenidno;
		private int _frozenstate;
		private DateTime _frozendate;
		private int _targetid;

        private string _UsrCustId;

        private int _bid_records_id;

		/// <summary>
		/// 
		/// </summary>
		public int Frozenid
		{
			set{ _frozenid=value;}
			get{return _frozenid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int MBT_Registerid
		{
			set{ _mbt_registerid=value;}
			get{return _mbt_registerid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal FrozenidAmount
		{
			set{ _frozenidamount=value;}
			get{return _frozenidamount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FrozenidNo
		{
			set{ _frozenidno=value;}
			get{return _frozenidno;}
		}
		/// <summary>
		/// 0 默认(冻结未成功)  1 冻结成功  2 交易成功
		/// </summary>
		public int FrozenState
		{
			set{ _frozenstate=value;}
			get{return _frozenstate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime FrozenDate
		{
			set{ _frozendate=value;}
			get{return _frozendate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int targetid
		{
			set{ _targetid=value;}
			get{return _targetid;}
		}

        public string UsrCustId
        {
            set { _UsrCustId = value; }
            get { return _UsrCustId; }
        }


        public int bid_records_id
        {
            set { _bid_records_id = value; }
            get { return _bid_records_id; }
        }
		#endregion Model

	}
}

