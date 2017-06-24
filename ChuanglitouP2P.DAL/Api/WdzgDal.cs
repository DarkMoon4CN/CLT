using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ChuangLiTou.Core.Entities;
using ChuangLiTou.Core.Entities.P2Peye;
using ChuangLiTou.Core.Entities.wdzg;
using ChuanglitouP2P.Common.Util;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
namespace ChuanglitouP2P.DAL.Api
{
    public class WdzgDal : ImplBase
    {
        /// <summary>
        /// 获取指定标信息
        /// </summary>
        /// <param name="pId">标的编号</param>
        /// <param name="date">*初审通过的日期，并非添加日期</param>
        /// <returns></returns>
        public dynamic SelectTargetByIdOrDate(int pId, string date)
        {
            var tde = new List<TargetDetailEntity>();

            try
            {
                var sb = new StringBuilder("select * from ViewThirdWangDaiZhongGuo where 1=1 ");

                if (pId > 0)
                {
                    sb.AppendFormat(" and id={0}", pId);
                }
                var vtime = ConvertHelper.ParseValue(date, DateTime.MinValue);
                if ((pId == 0 && vtime == DateTime.MinValue))
                {
                    return null;
                }



                if (!string.IsNullOrEmpty(date)&&vtime>DateTime.MinValue)
                {
                    var f = HttpHelper.ConvertDateTimeInt(vtime);
                    var e = HttpHelper.ConvertDateTimeInt(vtime.AddDays(1));

                    sb.AppendFormat(" and (verify_time>={0} and verify_time<{1})", f, e);
                }


                var ds = DbHelperSQL.GET_DataSet_List(sb.ToString());

                if (ds != null)
                {


                    tde = InitTargetList(ds.Tables[0]);



                }
                return tde;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                return "";
            }
        }

        /// <summary>
        /// 获取投资信息
        /// </summary>
        /// <param name="targetId">标的id</param>
        /// <returns></returns>
        private List<InvestRecordEntity> GetTargetInvestRecord(int targetId)
        {
            try
            {
                var sql =
              string.Format(
                  "SELECT bid_records_id AS tender_id,username,investment_amount AS account,targetid AS  borrow_id,invest_time AS tender_time,invest_state AS [state]  FROM ViewInvestRecord WHERE targetid={0}", targetId);

                var ds = DbHelper.Query(sql);

                if (ds != null)
                {
                    return InvestRecordEntity(ds.Tables[0]);
                }
                return null;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                return null;
            }
        }




