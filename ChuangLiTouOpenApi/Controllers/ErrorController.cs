using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using ChuanglitouP2P.Common;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Borrow;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Borrow;
using ChuanglitouP2P.Model.Invest;
using ChuangLiTouOpenApi.Factory;

namespace ChuangLiTouOpenApi.Controllers
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController:ApiController
    { 
        [HttpGet]
        public ResultInfo<string> Index()
        {
            var ri = new ResultInfo<string>("99999"); 
            ri.code = "500";
            ri.message = Settings.Instance.GetErrorMsg(ri.code);
            return ri;
        } 
    }
}