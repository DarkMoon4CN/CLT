///////////////////////////////////////////////////////////
//Name:IOC模型-默认IOC实体类
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using StructureMap;
namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// 默认IOC实体类
    /// </summary>
    public class IocManager
    {
        public static void Regist(System.Type interfase, object instance)
        {
            ObjectFactory.Inject(interfase, instance);
        }

        public static T Get<T>()
        {
            return ObjectFactory.GetInstance<T>();
        }
    }
}
