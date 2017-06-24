
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// td_UserCash:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_td_UserCash
	{
		public M_td_UserCash()
		{}
		#region Model
		private int _usercashid;
		private int _registerid;
		private string _usrcustid;
		private decimal _transamt;
		private decimal _feeamt;
		private string _ordid;
		private DateTime _ordidtime;
		private int _ordidstate=0;
		private DateTime _opertime;
		private string _reason;
		private string _remarks;
		private int _transstate=-1;
        private string _FeeObjFlag;
        private string _cashChl;

        /// <summary>
        /// 
        /// </summary>
        public int UserCashId
		{
			set{ _usercashid=value;}
			get{return _usercashid;}
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
		/// 由汇付生成的唯一标识
		/// </summary>
		public string UsrCustId
		{
			set{ _usrcustid=value;}
			get{return _usrcustid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal TransAmt
		{
			set{ _transamt=value;}
			get{return _transamt;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal FeeAmt
		{
			set{ _feeamt=value;}
			get{return _feeamt;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OrdId
		{
			set{ _ordid=value;}
			get{return _ordid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime OrdIdTime
		{
			set{ _ordidtime=value;}
			get{return _ordidtime;}
		}
		/// <summary>
		/// 0 待审核 1待付款  3 已付款 4未通过(必须填写原因)
		/// </summary>
		public int OrdIdState
		{
			set{ _ordidstate=value;}
			get{return _ordidstate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime OperTime
		{
			set{ _opertime=value;}
			get{return _opertime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Reason
		{
			set{ _reason=value;}
			get{return _reason;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remarks
		{
			set{ _remarks=value;}
			get{return _remarks;}
		}
		/// <summary>
		/// 999 处理中      400 取现失败    000成功
		/// </summary>
		public int TransState
		{
			set{ _transstate=value;}
			get{return _transstate;}
		}

        /// <summary>
        /// 手续费收取对象 M 向商户收取  U向用户收取
        /// </summary>
        public string FeeObjFlag
        {
            set { _FeeObjFlag = value; }
            get { return _FeeObjFlag; }
        }
        /// <summary>
        /// 手续费收取对象 M 向商户收取  U向用户收取
        /// </summary>
        public string CashChl
        {
            set { _cashChl = value; }
            get { return _cashChl; }
        }
        #endregion Model

    }
}

