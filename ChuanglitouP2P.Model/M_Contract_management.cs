using System;
namespace ChuanglitouP2P.Model
{
    /// <summary>
    /// 合同管理 [是见证人,保证人都是网站用户这里要确认]
    /// 合同管理是对网站甲方(出借人):   乙方(借款人)： 丙方(见证人)： 的合同管理 合同种类分为:
    ///   1 借款合同
    ///    角色身份: 甲方 (出借人):   乙方 (借款人)： 丙方(见证人)： 
    ///   
    ///   2 出借人咨询服务协议
    ///   角色身份: 甲方 (见证人)    乙方   (出借人):   
    ///   
    ///   3 保证合同
    ///   角色身份: 甲方 （保证人 担保公司）： 乙方   (出借人):   
    ///   
    /// </summary>
    [Serializable]
    public partial class M_Contract_management
    {
        public M_Contract_management()
        { }
        #region Model
        private int _contract;
        private decimal _loan_number;
        private int _targetid;
        private string _lender_name;
        private string _lender_username;
        private int _lender_registerid;
        private string _lender_id_card;
        private string _lenders_account_name;
        private string _lender_bank_account;
        private string _lender_bank;
        private string _lender_address;
        private string _lenders_telephone;
        private string _lenders_email;
        private DateTime _lendres_date_contract;
        private int _borrower_registerid;
        private string _borrower_name;
        private string _borrower_username;
        private string _borrower_id_card;
        private string _borrower_account_name;
        private string _borrower_bank_account;
        private string _borrower_date_contract;
        private string _borrower_bank;
        private string _witness_name;
        private string _witness_address;
        private string _witness_telephone;
        private string _witness_agent;
        private int _witness_admid;
        private string _witness_username;
        private string _witness_id_card;
        private DateTime _witness_date_contract;
        private string _surety_company_name;
        private string _guarantee_business_icense;
        private string _ensure_address;
        private string _guarantee_legal_representative;
        private string _guarantor_agent;
        private int _guarantor_companyid;
        private string _guarantor_agent_usernqme;
        private string _guarantor_agent_id_card;
        private DateTime _guarantor_agent_idate_contract;
        private string _contract_money;
        private string _counselling_service;
        private decimal _contract_amount;
        private string _suretyship_contract;
        private string _mode_payment;
        private DateTime _createtime;
        private int _contract_type;

