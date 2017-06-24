using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
    /// 担保方式
	/// </summary>
	[Serializable]
	public partial class M_guarantee_way
	{
		public M_guarantee_way()
		{}
		#region Model
		private int _guarantee_way_id;
		private string _guarantee_way_name;
		private decimal _amount_guaranteed;
		private DateTime _createtime;
		/// <summary>
        /// 担保方式id
		/// </summary>
		public int guarantee_way_id
		{
			set{ _guarantee_way_id=value;}
			get{return _guarantee_way_id;}
		}
		/// <summary>
        /// 担保方式名称
		/// </summary>
		public string guarantee_way_name
		{
			set{ _guarantee_way_name=value;}
			get{return _guarantee_way_name;}
		}
		/// <summary>
        /// 担保金额
		/// </summary>
		public decimal amount_guaranteed
		{
			set{ _amount_guaranteed=value;}
			get{return _amount_guaranteed;}
		}
		/// <summary>
        /// 录入时间
		/// </summary>
		public DateTime createtime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

