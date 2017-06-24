using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.AdNews;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.DAL.Api
{
    public class AdNewsDal : ImplBase
    {
        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <param name="adtypId">广告类型编号</param>
        /// <param name="top">获取条数</param>
        /// <returns></returns>
        public List<AdEntity> GetWebAd(int adtypId, int top)
        {
            string sql = "select  top " + top + " AdPath,AdLink,AdName from hx_td_Ad where  AdState=0 and AdTypeId=@adtypId order by Adid desc";
            SqlParameter[] parameters = {
                 new SqlParameter("@adtypId",SqlDbType.Int,4)
            };
            parameters[0].Value = adtypId;

            var ds = DbHelper.Query(sql, parameters);
            if (DataSetIsNotNull(ds))
            {
                return InitAdEntityList(ds.Tables[0]);
            }
            return null;
        }


        /// <summary>
        /// 获取网站新闻
        /// </summary>
        /// <param name="newType">新闻类型</param>
        /// <param name="pageIndex">页码</param>
        ///  <param name="pageSize">显示条数</param>
        /// <returns></returns>
        public BasePage<List<NewsEntity>> SelectWebNews(int newType, int pageIndex, int pageSize)
        {

            BasePage<List<NewsEntity>> page = new BasePage<List<NewsEntity>>();
            const string proc = @"pagination";
            var sbWhere = new StringBuilder(" 1>0");

            if (newType > 0)
            {
                sbWhere.Append(" and web_Type_menu_id = " + newType);
            }

            var recordCount = new SqlParameter("@RecordCount", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = DBNull.Value };
            var pageCount = new SqlParameter("@PageCount", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = DBNull.Value };
            IDataParameter[] parameters = {
                    new SqlParameter("@TableName", SqlDbType.NVarChar,255),
                    new SqlParameter("@StrWhere", SqlDbType.NVarChar,1500),
                    new SqlParameter("@PrimaryKey", SqlDbType.NVarChar,255),
                    new SqlParameter("@PageIndex", SqlDbType.Int,4),
                    new SqlParameter("@PageSize", SqlDbType.Int,4),
                    new SqlParameter("@OrderType", SqlDbType.Int,4),
                    new SqlParameter("@StrGetFields", SqlDbType.NVarChar,1000),
                    recordCount,
                pageCount  };
            parameters[0].Value = "V_type_news";
            parameters[1].Value = sbWhere.ToString();
            parameters[2].Value = "newid";
            parameters[3].Value = pageIndex;
            parameters[4].Value = pageSize;
            parameters[5].Value = 1;//desc
            parameters[6].Value = "newid,News_title,createtime";


            var ds = DbHelper.RunProcedure(proc, parameters, "ds");

            if (!string.IsNullOrEmpty(recordCount.Value.ToString()) && recordCount.Value.ToString() != "0")
            {
                if (DataSetIsNotNull(ds))
                {
                    page.recordCount = ConvertHelper.ParseValue(recordCount.Value.ToString(), 0);
                    page.pageCount = ConvertHelper.ParseValue(pageCount.Value.ToString(), 0);
                    var item = InitNewsEntity(ds.Tables[0]);
                    page.rows = item;
                    return page;
                }

            }
            return null;


        }

        /// <summary>
        /// 根据新闻编号获取详细
        /// </summary>
        /// <param name="newId">新闻编号</param>
        /// <returns></returns>
        public NewsEntity SelectNewsContent(int newId)
        {
            string sql = "select newid,news_title,[context],[news_Des] from V_type_news where newid=@newId";
            SqlParameter[] parameters = {
                 new SqlParameter("@newId",SqlDbType.Int,4)
            };
            parameters[0].Value = newId;

            var ds = DbHelper.Query(sql, parameters);
            if (DataSetIsNotNull(ds))
            {
                return InitNewsEntity(ds.Tables[0]).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 获取广告列表(分页)
        /// </summary>
        /// <param name="adtypId">广告类型编号</param>
        /// <param name="pageIndex">页码</param>
        ///  <param name="pageSize">显示条数</param>
        /// <returns></returns>
        public BasePage<List<AdEntity>> SelectWebAd(int adtypId, int pageIndex, int pageSize)
        {

            BasePage<List<AdEntity>> page = new BasePage<List<AdEntity>>();
            const string proc = @"pagination";
            var sbWhere = new StringBuilder(" 1>0");

            if (adtypId > 0)
            {
                sbWhere.Append(" and  AdState=0  and AdTypeId = " + adtypId);
            }

            var recordCount = new SqlParameter("@RecordCount", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = DBNull.Value };
            var pageCount = new SqlParameter("@PageCount", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = DBNull.Value };
            IDataParameter[] parameters = {
                    new SqlParameter("@TableName", SqlDbType.NVarChar,255),
                    new SqlParameter("@StrWhere", SqlDbType.NVarChar,1500),
                    new SqlParameter("@PrimaryKey", SqlDbType.NVarChar,255),
                    new SqlParameter("@OrderField", SqlDbType.NVarChar,255),
                    new SqlParameter("@PageIndex", SqlDbType.Int,4),
                    new SqlParameter("@PageSize", SqlDbType.Int,4),
                    new SqlParameter("@OrderType", SqlDbType.Int,4),
                    new SqlParameter("@StrGetFields", SqlDbType.NVarChar,1000),
                    recordCount,
                pageCount  };

            parameters[0].Value = "hx_td_Ad";//表名
            parameters[1].Value = sbWhere.ToString();//查询条件(注意: 不要加 where)
            parameters[2].Value = "Adid";//主键或者唯一约束字段
            parameters[3].Value = "Adid";
            parameters[4].Value = pageIndex;//
            parameters[5].Value = pageSize;//
            parameters[6].Value = 1;//设置排序类型, 非 0 值则降序
            parameters[7].Value = "Adid,AdName,Adcreatetime,AdPath,AdLink";//需要返回的列

            var ds = DbHelper.RunProcedure(proc, parameters, "ds");

            if (!string.IsNullOrEmpty(recordCount.Value.ToString()) && recordCount.Value.ToString() != "0")
            {
                if (DataSetIsNotNull(ds))
                {
                    page.recordCount = ConvertHelper.ParseValue(recordCount.Value.ToString(), 0);
                    page.pageCount = ConvertHelper.ParseValue(pageCount.Value.ToString(), 0);
                    var item = InitAdEntityList(ds.Tables[0]);
                    page.rows = item;
                    return page;
                }

            }
            return null;
        }

        /// <summary>
        /// 获取广告列表
        /// </summary>
        /// <param name="adtypId">广告类型编号</param>
        /// <param name="top">获取条数</param>
        /// <returns></returns>
        public AdEntity GetWebAdModel(int adtypId, string adName)
        {
            string sql = "select top 1 [Adid],[AdName],[Adcreatetime],[AdState],[AdTypeId],[AdPath],[AdLink] from hx_td_Ad where  AdState=0 and AdTypeId=@adtypId and AdName=@adName";
            SqlParameter[] parameters = {
                new SqlParameter("@adtypId",SqlDbType.Int,4),
                new SqlParameter("@adName",SqlDbType.VarChar,50)
            };
            parameters[0].Value = adtypId;
            parameters[1].Value = adName;

            var ds = DbHelper.Query(sql, parameters);
            if (DataSetIsNotNull(ds))
            {
                return InitAdEntityList(ds.Tables[0]).FirstOrDefault();
            }
            return null;
        }
    }
}
