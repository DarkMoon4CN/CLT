using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuangLiTou.Core.Entities.P2Peye;
using ChuangLiTou.Core.Entities.ttnz;
using ChuanglitouP2P.Common.Util;
using ChuangLiTou.Core.Entities.ChinaPnr;
using ChuanglitouP2P.DAL.Api;
using ChuanglitouP2P.Model;
namespace ChuanglitouP2P.BLL.Api
{
    public class RechargeHistoryLogic
    {
        private readonly RechargeHistoryDal _dal = new RechargeHistoryDal();

        public int Add(M_Recharge_history model)
        {
            return _dal.Add(model);

        }
    }
}
