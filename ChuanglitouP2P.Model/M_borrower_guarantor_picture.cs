using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
    /// 借款方关图片及担保方图片
	/// </summary>
	[Serializable]
	public partial class M_borrower_guarantor_picture
	{
		public M_borrower_guarantor_picture()
		{}
		#region Model
		private int _borrower_guarantor_picture_id;
		private int _borrower_registerid;
		private int _targetid;
		private int _type_picture;
		private string _picture_path;
		private string _picture_name;
		private DateTime _uploadtime;
		/// <summary>
        /// 自增图片id
		/// </summary>
		public int borrower_guarantor_picture_id
		{
			set{ _borrower_guarantor_picture_id=value;}
			get{return _borrower_guarantor_picture_id;}
		}
		/// <summary>
        /// 借款人id
		/// </summary>
		public int borrower_registerid
		{
			set{ _borrower_registerid=value;}
			get{return _borrower_registerid;}
		}
		/// <summary>
        /// 标的id
		/// </summary>
		public int targetid
		{
			set{ _targetid=value;}
			get{return _targetid;}
		}
		/// <summary>
        /// 图片类型
		/// 借款方图片类型 1
       ///    胆保方图片类型 2
		/// </summary>
		public int type_picture
		{
			set{ _type_picture=value;}
			get{return _type_picture;}
		}
		/// <summary>
        /// 图片路径
		/// </summary>
		public string picture_path
		{
			set{ _picture_path=value;}
			get{return _picture_path;}
		}
		/// <summary>
        /// 图片名称
		/// </summary>
		public string picture_name
		{
			set{ _picture_name=value;}
			get{return _picture_name;}
		}
		/// <summary>
        /// 上传时间
		/// </summary>
		public DateTime uploadtime
		{
			set{ _uploadtime=value;}
			get{return _uploadtime;}
		}
		#endregion Model

	}
}

