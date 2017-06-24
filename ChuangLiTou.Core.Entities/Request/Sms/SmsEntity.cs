using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.Sms
{
    public class SmsEntity
    {
        /// <summary>
        ///手机号
        /// </summary>
        /// <value>The mobile.</value>
        public string mobile { get; set; }
    }

    /// <summary>
    /// 绑定手机号
    /// </summary>
    public class RequestBindMobileEntity
    {
        /// <summary>
        ///手机号
        /// </summary>
        /// <value>The mobile.</value>
        public string mobile { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int userId { get; set; }
    }

}
