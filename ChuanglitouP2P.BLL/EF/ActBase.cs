using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq.Dynamic;
using System.Data.Linq.SqlClient;
using System.Text;

namespace ChuanglitouP2P.BLL.EF
{
    /// <summary>
    /// 活动解析操作类--基类
    /// </summary>
    public partial class ActBase
    {
        protected chuangtouEntities ef = new chuangtouEntities();

        #region 按照投资金额进行红包或加息券奖励发放（常规投资 红包或加息券）+hx_UserAct OPMcoupon(Mcoupon mcp,  hx_ActivityTable hat,int Registerid, decimal investAmt)
        /// <summary>
        /// 按照投资金额进行红包或加息券奖励发放（常规投资 红包或加息券） 未记录系统信息及奖励流水 
        /// 返回用户活动表hx_UserAct 对象
        /// </summary>
        /// <param name="mcp">规则对象</param>        
        ///  <param name="hua">活动项目表</param>    
        ///  <param name="Registerid">用户id</param>    
        ///  <param name="investAmt">投资金额</param>
        protected List<hx_UserAct> OPMcoupon(Mcoupon mcp, hx_ActivityTable hat, int Registerid, decimal investAmt)
        {
            List<hx_UserAct> t = new List<hx_UserAct>();
            List<Msplitarr> msp = new List<Msplitarr>();
            List<MAmtList> mat = new List<MAmtList>();
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (mcp.rule == 1) //统一赠送
            {
                #region 统一赠送
                t.Clear();
                string msplitarrc = "";
                //经过比对拆分不拆分代码逻辑一样，所以合并处理
                //if (mcp.ISsplit == 1)               
                try
                {
                    msplitarrc = mcp.Msplitarr.SerializeJSON();
                    msp = js.Deserialize<List<Msplitarr>>(msplitarrc);
                }
                catch
                {
                    msp = null;
                }
                foreach (var item in msp)
                {
                    hx_UserAct hua = new hx_UserAct();
                    hua.ActTypeId = hat.ActTypeId;
                    hua.registerid = Registerid;
                    hua.RewTypeID = hat.RewTypeID;
                    hua.ActID = hat.ActID;
                    hua.Amt = item.cashAmt;
                    hua.Uselower = item.startAmt;
                    hua.Usehight = item.endAmt;
                    if (item.validity > 0) //有效天数大于0
                    {
                        hua.AmtEndtime = DateTime.Now.Date.AddDays(item.validity + 1).AddSeconds(-1);
                    }
                    else
                    {
                        hua.AmtEndtime = item.endTime;
                    }

                    hua.AmtUses = mcp.Uses; //没指定情况下默认为单独使用
                    hua.UseState = 0;  //现金未转账
                    hua.AmtProid = 0; //未使用默认为0
                    hua.ISSmsOne = 0;
                    hua.IsSmsThree = 0;
                    hua.isSmsFifteen = 0;
                    hua.IsSmsSeven = 0;
                    hua.isSmsSixteen = 0;
                    hua.Createtime = DateTime.Now;
                    hua.Title = hat.ActName;
                    hua.UseLifeLoan = string.IsNullOrWhiteSpace(item.UseLifeLoan) ? "" : item.UseLifeLoan;
                    ef.hx_UserAct.Add(hua);
                    int i = ef.SaveChanges();
                    t.Add(hua);
                }
                #endregion
            }
            else if (mcp.rule == 2)//按投资赠送，需要判断投资金额区间，不支持拆分 只赠送一张代金券或加息券，使用限制按照金额在拆分中匹配
            {
                #region 按投资赠送 
                hx_UserAct hua = new hx_UserAct();
                t.Clear();
                string msplitarrc = "";
                hua.ActTypeId = hat.ActTypeId;
                hua.registerid = Registerid;
                hua.RewTypeID = hat.RewTypeID;
                hua.ActID = hat.ActID;
                try
                {
                    msplitarrc = mcp.Msplitarr.SerializeJSON();
                    msp = js.Deserialize<List<Msplitarr>>(msplitarrc);
                }
                catch
                {
                    msp = null;
                }
                hua.Amt = GetCouponAmtByInvestAmt(mcp, investAmt);
                hua.Uselower = GetCouponUseConByAmt(msp, decimal.Parse(hua.Amt.ToString()), 0);
                hua.Usehight = GetCouponUseConByAmt(msp, decimal.Parse(hua.Amt.ToString()), 1);
                hua.AmtEndtime = GetCouponEndTimeByAmt(msp, decimal.Parse(hua.Amt.ToString()));
                hua.AmtUses = mcp.Uses; //没指定情况下默认为单独使用
                hua.UseState = 0;  //现金未转账
                hua.AmtProid = 0; //未使用默认为0
                hua.ISSmsOne = 0;
                hua.IsSmsThree = 0;
                hua.isSmsFifteen = 0;
                hua.IsSmsSeven = 0;
                hua.isSmsSixteen = 0;
                hua.Createtime = DateTime.Now;
                hua.Title = hat.ActName;
                hua.UseLifeLoan = msp == null ? "" : msp[0].UseLifeLoan;
                if (hua.Uselower >= 0 || hua.Usehight >= 0 || hua.Amt <= 0)
                {
                    ef.hx_UserAct.Add(hua);
                    int i = ef.SaveChanges();
                    if (i > 0)
                    {
                        t.Add(hua);
                    }
                }
                #endregion
            }

            else if (mcp.rule == 3) //随机赠送，需要判断投资金额区间，不支持拆分 只赠送一张代金券或加息券，使用限制按照金额在拆分中匹配
            {
                #region 随机赠送
                t.Clear();
                foreach (var item in mcp.MAmtList)
                {
                    if (item.startAmt <= investAmt && (item.endAmt == 0 || item.endAmt >= investAmt))
                    {
                        for (int i = 0; i < item.num; i++)
                        {
                            hx_UserAct hua = new hx_UserAct();
                            string msplitarrc = "";
                            hua.ActTypeId = hat.ActTypeId;
                            hua.registerid = Registerid;
                            hua.RewTypeID = hat.RewTypeID;
                            hua.ActID = hat.ActID;
                            try
                            {
                                msplitarrc = mcp.Msplitarr.SerializeJSON();
                                msp = js.Deserialize<List<Msplitarr>>(msplitarrc);
                            }
                            catch
                            {
                                msp = null;
                            }
                            hua.Amt = Utils.GetActRandom(item.Amtstr);
                            hua.Uselower = GetCouponUseConByAmt(msp, decimal.Parse(hua.Amt.ToString()), 0);
                            hua.Usehight = GetCouponUseConByAmt(msp, decimal.Parse(hua.Amt.ToString()), 1);
                            hua.AmtEndtime = GetCouponEndTimeByAmt(msp, decimal.Parse(hua.Amt.ToString()));

                            hua.AmtUses = mcp.Uses; //没指定情况下默认为单独使用
                            hua.UseState = 0;  //现金未转账
                            hua.AmtProid = 0; //未使用默认为0
                            hua.ISSmsOne = 0;
                            hua.IsSmsThree = 0;
                            hua.isSmsFifteen = 0;
                            hua.IsSmsSeven = 0;
                            hua.isSmsSixteen = 0;
                            hua.Createtime = DateTime.Now;
                            hua.Title = hat.ActName;
                            hua.UseLifeLoan = msp == null ? "" : msp[0].UseLifeLoan;
                            if (hua.Uselower >= 0 || hua.Usehight >= 0 || hua.Amt <= 0)
                            {
                                ef.hx_UserAct.Add(hua);
                                int c1 = ef.SaveChanges();
                                if (c1 > 0)
                                {
                                    t.Add(hua);
                                }
                            }
                        }
                        break;
                    }
                }
                #endregion
            }
            return t;
        }

