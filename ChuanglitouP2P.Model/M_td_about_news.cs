
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// td_about_news:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_td_about_news
	{
		public M_td_about_news()
		{}
		#region Model
		private int _newsid;
		private int _web_type_menu_id;
		private string _news_title;
		private string _news_key;
		private string _news_des;
		private string _context;
		private DateTime _createtime;
		private int _adminuserid;
		/// <summary>
		/// 
		/// </summary>
		public int newid
		{
			set{ _newsid=value;}
			get{return _newsid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int web_Type_menu_id
		{
			set{ _web_type_menu_id=value;}
			get{return _web_type_menu_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string News_title
		{
			set{ _news_title=value;}
			get{return _news_title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string News_Key
		{
			set{ _news_key=value;}
			get{return _news_key;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string news_Des
		{
			set{ _news_des=value;}
			get{return _news_des;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string context
		{
			set{ _context=value;}
			get{return _context;}
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
		/// 
		/// </summary>
		public int adminuserid
		{
			set{ _adminuserid=value;}
			get{return _adminuserid;}
		}
		#endregion Model

	}
}

