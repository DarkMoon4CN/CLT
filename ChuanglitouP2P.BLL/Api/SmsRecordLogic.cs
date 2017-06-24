using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuangLiTou.Core.Entities.Response.SmsEmail;
using ChuanglitouP2P.DAL.Api;
namespace ChuanglitouP2P.BLL.Api
{
    public class SmsRecordLogic
    {

        private readonly SmsRecordDal _dal = new SmsRecordDal();

        public Int32 AddRecord(SmsRecordEntity srEntity)
        {
            return _dal.AddRecord(srEntity);

        }
        /// <summary>
        /// 验证当前IP是否在1分钟之内操作过.
        /// </summary>
        /// <param name="clientIp">The client ip.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CheckInOneMinute(string clientIp, int type1, int type2)
        { 
            return _dal.CheckInOneMinute(clientIp, type1, type2);
        }

        /// <summary>
        /// 同一IP 同种类型发送的次数.
        /// </summary>
        /// <param name="clientIp">The client ip.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <returns>System.Int32.</returns>
        public int CheckIpSendTimes(string clientIp, int t1, int t2)
        {

            return _dal.CheckIpSendTimes(clientIp, t1, t2);
        }

        /// <summary>
        /// 获取3分钟之内 历史发送记录.
        /// </summary>
        /// <param name="mobile">The client ip.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <returns>SmsRecordEntity.</returns>
        public SmsRecordEntity SelectHistory(string mobile, int t1, int t2)
        {

            return _dal.SelectHistory(mobile, t1, t2);

        }

        /// <summary>
        /// 更新发送记录.
        /// </summary>
        /// <param name="ent">The ent.</param>
        public void UpdateRecord(SmsRecordEntity ent)
        {

            _dal.UpdateRecord(ent);

        }


        /// <summary>
        /// 验证code合法性
        /// </summary>
        /// <param name="code">v</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public bool CheckCode(string code, int type, string mobile)
        {
            return _dal.CheckCode(code, type,mobile);
        
        }


    }
}
