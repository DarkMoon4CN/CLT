using ChuanglitouP2P.DAL;
using ChuanglitouP2P.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.BLL
{
    public class B_GrabIphone
    {
        private readonly D_GrabIphone dal = new D_GrabIphone();
        public B_GrabIphone() { }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int RegrsterID)
        {
            return dal.Exists(RegrsterID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(M_GrabIphone model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(string Color, int WinningState, DateTime WinningTime, int RegrsterID)
        {
            return dal.Update(Color, WinningState, WinningTime, RegrsterID);
        }
        /// <summary>
        /// 批量更新抽奖状态数据
        /// </summary>
        public bool UpdateLuckDrawState()
        {
            return dal.UpdateLuckDrawState();
        }
        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public List<M_GrabIphone> GetModelList(int Top, string strWhere, string filedOrder)
        {
            DataSet ds = dal.GetList(Top, strWhere, filedOrder);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 根据中奖状态获取数据
        /// </summary>
        /// <param name="awardID"></param>
        /// <param name="WinningState"></param>
        /// <returns></returns>
        public List<M_GrabIphone> GetGrabIPhone(int WinningState, int pageIndex, int pageSize, out int totalCount)
        {
            return dal.GetGrabIPhone(WinningState, pageIndex, pageSize, out totalCount);
        }
        /// <summary>
		/// 获得数据列表
		/// </summary>
		public List<M_GrabIphone> DataTableToList(DataTable dt)
        {
            List<M_GrabIphone> modelList = new List<M_GrabIphone>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                M_GrabIphone model;
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

    }
}
