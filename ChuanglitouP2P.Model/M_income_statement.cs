using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
    /// 投资人利息收入表
	/// </summary>
	[Serializable]
	public partial class M_income_statement
	{
		public M_income_statement()
		{}
		#region Model
		private int _income_statement_id;
		private int _targetid;
		private int _bid_records_id;
        private decimal _loan_number;
		private int _borrower_registerid;
        private string _OutCustId;
		private decimal _annual_revenue;
		private decimal _investment_amount;
		private DateTime _invest_time;
		private int _current_investment_period;
		private DateTime _value_date;
		private DateTime _interest_payment_date;
		private decimal _repayment_amount;
		private DateTime _repayment_period;
		private int _investor_registerid;

        private string _InCustId;
		private int _payment_status;

        private decimal _Principal;
        private decimal _interestpayment;

        private decimal _BidOrderid;

        private int _interestDay;

        private int _TotalInstallments;

		/// <summary>
        /// 投资明细id
		/// </summary>
		public int income_statement_id
		{
			set{ _income_statement_id=value;}
			get{return _income_statement_id;}
		}
		/// <summary>
        /// 标的id
		/// </summary>
		public int targetid
		{
			set{ _targetid=value;}
			get{return _targetid;}
		}
		/// <summary>
        /// 投标记录id
		/// </summary>
		public int bid_records_id
		{
			set{ _bid_records_id=value;}
			get{return _bid_records_id;}
		}
		/// <summary>
        /// 借款编号
		/// </summary>
        public decimal loan_number
		{
			set{ _loan_number=value;}
			get{return _loan_number;}
		}
		/// <summary>
        /// 借款人id
		/// </summary>
		public int borrower_registerid
		{
			set{ _borrower_registerid=value;}
			get{return _borrower_registerid;}
		}

        /// <summary>
        /// 借款人客户id
        /// </summary>
        public string OutCustId
        {
            set { _OutCustId = value; }
            get { return _OutCustId; }
        }

		/// <summary>
        /// 年化收益
		/// </summary>
		public decimal annual_revenue
		{
			set{ _annual_revenue=value;}
			get{return _annual_revenue;}
		}
		/// <summary>
        /// 投资金额
		/// </summary>
		public decimal investment_amount
		{
			set{ _investment_amount=value;}
			get{return _investment_amount;}
		}
		/// <summary>
        /// 投资时间
		/// </summary>
		public DateTime invest_time
		{
			set{ _invest_time=value;}
			get{return _invest_time;}
		}
		/// <summary>
        /// 当前投资期数
		/// </summary>
		public int current_investment_period
		{
			set{ _current_investment_period=value;}
			get{return _current_investment_period;}
		}
		/// <summary>
        /// 起息日
		/// </summary>
		public DateTime value_date
		{
			set{ _value_date=value;}
			get{return _value_date;}
		}
		/// <summary>
        /// 付息日
		/// </summary>
		public DateTime interest_payment_date
		{
			set{ _interest_payment_date=value;}
			get{return _interest_payment_date;}
		}
		/// <summary>
        /// 还款金额/应收本息
		/// </summary>
		public decimal repayment_amount
		{
			set{ _repayment_amount=value;}
			get{return _repayment_amount;}
		}
		/// <summary>
        /// 还款时间
		/// </summary>
		public DateTime repayment_period
		{
			set{ _repayment_period=value;}
			get{return _repayment_period;}
		}
		/// <summary>
        /// 投资人id
		/// </summary>
		public int investor_registerid
		{
			set{ _investor_registerid=value;}
			get{return _investor_registerid;}
		}

        /// <summary>
        /// 投资人客户id
        /// </summary>
        public string InCustId
        {
            set { _InCustId = value; }
            get { return _InCustId; }
        }

		/// <summary>
        /// 还款状态
		/// 0 未还款    1 借款人自己还款  2 平台代还
		/// </summary>
		public int payment_status
		{
			set{ _payment_status=value;}
			get{return _payment_status;}
		}

        /// <summary>
        /// 本金
        /// </summary>
        public decimal Principal
        {
            set { _Principal = value; }
            get { return _Principal; }
        }


        /// <summary>
        /// 每期利息
        /// </summary>
        public decimal interestpayment
        {
            set { _interestpayment = value;}
            get { return _interestpayment; }
        }

        /// <summary>
        /// 投资标的订单id
        /// </summary>
        public decimal BidOrderid
        {
            set { _BidOrderid = value; }
            get { return _BidOrderid; }
        }

        /// <summary>
        /// 计息天数
        /// </summary>
        public int interestDay
        {
            set { _interestDay = value; }
            get { return _interestDay; }
        }

        /// <summary>
        /// 总期数
        /// </summary>
        public int TotalInstallments
        {
            set { _TotalInstallments = value; }
            get { return _TotalInstallments; }
        
        }

		#endregion Model

	}
}

