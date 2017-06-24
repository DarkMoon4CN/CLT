using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuanglitouP2P.Common.Util;

namespace ChuangLiTou.Core.Entities.P2Peye
{ 
    /// <summary>
    /// 接口响应借款类 xiezh
    /// </summary>
    public class ResponseP2PeyeLoan : P2PeyeBase
    {
        public Pagination<LoanEntity> loans { get; set; }
    }
}
