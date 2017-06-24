using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.DAL
{
    public class D_GrabIphone
    {
        public D_GrabIphone() { }

        /// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_GrabIphone model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into GrabIphone(");
            strSql.Append("RegrsterID,Color,Addtime,LuckDrawState,WinningState,WinningTime,TargetID,BidRecordsID,InvestmentAmount)");
            strSql.Append(" values (");
            strSql.Append("@RegrsterID,@Color,@Addtime,@LuckDrawState,@WinningState,@WinningTime,@TargetID,@BidRecordsID,@InvestmentAmount)");

            SqlParameter[] parameters = {
                    new SqlParameter("@RegrsterID",SqlDbType.Int,4),
                    new SqlParameter("@Color", SqlDbType.VarChar,50),
                    new SqlParameter("@Addtime", SqlDbType.DateTime),
                    new SqlParameter("@LuckDrawState",SqlDbType.Int,4),
                    new SqlParameter("@WinningState",SqlDbType.Int,4),
                    new SqlParameter("@WinningTime",SqlDbType.DateTime),
                    new SqlParameter("@TargetID",SqlDbType.Int,4),
                    new SqlParameter("@BidRecordsID",SqlDbType.Int,4),
                    new SqlParameter("@InvestmentAmount",SqlDbType.NVarChar,50)};
            parameters[0].Value = model.RegrsterID;
            parameters[1].Value = model.Color;
            parameters[2].Value = model.Addtime;
            parameters[3].Value = model.LuckDrawState;
            parameters[4].Value = model.WinningState;
            parameters[5].Value = model.WinningTime;
            parameters[6].Value = model.TargetID;
            parameters[7].Value = model.BidRecordsID;
            parameters[8].Value = model.InvestmentAmount;
            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(string Color, int WinningState, DateTime WinningTime, int RegrsterID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update GrabIphone set ");
            strSql.Append("Color=@Color,");
            strSql.Append("WinningState=@WinningState,");
            strSql.Append("WinningTime=@WinningTime");
            strSql.Append(" where RegrsterID=@RegrsterID");
            SqlParameter[] parameters = {
                    new SqlParameter("@Color", SqlDbType.VarChar,50),
                    new SqlParameter("@WinningState",SqlDbType.Int,4),
                    new SqlParameter("@WinningTime",SqlDbType.DateTime),
                    new SqlParameter("@RegrsterID",SqlDbType.Int,4)};
            parameters[0].Value = Color;
            parameters[1].Value = WinningState;
            parameters[2].Value = WinningTime;
            parameters[3].Value = RegrsterID;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量更新抽奖状态数据
        /// </summary>
        public bool UpdateLuckDrawState()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update GrabIphone set LuckDrawState=1 where LuckDrawState=0");

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int RegrsterID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from GrabIphone");
            strSql.Append(" where RegrsterID=@RegrsterID");
            SqlParameter[] parameters = {
                    new SqlParameter("@RegrsterID", SqlDbType.Int,4)
            };
            parameters[0].Value = RegrsterID;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) FROM GrabIphone ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperSQL.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" ID,RegrsterID,Color,Addtime,LuckDrawState,WinningState,WinningTime,TargetID,BidRecordsID,InvestmentAmount ");
            strSql.Append(" FROM GrabIphone ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 根据中奖状态获取数据
        /// </summary>
        /// <param name="AwardType">奖品类型</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">页容量</param>
        /// <param name="totalCount">总数据条数</param>
        /// <returns></returns>
        public List<M_GrabIphone> GetGrabIPhone(int WinningState, int PageIndex, int PageSize, out int totalCount)
        {
            string tblName = "GrabIphone";
            string strGetFields = "ID,RegrsterID,Color,Addtime,LuckDrawState,WinningState,WinningTime,TargetID,BidRecordsID,InvestmentAmount";
            string strWhere = "";
            if (WinningState != -1)
            {
                strWhere += "WinningState = " + WinningState;
            }
            string fldName = "ID";
            string Sort = "desc";
            D_PublicPageList dpage = new D_PublicPageList();
            List<M_GrabIphone> res = DataHelper.GetEntities<M_GrabIphone>(dpage.GetPageIndexListByPage(tblName, strGetFields, fldName, PageSize, PageIndex, strWhere, Sort, out totalCount)).ToList();
            return res;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public M_GrabIphone DataRowToModel(DataRow row)
        {
            M_GrabIphone model = new M_GrabIphone();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
                if (row["RegrsterID"] != null && row["RegrsterID"].ToString() != "")
                {
                    model.RegrsterID = int.Parse(row["RegrsterID"].ToString());
                }
                if (row["Color"] != null)
                {
                    model.Color = row["Color"].ToString();
                }
                if (row["Addtime"] != null && row["Addtime"].ToString() != "")
                {
                    model.Addtime = DateTime.Parse(row["Addtime"].ToString());
                }
                if (row["LuckDrawState"] != null && row["LuckDrawState"].ToString() != "")
                {
                    model.LuckDrawState = int.Parse(row["LuckDrawState"].ToString());
                }
                if (row["WinningState"] != null && row["WinningState"].ToString() != "")
                {
                    model.WinningState = int.Parse(row["WinningState"].ToString());
                }
                if (row["WinningTime"] != null && row["WinningTime"].ToString() != "")
                {
                    model.WinningTime = DateTime.Parse(row["WinningTime"].ToString());
                }

                if (row["TargetID"] != null && row["TargetID"].ToString() != "")
                {
                    model.TargetID = int.Parse(row["TargetID"].ToString());
                }
                if (row["BidRecordsID"] != null && row["BidRecordsID"].ToString() != "")
                {
                    model.BidRecordsID = int.Parse(row["BidRecordsID"].ToString());
                }
                if (row["InvestmentAmount"] != null)
                {
                    model.InvestmentAmount = row["InvestmentAmount"].ToString();
                }
            }
            return model;
        }
    }
}
