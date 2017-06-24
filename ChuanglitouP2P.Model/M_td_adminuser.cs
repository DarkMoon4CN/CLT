
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// td_adminuser:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_td_adminuser
	{
		public M_td_adminuser()
		{}
		#region Model
		private int _adminuserid;
		private string _adminuser;
		private string _userpass;
		private int _state=0;
		private DateTime _datetime= DateTime.Now;
		private string _truename;
		private string _email;
		private string _province;
		private string _city;
		private string _tel;
		private string _phone_number;
		private DateTime _lastlogintime= DateTime.Now;
		private string _lastloginip;
		private int _logintimes=0;
		private string _worknum;
		private string _sex;
		private int _department_id=0;
		private int _area_id=0;
		/// <summary>
		/// 
		/// </summary>
		public int adminuserid
		{
			set{ _adminuserid=value;}
			get{return _adminuserid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string adminuser
		{
			set{ _adminuser=value;}
			get{return _adminuser;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string userpass
		{
			set{ _userpass=value;}
			get{return _userpass;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int state
		{
			set{ _state=value;}
			get{return _state;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime datetime
		{
			set{ _datetime=value;}
			get{return _datetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string trueName
		{
			set{ _truename=value;}
			get{return _truename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string email
		{
			set{ _email=value;}
			get{return _email;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string province
		{
			set{ _province=value;}
			get{return _province;}
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
		public string tel
		{
			set{ _tel=value;}
			get{return _tel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string phone_number
		{
			set{ _phone_number=value;}
			get{return _phone_number;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime lastLoginTime
		{
			set{ _lastlogintime=value;}
			get{return _lastlogintime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string lastLoginIP
		{
			set{ _lastloginip=value;}
			get{return _lastloginip;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int loginTimes
		{
			set{ _logintimes=value;}
			get{return _logintimes;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string worknum
		{
			set{ _worknum=value;}
			get{return _worknum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sex
		{
			set{ _sex=value;}
			get{return _sex;}
		}
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
		public int area_id
		{
			set{ _area_id=value;}
			get{return _area_id;}
		}
		#endregion Model

	}
}

