using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Common;

namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:borrowing_target
	/// </summary>
	public partial class D_borrowing_target
	{
		public D_borrowing_target()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("targetid", "hx_borrowing_target"); 
		}


      






		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int targetid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_borrowing_target");
			strSql.Append(" where targetid=@targetid");
			SqlParameter[] parameters = {
					new SqlParameter("@targetid", SqlDbType.Int,4)
			};
			parameters[0].Value = targetid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}
        

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_borrowing_target model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_borrowing_target(");
            strSql.Append("borrower_registerid,loan_number,borrowing_title,borrowing_thumbnail,project_type_id,annual_interest_rate,borrowing_balance,life_of_loan,unit_day,release_date,month_payment_date,repayment_date,minimum,maxmum,companyid,guarantee_way_id,payment_options,end_time,service_charge,loan_management_fee,investors_management_fee,ordinary_overdue_management_fees,seriously_overdue_management_fees,ordinary_overdue_penalty,seriously_overdue_penalty,transfer_Expenses,guarantee_fee,B_Rates,recommend,sys_time,IsUse)");
			strSql.Append(" values (");
            strSql.Append("@borrower_registerid,@loan_number,@borrowing_title,@borrowing_thumbnail,@project_type_id,@annual_interest_rate,@borrowing_balance,@life_of_loan,@unit_day,@release_date,@month_payment_date,@repayment_date,@minimum,@maxmum,@companyid,@guarantee_way_id,@payment_options,@end_time,@service_charge,@loan_management_fee,@investors_management_fee,@ordinary_overdue_management_fees,@seriously_overdue_management_fees,@ordinary_overdue_penalty,@seriously_overdue_penalty,@transfer_Expenses,@guarantee_fee,@B_Rates,@recommend,@sys_time,@IsUse)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@borrower_registerid", SqlDbType.Int,4),
					new SqlParameter("@loan_number", SqlDbType.Decimal,18),
					new SqlParameter("@borrowing_title", SqlDbType.VarChar,200),
					new SqlParameter("@borrowing_thumbnail", SqlDbType.VarChar,200),
					new SqlParameter("@project_type_id", SqlDbType.Int,4),
					new SqlParameter("@annual_interest_rate", SqlDbType.Decimal,18),
					new SqlParameter("@borrowing_balance", SqlDbType.Decimal,17),
					new SqlParameter("@life_of_loan", SqlDbType.Int,4),
					new SqlParameter("@unit_day", SqlDbType.Int,4),
					new SqlParameter("@release_date", SqlDbType.DateTime),
					
					new SqlParameter("@month_payment_date", SqlDbType.Int,4),
					new SqlParameter("@repayment_date", SqlDbType.DateTime),
					new SqlParameter("@minimum", SqlDbType.Decimal,17),
					new SqlParameter("@maxmum", SqlDbType.Decimal,17),
					new SqlParameter("@companyid", SqlDbType.Int,4),
					new SqlParameter("@guarantee_way_id", SqlDbType.Int,4),
					new SqlParameter("@payment_options", SqlDbType.Int,4),
					new SqlParameter("@end_time", SqlDbType.DateTime),
					new SqlParameter("@service_charge", SqlDbType.Decimal,18),
					new SqlParameter("@loan_management_fee", SqlDbType.Decimal,18),
					new SqlParameter("@investors_management_fee", SqlDbType.Decimal,18),
					new SqlParameter("@ordinary_overdue_management_fees", SqlDbType.Decimal,18),
					new SqlParameter("@seriously_overdue_management_fees", SqlDbType.Decimal,18),
					new SqlParameter("@ordinary_overdue_penalty", SqlDbType.Decimal,18),
					new SqlParameter("@seriously_overdue_penalty", SqlDbType.Decimal,18),
					new SqlParameter("@transfer_Expenses", SqlDbType.Decimal,18),
                    new SqlParameter("@guarantee_fee", SqlDbType.Decimal,18),
                    new SqlParameter("@B_Rates", SqlDbType.Decimal,18),
                    new SqlParameter("@recommend", SqlDbType.Int,4),
                    new SqlParameter("@sys_time", SqlDbType.DateTime),
                    new SqlParameter("@IsUse", SqlDbType.Int,4)
                    
                    
                    
					};
			parameters[0].Value = model.borrower_registerid;
			parameters[1].Value = model.loan_number;
			parameters[2].Value = model.borrowing_title;
			parameters[3].Value = model.borrowing_thumbnail;
			parameters[4].Value = model.project_type_id;
			parameters[5].Value = model.annual_interest_rate;
			parameters[6].Value = model.borrowing_balance;
			parameters[7].Value = model.life_of_loan;
			parameters[8].Value = model.unit_day;
			parameters[9].Value = model.release_date;		
			parameters[10].Value = model.month_payment_date;
			parameters[11].Value = model.repayment_date;
			parameters[12].Value = model.minimum;
			parameters[13].Value = model.maxmum;
			parameters[14].Value = model.companyid;
			parameters[15].Value = model.guarantee_way_id;
			parameters[16].Value = model.payment_options;
			parameters[17].Value = model.end_time;
			parameters[18].Value = model.service_charge;
			parameters[19].Value = model.loan_management_fee;
			parameters[20].Value = model.investors_management_fee;
			parameters[21].Value = model.ordinary_overdue_management_fees;
			parameters[22].Value = model.seriously_overdue_management_fees;
			parameters[23].Value = model.ordinary_overdue_penalty;
			parameters[24].Value = model.seriously_overdue_penalty;
			parameters[25].Value = model.transfer_Expenses;
            parameters[26].Value = model.guarantee_fee;
            parameters[27].Value = model.B_Rates;
            parameters[28].Value = model.recommend;
            parameters[29].Value = model.sys_time;
            parameters[30].Value = model.IsUse;
            

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
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
        /// 根据条件得到一个对象实体
        /// </summary>
        /// <param name="lifeOfLoan"></param>
        /// <returns></returns>
        public M_borrowing_target_ZhuoLu GetModelByLifeLoan(int lifeOfLoan)
        {
            string sql = @"select top 1 targetid,borrowing_title,annual_interest_rate,life_of_loan,fundraising_amount,borrowing_balance from [dbo].[hx_borrowing_target]
                            where tender_state >= 2 and end_time >GETDATE() and unit_day = 1 and life_of_loan = @life_of_loan and borrowing_balance>0
                            order by tender_state asc,annual_interest_rate desc";
            SqlParameter[] parameters = {
                    new SqlParameter("@life_of_loan", SqlDbType.Int) };
            parameters[0].Value = lifeOfLoan;
            DataSet ds = DbHelperSQL.Query(sql, parameters);
            if (ds != null && ds.Tables.Count > 0)
            {
                M_borrowing_target_ZhuoLu res = DataHelper.GetEntity<M_borrowing_target_ZhuoLu>(ds.Tables[0]);
                return res;
            }
            else
                return null;
        }

        /// <summary>
        /// 根据参数获取借款标部分信息
        /// </summary>
        /// <param name="tenderState"></param>
        /// <returns></returns>
        public M_borrowTargetZhuolu GetModelByParams(int tenderState)
        {
            string sql = string.Format(@"select top 1 c.targetid,c.project_type_name,c.annual_interest_rate,c.life_of_loan,c.unit_day from (
                            select c.targetid,d.project_type_name,c.annual_interest_rate,c.life_of_loan,c.unit_day,case when c.unit_day=1 then c.life_of_loan*30 else c.life_of_loan end as lifeLoan from hx_borrowing_target c
                            left join hx_Project_type d
                            on c.project_type_id = d.project_type_id
                            {0}
                            ) c
                            order by c.lifeLoan desc", tenderState != -1 ? "where c.tender_state = 2 and c.end_time > GETDATE() and c.fundraising_amount<c.borrowing_balance" : "where c.tender_state > 2");
            DataSet ds = DbHelperSQL.Query(sql);
            return DataHelper.GetEntity<M_borrowTargetZhuolu>(ds.Tables[0]);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(M_borrowing_target model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_borrowing_target set ");
			strSql.Append("borrower_registerid=@borrower_registerid,");
			strSql.Append("loan_number=@loan_number,");
			strSql.Append("borrowing_title=@borrowing_title,");
			strSql.Append("borrowing_thumbnail=@borrowing_thumbnail,");
			strSql.Append("project_type_id=@project_type_id,");
			strSql.Append("annual_interest_rate=@annual_interest_rate,");
			strSql.Append("borrowing_balance=@borrowing_balance,");
			strSql.Append("life_of_loan=@life_of_loan,");
			strSql.Append("unit_day=@unit_day,");
			strSql.Append("release_date=@release_date,");
			
			strSql.Append("month_payment_date=@month_payment_date,");
			strSql.Append("repayment_date=@repayment_date,");
			strSql.Append("minimum=@minimum,");
			strSql.Append("maxmum=@maxmum,");
			strSql.Append("companyid=@companyid,");
			strSql.Append("guarantee_way_id=@guarantee_way_id,");
			strSql.Append("payment_options=@payment_options,");
			strSql.Append("end_time=@end_time,");
			strSql.Append("service_charge=@service_charge,");
			strSql.Append("loan_management_fee=@loan_management_fee,");
			strSql.Append("investors_management_fee=@investors_management_fee,");
			strSql.Append("ordinary_overdue_management_fees=@ordinary_overdue_management_fees,");
			strSql.Append("seriously_overdue_management_fees=@seriously_overdue_management_fees,");
			strSql.Append("ordinary_overdue_penalty=@ordinary_overdue_penalty,");
			strSql.Append("seriously_overdue_penalty=@seriously_overdue_penalty,");
			strSql.Append("transfer_Expenses=@transfer_Expenses,");
            strSql.Append("guarantee_fee=@guarantee_fee,");

            strSql.Append("B_Rates=@B_Rates,");
            strSql.Append("recommend=@recommend,");
            strSql.Append("sys_time=@sys_time, ");
            strSql.Append("IsUse=@IsUse ");

            
            
			
			strSql.Append(" where targetid=@targetid");
			SqlParameter[] parameters = {
					new SqlParameter("@borrower_registerid", SqlDbType.Int,4),
					new SqlParameter("@loan_number", SqlDbType.Decimal,18),
					new SqlParameter("@borrowing_title", SqlDbType.VarChar,200),
					new SqlParameter("@borrowing_thumbnail", SqlDbType.VarChar,200),
					new SqlParameter("@project_type_id", SqlDbType.Int,4),
					new SqlParameter("@annual_interest_rate", SqlDbType.Decimal,18),
					new SqlParameter("@borrowing_balance", SqlDbType.Decimal,17),
					new SqlParameter("@life_of_loan", SqlDbType.Int,4),
					new SqlParameter("@unit_day", SqlDbType.Int,4),
					new SqlParameter("@release_date", SqlDbType.DateTime),
					
					new SqlParameter("@month_payment_date", SqlDbType.Int,4),
					new SqlParameter("@repayment_date", SqlDbType.DateTime),
					new SqlParameter("@minimum", SqlDbType.Decimal,17),
					new SqlParameter("@maxmum", SqlDbType.Decimal,17),
					new SqlParameter("@companyid", SqlDbType.Int,4),
					new SqlParameter("@guarantee_way_id", SqlDbType.Int,4),
					new SqlParameter("@payment_options", SqlDbType.Int,4),
					new SqlParameter("@end_time", SqlDbType.DateTime),
					new SqlParameter("@service_charge", SqlDbType.Decimal,18),
					new SqlParameter("@loan_management_fee", SqlDbType.Decimal,18),
					new SqlParameter("@investors_management_fee", SqlDbType.Decimal,18),
					new SqlParameter("@ordinary_overdue_management_fees", SqlDbType.Decimal,18),
					new SqlParameter("@seriously_overdue_management_fees", SqlDbType.Decimal,18),
					new SqlParameter("@ordinary_overdue_penalty", SqlDbType.Decimal,18),
					new SqlParameter("@seriously_overdue_penalty", SqlDbType.Decimal,18),
					new SqlParameter("@transfer_Expenses", SqlDbType.Decimal,18),
					new SqlParameter("@guarantee_fee", SqlDbType.Decimal,18),
                    new SqlParameter("@B_Rates", SqlDbType.Decimal,18),
                    new SqlParameter("@recommend", SqlDbType.Int,4),
                    new SqlParameter("@sys_time", SqlDbType.DateTime),  
                    new SqlParameter("@IsUse", SqlDbType.Int,4), 
					new SqlParameter("@targetid", SqlDbType.Int,4)};
			parameters[0].Value = model.borrower_registerid;
			parameters[1].Value = model.loan_number;
			parameters[2].Value = model.borrowing_title;
			parameters[3].Value = model.borrowing_thumbnail;
			parameters[4].Value = model.project_type_id;
			parameters[5].Value = model.annual_interest_rate;
			parameters[6].Value = model.borrowing_balance;
			parameters[7].Value = model.life_of_loan;
			parameters[8].Value = model.unit_day;
			parameters[9].Value = model.release_date;
		
			parameters[10].Value = model.month_payment_date;
			parameters[11].Value = model.repayment_date;
			parameters[12].Value = model.minimum;
			parameters[13].Value = model.maxmum;
			parameters[14].Value = model.companyid;
			parameters[15].Value = model.guarantee_way_id;
			parameters[16].Value = model.payment_options;
			parameters[17].Value = model.end_time;
			parameters[18].Value = model.service_charge;
			parameters[19].Value = model.loan_management_fee;
			parameters[20].Value = model.investors_management_fee;
			parameters[21].Value = model.ordinary_overdue_management_fees;
			parameters[22].Value = model.seriously_overdue_management_fees;
			parameters[23].Value = model.ordinary_overdue_penalty;
			parameters[24].Value = model.seriously_overdue_penalty;
			parameters[25].Value = model.transfer_Expenses;
            parameters[26].Value = model.guarantee_fee;
            parameters[27].Value = model.B_Rates;
            parameters[28].Value = model.recommend;
			parameters[29].Value = model.sys_time;
            parameters[30].Value = model.IsUse;
            parameters[31].Value = model.targetid;
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
		/// 删除一条数据
		/// </summary>
		public bool Delete(int targetid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_borrowing_target ");
			strSql.Append(" where targetid=@targetid");
			SqlParameter[] parameters = {
					new SqlParameter("@targetid", SqlDbType.Int,4)
			};
			parameters[0].Value = targetid;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string targetidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_borrowing_target ");
			strSql.Append(" where targetid in ("+targetidlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
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
		/// 得到一个对象实体
		/// </summary>
		public M_borrowing_target GetModel(int targetid)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 targetid,borrower_registerid,loan_number,borrowing_title,borrowing_thumbnail,project_type_id,annual_interest_rate,borrowing_balance,life_of_loan,unit_day,release_date,value_date,month_payment_date,repayment_date,minimum,maxmum,companyid,guarantee_way_id,payment_options,start_time,end_time,service_charge,loan_management_fee,investors_management_fee,ordinary_overdue_management_fees,seriously_overdue_management_fees,ordinary_overdue_penalty,seriously_overdue_penalty,transfer_Expenses,fundraising_amount,tender_state,full_scale_loan,flow_return,recommend,sys_time,repaymentperiods,guarantee_fee,consultingAMT,guaranteeAMT,B_Rates,IsUse from hx_borrowing_target ");
			strSql.Append(" where targetid=@targetid");
			SqlParameter[] parameters = {
					new SqlParameter("@targetid", SqlDbType.Int,4)
			};
			parameters[0].Value = targetid;

			M_borrowing_target model=new M_borrowing_target();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public M_borrowing_target DataRowToModel(DataRow row)
		{
			M_borrowing_target model=new M_borrowing_target();
			if (row != null)
			{
				if(row["targetid"]!=null && row["targetid"].ToString()!="")
				{
					model.targetid=int.Parse(row["targetid"].ToString());
				}
				if(row["borrower_registerid"]!=null && row["borrower_registerid"].ToString()!="")
				{
					model.borrower_registerid=int.Parse(row["borrower_registerid"].ToString());
				}
				if(row["loan_number"]!=null && row["loan_number"].ToString()!="")
				{
                    model.loan_number = decimal.Parse(row["loan_number"].ToString());
				}
				if(row["borrowing_title"]!=null)
				{
					model.borrowing_title=row["borrowing_title"].ToString();
				}
				if(row["borrowing_thumbnail"]!=null)
				{
					model.borrowing_thumbnail=row["borrowing_thumbnail"].ToString();
				}
				if(row["project_type_id"]!=null && row["project_type_id"].ToString()!="")
				{
					model.project_type_id=int.Parse(row["project_type_id"].ToString());
				}
				if(row["annual_interest_rate"]!=null && row["annual_interest_rate"].ToString()!="")
				{
					model.annual_interest_rate=decimal.Parse(row["annual_interest_rate"].ToString());
				}
				if(row["borrowing_balance"]!=null && row["borrowing_balance"].ToString()!="")
				{
					model.borrowing_balance=decimal.Parse(row["borrowing_balance"].ToString());
				}
				if(row["life_of_loan"]!=null && row["life_of_loan"].ToString()!="")
				{
					model.life_of_loan=int.Parse(row["life_of_loan"].ToString());
				}
				if(row["unit_day"]!=null && row["unit_day"].ToString()!="")
				{
					model.unit_day=int.Parse(row["unit_day"].ToString());
				}
				if(row["release_date"]!=null && row["release_date"].ToString()!="")
				{
					model.release_date=DateTime.Parse(row["release_date"].ToString());
				}
				if(row["value_date"]!=null && row["value_date"].ToString()!="")
				{
					model.value_date=DateTime.Parse(row["value_date"].ToString());
				}
				if(row["month_payment_date"]!=null && row["month_payment_date"].ToString()!="")
				{
					model.month_payment_date=int.Parse(row["month_payment_date"].ToString());
				}
				if(row["repayment_date"]!=null && row["repayment_date"].ToString()!="")
				{
					model.repayment_date=DateTime.Parse(row["repayment_date"].ToString());
				}
				if(row["minimum"]!=null && row["minimum"].ToString()!="")
				{
					model.minimum=decimal.Parse(row["minimum"].ToString());
				}
				if(row["maxmum"]!=null && row["maxmum"].ToString()!="")
				{
					model.maxmum=decimal.Parse(row["maxmum"].ToString());
				}
				if(row["companyid"]!=null && row["companyid"].ToString()!="")
				{
					model.companyid=int.Parse(row["companyid"].ToString());
				}
				if(row["guarantee_way_id"]!=null && row["guarantee_way_id"].ToString()!="")
				{
					model.guarantee_way_id=int.Parse(row["guarantee_way_id"].ToString());
				}
				if(row["payment_options"]!=null && row["payment_options"].ToString()!="")
				{
					model.payment_options=int.Parse(row["payment_options"].ToString());
				}



                if (row["start_time"] != null && row["start_time"].ToString() != "")
				{
                    model.start_time = DateTime.Parse(row["start_time"].ToString());
				}


				if(row["end_time"]!=null && row["end_time"].ToString()!="")
				{
					model.end_time=DateTime.Parse(row["end_time"].ToString());
				}
				if(row["service_charge"]!=null && row["service_charge"].ToString()!="")
				{
					model.service_charge=decimal.Parse(row["service_charge"].ToString());
				}
				if(row["loan_management_fee"]!=null && row["loan_management_fee"].ToString()!="")
				{
					model.loan_management_fee=decimal.Parse(row["loan_management_fee"].ToString());
				}
				if(row["investors_management_fee"]!=null && row["investors_management_fee"].ToString()!="")
				{
					model.investors_management_fee=decimal.Parse(row["investors_management_fee"].ToString());
				}
				if(row["ordinary_overdue_management_fees"]!=null && row["ordinary_overdue_management_fees"].ToString()!="")
				{
					model.ordinary_overdue_management_fees=decimal.Parse(row["ordinary_overdue_management_fees"].ToString());
				}
				if(row["seriously_overdue_management_fees"]!=null && row["seriously_overdue_management_fees"].ToString()!="")
				{
					model.seriously_overdue_management_fees=decimal.Parse(row["seriously_overdue_management_fees"].ToString());
				}
				if(row["ordinary_overdue_penalty"]!=null && row["ordinary_overdue_penalty"].ToString()!="")
				{
					model.ordinary_overdue_penalty=decimal.Parse(row["ordinary_overdue_penalty"].ToString());
				}
				if(row["seriously_overdue_penalty"]!=null && row["seriously_overdue_penalty"].ToString()!="")
				{
					model.seriously_overdue_penalty=decimal.Parse(row["seriously_overdue_penalty"].ToString());
				}
				if(row["transfer_Expenses"]!=null && row["transfer_Expenses"].ToString()!="")
				{
					model.transfer_Expenses=decimal.Parse(row["transfer_Expenses"].ToString());
				}
				if(row["fundraising_amount"]!=null && row["fundraising_amount"].ToString()!="")
				{
					model.fundraising_amount=decimal.Parse(row["fundraising_amount"].ToString());
				}
				if(row["tender_state"]!=null && row["tender_state"].ToString()!="")
				{
					model.tender_state=int.Parse(row["tender_state"].ToString());
				}
				if(row["full_scale_loan"]!=null && row["full_scale_loan"].ToString()!="")
				{
					model.full_scale_loan=int.Parse(row["full_scale_loan"].ToString());
				}
				if(row["flow_return"]!=null && row["flow_return"].ToString()!="")
				{
					model.flow_return=int.Parse(row["flow_return"].ToString());
				}
				if(row["recommend"]!=null && row["recommend"].ToString()!="")
				{
					model.recommend=int.Parse(row["recommend"].ToString());
				}
				if(row["sys_time"]!=null && row["sys_time"].ToString()!="")
				{
					model.sys_time=DateTime.Parse(row["sys_time"].ToString());
				}
                if (row["repaymentperiods"] != null && row["repaymentperiods"].ToString() != "")
				{
                    model.repayment_periods = int.Parse(row["repaymentperiods"].ToString());
				}

                if(row["guarantee_fee"]!=null && row["guarantee_fee"].ToString()!="")
				{
					model.guarantee_fee=decimal.Parse(row["guarantee_fee"].ToString());
				}

                if (row["consultingAMT"] != null && row["consultingAMT"].ToString() != "")
				{
                    model.consultingAMT = decimal.Parse(row["consultingAMT"].ToString());
				}


                if (row["guaranteeAMT"] != null && row["guaranteeAMT"].ToString() != "")
				{
                    model.guaranteeAMT = decimal.Parse(row["guaranteeAMT"].ToString());
				}

                if (row["B_Rates"] != null && row["B_Rates"].ToString() != "")
				{
                    model.B_Rates = decimal.Parse(row["B_Rates"].ToString());
				}

                if (row["sys_time"] != null && row["sys_time"].ToString() != "")
				{
                    model.sys_time = DateTime.Parse(row["sys_time"].ToString());
				}

                if (row["IsUse"] != null && row["IsUse"].ToString() != "")
				{
                    model.IsUse = int.Parse(row["IsUse"].ToString());
				}
                
                    
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select targetid,borrower_registerid,loan_number,borrowing_title,borrowing_thumbnail,project_type_id,annual_interest_rate,borrowing_balance,life_of_loan,unit_day,release_date,value_date,month_payment_date,repayment_date,minimum,maxmum,companyid,guarantee_way_id,payment_options,end_time,service_charge,loan_management_fee,investors_management_fee,ordinary_overdue_management_fees,seriously_overdue_management_fees,ordinary_overdue_penalty,seriously_overdue_penalty,transfer_Expenses,fundraising_amount,tender_state,full_scale_loan,flow_return,recommend,sys_time,repayment periods ");
			strSql.Append(" FROM hx_borrowing_target ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" targetid,borrower_registerid,loan_number,borrowing_title,borrowing_thumbnail,project_type_id,annual_interest_rate,borrowing_balance,life_of_loan,unit_day,release_date,value_date,month_payment_date,repayment_date,minimum,maxmum,companyid,guarantee_way_id,payment_options,end_time,service_charge,loan_management_fee,investors_management_fee,ordinary_overdue_management_fees,seriously_overdue_management_fees,ordinary_overdue_penalty,seriously_overdue_penalty,transfer_Expenses,fundraising_amount,tender_state,full_scale_loan,flow_return,recommend,sys_time,repayment periods ");
			strSql.Append(" FROM hx_borrowing_target ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM hx_borrowing_target ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.targetid desc");
			}
			strSql.Append(")AS Row, T.*  from hx_borrowing_target T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "hx_borrowing_target";
			parameters[1].Value = "targetid";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

