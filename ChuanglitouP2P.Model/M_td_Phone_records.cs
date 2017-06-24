
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// td_Phone_records:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_td_Phone_records
	{
		public M_td_Phone_records()
		{}
		#region Model
		private int _recordsid;
		private string _recordcontext;
		private DateTime _recordtime= DateTime.Now;
		private int _registerid=0;
		private int _adminid=0;
		/// <summary>
		/// 
		/// </summary>
		public int recordsid
		{
			set{ _recordsid=value;}
			get{return _recordsid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string recordcontext
		{
			set{ _recordcontext=value;}
			get{return _recordcontext;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime recordtime
		{
			set{ _recordtime=value;}
			get{return _recordtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int registerid
		{
			set{ _registerid=value;}
			get{return _registerid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int adminid
		{
			set{ _adminid=value;}
			get{return _adminid;}
		}
		#endregion Model

	}
}

