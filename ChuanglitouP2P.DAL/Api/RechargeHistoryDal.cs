using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Common;

namespace ChuanglitouP2P.DAL.Api
{
    /// <summary>
    /// Class InvestDal.
    /// </summary>
    public class RechargeHistoryDal : ImplBase
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(M_Recharge_history model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into hx_Recharge_history(");
            strSql.Append("membertable_registerid,recharge_amount,recharge_time,account_amount,order_No,recharge_condition,recharge_bank)");
            strSql.Append(" values (");
            strSql.Append("@membertable_registerid,@recharge_amount,@recharge_time,@account_amount,@order_No,@recharge_condition,@recharge_bank)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@membertable_registerid", SqlDbType.Int,4),
                    new SqlParameter("@recharge_amount", SqlDbType.Decimal,17),
                    new SqlParameter("@recharge_time", SqlDbType.DateTime),
                    new SqlParameter("@account_amount", SqlDbType.Decimal,17),
                    new SqlParameter("@order_No", SqlDbType.VarChar,30),
                    new SqlParameter("@recharge_condition", SqlDbType.Int,4),
                    new SqlParameter("@recharge_bank", SqlDbType.VarChar,100)};
            parameters[0].Value = model.membertable_registerid;
            parameters[1].Value = model.recharge_amount;
            parameters[2].Value = model.recharge_time;
            parameters[3].Value = model.account_amount;
            parameters[4].Value = model.order_No;
            parameters[5].Value = model.recharge_condition;
            parameters[6].Value = model.recharge_bank;

            object obj = DbHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
    }
}
