using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using System.Data;

namespace ChuanglitouP2P.BLL
{
    public class ActCount
    {


        #region 获取常规活动参与人数
        /// <summary>
        /// 获取常规活动参与人数
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int GetGeneralCount(int actid)
        {
            int i = 0;
            string sql = "select count(a.registerid) as num  from (select registerid from hx_UserAct    where   actid=" + actid.ToString() + "  group by registerid) a";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["num"].ToString(), out i);
            }
            return i;
        }
        #endregion



        #region 活动新增注册人数
        /// <summary>
        /// 活动新增注册人数
        /// </summary>
        /// <param name="actid"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public int GetGeneralNewReg(int actid, DateTime time)
        {
            int i = 0;
            string sql = "select count(a.registerid) as num  from (select registerid from V_ACT    where registration_time>= '" + time + "'  and actid=" + actid + "  group by registerid) a";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["num"].ToString(), out i);
            }
            return i;
        }
        #endregion



        #region 活动老用户
        /// <summary>
        /// 活动新增注册人数
        /// </summary>
        /// <param name="actid"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public int GetGeneralOldUser(int actid, DateTime time)
        {
            int i = 0;
            string sql = "select count(a.registerid) as num  from (select registerid from V_ACT    where registration_time<= '" + time + "'  and actid=" + actid + " group by registerid) a";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["num"].ToString(), out i);
            }
            return i;
        }
        #endregion



        #region 共投资人数
        /// <summary>
        /// 共投资人数
        /// </summary>
        /// <param name="actid"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public int GetGeneralTotalUser(int actid)
        {
            int i = 0;
            string sql = "select count(a.registerid) as num  from (select registerid from V_ACT    where  actid=" + actid.ToString() + " group by registerid) a ";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["num"].ToString(), out i);
            }
            return i;
        }
        #endregion



        #region 发放数量与总计
        /// <summary>
        /// 发放数量  num 数量  amt 金额
        /// </summary>
        /// <param name="actid"></param>       
        /// <returns></returns>
        public DataTable GetGeneralNum(int actid)
        {

            string sql = "select count(useract) as num ,sum(amt) as amt from  hx_UserAct where  actid= " + actid;
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            return dt;
        }
        #endregion


        #region 已用数量与总计
        /// <summary>
        /// 发放数量  num 数量  amt 金额
        /// </summary>
        /// <param name="actid"></param>       
        /// <returns></returns>
        public DataTable GetGeneralYNum(int actid)
        {

            string sql = "select count(useract) as num ,sum(amt) as amt from  hx_UserAct where  (UseState=1 or UseState=4) and actid= " + actid.ToString();
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            return dt;
        }
        #endregion


        #region 未用数量与总计
        /// <summary>
        /// 发放数量  num 数量  amt 金额
        /// </summary>
        /// <param name="actid"></param>       
        /// <returns></returns>
        public DataTable GetGeneralWNum(int actid)
        {
            string sql = "select count(useract) as num ,sum(amt) as amt from  hx_UserAct where  (UseState<>1 and UseState<>4) and actid=" + actid;
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            return dt;
        }
        #endregion


        #region 未用数量与总计
        /// <summary>
        /// 发放数量  num 数量  amt 金额
        /// </summary>
        /// <param name="actid"></param>       
        /// <returns></returns>
        public DataTable GetGeneralNumTotal(int actid)
        {
            string sql = "select count(useract) as num ,sum(amt) as amt from  hx_UserAct where   actid=" + actid;
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            return dt;
        }
        #endregion

        #region
        /// <summary>
        /// 查询指定日期之后有无新发现金，抵扣券，加息券
        /// </summary>
        /// <param name="RewTypeID"></param>
        /// <returns></returns>
        public static bool GetRewardTime(string RewTypeName, int RewTypeID,int userid)
        {
            bool isCount = false;
            if (userid != 0)
            {
                string CookCode = Utils.GetCookie(RewTypeName);
                if (string.IsNullOrWhiteSpace(CookCode))
                {
                    CookCode = "2016-12-6 00:00:00";
                }
                string sql = "select count(useract) as num from  hx_UserAct where registerid =" + userid + " and RewTypeID=" + RewTypeID + " and UseState=0 and AmtEndtime >='" + DateTime.Now + "' and Createtime > '" + Convert.ToDateTime(CookCode) + "'";
                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["num"]) > 0)
                    {
                        isCount = true;
                    }
                }
            }
            return isCount;
        }
        #endregion
    }
}
