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
	public partial class B_member_table
	{
        
		private readonly D_member_table dal=new D_member_table();
        public B_member_table()
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
		public bool Exists(int registerid)
		{
			return dal.Exists(registerid);
		}

         /// <summary>
        /// 验证登录 返回true登录成功,false登录失败
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>返回true登录成功,false登录失败</returns>
        public int CheckLogin(string username, string password)
        {

            return dal.CheckLogin(username, password);
        }
        
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(M_member_table model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(M_member_table model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int registerid)
		{
			
			return dal.Delete(registerid);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string registeridlist )
		{
			return dal.DeleteList(registeridlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public M_member_table GetModel(int registerid)
		{
			
			return dal.GetModel(registerid);
		}
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public M_member_table GetModelByUsrCustid(string usrCustid)
        {

            return dal.GetModelByUsrCustid(usrCustid);
        }

        public PartialMemberModel GetPartialModel(int registerid)
        {
            return dal.GetPartialModel(registerid);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
		public M_member_table GetModel(string username)
        {

            return dal.GetModel(username);
        }

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public M_member_table GetModelByCache(int registerid)
		{
			
			string CacheKey = "member_tableModel-" + registerid;
			object objModel =DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(registerid);
					if (objModel != null)
					{
						int ModelCache =ConfigHelper.GetConfigInt("ModelCache");
						DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (M_member_table)objModel;
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
		public List<M_member_table> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<M_member_table> DataTableToList(DataTable dt)
		{
			List<M_member_table> modelList = new List<M_member_table>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				M_member_table model;
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
        public M_member_table CheckUsrLogin(string username)
        {

            return dal.GetModel(username);
        }
		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

