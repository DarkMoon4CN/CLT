
using System;
using System.Data;
using System.Collections.Generic;

using ChuanglitouP2P.Model;
using ChuanglitouP2P.DAL;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.BLL
{
	/// <summary>
	/// CashAwards
	/// </summary>
	public partial class B_CashAwards
	{
		private readonly D_CashAwards dal=new D_CashAwards();
        public B_CashAwards()
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
		public bool Exists(int CashAwardsid)
		{
			return dal.Exists(CashAwardsid);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(M_CashAwards model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(M_CashAwards model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int CashAwardsid)
		{
			
			return dal.Delete(CashAwardsid);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string CashAwardsidlist )
		{
			return dal.DeleteList(CashAwardsidlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public M_CashAwards GetModel(int CashAwardsid)
		{
			
			return dal.GetModel(CashAwardsid);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public M_CashAwards GetModelByCache(int CashAwardsid)
		{
			
			string CacheKey = "CashAwardsModel-" + CashAwardsid;
			object objModel = DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(CashAwardsid);
					if (objModel != null)
					{
						int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
						DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (M_CashAwards)objModel;
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
		public List<M_CashAwards> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<M_CashAwards> DataTableToList(DataTable dt)
		{
			List<M_CashAwards> modelList = new List<M_CashAwards>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				M_CashAwards model;
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

