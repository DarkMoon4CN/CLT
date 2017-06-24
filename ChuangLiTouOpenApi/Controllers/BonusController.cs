using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Bonus;
using ChuangLiTou.Core.Entities.Request.Member;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTouOpenApi.Factory;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.BLL.Api;
namespace ChuangLiTouOpenApi.Controllers
{
    /// <summary>
    /// 优惠券相关接口--解志辉
    /// </summary>
    public class BonusController : BaseApi
    {

        private readonly BonusLogic bonusLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="BonusController"/> class.
        /// </summary>
        /// <param name="bonusLogic">The bonus logic.</param>
        public BonusController(BonusLogic bonusLogic)
        {
            this.bonusLogic = bonusLogic;
        }

        /// <summary>
        /// 获取特定红包当前是否可用--解志辉
        /// </summary>
        /// <param name="reqst">The reqst.</param>
        /// <returns>ResultInfo&lt;System.String&gt;.</returns>
        public ResultInfo<string> CheckBonus(RequestParam<RequestCheckBonus> reqst)
        {
            var ri = new ResultInfo<string>("99999");
            try
            {
                var userId = ConvertHelper.ParseValue(reqst.body.userId, 0);
                var bonusId = reqst.body.bonusId;

                if (reqst.body.userId <= 0)
                {
                    ri.code = "1000000000";
                }
                //else if (reqst.body.bonusId <= 0)
                //{
                //    ri.code = "3000000000";
                //}


                else
                {
                    #region 获取优惠券数据
                    var entity = bonusLogic.SelectBonusById(userId, bonusId);
                    #region 注释代码
                    //switch (entity.reward_state)
                    //{
                    //    case 1://已使用
                    //        {
                    //            ri.code = "3000000002";
                    //            ri.message = Settings.Instance.GetErrorMsg(ri.code);
                    //            return ri;
                    //        }
                    //        break;
                    //    case 2://已过期
                    //        {
                    //            ri.code = "3000000003";
                    //            ri.message = Settings.Instance.GetErrorMsg(ri.code);
                    //            return ri;
                    //        }
                    //        break;
                    //    case 3://已锁定
                    //        {
                    //            ri.code = "3000000004";
                    //            ri.message = Settings.Instance.GetErrorMsg(ri.code);
                    //            return ri;
                    //        }
                    //        break;
                    //}
                    #endregion
                    var investAmount = reqst.body.investAmount;

                    if (entity == null)
                    {
                        ri.code = "3000000000";
                        ri.message = Settings.Instance.GetErrorMsg(ri.code);
                        return ri;
                    }


                    var limitAmount = entity.Sum(t => t.use_lower_limit);
                    if (investAmount < limitAmount)
                    {
                        ri.code = "3000000005";
                        ri.message = Settings.Instance.GetErrorMsg(ri.code).Replace("$limit$", limitAmount.ToString());
                        return ri;
                    }
                    //可以使用 
                    ri.code = "1";
                    var str = "";
                    foreach (var item in entity)
                    {
                        switch (item.reward_state)
                        {
                            case 0://可以使用
                                {
                                    str += item.bonus_account_id + "|";
                                }
                                break;
                            default://不可以使用
                                {
                                }
                                break;
                        }
                    }
                    ri.body = str;
                    #endregion
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