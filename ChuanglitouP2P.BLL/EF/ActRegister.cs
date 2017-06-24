using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.Transfer;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;

namespace ChuanglitouP2P.BLL.EF
{
    /// <summary>
    /// 活动解析操作类--新人注册活动
    /// </summary>
    class ActRegister : ActBase
    {
        /// <summary>
        /// 新用户注册活动
        /// </summary>
        /// <param name="registerid">用户id</param>
        /// <param name="ActUser">面向用户 0不限  1首次投资用户 2非首投用户 3每标首投用户 4每标最大投资用户 5所有投资用户</param>
        protected internal void SendBonusAfterRegister(int registerid, string targetPlatform, int ActUser = 0)
        {
            /// <param name="ActTypeId">活动类型id  1新人活动/2短期活动/3常规活动/4邀请活动/5系统赠送 </param> 

            bool t = true;
            B_member_table oy = new B_member_table();
            hx_member_table investor = new hx_member_table();
            investor = ef.hx_member_table.Where(c => c.registerid == registerid).FirstOrDefault(); //oy.GetModel(Registerid);//被推荐人  也就是投资人
            string codesql = "SELECT invcode,Invpeopleid,invpersonid,invtime from  hx_td_Userinvitation where  invpersonid=" + registerid + " ";//查询本人是否已经被邀请注册过
            DataTable dtcode = DbHelperSQL.GET_DataTable_List(codesql);
            int uuid = dtcode.Rows.Count > 0 ? int.Parse(dtcode.Rows[0]["Invpeopleid"].ToString()) : 0; //邀请用户id

            //用户等级为渠道 不参与活动
            if (investor != null && investor.useridentity == 4)
            {
                t = false;
            }
            //老渠道机制判断 (推荐人等级为4渠道用户,投资次数大于等于1次 时可参与活动   
            if (uuid != 0)
            {
                M_member_table py = new M_member_table();
                py = oy.GetModel(uuid);//推荐人
                if (py != null && py.useridentity == 4)
                {
                    t = false;
                }
            }
            //新渠道机制判断              
            using (ChannelAct channelAct = new ChannelAct())
            {
                //按照渠道类型和投资次数判断是否参与此次活动 
                if (!channelAct.IsParticipateActivity(investor.channel_invitedcode, 0))
                {
                    t = false;
                }
            }

            if (t == true)
            {
                //新人注册奖励
                List<hx_UserAct> hut = RegisterActBonus(registerid, targetPlatform, ActUser);

                BonusRecordOperate(registerid, hut, "新人注册");
            }
        }

        private void BonusRecordOperate(int registerid, List<hx_UserAct> hut, string awardName)
        {
            //??待优化到奖励内部记录
            if (hut.Count > 0)
            {
                foreach (hx_UserAct item in hut)
                {
                    if (item.RewTypeID > 1) //现金转账时做过处理，这里无需再处理
                    {
                        #region MyRegion  记录奖励流水表
                        string awardDescription = string.Format("{2}获得{0}{1}", item.Amt.ToString(), GetBunusDescription(item.RewTypeID), awardName);
                        AddBonusAccoutWater(item.UserAct, registerid, decimal.Parse(item.Amt.ToString()), awardDescription);
                        #endregion

                        #region MyRegion  发送系统消息
                        string MContext = string.Format("{2}获得{0}{1}，如有问题可咨询创利投的客服！", item.Amt.ToString(), GetBunusDescription(item.RewTypeID), awardName);
                        AddSytemMessage(registerid, awardName + "奖励", MContext);
                        #endregion
                    }
                }
            }
        }

