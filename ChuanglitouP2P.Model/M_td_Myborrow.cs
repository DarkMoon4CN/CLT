
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// td_Myborrow:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_td_Myborrow
	{
		public M_td_Myborrow()
		{}
		#region Model
		private int _myborrowid;
		private string _username;
		private string _usertel;
		private string _borramt;
		private string _borrpurposes;
		private string _area;
		private string _compname;
		private string _industry;
		private string _regcapital;
		private int _timelimit=1;
		private DateTime _foundingtime;
		private int _mortgage=0;
		private DateTime _entrytime= DateTime.Now;
		private int _borrtype=0;
		private int _borrstate=0;
		/// <summary>
		/// 
		/// </summary>
		public int Myborrowid
		{
			set{ _myborrowid=value;}
			get{return _myborrowid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Username
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string userTel
		{
			set{ _usertel=value;}
			get{return _usertel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BorrAMT
		{
			set{ _borramt=value;}
			get{return _borramt;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BorrPurposes
		{
			set{ _borrpurposes=value;}
			get{return _borrpurposes;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Area
		{
			set{ _area=value;}
			get{return _area;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CompName
		{
			set{ _compname=value;}
			get{return _compname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Industry
		{
			set{ _industry=value;}
			get{return _industry;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RegCapital
		{
			set{ _regcapital=value;}
			get{return _regcapital;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int TimeLimit
		{
			set{ _timelimit=value;}
			get{return _timelimit;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime FoundingTime
		{
			set{ _foundingtime=value;}
			get{return _foundingtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Mortgage
		{
			set{ _mortgage=value;}
			get{return _mortgage;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime EntryTime
		{
			set{ _entrytime=value;}
			get{return _entrytime;}
		}
		/// <summary>
		/// 0 个人借款
        ///1 企业借款
		/// </summary>
		public int BorrType
		{
			set{ _borrtype=value;}
			get{return _borrtype;}
		}
		/// <summary>
		/// 0无效  1有效
		/// </summary>
		public int BorrState
		{
			set{ _borrstate=value;}
			get{return _borrstate;}
		}
		#endregion Model

	}
}

