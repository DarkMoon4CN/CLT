using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
namespace ChuanglitouP2P.DAL
{
	/// <summary>
	/// 数据访问类:repayment_plan
	/// </summary>
	public partial class D_repayment_plan
	{
		public D_repayment_plan()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("repayment_plan_id", "hx_repayment_plan"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int repayment_plan_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hx_repayment_plan");
			strSql.Append(" where repayment_plan_id=@repayment_plan_id");
			SqlParameter[] parameters = {
					new SqlParameter("@repayment_plan_id", SqlDbType.Int,4)
			};
			parameters[0].Value = repayment_plan_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 检查数据是否被录入过 返回真说明记录不存在可以新增
        /// </summary>
        /// <param name="targetid"></param>
        /// <param name="current_period"></param>
        /// <param name="repayment_period"></param>
        /// <returns></returns>
        public bool Exists(int targetid, int current_period, DateTime repayment_period, string BorrUsrCustId)
        {
           bool t = false;
           string sql = "select repayment_plan_id from hx_repayment_plan where targetid=" + targetid.ToString() + " and current_period= " + current_period.ToString() + " and  CONVERT(varchar(10), repayment_period, 23)=CONVERT(varchar(10), '" + repayment_period.ToString("yyyy-MM-dd") + "', 23)  and BorrUsrCustId='" + BorrUsrCustId + "' ";
           DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

           if (dt.Rows.Count > 0)
           {
               t = false;
           }
           else
           {
               t = true;
           }
            
           return t;
        }



        
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(M_repayment_plan model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hx_repayment_plan(");
            strSql.Append("borrower_registerid,targetid,current_period,repayment_period,repayment_type,repayment_amount,actual_amount_repayment,repayment_state,createtime,interestpayment,fees,O_penalty,shall_repayment,BorrUsrCustId)");
			strSql.Append(" values (");
            strSql.Append("@borrower_registerid,@targetid,@current_period,@repayment_period,@repayment_type,@repayment_amount,@actual_amount_repayment,@repayment_state,@createtime,@interestpayment,@fees,@O_penalty,@shall_repayment,@BorrUsrCustId)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@borrower_registerid", SqlDbType.Int,4),
					new SqlParameter("@targetid", SqlDbType.Int,4),
					new SqlParameter("@current_period", SqlDbType.Int,4),
					new SqlParameter("@repayment_period", SqlDbType.DateTime),
					new SqlParameter("@repayment_type", SqlDbType.Int,4),
					new SqlParameter("@repayment_amount", SqlDbType.Decimal,17),
					new SqlParameter("@actual_amount_repayment", SqlDbType.Decimal,17),
					new SqlParameter("@repayment_state", SqlDbType.Int,4),
					new SqlParameter("@createtime", SqlDbType.DateTime),
                    new SqlParameter("@interestpayment", SqlDbType.Decimal),
                    new SqlParameter("@fees", SqlDbType.Decimal),
                    new SqlParameter("@O_penalty", SqlDbType.Decimal),
                    new SqlParameter("@shall_repayment", SqlDbType.Decimal),
                    new SqlParameter("@BorrUsrCustId", SqlDbType.VarChar)};

            
			parameters[0].Value = model.borrower_registerid;
			parameters[1].Value = model.targetid;
			parameters[2].Value = model.current_period;
			parameters[3].Value = model.repayment_period;
			parameters[4].Value = model.repayment_type;
			parameters[5].Value = model.repayment_amount;
			parameters[6].Value = model.actual_amount_repayment;
			parameters[7].Value = model.repayment_state;
			parameters[8].Value = model.createtime;
            parameters[9].Value = model.interestpayment;
            parameters[10].Value = model.fees;
            parameters[11].Value = model.O_penalty;
            parameters[12].Value = model.shall_repayment;
            parameters[13].Value = model.BorrUsrCustId;
            

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
		public bool Update(M_repayment_plan model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hx_repayment_plan set ");
			strSql.Append("borrower_registerid=@borrower_registerid,");
			strSql.Append("targetid=@targetid,");
			strSql.Append("current_period=@current_period,");
			strSql.Append("repayment_period=@repayment_period,");
			strSql.Append("repayment_type=@repayment_type,");
			strSql.Append("repayment_amount=@repayment_amount,");
			strSql.Append("actual_amount_repayment=@actual_amount_repayment,");
			strSql.Append("repayment_state=@repayment_state,");
			strSql.Append("createtime=@createtime,");
            strSql.Append("interestpayment=@interestpayment");
            
			strSql.Append(" where repayment_plan_id=@repayment_plan_id");
			SqlParameter[] parameters = {
					new SqlParameter("@borrower_registerid", SqlDbType.Int,4),
					new SqlParameter("@targetid", SqlDbType.Int,4),
					new SqlParameter("@current_period", SqlDbType.Int,4),
					new SqlParameter("@repayment_period", SqlDbType.DateTime),
					new SqlParameter("@repayment_type", SqlDbType.Int,4),
					new SqlParameter("@repayment_amount", SqlDbType.Decimal,17),
					new SqlParameter("@actual_amount_repayment", SqlDbType.Decimal,17),
					new SqlParameter("@repayment_state", SqlDbType.Int,4),
					new SqlParameter("@createtime", SqlDbType.DateTime),
                    new SqlParameter("@interestpayment", SqlDbType.Decimal),                    
					new SqlParameter("@repayment_plan_id", SqlDbType.Int,4)};
			parameters[0].Value = model.borrower_registerid;
			parameters[1].Value = model.targetid;
			parameters[2].Value = model.current_period;
			parameters[3].Value = model.repayment_period;
			parameters[4].Value = model.repayment_type;
			parameters[5].Value = model.repayment_amount;
			parameters[6].Value = model.actual_amount_repayment;
			parameters[7].Value = model.repayment_state;
			parameters[8].Value = model.createtime;
            parameters[9].Value = model.interestpayment;
			parameters[10].Value = model.repayment_plan_id;

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
		public bool Delete(int repayment_plan_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_repayment_plan ");
			strSql.Append(" where repayment_plan_id=@repayment_plan_id");
			SqlParameter[] parameters = {
					new SqlParameter("@repayment_plan_id", SqlDbType.Int,4)
			};
			parameters[0].Value = repayment_plan_id;

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
		public bool DeleteList(string repayment_plan_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from hx_repayment_plan ");
			strSql.Append(" where repayment_plan_id in ("+repayment_plan_idlist + ")  ");
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
		public M_repayment_plan GetModel(int repayment_plan_id)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 repayment_plan_id,borrower_registerid,targetid,current_period,repayment_period,repayment_type,repayment_amount,actual_amount_repayment,repayment_state,createtime,interestpayment from hx_repayment_plan ");
			strSql.Append(" where repayment_plan_id=@repayment_plan_id");
			SqlParameter[] parameters = {
					new SqlParameter("@repayment_plan_id", SqlDbType.Int,4)
			};
			parameters[0].Value = repayment_plan_id;

			M_repayment_plan model=new M_repayment_plan();
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
		public M_repayment_plan DataRowToModel(DataRow row)
		{
			M_repayment_plan model=new M_repayment_plan();
			if (row != null)
			{
				if(row["repayment_plan_id"]!=null && row["repayment_plan_id"].ToString()!="")
				{
					model.repayment_plan_id=int.Parse(row["repayment_plan_id"].ToString());
				}
				if(row["borrower_registerid"]!=null && row["borrower_registerid"].ToString()!="")
				{
					model.borrower_registerid=int.Parse(row["borrower_registerid"].ToString());
				}
				if(row["targetid"]!=null && row["targetid"].ToString()!="")
				{
					model.targetid=int.Parse(row["targetid"].ToString());
				}
				if(row["current_period"]!=null && row["current_period"].ToString()!="")
				{
					model.current_period=int.Parse(row["current_period"].ToString());
				}
				if(row["repayment_period"]!=null && row["repayment_period"].ToString()!="")
				{
					model.repayment_period=DateTime.Parse(row["repayment_period"].ToString());
				}
				if(row["repayment_type"]!=null && row["repayment_type"].ToString()!="")
				{
					model.repayment_type=int.Parse(row["repayment_type"].ToString());
				}
				if(row["repayment_amount"]!=null && row["repayment_amount"].ToString()!="")
				{
					model.repayment_amount=decimal.Parse(row["repayment_amount"].ToString());
				}
				if(row["actual_amount_repayment"]!=null && row["actual_amount_repayment"].ToString()!="")
				{
					model.actual_amount_repayment=decimal.Parse(row["actual_amount_repayment"].ToString());
				}
				if(row["repayment_state"]!=null && row["repayment_state"].ToString()!="")
				{
					model.repayment_state=int.Parse(row["repayment_state"].ToString());
				}
				if(row["createtime"]!=null && row["createtime"].ToString()!="")
				{
					model.createtime=DateTime.Parse(row["createtime"].ToString());
				}
                if (row["interestpayment"] != null && row["interestpayment"].ToString() != "")
				{
                    model.interestpayment = decimal.Parse(row["interestpayment"].ToString());
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
			strSql.Append("select repayment_plan_id,borrower_registerid,targetid,current_period,repayment_period,repayment_type,repayment_amount,actual_amount_repayment,repayment_state,createtime ");
			strSql.Append(" FROM hx_repayment_plan ");
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
			strSql.Append(" repayment_plan_id,borrower_registerid,targetid,current_period,repayment_period,repayment_type,repayment_amount,actual_amount_repayment,repayment_state,createtime ");
			strSql.Append(" FROM hx_repayment_plan ");
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
			strSql.Append("select count(1) FROM hx_repayment_plan ");
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
				strSql.Append("order by T.repayment_plan_id desc");
			}
			strSql.Append(")AS Row, T.*  from hx_repayment_plan T ");
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
			parameters[0].Value = "hx_repayment_plan";
			parameters[1].Value = "repayment_plan_id";
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

