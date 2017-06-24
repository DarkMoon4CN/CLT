using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.Member
{
    /// <summary>
    /// 会员注册实体类
    /// </summary>
    public class RequestRegistMember
    {
        /// <summary>
        /// 用户手机号(账号)
        /// </summary>
        public string userMobile { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string userPass { get; set; }
        /// <summary>
        /// 注册验证码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 来源：1-IOS 2-安卓 3-PC 4-WAP
        /// </summary>
        public int sourceFrom { get; set; }
    }
}
