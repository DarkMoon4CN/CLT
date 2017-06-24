using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
    /// 借款标的详细
	/// </summary>
	[Serializable]
	public partial class M_borrowing_target_detailed
	{
		public M_borrowing_target_detailed()
		{}
		#region Model
		private int _target_detailed_id;
		private int _borrower_registerid;
		private int _targetid;
		private string _item_details;
		private string _borrower_circumstances;
		private string _borrower_base_material;
		private string _use_funds;
		private string _independent_advice;
		private string _guarantee_agency_views;
		private string _risk_control_measures;
		private DateTime _createtime;
		/// <summary>
        /// 标的详细id
		/// </summary>
		public int target_detailed_id
		{
			set{ _target_detailed_id=value;}
			get{return _target_detailed_id;}
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
        /// 项目详情
		/// </summary>
		public string item_details
		{
			set{ _item_details=value;}
			get{return _item_details;}
		}
		/// <summary>
        /// 借款人情况
		/// </summary>
		public string borrower_circumstances
		{
			set{ _borrower_circumstances=value;}
			get{return _borrower_circumstances;}
		}
		/// <summary>
        /// 借款人基础材料
		/// </summary>
		public string borrower_base_material
		{
			set{ _borrower_base_material=value;}
			get{return _borrower_base_material;}
		}
		/// <summary>
        /// 资金用途
		/// </summary>
		public string use_funds
		{
			set{ _use_funds=value;}
			get{return _use_funds;}
		}
		/// <summary>
        /// 创利投独立意见
		/// </summary>
		public string independent_advice
		{
			set{ _independent_advice=value;}
			get{return _independent_advice;}
		}
		/// <summary>
		/// 担保机构意见
		/// </summary>
		public string guarantee_agency_views
		{
			set{ _guarantee_agency_views=value;}
			get{return _guarantee_agency_views;}
		}
		/// <summary>
		/// 风险控制措施
		/// </summary>
		public string risk_control_measures
		{
			set{ _risk_control_measures=value;}
			get{return _risk_control_measures;}
		}
		/// <summary>
        /// 创建日期
		/// </summary>
		public DateTime createtime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

