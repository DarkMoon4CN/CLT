using ChuanglitouP2P.Common;
using ChuangLitouP2P.Models;
using System;
using System.Linq;

namespace ChuanglitouP2P.BLL
{
    public  class ChannelAct : IDisposable
    {
        chuangtouEntities ef = new chuangtouEntities();
        /// <summary>
        /// 新渠道机制下区分 渠道类型,不同渠道类型可参加活动不一样
        /// </summary>
        /// <param name="channelInviteCode">渠道邀请码</param>
        /// <param name="investNum">被邀请投资次数</param>
        /// <returns>true 参加活动</returns>
        public  bool IsParticipateActivity(string channelInviteCode,int investNum=0)
        {
            if (string.IsNullOrWhiteSpace(channelInviteCode))
            {
                return true;
            }
            string cacheName = "hx_Channel_" + channelInviteCode;
            hx_Channel channelEnitty = (hx_Channel)Utils.GetThirdCacheObject(cacheName);
            if (channelEnitty == null)
            {
                channelEnitty = ef.hx_Channel.AsNoTracking().Where(p => p.Invitedcode == channelInviteCode).FirstOrDefault();
                Utils.SetThirdCacheName(cacheName,channelEnitty,5);
            }
            if (channelEnitty == null)
            {
                return false;
            }else
            {
                if (channelEnitty.type == "cps1")//渠道cps只结算首投,复投可参与所有活动
                {
                    if (investNum <= 1) { return false; }
                    else { return true; }
                }
                else  if (channelEnitty.type == "cps2")//渠道cps2结算首投及复投,所有投资不参加活动
                {
                    return false;
                }
                else if (channelEnitty.type == "cpc")//cpc渠道结算与投资无关,参与所有活动
                {
                    return true;
                }
            }
            return false;
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
        ~ChannelAct()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(false);
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
    }
}
