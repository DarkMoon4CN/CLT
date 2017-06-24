using ChuanglitouP2P.DAL;
using ChuanglitouP2P.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.BLL
{
    public class B_LuckDraw
    {
        D_LuckDraw dal = new D_LuckDraw();
        /// <summary>
        /// 获取个人一段时间内的抽奖记录
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <param name="startTime">时间过滤，开始时间</param>
        /// <param name="endTime">时间过滤，结束时间</param>
        /// <returns></returns>
        public int GetRecordsCount(int userID, DateTime startTime, DateTime endTime)
        {
            return dal.GetRecordsCount(userID, startTime, endTime);
        }
        /// <summary>
        /// 添加新数据
        /// </summary>
        /// <param name="record">新数据</param>
        /// <returns></returns>
        public bool AddNewRecord(M_LuckDrawRecord record)
        {
            return dal.AddNewRecord(record);
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
            return dal.GetCash50RecordsCount(awardID, startTime, endTime);
        }
        /// <summary>
        /// 获取最近的20名中奖用户
        /// </summary>
        /// <returns></returns>
        public List<M_LuckMan> GetLastLuckMan(out int luckCount)
        {
            return dal.GetLastLuckMan(out luckCount);
        }
        /// <summary>
        /// 根据奖品类型获取数据
        /// </summary>
        /// <param name="awardID"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<M_LuckData> GetLuckDraw(int awardType, string ActivityName, int pageIndex, int pageSize, out int totalCount)
        {
            return dal.GetLuckDraw(awardType, ActivityName,  pageIndex, pageSize, out totalCount);
        }
        /// <summary>
        /// 获取奖励记录对象
        /// </summary>
        /// <param name="luckDrawID"></param>
        public M_LuckDrawRecord GetModel(string luckDrawID)
        {
            return dal.GetModel(luckDrawID);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public bool UpdateModel(M_LuckDrawRecord record)
        {
            return dal.UpdateModel(record);
        }

        /// <summary>
        /// 根据活动名称获取最近的XX名中奖用户
        /// </summary>
        /// <param name="top"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<M_LuckMan> GetLuckDrawRecordList(int top, string where, out int luckCount)
        {
            return dal.GetLuckDrawRecordList(top, where, out luckCount);
        }

        /// <summary>
        /// 获取所有活动名称
        /// </summary>
        /// <returns></returns>
        public List<M_LuckActivityNameData> GetActivityNameList()
        {
            return dal.GetActivityNameList();
        }
    }
}
