using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:bonding_company
	/// </summary>
	public partial class D_bonding_company
	{
		public D_bonding_company()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("companyid", "hx_bonding_company"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int companyid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_bonding_company");
			strSql.Append(" where companyid=@companyid");
			SqlParameter[] parameters = {
					new SqlParameter("@companyid", SqlDbType.Int,4)
			};
			parameters[0].Value = companyid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}

        
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_bonding_company model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_bonding_company(");
            strSql.Append("company_name,registered_capital,Date_incorporation,company_address,company_profile,business_licence,business_certificate,contract_covers,contract_bottom,legal_representative,agent,agent_name,agent_id_card,createtime,Tax_NO,GuarType,company_Url,company_tel )");
			strSql.Append(" values (");
            strSql.Append("@company_name,@registered_capital,@Date_incorporation,@company_address,@company_profile,@business_licence,@business_certificate,@contract_covers,@contract_bottom,@legal_representative,@agent,@agent_name,@agent_id_card,@createtime,@Tax_NO,@GuarType,@company_Url,@company_tel)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@company_name", SqlDbType.NVarChar,100),
					new SqlParameter("@registered_capital", SqlDbType.Decimal,17),
					new SqlParameter("@Date_incorporation", SqlDbType.DateTime),
					new SqlParameter("@company_address", SqlDbType.VarChar,100),
					new SqlParameter("@company_profile", SqlDbType.VarChar,-1),
					new SqlParameter("@business_licence", SqlDbType.VarChar,50),
					new SqlParameter("@business_certificate", SqlDbType.VarChar,50),
					new SqlParameter("@contract_covers", SqlDbType.VarChar,100),
					new SqlParameter("@contract_bottom", SqlDbType.VarChar,100),
					new SqlParameter("@legal_representative", SqlDbType.VarChar,50),
					new SqlParameter("@agent", SqlDbType.VarChar,50),
					new SqlParameter("@agent_name", SqlDbType.VarChar,50),
					new SqlParameter("@agent_id_card", SqlDbType.VarChar,50),
					new SqlParameter("@createtime", SqlDbType.DateTime),
                    new SqlParameter("@Tax_NO", SqlDbType.VarChar,50),                   
                    new SqlParameter("@GuarType", SqlDbType.Int,4),
                    new SqlParameter("@company_Url", SqlDbType.VarChar,350),
                     new SqlParameter("@company_tel", SqlDbType.VarChar,50)};
			parameters[0].Value = model.company_name;
			parameters[1].Value = model.registered_capital;
			parameters[2].Value = model.Date_incorporation;
			parameters[3].Value = model.company_address;
			parameters[4].Value = model.company_profile;
			parameters[5].Value = model.business_licence;
			parameters[6].Value = model.business_certificate;
			parameters[7].Value = model.contract_covers;
			parameters[8].Value = model.contract_bottom;
			parameters[9].Value = model.legal_representative;
			parameters[10].Value = model.agent;
			parameters[11].Value = model.agent_name;
			parameters[12].Value = model.agent_id_card;
			parameters[13].Value = model.createtime;
            parameters[14].Value = model.Tax_NO;
            parameters[15].Value = model.GuarType;
            parameters[16].Value = model.company_Url;
            parameters[17].Value = model.company_tel;
           
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
		public bool Update(M_bonding_company model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_bonding_company set ");
			strSql.Append("company_name=@company_name,");
			strSql.Append("registered_capital=@registered_capital,");
			strSql.Append("Date_incorporation=@Date_incorporation,");
			strSql.Append("company_address=@company_address,");
			strSql.Append("company_profile=@company_profile,");
			strSql.Append("business_licence=@business_licence,");
			strSql.Append("business_certificate=@business_certificate,");
			strSql.Append("contract_covers=@contract_covers,");
			strSql.Append("contract_bottom=@contract_bottom,");
			strSql.Append("legal_representative=@legal_representative,");
			strSql.Append("agent=@agent,");
			strSql.Append("agent_name=@agent_name,");
			strSql.Append("agent_id_card=@agent_id_card,");
            strSql.Append("Tax_NO=@Tax_NO,");
            strSql.Append("GuarType=@GuarType,");
            strSql.Append("company_Url=@company_Url,");
            strSql.Append("company_tel=@company_tel");            
            strSql.Append(" where companyid=@companyid");
			SqlParameter[] parameters = {
					new SqlParameter("@company_name", SqlDbType.NVarChar,100),
					new SqlParameter("@registered_capital", SqlDbType.Decimal,17),
					new SqlParameter("@Date_incorporation", SqlDbType.DateTime),
					new SqlParameter("@company_address", SqlDbType.VarChar,100),
					new SqlParameter("@company_profile", SqlDbType.VarChar,-1),
					new SqlParameter("@business_licence", SqlDbType.VarChar,50),
					new SqlParameter("@business_certificate", SqlDbType.VarChar,50),
					new SqlParameter("@contract_covers", SqlDbType.VarChar,100),
					new SqlParameter("@contract_bottom", SqlDbType.VarChar,100),
					new SqlParameter("@legal_representative", SqlDbType.VarChar,50),
					new SqlParameter("@agent", SqlDbType.VarChar,50),
					new SqlParameter("@agent_name", SqlDbType.VarChar,50),
					new SqlParameter("@agent_id_card", SqlDbType.VarChar,50),	
				    new SqlParameter("@Tax_NO", SqlDbType.VarChar,50),	
                    new SqlParameter("@GuarType", SqlDbType.Int,4),	
                    new SqlParameter("@company_Url", SqlDbType.VarChar,500),
                    new SqlParameter("@company_tel", SqlDbType.VarChar,50),                    
					new SqlParameter("@companyid", SqlDbType.Int,4)};
			parameters[0].Value = model.company_name;
			parameters[1].Value = model.registered_capital;
			parameters[2].Value = model.Date_incorporation;
			parameters[3].Value = model.company_address;
			parameters[4].Value = model.company_profile;
			parameters[5].Value = model.business_licence;
			parameters[6].Value = model.business_certificate;
			parameters[7].Value = model.contract_covers;
			parameters[8].Value = model.contract_bottom;
			parameters[9].Value = model.legal_representative;
			parameters[10].Value = model.agent;
			parameters[11].Value = model.agent_name;
			parameters[12].Value = model.agent_id_card;
            parameters[13].Value = model.Tax_NO;
            parameters[14].Value = model.GuarType;
            parameters[15].Value = model.company_Url;
            parameters[16].Value = model.company_tel; 
		    parameters[17].Value = model.companyid;

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
		public bool Delete(int companyid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_bonding_company ");
			strSql.Append(" where companyid=@companyid");
			SqlParameter[] parameters = {
					new SqlParameter("@companyid", SqlDbType.Int,4)
			};
			parameters[0].Value = companyid;

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
		public bool DeleteList(string companyidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_bonding_company ");
			strSql.Append(" where companyid in ("+companyidlist + ")  ");
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
		public M_bonding_company GetModel(int companyid)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 companyid,company_name,registered_capital,Date_incorporation,company_address,company_profile,business_licence,business_certificate,contract_covers,contract_bottom,legal_representative,agent,agent_name,agent_id_card,createtime,Tax_NO,GuarType,company_Url,company_tel from hx_bonding_company ");
			strSql.Append(" where companyid=@companyid");
			SqlParameter[] parameters = {
					new SqlParameter("@companyid", SqlDbType.Int,4)
			};
			parameters[0].Value = companyid;

			M_bonding_company model=new M_bonding_company();
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
		public M_bonding_company DataRowToModel(DataRow row)
		{
			M_bonding_company model=new M_bonding_company();
			if (row != null)
			{
				if(row["companyid"]!=null && row["companyid"].ToString()!="")
				{
					model.companyid=int.Parse(row["companyid"].ToString());
				}
				if(row["company_name"]!=null)
				{
					model.company_name=row["company_name"].ToString();
				}
				if(row["registered_capital"]!=null && row["registered_capital"].ToString()!="")
				{
					model.registered_capital=decimal.Parse(row["registered_capital"].ToString());
				}
				if(row["Date_incorporation"]!=null && row["Date_incorporation"].ToString()!="")
				{
					model.Date_incorporation=DateTime.Parse(row["Date_incorporation"].ToString());
				}
				if(row["company_address"]!=null)
				{
					model.company_address=row["company_address"].ToString();
				}
				if(row["company_profile"]!=null)
				{
					model.company_profile=row["company_profile"].ToString();
				}
				if(row["business_licence"]!=null)
				{
					model.business_licence=row["business_licence"].ToString();
				}
				if(row["business_certificate"]!=null)
				{
					model.business_certificate=row["business_certificate"].ToString();
				}
				if(row["contract_covers"]!=null)
				{
					model.contract_covers=row["contract_covers"].ToString();
				}
				if(row["contract_bottom"]!=null)
				{
					model.contract_bottom=row["contract_bottom"].ToString();
				}
				if(row["legal_representative"]!=null)
				{
					model.legal_representative=row["legal_representative"].ToString();
				}
				if(row["agent"]!=null)
				{
					model.agent=row["agent"].ToString();
				}
				if(row["agent_name"]!=null)
				{
					model.agent_name=row["agent_name"].ToString();
				}
				if(row["agent_id_card"]!=null)
				{
					model.agent_id_card=row["agent_id_card"].ToString();
				}
				if(row["createtime"]!=null && row["createtime"].ToString()!="")
				{
					model.createtime=DateTime.Parse(row["createtime"].ToString());
				}

                if (row["Tax_NO"] != null && row["Tax_NO"].ToString() != "")
				{
                    model.Tax_NO = row["Tax_NO"].ToString();
				}
                if (row["GuarType"] != null && row["GuarType"].ToString() != "")
				{
                    model.GuarType = int.Parse(row["GuarType"].ToString());
				}
                if (row["company_Url"] != null)
				{
                    model.company_Url = row["company_Url"].ToString();
				}
                if (row["company_tel"] != null)
				{
                    model.company_tel = row["company_tel"].ToString();
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
			strSql.Append("select companyid,company_name,registered_capital,Date_incorporation,company_address,company_profile,business_licence,business_certificate,contract_covers,contract_bottom,legal_representative,agent,agent_name,agent_id_card,createtime ");
			strSql.Append(" FROM hx_bonding_company ");
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
			strSql.Append(" companyid,company_name,registered_capital,Date_incorporation,company_address,company_profile,business_licence,business_certificate,contract_covers,contract_bottom,legal_representative,agent,agent_name,agent_id_card,createtime ");
			strSql.Append(" FROM hx_bonding_company ");
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
			strSql.Append("select count(1) FROM hx_bonding_company ");
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
				strSql.Append("order by T.companyid desc");
			}
			strSql.Append(")AS Row, T.*  from hx_bonding_company T ");
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
			parameters[0].Value = "hx_bonding_company";
			parameters[1].Value = "companyid";
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

