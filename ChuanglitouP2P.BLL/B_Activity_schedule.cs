using System;
using System.Data;
using System.Collections.Generic;
using ChuanglitouP2P.DAL;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.BLL
{
	/// <summary>
	/// 合同管理是对网站甲方
	/// </summary>
	public partial class B_Activity_schedule
	{
       

		private readonly D_Activity_schedule dal=new D_Activity_schedule();
        public B_Activity_schedule()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int activity_schedule_id)
		{
			return dal.Exists(activity_schedule_id);
		}
        
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(M_Activity_schedule model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(M_Activity_schedule model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int activity_schedule_id)
		{
			
			return dal.Delete(activity_schedule_id);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string activity_schedule_idlist )
		{
			return dal.DeleteList(activity_schedule_idlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public M_Activity_schedule GetModel(int activity_schedule_id)
		{
			
			return dal.GetModel(activity_schedule_id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public M_Activity_schedule GetModelByCache(int activity_schedule_id)
		{
			
			string CacheKey = "Activity_scheduleModel-" + activity_schedule_id;
			object objModel = DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(activity_schedule_id);
					if (objModel != null)
					{
						int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
						DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (M_Activity_schedule)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<M_Activity_schedule> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<M_Activity_schedule> DataTableToList(DataTable dt)
		{
			List<M_Activity_schedule> modelList = new List<M_Activity_schedule>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				M_Activity_schedule model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

