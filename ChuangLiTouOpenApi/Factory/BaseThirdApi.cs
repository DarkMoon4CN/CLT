using System.Web.Http;
using System.Web.Http.Description;

namespace ChuangLiTouOpenApi.Factory
{
    /// <summary>
    ///     第三方接口权限验证基础类 xiezh
    /// </summary>
    [ThirdApiAuth]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BaseThirdApi : ApiController
    {
    }
}