using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.Member
{
    /// <summary>
    /// Class RequestMemberInvest.
    /// </summary>
    public class RequestMemberInvest:RequestPage
    {
      /// <summary>
      /// 用户ID
      /// </summary>
        public int userId { get; set; }
    }
}
