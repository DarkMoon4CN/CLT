using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.Member
{
    public class RequestMemberBonus
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int userId { get; set; }
        /// <summary>
        /// 已选中的id 红包格式：b_id1,id2 加息券格式：r_id1,id2
        /// </summary>
        public string selectedIds { get; set; }
    }
}
