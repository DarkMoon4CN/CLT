using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
    /// 奖励帐户
	/// bonus_account:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_bonus_account
	{
		public M_bonus_account()
		{}
		#region Model
		private int _bonus_account_id;
		private int _activity_schedule_id;
		private int _membertable_registerid;
		private string _activity_schedule_name;
		private decimal _amount_of_reward;
		private decimal _use_lower_limit;
		private int _reward;
		private DateTime _start_date;
		private DateTime _end_date;
		private int _reward_state;
		private string _reward_remarks;
		private DateTime _entry_time;
		/// <summary>
        /// 奖励帐户id
		/// </summary>
		public int bonus_account_id
		{
			set{ _bonus_account_id=value;}
			get{return _bonus_account_id;}
		}
		/// <summary>
        /// 活动计划表id
		/// </summary>
		public int activity_schedule_id
		{
			set{ _activity_schedule_id=value;}
			get{return _activity_schedule_id;}
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
        /// 活动计划名称
		/// </summary>
		public string activity_schedule_name
		{
			set{ _activity_schedule_name=value;}
			get{return _activity_schedule_name;}
		}
		/// <summary>
        /// 奖励金额
		/// </summary>
		public decimal amount_of_reward
		{
			set{ _amount_of_reward=value;}
			get{return _amount_of_reward;}
		}
		/// <summary>
        /// 使用下限
		/// </summary>
		public decimal use_lower_limit
		{
			set{ _use_lower_limit=value;}
			get{return _use_lower_limit;}
		}
		/// <summary>
        /// 奖励方式
		/// 0 常规(只奖励单方)  1 邀请奖励双方
		/// </summary>
		public int reward
		{
			set{ _reward=value;}
			get{return _reward;}
		}
		/// <summary>
        /// 有效开始日期
		/// </summary>
		public DateTime start_date
		{
			set{ _start_date=value;}
			get{return _start_date;}
		}
		/// <summary>
        /// 结束日期
		/// </summary>
		public DateTime end_date
		{
			set{ _end_date=value;}
			get{return _end_date;}
		}
		/// <summary>
        /// 奖励状态
		/// 0 未使用   1已使用  2 已过期
		/// </summary>
		public int reward_state
		{
			set{ _reward_state=value;}
			get{return _reward_state;}
		}
		/// <summary>
        /// 奖励备注
		/// </summary>
		public string reward_remarks
		{
			set{ _reward_remarks=value;}
			get{return _reward_remarks;}
		}
		/// <summary>
        /// 录入时间
		/// </summary>
		public DateTime entry_time
		{
			set{ _entry_time=value;}
			get{return _entry_time;}
		}
		#endregion Model

	}
}

