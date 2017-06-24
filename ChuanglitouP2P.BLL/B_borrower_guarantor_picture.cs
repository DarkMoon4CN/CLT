using System;
using System.Data;
using System.Collections.Generic;
using ChuanglitouP2P.DAL;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.BLL
{
	/// <summary>
	/// 借款方图片类型 1
	/// </summary>
	public partial class B_borrower_guarantor_picture
	{
        
		private readonly D_borrower_guarantor_picture dal=new D_borrower_guarantor_picture();
        public B_borrower_guarantor_picture()
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
		public bool Exists(int borrower_guarantor_picture_id)
		{
			return dal.Exists(borrower_guarantor_picture_id);
		}

        
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(M_borrower_guarantor_picture model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(M_borrower_guarantor_picture model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int borrower_guarantor_picture_id)
		{
			
			return dal.Delete(borrower_guarantor_picture_id);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string borrower_guarantor_picture_idlist )
		{
			return dal.DeleteList(borrower_guarantor_picture_idlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public M_borrower_guarantor_picture GetModel(int borrower_guarantor_picture_id)
		{
			
			return dal.GetModel(borrower_guarantor_picture_id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public M_borrower_guarantor_picture GetModelByCache(int borrower_guarantor_picture_id)
		{
			
			string CacheKey = "borrower_guarantor_pictureModel-" + borrower_guarantor_picture_id;
			object objModel = DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(borrower_guarantor_picture_id);
					if (objModel != null)
					{
						int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
						DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (M_borrower_guarantor_picture)objModel;
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
		public List<M_borrower_guarantor_picture> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<M_borrower_guarantor_picture> DataTableToList(DataTable dt)
		{
			List<M_borrower_guarantor_picture> modelList = new List<M_borrower_guarantor_picture>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				M_borrower_guarantor_picture model;
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

