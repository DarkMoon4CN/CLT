using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using ChuangLiTou.Core.Entities;
using ChuangLiTou.Core.Entities.P2Peye;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuangLiTou.Core.Entities.Response.SmsEmail;
using ChuangLiTou.Core.Entities.Response.Borrow;
using ChuanglitouP2P.Common.Util;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.DAL.Api
{
    /// <summary>
    /// Class BorrowDal.
    /// </summary>
    public class BorrowDal : ImplBase
    {
        /// <summary>
        /// 标数据
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页数据条数</param>
        /// <param name="status">标状态</param>
        /// <param name="timeFrom">开始时间</param>
        /// <param name="timeTo">结束时间</param>
        /// <returns>BasePage&lt;List&lt;BorrowEntity&gt;&gt;.</returns>
        public BasePage<List<BorrowEntity>> SelectBorrowList(int pageIndex, int pageSize, int status, string timeFrom, string timeTo)
        {
            BasePage<List<BorrowEntity>> page = new BasePage<List<BorrowEntity>>();
            const string proc = @"procPagination";
            var sbWhere = new StringBuilder(" 1>0 ");// or (project_type_id=6 and tender_state>2 ) ");
            if (status == 0)
            {
                sbWhere.Append(" and tender_state in(2,3,4,5) ");
            }

            if (status == 1)
            {
                // sbWhere.Append(" and ( tender_state=3 or tender_state=4 )");
                //tender_state  2 复审通过(开标上线)     3 满标 (还款中)     4放款 (还款中)   5 已还清   
                sbWhere.Append(" and ( tender_state=5)");
            }

            if (!string.IsNullOrEmpty(timeFrom) && !string.IsNullOrEmpty(timeTo))
            {
                sbWhere.AppendFormat(" and (CreatedOn BETWEEN '{0}' AND '{1}') ", timeFrom, timeTo);
            }

            //@tblName NVarChar(255), --表名
            //@strGetFields NVarChar(1000), --需要返回的列
            //@fldName NVarChar(255), --排序的字段名
            //@prKey NVarChar(255), --主键
            //@pageSize   int, --页尺寸
            //@pageIndex  int, --页码
            //@strWhere  NVarChar(1500), --查询条件(注意: 不要加where)
            //@sort NVarChar(255), --排序的方法
            //@recordCount int OUTPUT--总记录数(存储过程输出参数)
            sbWhere.AppendFormat(" and targetid not in (select targetid from ViewTargetWithRecords where project_type_id=6 and tender_state=2) ");//此处需要优化为   排除 not in
            var recordCount = new SqlParameter("@recordCount", SqlDbType.Int) { Direction = ParameterDirection.Output, Value = DBNull.Value };
            IDataParameter[] parameters = {
                    new SqlParameter("@tblName", SqlDbType.NVarChar,255),
                    new SqlParameter("@strWhere", SqlDbType.NVarChar,1500),
                    new SqlParameter("@prKey", SqlDbType.NVarChar,255),
                    new SqlParameter("@pageIndex", SqlDbType.Int,4),
                    new SqlParameter("@pageSize", SqlDbType.Int,4),
                    new SqlParameter("@fldName", SqlDbType.NVarChar,255),
                    new SqlParameter("@sort", SqlDbType.NVarChar,4),
                    new SqlParameter("@strGetFields", SqlDbType.NVarChar,1000),
                    recordCount  };
            parameters[0].Value = "ViewTargetWithRecords";
            parameters[1].Value = sbWhere.ToString();
            parameters[2].Value = "targetid";
            parameters[3].Value = pageIndex;
            parameters[4].Value = pageSize;
            parameters[5].Value = "tender_state,indexorder desc,targetid";
            parameters[6].Value = "desc";//asc
            parameters[7].Value = "targetid,borrowing_title,borrowing_balance,CreatedOn,life_of_loan,unit_day,annual_interest_rate,repayment_date,month_payment_date,tender_state,fundraising_amount,end_time,start_time,sys_time,payment_options,minimum,maxmum,project_type_id";

            var ds = DbHelper.RunProcedure(proc, parameters, "ds");
            if (!string.IsNullOrEmpty(recordCount.Value.ToString()) && recordCount.Value.ToString() != "0")
            {
                if (DataSetIsNotNull(ds))
                {
                    page.recordCount = ConvertHelper.ParseValue(recordCount.Value.ToString(), 0);
                    page.pageCount = (page.recordCount + pageSize - 1) / pageSize;
                    var item = InitEntity(ds.Tables[0]);
                    page.rows = item;
                    return page;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取标详细数据
        /// </summary>
        /// <param name="targetId">标ID</param>
        /// <returns>BorrowEntity.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public BorrowEntity SelectBorrowDetail(int targetId)
        {
            var sql = string.Format("SELECT * FROM dbo.ViewTargetWithRecords WHERE targetid={0}", targetId);

            var ds = DbHelper.Query(sql);
            if (DataSetIsNotNull(ds))
            {
                return InitEntity(ds.Tables[0]).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 获取借款标详细信息接口-解志辉
        /// </summary>
        /// <param name="targetId">The target identifier.</param>
        /// <returns>BorrowDetailEntity.</returns>
        public BorrowDetailEntity SelectBorrowInfor(int targetId)
        {
            var sql = string.Format("SELECT * FROM dbo.hx_borrowing_target_detailed WHERE targetid={0}", targetId);

            var ds = DbHelper.Query(sql);
            if (DataSetIsNotNull(ds))
            {
                return InitBorrowInforEntity(ds.Tables[0]).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 确认投资接口
        /// </summary>
        /// <param name="usrId">投资者ID</param>
        /// <param name="targetId">标Id</param>
        /// <param name="investAmount">投资金额</param>
        /// <param name="ids">优惠券Id集合</param>
        /// <param name="code">邀请码</param>
        /// <param name="ordCode">订单号</param>
        /// <param name="withoutInterest">投资到期可获取总收益</param>
        /// <param name="frozenidNo">冻结号</param>
        /// <param name="frozenidAmount">冻结金额=投资钱-优惠券钱</param>
        /// <param name="cPeriod">分期总数</param>
        /// <param name="incomeStatementStr">投资人利息收入表</param>
        /// <returns>System.Int32.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-06-02 09:52:51
        /// 创 建 者：解志辉
        /// 创建日期：2016-06-01 17:07:43
        public int SubmitTender(int usrId, int targetId, decimal investAmount, string ids, string code, string ordCode, decimal withoutInterest, string frozenidNo, decimal frozenidAmount, int cPeriod, string incomeStatementStr)
        {
            const string proc = @"prSubmitTender";


            LoggerHelper.Error("--SubmitTender:usrId=" + usrId.ToString() + ",targetId=" + targetId.ToString() + ",investAmount=" + investAmount.ToString() + ",ids=" + ids + ",code=" + code + ",ordCode=" + ordCode + ",withoutInterest=" + withoutInterest.ToString() + ",frozenidNo=" + frozenidNo + ",frozenidAmount=" + frozenidAmount.ToString() + ",cPeriod=" + cPeriod.ToString() + ",incomeStatementStr=" + incomeStatementStr + ";");

            //@usrId INT,
            //@targetId INT,
            //@investAmount decimal(18, 2),--投资金额
            //@ids NVARCHAR(500),--优惠券ids
            //@cPeriod INT,--
            //@code NVARCHAR(500),--邀请码
            //@ordCode DECIMAL(18),--订单号
            //@withoutInterest DECIMAL(18, 2)--投资到期可获取总收益
            //,@frozenidNo nvarchar(500)--冻结号
            //,@frozenidAmount DECIMAL(18,2)--冻结金额
            //LoggerHelper.Error("------------withoutInterest" + withoutInterest + "--------------");
            //LoggerHelper.Error("------------frozenidAmount" + frozenidAmount + "--------------");
            //LoggerHelper.Error("------------investAmount" + investAmount + "--------------");
            //LoggerHelper.Error("------------incomeStatementStr" + incomeStatementStr + "--------------");
            IDataParameter[] parameters = {
                    new SqlParameter("@usrId", SqlDbType.Int,4),
                    new SqlParameter("@targetId", SqlDbType.Int,4),
                    new SqlParameter("@investAmount", SqlDbType.Decimal,17),
                    new SqlParameter("@ids", SqlDbType.NVarChar,500),
                    new SqlParameter("@cPeriod", SqlDbType.Int,4),
                    new SqlParameter("@code", SqlDbType.NVarChar,50),
                    new SqlParameter("@ordCode", SqlDbType.Decimal,20),
                    new SqlParameter("@withoutInterest", SqlDbType.Decimal,18),
                    new SqlParameter("@frozenidNo", SqlDbType.NVarChar,50),
                    new SqlParameter("@frozenidAmount", SqlDbType.Decimal,18),
                    new SqlParameter("@incomeStatementStr", SqlDbType.NVarChar,4000)
                                          };
            parameters[0].Value = usrId;
            parameters[1].Value = targetId;
            parameters[2].Value = investAmount;
            parameters[3].Value = ids;
            parameters[4].Value = cPeriod;
            parameters[5].Value = code;
            parameters[6].Value = ordCode;
            parameters[7].Value = withoutInterest;
            parameters[8].Value = frozenidNo;
            parameters[9].Value = frozenidAmount;
            parameters[10].Value = incomeStatementStr;

            return DbHelper.RunProcedure(proc, parameters);
        }

        public BorrowEntity GetNewHand()
        {
            var result = DbHelper.Query(" select top 1 * from hx_borrowing_target where project_type_id=6 and tender_state=2 and start_time>'2016-12-01 00:00:00' order by sys_time desc ");
            if (result == null || result.Tables.Count < 1 || result.Tables[0].Rows.Count < 1)
                return null;
            return InitEntity(result.Tables[0]).FirstOrDefault();
        }

        protected static List<BorrowGuarantorPictureEntity> SelectBorrowPictures(int targetId)
        {
            var sql = string.Format("SELECT type_picture,picture_path FROM dbo.hx_borrower_guarantor_picture WHERE targetid={0}", targetId);
            var ds = DbHelper.Query(sql);
            if (DataSetIsNotNull(ds))
            {
                return InitBorrowPicturesEntity(ds.Tables[0]);
            }
            return null;
        }

        /// <summary>
        /// 封装标图片信息结果集
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected static List<BorrowGuarantorPictureEntity> InitBorrowPicturesEntity(DataTable dt)
        {
            var entityList = new List<BorrowGuarantorPictureEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new BorrowGuarantorPictureEntity();
                        var row = dt.Rows[n];
                        if (ContainsColumn(dt.Columns, "targetid", row))
                        {
                            entity.targetid = ConvertHelper.ParseValue(dt.Rows[n]["targetid"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "borrower_guarantor_picture_id", row))
                        {
                            entity.borrower_guarantor_picture_id = ConvertHelper.ParseValue(dt.Rows[n]["borrower_guarantor_picture_id"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "borrower_registerid", row))
                        {
                            entity.borrower_registerid = ConvertHelper.ParseValue(dt.Rows[n]["borrower_registerid"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "targetid", row))
                        {
                            entity.targetid = ConvertHelper.ParseValue(dt.Rows[n]["targetid"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "type_picture", row))
                        {
                            entity.type_picture = ConvertHelper.ParseValue(dt.Rows[n]["type_picture"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "picture_path", row))
                        {
                            entity.picture_path = dt.Rows[n]["picture_path"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "picture_name", row))
                        {
                            entity.picture_name = dt.Rows[n]["picture_name"].ToString();
                        }
                        if (ContainsColumn(dt.Columns, "uploadtime", row))
                        {
                            entity.uploadtime = ConvertHelper.ParseValue(dt.Rows[n]["uploadtime"].ToString(), DateTime.MinValue);
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
                LoggerHelper.Error("protected static List<BorrowGuarantorPictureEntity> InitMaticsoft.Model.hx_borrower_guarantor_picture(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }

        /// <summary>
        /// 封装标基本信息结果集
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected static List<BorrowEntity> InitEntity(DataTable dt)
        {
            var entityList = new List<BorrowEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new BorrowEntity();
                        var row = dt.Rows[n];

                        if (ContainsColumn(dt.Columns, "targetid", row))
                        {
                            entity.targetId = ConvertHelper.ParseValue(dt.Rows[n]["targetid"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "borrowing_title", row))
                        {
                            entity.borrowing_title = dt.Rows[n]["borrowing_title"].ToString();
                        }

                        if (ContainsColumn(dt.Columns, "borrower_registerid", row))
                        {
                            entity.borrower_registerid = ConvertHelper.ParseValue(dt.Rows[n]["borrower_registerid"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "username", row))
                        {
                            entity.userName = dt.Rows[n]["username"].ToString().Substring(0, 4) + "*****";
                        }

                        if (ContainsColumn(dt.Columns, "tender_state", row))
                        {
                            entity.tender_state = ConvertHelper.ParseValue(dt.Rows[n]["tender_state"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "project_type_id", row))
                        {
                            entity.project_type_id = ConvertHelper.ParseValue(dt.Rows[n]["project_type_id"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "borrowing_balance", row))
                        {
                            entity.borrowing_balance = ConvertHelper.ParseValue(dt.Rows[n]["borrowing_balance"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "annual_interest_rate", row))
                        {
                            entity.annual_interest_rate = ConvertHelper.ParseValue(dt.Rows[n]["annual_interest_rate"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "life_of_loan", row))
                        {
                            entity.life_of_loan = ConvertHelper.ParseValue(dt.Rows[n]["life_of_loan"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "payment_options", row))
                        {
                            entity.payment_options = ConvertHelper.ParseValue(dt.Rows[n]["payment_options"], 0);
                        }

                        if (ContainsColumn(dt.Columns, "fundraising_amount", row))
                        {
                            entity.fundraising_amount = ConvertHelper.ParseValue(dt.Rows[n]["fundraising_amount"], 0M);
                        }

                        if (ContainsColumn(dt.Columns, "month_payment_date", row))
                        {
                            entity.month_payment_date = ConvertHelper.ParseValue(dt.Rows[n]["month_payment_date"], 0);
                        }

                        if (ContainsColumn(dt.Columns, "H_Repayment_Amt", row))
                        {
                            entity.H_Repayment_Amt = ConvertHelper.ParseValue(dt.Rows[n]["H_Repayment_Amt"], 0M);

                        }
                        if (ContainsColumn(dt.Columns, "invest_num", row))
                        {
                            entity.investCount = ConvertHelper.ParseValue(dt.Rows[n]["invest_num"], 0);
                        }

                        if (ContainsColumn(dt.Columns, "repayment_date", row))
                        {
                            entity.repayment_date = ConvertHelper.ParseValue(dt.Rows[n]["repayment_date"], DateTime.Now);
                        }

                        if (ContainsColumn(dt.Columns, "month_payment_date", row))
                        {
                            entity.month_payment_date = ConvertHelper.ParseValue(dt.Rows[n]["month_payment_date"], 0);
                        }

                        if (ContainsColumn(dt.Columns, "CreatedOn", row))
                        {
                            entity.CreatedOn = ConvertHelper.ParseValue(dt.Rows[n]["CreatedOn"], DateTime.Now);
                        }

                        if (ContainsColumn(dt.Columns, "start_time", row))
                        {
                            entity.start_time = ConvertHelper.ParseValue(dt.Rows[n]["start_time"], DateTime.Now);
                        }

                        if (ContainsColumn(dt.Columns, "end_time", row))
                        {
                            entity.end_time = ConvertHelper.ParseValue(dt.Rows[n]["end_time"], DateTime.Now);
                        }

                        if (ContainsColumn(dt.Columns, "release_date", row))
                        {
                            entity.release_date = ConvertHelper.ParseValue(dt.Rows[n]["release_date"], DateTime.Now);
                        }

                        if (ContainsColumn(dt.Columns, "unit_day", row))
                        {
                            entity.unit_day = ConvertHelper.ParseValue(dt.Rows[n]["unit_day"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "minimum", row))
                        {
                            entity.minimum = ConvertHelper.ParseValue(dt.Rows[n]["minimum"].ToString(), 0M);
                        }

                        if (ContainsColumn(dt.Columns, "maxmum", row))
                        {
                            entity.maxmum = ConvertHelper.ParseValue(dt.Rows[n]["maxmum"].ToString(), 0M);
                            //if (entity.maxmum == 0M) entity.maxmum = 50000;
                        }

                        if (ContainsColumn(dt.Columns, "sys_time", row))
                        {
                            entity.sys_time = ConvertHelper.ParseValue(dt.Rows[n]["sys_time"], DateTime.Now);
                        }

                        //FLAG: 当 标的为开标上线时，通过判定 标的剩余值来更新标状态为满标状态。
                        if (entity.tender_state == 2)
                        {
                            if (entity.borrowing_balance - entity.fundraising_amount <= 0)
                            {
                                entity.tender_state = 3;
                                entity.borrowing_balance = entity.fundraising_amount;//避免APP端出现 投资负数的问题
                            }
                        }
                        else if (entity.tender_state > 2)
                        {
                            entity.borrowing_balance = entity.fundraising_amount;//避免APP端出现 状态为已放款，但是可投金额不为0的问题
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
                LoggerHelper.Error(" public List<BorrowEntity> InitEntity(DataTable dt) throw exception:", ex);
            }

            return entityList;
        }
        /// <summary>
        /// 封装标详细信息结果集.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>List&lt;BorrowDetailEntity&gt;.</returns>
        protected static List<BorrowDetailEntity> InitBorrowInforEntity(DataTable dt)
        {
            var entityList = new List<BorrowDetailEntity>();
            try
            {
                int rowsCount = dt.Rows.Count;
                if (rowsCount > 0)
                {
                    for (int n = 0; n < rowsCount; n++)
                    {
                        var entity = new BorrowDetailEntity();
                        var row = dt.Rows[n];
                        if (ContainsColumn(dt.Columns, "targetid", row))
                        {
                            entity.targetid = ConvertHelper.ParseValue(dt.Rows[n]["targetid"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "target_detailed_id", row))
                        {
                            entity.target_detailed_id = ConvertHelper.ParseValue(dt.Rows[n]["target_detailed_id"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "risk_control_measures", row))
                        {
                            entity.risk_control_measures = HttpHelper.RemoveHtml(dt.Rows[n]["risk_control_measures"].ToString());
                        }
                        if (ContainsColumn(dt.Columns, "createtime", row))
                        {
                            entity.createtime = ConvertHelper.ParseValue(dt.Rows[n]["createtime"].ToString(), DateTime.MinValue);
                        }
                        if (ContainsColumn(dt.Columns, "borrower_registerid", row))
                        {
                            entity.borrower_registerid = ConvertHelper.ParseValue(dt.Rows[n]["borrower_registerid"].ToString(), 0);
                        }
                        if (ContainsColumn(dt.Columns, "targetid", row))
                        {
                            entity.targetid = ConvertHelper.ParseValue(dt.Rows[n]["targetid"].ToString(), 0);
                        }

                        if (ContainsColumn(dt.Columns, "item_details", row))
                        {
                            entity.item_details = HttpHelper.RemoveHtml(dt.Rows[n]["item_details"].ToString());
                        }

                        if (ContainsColumn(dt.Columns, "borrower_circumstances", row))
                        {
                            entity.borrower_circumstances = HttpHelper.RemoveHtml(dt.Rows[n]["borrower_circumstances"].ToString());
                        }

                        if (ContainsColumn(dt.Columns, "borrower_base_material", row))
                        {
                            entity.borrower_base_material = HttpHelper.RemoveHtml(dt.Rows[n]["borrower_base_material"].ToString());
                        }

                        if (ContainsColumn(dt.Columns, "use_funds", row))
                        {
                            entity.use_funds = HttpHelper.RemoveHtml(dt.Rows[n]["use_funds"].ToString());
                        }

                        if (ContainsColumn(dt.Columns, "independent_advice", row))
                        {
                            entity.independent_advice = HttpHelper.RemoveHtml(dt.Rows[n]["independent_advice"].ToString());
                        }

                        if (ContainsColumn(dt.Columns, "guarantee_agency_views", row))
                        {
                            entity.guarantee_agency_views = HttpHelper.RemoveHtml(dt.Rows[n]["guarantee_agency_views"].ToString());
                        }
                        entity.pictures = SelectBorrowPictures(entity.targetid);
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
                LoggerHelper.Error("protected static List<Maticsoft.Model.hx_borrowing_target_detailed> InitMaticsoft.Model.hx_borrowing_target_detailed(DataTable dt) throw exception:", ex);
            }
            return entityList;
        }
    }
}
