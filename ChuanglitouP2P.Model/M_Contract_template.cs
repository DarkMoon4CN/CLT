
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// Contract_template:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_Contract_template
	{
		public M_Contract_template()
		{}
		#region Model
		private int _contract_template_id;
		private int _contract_type_id;
		private string _contract_template_name;
		private string _contract_template_context;
		private int   _usestate;
		private DateTime   _cretatetime= DateTime.Now;
		/// <summary>
		/// 
		/// </summary>
		public int contract_template_id
		{
			set{ _contract_template_id=value;}
			get{return _contract_template_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int contract_type_id
		{
			set{ _contract_type_id=value;}
			get{return _contract_type_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string contract_template_name
		{
			set{ _contract_template_name=value;}
			get{return _contract_template_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string contract_template_context
		{
			set{ _contract_template_context=value;}
			get{return _contract_template_context;}
		}
		/// <summary>
		/// 0 默认 未使用   1当前使用中
		/// </summary>
		public int   usestate
		{
			set{ _usestate=value;}
			get{return _usestate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime   cretatetime
		{
			set{ _cretatetime=value;}
			get{return _cretatetime;}
		}
		#endregion Model

	}
}

