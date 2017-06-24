using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChuangLitouP2P.Models;
using static ChuangLitouP2P.Models.chuangtouEntities;
namespace ChuanglitouP2P.BLL.Api
{
    /// <summary>
    /// 邀请关联信息业务逻辑类
    /// </summary>
    public class InvitationLogic : IDisposable
    {
        chuangtouEntities ef = new chuangtouEntities();
        /// <summary>
        /// 获取用户的被邀请信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public hx_td_Userinvitation GetUserInvited(int userId)
        {
            var query = from data in ef.hx_td_Userinvitation
                        where data.invpersonid == userId
                        orderby data.invitationid ascending
                        select data;
            return query.ToList().FirstOrDefault();
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
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~InvitationLogic() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        void IDisposable.Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
