using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// area:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_area
	{
		public M_area()
		{}
		#region Model
		private int _id;
		private int _areaid;
		private string _area;
		private int _fatherid;
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
		public int areaID
		{
			set{ _areaid=value;}
			get{return _areaid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string area
		{
			set{ _area=value;}
			get{return _area;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int fatherID
		{
			set{ _fatherid=value;}
			get{return _fatherid;}
		}
		#endregion Model

	}
}

