#region 描述信息

/*-------------------------------------------------------------------------
 * <copyright>ControllerFactory ©2012 XieZhihui</copyright>
 * <author>XieZhihui<author>
 *<createdOn>2012/12/25 15:35:54</createdOn>
 * <ver>v1.0</ver>
 *  -------------------------------------------------------------------------*/

#endregion

using System.Web.Http.Dependencies;
using StructureMap;

namespace ChuangLiTouOpenApi.Factory.WebApiIoc
{
    /// <summary>
    ///     解决方案
    /// </summary>
    public class SmDependencyResolver : StructureMapScope, IDependencyResolver
    {
        private IContainer _container;

        public SmDependencyResolver(IContainer container)
            : base(container)
        {
            _container = container;
        }

        public IDependencyScope BeginScope()
        {
            _container = IocManage.Initialize();
            return new StructureMapScope(_container);
        }
    }
}