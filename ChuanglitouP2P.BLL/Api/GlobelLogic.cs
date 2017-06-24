using ChuangLiTou.Core.Entities.Request;
using ChuanglitouP2P.Model.Invest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.BLL.Api
{
    public class GlobelLogic
    {
        public GlobelInvestStatistics PlatformStatistics(RequestParam reqst)
        {
            GlobelInvestStatistics result = new GlobelInvestStatistics();
            result.InvestTotalAmount = ChuanglitouP2P.BLL.B_usercenter.GetALLFinance().ToString();// + "元";
            result.InvestTotalPeople = ChuanglitouP2P.BLL.B_usercenter.GetInvestment().ToString();// + "人";
            return result;
        }
    }
}
