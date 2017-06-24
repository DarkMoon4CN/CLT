
using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
	/// contract_type:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_contract_type
	{
		public M_contract_type()
		{}
		#region Model
		private int _contract_type_id;
		private string _contract_type_name;
		private DateTime _createtime= DateTime.Now;
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
		public string contract_type_name
		{
			set{ _contract_type_name=value;}
			get{return _contract_type_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime createtime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

