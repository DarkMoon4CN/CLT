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
	public partial class B_repayment_plan
	{
        
		private readonly D_repayment_plan dal=new D_repayment_plan();
        public B_repayment_plan()
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
		public bool Exists(int repayment_plan_id)
		{
			return dal.Exists(repayment_plan_id);
		}


         /// <summary>
        /// 检查数据是否被录入过 返回真说明记录不存在可以新增
        /// </summary>
        /// <param name="targetid"></param>
        /// <param name="current_period"></param>
        /// <param name="repayment_period"></param>
        /// <returns></returns>
        public bool Exists(int targetid, int current_period, DateTime repayment_period, string BorrUsrCustId)
        {

            return dal.Exists(targetid, current_period, repayment_period, BorrUsrCustId);
        }


        
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(M_repayment_plan model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(M_repayment_plan model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int repayment_plan_id)
		{
			
			return dal.Delete(repayment_plan_id);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string repayment_plan_idlist )
		{
			return dal.DeleteList(repayment_plan_idlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public M_repayment_plan GetModel(int repayment_plan_id)
		{
			
			return dal.GetModel(repayment_plan_id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public M_repayment_plan GetModelByCache(int repayment_plan_id)
		{
			
			string CacheKey = "repayment_planModel-" + repayment_plan_id;
			object objModel = DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(repayment_plan_id);
					if (objModel != null)
					{
						int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
						DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (M_repayment_plan)objModel;
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
		public List<M_repayment_plan> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<M_repayment_plan> DataTableToList(DataTable dt)
		{
			List<M_repayment_plan> modelList = new List<M_repayment_plan>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				M_repayment_plan model;
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

