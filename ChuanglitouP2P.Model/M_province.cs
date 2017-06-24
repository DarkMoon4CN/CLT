using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// 与用户表，标的表 还款计划表相关联
	/// </summary>
	[Serializable]
	public partial class M_province
	{
		public M_province()
		{}
		#region Model
		private int _id;
		private int _provinceid;
		private string _province;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int provinceID
		{
			set{ _provinceid=value;}
			get{return _provinceid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string province
		{
			set{ _province=value;}
			get{return _province;}
		}
		#endregion Model

	}
}

