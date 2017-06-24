
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// td_Userinvitation:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_td_Userinvitation
	{
		public M_td_Userinvitation()
		{}
		#region Model
		private int _invitationid;
		private string _invcode;
		private DateTime _invtime;
		private int _invpersonid;
		private int _invpeopleid;
		private int _invitesstates;
		private decimal _invitereward;
        private int _UserAct;


        /// <summary>
        /// 邀请参与奖励活动ID
        /// </summary>
        public int UserAct
        {
            set { _UserAct = value; }
            get { return _UserAct; }
        }


		/// <summary>
		/// 
		/// </summary>
		public int invitationid
		{
			set{ _invitationid=value;}
			get{return _invitationid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string invcode
		{
			set{ _invcode=value;}
			get{return _invcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime invtime
		{
			set{ _invtime=value;}
			get{return _invtime;}
		}
        /// <summary>
        /// 被邀请人
        /// </summary>
        public int invpersonid
		{
			set{ _invpersonid=value;}
			get{return _invpersonid;}
		}
        /// <summary>
        /// 邀请人
        /// </summary>
        public int Invpeopleid
		{
			set{ _invpeopleid=value;}
			get{return _invpeopleid;}
		}
		/// <summary>
		/// 0 邀请码未使用   1已使用
		/// </summary>
		public int InvitesStates
		{
			set{ _invitesstates=value;}
			get{return _invitesstates;}
		}

        /// <summary>
        /// 邀请奖励
        /// </summary>
        public decimal Invitereward
		{
			set{ _invitereward=value;}
			get{return _invitereward;}
		}
		#endregion Model

	}
}

