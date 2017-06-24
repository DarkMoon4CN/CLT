using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
    /// 逾期还款
	/// 与用户表，标的表 还款计划表相关联
	/// </summary>
	[Serializable]
	public partial class M_Overdue_repayment
	{
		public M_Overdue_repayment()
		{}
		#region Model
		private int _overdue_repayment_id;
		private int _repayment_plan_id;
		private int _membertable_registerid;
		private int _targetid;
		private decimal _loan_management_fee;
		private decimal _ordinary_overdue_management_fees;
		private decimal _seriously_overdue_management_fees;
		private decimal _ordinary_overdue_penalty;
		private decimal _seriously_overdue_penalty;
		private DateTime _actual_repayment_time;
		private int _overdue_days;
		private int _interest_state;
		/// <summary>
        /// 逾期还款id
		/// </summary>
		public int overdue_repayment_id
		{
			set{ _overdue_repayment_id=value;}
			get{return _overdue_repayment_id;}
		}
		/// <summary>
        /// 返款计划id
		/// </summary>
		public int repayment_plan_id
		{
			set{ _repayment_plan_id=value;}
			get{return _repayment_plan_id;}
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
        /// 标的id
		/// </summary>
		public int targetid
		{
			set{ _targetid=value;}
			get{return _targetid;}
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
        /// 实际还款时间
		/// </summary>
		public DateTime actual_repayment_time
		{
			set{ _actual_repayment_time=value;}
			get{return _actual_repayment_time;}
		}
		/// <summary>
        /// 逾期天数
		/// </summary>
		public int overdue_days
		{
			set{ _overdue_days=value;}
			get{return _overdue_days;}
		}
		/// <summary>
        /// 罚息状态
		/// 0 初始值  1 罚息未还  2 罚息还清
        //产生逾期时状态应改为1  
		/// </summary>
		public int interest_state
		{
			set{ _interest_state=value;}
			get{return _interest_state;}
		}
		#endregion Model

	}
}

