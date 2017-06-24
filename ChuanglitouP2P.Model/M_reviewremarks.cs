
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// td_reviewremarks:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_reviewremarks
	{
		public M_reviewremarks()
		{}
		#region Model
		private int _reviewid;
		private int _targetid;
		private int _tender_state;
		private string _reviewremarks;
		private DateTime _reviewtime= DateTime.Now;
		private int _admin_operator;
		/// <summary>
		/// 
		/// </summary>
		public int reviewid
		{
			set{ _reviewid=value;}
			get{return _reviewid;}
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
		/// -1  录入 0 审核中 1 初审通过  2 复审通过 3 进行中(开款上线)  4 满标(还款中)   5 已还清   6初审未通过   7 复审未通过  8 流标
		/// </summary>
		public int tender_state
		{
			set{ _tender_state=value;}
			get{return _tender_state;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string reviewremarks
		{
			set{ _reviewremarks=value;}
			get{return _reviewremarks;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime reviewtime
		{
			set{ _reviewtime=value;}
			get{return _reviewtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int admin_operator
		{
			set{ _admin_operator=value;}
			get{return _admin_operator;}
		}
		#endregion Model

	}
}