        /// <summary>
        /// 按照投资金额进行红包或加息券奖励发放（常规投资 红包或加息券） 未记录系统信息及奖励流水 
        /// 返回用户活动表hx_UserAct 对象
        /// </summary>
        /// <param name="mcp">规则对象</param>        
        ///  <param name="hua">活动项目表</param>    
        ///  <param name="Registerid">用户id</param>    
        ///  <param name="investAmt">投资金额</param>
        protected List<hx_UserAct> OPMcoupon(Mcoupon mcp, hx_ActivityTable hat, int Registerid, decimal investAmt, DateTime endTime)
        {
            List<hx_UserAct> t = new List<hx_UserAct>();
            List<Msplitarr> msp = new List<Msplitarr>();
            List<MAmtList> mat = new List<MAmtList>();
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (mcp.rule == 1) //统一赠送
            {
                #region 统一赠送
                t.Clear();
                string msplitarrc = "";
                //经过比对拆分不拆分代码逻辑一样，所以合并处理
                //if (mcp.ISsplit == 1)               
                try
                {
                    msplitarrc = mcp.Msplitarr.SerializeJSON();
                    msp = js.Deserialize<List<Msplitarr>>(msplitarrc);
                }
                catch
                {
                    msp = null;
                }
                foreach (var item in msp)
                {
                    hx_UserAct hua = new hx_UserAct();
                    hua.ActTypeId = hat.ActTypeId;
                    hua.registerid = Registerid;
                    hua.RewTypeID = hat.RewTypeID;
                    hua.ActID = hat.ActID;
                    hua.Amt = item.cashAmt;
                    hua.Uselower = item.startAmt;
                    hua.Usehight = item.endAmt;
                    hua.AmtEndtime = endTime;//item.endTime;
                    hua.AmtUses = mcp.Uses; //没指定情况下默认为单独使用
                    hua.UseState = 0;  //现金未转账
                    hua.AmtProid = 0; //未使用默认为0
                    hua.ISSmsOne = 0;
                    hua.IsSmsThree = 0;
                    hua.isSmsFifteen = 0;
                    hua.IsSmsSeven = 0;
                    hua.isSmsSixteen = 0;
                    hua.Createtime = DateTime.Now;
                    hua.Title = hat.ActName;
                    hua.UseLifeLoan = string.IsNullOrWhiteSpace(item.UseLifeLoan) ? "" : item.UseLifeLoan;
                    ef.hx_UserAct.Add(hua);
                    int i = ef.SaveChanges();
                    t.Add(hua);
                }
                #endregion
            }
            else if (mcp.rule == 2)//按投资赠送，需要判断投资金额区间，不支持拆分 只赠送一张代金券或加息券，使用限制按照金额在拆分中匹配
            {
                #region 按投资赠送 
                hx_UserAct hua = new hx_UserAct();
                t.Clear();
                string msplitarrc = "";
                hua.ActTypeId = hat.ActTypeId;
                hua.registerid = Registerid;
                hua.RewTypeID = hat.RewTypeID;
                hua.ActID = hat.ActID;
                try
                {
                    msplitarrc = mcp.Msplitarr.SerializeJSON();
                    msp = js.Deserialize<List<Msplitarr>>(msplitarrc);
                }
                catch
                {
                    msp = null;
                }
                hua.Amt = GetCouponAmtByInvestAmt(mcp, investAmt);
                hua.Uselower = GetCouponUseConByAmt(msp, decimal.Parse(hua.Amt.ToString()), 0);
                hua.Usehight = GetCouponUseConByAmt(msp, decimal.Parse(hua.Amt.ToString()), 1);
                hua.AmtEndtime = endTime;//GetCouponEndTimeByAmt(msp, decimal.Parse(hua.Amt.ToString()));
                hua.AmtUses = mcp.Uses; //没指定情况下默认为单独使用
                hua.UseState = 0;  //现金未转账
                hua.AmtProid = 0; //未使用默认为0
                hua.ISSmsOne = 0;
                hua.IsSmsThree = 0;
                hua.isSmsFifteen = 0;
                hua.IsSmsSeven = 0;
                hua.isSmsSixteen = 0;
                hua.Createtime = DateTime.Now;
                hua.Title = hat.ActName;
                hua.UseLifeLoan = msp == null ? "" : msp[0].UseLifeLoan;
                if (hua.Uselower >= 0 || hua.Usehight >= 0 || hua.Amt <= 0)
                {
                    ef.hx_UserAct.Add(hua);
                    int i = ef.SaveChanges();
                    if (i > 0)
                    {
                        t.Add(hua);
                    }
                }
                #endregion
            }

            else if (mcp.rule == 3) //随机赠送，需要判断投资金额区间，不支持拆分 只赠送一张代金券或加息券，使用限制按照金额在拆分中匹配
            {
                #region 随机赠送
                t.Clear();
                foreach (var item in mcp.MAmtList)
                {
                    if (item.startAmt <= investAmt && (item.endAmt == 0 || item.endAmt >= investAmt))
                    {
                        for (int i = 0; i < item.num; i++)
                        {
                            hx_UserAct hua = new hx_UserAct();
                            string msplitarrc = "";
                            hua.ActTypeId = hat.ActTypeId;
                            hua.registerid = Registerid;
                            hua.RewTypeID = hat.RewTypeID;
                            hua.ActID = hat.ActID;
                            try
                            {
                                msplitarrc = mcp.Msplitarr.SerializeJSON();
                                msp = js.Deserialize<List<Msplitarr>>(msplitarrc);
                            }
                            catch
                            {
                                msp = null;
                            }
                            hua.Amt = Utils.GetActRandom(item.Amtstr);
                            hua.Uselower = GetCouponUseConByAmt(msp, decimal.Parse(hua.Amt.ToString()), 0);
                            hua.Usehight = GetCouponUseConByAmt(msp, decimal.Parse(hua.Amt.ToString()), 1);
                            hua.AmtEndtime = endTime;//GetCouponEndTimeByAmt(msp, decimal.Parse(hua.Amt.ToString()));

                            hua.AmtUses = mcp.Uses; //没指定情况下默认为单独使用
                            hua.UseState = 0;  //现金未转账
                            hua.AmtProid = 0; //未使用默认为0
                            hua.ISSmsOne = 0;
                            hua.IsSmsThree = 0;
                            hua.isSmsFifteen = 0;
                            hua.IsSmsSeven = 0;
                            hua.isSmsSixteen = 0;
                            hua.Createtime = DateTime.Now;
                            hua.Title = hat.ActName;
                            hua.UseLifeLoan = msp == null ? "" : msp[0].UseLifeLoan;
                            if (hua.Uselower >= 0 || hua.Usehight >= 0 || hua.Amt <= 0)
                            {
                                ef.hx_UserAct.Add(hua);
                                int c1 = ef.SaveChanges();
                                if (c1 > 0)
                                {
                                    t.Add(hua);
                                }
                            }
                        }
                        break;
                    }
                }
                #endregion
            }
            return t;
        }
        #endregion

