
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// td_menu:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_td_menu
	{
		public M_td_menu()
		{}
		#region Model
		private int _menu_id;
		private string _menu_name;
		private int _parentid;
		private string _parentpath;
		private int _depath;
		private int _rootid;
		private int _child;
		private int _previd;
		private int _nextid;
		private int _orderid;
		private DateTime _createtime;
		private string _path1;
		private string _path2;
		private string _path3;
		private string _path4;
		/// <summary>
		/// 
		/// </summary>
		public int menu_id
		{
			set{ _menu_id=value;}
			get{return _menu_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string menu_name
		{
			set{ _menu_name=value;}
			get{return _menu_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int parentid
		{
			set{ _parentid=value;}
			get{return _parentid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string parentpath
		{
			set{ _parentpath=value;}
			get{return _parentpath;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int depath
		{
			set{ _depath=value;}
			get{return _depath;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int rootid
		{
			set{ _rootid=value;}
			get{return _rootid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int child
		{
			set{ _child=value;}
			get{return _child;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int previd
		{
			set{ _previd=value;}
			get{return _previd;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int nextid
		{
			set{ _nextid=value;}
			get{return _nextid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int orderid
		{
			set{ _orderid=value;}
			get{return _orderid;}
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
		public string path1
		{
			set{ _path1=value;}
			get{return _path1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string path2
		{
			set{ _path2=value;}
			get{return _path2;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string path3
		{
			set{ _path3=value;}
			get{return _path3;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string path4
		{
			set{ _path4=value;}
			get{return _path4;}
		}
		#endregion Model

	}
}

