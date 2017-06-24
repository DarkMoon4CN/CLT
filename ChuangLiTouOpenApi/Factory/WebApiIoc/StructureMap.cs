#region 描述信息

/*-------------------------------------------------------------------------
 * <copyright>ControllerFactory ©2012 XieZhihui</copyright>
 * <author>XieZhihui<author>
 *<createdOn>2012/12/25 15:35:54</createdOn>
 * <ver>v1.0</ver>
 *  -------------------------------------------------------------------------*/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using StructureMap;

namespace ChuangLiTouOpenApi.Factory.WebApiIoc
{
    /// <summary>
    ///     StructureMap 服务类
    /// </summary>
    public class StructureMapScope : IDependencyScope
    {
        protected IContainer Container;

        public StructureMapScope(IContainer container)
        {
            Container = container;
        }

        public void Dispose()
        {
            var disposable = (IDisposable) Container;
            if (disposable != null)
            {
                disposable.Dispose();
            }
            Container = null;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                return null;
            }
            try
            {
                if (serviceType.IsAbstract || serviceType.IsInterface)
                    return Container.TryGetInstance(serviceType);

                return Container.GetInstance(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.GetAllInstances<object>().Where(s => s.GetType() == serviceType);
        }
    }
}