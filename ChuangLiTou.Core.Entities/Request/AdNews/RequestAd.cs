using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.AdNews
{
    /// <summary>
    /// 获取广告参数实体
    /// </summary>
  public  class RequestAd
    {
        /// <summary>
        /// 获取条数
        /// </summary>
        public int top { get; set; }
        /// <summary>
        /// 广告列表编号
        /// </summary>
        public int adtypeId { get; set; }

   
    }
}
