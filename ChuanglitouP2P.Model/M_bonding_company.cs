using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
    /// 担保公司
	/// 用户投标
	/// </summary>
	[Serializable]
	public partial class M_bonding_company
	{
		public M_bonding_company()
		{}
		#region Model
		private int _companyid;
		private string _company_name;
		private decimal _registered_capital;
		private DateTime _date_incorporation;
		private string _company_address;
		private string _company_profile;
		private string _business_licence;
		private string _business_certificate;
		private string _contract_covers;
		private string _contract_bottom;
		private string _legal_representative;
		private string _agent;
		private string _agent_name;
		private string _agent_id_card;
		private DateTime _createtime;
        private int _GuarType;


        private string _Tax_NO;

        private string _company_Url;

        private string _company_tel;
		/// <summary>
        /// 担保公司id
		/// </summary>
		public int companyid
		{
			set{ _companyid=value;}
			get{return _companyid;}
		}
		/// <summary>
        /// 担保公司名称
		/// </summary>
		public string company_name
		{
			set{ _company_name=value;}
			get{return _company_name;}
		}
		/// <summary>
        /// 注册资金
		/// </summary>
		public decimal registered_capital
		{
			set{ _registered_capital=value;}
			get{return _registered_capital;}
		}
		/// <summary>
        /// 公司成立日期
		/// </summary>
		public DateTime Date_incorporation
		{
			set{ _date_incorporation=value;}
			get{return _date_incorporation;}
		}
		/// <summary>
        /// 公司地址
		/// </summary>
		public string company_address
		{
			set{ _company_address=value;}
			get{return _company_address;}
		}
		/// <summary>
        /// 公司简介
		/// </summary>
		public string company_profile
		{
			set{ _company_profile=value;}
			get{return _company_profile;}
		}
		/// <summary>
        /// 营业执照
		/// </summary>
		public string business_licence
		{
			set{ _business_licence=value;}
			get{return _business_licence;}
		}
		/// <summary>
        /// 经营许可证
		/// </summary>
		public string business_certificate
		{
			set{ _business_certificate=value;}
			get{return _business_certificate;}
		}
		/// <summary>
        /// 合同封面
		/// </summary>
		public string contract_covers
		{
			set{ _contract_covers=value;}
			get{return _contract_covers;}
		}
		/// <summary>
        /// 合同盖章底面
		/// </summary>
		public string contract_bottom
		{
			set{ _contract_bottom=value;}
			get{return _contract_bottom;}
		}
		/// <summary>
        /// 法定代表人
		/// </summary>
		public string legal_representative
		{
			set{ _legal_representative=value;}
			get{return _legal_representative;}
		}
		/// <summary>
        /// 代理人
		/// </summary>
		public string agent
		{
			set{ _agent=value;}
			get{return _agent;}
		}
		/// <summary>
        /// 代理人用户名
		/// </summary>
		public string agent_name
		{
			set{ _agent_name=value;}
			get{return _agent_name;}
		}
		/// <summary>
        /// 代理人身份证号
		/// </summary>
		public string agent_id_card
		{
			set{ _agent_id_card=value;}
			get{return _agent_id_card;}
		}
		/// <summary>
        /// 录入时间
		/// </summary>
		public DateTime createtime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
        /// <summary>
        /// 税务登记号
        /// </summary>
        public string Tax_NO
        {
            set { _Tax_NO = value; }
            get { return _Tax_NO; }
        }


        /// <summary>
        /// 担保类型  0 N   1 Y
        /// </summary>
        public int GuarType
        {
            set { _GuarType = value; }
            get { return _GuarType; }
        }


        /// <summary>
        /// 公司网址
        /// </summary>
        public string company_Url
        {
            set { _company_Url = value; }
            get { return _company_Url; }
        }


        /// <summary>
        /// 公司电话
        /// </summary>
        public string company_tel
        {
            set { _company_tel = value; }
            get { return _company_tel; }
        }

		#endregion Model

	}
}

