using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.P2Peye
{
    /// <summary>
    /// 接口响应投资类 xiezh
    /// </summary>
    public class ResponseP2PeyeInvestment : P2PeyeBase
    {
        public List<InvestmentEntity> loans { get; set; }
    }
}
