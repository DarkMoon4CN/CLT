using ChuangLiTou.Core.Entities;
using ChuangLiTou.Core.Entities.ProEnt;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Response;
using ChuanglitouP2P.DAL.Api;

namespace ChuanglitouP2P.BLL.Api
{
    public class OpenApiAuthorizationLogic
    {
        private readonly OpenApiAuthorizationDal _dal = new OpenApiAuthorizationDal();

        public AuthEntity SelectAppAuthorInforById(int id)
        {
            return _dal.SelectAppAuthorInforById(id);
        }
    }
}
