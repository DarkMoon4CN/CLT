using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Http;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Borrow;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Borrow;
using ChuanglitouP2P.Model.Invest;
using ChuangLiTouOpenApi.Factory;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.BLL.Api;
namespace ChuangLiTouOpenApi.Controllers
{
    /// <summary>
    /// 项目标相关接口
    /// </summary>
    public class BorrowController : BaseApi
    {
        private readonly BorrowLogic _logic;

        public BorrowController(BorrowLogic logic)
        {
            _logic = logic;
        }


        /// <summary>
        /// 获取标的数据列表--解志辉
        /// </summary>
        /// <param name="reqst">RequestBorrow</param>
        /// <returns>ResultInfo&lt;BasePage&lt;List&lt;BorrowEntity&gt;&gt;&gt;.</returns>
        [HttpPost]
        public ResultInfo<BasePage<List<BorrowEntity>>> SelectBorrowList(RequestParam<RequestBorrow> reqst)
        {
            var ri = new ResultInfo<BasePage<List<BorrowEntity>>>("99999");
            try
            {
                int status = -1;
                int pageIndex = ConvertHelper.ParseValue(reqst.body.pageIndex.ToString(), 0);
                int pageSize = ConvertHelper.ParseValue(reqst.body.pageSize.ToString(), 0);
                try
                { 
                    status = ConvertHelper.ParseValue(reqst.body.status.ToString(), 0);
                }
                catch (Exception)
                {
                }

                ri.body = _logic.SelectBorrowList(pageIndex, pageSize, status, "", "");

                if (ri.body == null)
                {
                    ri.code = "1000000010";
                }
                else
                {
                    ri.code = "1";
                }

                //ri.ServerTime = DateTime.Now;
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }


        /// <summary>
        ///  获取特定标的详情信息--解志辉
        /// </summary>
        /// <param name="reqst">targetId</param>
        /// <returns>ResultInfo&lt;BorrowEntity&gt;.</returns>
        [HttpPost]
        public ResultInfo<BorrowEntity> SelectBorrowDetail(RequestParam<RequestBorrowDetail> reqst)
        {
            var ri = new ResultInfo<BorrowEntity>("99999");
            try
            {
                int targetId = ConvertHelper.ParseValue(reqst.body.targetId.ToString(), 0);
                ri.body = _logic.SelectBorrowDetail(targetId);
                if (ri.body == null)
                {
                    ri.code = "1000000010";
                }
                else
                {
                    ri.code = "1";
                }

                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }



        /// <summary>
        /// 确认投资接口--解志辉
        /// </summary>
        /// <param name="reqst">The reqst.</param>
        /// <returns>ResultInfo&lt;System.Int32&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-31 18:01:28
        public ResultInfo<int> SubmitTender(RequestParam<RequestTender> reqst)
        {
            ResultInfo<int> res = new ResultInfo<int>("99999");

            var usrId = ConvertHelper.ParseValue(reqst.body.userId, 0);
            var targetId = ConvertHelper.ParseValue(reqst.body.targetId, 0);
            var bds = reqst.body.bonusIds;
            var investAmount = ConvertHelper.ParseValue(reqst.body.investAmount, 0M);
            //  var code = reqst.body.invitedcode;



            if (usrId <= 0)
            {
                res.code = "1000000015";
                res.message = Settings.Instance.GetErrorMsg(res.code);
                return res;
            }
            if (targetId <= 0)
            {
                res.code = "1000000014";
                res.message = Settings.Instance.GetErrorMsg(res.code);
                return res;
            }


            try
            {
                var ent = _logic.SelectBorrowDetail(targetId);

                //最低可投金额应该从标的记录获取
                if (investAmount < ent.minimum)
                {
                    res.code = "2000000000";
                    res.message = Settings.Instance.GetErrorMsg(res.code);
                    return res;
                }
                else if (investAmount + ent.fundraising_amount > ent.borrowing_balance)
                {//超过可投金额
                    res.code = "2000000002";
                    res.message = Settings.Instance.GetErrorMsg(res.code);
                    return res;
                }


                InvestmentParameters mp = new InvestmentParameters
                {
                    Amount = investAmount,
                    Circle = ConvertHelper.ParseValue(ent.life_of_loan, 0),
                    CircleType = ConvertHelper.ParseValue(ent.unit_day, 0),
                    NominalYearRate = ConvertHelper.ParseValue(ent.annual_interest_rate, 0D),
                    OverheadsRate = 0f,
                    RepaymentMode = ConvertHelper.ParseValue(ent.payment_options, 0),
                    RewardRate = 0f,
                    IsThirtyDayMonth = false,
                    InvestDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),

                    ReleaseDate = DateTime.Parse(ent.release_date.ToString()).ToString("yyyy-MM-dd"),
                    Investmentenddate =
                        DateTime.Parse(DateTime.Parse(ent.repayment_date.ToString()).ToString("yyyy-MM-dd")),
                    Payinterest = ConvertHelper.ParseValue(ent.month_payment_date, 0),
                    InvestObject = 1
                };

                List<InvestmentReceiveRecordInfo> records = InvestCalculator.CalculateReceiveRecord(mp);
                StringBuilder sb = new StringBuilder("");
                if (records != null && records.Any())
                {
                    int i = 1;
                    foreach (var item in records)
                    {
                        //current_investment_period,value_date,interest_payment_date,repayment_amount,interestpayment,Principal,TotalInstallments,interestDay
                        sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7}|", i, item.interestvalue_date, item.NominalReceiveDate, item.Balance, item.Interest, item.Principal, item.TotalInstallments, item.TotalDays);

                        i = i + 1;
                    }
                }

                //(int usrId, int targetId, decimal investAmount, List<int> bds, string code, string ordCode, decimal withoutInterest, string frozenidNo, decimal frozenidAmount, int cPeriod) 


                var resVal = _logic.SubmitTender(usrId, targetId, investAmount, bds, "", Settings.Instance.OrderCode
                    , ((records != null && records.Any()) ? records.Sum(t => t.Interest) : 0M), Settings.Instance.OrderCode
                    , 0M
                    , ((records != null && records.Any()) ? records.Count : 0), sb.ToString());

                if (resVal < 200)
                {

                    switch (resVal)
                    {
                        case -100:
                            {
                                res.code = "2000000001";
                            }
                            break;
                        case -200:
                            {
                                res.code = "2000000002";
                            }
                            break;
                        case -300:
                            {
                                res.code = "2000000003";
                            }
                            break;
                        case -400:
                            {
                                res.code = "2000000004";
                            }
                            break;
                        case -500:
                            {
                                res.code = "2000000007";
                            }
                            break;
                        case -600:
                            {
                                res.code = "2000000006";
                            }
                            break;
                    }
                }
                else
                {
                    res.code = "1";
                    res.body = resVal;
                }

                res.message = Settings.Instance.GetErrorMsg(res.code);
                return res;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst));
                res.code = "500";
                res.message = Settings.Instance.GetErrorMsg(res.code);
                return res;
            }
        }

        /// <summary>
        /// 获取特定标的详细信息.包括:项目详情模块，风险控制模块，担保意见模块 --解志辉
        /// <remark>位置：特定标的详情页下方</remark>
        /// <remark>还有 投资记录 是另外一个接口Invest/SelectInvestRecordsByID。</remark>
        /// </summary>
        /// <param name="reqst">targetId</param>
        /// <returns>ResultInfo&lt;BorrowDetailEntity&gt;.</returns>

        [HttpPost]
        public ResultInfo<BorrowDetailEntity> SelectBorrowInfor(RequestParam<RequestBorrowDetail> reqst)
        {
            var ri = new ResultInfo<BorrowDetailEntity>("99999");
            try
            {
                int targetId = ConvertHelper.ParseValue(reqst.body.targetId.ToString(), 0);
                ri.code = "1";
                ri.body = _logic.SelectBorrowInfor(targetId);
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }

        /// <summary>
        /// 获取新手标数据
        /// </summary>
        /// <param name="reqst">targetId</param>
        /// <returns>ResultInfo&lt;BorrowDetailEntity&gt;.</returns>
        [HttpPost]
        public ResultInfo<BorrowEntity> SelectNewHandBorrow(RequestParam reqst)
        {
            ResultInfo<BorrowEntity> ri = new ResultInfo<BorrowEntity>("99999");
            ri.body = _logic.SelectNewHandBorrow();
            ri.code = "1";
            ri.message = Settings.Instance.GetErrorMsg(ri.code);
            return ri;
        }
    }
}