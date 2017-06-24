using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.Member
{
    /// <summary>
    /// 
    /// </summary>
    public class RequestValidate
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string userIdNo { get; set; }
    }
}
