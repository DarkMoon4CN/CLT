using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
    ///充值记录
	/// </summary>
	[Serializable]
	public partial class M_Recharge_history
	{
		public M_Recharge_history()
		{}
		#region Model
		private int _recharge_history_id;
		private int _membertable_registerid;
		private decimal _recharge_amount;
		private DateTime _recharge_time;
		private decimal _account_amount;
		private string _order_no;
		private int _recharge_condition;
		private string _recharge_bank;
		/// <summary>
        /// 充值id
		/// </summary>
		public int recharge_history_id
		{
			set{ _recharge_history_id=value;}
			get{return _recharge_history_id;}
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
        /// 充值金额
		/// </summary>
		public decimal recharge_amount
		{
			set{ _recharge_amount=value;}
			get{return _recharge_amount;}
		}
		/// <summary>
        /// 充值时间
		/// </summary>
		public DateTime recharge_time
		{
			set{ _recharge_time=value;}
			get{return _recharge_time;}
		}
		/// <summary>
        /// 到帐金额
		/// </summary>
		public decimal account_amount
		{
			set{ _account_amount=value;}
			get{return _account_amount;}
		}
		/// <summary>
        /// 对账订单号
		/// </summary>
		public string order_No
		{
			set{ _order_no=value;}
			get{return _order_no;}
		}
		/// <summary>
        /// 充值状态
		/// 0未支付成功 1 支付成功
		/// </summary>
		public int recharge_condition
		{
			set{ _recharge_condition=value;}
			get{return _recharge_condition;}
		}
		/// <summary>
        /// 充值银行
		/// </summary>
		public string recharge_bank
		{
			set{ _recharge_bank=value;}
			get{return _recharge_bank;}
		}
		#endregion Model

	}
}

