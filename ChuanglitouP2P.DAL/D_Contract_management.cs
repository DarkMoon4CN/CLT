using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:Contract_management
	/// </summary>
	public partial class D_Contract_management
	{
		public D_Contract_management()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("contract", "Contract_management"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int contract)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Contract_management");
			strSql.Append(" where contract=@contract");
			SqlParameter[] parameters = {
					new SqlParameter("@contract", SqlDbType.Int,4)
			};
			parameters[0].Value = contract;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}

        
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_Contract_management model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Contract_management(");
            strSql.Append("loan_number,targetid,lender_name,lender_username,lender_registerid,lender_id_card,lenders_account_name,lender_bank_account,lender_bank,lender_address,lenders_telephone,lenders_email,lendres_date_contract,borrower_registerid,borrower_name,borrower_username,borrower_id_card,borrower_account_name,borrower_bank_account,borrower_date_contract,borrower_bank,witness_name,witness_address,witness_telephone,witness_agent,witness_admid,witness_username,witness_id_card,witness_date_contract,surety_company_name,guarantee_business_icense,ensure_address,guarantee_legal_representative,guarantor_agent,guarantor_companyid,guarantor_agent_usernqme,guarantor_agent_id_card,guarantor_agent_idate_contract,contract_money,counselling_service,contract_amount,suretyship_contract,mode_payment,createtime,contract_type,contractpath,bid_records_id)");
			strSql.Append(" values (");
            strSql.Append("@loan_number,@targetid,@lender_name,@lender_username,@lender_registerid,@lender_id_card,@lenders_account_name,@lender_bank_account,@lender_bank,@lender_address,@lenders_telephone,@lenders_email,@lendres_date_contract,@borrower_registerid,@borrower_name,@borrower_username,@borrower_id_card,@borrower_account_name,@borrower_bank_account,@borrower_date_contract,@borrower_bank,@witness_name,@witness_address,@witness_telephone,@witness_agent,@witness_admid,@witness_username,@witness_id_card,@witness_date_contract,@surety_company_name,@guarantee_business_icense,@ensure_address,@guarantee_legal_representative,@guarantor_agent,@guarantor_companyid,@guarantor_agent_usernqme,@guarantor_agent_id_card,@guarantor_agent_idate_contract,@contract_money,@counselling_service,@contract_amount,@suretyship_contract,@mode_payment,@createtime,@contract_type,@contractpath,@bid_records_id)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@loan_number", SqlDbType.Decimal,18),
					new SqlParameter("@targetid", SqlDbType.Int,4),
					new SqlParameter("@lender_name", SqlDbType.VarChar,50),
					new SqlParameter("@lender_username", SqlDbType.VarChar,50),
					new SqlParameter("@lender_registerid", SqlDbType.Int,4),
					new SqlParameter("@lender_id_card", SqlDbType.VarChar,50),
					new SqlParameter("@lenders_account_name", SqlDbType.VarChar,50),
					new SqlParameter("@lender_bank_account", SqlDbType.VarChar,50),
					new SqlParameter("@lender_bank", SqlDbType.VarChar,50),
					new SqlParameter("@lender_address", SqlDbType.VarChar,100),
					new SqlParameter("@lenders_telephone", SqlDbType.VarChar,50),
					new SqlParameter("@lenders_email", SqlDbType.VarChar,50),
					new SqlParameter("@lendres_date_contract", SqlDbType.DateTime),
					new SqlParameter("@borrower_registerid", SqlDbType.Int,4),
					new SqlParameter("@borrower_name", SqlDbType.VarChar,50),
					new SqlParameter("@borrower_username", SqlDbType.VarChar,50),
					new SqlParameter("@borrower_id_card", SqlDbType.VarChar,50),
					new SqlParameter("@borrower_account_name", SqlDbType.VarChar,50),
					new SqlParameter("@borrower_bank_account", SqlDbType.VarChar,50),
					new SqlParameter("@borrower_date_contract", SqlDbType.VarChar,50),
					new SqlParameter("@borrower_bank", SqlDbType.VarChar,50),
					new SqlParameter("@witness_name", SqlDbType.VarChar,50),
					new SqlParameter("@witness_address", SqlDbType.VarChar,50),
					new SqlParameter("@witness_telephone", SqlDbType.VarChar,50),
					new SqlParameter("@witness_agent", SqlDbType.VarChar,50),
					new SqlParameter("@witness_admid", SqlDbType.Int,4),
					new SqlParameter("@witness_username", SqlDbType.VarChar,50),
					new SqlParameter("@witness_id_card", SqlDbType.VarChar,50),
					new SqlParameter("@witness_date_contract", SqlDbType.DateTime),
					new SqlParameter("@surety_company_name", SqlDbType.VarChar,100),
					new SqlParameter("@guarantee_business_icense", SqlDbType.VarChar,50),
					new SqlParameter("@ensure_address", SqlDbType.VarChar,50),
					new SqlParameter("@guarantee_legal_representative", SqlDbType.VarChar,50),
					new SqlParameter("@guarantor_agent", SqlDbType.VarChar,50),
					new SqlParameter("@guarantor_companyid", SqlDbType.Int,4),
					new SqlParameter("@guarantor_agent_usernqme", SqlDbType.VarChar,50),
					new SqlParameter("@guarantor_agent_id_card", SqlDbType.VarChar,50),
					new SqlParameter("@guarantor_agent_idate_contract", SqlDbType.DateTime),
					new SqlParameter("@contract_money", SqlDbType.VarChar,-1),
					new SqlParameter("@counselling_service", SqlDbType.VarChar,-1),
					new SqlParameter("@contract_amount", SqlDbType.Decimal,17),
					new SqlParameter("@suretyship_contract", SqlDbType.VarChar,-1),
					new SqlParameter("@mode_payment", SqlDbType.VarChar,255),
					new SqlParameter("@createtime", SqlDbType.DateTime),
                    new SqlParameter("@contract_type", SqlDbType.Int,4),
                    new SqlParameter("@contractpath", SqlDbType.VarChar,255),
                     new SqlParameter("@bid_records_id", SqlDbType.Int,4)};


            
			parameters[0].Value = model.loan_number;
			parameters[1].Value = model.targetid;
			parameters[2].Value = model.lender_name;
			parameters[3].Value = model.lender_username;
			parameters[4].Value = model.lender_registerid;
			parameters[5].Value = model.lender_id_card;
			parameters[6].Value = model.lenders_account_name;
			parameters[7].Value = model.lender_bank_account;
			parameters[8].Value = model.lender_bank;
			parameters[9].Value = model.lender_address;
			parameters[10].Value = model.lenders_telephone;
			parameters[11].Value = model.lenders_email;
			parameters[12].Value = model.lendres_date_contract;
			parameters[13].Value = model.borrower_registerid;
			parameters[14].Value = model.borrower_name;
			parameters[15].Value = model.borrower_username;
			parameters[16].Value = model.borrower_id_card;
			parameters[17].Value = model.borrower_account_name;
			parameters[18].Value = model.borrower_bank_account;
			parameters[19].Value = model.borrower_date_contract;
			parameters[20].Value = model.borrower_bank;
			parameters[21].Value = model.witness_name;
			parameters[22].Value = model.witness_address;
			parameters[23].Value = model.witness_telephone;
			parameters[24].Value = model.witness_agent;
			parameters[25].Value = model.witness_admid;
			parameters[26].Value = model.witness_username;
			parameters[27].Value = model.witness_id_card;
			parameters[28].Value = model.witness_date_contract;
			parameters[29].Value = model.surety_company_name;
			parameters[30].Value = model.guarantee_business_icense;
			parameters[31].Value = model.ensure_address;
			parameters[32].Value = model.guarantee_legal_representative;
			parameters[33].Value = model.guarantor_agent;
			parameters[34].Value = model.guarantor_companyid;
			parameters[35].Value = model.guarantor_agent_usernqme;
			parameters[36].Value = model.guarantor_agent_id_card;
			parameters[37].Value = model.guarantor_agent_idate_contract;
			parameters[38].Value = model.contract_money;
			parameters[39].Value = model.counselling_service;
			parameters[40].Value = model.contract_amount;
			parameters[41].Value = model.suretyship_contract;
			parameters[42].Value = model.mode_payment;
			parameters[43].Value = model.createtime;
            parameters[44].Value = model.contract_type;
            parameters[45].Value = model.contractpath;
            parameters[46].Value = model.bid_records_id;
            
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
		/// 更新一条数据
		/// </summary>
		public bool Update(M_Contract_management model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Contract_management set ");
			strSql.Append("loan_number=@loan_number,");
			strSql.Append("targetid=@targetid,");
			strSql.Append("lender_name=@lender_name,");
			strSql.Append("lender_username=@lender_username,");
			strSql.Append("lender_registerid=@lender_registerid,");
			strSql.Append("lender_id_card=@lender_id_card,");
			strSql.Append("lenders_account_name=@lenders_account_name,");
			strSql.Append("lender_bank_account=@lender_bank_account,");
			strSql.Append("lender_bank=@lender_bank,");
			strSql.Append("lender_address=@lender_address,");
			strSql.Append("lenders_telephone=@lenders_telephone,");
			strSql.Append("lenders_email=@lenders_email,");
			strSql.Append("lendres_date_contract=@lendres_date_contract,");
			strSql.Append("borrower_registerid=@borrower_registerid,");
			strSql.Append("borrower_name=@borrower_name,");
			strSql.Append("borrower_username=@borrower_username,");
			strSql.Append("borrower_id_card=@borrower_id_card,");
			strSql.Append("borrower_account_name=@borrower_account_name,");
			strSql.Append("borrower_bank_account=@borrower_bank_account,");
			strSql.Append("borrower_date_contract=@borrower_date_contract,");
			strSql.Append("borrower_bank=@borrower_bank,");
			strSql.Append("witness_name=@witness_name,");
			strSql.Append("witness_address=@witness_address,");
			strSql.Append("witness_telephone=@witness_telephone,");
			strSql.Append("witness_agent=@witness_agent,");
			strSql.Append("witness_admid=@witness_admid,");
			strSql.Append("witness_username=@witness_username,");
			strSql.Append("witness_id_card=@witness_id_card,");
			strSql.Append("witness_date_contract=@witness_date_contract,");
			strSql.Append("surety_company_name=@surety_company_name,");
			strSql.Append("guarantee_business_icense=@guarantee_business_icense,");
			strSql.Append("ensure_address=@ensure_address,");
			strSql.Append("guarantee_legal_representative=@guarantee_legal_representative,");
			strSql.Append("guarantor_agent=@guarantor_agent,");
			strSql.Append("guarantor_companyid=@guarantor_companyid,");
			strSql.Append("guarantor_agent_usernqme=@guarantor_agent_usernqme,");
			strSql.Append("guarantor_agent_id_card=@guarantor_agent_id_card,");
			strSql.Append("guarantor_agent_idate_contract=@guarantor_agent_idate_contract,");
			strSql.Append("contract_money=@contract_money,");
			strSql.Append("counselling_service=@counselling_service,");
			strSql.Append("contract_amount=@contract_amount,");
			strSql.Append("suretyship_contract=@suretyship_contract,");
			strSql.Append("mode_payment=@mode_payment,");
			strSql.Append("createtime=@createtime");
			strSql.Append(" where contract=@contract");
			SqlParameter[] parameters = {
					new SqlParameter("@loan_number", SqlDbType.Decimal,18),
					new SqlParameter("@targetid", SqlDbType.Int,4),
					new SqlParameter("@lender_name", SqlDbType.VarChar,50),
					new SqlParameter("@lender_username", SqlDbType.VarChar,50),
					new SqlParameter("@lender_registerid", SqlDbType.Int,4),
					new SqlParameter("@lender_id_card", SqlDbType.VarChar,50),
					new SqlParameter("@lenders_account_name", SqlDbType.VarChar,50),
					new SqlParameter("@lender_bank_account", SqlDbType.VarChar,50),
					new SqlParameter("@lender_bank", SqlDbType.VarChar,50),
					new SqlParameter("@lender_address", SqlDbType.VarChar,100),
					new SqlParameter("@lenders_telephone", SqlDbType.VarChar,50),
					new SqlParameter("@lenders_email", SqlDbType.VarChar,50),
					new SqlParameter("@lendres_date_contract", SqlDbType.DateTime),
					new SqlParameter("@borrower_registerid", SqlDbType.Int,4),
					new SqlParameter("@borrower_name", SqlDbType.VarChar,50),
					new SqlParameter("@borrower_username", SqlDbType.VarChar,50),
					new SqlParameter("@borrower_id_card", SqlDbType.VarChar,50),
					new SqlParameter("@borrower_account_name", SqlDbType.VarChar,50),
					new SqlParameter("@borrower_bank_account", SqlDbType.VarChar,50),
					new SqlParameter("@borrower_date_contract", SqlDbType.VarChar,50),
					new SqlParameter("@borrower_bank", SqlDbType.VarChar,50),
					new SqlParameter("@witness_name", SqlDbType.VarChar,50),
					new SqlParameter("@witness_address", SqlDbType.VarChar,50),
					new SqlParameter("@witness_telephone", SqlDbType.VarChar,50),
					new SqlParameter("@witness_agent", SqlDbType.VarChar,50),
					new SqlParameter("@witness_admid", SqlDbType.Int,4),
					new SqlParameter("@witness_username", SqlDbType.VarChar,50),
					new SqlParameter("@witness_id_card", SqlDbType.VarChar,50),
					new SqlParameter("@witness_date_contract", SqlDbType.DateTime),
					new SqlParameter("@surety_company_name", SqlDbType.VarChar,100),
					new SqlParameter("@guarantee_business_icense", SqlDbType.VarChar,50),
					new SqlParameter("@ensure_address", SqlDbType.VarChar,50),
					new SqlParameter("@guarantee_legal_representative", SqlDbType.VarChar,50),
					new SqlParameter("@guarantor_agent", SqlDbType.VarChar,50),
					new SqlParameter("@guarantor_companyid", SqlDbType.Int,4),
					new SqlParameter("@guarantor_agent_usernqme", SqlDbType.VarChar,50),
					new SqlParameter("@guarantor_agent_id_card", SqlDbType.VarChar,50),
					new SqlParameter("@guarantor_agent_idate_contract", SqlDbType.DateTime),
					new SqlParameter("@contract_money", SqlDbType.VarChar,-1),
					new SqlParameter("@counselling_service", SqlDbType.VarChar,-1),
					new SqlParameter("@contract_amount", SqlDbType.Decimal,17),
					new SqlParameter("@suretyship_contract", SqlDbType.VarChar,-1),
					new SqlParameter("@mode_payment", SqlDbType.VarChar,255),
					new SqlParameter("@createtime", SqlDbType.DateTime),
					new SqlParameter("@contract", SqlDbType.Int,4)};
			parameters[0].Value = model.loan_number;
			parameters[1].Value = model.targetid;
			parameters[2].Value = model.lender_name;
			parameters[3].Value = model.lender_username;
			parameters[4].Value = model.lender_registerid;
			parameters[5].Value = model.lender_id_card;
			parameters[6].Value = model.lenders_account_name;
			parameters[7].Value = model.lender_bank_account;
			parameters[8].Value = model.lender_bank;
			parameters[9].Value = model.lender_address;
			parameters[10].Value = model.lenders_telephone;
			parameters[11].Value = model.lenders_email;
			parameters[12].Value = model.lendres_date_contract;
			parameters[13].Value = model.borrower_registerid;
			parameters[14].Value = model.borrower_name;
			parameters[15].Value = model.borrower_username;
			parameters[16].Value = model.borrower_id_card;
			parameters[17].Value = model.borrower_account_name;
			parameters[18].Value = model.borrower_bank_account;
			parameters[19].Value = model.borrower_date_contract;
			parameters[20].Value = model.borrower_bank;
			parameters[21].Value = model.witness_name;
			parameters[22].Value = model.witness_address;
			parameters[23].Value = model.witness_telephone;
			parameters[24].Value = model.witness_agent;
			parameters[25].Value = model.witness_admid;
			parameters[26].Value = model.witness_username;
			parameters[27].Value = model.witness_id_card;
			parameters[28].Value = model.witness_date_contract;
			parameters[29].Value = model.surety_company_name;
			parameters[30].Value = model.guarantee_business_icense;
			parameters[31].Value = model.ensure_address;
			parameters[32].Value = model.guarantee_legal_representative;
			parameters[33].Value = model.guarantor_agent;
			parameters[34].Value = model.guarantor_companyid;
			parameters[35].Value = model.guarantor_agent_usernqme;
			parameters[36].Value = model.guarantor_agent_id_card;
			parameters[37].Value = model.guarantor_agent_idate_contract;
			parameters[38].Value = model.contract_money;
			parameters[39].Value = model.counselling_service;
			parameters[40].Value = model.contract_amount;
			parameters[41].Value = model.suretyship_contract;
			parameters[42].Value = model.mode_payment;
			parameters[43].Value = model.createtime;
			parameters[44].Value = model.contract;

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
		public bool Delete(int contract)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Contract_management ");
			strSql.Append(" where contract=@contract");
			SqlParameter[] parameters = {
					new SqlParameter("@contract", SqlDbType.Int,4)
			};
			parameters[0].Value = contract;

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
		public bool DeleteList(string contractlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Contract_management ");
			strSql.Append(" where contract in ("+contractlist + ")  ");
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
		public M_Contract_management GetModel(int contract)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 contract,loan_number,targetid,lender_name,lender_username,lender_registerid,lender_id_card,lenders_account_name,lender_bank_account,lender_bank,lender_address,lenders_telephone,lenders_email,lendres_date_contract,borrower_registerid,borrower_name,borrower_username,borrower_id_card,borrower_account_name,borrower_bank_account,borrower_date_contract,borrower_bank,witness_name,witness_address,witness_telephone,witness_agent,witness_admid,witness_username,witness_id_card,witness_date_contract,surety_company_name,guarantee_business_icense,ensure_address,guarantee_legal_representative,guarantor_agent,guarantor_companyid,guarantor_agent_usernqme,guarantor_agent_id_card,guarantor_agent_idate_contract,contract_money,counselling_service,contract_amount,suretyship_contract,mode_payment,createtime from Contract_management ");
			strSql.Append(" where contract=@contract");
			SqlParameter[] parameters = {
					new SqlParameter("@contract", SqlDbType.Int,4)
			};
			parameters[0].Value = contract;

			M_Contract_management model=new M_Contract_management();
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
		public M_Contract_management DataRowToModel(DataRow row)
		{
			M_Contract_management model=new M_Contract_management();
			if (row != null)
			{
				if(row["contract"]!=null && row["contract"].ToString()!="")
				{
					model.contract=int.Parse(row["contract"].ToString());
				}
				if(row["loan_number"]!=null && row["loan_number"].ToString()!="")
				{
					model.loan_number=decimal.Parse(row["loan_number"].ToString());
				}
				if(row["targetid"]!=null && row["targetid"].ToString()!="")
				{
					model.targetid=int.Parse(row["targetid"].ToString());
				}
				if(row["lender_name"]!=null)
				{
					model.lender_name=row["lender_name"].ToString();
				}
				if(row["lender_username"]!=null)
				{
					model.lender_username=row["lender_username"].ToString();
				}
				if(row["lender_registerid"]!=null && row["lender_registerid"].ToString()!="")
				{
					model.lender_registerid=int.Parse(row["lender_registerid"].ToString());
				}
				if(row["lender_id_card"]!=null)
				{
					model.lender_id_card=row["lender_id_card"].ToString();
				}
				if(row["lenders_account_name"]!=null)
				{
					model.lenders_account_name=row["lenders_account_name"].ToString();
				}
				if(row["lender_bank_account"]!=null)
				{
					model.lender_bank_account=row["lender_bank_account"].ToString();
				}
				if(row["lender_bank"]!=null)
				{
					model.lender_bank=row["lender_bank"].ToString();
				}
				if(row["lender_address"]!=null)
				{
					model.lender_address=row["lender_address"].ToString();
				}
				if(row["lenders_telephone"]!=null)
				{
					model.lenders_telephone=row["lenders_telephone"].ToString();
				}
				if(row["lenders_email"]!=null)
				{
					model.lenders_email=row["lenders_email"].ToString();
				}
				if(row["lendres_date_contract"]!=null && row["lendres_date_contract"].ToString()!="")
				{
					model.lendres_date_contract=DateTime.Parse(row["lendres_date_contract"].ToString());
				}
				if(row["borrower_registerid"]!=null && row["borrower_registerid"].ToString()!="")
				{
					model.borrower_registerid=int.Parse(row["borrower_registerid"].ToString());
				}
				if(row["borrower_name"]!=null)
				{
					model.borrower_name=row["borrower_name"].ToString();
				}
				if(row["borrower_username"]!=null)
				{
					model.borrower_username=row["borrower_username"].ToString();
				}
				if(row["borrower_id_card"]!=null)
				{
					model.borrower_id_card=row["borrower_id_card"].ToString();
				}
				if(row["borrower_account_name"]!=null)
				{
					model.borrower_account_name=row["borrower_account_name"].ToString();
				}
				if(row["borrower_bank_account"]!=null)
				{
					model.borrower_bank_account=row["borrower_bank_account"].ToString();
				}
				if(row["borrower_date_contract"]!=null)
				{
					model.borrower_date_contract=row["borrower_date_contract"].ToString();
				}
				if(row["borrower_bank"]!=null)
				{
					model.borrower_bank=row["borrower_bank"].ToString();
				}
				if(row["witness_name"]!=null)
				{
					model.witness_name=row["witness_name"].ToString();
				}
				if(row["witness_address"]!=null)
				{
					model.witness_address=row["witness_address"].ToString();
				}
				if(row["witness_telephone"]!=null)
				{
					model.witness_telephone=row["witness_telephone"].ToString();
				}
				if(row["witness_agent"]!=null)
				{
					model.witness_agent=row["witness_agent"].ToString();
				}
				if(row["witness_admid"]!=null && row["witness_admid"].ToString()!="")
				{
					model.witness_admid=int.Parse(row["witness_admid"].ToString());
				}
				if(row["witness_username"]!=null)
				{
					model.witness_username=row["witness_username"].ToString();
				}
				if(row["witness_id_card"]!=null)
				{
					model.witness_id_card=row["witness_id_card"].ToString();
				}
				if(row["witness_date_contract"]!=null && row["witness_date_contract"].ToString()!="")
				{
					model.witness_date_contract=DateTime.Parse(row["witness_date_contract"].ToString());
				}
				if(row["surety_company_name"]!=null)
				{
					model.surety_company_name=row["surety_company_name"].ToString();
				}
				if(row["guarantee_business_icense"]!=null)
				{
					model.guarantee_business_icense=row["guarantee_business_icense"].ToString();
				}
				if(row["ensure_address"]!=null)
				{
					model.ensure_address=row["ensure_address"].ToString();
				}
				if(row["guarantee_legal_representative"]!=null)
				{
					model.guarantee_legal_representative=row["guarantee_legal_representative"].ToString();
				}
				if(row["guarantor_agent"]!=null)
				{
					model.guarantor_agent=row["guarantor_agent"].ToString();
				}
				if(row["guarantor_companyid"]!=null && row["guarantor_companyid"].ToString()!="")
				{
					model.guarantor_companyid=int.Parse(row["guarantor_companyid"].ToString());
				}
				if(row["guarantor_agent_usernqme"]!=null)
				{
					model.guarantor_agent_usernqme=row["guarantor_agent_usernqme"].ToString();
				}
				if(row["guarantor_agent_id_card"]!=null)
				{
					model.guarantor_agent_id_card=row["guarantor_agent_id_card"].ToString();
				}
				if(row["guarantor_agent_idate_contract"]!=null && row["guarantor_agent_idate_contract"].ToString()!="")
				{
					model.guarantor_agent_idate_contract=DateTime.Parse(row["guarantor_agent_idate_contract"].ToString());
				}
				if(row["contract_money"]!=null)
				{
					model.contract_money=row["contract_money"].ToString();
				}
				if(row["counselling_service"]!=null)
				{
					model.counselling_service=row["counselling_service"].ToString();
				}
				if(row["contract_amount"]!=null && row["contract_amount"].ToString()!="")
				{
					model.contract_amount=decimal.Parse(row["contract_amount"].ToString());
				}
				if(row["suretyship_contract"]!=null)
				{
					model.suretyship_contract=row["suretyship_contract"].ToString();
				}
				if(row["mode_payment"]!=null)
				{
					model.mode_payment=row["mode_payment"].ToString();
				}
				if(row["createtime"]!=null && row["createtime"].ToString()!="")
				{
					model.createtime=DateTime.Parse(row["createtime"].ToString());
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
			strSql.Append("select contract,loan_number,targetid,lender_name,lender_username,lender_registerid,lender_id_card,lenders_account_name,lender_bank_account,lender_bank,lender_address,lenders_telephone,lenders_email,lendres_date_contract,borrower_registerid,borrower_name,borrower_username,borrower_id_card,borrower_account_name,borrower_bank_account,borrower_date_contract,borrower_bank,witness_name,witness_address,witness_telephone,witness_agent,witness_admid,witness_username,witness_id_card,witness_date_contract,surety_company_name,guarantee_business_icense,ensure_address,guarantee_legal_representative,guarantor_agent,guarantor_companyid,guarantor_agent_usernqme,guarantor_agent_id_card,guarantor_agent_idate_contract,contract_money,counselling_service,contract_amount,suretyship_contract,mode_payment,createtime ");
			strSql.Append(" FROM Contract_management ");
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
			strSql.Append(" contract,loan_number,targetid,lender_name,lender_username,lender_registerid,lender_id_card,lenders_account_name,lender_bank_account,lender_bank,lender_address,lenders_telephone,lenders_email,lendres_date_contract,borrower_registerid,borrower_name,borrower_username,borrower_id_card,borrower_account_name,borrower_bank_account,borrower_date_contract,borrower_bank,witness_name,witness_address,witness_telephone,witness_agent,witness_admid,witness_username,witness_id_card,witness_date_contract,surety_company_name,guarantee_business_icense,ensure_address,guarantee_legal_representative,guarantor_agent,guarantor_companyid,guarantor_agent_usernqme,guarantor_agent_id_card,guarantor_agent_idate_contract,contract_money,counselling_service,contract_amount,suretyship_contract,mode_payment,createtime ");
			strSql.Append(" FROM Contract_management ");
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
			strSql.Append("select count(1) FROM Contract_management ");
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
				strSql.Append("order by T.contract desc");
			}
			strSql.Append(")AS Row, T.*  from Contract_management T ");
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
			parameters[0].Value = "Contract_management";
			parameters[1].Value = "contract";
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

