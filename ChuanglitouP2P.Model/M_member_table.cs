using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
    /// 用户表
	/// </summary>
	[Serializable]
	public partial class M_member_table
	{
		public M_member_table()
		{}
		#region Model
		private int _registerid;
		private string _username;
		private string _password;
		private string _mobile;
		private string _email;
		private string _realname;
		private string _id_number;
		private string _transactionpassword;
		private int _istransactionpassword;
		private int _ismobile;
		private int _isrealname;
		private int _isbankcard;
		private int _isemail;
		private int _userstate;
		private decimal _account_total_assets;
		private decimal _available_balance;
		private decimal _collect_total_amount;
		private decimal _frozen_sum;
		private int _open_tonto_account;
		private string _tonto_account_user;
        private int _usertypes;
        private string _invitedcode;

        private string _UsrCustId;

        private string _UsrId;

        private int _useridentity;

        private int _Channelsource;

        private string _Tid;
        private DateTime _lastlogintime;
        private string _lastloginIP;
        private DateTime _registration_time;

        private int _LostInvitation;







        /// <summary>
        /// 注册id
        /// </summary>
        public int registerid
		{
			set{ _registerid=value;}
			get{return _registerid;}
		}
		/// <summary>
        /// 用户名
		/// </summary>
		public string username
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
        /// 密码
		/// </summary>
		public string password
		{
			set{ _password=value;}
			get{return _password;}
		}
		/// <summary>
        /// 手机号
		/// </summary>
		public string mobile
		{
			set{ _mobile=value;}
			get{return _mobile;}
		}
		/// <summary>
        /// 邮箱
		/// </summary>
		public string email
		{
			set{ _email=value;}
			get{return _email;}
		}
		/// <summary>
        /// 真实姓名
		/// </summary>
		public string realname
		{
			set{ _realname=value;}
			get{return _realname;}
		}
		/// <summary>
        /// 身份证号
		/// </summary>
		public string iD_number
		{
			set{ _id_number=value;}
			get{return _id_number;}
		}
		/// <summary>
        /// 交易密码
		/// </summary>
		public string transactionpassword
		{
			set{ _transactionpassword=value;}
			get{return _transactionpassword;}
		}
		/// <summary>
        /// 是否设置效易密码
		/// </summary>
		public int istransactionpassword
		{
			set{ _istransactionpassword=value;}
			get{return _istransactionpassword;}
		}
		/// <summary>
        /// 是否通过手机认证
		/// </summary>
		public int ismobile
		{
			set{ _ismobile=value;}
			get{return _ismobile;}
		}
		/// <summary>
        /// 是否实名认证
		/// </summary>
		public int isrealname
		{
			set{ _isrealname=value;}
			get{return _isrealname;}
		}
		/// <summary>
        /// 是否银行卡认证
		/// </summary>
		public int isbankcard
		{
			set{ _isbankcard=value;}
			get{return _isbankcard;}
		}
		/// <summary>
        /// 是否邮箱认证
		/// </summary>
		public int isemail
		{
			set{ _isemail=value;}
			get{return _isemail;}
		}
		/// <summary>
        /// 用户状态
		/// 0 正常  1禁止登录
		/// </summary>
		public int userstate
		{
			set{ _userstate=value;}
			get{return _userstate;}
		}
		/// <summary>
        /// 帐户总资产
		/// </summary>
		public decimal account_total_assets
		{
			set{ _account_total_assets=value;}
			get{return _account_total_assets;}
		}
		/// <summary>
        /// 可用余额
		/// </summary>
		public decimal available_balance
		{
			set{ _available_balance=value;}
			get{return _available_balance;}
		}
		/// <summary>
        /// 待收总额
		/// </summary>
		public decimal collect_total_amount
		{
			set{ _collect_total_amount=value;}
			get{return _collect_total_amount;}
		}
		/// <summary>
        /// 冻结金额
		/// </summary>
		public decimal frozen_sum
		{
			set{ _frozen_sum=value;}
			get{return _frozen_sum;}
		}
		/// <summary>
        /// 是否开通托管帐户
		/// 0 默认未开通   1 已开通
		/// </summary>
		public int open_tonto_account
		{
			set{ _open_tonto_account=value;}
			get{return _open_tonto_account;}
		}
		/// <summary>
        /// 托管帐户id
		/// </summary>
		public string tonto_account_user
		{
			set{ _tonto_account_user=value;}
			get{return _tonto_account_user;}
		}

        /// <summary>
        /// 用户类型 0 投资者 1借款者
        /// </summary>
        public int usertypes
        {
            set { _usertypes = value; }
            get { return _usertypes; }
        
        }

        /// <summary>
        /// 邀请码
        /// </summary>
        public string invitedcode
        {
            set { _invitedcode = value; }
            get { return _invitedcode; }
        }
		#endregion Model

        /// <summary>
        /// 用户客户号
        /// </summary>
        public string UsrCustId
        {
            set { _UsrCustId = value; }
            get { return _UsrCustId; }
        }

        /// <summary>
        /// 用户号
        /// </summary>
        public string UsrId
        {
            set { _UsrId = value; }
            get { return _UsrId; }
        
        }

        /// <summary>
        /// 0 普通会员  1 vip会员 2 黄金 3 虚假会员 4 渠道合作 5 白金  6钻石
        /// </summary>
        public int useridentity
        {
            set { _useridentity = value; }
            get { return _useridentity; }
        }



        public int Channelsource
        {
            set { _Channelsource = value; }
            get { return _Channelsource; }
        }


        public string Tid
        {
            set { _Tid = value; }
            get { return _Tid; }
        
        }

        public string CreatedOn { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime lastlogintime
        {
            set { _lastlogintime = value; }
            get { return _lastlogintime; }

        }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        public string lastloginIP
        {
            set { _lastloginIP = value; }
            get { return _lastloginIP; }

        }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime Registration_time
        {
            get
            {
                return _registration_time;
            }

            set
            {
                _registration_time = value;
            }
        }
		public int LostInvitation
        {
            set { _LostInvitation = value; }
            get { return _LostInvitation; }
        }

        public string channel_invitedcode { get; set; }
    }
}

