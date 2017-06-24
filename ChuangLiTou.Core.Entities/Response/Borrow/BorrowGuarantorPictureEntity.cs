using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.Response.Borrow
{
    /// <summary>
    /// 借款方关图片及担保方图片
    /// </summary>
    public class BorrowGuarantorPictureEntity
    {
        /// <summary>
        /// 自增图片id
        /// </summary>
        [JsonProperty("pictureId", NullValueHandling = NullValueHandling.Ignore)]
        public int? borrower_guarantor_picture_id { get; set; }
        /// <summary>
        /// 借款人id
        /// </summary>
        [JsonProperty("borrowerRegisterId", NullValueHandling = NullValueHandling.Ignore)]
        public int? borrower_registerid { get; set; }
        /// <summary>
        /// 标的id
        /// </summary>
        [JsonProperty("targetId", NullValueHandling = NullValueHandling.Ignore)]
        public int? targetid { get; set; }
        /// <summary>
        /// 借款方图片类型   1基础材料 2担保材料 3现场图片
        /// </summary>
        [JsonProperty("pictureType", NullValueHandling = NullValueHandling.Ignore)]
        public int? type_picture { get; set; }
        /// <summary>
        /// 图片路径
        /// </summary>
        [JsonProperty("picturePath", NullValueHandling = NullValueHandling.Ignore)]
        public string picture_path { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        [JsonProperty("pictureName", NullValueHandling = NullValueHandling.Ignore)]
        public string picture_name { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        [JsonProperty("createdOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? uploadtime { get; set; }     
    }
}
