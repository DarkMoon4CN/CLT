using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
    /// 投标记录
	/// 用户投标
	/// </summary>
	[Serializable]
	public partial class M_Bid_records
	{
		public M_Bid_records()
		{}
		#region Model
		private int _bid_records_id;
		private int _borrower_registerid;
		private int _targetid;
        private decimal _loan_number;
		private decimal _annual_interest_rate;
		private int _current_period;
		private decimal _investment_amount;
		private DateTime _value_date;
		private DateTime _investment_maturity;
		private DateTime _invest_time;
		private int _invest_state;
		private int _flow_return;
		private decimal _repayment_amount;
		private DateTime _repayment_period;
		private int _investor_registerid;
		private int _payment_status;
        private decimal _withoutinterest;

        private decimal _haveinterest;

        private string _invitationcode;

        private decimal _OrdId;

        private decimal _JiaxiNum;

        private decimal _BonusAmt;


        /// <summary>
        /// 奖励金额
        /// </summary>
        public decimal BonusAmt
        {
            set { _BonusAmt = value; }
            get { return _BonusAmt; }
        }

        /// <summary>
        /// 加息数量
        /// </summary>
        public decimal JiaxiNum
        {
            set { _JiaxiNum = value; }
            get { return _JiaxiNum; }
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
		/// 借款人id
		/// </summary>
		public int borrower_registerid
		{
			set{ _borrower_registerid=value;}
			get{return _borrower_registerid;}
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
		/// 借款编号
		/// </summary>
        public decimal loan_number
		{
			set{ _loan_number=value;}
			get{return _loan_number;}
		}
		/// <summary>
		/// 年化收益
		/// </summary>
		public decimal annual_interest_rate
		{
			set{ _annual_interest_rate=value;}
			get{return _annual_interest_rate;}
		}
		/// <summary>
		/// 分期总数
		/// </summary>
		public int current_period
		{
			set{ _current_period=value;}
			get{return _current_period;}
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
		/// 起息日
		/// </summary>
		public DateTime value_date
		{
			set{ _value_date=value;}
			get{return _value_date;}
		}
		/// <summary>
		/// 投资到期日
		/// </summary>
		public DateTime investment_maturity
		{
			set{ _investment_maturity=value;}
			get{return _investment_maturity;}
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
        /// 投次状态
		/// 1 成功  2 失败  3流标返还 
        ///如果是投标返还的话通过触发器更改对应金额问题
		/// </summary>
		public int invest_state
		{
			set{ _invest_state=value;}
			get{return _invest_state;}
		}
		/// <summary>
        /// 流标返还
		/// 1 无返还  2已返还
		/// </summary>
		public int flow_return
		{
			set{ _flow_return=value;}
			get{return _flow_return;}
		}
		/// <summary>
        /// 还款金额
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
        /// 还款状态
		/// 0 未还款    1 借款人自己还款  2 平台代还
		/// </summary>
		public int payment_status
		{
			set{ _payment_status=value;}
			get{return _payment_status;}
		}

        /// <summary>
        /// 未付息
        /// </summary>
        public decimal withoutinterest
        {
            set { _withoutinterest = value; }
            get { return _withoutinterest; }
        }

        /// <summary>
        /// 已付息
        /// </summary>
        public decimal haveinterest
        {
            set { _haveinterest=value;}
            get{return _haveinterest;}
        }

        /// <summary>
        /// 邀请码
        /// </summary>
        public string invitationcode
        {
            set { _invitationcode = value; }
            get { return _invitationcode; }
        }


        /// <summary>
        /// 投标订单id
        /// </summary>
        public decimal OrdId {

            set { _OrdId = value; }
            get { return _OrdId; }
        }
     

   
		#endregion Model

	}
}

