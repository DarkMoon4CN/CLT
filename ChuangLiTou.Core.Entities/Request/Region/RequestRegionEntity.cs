using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.Region
{
    /// <summary>
    /// Class RequestRegionEntity.
    /// </summary>
    public class RequestRegionEntity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        /// <value>The user identifier.</value>
        public int userId { get; set; }
        /// <summary>
        /// 省份id
        /// </summary>
        /// <value>The province identifier.</value>
        public int provinceId { get; set; }
        /// <summary>
        /// 市id
        /// </summary>
        /// <value>The city identifier.</value>
        public int cityId { get; set; }
        /// <summary>
        /// 县id
        /// </summary>
        /// <value>The county identifier.</value>
        public int countyId { get; set; }

        /// <summary>
        /// 详细地址.
        /// </summary>
        /// <value>The detail address.</value>
        public string detailAddress { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string zipCode { get; set; }
    }
}
