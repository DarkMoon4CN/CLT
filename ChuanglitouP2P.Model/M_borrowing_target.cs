using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
    /// 借款标的
	/// 借款方图片类型 1
	///   胆保方图片类型 2
	/// </summary>
	[Serializable]
	public partial class M_borrowing_target
	{
		public M_borrowing_target()
		{}
		#region Model
		private int _targetid;
		private int _borrower_registerid;
        private decimal _loan_number;
		private string _borrowing_title;
		private string _borrowing_thumbnail;
		private int _project_type_id;
		private decimal _annual_interest_rate;
		private decimal _borrowing_balance;
		private int _life_of_loan;
		private int _unit_day;
		private DateTime _release_date;
		private DateTime _value_date;
		private int _month_payment_date;
		private DateTime _repayment_date;
		private decimal _minimum;
		private decimal _maxmum;
		private int _companyid;
		private int _guarantee_way_id;
		private int _payment_options;
		private DateTime _end_time;
		private decimal _service_charge;
		private decimal _loan_management_fee;
		private decimal _investors_management_fee;
		private decimal _ordinary_overdue_management_fees;
		private decimal _seriously_overdue_management_fees;
		private decimal _ordinary_overdue_penalty;
		private decimal _seriously_overdue_penalty;
		private decimal _transfer_expenses;
		private decimal _fundraising_amount;
		private int _tender_state;
		private int _full_scale_loan;
		private int _flow_return;
		private int _recommend;
		private DateTime _sys_time;
		private int _repayment_periods;
        private decimal _guarantee_fee;

        private decimal _consultingAMT;

        private decimal _guaranteeAMT;

        private decimal _B_Rates;

        private DateTime _start_time;

        private int _IsUse;

     

        


		/// <summary>
        /// 标的id
		/// </summary>
		public int targetid
		{
			set{ _targetid=value;}
			get{return _targetid;}
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
        /// 借款编号(合同编号)
		/// 用于合同编号
		/// </summary>
        public decimal loan_number
		{
			set{ _loan_number=value;}
			get{return _loan_number;}
		}
		/// <summary>
        /// 借款标题
		/// </summary>
		public string borrowing_title
		{
			set{ _borrowing_title=value;}
			get{return _borrowing_title;}
		}
		/// <summary>
        /// 借款缩略图
		/// </summary>
		public string borrowing_thumbnail
		{
			set{ _borrowing_thumbnail=value;}
			get{return _borrowing_thumbnail;}
		}
		/// <summary>
        /// 项目类型id
		/// </summary>
		public int project_type_id
		{
			set{ _project_type_id=value;}
			get{return _project_type_id;}
		}
		/// <summary>
        /// 投资年利率
		/// </summary>
		public decimal annual_interest_rate
		{
			set{ _annual_interest_rate=value;}
			get{return _annual_interest_rate;}
		}
		/// <summary>
        /// 借款金额
		/// </summary>
		public decimal borrowing_balance
		{
			set{ _borrowing_balance=value;}
			get{return _borrowing_balance;}
		}
		/// <summary>
        /// 借款期限
		/// </summary>
		public int life_of_loan
		{
			set{ _life_of_loan=value;}
			get{return _life_of_loan;}
		}
		/// <summary>
        /// 单位(月/天)
		/// 1 月  2 天
		/// </summary>
		public int unit_day
		{
			set{ _unit_day=value;}
			get{return _unit_day;}
		}
		/// <summary>
        /// 发布日期
		/// </summary>
		public DateTime release_date
		{
			set{ _release_date=value;}
			get{return _release_date;}
		}
		/// <summary>
        /// 统一起息日
		/// </summary>
		public DateTime value_date
		{
			set{ _value_date=value;}
			get{return _value_date;}
		}
		/// <summary>
		/// 每月付息日期 
		/// </summary>
		public int month_payment_date
		{
			set{ _month_payment_date=value;}
			get{return _month_payment_date;}
		}
		/// <summary>
        /// 还款日期
		/// </summary>
		public DateTime repayment_date
		{
			set{ _repayment_date=value;}
			get{return _repayment_date;}
		}
		/// <summary>
        /// 最低投资金额限制
		/// 0表示不限 
		/// </summary>
		public decimal minimum
		{
			set{ _minimum=value;}
			get{return _minimum;}
		}
		/// <summary>
        /// 最高投资金额限制
		/// 0 表示不限
		/// </summary>
		public decimal maxmum
		{
			set{ _maxmum=value;}
			get{return _maxmum;}
		}
		/// <summary>
        /// 担保公司id
		/// </summary>
		public int companyid
		{
			set{ _companyid=value;}
			get{return _companyid;}
		}
		/// <summary>
        /// 担保方式id
		/// </summary>
		public int guarantee_way_id
		{
			set{ _guarantee_way_id=value;}
			get{return _guarantee_way_id;}
		}
		/// <summary>
        /// 还款方式
		/// 1 按月等额本息 3 每月还息，到期还本  4 一次性还本付息
		/// </summary>
		public int payment_options
		{
			set{ _payment_options=value;}
			get{return _payment_options;}
		}
		/// <summary>
        /// 项目结束时间
		/// </summary>
		public DateTime end_time
		{
			set{ _end_time=value;}
			get{return _end_time;}
		}
		/// <summary>
        /// 成交服务费
		/// </summary>
		public decimal service_charge
		{
			set{ _service_charge=value;}
			get{return _service_charge;}
		}
		/// <summary>
        /// 借款者管理费
		/// </summary>
		public decimal loan_management_fee
		{
			set{ _loan_management_fee=value;}
			get{return _loan_management_fee;}
		}
		/// <summary>
        /// 投资者管理费
		/// </summary>
		public decimal investors_management_fee
		{
			set{ _investors_management_fee=value;}
			get{return _investors_management_fee;}
		}
		/// <summary>
        /// 普通逾期管理费
		/// </summary>
		public decimal ordinary_overdue_management_fees
		{
			set{ _ordinary_overdue_management_fees=value;}
			get{return _ordinary_overdue_management_fees;}
		}
		/// <summary>
        /// 严重逾期管理费
		/// </summary>
		public decimal seriously_overdue_management_fees
		{
			set{ _seriously_overdue_management_fees=value;}
			get{return _seriously_overdue_management_fees;}
		}
		/// <summary>
        /// 普通逾期罚息
		/// </summary>
		public decimal ordinary_overdue_penalty
		{
			set{ _ordinary_overdue_penalty=value;}
			get{return _ordinary_overdue_penalty;}
		}
		/// <summary>
        /// 严重逾期罚息
		/// </summary>
		public decimal seriously_overdue_penalty
		{
			set{ _seriously_overdue_penalty=value;}
			get{return _seriously_overdue_penalty;}
		}
		/// <summary>
        /// 债权转让管理费
		/// </summary>
		public decimal transfer_Expenses
		{
			set{ _transfer_expenses=value;}
			get{return _transfer_expenses;}
		}
		/// <summary>
        /// 筹款金额
		/// 根据和投资明细表触发器计算
		/// </summary>
		public decimal fundraising_amount
		{
			set{ _fundraising_amount=value;}
			get{return _fundraising_amount;}
		}
		/// <summary>
        /// 投标状态
		/// 0 审核中 1 审核通过 2 审核未通过 3 进行中 4 满标 5 已还清 
		/// </summary>
		public int tender_state
		{
			set{ _tender_state=value;}
			get{return _tender_state;}
		}
		/// <summary>
        /// 满标放款
		/// 0 未放款  1已放款
		/// </summary>
		public int full_scale_loan
		{
			set{ _full_scale_loan=value;}
			get{return _full_scale_loan;}
		}
		/// <summary>
        /// 流标返回
		/// 0 未满标 1 未返还
		/// </summary>
		public int flow_return
		{
			set{ _flow_return=value;}
			get{return _flow_return;}
		}
		/// <summary>
        /// 推荐
		/// </summary>
		public int recommend
		{
			set{ _recommend=value;}
			get{return _recommend;}
		}
		/// <summary>
        /// 系统时间
		/// </summary>
		public DateTime sys_time
		{
			set{ _sys_time=value;}
			get{return _sys_time;}
		}
		/// <summary>
        /// 还款期数
		/// 默认为 0   在生成原款计划时触发器自动生成还款期数总数
		/// </summary>
		public int repayment_periods
		{
			set{ _repayment_periods=value;}
			get{return _repayment_periods;}
		}


        /// <summary>
        /// 担保服务费率
        /// </summary>
        public decimal guarantee_fee 
        {
            set { _guarantee_fee = value; }
            get { return _guarantee_fee; }
        }


        /// <summary>
        /// 咨询服务费
        /// </summary>
        public decimal consultingAMT
        {
            set { _consultingAMT = value; }
            get { return _consultingAMT; }
        
        }


        /// <summary>
        /// 担保服务费
        /// </summary>
        public decimal guaranteeAMT
        {
            set { _guaranteeAMT = value; }
            get { return _guaranteeAMT; }
        }

        /// <summary>
        /// 借款年利率
        /// </summary>
        public decimal B_Rates
        {
            set { _B_Rates = value; }
            get { return _B_Rates; }
        }

        /// <summary>
        /// 宣传预上线开时间
        /// </summary>
        public DateTime start_time
        {
            set { _start_time = value; }
            get { return _start_time; }
        }


        /// <summary>
        /// 是否使用担保公司  0不使用  1使用
        /// </summary>
        public int IsUse {
            set { _IsUse = value; }
            get { return _IsUse; }
        
        }

		#endregion Model

	}
}

