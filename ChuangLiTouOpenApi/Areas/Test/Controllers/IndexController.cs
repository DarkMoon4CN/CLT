using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ChuangLiTou.Core.Entities.ChinaPnr;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Member;
using ChuangLiTou.Core.Entities.Request.Recharge;
using ChuangLiTou.Core.Entities.Request.Borrow;
using ChuanglitouP2P.Common;

namespace ChuangLiTouOpenApi.Areas.Test.Controllers
{
    public class IndexController : Controller
    {
        Dictionary<string, string> dic = new Dictionary<string, string>
        {
            {"appId","123456"},
            {"appSecret","123456"},
            {"timeStamp", "1477803469"}
        };
        readonly string  APPSAFECODE = "4A56BA9059D42BB07C72C9D368934FBD";
        public IndexController()
            : this(GlobalConfiguration.Configuration)
        {
         
        }
        public HttpConfiguration Configuration { get; private set; }


        private IndexController(HttpConfiguration configuration)
        {

            Configuration = configuration;
        }
        /// <summary>
        /// 实名认证测试接口
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult Index(string userId = "0")
        {
            var m = new RequestParam<RequestValidate>();
            m.body = new RequestValidate { userId = userId };
            string bodyStr = JsonHelper.Entity2Json(m.body);
            bodyStr = bodyStr.Replace("\n", "").Replace("\r", "").Replace(" ", "").Trim();
            string token = HttpHelper.GetAccessToken(dic, bodyStr, APPSAFECODE);
            string specialStamp = ChuanglitouP2P.Common.Utils.Base64Encoder(bodyStr);
            m.header = new RequestHeader { appId = 123456, appSecret = "123456", accessToken = token, timeStamp = "1477803469", specialStamp = specialStamp };

            
            return View(m);
        }
        
        /// <summary>
        /// 充值接口
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult Recharge(int userId = 0)
        {
            var m = new RequestParam<RequestRecharge>();
            m.body = new RequestRecharge { userId = 7, bankType = "CIB", amountOfCharge = 2000 };
            string bodyStr = JsonHelper.Entity2Json(m.body);
            bodyStr = bodyStr.Replace("\n", "").Replace("\r", "").Replace(" ","").Trim();
            string token = HttpHelper.GetAccessToken(dic, bodyStr, APPSAFECODE);
            string specialStamp= ChuanglitouP2P.Common.Utils.Base64Encoder(bodyStr);
            m.header = new RequestHeader { appId = 123456, appSecret = "123456", accessToken = token, timeStamp = "1477803469", specialStamp= specialStamp };
            return View(m);
        }

        /// <summary>
        /// 绑卡接口
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult BindCard(int userId = 0)
        {
            var m = new RequestParam<RequestRecharge>();
            m.body = new RequestRecharge { userId = 7 };

            string bodyStr = JsonHelper.Entity2Json(m.body);
            bodyStr = bodyStr.Replace("\n", "").Replace("\r", "").Replace(" ", "").Trim();
            string token = HttpHelper.GetAccessToken(dic, bodyStr, APPSAFECODE);
            string specialStamp = ChuanglitouP2P.Common.Utils.Base64Encoder(bodyStr);
            m.header = new RequestHeader { appId = 123456, appSecret = "123456", accessToken = token, timeStamp = "1477803469", specialStamp = specialStamp };

            return View(m);
        }


        /// <summary>
        /// 管理我的托管帐号
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult ThirdManage(int userId = 0)
        {
            var m = new RequestParam<RequestRecharge>();

            m.body = new RequestRecharge { userId = 7 };

            string bodyStr = JsonHelper.Entity2Json(m.body);
            bodyStr = bodyStr.Replace("\n", "").Replace("\r", "").Replace(" ", "").Trim();
            string token = HttpHelper.GetAccessToken(dic, bodyStr, APPSAFECODE);
            string specialStamp = ChuanglitouP2P.Common.Utils.Base64Encoder(bodyStr);

            m.header = new RequestHeader { appId = 123456, appSecret = "123456", accessToken = token, timeStamp = "1477803469", specialStamp = specialStamp };

            

            return View(m);
        }
        /// <summary>
        /// InvestSubmit
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult InvestSubmit(int userId = 0)
        {
            var m = new RequestParam<RequestTender>();
            m.body = new RequestTender { userId = 7, targetId = 12244, investAmount = 1000, addRateIds = "", bonusIds = "" };

            string bodyStr = JsonHelper.Entity2Json(m.body);
            bodyStr = bodyStr.Replace("\n", "").Replace("\r", "").Replace(" ", "").Trim();
            string token = HttpHelper.GetAccessToken(dic, bodyStr, APPSAFECODE);
            string specialStamp = ChuanglitouP2P.Common.Utils.Base64Encoder(bodyStr);
            m.header = new RequestHeader { appId = 123456, appSecret = "123456", accessToken = token, timeStamp = "1477803469", specialStamp = specialStamp };
            return View(m);
        }


        /// <summary>
        /// InvestSubmit
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult ImgUpload(int userId = 0)
        {
            var m = new RequestParam<RequestTender>();
            string token = HttpHelper.GetAccessToken(dic, null, APPSAFECODE);
            m.header = new RequestHeader { appId = 123456, appSecret = "123456", accessToken = "294b8a0fe2ffaa85243bdb2b4ff94fe9", timeStamp = "1477803469" };
            return View(m);
        }
    }
}