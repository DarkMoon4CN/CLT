using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChuanglitouP2P.DBUtility;


namespace ChuanglitouP2P.Common
{
    public class Calculator
    {


        /// <summary>
        /// 计算每期管理费  本金X费率
        /// </summary>
        /// <param name="loan_management_fee">费率</param>
        /// <param name="total_amount">本金</param>
        /// <returns></returns>
        public static decimal C_fees(decimal loan_management_fee, decimal total_amount)
        {
            decimal dec = 0.0M;

            dec = total_amount * loan_management_fee / 10;

            return Math.Round(dec, 2);

        }

        /// <summary>
        /// 计算罚息
        /// </summary>
        /// <param name="targetid"></param>
        /// <param name="repayment_plan_id"></param>
        /// <returns></returns>
        public static decimal O_penalty(string targetid, string repayment_plan_id)
        {
            //这里计算罚息业务逻辑



            return 0.00M;
        }


        /// <summary>
        /// 返回统计标的所有投次人当月（当期）投资本息
        /// </summary>
        /// <param name="targetid">标的id</param>
        /// <param name="Rep_date">需要本期的还款日期</param>
        /// <returns></returns>
        public static decimal Inves_Repayment_amount(string targetid,string Rep_date)
        {
           // string sql = "select  COALESCE(sum(repayment_amount),0) as repayment_amount from hx_income_statement  where convert(varchar(7),CONVERT(DATETIME,CONVERT(VARCHAR,interest_payment_date)),120) =  convert(varchar(7),CONVERT(DATETIME,CONVERT(VARCHAR,CONVERT(VARCHAR(20),'" + Rep_date + "',110))),120) and targetid="+targetid;


            string sql = "select  COALESCE(sum(repayment_amount),0) as repayment_amount from hx_income_statement  where convert(varchar(100),CONVERT(DATETIME,CONVERT(VARCHAR,interest_payment_date)),23) =  convert(varchar(100),CONVERT(DATETIME,CONVERT(VARCHAR,CONVERT(VARCHAR(100),'" + Rep_date + "',23))),23) and targetid=" + targetid;

            return decimal.Parse(DbHelperSQL.Re_String(sql));

        }


        /// <summary>
        /// 获取交易费
        /// </summary>
        /// <param name="targetid"></param>
        /// <returns></returns>
        public static decimal GetLoan_management_fee(int targetid)
        {

            string sql = "SELECT  loan_management_fee  FROM hx_borrowing_target where targetid=" + targetid;

            return decimal.Parse(DbHelperSQL.Re_String(sql));
        }





        #region 从数据库里返回唯一的邀请码
        /// <summary>
        /// 从数据库里返回唯一的邀请码
        /// </summary>
        /// <returns></returns>
        public static string Getinvitedcode()
        {

            string code = "", sql = "";

            code = Utils.RndNumChar(8);


            sql = "select invitedcode from hx_member_table where invitedcode='" + code + "'";

            int df = DbHelperSQL.ExecuteSql(sql);

            if (df <= 0)
            {
                return code;
            }
            else
            {
                Getinvitedcode();
            }

            return code;
        } 
        #endregion





    }
}
