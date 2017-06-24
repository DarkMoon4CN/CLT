using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.AppUpdate
{
    public class RequestVersion
    {
        /// <summary>
        /// 平台标识
        /// </summary>
        public string Platform { get; set; }
        /// <summary>
        /// 渠道
        /// </summary>
        public string Channel { get; set; }
        /// <summary>
        /// 设备当前版本号
        /// </summary>
        public string DeviceVersion { get; set; }
    }
}
