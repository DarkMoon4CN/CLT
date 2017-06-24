using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.Member
{
    public class RequestValidateOrgMobileEntity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 短信验证码 获取验证码时 该参数可不传
        /// </summary>
        /// <value>The code.</value>
        public string code { get; set; }

    }
}
