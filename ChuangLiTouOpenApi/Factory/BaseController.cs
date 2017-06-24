using System.Web.Mvc;

namespace ChuangLiTouOpenApi.Factory
{
    [ControllerAuth]
    public class BaseController : Controller
    {
        protected virtual void AppendDeviceFlag(string deviceKey, ref string host)
        {
            if (deviceKey == "123456")//IOS
                host = host + "_3";
            else if (deviceKey == "654321")//Android
                host = host + "_7";
            else host = host + "_0";
        }

        protected virtual string PickoutDeviceFlag(ref string host)
        {
            string[] temp = host.Split('_');
            host = temp[0];
            if (temp[1] == "3")
                return "123456";
            if (temp[1] == "7")
                return "654321";
            return string.Empty;
        }
    }
}