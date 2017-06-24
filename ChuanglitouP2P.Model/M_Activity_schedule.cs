using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
    ///   活动计划奖励表
	/// </summary>
	[Serializable]
	public partial class M_Activity_schedule
	{
		public M_Activity_schedule()
		{}
		#region Model
		private int _activity_schedule_id;
		private string _activity_schedule_name;
		private decimal _amount_of_reward;
		private decimal _use_lower_limit;
		private int _reward;
		private DateTime _start_date;
		private DateTime _end_date;
		private DateTime _entry_time;
		/// <summary>
        /// 活动计划表id
		/// </summary>
		public int activity_schedule_id
		{
			set{ _activity_schedule_id=value;}
			get{return _activity_schedule_id;}
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

