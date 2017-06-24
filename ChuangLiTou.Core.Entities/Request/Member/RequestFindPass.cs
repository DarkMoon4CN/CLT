using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.Member
{
    /// <summary>
    /// 找回密码请求实体类
    /// </summary>
    public class RequestFindPass
    {
        /// <summary>
        /// 手机号（登录账号）
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 找回密码短信验证码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string newPwd { get; set; }

    }
}
