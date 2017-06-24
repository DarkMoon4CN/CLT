using System.Web.Mvc;
using ChuangLiTouOpenApi.Factory;

namespace ChuangLiTouOpenApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AppHandleErrorAttribute());
        }
    }
}
