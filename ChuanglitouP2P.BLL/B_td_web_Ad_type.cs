
using System;
using System.Data;
using System.Collections.Generic;

using ChuanglitouP2P.Model;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DAL;
namespace ChuanglitouP2P.BLL
{
	/// <summary>
	/// td_web_Ad_type
	/// </summary>
	public partial class B_td_web_Ad_type
	{
		private readonly D_td_web_Ad_type dal=new D_td_web_Ad_type();
		public B_td_web_Ad_type()
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
		public bool Exists(int AdTypeId)
		{
			return dal.Exists(AdTypeId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(M_td_web_Ad_type model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(M_td_web_Ad_type model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int AdTypeId)
		{
			
			return dal.Delete(AdTypeId);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string AdTypeIdlist )
		{
			return dal.DeleteList(AdTypeIdlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public M_td_web_Ad_type GetModel(int AdTypeId)
		{
			
			return dal.GetModel(AdTypeId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public M_td_web_Ad_type GetModelByCache(int AdTypeId)
		{
			
			string CacheKey = "td_web_Ad_typeModel-" + AdTypeId;
			object objModel = DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(AdTypeId);
					if (objModel != null)
					{
						int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
						DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (M_td_web_Ad_type)objModel;
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
		public List<M_td_web_Ad_type> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<M_td_web_Ad_type> DataTableToList(DataTable dt)
		{
			List<M_td_web_Ad_type> modelList = new List<M_td_web_Ad_type>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				M_td_web_Ad_type model;
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

