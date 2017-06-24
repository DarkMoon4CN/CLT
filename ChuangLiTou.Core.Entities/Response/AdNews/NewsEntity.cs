using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Response.AdNews
{
    /// <summary>
    /// 新闻列表
    /// </summary>
  public  class NewsEntity
    {

       //"newid": 21,
       //     "web_Type_menu_id": 11,
       //     "News_title": "如果创利投不存在了，投资人的收益如何保障？",
       //     "News_Key": "创利投投资互联网金融p2p收益保障",
       //     "news_Des": "如果创利投不存在了，投资人的收益如何保障？",
       //     "context": "创利投作为信息中介平台，提供的是居间服务。即使平台不存在，投资人与借款人的债权债务关系依然有效，且担保公司会负责投资人的全额本息担保，而第三方支付平台依然保证还本付息的资金会在项目到期日自动划转到投资人在创利投绑定的银行账户中。",
       //     "createtime": "2015-03-11 10: 57: 25",
       //     "adminuserid": 1,
       //     "menu_name": "认证安全",
       //     "parentid": 2,
       //     "rootid": 2,
       //     "path1": "Certification",
       //     "orderid": 3,
       //     "topmenuname": "帮助中心",
       //     "comm": 0,
       //     "listcomm": 0

        /// <summary>
        /// 新闻Id
        /// </summary>        
        [JsonProperty("newId", NullValueHandling = NullValueHandling.Ignore)]
        public int? newid { get; set; }

        /// <summary>
        /// web_Type_menu_id
        /// </summary>        
        [JsonProperty("newsType", NullValueHandling = NullValueHandling.Ignore)]
        public int? web_Type_menu_id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>        
        [JsonProperty("newsTitle", NullValueHandling = NullValueHandling.Ignore)]
        public string News_title { get; set; }

        /// <summary>
        /// News_Key
        /// </summary>        
        [JsonProperty("News_Key", NullValueHandling = NullValueHandling.Ignore)]
        public string News_Key { get; set; }

        /// <summary>
        /// 新闻描述
        /// </summary>        
        [JsonProperty("newsDesc", NullValueHandling = NullValueHandling.Ignore)]
        public string news_Des { get; set; }

        /// <summary>
        /// 内容
        /// </summary>        
        [JsonProperty("newContext", NullValueHandling = NullValueHandling.Ignore)]
        public string context { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>        
        [JsonProperty("createdOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? createtime { get; set; }

        /// <summary>
        /// adminuserid
        /// </summary>        
        [JsonProperty("adminuserid", NullValueHandling = NullValueHandling.Ignore)]
        public int? adminuserid { get; set; }

        /// <summary>
        /// menu_name
        /// </summary>        
        [JsonProperty("menu_name", NullValueHandling = NullValueHandling.Ignore)]
        public string menu_name { get; set; }

         [JsonIgnore]
        public int? parentid { get; set; }

        /// <summary>
        /// rootid
        /// </summary>        
        [JsonProperty("rootid", NullValueHandling = NullValueHandling.Ignore)]
        public int? rootid { get; set; }

        /// <summary>
        /// path1
        /// </summary>        
        [JsonProperty("urlPath", NullValueHandling = NullValueHandling.Ignore)]
        public string path1 { get; set; }

        /// <summary>
        /// orderid
        /// </summary>        
        [JsonProperty("orderid", NullValueHandling = NullValueHandling.Ignore)]
        public int? orderid { get; set; }

        /// <summary>
        /// topmenuname
        /// </summary>        
        [JsonProperty("topmenuname", NullValueHandling = NullValueHandling.Ignore)]
        public string topmenuname { get; set; }

        /// <summary>
        /// comm
        /// </summary>        
        [JsonProperty("comm", NullValueHandling = NullValueHandling.Ignore)]
        public int? comm { get; set; }

        /// <summary>
        /// listcomm
        /// </summary>        
        [JsonProperty("listcomm", NullValueHandling = NullValueHandling.Ignore)]
        public int? listcomm { get; set; }

        /// <summary>
        /// newimg
        /// </summary>        
        [JsonProperty("newImgUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string newimg { get; set; }   



        public string jumpUrl { get; set; }
				       
    }
}
