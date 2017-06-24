
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// td_LoginInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_td_LoginInfo
	{
		public M_td_LoginInfo()
		{}
		#region Model
		private int _id;
		private string _adminusername;
		private string _pwd;
		private DateTime _logintime= DateTime.Now;
		private string _loginip;
		private int _loginsuccess=0;
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
		public string AdminUserName
		{
			set{ _adminusername=value;}
			get{return _adminusername;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Pwd
		{
			set{ _pwd=value;}
			get{return _pwd;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime LoginTime
		{
			set{ _logintime=value;}
			get{return _logintime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LoginIP
		{
			set{ _loginip=value;}
			get{return _loginip;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int LoginSuccess
		{
			set{ _loginsuccess=value;}
			get{return _loginsuccess;}
		}
		#endregion Model

	}
}

