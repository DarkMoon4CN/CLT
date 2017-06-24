#region 描述信息

/*-------------------------------------------------------------------------
 * <copyright>ControllerFactoryConfiguration ©2012 XieZhihui</copyright>
 * <author>XieZhihui<author>
 *<createdOn>2012/12/25 16:44:11</createdOn>
 * <ver>v1.0</ver>
 *  -------------------------------------------------------------------------*/

#endregion

using System.Web.Mvc;
using StructureMap;

namespace ChuangLiTouOpenApi.Factory
{
    public class ControllerFactoryConfiguration
    {
        public static void Configure()
        {
            ObjectFactory.Initialize(InitializeStructureMap);
        }

        private static void InitializeStructureMap(IInitializationExpression x)
        {
            x.Scan(y =>
            {
                y.Assembly("ChuangLiTou.Core.IDal");
                y.WithDefaultConventions();
            });

            x.Scan(y =>
            {
                y.Assembly("ChuangLiTou.Core.ImplDal");
                y.WithDefaultConventions();
            });

            x.Scan(y =>
            {
                y.TheCallingAssembly();
                y.AddAllTypesOf<IController>()
                    .NameBy(type => type.FullName != null ? type.FullName.ToLowerInvariant() : null);
            });
        }
    }
}