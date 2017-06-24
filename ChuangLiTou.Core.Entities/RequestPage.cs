using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities
{
    /// <summary>
    /// 分页实体
    /// </summary>
   public  class RequestPage
    {
       /// <summary>
        /// 请求的页码
       /// </summary>
       public int pageIndex { get; set; }
       /// <summary>
       /// 每页记录条数
       /// </summary>
       public int pageSize { get; set; }
    }
}
