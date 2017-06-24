using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Common;

namespace ChuanglitouP2P.DAL
{
    public class D_LuckDraw
    {
        /// <summary>
        /// 获取抽奖记录个数
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <param name="startTime">时间过滤，开始时间</param>
        /// <param name="endTime">时间过滤，结束时间</param>
        /// <returns></returns>
        public int GetRecordsCount(int userID, DateTime startTime, DateTime endTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT count(*) FROM [dbo].[LuckDrawRecord]");
            strSql.Append(" where Ldre_UserID=@Ldre_UserID and Ldre_CreatTime < @EndTime and Ldre_CreatTime >= @StartTime");
            SqlParameter[] parameters = {
                    new SqlParameter("@Ldre_UserID", SqlDbType.Int,4),
                    new SqlParameter("@EndTime", SqlDbType.DateTime),
                    new SqlParameter("@StartTime", SqlDbType.DateTime)
            };
            parameters[0].Value = userID;
            parameters[1].Value = endTime;
            parameters[2].Value = startTime;

            object res = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            return Convert.ToInt32(res);
        }
        /// <summary>
        /// 添加新数据
        /// </summary>
        /// <param name="record">新数据</param>
        /// <returns></returns>
        public bool AddNewRecord(M_LuckDrawRecord record)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"INSERT INTO [dbo].[LuckDrawRecord]
                                   ([Ldre_UserID]
                                   ,[Ldre_AwardID]
                                   ,[Ldre_AwardName]
                                   ,[Ldre_CreatTime]
                                   ,[Ldre_AwardType]
                                   ,[Ldre_OrderID]
                                   ,[Ldre_ActivityName])
                             VALUES
                                   (@Ldre_UserID
                                   ,@Ldre_AwardID
                                   ,@Ldre_AwardName
                                   ,@Ldre_CreatTime
                                   ,@Ldre_AwardType
                                   ,@Ldre_OrderID
                                   ,@Ldre_ActivityName)");
            SqlParameter[] parameters = {
                    new SqlParameter("@Ldre_UserID", SqlDbType.Int),
                    new SqlParameter("@Ldre_AwardID", SqlDbType.Int),
                    new SqlParameter("@Ldre_AwardName", SqlDbType.NVarChar,50),
                    new SqlParameter("@Ldre_CreatTime", SqlDbType.DateTime),
                    new SqlParameter("@Ldre_AwardType", SqlDbType.Int),
                    new SqlParameter("@Ldre_OrderID", SqlDbType.NVarChar,500),
                    new SqlParameter("@Ldre_ActivityName", SqlDbType.NVarChar,200)
            };
            parameters[0].Value = record.Ldre_UserID;
            parameters[1].Value = record.Ldre_AwardID;
            parameters[2].Value = record.Ldre_AwardName;
            parameters[3].Value = record.Ldre_CreatTime;
            parameters[4].Value = record.Ldre_AwardType;
            parameters[5].Value = record.Ldre_OrderID;
            parameters[6].Value = record.Ldre_ActivityName;

