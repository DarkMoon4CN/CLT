using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using System.Web.UI;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections;

using System.Web.UI.WebControls;
using System.Web.Caching;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/Login
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult DoLogin(string txtUserName, string txtPassword, string txtCheckCode)
        {
            B_td_adminuser o = new B_td_adminuser();
            M_td_adminuser p = new M_td_adminuser();

            B_td_LoginInfo b1 = new B_td_LoginInfo();
            M_td_LoginInfo m1 = new M_td_LoginInfo();
            string username1 = null;
            string userpass1 = null;
            string code = null;

            if (Request.Form["txtUserName"] != null)
            {
                username1 = Utils.CheckSQLHtml(Request.Form["txtUserName"].ToString());
            }
            if (Request.Form["txtPassword"] != null)
            {
                userpass1 = Utils.CheckSQLHtml(Request.Form["txtPassword"].ToString());
            }
            if (Request.Form["txtCheckCode"] != null)
            {
                code = Utils.CheckSQLHtml(Request.Form["txtCheckCode"].ToString());
            }

            userpass1 = Utils.MD5(userpass1);


            #region 检查验证码
            if (Session["CheckCode"] != null)
            {

                if (code != Session["CheckCode"].ToString())
                {
                    //CommonOperate.Show_Msg("验证码不正确");
                    //Response.End();

                    return Content(StringAlert.Alert("验证码不正确"), "text/html");

                }
                else
                {

                }
            }
            else
            {
                //CommonOperate.Show_Msg("验证码过期");
                //Response.End();
                return Content(StringAlert.Alert("验证码过期"), "text/html");
            }
            #endregion

            string ip = Utils.GetRealIP();

            int adminuserid = o.Check_userpass(username1, userpass1, ip);

            if (adminuserid > 0)
            {
                p = o.GetModel(adminuserid);
                Session["username"] = p.adminuser.ToString();
                Session["userid_gpt"] = p.adminuserid.ToString();
                //Session["area"] = p.Areacode.ToString();
                //Session["purview"] = p.Purview.ToString();
                Session["adminuserid"] = adminuserid.ToString();

                ///添加登录日志
                m1.AdminUserName = username1;
                m1.Pwd = "***";
                m1.LoginSuccess = 1;
                m1.LoginIP = Request.UserHostAddress;
                b1.Add(m1);

                //Response.Redirect("Deflault.aspx");
                return RedirectToAction("Index", "default");
            }
            else
            {
                ///添加登录日志
                m1.AdminUserName = username1;
                m1.Pwd = userpass1;
                m1.LoginSuccess = 0;
                m1.LoginIP = Request.UserHostAddress;
                b1.Add(m1);

                //Response.Redirect("login.aspx");
                return RedirectToAction("Index", "Login");
            }
        }


        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult ImageValidate()
        {
            string strIdentify = "CheckCode"; //随机字串存储键值，以便存储到Session中

            string checkCode = Utils.RndNum(4);

            int iwidth = (int)(checkCode.Length * 13);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, 25);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //定义颜色
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //定义字体 
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            Random rand = new Random();
            //随机输出噪点
            for (int i = 0; i < 50; i++)
            {
                int x = rand.Next(image.Width);
                int y = rand.Next(image.Height);
                g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
            }
            //输出不同字体和颜色的验证码字符
            for (int i = 0; i < checkCode.Length; i++)
            {
                int cindex = rand.Next(7);
                int findex = rand.Next(5);
                Font f = new System.Drawing.Font(font[findex], 10, System.Drawing.FontStyle.Bold);
                Brush b = new System.Drawing.SolidBrush(c[cindex]);
                int ii = 4;
                if ((i + 1) % 2 == 0)
                {
                    ii = 2;
                }
                g.DrawString(checkCode.Substring(i, 1), f, b, 3 + (i * 12), ii);
            }
            //画一个边框
            g.DrawRectangle(new Pen(Color.Black, 0), 0, 0, image.Width - 1, image.Height - 1);

            //设置输出流图片格式
            //context.Response.ContentType = "image/gif";

            //输出到浏览器
            // System.IO.MemoryStream ms = new System.IO.MemoryStream();
            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Gif);
            // HttpContext.Current.Response.ClearContent();
            //Response.ClearContent();

            Session[strIdentify] = checkCode;

            return File(stream.ToArray(), "image/gif");
        }


        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOut()
        {
            Session.Abandon();
            Session.Clear();

            //return RedirectToAction("index");
            return Content("<script>top.location.href='/Admin/Login'</script>");
        }




        public static List<T> DataTableFillObject<T>(DataTable dt)
        {
            if (dt == null)
                return null;
            List<T> result = new List<T>();
            Type type = typeof(T);
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    T t = Activator.CreateInstance<T>();
                    PropertyInfo[] ps = type.GetProperties();
                    FieldInfo[] fs = type.GetFields();
                    foreach (PropertyInfo p in ps)
                    {
                        p.SetValue(t, row[p.Name], null);
                    }
                    foreach (FieldInfo f in fs)
                    {

                        f.SetValue(t, row[f.Name]);
                    }
                    result.Add(t);
                }
                catch { }
            }
            return result;
        }

        [OutputCache(Duration = 180, Location = OutputCacheLocation.ServerAndClient)]
        public List<SelectListItem> PDropDownList()
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            List<hx_td_department> dep = ef.hx_td_department.Where(p => p.department_id > 0).OrderBy(p => p.parentid).OrderByDescending(p => (int)p.orderid).ToList();

            foreach (hx_td_department de in dep.Where(n => n.parentid == 0))
            {
                string parentName = de.department_name;
                string parentId = de.department_id.ToString();
                selectList.Add(new SelectListItem { Text = "├-" + parentName, Value = parentId });
                string pid = parentId;
                string tag = "　|- ";
                ChildrenList(pid, tag, selectList, dep);

            }

            return selectList;
        }
        [OutputCache(Duration = 180, Location = OutputCacheLocation.ServerAndClient)]
        private void ChildrenList(string pid, string tag, List<SelectListItem> selectList, List<hx_td_department> dep1)
        {
            //List<hx_td_department> child = dep1.Where(c => c.parentid > 0 && c.parentid.ToString() == pid).ToList();

            int intpid = int.Parse(pid);
            List<hx_td_department> child = (from a in dep1 where a.parentid == intpid && a.parentid > 0 select a).ToList();

            if (child.Count > 0)
            {

                foreach (var item in child)
                {
                    string clname = tag + item.department_name;
                    string clid = item.department_id.ToString();

                    selectList.Add(new SelectListItem { Text = clname, Value = clid });
                    if (item.depath > 1)
                    {
                        string pid2 = clid;
                        string tag2 = tag + " -- ";
                        ChildrenList(pid, tag, selectList, dep1);
                    }
                }

            }







            /*
            private void ParentDropDownList()
            {
                List<SAS.Model.BPMS_SysMenu> list = new List<SAS.Model.BPMS_SysMenu>();
                StringBuilder strWhere = new StringBuilder();
                List<SelectListItem> selectList = new List<SelectListItem>();
                list = bll.GetModelList(strWhere.ToString());
                foreach (var item in list)
                {
                    if (item.ParentId.Equals("0"))
                    {
                        var parentList = bll.GetModelList(strWhere.ToString()).Where(d => d.ParentId == item.MenuId);
                        foreach (var plitem in parentList)
                        {
                            string parentName = plitem.FullName;
                            string parentId = plitem.MenuId.ToString();
                            selectList.Add(new SelectListItem { Text = "├-" + parentName, Value = parentId });
                            string pid = parentId; 
                            string tag = "　|- ";
                            ChildrenList(pid, tag, selectList);
                        }

                   ViewBag.ParentItemList = new SelectList(selectList, "Value", "Text");
                    }
                }
            }



            private void ChildrenList(string pid, string tag, List<SelectListItem> selectList)
            {
                StringBuilder strWhere = new StringBuilder();
                var childrenList = bll.GetModelList(strWhere.ToString()).Where(d => d.ParentId == pid);
                foreach (var clitem in childrenList)
                {
                    string clname = tag + clitem.FullName;
                    string clid = clitem.MenuId.ToString();
                    selectList.Add(new SelectListItem { Text = clname, Value = clid });
                    string pid2 = clid; 
                    string tag2 = tag + " -- "; 
                    ChildrenList(pid2, tag2, selectList);
                }
            }






            public ActionResult Edit(string id)
            {
                ViewBag.ControllerName = RouteData.Values["controller"].ToString().ToLower();
                var model = new SAS.Model.BPMS_SysMenu();
                model = bll.GetModel(id);
                if (model != null)
                {
                    ParentDropDownList();
                    return View(model);
                } else {
                    return View("404");
                }
            }


            <div class="form-group"> 

              <label class="col-md-3 control-label">上级模块</label>      
              <div class="col-md-4">
              @Html.DropDownListFor(model => model.ParentId, ViewBag.ParentItemList as IEnumerable<SelectListItem>, new { @class = "form-control" }) 
              <span class="help-block">  
              </span>         
              </div>    
              </div>

        */



        }

        public ActionResult RemoveAllCache()
        {
            CacheRemove.ClearAllCache();
            RemoveAllWebSiteCache();
            return Content(StringAlert.Alert("缓存清除成功"));
        }

        public void RemoveAllWebSiteCache()
        {
            try
            {
                string aurl = "";
                string murl = "";
                if (Utils.GetAppSetting("DeBug") == "1")
                {
                    aurl = Utils.GetAppSetting("ADeBugURL") + "console/GlobalConsole/ClearCaching";
                    murl = Utils.GetAppSetting("MDeBugURL") + "login/RemoveAllCache";
                }
                else
                {
                    aurl = Utils.GetAppSetting("AReleaseURL") + "console/GlobalConsole/ClearCaching";
                    murl = Utils.GetAppSetting("MReleaseURL") + "login/RemoveAllCache";
                }
                Utils.PostWebRequest(aurl, "", System.Text.Encoding.UTF8);
                Utils.PostWebRequest(murl, "", System.Text.Encoding.UTF8);
            }
            catch { }
        }
    }
}