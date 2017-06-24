#region 描述信息

/*-------------------------------------------------------------------------
 * <copyright>ControllerFactory ©2012 XieZhihui</copyright>
 * <author>XieZhihui<author>
 *<createdOn>2012/12/25 15:35:54</createdOn>
 * <ver>v1.0</ver>
 *  -------------------------------------------------------------------------*/

#endregion

using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;
using ChuanglitouP2P.Common;
namespace ChuangLiTouOpenApi.Factory
{
    public class ControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            try
            {
                IController controller = null;
                if (controllerType == null)
                {
                    try
                    {
                        controller = base.GetControllerInstance(requestContext, controllerType);
                    }
                    catch (HttpException ex)
                    {
                        LoggerHelper.Error("ControllerFactory=>GetControllerInstance:" + ex);
                    }
                    if (controller != null)
                        LoggerHelper.Info(string.Format("{0} controller was created at {1}",
                            controller.GetType().FullName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                }
                else
                {
                    if (controllerType.FullName != null)
                    {
                        var key = controllerType.FullName.ToLowerInvariant();
                        controller = ObjectFactory.GetNamedInstance<IController>(key);
                        LoggerHelper.Info(string.Format("{0} controller was created by object factory at {1}", key,
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    }
                }
                return controller;
            }
            catch (StructureMapException e)
            {
                //Use the default logic.
                LoggerHelper.Error("StructureMapException" + e);
                throw;
            }
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            try
            {
                LoggerHelper.Info(string.Format("{0} controller is creating at {1},route url is {2}", controllerName,
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ((Route) requestContext.RouteData.Route).Url));

                var controllerType = GetControllerType(requestContext, controllerName);
                if (controllerType.FullName != null)
                {
                    var key = controllerType.FullName.ToLowerInvariant();
                    var iController = ObjectFactory.GetNamedInstance<IController>(key);
                    LoggerHelper.Info(
                        string.Format(
                            "{0} controller was finish at {1},reqeust route url is {2},response route url is {3}",
                            controllerName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            ((Route) requestContext.RouteData.Route).Url, iController));
                    return iController;
                }
                LoggerHelper.Error(string.Format("controllerType is null {0}-{1}", controllerName, requestContext));
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(string.Format("create {0} controller throw exception :", controllerName), ex);
                return base.CreateController(requestContext, controllerName);
            }

            return null;
        }
    }
}