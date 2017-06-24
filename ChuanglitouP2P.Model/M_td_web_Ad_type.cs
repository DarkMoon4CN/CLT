
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// td_web_Ad_type:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_td_web_Ad_type
	{
		public M_td_web_Ad_type()
		{}
		#region Model
		private int _adtypeid;
		private string _adtypename;
		private DateTime _createtime= DateTime.Now;
		/// <summary>
		/// 
		/// </summary>
		public int AdTypeId
		{
			set{ _adtypeid=value;}
			get{return _adtypeid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AdtypeName
		{
			set{ _adtypename=value;}
			get{return _adtypename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime createtime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

