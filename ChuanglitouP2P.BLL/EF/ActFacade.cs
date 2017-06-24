using ChuanglitouP2P.Common;
using ChuangLitouP2P.Models;
using System;
using System.Data;
using System.Linq;

namespace ChuanglitouP2P.BLL.EF
{
    /// <summary>
    /// 活动解析操作类,对外公开使用类
    /// </summary>
    public class ActFacade : System.IDisposable
    {
        chuangtouEntities ef = new chuangtouEntities();

        /// <summary>
        /// app登录发放奖励
        /// </summary>
        /// <param name="registerID"></param>
        /// <param name="targetPlatform"></param>
        public void SendBonusAfterLogin(int registerID, string targetPlatform = "0000")
        {
          
            try
            {
                //hx_member_table user = ef.hx_member_table.Where(c => c.registerid == registerID).FirstOrDefault();
                //if (user == null)
                //return;
                //var act = ef.hx_ActivityTable.Where(d => d.ActName == "下载app送1260").First();
                var isChannel1 = (from item in ef.hx_member_table
                                  join uInvited in
                                  (from htu in ef.hx_td_Userinvitation
                                   join peo in ef.hx_member_table
                                   on htu.Invpeopleid equals peo.registerid
                                   select new { useridentity = peo.useridentity, invpersonid = htu.invpersonid })
                                  on item.registerid equals uInvited.invpersonid into lu
                                  from luLeft in lu.DefaultIfEmpty()
                                  join invest in (from hbr in ef.hx_Bid_records
                                                  group hbr by hbr.investor_registerid into g
                                                  select new { investor_registerid = g.Key, investCount = g.Count() })
                                  on item.registerid equals invest.investor_registerid into investL
                                  from investLeft in investL.DefaultIfEmpty()
                                  where item.registerid == registerID
                                  select new
                                  {
                                      channel_invitedcode = item.channel_invitedcode,
                                      useridentity = luLeft == null ? -1 : luLeft.useridentity,//邀请人用户等级
                                      uidentity = item.useridentity,//被邀请人用户等级
                                      investCount = investLeft == null ? 0 : investLeft.investCount
                                  });
                var isChannel=isChannel1.ToList();

                //var aa = isChannel1.Where(c => c.uidentity != 4);
                //var bb = isChannel1.Where(c => (c.useridentity != 4 || c.investCount >= 1));
                //var cc = isChannel1.Where(c => new ChannelAct().IsParticipateActivity(c.channel_invitedcode, c.investCount) == true);
                ////增加逻辑，如果用户为渠道用户，并且历史投资次数大于等于一次，则允许参与1260红包活动
                //if (isChannel.Where(c => c.investCount >= 1 || (string.IsNullOrWhiteSpace(c.channel_invitedcode) && c.useridentity != 4 && c.uidentity != 4)).Count() <= 0)
                //    return;
                //用户等级为渠道 不参与活动
                if (isChannel.Where(c => c.uidentity != 4).Count() <= 0)
                {
                    return;
                }
                //老渠道机制判断 (推荐人等级为4渠道用户,投资次数大于等于1次 时可参与活动   
                if (isChannel.Where(c=> (c.useridentity != 4||c.investCount >= 1)).Count() <= 0)
                    return;
                //新渠道机制判断              
                using (ChannelAct channelAct=new ChannelAct())
                {
                    //按照渠道类型和投资次数判断是否参与此次活动 
                    //IsParticipateActivity方法中是已投资后奖励发放为主，为兼容该类登录红包，投资次数默认+1后计算
                    if (isChannel.Where(c => channelAct.IsParticipateActivity(c.channel_invitedcode, c.investCount+1) ==true).Count() <= 0)
                    {
                        return;
                    }
                }
                int sended = (from item in ef.hx_UserAct
                              //join user in ef.hx_member_table
                              //on item.registerid equals user.registerid
                              join act in ef.hx_ActivityTable
                              on item.ActID equals act.ActID
                              where item.registerid == registerID && (act.ActName == "下载app登录送1260" || act.ActName == "下载app登录送100")
                              select item.ActID).Count();
                if (sended == 0)
                {
                    new ActRegister().SendBonusAfterLogin(registerID, targetPlatform, 1);
                }

                //所有用户登录送1580元抵扣券（活动时间：2017.01.09-01.24）
                int sended2 = (from item in ef.hx_UserAct
                                  //join user in ef.hx_member_table
                                  //on item.registerid equals user.registerid
                              join act in ef.hx_ActivityTable
                              on item.ActID equals act.ActID
                              where item.registerid == registerID && act.ActName == "登录送1580元抵扣券"
                              select item.ActID).Count();
                if (sended2 == 0)
                {
                    new ActRegister().SendBonusAfterLogin(registerID, "1111", 5);//ActUser面向用户 首次投资用户=1，非首投用户=2 ，每标首投用户=3，每标最大投资用户=4，所有投资用户=5，续投用户=6 , 特殊复投用户=7
                }
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog("app登录奖励发放异常：" + ex.Message + "；StackTrace：" + ex.StackTrace);
            }
        }
        #region 投资后按照常规活动进行奖励发放（常规投资、邀请奖励）

        /// <summary>
        /// 汇付返回成功,投资奖励发放   DataTable dt 投资记录视图 V_hx_Bid_records_borrowing_target    投资完成执行
        /// </summary>
        /// <param name="dt"></param>
        public void SendBonusAfterInvest(DataTable dt, string targetPlatform = "1111")
        {
            new ActInvest().SendBonusAfterInvest(dt, targetPlatform);
            new ActInvite().SendBonusForInviteAfterInvest(dt, targetPlatform);
        }
        #endregion

