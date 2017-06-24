
using System;
using System.Data;
using System.Collections.Generic;

using ChuanglitouP2P.Model;
using ChuanglitouP2P.DAL;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.BLL
{
    /// <summary>
    /// td_Userinvitation
    /// </summary>
    public partial class B_td_Userinvitation
    {
        private readonly D_td_Userinvitation dal = new D_td_Userinvitation();
        public B_td_Userinvitation()
        { }
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
        public bool Exists(int invitationid)
        {
            return dal.Exists(invitationid);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(M_td_Userinvitation model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(M_td_Userinvitation model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int invitationid)
        {

            return dal.Delete(invitationid);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string invitationidlist)
        {
            return dal.DeleteList(invitationidlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public M_td_Userinvitation GetModel(int invitationid)
        {

            return dal.GetModel(invitationid);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public M_td_Userinvitation GetModelByCache(int invitationid)
        {

            string CacheKey = "td_UserinvitationModel-" + invitationid;
            object objModel = DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(invitationid);
                    if (objModel != null)
                    {
                        int ModelCache = ConfigHelper.GetConfigInt("ModelCache");
                        DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (M_td_Userinvitation)objModel;
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
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<M_td_Userinvitation> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<M_td_Userinvitation> DataTableToList(DataTable dt)
        {
            List<M_td_Userinvitation> modelList = new List<M_td_Userinvitation>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                M_td_Userinvitation model = new M_td_Userinvitation();
                for (int n = 0; n < rowsCount; n++)
                {
                    //	model = dal.DataRowToModel(dt.Rows[n]);
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
            return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
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
        /// <summary>
        /// 获取邀请的好友，注册且投资的个数
        /// </summary>
        public int GetTotalCanUseCount(DateTime startTime, int registerID)
        {
            return dal.GetTotalCanUseCount(startTime, registerID);
        }
        #endregion  ExtensionMethod
    }
}