            int res = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            return res > 0;
        }
        /// <summary>
        /// 根据主键获取对象
        /// </summary>
        /// <param name="luckDrawID"></param>
        /// <returns></returns>
        public M_LuckDrawRecord GetModel(string luckDrawID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT top 1 * FROM [dbo].[LuckDrawRecord]");
            strSql.Append("Where Ldre_ID = @Ldre_ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@Ldre_ID", SqlDbType.Int)
            };
            parameters[0].Value = luckDrawID;
            return DataHelper.GetEntity<M_LuckDrawRecord>(DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0]);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public bool UpdateModel(M_LuckDrawRecord record)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"UPDATE [dbo].[LuckDrawRecord]
                       SET [Ldre_UserID] = @Ldre_UserID
                          ,[Ldre_AwardID] = @Ldre_AwardID
                          ,[Ldre_AwardName] = @Ldre_AwardName
                          ,[Ldre_CreatTime] = @Ldre_CreatTime
                          ,[Ldre_AwardType] = @Ldre_AwardType
                          ,[Ldre_OrderID] = @Ldre_OrderID
                     WHERE [Ldre_ID] = @Ldre_ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@Ldre_UserID", SqlDbType.Int),
                    new SqlParameter("@Ldre_AwardID", SqlDbType.Int),
                    new SqlParameter("@Ldre_AwardName", SqlDbType.NVarChar,50),
                    new SqlParameter("@Ldre_CreatTime", SqlDbType.DateTime),
                    new SqlParameter("@Ldre_AwardType", SqlDbType.Int),
                    new SqlParameter("@Ldre_ID", SqlDbType.Int),
                    new SqlParameter("@Ldre_OrderID",SqlDbType.NVarChar,500)
            };
            parameters[0].Value = record.Ldre_UserID;
            parameters[1].Value = record.Ldre_AwardID;
            parameters[2].Value = record.Ldre_AwardName;
            parameters[3].Value = record.Ldre_CreatTime;
            parameters[4].Value = record.Ldre_AwardType;
            parameters[5].Value = record.Ldre_ID;

            int res = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            return res > 0;
        }
        /// <summary>
        /// 获取分页查询数据
        /// </summary>
        /// <param name="AwardType">奖品类型</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">页容量</param>
        /// <param name="totalCount">总数据条数</param>
        /// <returns></returns>
        public List<M_LuckData> GetLuckDraw(int AwardType,string ActivityName, int PageIndex, int PageSize, out int totalCount)
        {
            string tblName = "V_LuckDrawData";
            string strGetFields = "ID, AwardName, AwardType, UserName, AwardTime,ActivityName";
            string strWhere = " 1=1 ";
            if (AwardType != -1)
            {
                strWhere += " and AwardType = " + AwardType;
            }
            if (AwardType == 0)
            {
                strWhere += " or AwardType = 4";
            }
            if (ActivityName != "" && ActivityName != null && ActivityName != "-1")
            {
                strWhere += " And ActivityName='"+ ActivityName + "'";
            }
            string fldName = "AwardTime";
            string Sort = "desc";
            D_PublicPageList dpage = new D_PublicPageList();
            List<M_LuckData> res = DataHelper.GetEntities<M_LuckData>(dpage.GetPageIndexListByPage(tblName, strGetFields, fldName, PageSize, PageIndex, strWhere, Sort, out totalCount)).ToList();
            return res;
        }

        /// <summary>
        /// 获取对某个奖项一段时间的获奖记录个数
        /// </summary>
        /// <param name="awardID">奖项编号</param>
        /// <param name="startTime">时间过滤，开始时间</param>
        /// <param name="endTime">时间过滤，结束时间</param>
        /// <returns></returns>
        public int GetCash50RecordsCount(int awardID, DateTime startTime, DateTime endTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT count(*) FROM [dbo].[LuckDrawRecord]");
            strSql.Append(" where Ldre_CreatTime < @EndTime and Ldre_CreatTime >= @StartTime and Ldre_AwardID=@Ldre_AwardID");
            SqlParameter[] parameters = {
                    new SqlParameter("@EndTime", SqlDbType.DateTime),
                    new SqlParameter("@StartTime", SqlDbType.DateTime),
                    new SqlParameter("@Ldre_AwardID", SqlDbType.Int,4)
            };
            parameters[0].Value = endTime;
            parameters[1].Value = startTime;
            parameters[2].Value = awardID;

            object res = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            return Convert.ToInt32(res);
        }
        /// <summary>
        /// 获取最近的20名中奖用户
        /// </summary>
        /// <returns></returns>
        public List<M_LuckMan> GetLastLuckMan(out int luckCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select top 20 d.UserName,c.Ldre_AwardName AwardName  from [LuckDrawRecord] c
                            left join [hx_member_table] d
                            on c.Ldre_UserID = d.registerid
                            where c.Ldre_AwardType != 3
                            order by c.Ldre_CreatTime desc");

            string sqlCount = @"select count(*) from [LuckDrawRecord]
                               where Ldre_AwardType != 3";

            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            List<M_LuckMan> res = DataHelper.GetEntities<M_LuckMan>(ds.Tables[0]).ToList();
            luckCount = Convert.ToInt32(DbHelperSQL.GetSingle(sqlCount));
            return res;
        }
        /// <summary>
        /// 根据活动名称获取最近的XX名中奖用户
        /// </summary>
        /// <param name="top"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<M_LuckMan> GetLuckDrawRecordList(int top, string where, out int luckCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT TOP (@TOP) D.USERNAME,D.MOBILE,C.LDRE_AWARDNAME AWARDNAME  FROM [LUCKDRAWRECORD] C
                            LEFT JOIN [HX_MEMBER_TABLE] D
                            ON C.LDRE_USERID = D.REGISTERID
                            WHERE C.LDRE_AWARDTYPE != 3 AND LDRE_ACTIVITYNAME=@WHERE
                            ORDER BY C.LDRE_CREATTIME DESC");
            SqlParameter[] parameters = {
                    new SqlParameter("@top",SqlDbType.Int,4),
                    new SqlParameter("@where", SqlDbType.NVarChar,200)
            };
            parameters[0].Value = top;
            parameters[1].Value = where;

            string sqlCount = @"SELECT COUNT(1) FROM [LUCKDRAWRECORD]
                               WHERE LDRE_AWARDTYPE != 3";
            if (where != "")
            {
                sqlCount += " AND LDRE_ACTIVITYNAME ='" + where + "'";
            }

            parameters[0].Value = top;
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            List<M_LuckMan> res = DataHelper.GetEntities<M_LuckMan>(ds.Tables[0]).ToList();
            luckCount = Convert.ToInt32(DbHelperSQL.GetSingle(sqlCount, parameters));
            return res;
        }

        /// <summary>
        /// 获取所有活动名称
        /// </summary>
        /// <returns></returns>
        public List<M_LuckActivityNameData> GetActivityNameList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT DISTINCT(LDRE_ACTIVITYNAME) ACTIVITYNAME  FROM [LUCKDRAWRECORD]
                               WHERE LDRE_ACTIVITYNAME IS NOT NULL");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            List<M_LuckActivityNameData> res = DataHelper.GetEntities<M_LuckActivityNameData>(ds.Tables[0]).ToList();
            return res;
        }
    }
}
