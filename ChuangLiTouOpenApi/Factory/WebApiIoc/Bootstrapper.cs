#region 描述信息

/*-------------------------------------------------------------------------
 * <copyright>ControllerFactory ©2012 XieZhihui</copyright>
 * <author>XieZhihui<author>
 *<createdOn>2012/12/25 15:35:54</createdOn>
 * <ver>v1.0</ver>
 *  -------------------------------------------------------------------------*/

#endregion

using System.Web.Http;
using System.Web.Http.Description;

namespace ChuangLiTouOpenApi.Factory.WebApiIoc
{
    /// <summary>
    /// Class Bootstrapper.
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
 
    public static class Bootstrapper
    {
        /// <summary>
        /// Starts
        /// </summary>
        public static void Start()
        {
            var container = IocManage.Initialize();
            //  var container = ObjectFactory.Container;
            GlobalConfiguration.Configuration.DependencyResolver = new SmDependencyResolver(container);
        }
    }
}