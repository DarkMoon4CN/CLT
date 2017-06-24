
using System;
using System.Data;
using System.Collections.Generic;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.DAL;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.BLL
{
	/// <summary>
	/// td_Myborrow
	/// </summary>
	public partial class B_td_Myborrow
	{
		private readonly D_td_Myborrow dal=new D_td_Myborrow();
        public B_td_Myborrow()
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
		public bool Exists(int Myborrowid)
		{
			return dal.Exists(Myborrowid);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(M_td_Myborrow model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(M_td_Myborrow model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Myborrowid)
		{
			
			return dal.Delete(Myborrowid);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string Myborrowidlist )
		{
			return dal.DeleteList(Myborrowidlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public M_td_Myborrow GetModel(int Myborrowid)
		{
			
			return dal.GetModel(Myborrowid);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public M_td_Myborrow GetModelByCache(int Myborrowid)
		{
			
			string CacheKey = "td_MyborrowModel-" + Myborrowid;
			object objModel = DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Myborrowid);
					if (objModel != null)
					{
						int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
						DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (M_td_Myborrow)objModel;
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
		public List<M_td_Myborrow> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<M_td_Myborrow> DataTableToList(DataTable dt)
		{
			List<M_td_Myborrow> modelList = new List<M_td_Myborrow>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				M_td_Myborrow model;
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