        protected internal void SendBonusAfterLogin(int registerid, string targetPlatform, int actUser = 0)
        {
            List<hx_UserAct> hut = LoginActBonus(registerid, targetPlatform, actUser, DateTime.Now.Date.AddMonths(1).AddSeconds(-1));
            BonusRecordOperate(registerid, hut, "恭喜您获得");
        }
        #region 新用户注册活动奖励
        /// <summary>新用户注册活动奖励</summary>
        /// <param name="Registerid">注册用户id</param>
        /// <param name="ActUser">ActUser=0;面向用户 首次投资用户=1，非首投用户=2 ，每标首投用户=3，每标最大投资用户=4，所有投资用户=5，续投用户=6</param>
        /// <returns></returns>
        public List<hx_UserAct> RegisterActBonus(int Registerid, string targetPlatform, int ActUser = 0)
        {
            //ActTypeId=1  活动类型id 1新人活动/2短期活动/3常规活动/4邀请活动/5系统赠送
            int ActTypeId = 1;
            List<hx_UserAct> t = new List<hx_UserAct>();
            hx_ActivityTable hat = GetActTableInfo(ActTypeId, targetPlatform, ActUser, 1);
            if (hat != null)
            {
                if (hat.RewTypeID == 1) //现金奖励,后台界面暂不支持
                {
                    GeneralActCash(Registerid, hat);
                }
                else if (hat.RewTypeID == 2) //代金券奖励
                {
                    t = GeneralActCoupon(Registerid, ActTypeId, hat);
                }
                else if (hat.RewTypeID == 3) //加息券奖励
                {
                    GeneralActInterestRatesPlus(Registerid, ActTypeId, hat);
                }
                //ef.hx_ActivityTable.Where(a => a.ActID == hat.ActID).Update(a => new hx_ActivityTable { ActState = 2 });
            }
            return t;
        }
        #endregion

