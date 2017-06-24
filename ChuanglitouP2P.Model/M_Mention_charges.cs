using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	///提现手续费
	/// </summary>
	[Serializable]
	public partial class M_Mention_charges
	{
		public M_Mention_charges()
		{}
		#region Model
		private int _mention_charges_id;
		private string _mention_charges_name;
		private decimal _minimum_amount;
		private decimal _maximum_amount;
		private decimal _fees;
		private int _fees_unit;
		/// <summary>
        /// 手续费id
		/// </summary>
		public int mention_charges_id
		{
			set{ _mention_charges_id=value;}
			get{return _mention_charges_id;}
		}
		/// <summary>
        /// 手续费名称
		/// </summary>
		public string mention_charges_name
		{
			set{ _mention_charges_name=value;}
			get{return _mention_charges_name;}
		}
		/// <summary>
        /// 最低金额
		/// </summary>
		public decimal minimum_amount
		{
			set{ _minimum_amount=value;}
			get{return _minimum_amount;}
		}
		/// <summary>
        /// 最高金额
		/// </summary>
		public decimal maximum_amount
		{
			set{ _maximum_amount=value;}
			get{return _maximum_amount;}
		}
		/// <summary>
        /// 手续费
		/// </summary>
		public decimal fees
		{
			set{ _fees=value;}
			get{return _fees;}
		}
		/// <summary>
        /// 单位
		/// 0 单笔( 元)  1 百分比(%)   收取
		/// </summary>
		public int fees_unit
		{
			set{ _fees_unit=value;}
			get{return _fees_unit;}
		}
		#endregion Model

	}
}

