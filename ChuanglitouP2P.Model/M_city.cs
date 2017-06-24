using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// 奖励帐户流水记录
	/// </summary>
	[Serializable]
	public partial class M_city
	{
		public M_city()
		{}
		#region Model
		private int _id;
		private int _cityid;
		private string _city;
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
		public int cityID
		{
			set{ _cityid=value;}
			get{return _cityid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string city
		{
			set{ _city=value;}
			get{return _city;}
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

