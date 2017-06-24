using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.Response.Borrow
{
    public class BorrowDetailEntity
    {
        /// <summary>
        /// 标的详细id
        /// </summary>
        [JsonProperty("detailId", NullValueHandling = NullValueHandling.Ignore)]
        public int target_detailed_id { get; set; }
        /// <summary>
        /// 借款人id
        /// </summary>
        [JsonProperty("borrowerRegisterId", NullValueHandling = NullValueHandling.Ignore)]
        public int borrower_registerid { get; set; }
        /// <summary>
        /// 标的id
        /// </summary>
        [JsonProperty("targetId", NullValueHandling = NullValueHandling.Ignore)]
        public int targetid { get; set; }
        /// <summary>
        /// 项目详情
        /// </summary>
        [JsonProperty("itemDetail", NullValueHandling = NullValueHandling.Ignore)]
        public string item_details { get; set; }
        /// <summary>
        /// 借款人情况
        /// </summary>
        [JsonProperty("borrowerCircumstances", NullValueHandling = NullValueHandling.Ignore)]
        public string borrower_circumstances { get; set; }
        /// <summary>
        /// 借款人基础材料
        /// </summary>
        [JsonProperty("borrowerBaseMaterial", NullValueHandling = NullValueHandling.Ignore)]
        public string borrower_base_material { get; set; }
        /// <summary>
        /// 资金用途
        /// </summary>
        [JsonProperty("useFunds", NullValueHandling = NullValueHandling.Ignore)]
        public string use_funds { get; set; }
        /// <summary>
        /// 创利投独立意见
        /// </summary>
        [JsonProperty("independentAdvice", NullValueHandling = NullValueHandling.Ignore)]
        public string independent_advice { get; set; }
        /// <summary>
        /// 担保机构意见
        /// </summary>
        [JsonProperty("guaranteeAgencyViews", NullValueHandling = NullValueHandling.Ignore)]
        public string guarantee_agency_views { get; set; }
        /// <summary>
        /// 风险控制措施
        /// </summary>
        [JsonProperty("riskControlMeasures", NullValueHandling = NullValueHandling.Ignore)]
        public string risk_control_measures { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [JsonProperty("createdOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime createtime { get; set; }

        /// <summary>
        /// 担保方图片
        /// </summary>      
         [JsonProperty("pictures", NullValueHandling = NullValueHandling.Ignore)]
        public List<BorrowGuarantorPictureEntity> pictures { get; set; }

    }
}
