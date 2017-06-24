using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Response.AppUpdate
{
    public class AppDownloadInfo
    {
        /// <summary>
        /// 更新包编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 更新包文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 更新包版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 更新包描述信息
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 针对渠道
        /// </summary>
        public string Channel { get; set; }
        /// <summary>
        /// 更新包级别.1:接口变更-非常严重;2:业务增加-较严重;3:业务变更-严重;4:阶段性Bug修复-一般;5:日常更新-一般
        /// </summary>
        public int UpdateLevel { get; set; }
        /// <summary>
        /// 更新包校验码.默认MD5
        /// </summary>
        public string ValideCode { get; set; }
        /// <summary>
        /// 更新包是否可用。0:不可以更新，1:可以更新
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 更新包发布时间
        /// </summary>
        public string CreateTime { get; set; }
    }
}