        /// <summary>
        /// 得到一个对象实体标信息
        /// </summary>
        public List<TargetDetailEntity> InitTargetList(DataTable dt)
        {
            var lst = new List<TargetDetailEntity>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var model = new TargetDetailEntity();

                if (row != null)
                {
                    if (row["url"] != null)
                    {
                        model.url = row["url"].ToString();
                    }
                    if (row["id"] != null && row["id"].ToString() != "")
                    {
                        model.id = int.Parse(row["id"].ToString());
                    }
                    if (row["title"] != null)
                    {
                        model.title = row["title"].ToString();
                    }
                    if (row["account"] != null && row["account"].ToString() != "")
                    {
                        model.account = decimal.Parse(row["account"].ToString());
                    }
                    if (row["apr"] != null && row["apr"].ToString() != "")
                    {
                        model.apr = decimal.Parse(row["apr"].ToString());
                    }
                    if (row["limit"] != null && row["limit"].ToString() != "")
                    {
                        model.limit = int.Parse(row["limit"].ToString());
                    }
                    if (row["limit_type"] != null && row["limit_type"].ToString() != "")
                    {
                        model.limit_type = int.Parse(row["limit_type"].ToString());
                    }
                    if (row["way"] != null && row["way"].ToString() != "")
                    {
                        model.way = int.Parse(row["way"].ToString());
                    }
                    if (row["account_yes"] != null && row["account_yes"].ToString() != "")
                    {
                        model.account_yes = decimal.Parse(row["account_yes"].ToString());
                    }
                    if (row["account_min"] != null && row["account_min"].ToString() != "")
                    {
                        model.account_min = decimal.Parse(row["account_min"].ToString());
                    }
                    if (row["account_max"] != null && row["account_max"].ToString() != "")
                    {
                        model.account_max = decimal.Parse(row["account_max"].ToString());
                    }
                    if (row["username"] != null)
                    {
                        model.username = row["username"].ToString();
                    }
                    if (row["add_time"] != null && row["add_time"].ToString() != "")
                    {
                        model.add_time = int.Parse(row["add_time"].ToString());
                    }
                    if (row["verify_time"] != null)
                    {
                        model.verify_time = row["verify_time"].ToString();
                    }
                    if (row["success_time"] != null)
                    {
                        model.success_time = row["success_time"].ToString();
                    }
                    if (row["type"] != null)
                    {
                        model.type = row["type"].ToString();
                    }
                    if (row["status"] != null && row["status"].ToString() != "")
                    {
                        model.status = int.Parse(row["status"].ToString());
                    }
                    if (row["reward"] != null)
                    {
                        model.reward = row["reward"].ToString();
                    }
                    if (row["is_diya"] != null && row["is_diya"].ToString() != "")
                    {
                        model.is_diya = int.Parse(row["is_diya"].ToString());
                    }
                    if (row["is_danbao"] != null && row["is_danbao"].ToString() != "")
                    {
                        model.is_danbao = int.Parse(row["is_danbao"].ToString());
                    }
                    if (row["is_mima"] != null && row["is_mima"].ToString() != "")
                    {
                        model.is_mima = int.Parse(row["is_mima"].ToString());
                    }
                    if (row["is_miao"] != null && row["is_miao"].ToString() != "")
                    {
                        model.is_miao = int.Parse(row["is_miao"].ToString());
                    }
                    if (row["is_zhuan"] != null && row["is_zhuan"].ToString() != "")
                    {
                        model.is_zhuan = int.Parse(row["is_zhuan"].ToString());
                    }
                    model.tender = GetTargetInvestRecord(model.id);

                    lst.Add(model);
                }

            }
            return lst;
        }



        /// <summary>
        /// 投资记录实体列表
        /// </summary>
        public List<InvestRecordEntity> InvestRecordEntity(DataTable dt)
        {

            var entityList = new List<InvestRecordEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new InvestRecordEntity();


                        var row=dt.Rows[n];
                        if (ContainsColumn(dt.Columns, "tender_id", row))
                        {
                            entity.tender_id = dt.Rows[n]["tender_id"].ToString();
                        }


                        if (ContainsColumn(dt.Columns, "username", row))
                        {
                            entity.username = dt.Rows[n]["username"].ToString().Substring(0, 4) + "*****";

                        }

                        if (ContainsColumn(dt.Columns, "account", row))
                        {

                            entity.account =
                                ConvertHelper.ParseValue(dt.Rows[n]["account"], 0M);
                        }

                        //系统状态：1 成功  2 失败  3流标返还 
                        //第三方状态：0 待审核 1 待回款 2 成功回款 3 坏账 4债权转让 5投标失败

                        if (ContainsColumn(dt.Columns, "state", row))
                        {
                            switch (dt.Rows[n]["state"].ToString())
                            {
                                case "1":
                                    entity.status = "2";
                                    break;
                                case "2":
                                    entity.status = "5";
                                    break;
                                case "3":
                                    entity.status = "2";
                                    break;
                                default:
                                    entity.status = "0";
                                    break;
                            }

                        }

                        if (ContainsColumn(dt.Columns, "borrow_id", row))
                        {
                            entity.borrow_id = dt.Rows[n]["borrow_id"].ToString();

                        }


                        if (ContainsColumn(dt.Columns, "tender_time", row))
                        {
                            entity.tender_time = dt.Rows[n]["tender_time"].ToString();

                        }


                        entityList.Add(entity);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(" public List<InvestRecordEntity> InvestRecordEntity(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }
    }
}
