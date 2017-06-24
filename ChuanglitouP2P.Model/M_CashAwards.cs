
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// CashAwards:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_CashAwards
	{
		public M_CashAwards()
		{}
		#region Model
		private int _cashawardsid;
		private int _membertable_registerid;
		private int _proid;
		private decimal _amounts=0.00M;
		private DateTime _awardstime= DateTime.Now;
		private int _targetid;
		private decimal _ordid;
		private int _ordidstate=0;
		private string _usrcustid;
		/// <summary>
		/// 
		/// </summary>
		public int CashAwardsid
		{
			set{ _cashawardsid=value;}
			get{return _cashawardsid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int membertable_registerid
		{
			set{ _membertable_registerid=value;}
			get{return _membertable_registerid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int proid
		{
			set{ _proid=value;}
			get{return _proid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal Amounts
		{
			set{ _amounts=value;}
			get{return _amounts;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime AwardsTime
		{
			set{ _awardstime=value;}
			get{return _awardstime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int targetid
		{
			set{ _targetid=value;}
			get{return _targetid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal OrdId
		{
			set{ _ordid=value;}
			get{return _ordid;}
		}
		/// <summary>
		/// 0  初始   1 转账中   3 转账成功
		/// </summary>
		public int OrdIdstate
		{
			set{ _ordidstate=value;}
			get{return _ordidstate;}
		}
		/// <summary>
		/// 由汇付生成的唯一标识
		/// </summary>
		public string UsrCustId
		{
			set{ _usrcustid=value;}
			get{return _usrcustid;}
		}
		#endregion Model

	}
}

