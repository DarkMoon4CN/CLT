using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ChuangLiTou.Core.Entities;
using ChuangLiTou.Core.Entities.ProEnt;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Response;
using ChuanglitouP2P.Common;

namespace ChuanglitouP2P.DAL.Api
{
    public class OpenApiAuthorizationDal
    {
        /// <summary>
        /// 获取应用授权实体.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns></returns>
        /// 创建者：解志辉
        public AuthEntity SelectAppAuthorInforById(int id)
        {
            var strSql = new StringBuilder();
            strSql.Append("select  top 1 AppId,AppName,AppSecret,AppSafeCode,AppServerIps,IsDelete,AppStatus,CreatedOn,UpdatedOn from ApplicationAuthorization ");
            strSql.Append(" where AppId=@AppId and IsDelete=0 and AppStatus=0");
            SqlParameter[] parameters = {
					new SqlParameter("@AppId", SqlDbType.Int,4)
			};
            parameters[0].Value = id;
             
            DataSet ds = DbHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="row">row.</param>
        /// 创建者：解志辉
        private AuthEntity DataRowToModel(DataRow row)
        {
            var model = new AuthEntity();
            if (row != null)
            {
                if (row["AppId"] != null && row["AppId"].ToString() != "")
                {
                    model.appId = int.Parse(row["AppId"].ToString());
                }
                if (row["AppName"] != null)
                {
                    model.appName = row["AppName"].ToString();
                }
                if (row["AppSecret"] != null)
                {
                    model.appSecret = row["AppSecret"].ToString();
                }
                if (row["AppSafeCode"] != null)
                {
                    model.appSafeCode = row["AppSafeCode"].ToString();
                }
                if (row["AppServerIps"] != null)
                {
                    model.appServerIps = row["AppServerIps"].ToString();
                }
                if (row["IsDelete"] != null && row["IsDelete"].ToString() != "")
                {
                    model.isDelete = int.Parse(row["IsDelete"].ToString());
                }
                if (row["AppStatus"] != null && row["AppStatus"].ToString() != "")
                {
                    model.appStatus = int.Parse(row["AppStatus"].ToString());
                }
                if (row["CreatedOn"] != null && row["CreatedOn"].ToString() != "")
                {
                    model.createdOn = DateTime.Parse(row["CreatedOn"].ToString());
                }
                if (row["UpdatedOn"] != null && row["UpdatedOn"].ToString() != "")
                {
                    model.updatedOn = DateTime.Parse(row["UpdatedOn"].ToString());
                }
            }
            return model;
        }
    }
}
