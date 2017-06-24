using ChuanglitouP2P.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeiXin.Controllers
{
    public class XiCaiChannelController : Controller
    {
        private string _webp = ConfigurationManager.AppSettings["webp"].ToString();
        XiCaiHelper xicai;
        public XiCaiChannelController()
        {
            xicai = new XiCaiHelper(_webp);
        }
        //    // GET: XiCaiChannel
        //public ActionResult Index()
        //{
        //    return View();
        //}
        //[HttpGet]
        //public ActionResult get_p2p(string pid, string client_secret)
        //{
        //    string res = xicai.get_p2p(pid, client_secret);
        //    return Content(res);
        //}
        [HttpGet]
        public ActionResult AutoRegisterLogin(string sign)
        {
            XiCaiHelper.register reg = new registerController().Register;
            XiCaiHelper.loginIn log = new loginController().LoginIN;
            string res = xicai.AutoRegisterLogin(sign, Response, reg, Request, log);
            if (res.StartsWith("RedictXXX"))
            {
                return Redirect(res.Replace("RedictXXX", ""));
            }
            return Content(res);
        }
        //[HttpGet]
        //public ActionResult Tongji_User(string t = "1", string token = "", string startdate = "", string enddate = "", int page = 1, int pagesize = 10)
        //{
        //    string res = xicai.Tongji_User(t, token, startdate, enddate, page, pagesize);
        //    return Content(res);
        //}
        //[HttpGet]
        //public ActionResult Tongji_Invest(string t = "1", string token = "", string startdate = "", string enddate = "", int page = 1, int pagesize = 10)
        //{
        //    string res = xicai.Tongji_Invest(Request, t, token, startdate, enddate, page, pagesize);
        //    return Content(res);
        //}
    }
}