
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// td_Ad:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_td_Ad
	{
		public M_td_Ad()
		{}
		#region Model
		private int _adid;
		private string _adname;
		private DateTime _adcreatetime= DateTime.Now;
		private int _adstate=0;
        private int _AdTypeId;
		private string _adpath;
		private string _adlink;
		/// <summary>
		/// 
		/// </summary>
		public int Adid
		{
			set{ _adid=value;}
			get{return _adid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AdName
		{
			set{ _adname=value;}
			get{return _adname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime Adcreatetime
		{
			set{ _adcreatetime=value;}
			get{return _adcreatetime;}
		}
		/// <summary>
		/// 0 默认 (有效)  1下架(无效)
		/// </summary>
		public int AdState
		{
			set{ _adstate=value;}
			get{return _adstate;}
		}
		/// <summary>
		/// 
		/// </summary>
        public int AdTypeId
		{
            set { _AdTypeId = value; }
            get { return _AdTypeId; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string AdPath
		{
			set{ _adpath=value;}
			get{return _adpath;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string AdLink
		{
			set{ _adlink=value;}
			get{return _adlink;}
		}
		#endregion Model

	}
}

