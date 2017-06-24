#region 描述信息
/*-------------------------------------------------------------------------
 * <copyright>DataAccess ©2012 XieZhihui</copyright>
 * <author>XieZhihui<author>
 *<createdOn>2012/12/24 23:22:40</createdOn>
 * <ver>v1.0</ver>
 *  -------------------------------------------------------------------------*/
#endregion

using System;
using System.Reflection;
using ChuanglitouP2P.Common;
namespace ChuangLiTou.Core.Helpers
{
    /// <summary>
    /// 抽象工厂模式创建DAL。
    /// web.config 需要加入配置：(利用工厂模式+反射机制+缓存机制,实现动态创建不同的数据层对象接口)  
    /// </summary>
    public sealed class DataAccess
    {
        private static readonly string AssemblyPath = Settings.Instance.AssemblyNameSpace;
        /// <summary>
        /// 创建对象或从缓存获取
        /// </summary>
        public static object CreateObject(string assemblyPath, string classNamespace)
        {
            object objType = CacheHelper.GetCache(classNamespace);//从缓存读取
            if (objType == null)
            {
                try
                {
                    objType = Assembly.Load(AssemblyPath).CreateInstance(classNamespace);//反射创建
                    CacheHelper.SetCache(classNamespace, objType);// 写入缓存
                }
                catch (Exception ex)
                {
                    LoggerHelper.Error("assembly error:" + assemblyPath + "  " + classNamespace, ex);

                }
            }
            return objType;
        }
        /// <summary>
        /// 接口反射
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <param name="className">实现类 类名</param>
        /// <returns></returns>
        public static T CreateAssembly<T>(string className)
        {
            string classNamespace = AssemblyPath + "." + className;
            object objType = CreateObject(AssemblyPath, classNamespace);
            return (T)objType;
        }
    }
}