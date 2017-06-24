using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// 奖励帐户流水记录
	/// </summary>
	[Serializable]
	public partial class M_bonus_account_water
	{
		public M_bonus_account_water()
		{}
		#region Model
		private int _account_water_id;
		private int _bonus_account_id;
		private int _membertable_registerid;
		private decimal _income;
		private decimal _expenditure;
		private DateTime _time_of_occurrence;
		private decimal _reward_balance;
		private string _award_description;
		private int _water_type;
		/// <summary>
        /// 帐户流水id
		/// </summary>
		public int account_water_id
		{
			set{ _account_water_id=value;}
			get{return _account_water_id;}
		}
		/// <summary>
        /// 奖励帐户id
		/// </summary>
		public int bonus_account_id
		{
			set{ _bonus_account_id=value;}
			get{return _bonus_account_id;}
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
        /// 支出
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
        /// 奖励余额
		/// </summary>
		public decimal reward_balance
		{
			set{ _reward_balance=value;}
			get{return _reward_balance;}
		}
		/// <summary>
        /// 奖励描述
		/// </summary>
		public string award_description
		{
			set{ _award_description=value;}
			get{return _award_description;}
		}
		/// <summary>
        /// 流水类型
		/// 0 收入  1 支出
		/// </summary>
		public int water_type
		{
			set{ _water_type=value;}
			get{return _water_type;}
		}
		#endregion Model

	}
}

