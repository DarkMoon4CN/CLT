using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChuanglitouP2P.DAL;
using ChuangLitouP2P.Models;
using ChuanglitouP2P.Common;
namespace ChuanglitouP2P.BLL
{
    public class B_AppUpdatePackage : IDisposable
    {
        private readonly D_AppUpdatePackage dal = new D_AppUpdatePackage();

        public int Add(hx_AppUpdatePackage model)
        {
            return dal.Add(model);
        }

        public bool Update(int id, hx_AppUpdatePackage model)
        {
            return dal.Update(id, model);
        }

        public bool Delete(int id)
        {
            return dal.Delete(id);
        }

        public hx_AppUpdatePackage GetModel(string code)
        {
            return dal.GetModel(code);
        }

        public hx_AppUpdatePackage GetDownloadModel(string code)
        {
            return dal.GetDownloadModel(code);
        }

        public hx_AppUpdatePackage GetLastModel(string platform, string channel, string version)
        {
            var current = dal.GetModel(platform, channel, version);
            if (current != null)
            {
                var dependedModel = dal.GetDependedModel(platform, channel, version);
                if (dependedModel != null)
                    return dependedModel;
            }
            return dal.GetLastModel(platform, channel);
        }

        public List<hx_AppUpdatePackage> GetList(hx_AppUpdatePackage where)
        {
            return dal.GetList(where);
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

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~B_AppUpdatePackage() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

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
