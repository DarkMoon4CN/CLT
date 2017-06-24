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
	public partial class B_guarantee_way
	{
        
		private readonly D_guarantee_way dal=new D_guarantee_way();
        public B_guarantee_way()
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
		public bool Exists(int guarantee_way_id)
		{
			return dal.Exists(guarantee_way_id);
		}

        
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(M_guarantee_way model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(M_guarantee_way model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int guarantee_way_id)
		{
			
			return dal.Delete(guarantee_way_id);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string guarantee_way_idlist )
		{
			return dal.DeleteList(guarantee_way_idlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public M_guarantee_way GetModel(int guarantee_way_id)
		{
			
			return dal.GetModel(guarantee_way_id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public M_guarantee_way GetModelByCache(int guarantee_way_id)
		{
			
			string CacheKey = "guarantee_wayModel-" + guarantee_way_id;
			object objModel = DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(guarantee_way_id);
					if (objModel != null)
					{
						int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
						DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (M_guarantee_way)objModel;
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
		public List<M_guarantee_way> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<M_guarantee_way> DataTableToList(DataTable dt)
		{
			List<M_guarantee_way> modelList = new List<M_guarantee_way>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				M_guarantee_way model;
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

