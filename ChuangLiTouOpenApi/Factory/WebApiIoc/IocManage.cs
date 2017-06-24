#region 描述信息

/*-------------------------------------------------------------------------
 * <copyright>ControllerFactory ©2012 XieZhihui</copyright>
 * <author>XieZhihui<author>
 *<createdOn>2012/12/25 15:35:54</createdOn>
 * <ver>v1.0</ver>
 *  -------------------------------------------------------------------------*/

#endregion

using System.Web.Http.Description;
using StructureMap;

namespace ChuangLiTouOpenApi.Factory.WebApiIoc
{
    /// <summary>
    ///     IOC初始化方法
    /// </summary>

    [ApiExplorerSettings(IgnoreApi = true)]
    public static class IocManage
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x => x.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            }));
            return ObjectFactory.Container;
        }
    }
}