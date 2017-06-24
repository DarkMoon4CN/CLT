using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ChuanglitouP2P.DAL;

namespace ChuanglitouP2P.BLL
{
    public class B_PublicPageList
    {

        D_PublicPageList o = new D_PublicPageList();

        /// <summary>
        /// 根据主键获取分页数据列表()
        /// </summary>
        /// <param name="tblName">表名</param>
        /// <param name="strGetFields">需要返回的列</param>
        /// <param name="fldName">排序的字段名</param>
        /// <param name="PageSize">页尺寸</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="Sort">排序的方法</param>
        /// <returns>返回结果集</returns>
        public DataTable GetPageIndexListByPage(string tblName, string strGetFields, string fldName, int PageSize, int PageIndex, string strWhere, string Sort, out int RecordCount)
        {
            return o.GetPageIndexListByPage(tblName, strGetFields, fldName, PageSize, PageIndex, strWhere, Sort, out RecordCount);
        }

        ///// <summary>
        ///// xiezh add
        ///// </summary>
        ///// <param name="tblName"></param>
        ///// <param name="strGetFields"></param>
        ///// <param name="fldName"></param>
        ///// <param name="PageSize"></param>
        ///// <param name="PageIndex"></param>
        ///// <param name="strWhere"></param>
        ///// <param name="Sort"></param>
        ///// <param name="RecordCount"></param>
        ///// <returns></returns>
        //public DataTable GetPageList(string tblName, string strGetFields, string fldName, string prKey, int PageSize, int PageIndex, string strWhere, string Sort, out int RecordCount)
        //{
        //    return o.GetPageList(tblName, strGetFields, fldName, prKey, PageSize, PageIndex, strWhere, Sort, out RecordCount);
        //}

        /// <summary>
        /// 根据存储过程获取分页数据列表
        /// </summary>
        /// <param name="tblName">表名</param>
        /// <param name="strGetFields">需要返回的列</param>
        /// <param name="fldName">排序的字段名</param>
        /// <param name="PageSize">页尺寸</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="Sort">排序的方法</param>
        /// <returns>返回结果集</returns>
        public DataTable GetListByPage(string tblName, string strGetFields, string fldName, int PageSize, int PageIndex, string strWhere, out int RecordCount)
        {
            return o.GetListByPage(tblName, strGetFields, fldName, PageSize, PageIndex, strWhere, out RecordCount);
        }

        /// <summary>
        /// 此过程序用分页排序支持多表 Group by 
        /// </summary>
        /// <param name="TableNames">表名，可以是多个表，但不能用别名</param>
        /// <param name="PrimaryKey">主键，可以为空，但@Order为空时该值不能为空</param>
        /// <param name="Fields">要取出的字段，可以是多个表的字段，可以为空，为空表示select *</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="CurrentPage">当前页，0表示第1页</param>
        /// <param name="FilterWhere">条件，可以为空，不用填 where</param>
        /// <param name="GroupBy">分组依据，可以为空，不用填 group by</param>
        /// <param name="Order">排序，可以为空，为空默认按主键升序排列，不用填 order by</param>
        /// <returns></returns>
        public DataTable GetListByPageGroupBy(string TableNames, string PrimaryKey, string Fields, int PageSize, int CurrentPage, string FilterWhere, string GroupBy, string Order)
        {
            return o.GetListByPageGroupBy(TableNames, PrimaryKey, Fields, PageSize, CurrentPage, FilterWhere, GroupBy, Order);

        }





        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string RunSql(string sql)
        {
            return o.RunSql(sql);
        }
        /// <summary>
        /// 执行传入的SQL语句，返回一个SqlDataReader对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqlDataReader Re_dr(string sql)
        {
            return o.Re_dr(sql);
        }

        /// <summary>
        /// 返回SQL语句执行结果的首行首列（此函数返回结果为数字）
        /// </summary>
        /// <param name="sql"></param>
        public int Execint(string sql)
        {
            return o.Execint(sql);
        }

        /// <summary>
        /// 返回执行结果的首行首列字符串
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string Re_String(string sql)
        {
            return o.Re_String(sql);
        }





    }
}
