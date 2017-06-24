using System;
using System.Collections.Generic;
using System.Web.Http;
using ChuangLiTou.Core.Entities.ProEnt;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Capital;
using ChuangLiTou.Core.Entities.Request.Member;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Capital;
using ChuangLiTou.Core.Entities.Response.Invest;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuangLiTouOpenApi.Factory;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.BLL.Api;
using ChuangLiTou.Core.Entities.Request.Recharge;
using System.Linq;
namespace ChuangLiTouOpenApi.Controllers
{
    /// <summary>
    /// 资金相关接口
    /// </summary>
    public class CapitalController : BaseApi
    {
        private readonly CapitalLogic _logic;

        public CapitalController(CapitalLogic logic)
        {
            _logic = logic;
        }

        /// <summary>
        /// 获取特定用户资金管理,包括：累计投资、应收利息  --解志辉
        /// </summary>
        /// <param name="reqst">{"userId":"用户ID"}</param>
        /// <returns>ResultInfo&lt;MemberEntity&gt;.</returns>
        [HttpPost]
        public ResultInfo<MemberEntity> FundsManage(RequestParam<RequestMemberDetail> reqst)
        {
            var ri = new ResultInfo<MemberEntity>("99999");
            try
            {
                var userId = reqst.body.userId.ToString();

                if (string.IsNullOrEmpty(userId))
                {
                    ri.code = "1000000000";
                }
                else
                {
                    ri.code = "1";
                    var ent = _logic.SelectMemberFundsInformation(ConvertHelper.ParseValue(userId, 0));
                    if (ent == null)
                    {
                        ri.code = "1000000015";
                    }
                    ri.body = ent;
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
        /// 查询用户资金明细--解志辉
        /// </summary>
        /// <param name="reqst">The reqst.</param>
        /// <returns>ResultInfo&lt;MemberEntity&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-25 16:40:17
        [HttpPost]
        public ResultInfo<BasePage<List<CapitalAccountWater>>> FundDetail(RequestParam<RequestCapital> reqst)
        {
            var ri = new ResultInfo<BasePage<List<CapitalAccountWater>>>("99999");
            try
            {


                var userId = reqst.body.userId.ToString();

                if (string.IsNullOrEmpty(userId))
                {
                    ri.code = "1000000000";
                }
                else
                {
                    ri.code = "1";
                    var ent = _logic.SelectFundWater(reqst.body.pageIndex, reqst.body.pageSize, reqst.body.userId);
                    ri.body = ent;
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

        ///// <summary>
        ///// 累计投资接口--解志辉
        ///// </summary>
        ///// <param name="reqst">The reqst.</param>
        ///// <returns>ResultInfo&lt;MemberEntity&gt;.</returns>
        /////  创 建 者：解志辉
        /////  创建日期：2016-05-25 16:41:12
        //[HttpPost]
        //public ResultInfo<BasePage<List<InvestEntity>>> TotalInvest(RequestParam<RequestCapital> reqst)
        //{
        //    var ri = new ResultInfo<BasePage<List<InvestEntity>>>("99999");
        //    try
        //    {
        //        var userId = reqst.body.userId.ToString();

        //        if (string.IsNullOrEmpty(userId))
        //        {
        //            ri.code = "1000000000";
        //        }
        //        else
        //        {
        //            ri.code = "1";
        //            var ent = _logic.SelectTotalInvest(reqst.body.pageIndex, reqst.body.pageSize, reqst.body.userId);

        //            ri.body = ent;
        //        }
        //        ri.message = Settings.Instance.GetErrorMsg(ri.code);
        //        return ri;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.Error(ex.ToString());
        //        LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
        //        ri.message = Settings.Instance.GetErrorMsg(ri.code);
        //        return ri;
        //    }
        //}

        /// <summary>
        /// 获取特定用户提现记录列表--解志辉
        /// </summary>
        /// <param name="reqst">The reqst.</param>
        /// <returns>ResultInfo&lt;MemberEntity&gt;.</returns>
        [HttpPost]
        public ResultInfo<BasePage<List<PresentEntity>>> PresentRecord(RequestParam<RequestCapital> reqst)
        {
            var ri = new ResultInfo<BasePage<List<PresentEntity>>>("99999");
            try
            {
                var userId = reqst.body.userId.ToString();

                if (string.IsNullOrEmpty(userId))
                {
                    ri.code = "1000000000";
                }
                else
                {
                    ri.code = "1";
                    var ent = _logic.SelectPresentRecord(reqst.body.pageIndex, reqst.body.pageSize, reqst.body.userId);

                    ri.body = ent;
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



        ///// <summary>
        ///// 获取特定用户当前已提现总额--解志辉
        ///// </summary>
        ///// <param name="reqst">The reqst.</param>
        ///// <returns>ResultInfo&lt;RequestMemberDetail&gt;.</returns>
        //[HttpPost]
        //public ResultInfo<decimal> SelectTotalPresent(RequestParam<RequestMemberDetail> reqst)
        //{
        //    var ri = new ResultInfo<decimal>("99999");
        //    try
        //    {
        //        var userId = reqst.body.userId.ToString();

        //        if (string.IsNullOrEmpty(userId))
        //        {
        //            ri.code = "1000000000";
        //        }
        //        else
        //        {
        //            ri.code = "1";
        //            var ent = _logic.SelectTotalPresent(reqst.body.userId);

        //            ri.body = ent;
        //        }
        //        ri.message = Settings.Instance.GetErrorMsg(ri.code);
        //        return ri;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.Error(ex.ToString());
        //        LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
        //        ri.message = Settings.Instance.GetErrorMsg(ri.code);
        //        return ri;
        //    }
        //}


        /// <summary>
        /// 获取特定用户收支流水列表--解志辉
        /// </summary>
        /// <param name="reqst">
        ///     {"userId":"用户ID","pageIndex":"当前页数", "pageSize":"每页条数"}
        /// </param>
        /// <returns>ResultInfo&lt;BasePage&lt;List&lt;CapitalAccountWater&gt;&gt;&gt;.</returns>
        [HttpPost]
        public ResultInfo<BasePage<List<CapitalAccountWater>>> SelectFundWater(RequestParam<RequestMemberCapital> reqst)
        {
            var ri = new ResultInfo<BasePage<List<CapitalAccountWater>>>("99999");
            try
            {
                int userId = ConvertHelper.ParseValue(reqst.body.userId.ToString(), 0);
                int pageIndex = ConvertHelper.ParseValue(reqst.body.pageIndex.ToString(), 0);
                int pageSize = ConvertHelper.ParseValue(reqst.body.pageSize.ToString(), 0);


                ri.body = _logic.SelectFundWater(pageIndex, pageSize, userId);

                if (ri.body == null)
                {
                    ri.code = "1000000015";
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
        /// 获取特定用户资产统计信息--解志辉
        /// </summary>
        /// <param name="reqst">The reqst.</param>
        /// <returns>ResultInfo&lt;MemberEntity&gt;.</returns>
        [HttpPost]
        public ResultInfo<MemberEntity> TotalCapital(RequestParam<RequestMemberDetail> reqst)
        {
            var ri = new ResultInfo<MemberEntity>("99999");
            try
            {
                int userId = ConvertHelper.ParseValue(reqst.body.userId.ToString(), 0);


                ri.body = _logic.TotalCapital(userId);
                if (ri.body == null)
                {
                    ri.code = "1000000015";
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
        ///// <summary>
        ///// 累计收益接口--解志辉
        ///// </summary>
        ///// <param name="reqst">The reqst.</param>
        ///// <returns>ResultInfo&lt;MemberEntity&gt;.</returns>
        /////  创 建 者：解志辉
        /////  创建日期：2016-05-25 18:09:51
        //[HttpPost]
        //public ResultInfo<MemberEntity> TotalGains(RequestParam<RequestMemberDetail> reqst)
        //{
        //    var ri = new ResultInfo<MemberEntity>("99999");
        //    try
        //    {
        //        int userId = ConvertHelper.ParseValue(reqst.body.userId.ToString(), 0);


        //        ri.body = _logic.SelectTotalGains(userId);
        //        if (ri.body == null)
        //        {
        //            ri.code = "1000000015";
        //        }
        //        else
        //        {
        //            ri.code = "1";
        //        }

        //        ri.message = Settings.Instance.GetErrorMsg(ri.code);
        //        return ri;
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerHelper.Error(ex.ToString());
        //        LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
        //        ri.message = Settings.Instance.GetErrorMsg(ri.code);
        //        return ri;
        //    }
        //}


        /// <summary>
        ///  是否允许用户充值资金
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<string> IsAllowRecharge(RequestParam<RequestRecharge> reqst)
        {
            var ri = new ResultInfo<string>("99999");
            try
            {
                ri.code = "1";
                int userId = ConvertHelper.ParseValue(reqst.body.userId.ToString(), 0);
                MemberLogic mLogic = new MemberLogic();
                string openAcctIds = string.Empty;
                var userCardList = mLogic.SelectUserBankList(userId);
                if (userCardList == null)
                {
                    userCardList = new List<MemberBankEntity>();
                }
                var quickList = userCardList.Where(d => d.BindCardType == 1).ToList();
                if (quickList == null || quickList.Count == 0) //用户没有快捷卡时，有提现审核都拒绝
                {
                    foreach (var item in userCardList)
                    {
                        if (openAcctIds == string.Empty)
                        {
                            openAcctIds += "'" + item.OpenAcctId + "'";
                        }
                        else
                        {
                            openAcctIds += "," + "'" + item.OpenAcctId + "'";
                        }
                    }

                    if (openAcctIds != string.Empty)//有卡时才判定,无卡时允许绑定
                    {
                        bool isExist = mLogic.SelectVUserCashBank(openAcctIds, 0);
                        if (isExist)
                        {
                            LoggerHelper.Info("提现失败,提现审核中,暂不能进行其他操作！" + JsonHelper.Entity2Json(reqst));
                            ri.code = "4000000001";
                        }
                    }
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

    }
}