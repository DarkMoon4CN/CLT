
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// td_Links:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_td_Links
	{
		public M_td_Links()
		{}
		#region Model
		private int _linkid;
		private string _linkname;
		private string _linkurl;
		private int _linktype;
		private string _linklogo;
		private DateTime _createtime;
		private int _linkstate;
		/// <summary>
		/// 
		/// </summary>
		public int Linkid
		{
			set{ _linkid=value;}
			get{return _linkid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Linkname
		{
			set{ _linkname=value;}
			get{return _linkname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LinkUrl
		{
			set{ _linkurl=value;}
			get{return _linkurl;}
		}
		/// <summary>
		/// 0 文字  1图片
		/// </summary>
		public int LinkType
		{
			set{ _linktype=value;}
			get{return _linktype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LinkLogo
		{
			set{ _linklogo=value;}
			get{return _linklogo;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime createtime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 0 有效 1无效
		/// </summary>
		public int Linkstate
		{
			set{ _linkstate=value;}
			get{return _linkstate;}
		}
		#endregion Model

	}
}

