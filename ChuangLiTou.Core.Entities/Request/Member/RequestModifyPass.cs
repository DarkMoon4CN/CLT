using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.Member
{
    /// <summary>
    /// 修改密码请求实体类
    /// </summary>
    public class RequestModifyPass
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 原始密码
        /// </summary>
        public string orgPwd { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string newPwd { get; set; }

    }
}
