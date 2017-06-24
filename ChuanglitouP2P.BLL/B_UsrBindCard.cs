using System;
using System.Data;
using System.Collections.Generic;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.DAL;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.BLL
{
	/// <summary>
	/// UsrBindCard
	/// </summary>
	public partial class B_UsrBindCard
	{
		private readonly D_UsrBindCard dal=new D_UsrBindCard();
        public B_UsrBindCard()
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
        public bool Exists(string UsrCustId)
		{
            return dal.Exists(UsrCustId);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(M_UsrBindCard model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(M_UsrBindCard model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int UsrBindCardID)
		{
			
			return dal.Delete(UsrBindCardID);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string UsrBindCardIDlist )
		{
			return dal.DeleteList(UsrBindCardIDlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        public M_UsrBindCard GetModel(string UsrCustId)
		{

            return dal.GetModel(UsrCustId);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
        public M_UsrBindCard GetModelByCache(string UsrCustId)
		{

            string CacheKey = "UsrBindCardModel-" + UsrCustId;
			object objModel = DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
                    objModel = dal.GetModel(UsrCustId);
					if (objModel != null)
					{
						int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
						DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (M_UsrBindCard)objModel;
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
		public List<M_UsrBindCard> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<M_UsrBindCard> DataTableToList(DataTable dt)
		{
			List<M_UsrBindCard> modelList = new List<M_UsrBindCard>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				M_UsrBindCard model;
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