        #region 获取符合条件在有效时间内的一个活动对象
        /// <summary>获取符合条件在有效时间内的一个活动对象</summary>
        /// <param name="ActTypeId">活动类型id 1新人活动/2短期活动/3常规活动/4邀请活动/5系统赠送 </param> 
        /// <param name="ActUser">面向用户 首次投资用户=1，非首投用户=2 ，每标首投用户=3，每标最大投资用户=4，所有投资用户=5，续投用户=6 , 特殊复投用户=7</param>
        /// <param name="ActState">活动状态  0默认(未上线)  1进行中(上线)  2结束(下线) 3停止</param>
        /// <param name="RewTypeID">RewTypeID 默认0不限制  奖励类型id 1现金/2抵扣券/3加息券</param>
        /// <returns>hx_ActivityTable 符合条件的一个活动对象</returns>
        protected hx_ActivityTable GetActTableInfo(int ActTypeId, string targetPlatform, int ActUser = 0, int ActState = 1, int RewTypeID = 0)
        {
            hx_ActivityTable hat = null; //new hx_ActivityTable();
            string key = "Act" + ActTypeId.ToString() + ActUser.ToString() + ActState.ToString() + RewTypeID.ToString();
            StringBuilder sql = new StringBuilder();
            if (HttpRuntime.Cache[key] == null)
            {
                sql.AppendFormat("select top 1 * from hx_ActivityTable where ActTypeId = {0} and ActState = {1} and ActTypeId != {2} and ActStarttime < '{3}' and ActEndtime > '{3}'", ActTypeId, ActState, 5, DateTime.Now);
                //var query = ef.hx_ActivityTable.Where(p => p.ActTypeId == ActTypeId && p.ActState == ActState && p.ActTypeId != 5 && p.ActStarttime < DateTime.Now && p.ActEndtime > DateTime.Now);
                //if (ActUser != 0)
                //{
                //    query = query.Where(p => p.ActUser == ActUser);
                //}
                if (ActUser != 0)
                {
                    sql.AppendFormat(" and ActUser = {0} ", ActUser);
                }
                //if (RewTypeID != 0)
                //{
                //    query = query.Where(p => p.RewTypeID == RewTypeID);
                //}
                if (RewTypeID != 0)
                {
                    sql.AppendFormat(" and RewTypeID = {0} ", RewTypeID);
                }
                if (!string.IsNullOrWhiteSpace(targetPlatform) && targetPlatform != EnumCommon.E_hx_ActivityTable.E_ActTargetPlatform.all)
                {
                    string tp = targetPlatform.Replace("0", "_");
                    sql.AppendFormat(" and (ActTargetPlatform is null or ActTargetPlatform like '{0}') ", tp);
                    //query = query.AsQueryable().Where(p => p.ActTargetPlatform == null || SqlMethods.Like(p.ActTargetPlatform, targetPlatform.Replace("0", "_")));
                    //query = query.Where(" ActTargetPlatform like @atp ", new ObjectParameter("atp", tp));
                }
                sql.Append(" order by ActID desc ");
                DataTable dt = DBUtility.DbHelperSQL.Query(sql.ToString()).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    hat = ConvertDataTable<hx_ActivityTable>.ConvertToList(dt)[0];
                }
                //hat = query.OrderByDescending(p => p.ActID).FirstOrDefault();

                if (hat != null)
                {//放入缓存
                    HttpRuntime.Cache.Add(key, hat, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                }
            }
            else
            {
                hat = (hx_ActivityTable)HttpRuntime.Cache[key];
            }
            return hat;
        }
        #endregion


