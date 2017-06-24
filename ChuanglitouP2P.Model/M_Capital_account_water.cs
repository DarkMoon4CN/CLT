using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
    /// 资金账户流水
	///
	/// </summary>
	[Serializable]
	public partial class M_Capital_account_water
	{
		public M_Capital_account_water()
		{}
		#region Model
		private int _account_water_id;
		private int _membertable_registerid;
		private decimal _income;
		private decimal _expenditure;
		private DateTime _time_of_occurrence;
		private decimal _account_balance;
		private int _types_finance;
		private DateTime _createtime;
        private int _keyid;
        private string _remarks;

		/// <summary>
        /// 帐户流水id
		/// </summary>
		public int account_water_id
		{
			set{ _account_water_id=value;}
			get{return _account_water_id;}
		}
		/// <summary>
        /// 用户注册id
		/// </summary>
		public int membertable_registerid
		{
			set{ _membertable_registerid=value;}
			get{return _membertable_registerid;}
		}
		/// <summary>
        /// 收入
		/// </summary>
		public decimal income
		{
			set{ _income=value;}
			get{return _income;}
		}
		/// <summary>
        ///支出
		/// </summary>
		public decimal expenditure
		{
			set{ _expenditure=value;}
			get{return _expenditure;}
		}
		/// <summary>
        /// 发生时间
		/// </summary>
		public DateTime time_of_occurrence
		{
			set{ _time_of_occurrence=value;}
			get{return _time_of_occurrence;}
		}
		/// <summary>
        /// 账户余额
		/// </summary>
		public decimal account_balance
		{
			set{ _account_balance=value;}
			get{return _account_balance;}
		}
		/// <summary>
        /// 资金类型
		/// </summary>
		public int types_Finance
		{
			set{ _types_finance=value;}
			get{return _types_finance;}
		}
		/// <summary>
        /// 生成时间
		/// </summary>
		public DateTime createtime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}


        /// <summary>
        /// 关键主键id
        /// </summary>
        public int keyid
        {
            set { _keyid = value; }
            get { return _keyid; }
        }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string remarks
        {
            set { _remarks = value; }
            get { return _remarks; }
        
        }
		#endregion Model

	}
}

