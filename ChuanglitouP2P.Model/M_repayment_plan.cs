using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
    ///借款人还款计划表
	/// </summary>
	[Serializable]
	public partial class M_repayment_plan
	{
		public M_repayment_plan()
		{}
		#region Model
		private int _repayment_plan_id;
		private int _borrower_registerid;
        private string _BorrUsrCustId;
		private int _targetid;
		private int _current_period;
		private DateTime _repayment_period;
		private int _repayment_type;
		private decimal _repayment_amount;
		private decimal _actual_amount_repayment;
		private int _repayment_state;
		private DateTime _createtime;

        private decimal _interestpayment;

        private decimal _fees;

        private decimal _O_penalty;

        private decimal _shall_repayment;


		/// <summary>
        /// 返款计划id
		/// </summary>
		public int repayment_plan_id
		{
			set{ _repayment_plan_id=value;}
			get{return _repayment_plan_id;}
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
        /// 借款人客户号
        /// </summary>
        public string BorrUsrCustId
        {
            set { _BorrUsrCustId = value; }
            get { return _BorrUsrCustId; }
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
        /// 当前期数
		/// </summary>
		public int current_period
		{
			set{ _current_period=value;}
			get{return _current_period;}
		}
		/// <summary>
        /// 预计还款时间
		/// </summary>
		public DateTime repayment_period
		{
			set{ _repayment_period=value;}
			get{return _repayment_period;}
		}
		/// <summary>
        /// 还款类型
		/// 1 利息 2本金
		/// </summary>
		public int repayment_type
		{
			set{ _repayment_type=value;}
			get{return _repayment_type;}
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
        /// 实际还款金额
		/// </summary>
		public decimal actual_amount_repayment
		{
			set{ _actual_amount_repayment=value;}
			get{return _actual_amount_repayment;}
		}
		/// <summary>
        /// 还款状态
		/// 0 未还款   1已还款   2逾期未还  3 平台代还 
		/// </summary>
		public int repayment_state
		{
			set{ _repayment_state=value;}
			get{return _repayment_state;}
		}
		/// <summary>
        /// 生成时间
		/// 系统默认生成
		/// </summary>
		public DateTime createtime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}

        /// <summary>
        /// 利息
        /// </summary>
        public decimal interestpayment
        {
            set { _interestpayment = value; }
            get { return _interestpayment; }
        
        }

        /// <summary>
        /// 管理费
        /// </summary>
        public decimal fees
        {
            set { _fees = value; }
            get { return _fees;  }
        }

        /// <summary>
        /// 逾期罚息
        /// </summary>
        public decimal O_penalty
        {
            set { _O_penalty = value; }
            get { return _O_penalty; }       
        
        }

        /// <summary>
        /// 应还款总额
        /// </summary>
        public decimal shall_repayment
        {
            set { _shall_repayment = value; }
            get { return _shall_repayment; }
        }



		#endregion Model

	}
}

