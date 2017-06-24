using ChuanglitouP2P.Model.Invest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChuangLitouP2P.Models;

namespace ChuanglitouP2P.BLL.Api
{
    public class GlobalLogic : IDisposable
    {
        chuangtouEntities ef = new chuangtouEntities();
        public GlobalNotificationMarks GetGlobalNotificationMarks(GlobalNotificationMarksRequest request)
        {
            DateTime tempDt = DateTime.MinValue;
            GlobalNotificationMarks result = new GlobalNotificationMarks();
            if (DateTime.TryParse(request.referenceDateTime, out tempDt))
            {
                if (tempDt < Convert.ToDateTime("2016-12-01"))
                    return result;
                var query = from data in ef.hx_UserAct
                            where data.Createtime > tempDt && data.registerid == request.userId && data.RewTypeID > 1
                            select data;
                var list = query.ToList();
                if (list.Count > 0)
                    result.BonusHasChanged = true;
                return result;
            }
            return null;
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
                    GC.Collect();
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        ~GlobalLogic()
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
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
