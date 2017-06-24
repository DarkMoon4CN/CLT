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
	public partial class B_borrowing_target
	{

        
		private readonly D_borrowing_target dal=new D_borrowing_target();
        public B_borrowing_target()
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
		public bool Exists(int targetid)
		{
			return dal.Exists(targetid);
		}

        
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(M_borrowing_target model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(M_borrowing_target model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int targetid)
		{
			
			return dal.Delete(targetid);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string targetidlist )
		{
			return dal.DeleteList(targetidlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public M_borrowing_target GetModel(int targetid)
		{
			
			return dal.GetModel(targetid);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public M_borrowing_target GetModelByCache(int targetid)
		{
			
			string CacheKey = "borrowing_targetModel-" + targetid;
			object objModel = DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(targetid);
					if (objModel != null)
					{
						int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
						DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (M_borrowing_target)objModel;
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
		public List<M_borrowing_target> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<M_borrowing_target> DataTableToList(DataTable dt)
		{
			List<M_borrowing_target> modelList = new List<M_borrowing_target>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				M_borrowing_target model;
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

        #region zhangyf

        ///合同编号
        public string GetLoanNumber()
        {
            return DBUtility.DbHelperSQL.Get_loan_number();
        }

        /// <summary>
        /// 根据条件得到一个对象实体
        /// </summary>
        public M_borrowing_target_ZhuoLu GetModelByLifeLoan(int lifeOfLoan)
        {
            return dal.GetModelByLifeLoan(lifeOfLoan);
        }
        /// <summary>
        /// 根据参数获取借款标部分信息
        /// </summary>
        /// <param name="tenderState"></param>
        /// <returns></returns>
        public M_borrowTargetZhuolu GetModelByParams(int tenderState)
        {
            return dal.GetModelByParams(tenderState);
        }

        #endregion
    }
}

