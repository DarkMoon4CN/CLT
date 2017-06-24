
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// td_Loan_records:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_td_Loan_records
	{
		public M_td_Loan_records()
		{}
		#region Model
		private int _loan_records_id;
		private int _targetid;
		private int _borrower_registerid;
		private int _investor_registerid;
		private decimal _loanamt;
		private string _loanordid;
		private DateTime _loandate;
		private decimal _free;
		private string _divdetails;
		private string _subordid;
		private string _unfreezeordid;
		private string _freezetrxid;
		/// <summary>
		/// 
		/// </summary>
		public int Loan_records_id
		{
			set{ _loan_records_id=value;}
			get{return _loan_records_id;}
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
		public int borrower_registerid
		{
			set{ _borrower_registerid=value;}
			get{return _borrower_registerid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int investor_registerid
		{
			set{ _investor_registerid=value;}
			get{return _investor_registerid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal LoanAMT
		{
			set{ _loanamt=value;}
			get{return _loanamt;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LoanOrdId
		{
			set{ _loanordid=value;}
			get{return _loanordid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime LoanDate
		{
			set{ _loandate=value;}
			get{return _loandate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal Free
		{
			set{ _free=value;}
			get{return _free;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DivDetails
		{
			set{ _divdetails=value;}
			get{return _divdetails;}
		}
		/// <summary>
		/// 本字段关链投标订单
		/// </summary>
		public string SubOrdid
		{
			set{ _subordid=value;}
			get{return _subordid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string unFreezeOrdId
		{
			set{ _unfreezeordid=value;}
			get{return _unfreezeordid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string FreezeTrxId
		{
			set{ _freezetrxid=value;}
			get{return _freezetrxid;}
		}
		#endregion Model

	}
}

