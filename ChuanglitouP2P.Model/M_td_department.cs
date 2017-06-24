
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// td_department:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_td_department
	{
		public M_td_department()
		{}
		#region Model
		private int _department_id;
		private string _department_name;
		private int _parentid;
		private string _parentpath;
		private int _depath;
		private int _rootid;
		private int _child;
		private int _previd;
		private int _nextid;
		private int _orderid;
		private DateTime _createtime;
		/// <summary>
		/// 
		/// </summary>
		public int department_id
		{
			set{ _department_id=value;}
			get{return _department_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string department_name
		{
			set{ _department_name=value;}
			get{return _department_name;}
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
		#endregion Model

	}
}

