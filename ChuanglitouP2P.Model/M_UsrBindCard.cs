
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// UsrBindCard:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_UsrBindCard
	{
		public M_UsrBindCard()
		{}
		#region Model
		private int _usrbindcardid;
		private int _registerid;
		private string _usrcustid;
		private string _openacctid;
		private string _openbankid;
		private int _defcard;
		/// <summary>
		/// 
		/// </summary>
		public int UsrBindCardID
		{
			set{ _usrbindcardid=value;}
			get{return _usrbindcardid;}
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
		public string UsrCustId
		{
			set{ _usrcustid=value;}
			get{return _usrcustid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OpenAcctId
		{
			set{ _openacctid=value;}
			get{return _openacctid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OpenBankId
		{
			set{ _openbankid=value;}
			get{return _openbankid;}
		}
		/// <summary>
		/// 0 默认   1设置为默认卡
		/// </summary>
		public int defCard
		{
			set{ _defcard=value;}
			get{return _defcard;}
		}
		#endregion Model

	}
}