        /// <summary>
        /// 汇付返回成功,注册奖励发放
        /// </summary>
        /// <param name="registerid"></param>
        /// <param name="ActUser"></param>
        public void SendBonusAfterRegister(int registerid, string targetPlatform = "1111") //注册完成
        {
            //用户注册活动
            new ActRegister().SendBonusAfterRegister(registerid, targetPlatform, 0);
            //邀请用户注册奖励处理逻辑,暂无邀请奖励
            new ActInvite().SendBonusForInviteAfterRegister(registerid, targetPlatform, 0);
        }
        /// <summary>
        /// 邀请奖励活动-注册（建议实名后调用）
        /// </summary>
        /// <param name="registerid">用户id</param>
        /// <param name="ActUser">面向用户 0不限  1首次投资用户 2非首投用户 3每标首投用户 4每标最大投资用户 5所有投资用户</param>
        private void SendBonusForInviteAfterRegister(int registerid, string targetPlatform, int ActUser = 0)
        {
            new ActInvite().SendBonusForInviteAfterRegister(registerid, targetPlatform, 0);
        }
        /// <summary>
        /// 邀请奖励活动-投资
        /// </summary>
        /// <param name="registerid">用户id</param>
        /// <param name="ActUser">面向用户 0不限  1首次投资用户 2非首投用户 3每标首投用户 4每标最大投资用户 5所有投资用户</param>
        private void SendBonusForInviteAfterInvest(DataTable dt, string targetPlatform)
        {
            new ActInvite().SendBonusForInviteAfterInvest(dt, targetPlatform);
        }

        /// <summary>
        /// 根据ActID编号获取活动对象
        /// </summary>
        /// <param name="ActID"></param>
        /// <returns></returns>
        public hx_ActivityTable GetActivityModel(int ActID)
        {
            hx_ActivityTable res = (from item in ef.hx_ActivityTable
                                    where item.ActID == ActID
                                    select item).FirstOrDefault();
            return res;
        }

        public decimal SpecilBonus(int userid, int bid_records_id, decimal investAmt)
        {
            return new ActInvest().GetSepcialActAmt(userid, bid_records_id, investAmt);
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    System.GC.Collect();
                }
                ef.Dispose();
                ef = null;
                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        ~ActFacade()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion


        /// <summary>
        /// 登录送抵扣券（1.9-1.24）
        /// </summary>
        /// <param name="registerID">用户ID</param>
        public void LoginSendDKQ(int registerID)
        {
            var isChannel1 = (from item in ef.hx_member_table
                              join uInvited in
                              (from htu in ef.hx_td_Userinvitation
                               join peo in ef.hx_member_table
                               on htu.Invpeopleid equals peo.registerid
                               select new { useridentity = peo.useridentity, invpersonid = htu.invpersonid })
                              on item.registerid equals uInvited.invpersonid into lu
                              from luLeft in lu.DefaultIfEmpty()
                              join invest in (from hbr in ef.hx_Bid_records
                                              group hbr by hbr.investor_registerid into g
                                              select new { investor_registerid = g.Key, investCount = g.Count() })
                              on item.registerid equals invest.investor_registerid into investL
                              from investLeft in investL.DefaultIfEmpty()
                              where item.registerid == registerID
                              select new
                              {
                                  channel_invitedcode = item.channel_invitedcode,
                                  useridentity = luLeft == null ? -1 : luLeft.useridentity,//邀请人用户等级
                                  uidentity = item.useridentity,//被邀请人用户等级
                                  investCount = investLeft == null ? 0 : investLeft.investCount
                              });
            var isChannel = isChannel1.ToList();

            //var aa = isChannel1.Where(c => c.uidentity != 4);
            //var bb = isChannel1.Where(c => (c.useridentity != 4 || c.investCount >= 1));
            //var cc = isChannel1.Where(c => new ChannelAct().IsParticipateActivity(c.channel_invitedcode, c.investCount) == true);
            ////增加逻辑，如果用户为渠道用户，并且历史投资次数大于等于一次，则允许参与1260红包活动
            //if (isChannel.Where(c => c.investCount >= 1 || (string.IsNullOrWhiteSpace(c.channel_invitedcode) && c.useridentity != 4 && c.uidentity != 4)).Count() <= 0)
            //    return;
            //用户等级为渠道 不参与活动
            if (isChannel.Where(c => c.uidentity != 4).Count() <= 0)
            {
                return;
            }
            //老渠道机制判断 (推荐人等级为4渠道用户,投资次数大于等于1次 时可参与活动   
            if (isChannel.Where(c => (c.useridentity != 4 || c.investCount >= 1)).Count() <= 0)
                return;
            //新渠道机制判断              
            using (ChannelAct channelAct = new ChannelAct())
            {
                //按照渠道类型和投资次数判断是否参与此次活动 
                //IsParticipateActivity方法中是已投资后奖励发放为主，为兼容该类登录红包，投资次数默认+1后计算
                if (isChannel.Where(c => channelAct.IsParticipateActivity(c.channel_invitedcode, c.investCount + 1) == true).Count() <= 0)
                {
                    return;
                }
            }
            int sended = (from item in ef.hx_UserAct
                              //join user in ef.hx_member_table
                              //on item.registerid equals user.registerid
                          join act in ef.hx_ActivityTable
                          on item.ActID equals act.ActID
                          where item.registerid == registerID && act.ActName == "登录送1580元抵扣券"
                          select item.ActID).Count();
            if (sended > 0) return;
            new ActRegister().SendBonusAfterLogin(registerID, "1111", 5);//ActUser面向用户 首次投资用户=1，非首投用户=2 ，每标首投用户=3，每标最大投资用户=4，所有投资用户=5，续投用户=6 , 特殊复投用户=7
        }
    }
}
