using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WeiXin.Controllers
{

    [ActAuth]
    public class BaseController : Controller
    {
        public int CurrentUserId
        {
            get
            {
                return CurrentUser != null ? CurrentUser.userid : 0;
            }
        }

        public string CurrentUserName
        {
            get
            {
                return CurrentUser != null ? CurrentUser.username : "";
            }
        } 

        public M_login CurrentUser { get { return GetUser(); } }

        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <returns></returns>
        protected M_login GetUser()
        {
            if (System.Web.HttpContext.Current.Request.IsAuthenticated)//是否通过身份验证
            {
                HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];//获取cookie
                if (authCookie != null)
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);//解密
                    return SerializeHelper.Instance.JsonDeserialize<M_login>(ticket.UserData);//反序列化
                }
            }
            return null;
        }
       

    }
}