using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.Request
{
    /// <summary>
    /// 接口请求参数实体
    /// </summary>
    public class RequestParam<T>
    {
        /// <summary>
        /// 权限验证数据
        /// </summary>
        public RequestHeader header { get; set; }
        /// <summary>
        /// 业务参数
        /// </summary>
        public T body { get; set; }
    }

    public class RequestParam
    {
        /// <summary>
        /// 权限验证数据
        /// </summary>
        public RequestHeader header { get; set; }
    }
}
