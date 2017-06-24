using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Response.Invest
{
    /// <summary>
    /// 返回累计邀请信息
    /// </summary>
    public class InvestCountEntity
    {
        /// <summary>
        /// 获取累计邀请注册人数
        /// </summary>
        public int RegisterCount { get; set; }

        /// <summary>
        /// 获取累计邀请投资人数
        /// </summary>
        public int InvestCount { get; set; }

        /// <summary>
        /// 生产二维码链接地址
        /// </summary>
        public string strLink { get; set; }
    }
}