        private string _contractpath;
        private int _bid_records_id;
        /// <summary>
        /// 
        /// </summary>
        public int contract
        {
            set { _contract = value; }
            get { return _contract; }
        }
        /// <summary>
        /// 用于合同编号
        /// 借款编号(合同编号)
        /// </summary>
        public decimal loan_number
        {
            set { _loan_number = value; }
            get { return _loan_number; }
        }
        /// <summary>
        /// 标的id
        /// </summary>
        public int targetid
        {
            set { _targetid = value; }
            get { return _targetid; }
        }
        /// <summary>
        /// 出借人姓名
        /// </summary>
        public string lender_name
        {
            set { _lender_name = value; }
            get { return _lender_name; }
        }
        /// <summary>
        /// 出借人用户名
        /// </summary>
        public string lender_username
        {
            set { _lender_username = value; }
            get { return _lender_username; }
        }
        /// <summary>
        /// 出借人用户id
        /// </summary>
        public int lender_registerid
        {
            set { _lender_registerid = value; }
            get { return _lender_registerid; }
        }
        /// <summary>
        /// 出借人身份证/营业执照号
        /// </summary>
        public string lender_id_card
        {
            set { _lender_id_card = value; }
            get { return _lender_id_card; }
        }
        /// <summary>
        /// 出借人开户名
        /// </summary>
        public string lenders_account_name
        {
            set { _lenders_account_name = value; }
            get { return _lenders_account_name; }
        }
        /// <summary>
        /// 出借人银行帐号
        /// </summary>
        public string lender_bank_account
        {
            set { _lender_bank_account = value; }
            get { return _lender_bank_account; }
        }
        /// <summary>
        /// 出借人开户行
        /// </summary>
        public string lender_bank
        {
            set { _lender_bank = value; }
            get { return _lender_bank; }
        }
        /// <summary>
        /// 出借人地址
        /// </summary>
        public string lender_address
        {
            set { _lender_address = value; }
            get { return _lender_address; }
        }
        /// <summary>
        /// 出借人电话
        /// </summary>
        public string lenders_telephone
        {
            set { _lenders_telephone = value; }
            get { return _lenders_telephone; }
        }
        /// <summary>
        /// 出借人邮箱
        /// </summary>
        public string lenders_email
        {
            set { _lenders_email = value; }
            get { return _lenders_email; }
        }
        /// <summary>
        /// 出借人合同日期
        /// </summary>
        public DateTime lendres_date_contract
        {
            set { _lendres_date_contract = value; }
            get { return _lendres_date_contract; }
        }
        /// <summary>
        /// 借款人id
        /// </summary>
        public int borrower_registerid
        {
            set { _borrower_registerid = value; }
            get { return _borrower_registerid; }
        }
        /// <summary>
        /// 借款人姓名
        /// </summary>
        public string borrower_name
        {
            set { _borrower_name = value; }
            get { return _borrower_name; }
        }
        /// <summary>
        /// 借款人用户名
        /// </summary>
        public string borrower_username
        {
            set { _borrower_username = value; }
            get { return _borrower_username; }
        }
        /// <summary>
        /// 借款人身份证号
        /// </summary>
        public string borrower_id_card
        {
            set { _borrower_id_card = value; }
            get { return _borrower_id_card; }
        }
        /// <summary>
        /// 借款人开户名
        /// </summary>
        public string borrower_account_name
        {
            set { _borrower_account_name = value; }
            get { return _borrower_account_name; }
        }
        /// <summary>
        /// 借款人银行帐号
        /// </summary>
        public string borrower_bank_account
        {
            set { _borrower_bank_account = value; }
            get { return _borrower_bank_account; }
        }
        /// <summary>
        /// 借款人合同日期
        /// </summary>
        public string borrower_date_contract
        {
            set { _borrower_date_contract = value; }
            get { return _borrower_date_contract; }
        }
        /// <summary>
        /// 借款人开户行
        /// </summary>
        public string borrower_bank
        {
            set { _borrower_bank = value; }
            get { return _borrower_bank; }
        }
        /// <summary>
        /// 见证人名称
        /// </summary>
        public string witness_name
        {
            set { _witness_name = value; }
            get { return _witness_name; }
        }
        /// <summary>
        /// 见证人地址
        /// </summary>
        public string witness_address
        {
            set { _witness_address = value; }
            get { return _witness_address; }
        }
        /// <summary>
        /// 见证人电话
        /// </summary>
        public string witness_telephone
        {
            set { _witness_telephone = value; }
            get { return _witness_telephone; }
        }
        /// <summary>
        /// 见证人代理人姓名
        /// </summary>
        public string witness_agent
        {
            set { _witness_agent = value; }
            get { return _witness_agent; }
        }
        /// <summary>
        /// 见证人id
        /// 关联担保平台管理员id
        /// </summary>
        public int witness_admid
        {
            set { _witness_admid = value; }
            get { return _witness_admid; }
        }
        /// <summary>
        /// 见证人用户名
        /// </summary>
        public string witness_username
        {
            set { _witness_username = value; }
            get { return _witness_username; }
        }
        /// <summary>
        /// 见证人身份证号
        /// </summary>
        public string witness_id_card
        {
            set { _witness_id_card = value; }
            get { return _witness_id_card; }
        }
        /// <summary>
        /// 见证人合同日期
        /// </summary>
        public DateTime witness_date_contract
        {
            set { _witness_date_contract = value; }
            get { return _witness_date_contract; }
        }
        /// <summary>
        /// 保证人公司名称
        /// </summary>
        public string surety_company_name
        {
            set { _surety_company_name = value; }
            get { return _surety_company_name; }
        }
        /// <summary>
        /// 保证人营业执照
        /// </summary>
        public string guarantee_business_icense
        {
            set { _guarantee_business_icense = value; }
            get { return _guarantee_business_icense; }
        }
        /// <summary>
        /// 保证人地址
        /// </summary>
        public string ensure_address
        {
            set { _ensure_address = value; }
            get { return _ensure_address; }
        }
        /// <summary>
        /// 保证人法定代表人
        /// </summary>
        public string guarantee_legal_representative
        {
            set { _guarantee_legal_representative = value; }
            get { return _guarantee_legal_representative; }
        }
        /// <summary>
        /// 保证人代理人
        /// </summary>
        public string guarantor_agent
        {
            set { _guarantor_agent = value; }
            get { return _guarantor_agent; }
        }
        /// <summary>
        /// 保证人代理人id
        /// 关联保险公司id
        /// </summary>
        public int guarantor_companyid
        {
            set { _guarantor_companyid = value; }
            get { return _guarantor_companyid; }
        }
        /// <summary>
        /// 保证人代理人用户名
        /// </summary>
        public string guarantor_agent_usernqme
        {
            set { _guarantor_agent_usernqme = value; }
            get { return _guarantor_agent_usernqme; }
        }
        /// <summary>
        /// 保证人代理人身份证号
        /// </summary>
        public string guarantor_agent_id_card
        {
            set { _guarantor_agent_id_card = value; }
            get { return _guarantor_agent_id_card; }
        }
        /// <summary>
        /// 保证人合同日期
        /// </summary>
        public DateTime guarantor_agent_idate_contract
        {
            set { _guarantor_agent_idate_contract = value; }
            get { return _guarantor_agent_idate_contract; }
        }
        /// <summary>
        /// 借款合同内容
        /// </summary>
        public string contract_money
        {
            set { _contract_money = value; }
            get { return _contract_money; }
        }
        /// <summary>
        /// 出借人咨询服务协议内容
        /// </summary>
        public string counselling_service
        {
            set { _counselling_service = value; }
            get { return _counselling_service; }
        }
        /// <summary>
        /// 合同金额
        /// </summary>
        public decimal contract_amount
        {
            set { _contract_amount = value; }
            get { return _contract_amount; }
        }
        /// <summary>
        /// 保证合同内容
        /// </summary>
        public string suretyship_contract
        {
            set { _suretyship_contract = value; }
            get { return _suretyship_contract; }
        }
        /// <summary>
        /// 付款方式
        /// </summary>
        public string mode_payment
        {
            set { _mode_payment = value; }
            get { return _mode_payment; }
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime createtime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }

        /// <summary>
        /// 合同类型  0网站合同模板 1用户合同模板
        /// </summary>
        public int contract_type
        {
            set { _contract_type = value; }
            get { return _contract_type; }
        }


        /// <summary>
        /// 合同路径
        /// </summary>
        public string contractpath
        {
            set { _contractpath = value; }
            get { return _contractpath; }
        }


        /// <summary>
        /// 投标记录id
        /// </summary>
        public int bid_records_id
        {
            set { _bid_records_id = value; }
            get { return _bid_records_id; }
        }

        /// <summary>
        /// 标的开始时间
        /// </summary>
        public string Start_Time { get; set; }
        /// <summary>
        /// 标的结束时间
        /// </summary>
        public string End_Time { get; set; }
        /// <summary>
        /// 标的持续时间
        /// </summary>
        public string DurationTime { get; set; }
        #endregion Model

    }
}

