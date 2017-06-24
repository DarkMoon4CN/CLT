using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Request.Bonus
{
    public class RequestOwnBonus : RequestPage
    {
        /// <summary>
        ///用户id
        /// </summary> 
        public int userId { get; set; }
    }
}
