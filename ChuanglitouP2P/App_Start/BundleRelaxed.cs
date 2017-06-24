using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Optimization;

namespace ChuanglitouP2P.App_Start
{
    public class BundleRelaxed : Bundle
    {
        public BundleRelaxed(string virtualPath)
            : base(virtualPath)
        {
        }

        public new BundleRelaxed IncludeDirectory(string directoryVirtualPath, string searchPattern, bool searchSubdirectories)
        {
            var truePath = HostingEnvironment.MapPath(directoryVirtualPath);
            if (truePath == null) return this;

            var dir = new System.IO.DirectoryInfo(truePath);
            if (!dir.Exists || dir.GetFiles(searchPattern).Length < 1) return this;

            base.IncludeDirectory(directoryVirtualPath, searchPattern);
            return this;
        }

        public new BundleRelaxed IncludeDirectory(string directoryVirtualPath, string searchPattern)
        {
            return IncludeDirectory(directoryVirtualPath, searchPattern, false);
        }
    }
}