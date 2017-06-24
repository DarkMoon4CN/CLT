using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuanglitouP2P.DAL.Api;
using ChuanglitouP2P.Model;
namespace ChuanglitouP2P.BLL.Api
{
    public class ContractLogic
    {
        private readonly ContractManagementDal _dal = new ContractManagementDal();

        public M_Contract_management GetContract(int contractId)
        {
            return _dal.GetContract(contractId);
        }

        public List<M_Contract_management> GetContractListForApp(int targetId, int bidRecordsId = 0, bool orderbyContractAsc = true)
        {
            return _dal.GetContractListForApp(targetId, bidRecordsId, orderbyContractAsc);
        }
    }
}
