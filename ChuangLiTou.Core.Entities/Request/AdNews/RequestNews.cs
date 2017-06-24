using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.AdNews
{
    /// <summary>
    /// 新闻参数实体
    /// </summary>
public    class RequestNews: RequestPage
    {
       
        /// <summary>
        /// 新闻类型编号
        /// </summary>
        public int newType { get; set; }


    }
}
