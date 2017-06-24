using System;
namespace ChuanglitouP2P.Model
{
	/// <summary>
    ///项目类型
	/// </summary>
	[Serializable]
	public partial class M_Project_type
	{
		public M_Project_type()
		{}
		#region Model
		private int _project_type_id;
		private string _project_type_name;
		/// <summary>
        /// 项目类型id
		/// </summary>
		public int project_type_id
		{
			set{ _project_type_id=value;}
			get{return _project_type_id;}
		}
		/// <summary>
        /// 项目类型名称
		/// </summary>
		public string project_type_name
		{
			set{ _project_type_name=value;}
			get{return _project_type_name;}
		}
		#endregion Model

	}
}

