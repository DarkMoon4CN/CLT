using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChuangLitouP2P.Models;
namespace ChuanglitouP2P.DAL
{
    public class D_AppUpdatePackage
    {
        chuangtouEntities ef = new chuangtouEntities();
        public int Add(hx_AppUpdatePackage model)
        {
            if (string.IsNullOrWhiteSpace(model.Code))
                model.Code = Guid.NewGuid().ToString().Replace("-", "");
            ef.hx_AppUpdatePackage.Add(model);
            return ef.SaveChanges();
        }

        public bool Update(long id, hx_AppUpdatePackage model)
        {
            var query = from data in ef.hx_AppUpdatePackage
                        where data.id == id
                        select data;
            var tmp = query.ToList().FirstOrDefault();
            if (tmp != null)
            {
                tmp.Channel = model.Channel;
                tmp.DependCode = model.DependCode;
                tmp.Description = model.Description;
                tmp.DownloadCount = model.DownloadCount;
                tmp.IsEnable = model.IsEnable;
                tmp.UpdateLevel = model.UpdateLevel;
                tmp.ValideCode = model.ValideCode;
                tmp.Version = model.Version;
                tmp.VirtualPath = model.VirtualPath;
                tmp.Ways = model.Ways;
                ef.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Delete(long id)
        {
            var query = from data in ef.hx_AppUpdatePackage
                        where data.id == id
                        select data;
            var tmp = query.ToList().FirstOrDefault();
            if (tmp != null)
            {
                ef.hx_AppUpdatePackage.Remove(tmp);
                ef.SaveChanges();
            }
            return true;
        }

        public hx_AppUpdatePackage GetModel(int id)
        {
            var query = from data in ef.hx_AppUpdatePackage
                        where data.id == id
                        select data;
            var tmp = query.ToList().FirstOrDefault();
            return tmp;
        }
        public hx_AppUpdatePackage GetModel(string platform, string channel, string version)
        {
            var query = from data in ef.hx_AppUpdatePackage
                        where data.Platform == platform && data.Channel == channel && data.Version == version
                        select data;
            var tmp = query.ToList().FirstOrDefault();
            return tmp;
        }
        public hx_AppUpdatePackage GetDependedModel(string platform, string channel, string version)
        {
            var query = from data in ef.hx_AppUpdatePackage
                        where data.Platform == platform && data.Channel == channel && data.Version == version
                        select data;
            var tmp = query.ToList().FirstOrDefault();
            if (tmp != null)
            {
                query = from data in ef.hx_AppUpdatePackage
                        where data.Platform == platform && data.Channel == channel && data.DependCode == tmp.Code
                        select data;
                tmp = query.ToList().FirstOrDefault();
            }
            return tmp;
        }
        public hx_AppUpdatePackage GetLastModel(string platform, string channel)
        {
            var query = from data in ef.hx_AppUpdatePackage
                        where data.Platform == platform && data.Channel == channel
                        orderby data.CreateTime descending
                        select data;
            return query.ToList().FirstOrDefault();
        }
        public hx_AppUpdatePackage GetModel(string code)
        {
            var query = from data in ef.hx_AppUpdatePackage
                        where data.Code == code
                        select data;
            return query.ToList().FirstOrDefault();
        }

        public hx_AppUpdatePackage GetDownloadModel(string code)
        {
            var query = from data in ef.hx_AppUpdatePackage
                        where data.Code == code
                        select data;
            var item = query.ToList().FirstOrDefault();
            if (item != null)
            {
                item.DownloadCount = item.DownloadCount + 1;
                ef.SaveChanges();
            }
            return item;
        }

        public List<hx_AppUpdatePackage> GetList(hx_AppUpdatePackage model)
        {
            long? id = null;
            if (model.id > 0) id = model.id;
            int? updateLevel = null;
            if (model.UpdateLevel > 0) updateLevel = model.UpdateLevel;

            return GetList(id, model.Code, model.Platform, model.Version, updateLevel, model.Description, model.DependCode, model.Channel, model.ValideCode, model.VirtualPath, model.Ways, model.IsEnable, null, null, null);
        }

        public List<hx_AppUpdatePackage> GetList(
            long? id, string code, string platform, string version, int? updateLevel, string description, string dependCode, string channel, string valideCode, string virtualPath, string ways, int? isEnable, long? downloadCount, DateTime? createTimeFrom, DateTime? createTimeTo)
        {
            var query = from data in ef.hx_AppUpdatePackage select data;
            if (id != null)
                query = query.Where(q => q.id == id.Value);
            if (!string.IsNullOrWhiteSpace(code))
                query = query.Where(q => q.Code == code);
            if (!string.IsNullOrWhiteSpace(platform))
                query = query.Where(q => q.Platform == platform);
            if (!string.IsNullOrWhiteSpace(version))
                query = query.Where(q => q.Version == version);
            if (updateLevel != null)
                query = query.Where(q => q.UpdateLevel == updateLevel.Value);
            if (!string.IsNullOrWhiteSpace(description))
                query = query.Where(q => q.Description == description);
            if (!string.IsNullOrWhiteSpace(dependCode))
                query = query.Where(q => q.DependCode == dependCode);
            if (!string.IsNullOrWhiteSpace(channel))
                query = query.Where(q => q.Channel == channel);
            if (!string.IsNullOrWhiteSpace(valideCode))
                query = query.Where(q => q.ValideCode == valideCode);
            if (!string.IsNullOrWhiteSpace(virtualPath))
                query = query.Where(q => q.VirtualPath == virtualPath);
            if (!string.IsNullOrWhiteSpace(ways))
                query = query.Where(q => q.Ways == ways);
            if (isEnable != null)
                query = query.Where(q => q.IsEnable == isEnable.Value);
            if (downloadCount != null)
                query = query.Where(q => q.DownloadCount == downloadCount.Value);
            if (createTimeFrom != null && createTimeTo != null)
                query = query.Where(q => q.CreateTime >= createTimeFrom.Value && q.CreateTime <= createTimeTo.Value);
            else if (createTimeFrom != null && createTimeTo == null)
                query = query.Where(q => q.CreateTime >= createTimeFrom.Value);
            else if (createTimeFrom == null && createTimeTo != null)
                query = query.Where(q => q.CreateTime <= createTimeTo.Value);
            return query.ToList();
        }
    }
}