        #region 新人注册-现金奖励+bool GENERALAmtAct(int Registerid, decimal InvestAmt)
        /// <summary>
        /// 新人注册 现金奖励 
        /// </summary>
        /// <returns></returns>
        private bool GeneralActCash(int Registerid, hx_ActivityTable hat)
        {
            bool t = false;
            if (hat != null)
            {
                string ActRule = hat.ActRule;
                List<MAmtList> mlist = new List<MAmtList>();
                JavaScriptSerializer js = new JavaScriptSerializer();
                MActCash mc = new MActCash();
                mc = js.Deserialize<MActCash>(ActRule);
                //获取对应奖励 
                decimal actamt = GetActAmt(mc, 0, 0, 0);
                if (actamt > 0)//大于 0里写入对应的奖励数据
                {
                    hx_UserAct hua = new hx_UserAct();
                    hua.ActTypeId = hat.ActTypeId;
                    hua.registerid = Registerid;
                    hua.RewTypeID = hat.RewTypeID;
                    hua.ActID = hat.ActID;
                    hua.Amt = actamt;
                    hua.Uselower = 0.00M;
                    hua.Usehight = 0.00M;
                    hua.AmtEndtime = DateTime.Parse(hat.ActEndtime.ToString()).AddMonths(1);
                    hua.AmtUses = 1; //没指定情况下默认为单独使用
                    hua.UseState = 5;  //现金未转账
                    hua.AmtProid = 0; //未使用默认为0
                    hua.ISSmsOne = 0;
                    hua.IsSmsThree = 0;
                    hua.isSmsFifteen = 0;
                    hua.IsSmsSeven = 0;
                    hua.isSmsSixteen = 0;
                    hua.OrderID = decimal.Parse(Utils.Createcode());
                    hua.Createtime = DateTime.Now;
                    ef.hx_UserAct.Add(hua);
                    int i = ef.SaveChanges();
                    if (i > 0)
                    {
                        //录入成功，后进行转账操作 1.获取用户对向
                        M_member_table p = new M_member_table();
                        B_member_table o = new B_member_table();
                        p = o.GetModel(Registerid);
                        if (p != null)
                        {
                            //2.调用商户向用户转账接口
                            Transfer tf = new Transfer();
                            ReTransfer retf = tf.ToUserTransfer(p.UsrCustId, actamt, hua.OrderID.ToString(), hua.ActID.ToString(), "/Thirdparty/ToUserTransfer");
                            if (retf != null)
                            {
                                if (retf.RespCode == "000")
                                {//3.事务处理操作账户及插入流水
                                    #region 验签缓存处理
                                    string cachename = retf.OrdId + "ToUserTransfer" + retf.InCustId;
                                    if (Utils.GeTThirdCache(cachename) == 0)
                                    {
                                        Utils.SetThirdCache(cachename);
                                        B_usercenter BUC = new B_usercenter();
                                        int ic = BUC.UpateActToUserTransfer(retf, 0);
                                        if (ic > 0)
                                        {
                                            string sql = "SELECT registerid,username,mobile  from hx_member_table where UsrCustId='" + retf.InCustId + "'";
                                            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                                            if (dt.Rows.Count > 0)
                                            {
                                                #region MyRegion  记录奖励流水表
                                                string awardDescription = string.Format("{0},已汇入个人账户{1}{2}", hat.ActName, retf.TransAmt, GetBunusDescription(hat.RewTypeID));
                                                AddBonusAccoutWater(int.Parse(hua.UserAct.ToString()), Registerid, decimal.Parse(retf.TransAmt), awardDescription);
                                                #endregion

                                                #region MyRegion  发送系统消息
                                                string MContext = string.Format("尊敬的用户：您好！恭喜您成功参与{0}，获得{1}{2}如有问题可咨询创利投的客服！", hat.ActName, retf.TransAmt, GetBunusDescription(hat.RewTypeID));
                                                AddSytemMessage(Registerid, hat.ActName, MContext);
                                                #endregion                                                

                                                /*短信接口*/
                                            }
                                        }
                                        t = true;
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
            return t;
        }
        #endregion

        #region 新人注册 红包奖励
        /// <summary>
        /// 新人注册 抵扣券 红包奖励 发放  未记录系统信息及奖励流水 与加息券方法一致 GeneralActInterestRatesPlus
        /// </summary>
        /// <param name="registerid">用户id</param>
        /// <param name="investAmt">投资金额</param>
        /// <param name="hat">活动对象</param>
        /// <returns>list 用户活动奖励记录集合</returns>
        /// <returns></returns>
        private List<hx_UserAct> GeneralActCoupon(int registerid, decimal investAmt, hx_ActivityTable hat)
        {
            List<hx_UserAct> t = new List<hx_UserAct>();
            if (hat != null)
            {
                hx_UserAct hua = new hx_UserAct();
                Mcoupon mcp = new Mcoupon();
                JavaScriptSerializer js = new JavaScriptSerializer();
                mcp = js.Deserialize<Mcoupon>(hat.ActRule);
                //获取用户活动表对象
                t = OPMcoupon(mcp, hat, registerid, investAmt);
            }
            return t;
        }
        private List<hx_UserAct> GeneralActCoupon(int registerid, decimal investAmt, hx_ActivityTable hat, DateTime endTime)
        {
            List<hx_UserAct> t = new List<hx_UserAct>();
            if (hat != null)
            {
                hx_UserAct hua = new hx_UserAct();
                Mcoupon mcp = new Mcoupon();
                JavaScriptSerializer js = new JavaScriptSerializer();
                mcp = js.Deserialize<Mcoupon>(hat.ActRule);
                //获取用户活动表对象
                t = OPMcoupon(mcp, hat, registerid, investAmt, endTime);
            }
            return t;
        }
        #endregion

        #region 新人注册活动加息券 
        /// <summary>
        /// 活动加息券
        /// </summary>
        /// 常规活动 加息券奖励 未记录系统信息及奖励流水 //与红包方法一致GeneralActCoupon
        /// </summary>
        /// <param name="registerid">用户id</param>
        /// <param name="investAmt">投资金额</param>
        /// <param name="hat">活动对象</param>
        /// <returns>list 用户活动奖励记录集合</returns>
        /// <returns></returns> ActTypeId=3
        private List<hx_UserAct> GeneralActInterestRatesPlus(int registerid, decimal investAmt, hx_ActivityTable hat)
        {
            List<hx_UserAct> t = new List<hx_UserAct>();
            if (hat != null)
            {
                hx_UserAct hua = new hx_UserAct();
                Mcoupon mcp = new Mcoupon();
                JavaScriptSerializer js = new JavaScriptSerializer();
                mcp = js.Deserialize<Mcoupon>(hat.ActRule);
                //获取用户活动表对象
                t = OPMcoupon(mcp, hat, registerid, investAmt);
            }
            return t;
        }
        #endregion


        public List<hx_UserAct> LoginActBonus(int registerID, string targetPlatform, int actUser, DateTime endTime)
        {
            //ActTypeId=1  活动类型id 1新人活动/2短期活动/3常规活动/4邀请活动/5系统赠送
            int ActTypeId = 2;
            hx_ActivityTable hat = GetActTableInfo(ActTypeId, targetPlatform, actUser);
  
            if (hat!=null && hat.RewTypeID == 2) //代金券奖励
            {
                return GeneralActCoupon(registerID, ActTypeId, hat, endTime);
            }
            return new List<hx_UserAct>();
        }
    }
}
