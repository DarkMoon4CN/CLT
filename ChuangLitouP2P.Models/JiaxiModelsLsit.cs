using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace ChuangLitouP2P.Models
{
    public class JiaxiModelsLsit
    {

        /// <summary>
        /// 正常加息券
        /// </summary>
        public PagedList<hx_UserAct> Jiaxi { get; set; }


        /// <summary>
        /// 使用的加息券
        /// </summary>
        public PagedList<hx_UserAct> JiaxiUses { get; set; }


        /// <summary>
        /// 过期的加息券
        /// </summary>
        public PagedList<hx_UserAct> JiaxiLost { get; set; }


    }
}