        #region 获取符合条件的一个活动对象
        /// <summary>获取符合条件在有效时间内的一个活动对象</summary>
        /// <param name="ActTypeId">活动类型id 1新人活动/2短期活动/3常规活动/4邀请活动/5系统赠送 </param> 
        /// <param name="ActUser">面向用户 首次投资用户=1，非首投用户=2 ，每标首投用户=3，每标最大投资用户=4，所有投资用户=5，续投用户=6 , 特殊复投用户=7</param>
        /// <param name="ActState">活动状态 -1不限制 0默认(未上线)  1进行中(上线)  2结束(下线) 3停止</param>
        /// <param name="RewTypeID">RewTypeID 默认0不限制  奖励类型id 1现金/2抵扣券/3加息券</param>
        /// <param name="isDescending">是否按照id降序排列</param>
        /// <returns>hx_ActivityTable 符合条件的一个活动对象</returns>
        protected hx_ActivityTable GetActTableInfoWithoutTimeLimit(int ActTypeId, int ActUser = 0, int ActState = 1, int RewTypeID = 0, bool isDescending = true)
        {
            hx_ActivityTable hat = new hx_ActivityTable();
            string key = "ActWithoutTimeLimit" + ActTypeId.ToString() + ActUser.ToString() + ActState.ToString() + RewTypeID.ToString();
            if (HttpRuntime.Cache[key] == null)
            {
                var query = ef.hx_ActivityTable.Where(p => p.ActTypeId == ActTypeId);
                if (ActState != -1)
                {
                    query = query.Where(p => p.ActState == ActState);
                }
                if (ActUser != 0)
                {
                    query = query.Where(p => p.ActUser == ActUser);
                }
                if (RewTypeID != 0)
                {
                    query = query.Where(p => p.RewTypeID == RewTypeID);
                }
                if (isDescending)
                {
                    hat = query.OrderByDescending(p => p.ActID).FirstOrDefault();
                }
                else
                {
                    hat = query.OrderBy(p => p.ActID).FirstOrDefault();
                }
                if (hat != null)
                {//放入缓存
                    HttpRuntime.Cache.Add(key, hat, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                }
            }
            else
            {
                hat = (hx_ActivityTable)HttpRuntime.Cache[key];
            }
            return hat;
        }
        #endregion 
        #region 获取抵扣券数值，按投资范围赠送
        private decimal GetCouponAmtByInvestAmt(Mcoupon mcp, decimal InvestAmt)
        {
            decimal amt = 0.00M;
            foreach (MAmtList item in mcp.MAmtList)
            {
                if (item.startAmt <= InvestAmt && (item.endAmt == 0 || item.endAmt >= InvestAmt))
                {
                    if (mcp.rule == 2)  //按投资赠送 ，随机赠送逻辑单独进行
                    {
                        amt = item.percent;
                        break;
                    }
                }
            }
            return amt;
        }
        #endregion

        #region 根据活动规则计算奖励现金金额
        /// <summary>
        /// 根据活动规则计算奖励现金金额
        /// </summary>
        /// <param name="mc">现金奖励</param>
        /// <param name="InvestAmt">投资金额</param>
        /// <param name="TopNum">当前该活动已参与人数</param>
        /// <param name="lifeLoan">所投标的期限 0不限制</param>
        /// <returns></returns>
        protected decimal GetActAmt(MActCash mc, decimal InvestAmt, int TopNum = 0, int lifeLoan = 0)
        {
            decimal amt = 0.00M;
            if (TopNum <= mc.TopNum || mc.TopNum == 0)
            {
                foreach (MAmtList item in mc.MAmtList)
                {
                    if ((item.startAmt <= InvestAmt || InvestAmt == -1) && (item.endAmt == 0 || item.endAmt >= InvestAmt) && (lifeLoan == 0 || item.LifeLoan == 0 || item.LifeLoan == lifeLoan))
                    {
                        if (mc.require == 1)  //按百分比赠送  
                        {
                            amt = (item.percent / 100) * InvestAmt;
                            if (amt > mc.TopAmt1 && mc.TopAmt1 != 0)//大于单比封顶金额
                            {
                                amt = mc.TopAmt1;
                            }
                        }
                        else if (mc.require == 2)//2 投投资赠送
                        {
                            amt = item.percent;
                        }
                        break;
                    }
                }
            }
            return amt;
        }
        #endregion

        #region 根据红包奖励金额返回对应的使用上限或下限的值 i=0 获取下限,i=1获取上限+decimal GetMsplitarr(Msplitarr msp, decimal investAmt, int i = 0)
        /// <summary>
        /// 根据红包奖励金额返回对应的使用上限或下限的值 i=0 获取下限,i=1获取上限
        /// </summary>
        /// <param name="msp">使用条件规则对象</param>
        /// <param name="Amt">红包金额</param>
        /// <param name="i"> i=0 获取下限,i=1获取上限</param>
        /// <returns></returns>
        private decimal GetCouponUseConByAmt(List<Msplitarr> msp, decimal Amt, int i = 0)
        {
            decimal dec = 0M;
            if (i != 0 && i != 1)
            {
                return dec;
            }
            Msplitarr msplitarr = GetMsplitarrByAmt(msp, Amt);
            if (msplitarr != null)
            {
                if (i == 0) //获取下限条件限制
                {
                    dec = msplitarr.startAmt;
                }
                else if (i == 1) //获取上限条件限制
                {
                    dec = msplitarr.endAmt;
                }
            }
            return dec;
        }
        #endregion

        #region 根据红包奖励金额返回对应的使用期限
        /// <summary>
        /// 根据红包奖励金额返回对应的使用期限
        /// </summary>
        /// <param name="msp">使用条件规则对象</param>
        /// <param name="Amt">红包金额</param>
        /// <returns></returns>
        private DateTime GetCouponEndTimeByAmt(List<Msplitarr> msp, decimal Amt)
        {
            DateTime datet = DateTime.Now.AddMonths(1);
            Msplitarr msplitarr = GetMsplitarrByAmt(msp, Amt);
            if (msplitarr != null)
            {
                datet = msplitarr.endTime;
            }
            return datet;
        }
        #endregion

        #region 根据红包奖励金额返回对应的Msplitarr对象
        /// <summary>
        /// 根据红包奖励金额返回对应的Msplitarr对象
        /// </summary>
        /// <param name="msp">使用条件规则对象</param>
        /// <param name="Amt">红包金额</param>
        /// <returns>Msplitarr对象</returns>
        private Msplitarr GetMsplitarrByAmt(List<Msplitarr> msp, decimal Amt)
        {
            Msplitarr msplitarr = null;
            foreach (var item in msp)
            {
                if (item.cashAmt == Amt)
                {
                    msplitarr = item;
                    break;
                }
            }
            return msplitarr;
        }
        #endregion

        //根据奖励类型，返回奖励流水描述后缀
        protected string GetBunusDescription(int? RewTypeID)
        {
            string description = "元奖励。";
            if (RewTypeID == 3)
            {
                description = "%加息券奖励。";
            }
            else if (RewTypeID == 2)
            {
                description = "元抵扣券奖励。";
            }
            return description;
        }
        //
        //
        /// <summary>
        /// 生成奖励流水(抵扣券、现金)，加息券不记录
        /// </summary>
        /// <param name="UserActid">用户奖励id</param>
        /// <param name="registerid">用户id</param>
        /// <param name="bonusAmt">奖励金额</param>
        /// <param name="awardDescription">奖励描述</param>
        /// <param name="waterType">waterType 默认为0，当为邀请奖励时，存放被邀请人的id</param>
        /// <returns></returns>
        protected bool AddBonusAccoutWater(int UserActid, int registerid, decimal bonusAmt, string awardDescription, int waterType = 0)
        {
            B_bonus_account_water bbaw = new B_bonus_account_water();
            M_bonus_account_water mbaw = new M_bonus_account_water();
            mbaw.bonus_account_id = UserActid;
            mbaw.membertable_registerid = registerid;
            mbaw.income = bonusAmt;
            mbaw.expenditure = 0.00M;
            mbaw.time_of_occurrence = DateTime.Now;
            mbaw.award_description = awardDescription;
            mbaw.water_type = waterType;
            bbaw.Add(mbaw);
            return true;
        }
        /// <summary>
        ///生成站内信 系统消息（发放奖励）
        /// </summary>
        ///<param name="registerid">用户id</param>
        ///<param name="MTitle">消息标题</param>
        ///<param name="MContext">消息内容</param>
        ///<param name="Mtye">消息类型 0系统消息 1投资通知   2收益通知   3提现 4充值 5系统通知</param> 
        protected bool AddSytemMessage(int registerid, string MTitle, string MContext, int Mtype = 5)
        {
            DateTime dtiff = DateTime.Now;
            M_td_System_message pm2 = new M_td_System_message();
            pm2.MReg = registerid;
            pm2.Mstate = 0;
            pm2.MTitle = MTitle;
            pm2.MContext = MContext;
            pm2.PubTime = dtiff;
            pm2.Mtype = Mtype;
            B_usercenter.AddMessage(pm2);
            return true;
        }
        /// <summary>
        /// 获取加息券、抵扣券使用限制说明
        /// </summary>
        /// <param name="useLifeLoan"></param>
        /// <returns></returns>
        public List<int> GetCanUseLimit(string useLifeLoan, out string msg)
        {
            List<int> res = new List<int>();
            msg = "投资使用无限制";
            res.Add(0);
            if (string.IsNullOrWhiteSpace(useLifeLoan))
                return res;
            string[] lifeLoans = useLifeLoan.Split('-');
            if (lifeLoans.Length != 2)
                return res;
            int min = 0, max = 0;
            if (int.TryParse(lifeLoans[0], out min) && int.TryParse(lifeLoans[1], out max))
            {
                if (min == 0 && max == 0)
                    return res;

                res = new List<int>();
                res.Add(min);
                res.Add(max);
                if (min > 0 && max > 0)
                {
                    if (min == max)
                        msg = string.Format("投资{0}个月标可用", min);
                    else
                        msg = string.Format("投资{0}-{1}个月标可用", min, max);
                }
                else if (min == 0 && max > 0)
                    msg = string.Format("投资小于等于{0}个月的标可用", max);
                else
                    msg = string.Format("投资大于等于{0}个月的标可用", min);
            }
            return res;
        }
        /// <summary>
        /// 检查当前标的是否可以使用该抵扣券、加息券
        /// </summary>
        /// <param name="limitInt"></param>
        /// <param name="unitDay"></param>
        /// <param name="lifeLoan"></param>
        /// <returns></returns>
        public bool CheckLimit(List<int> limitInt, int unitDay, int lifeLoan)
        {
            if (limitInt.Count == 1 && limitInt[0] == 0)
                return true;
            if (limitInt.Count != 2)
                return false;
            int lifeLoanMonth = 0;
            if (unitDay == 1) { lifeLoanMonth = lifeLoan >= 6 ? 6 : (lifeLoan < 3 ? 1 : 3); }
            if (unitDay == 3) { lifeLoanMonth = (lifeLoan / 30) >= 6 ? 6 : ((lifeLoan / 30) < 3 ? ((lifeLoan / 30) >= 1 ? 1 : -1) : 3); }
            return (lifeLoanMonth >= limitInt[0] || limitInt[0] == 0) && (lifeLoanMonth <= limitInt[1] || limitInt[1] == 0);
        }
    }
}
