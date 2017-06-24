using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuangLitouP2P.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Data;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.DBUtility;
using System.Data.Entity;
using ChuanglitouP2P.Areas.Admin.Controllers.Filters;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class ChannelController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        #region 渠道
        [AdminVaildate()]
        public ActionResult Index(int page = 1,int pgaesize=10)
        {
            int pageIndex = page / 1;
            string channelName = Utils.CheckSQLHtml(DNTRequest.GetString("ChannelName"));
            Expression<Func<hx_Channel, bool>> where = PredicateExtensionses.True<hx_Channel>();
            if (!string.IsNullOrEmpty(channelName))
            {
                where = where.And(p => p.ChannelName.Contains(channelName));
            }
            var list = ef.hx_Channel.Where(where).OrderByDescending(p => p.ChannelID).ToPagedList(pageIndex, pgaesize);
            Dictionary<int,int> dictRegister = new Dictionary<int, int>();//邀请注册总数
            Dictionary<int, int> dictOrder = new Dictionary<int, int>();//邀请注册下单总数
            foreach (var item in list)
            {
                var count=ef.hx_member_table.Where(p => p.channel_invitedcode == item.Invitedcode).Count();
                dictRegister.Add(item.ChannelID, count);

                count=ef.V_Channel_Invite.Where(p => p.channel_invitedcode == item.Invitedcode).Count();
                dictOrder.Add(item.ChannelID,count);
            }
            ViewBag.DictRegister = dictRegister;
            ViewBag.DictOrder = dictOrder;
            ViewBag.ChannelName = channelName;
            return View(list);
        }

        [AdminVaildate(false)]
        public ActionResult Add()
        {
            return View();
        }

        [AdminVaildate(false)]
        public ActionResult Editor(int channelId = 0)
        {
            hx_Channel channel = ef.hx_Channel.Where(p => p.ChannelID == channelId).ToList().FirstOrDefault();
            if (channelId == 0 || channel == null)
            {
                Response.End();
            }
            ViewBag.Channel = channel;
            return View();
        }

        public JsonResult GetInvitedcode()
        {
            var result = new { state = 1, msg = "获取成功！", data = Calculator.Getinvitedcode() };
            return Json(result);
        }

        [AdminVaildate(false)]
        public JsonResult DoAdd(string channelName, string invitedcode, int status, string adminUserName, string adminUserPassword, string trueName,string channelType = "cps1")
        {
            var result = new { state = 1, msg = "渠道 " + channelName + " 增加成功！" };

            if (string.IsNullOrEmpty(channelName) || string.IsNullOrEmpty(invitedcode) 
             || string.IsNullOrEmpty(adminUserName) || string.IsNullOrEmpty(adminUserPassword))
            {
                result = new { state = 0, msg = "缺少增加的参数！" };
                return Json(result);
            }

            var ckChannelNameList = ef.hx_Channel.Any(p => p.ChannelName == channelName);

            if (ckChannelNameList)
            {
                result = new { state = 0, msg = "渠道名称已经存在！" };
                return Json(result);
            }

            var ckAdminNameList = ef.hx_Channel_AdminUser.Any(p => p.AdminUserName == adminUserName);

            if (ckAdminNameList)
            {
                result = new { state = 0, msg = "渠道使用者名称已存在！" };
                return Json(result);
            }

            //写入 Channel 与  Channel_AdminUser 表
            hx_Channel channelEnitty = new hx_Channel();
            channelEnitty.ChannelName = channelName;
            channelEnitty.Invitedcode = invitedcode;
            channelEnitty.Status = (byte)status;
            channelEnitty.Creator = Utils.GetAdmUserID().ToString();
            channelEnitty.CreateTime = DateTime.Now;
            channelEnitty.type = channelType;//渠道类型
            channelEnitty.UpdateTime = channelEnitty.CreateTime;

            hx_Channel_AdminUser cAdminUserEntity = new hx_Channel_AdminUser();
            cAdminUserEntity.CreateTime = channelEnitty.CreateTime;
            cAdminUserEntity.AdminUserName = adminUserName;
            cAdminUserEntity.AdminUserPassword = Utils.MD5(adminUserPassword) ;
            cAdminUserEntity.Status = (byte)1;
            if (string.IsNullOrEmpty(trueName))
            {
                cAdminUserEntity.TrueName = adminUserName;
            }
            else
            {
                cAdminUserEntity.TrueName = trueName;
            }
            List<hx_Channel_AdminUser> cauList = new List<hx_Channel_AdminUser>();
            cauList.Add(cAdminUserEntity);
            channelEnitty.hx_Channel_AdminUser = cauList;
            ef.hx_Channel.Add(channelEnitty);
            ef.SaveChanges();
            return Json(result);
        }

        [AdminVaildate(false)]
        public JsonResult DoEditor(string channelName,int status, string adminUserName,string adminUserPassword,string trueName, int channelId = 0,string channelType="cps1")
        {
            var result = new { state = 1, msg = "渠道 " + channelName + " 编辑完成！" };
            if (string.IsNullOrEmpty(channelName) || channelId==0
               || string.IsNullOrEmpty(adminUserName) || string.IsNullOrEmpty(adminUserPassword))
            {
                result = new { state = 0, msg = "缺少编辑的参数！" };
                return Json(result);
            }
            hx_Channel channelEnitty = ef.hx_Channel.Where(p => p.ChannelID == channelId).ToList().FirstOrDefault();

            var ckChannelNameList = ef.hx_Channel.Where(p => p.ChannelName == channelName && p.ChannelID !=channelId).ToList();
            if (ckChannelNameList.Count() > 0)
            {
                result = new { state = 0, msg = "渠道名称已经存在！" };
                return Json(result);
            }
            var cAdminuserId = channelEnitty.hx_Channel_AdminUser.FirstOrDefault().AdminUserID;
            var ckAdminNameList = ef.hx_Channel_AdminUser.Where(p => p.AdminUserName == adminUserName &&p.AdminUserID != cAdminuserId).ToList();
            if (ckAdminNameList.Count() > 0)
            {
                result = new { state = 0, msg = "渠道使用者名称已存在！" };
                return Json(result);
            }

            //编辑 Channel 与  Channel_AdminUser 表

            channelEnitty.ChannelName = channelName;
            channelEnitty.Status = (byte)status;
            channelEnitty.type = channelType;
            channelEnitty.UpdateTime= DateTime.Now;
            hx_Channel_AdminUser cAdminUserEntity = channelEnitty.hx_Channel_AdminUser.ToList().FirstOrDefault();
            cAdminUserEntity.AdminUserName = adminUserName;

            if (cAdminUserEntity.AdminUserPassword != adminUserPassword)
            {
                cAdminUserEntity.AdminUserPassword = Utils.MD5(adminUserPassword);
            }
            cAdminUserEntity.TrueName = trueName == "" || string.IsNullOrEmpty(trueName) ? cAdminUserEntity.TrueName : trueName;
            ef.SaveChanges();
            Utils.RemoveCache("hx_Channel_" + channelEnitty.Invitedcode);
            return Json(result);
        }

        #endregion

        #region 渠道邀请首投
        [AdminVaildate()]
        public ActionResult ChannelInvList(int page = 1, int pagesize = 10)
        {
            int pageIndex = page / 1;
            string adminUserName = Utils.CheckSQLHtml(DNTRequest.GetString("AdminUserName"));
            string channelName = Utils.CheckSQLHtml(DNTRequest.GetString("ChannelName"));
            string ordId = Utils.CheckSQLHtml(DNTRequest.GetString("OrdId"));
            string btitle = Utils.CheckSQLHtml(DNTRequest.GetString("BTitle"));
            string startTime= DNTRequest.GetString("StartTime");
            string endTime = DNTRequest.GetString("EndTime");
            decimal orderid=0;
            decimal.TryParse(ordId,out orderid);
            Expression<Func<V_Channel_Invite, bool>> where = PredicateExtensionses.True<V_Channel_Invite>();
            if (!string.IsNullOrEmpty(channelName))
            {
                where = where.And(p => p.ChannelName.Contains(channelName));
            }
            if (!string.IsNullOrEmpty(adminUserName))
            {
                where = where.And(p => p.AdminUserName.Contains(adminUserName.Trim()));
            }
            if (!string.IsNullOrEmpty(ordId) && orderid > 0)
            {
                where = where.And(p => p.OrdId.ToString().Contains(ordId.Trim()));
            }
            if (!string.IsNullOrEmpty(btitle))
            {
                where = where.And(p => p.borrowing_title.Contains(btitle.Trim()));
            }

            if (!string.IsNullOrEmpty(startTime) && startTime != "")
            {
                DateTime stime = Convert.ToDateTime(startTime);
                where = where.And(p => DbFunctions.DiffDays(p.invest_time, stime) <= 0);
                DateTime etime = DateTime.Now;
                if (!string.IsNullOrEmpty(endTime) && endTime != "")
                {
                    etime = Convert.ToDateTime(endTime);
                }
                etime = etime.AddDays(1).AddSeconds(-1);
                where = where.And(p => DbFunctions.DiffDays(p.invest_time, etime) >= 0);

            }

            var list1 = ef.V_Channel_Invite.Where(where).ToList();

            var list2 = ef.V_Channel_Invite.Where(where).OrderByDescending(p => p.invest_time).ToList();

            var list = ef.V_Channel_Invite.Where(where).OrderByDescending(p => p.invest_time).ToPagedList(pageIndex, pagesize);


            ViewBag.AdminUserName = adminUserName;
            ViewBag.ChannelName = channelName;
            ViewBag.OrdId = ordId;
            ViewBag.StartTime = startTime;
            ViewBag.EndTime = endTime;
            ViewBag.BTitle = btitle;
            return View(list);
        }
        #endregion

        public ActionResult Login()
        {
            return View();
        }

        public JsonResult DoLogin()
        {
            var result = new { state=1,msg = "登录成功！" };
            string username = Utils.CheckSQLHtml(Request.Form["txtUserName"].ToString());
            string password = Utils.CheckSQLHtml(Request.Form["txtPassword"].ToString());
            string code = Utils.CheckSQLHtml(Request.Form["txtCheckCode"].ToString());
            if (string.IsNullOrEmpty(username))
            {
                result = new { state = 0, msg = "渠道用户名不能为空！" };
                return Json(result);
            }
            if (string.IsNullOrEmpty(password))
            {
                result = new { state = 0, msg = "密码不能为空！" };
                return Json(result);
            }
            if (string.IsNullOrEmpty(username))
            {
                result = new { state = 0, msg = "验证码不能为空！" };
                return Json(result);
            }
            password = Utils.MD5(password);
            if (Session["CheckCode"] != null)
            {
                string serverCode = Session["CheckCode"].ToString();
                if (code != serverCode)
                {
                    result = new { state = 0, msg = "验证码错误！" };
                    return Json(result);
                }
            }
            else
            {
                result = new { state = 0, msg = "验证码过期！" };
                return Json(result);
            }
            //渠道用户
            hx_Channel_AdminUser adminUser=ef.hx_Channel_AdminUser.Where(p=>p.AdminUserName==username).FirstOrDefault();
            hx_Channel channel=null;
            if (adminUser == null)
            {
                result = new { state = 0, msg = "此渠道用户不存在！" };
                return Json(result);
            }
            else
            {
                channel=adminUser.hx_Channel.Where(p => p.Status == 1).FirstOrDefault();
            }
            if (channel ==null)
            {
                result = new { state = 0, msg = "渠道用户已禁用！" };
                return Json(result);
            }
            else if (adminUser.AdminUserPassword != password)
            {
                result = new { state = 0, msg = "密码错误！" };
                return Json(result);
            }
            else
            {
                Session["Channel_AdminUser"] = adminUser;
                Session["adminuserid"] = adminUser.AdminUserID+10000;//防止和后台管理帐号冲突,加1w偏移
                
            }
            return Json(result);
        }

        [AdminVaildate(false)]
        public ActionResult UserList(int page = 1, int pagesize = 10)
        {
            int pageIndex = page / 1;
            string realname = DNTRequest.GetString("Realname");
            string ordId = Utils.CheckSQLHtml(DNTRequest.GetString("OrdId"));
            string btitle = Utils.CheckSQLHtml(DNTRequest.GetString("BTitle"));
            string startTime = DNTRequest.GetString("StartTime");
            string endTime = DNTRequest.GetString("EndTime");


            //渠道用户,非后台登录用户
            hx_Channel_AdminUser adminUser =Session["Channel_AdminUser"] as hx_Channel_AdminUser;

            //判定用户
            if (adminUser == null)
            {
                return RedirectToAction("LoginOut", "Channel");
            }
            Expression<Func<V_Channel_Invite, bool>> where = PredicateExtensionses.True<V_Channel_Invite>();
            decimal orderid = 0;
            decimal.TryParse(ordId, out orderid);
            if (!string.IsNullOrEmpty(ordId) && orderid > 0)
            {
                where = where.And(p => p.OrdId.ToString().Contains(ordId.Trim()));
            }
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(p => p.realname.Contains(realname));
            }
            if (!string.IsNullOrEmpty(btitle))
            {
                where = where.And(p => p.borrowing_title.Contains(btitle));
            }

            if (!string.IsNullOrEmpty(startTime) && startTime != "")
            {
                DateTime stime = Convert.ToDateTime(startTime);
                where = where.And(p => DbFunctions.DiffDays(p.invest_time, stime) <= 0);
                DateTime etime = DateTime.Now;
                if (!string.IsNullOrEmpty(endTime) && endTime != "")
                {
                    etime = Convert.ToDateTime(endTime);
                }
                etime = etime.AddDays(1).AddSeconds(-1);
                where = where.And(p => DbFunctions.DiffDays(p.invest_time, etime) >= 0);
            }

            where = where.And(p=>p.AdminUserName==adminUser.AdminUserName);
            //查出此用户下所有的数据
            var list = ef.V_Channel_Invite.Where(where).OrderByDescending(p =>p.invest_time).ToPagedList(pageIndex, pagesize);
            ViewBag.AdminUserName =  string.IsNullOrEmpty(adminUser.TrueName)==true || adminUser.TrueName=="" ? adminUser.AdminUserName: adminUser.TrueName;
            ViewBag.Invitedcode = adminUser.hx_Channel.FirstOrDefault().Invitedcode;

            ViewBag.BTitle = btitle;
            ViewBag.Realname = realname;
            ViewBag.OrdId = ordId;
            ViewBag.StartTime = startTime;
            ViewBag.EndTime = endTime;

            return View(list); 
        }

        [AdminVaildate(false)]
        public ActionResult AdminUserList(int page = 1, int pagesize = 10)
        {
           
            //渠道用户非,非后台登录用户
            hx_Channel_AdminUser adminUser = Session["Channel_AdminUser"] as hx_Channel_AdminUser;
            //判定用户
            if (adminUser == null)
            {
                return RedirectToAction("LoginOut", "Channel");
            }
            int pageIndex = page / 1;
            string realname = Utils.CheckSQLHtml(DNTRequest.GetString("RealName"));
            string username = Utils.CheckSQLHtml(DNTRequest.GetString("UserName"));
            string startTime = DNTRequest.GetString("StartTime");
            string endTime = DNTRequest.GetString("EndTime");
            Expression<Func<V_Channel_UserList, bool>> where = PredicateExtensionses.True<V_Channel_UserList>();
            where = where.And(p => p.AdminUserName==adminUser.AdminUserName);
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(p => p.realname.Contains(realname.Trim()));
            }
            if (!string.IsNullOrEmpty(username))
            {
                where = where.And(p => p.username.Contains(username.Trim()));
            }
            if (!string.IsNullOrEmpty(startTime) && startTime != "")
            {
                DateTime stime = Convert.ToDateTime(startTime);
                where = where.And(p => DbFunctions.DiffDays(p.registration_time, stime) <= 0);
                DateTime etime = DateTime.Now;
                if (!string.IsNullOrEmpty(endTime) && endTime != "")
                {
                    etime = Convert.ToDateTime(endTime);
                }
                etime = etime.AddDays(1).AddSeconds(-1);
                where = where.And(p => DbFunctions.DiffDays(p.registration_time, etime) >= 0);
            }
            var list = ef.V_Channel_UserList.Where(where).OrderByDescending(p => p.registration_time).ToPagedList(pageIndex, pagesize);
            ViewBag.Username = username;
            ViewBag.RealName = realname;
            ViewBag.StartTime = startTime;
            ViewBag.EndTime = endTime;
            return View(list);
        }


        [AdminVaildate(false)]
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult LoginOut()
        {
            Session["Channel_AdminUser"] = null;
            return RedirectToAction("Login", "Channel");
        }

        [AdminVaildate()]
        public ActionResult ChannelUserList(int page =1,int pagesize = 10)
        {
            int pageIndex = page / 1;
            string adminUserName = Utils.CheckSQLHtml(DNTRequest.GetString("AdminUserName"));
            string username = Utils.CheckSQLHtml(DNTRequest.GetString("UserName"));
            string startTime = DNTRequest.GetString("StartTime");
            string endTime = DNTRequest.GetString("EndTime");
            Expression<Func<V_Channel_UserList, bool>> where = PredicateExtensionses.True<V_Channel_UserList>();
            if (!string.IsNullOrEmpty(adminUserName))
            {
                where = where.And(p => p.AdminUserName.Contains(adminUserName.Trim()));
            }
            if (!string.IsNullOrEmpty(username))
            {
                where = where.And(p=>p.username.Contains(username.Trim()));
            }
            if (!string.IsNullOrEmpty(startTime) && startTime != "")
            {
                DateTime stime = Convert.ToDateTime(startTime);
                where = where.And(p => DbFunctions.DiffDays(p.registration_time, stime) <= 0);
                DateTime etime = DateTime.Now;
                if (!string.IsNullOrEmpty(endTime) && endTime != "")
                {
                    etime = Convert.ToDateTime(endTime);
                }
                etime = etime.AddDays(1).AddSeconds(-1);
                where = where.And(p => DbFunctions.DiffDays(p.registration_time, etime) >= 0);
            }
            var list = ef.V_Channel_UserList.Where(where).OrderByDescending(p => p.registration_time).ToPagedList(pageIndex,pagesize);
            ViewBag.AdminUserName = adminUserName;
            ViewBag.Username = username;
            ViewBag.StartTime = startTime;
            ViewBag.EndTime = endTime;
            ViewBag.TotalItemCount = list.TotalItemCount;
            ViewBag.TotalPageCount = (list.TotalItemCount - 1) / pagesize + 1;
            return View(list);
        }


        #region 导出Excel
        /// <summary>
        ///  客服->渠道用户列表Excel
        /// </summary>
        /// <returns></returns>
        [AdminVaildate(false)]
        public JsonResult AUListExcel()
        {
            //导出已注册的用户列表
            string adminUserName = Utils.CheckSQLHtml(DNTRequest.GetString("AdminUserName"));
            string username = Utils.CheckSQLHtml(DNTRequest.GetString("UserName"));
            string startTime = DNTRequest.GetString("StartTime");
            string endTime = DNTRequest.GetString("EndTime");
            Expression<Func<V_Channel_UserList, bool>> where = PredicateExtensionses.True<V_Channel_UserList>();
            if (!string.IsNullOrEmpty(adminUserName))
            {
                where = where.And(p => p.AdminUserName.Contains(adminUserName.Trim()));
            }
            if (!string.IsNullOrEmpty(username))
            {
                where = where.And(p => p.username.Contains(username.Trim()));
            }
            if (!string.IsNullOrEmpty(startTime) && startTime != "")
            {
                DateTime stime = Convert.ToDateTime(startTime);
                where = where.And(p => DbFunctions.DiffDays(p.registration_time, stime) <= 0);
                DateTime etime = DateTime.Now;
                if (!string.IsNullOrEmpty(endTime) && endTime != "")
                {
                    etime = Convert.ToDateTime(endTime);
                }
                etime = etime.AddDays(1).AddSeconds(-1);
                where = where.And(p => DbFunctions.DiffDays(p.registration_time, etime) >= 0);
            }
            var list = ef.V_Channel_UserList.Where(where).OrderByDescending(p => p.registration_time).ToList();
            var dt =new DataTable();
            #region List TO DataTable
            dt.Columns.Add("编号", typeof(string));
            dt.Columns.Add("渠道使用者", typeof(string));
            dt.Columns.Add("被邀请账号", typeof(string));
            dt.Columns.Add("被邀请真实姓名", typeof(string));
            dt.Columns.Add("注册时间", typeof(string));
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.ID.ToString();
                dr[1] = item.AdminUserName;
                dr[2] = item.username;
                dr[3] = item.realname;
                dr[4] = item.registration_time;
                dt.Rows.Add(dr);
            }
            #endregion
            string url= Extensions.ExportExcel(dt);
            var result = new { state = 1, msg = "投资列表导出完成！", data = url };
            if (string.IsNullOrEmpty(url))
            {
                result = new { state = 0, msg = "导出数据为空！", data = string.Empty };
            }
            return Json(result);
        }

        /// <summary>
        /// 客服->渠道邀请列表Excel
        /// </summary>
        /// <returns></returns>
        [AdminVaildate(false)]
        public JsonResult CIListExcel()
        {
            string adminUserName = Utils.CheckSQLHtml(DNTRequest.GetString("AdminUserName"));
            string channelName = Utils.CheckSQLHtml(DNTRequest.GetString("ChannelName"));
            string ordId = Utils.CheckSQLHtml(DNTRequest.GetString("OrdId"));
            string btitle = Utils.CheckSQLHtml(DNTRequest.GetString("BTitle"));
            string startTime = DNTRequest.GetString("StartTime");
            string endTime = DNTRequest.GetString("EndTime");
            decimal orderid = 0;
            decimal.TryParse(ordId, out orderid);
            Expression<Func<V_Channel_Invite, bool>> where = PredicateExtensionses.True<V_Channel_Invite>();
            if (!string.IsNullOrEmpty(channelName))
            {
                where = where.And(p => p.ChannelName.Contains(channelName));
            }
            if (!string.IsNullOrEmpty(adminUserName))
            {
                where = where.And(p => p.AdminUserName.Contains(adminUserName));
            }
            if (!string.IsNullOrEmpty(ordId) && orderid > 0)
            {
                where = where.And(p => p.OrdId.ToString().Contains(ordId.Trim()));
            }
            if (!string.IsNullOrEmpty(btitle))
            {
                where = where.And(p => p.borrowing_title.Contains(btitle));
            }

            if (!string.IsNullOrEmpty(startTime) && startTime != "")
            {
                DateTime stime = Convert.ToDateTime(startTime);
                where = where.And(p => DbFunctions.DiffDays(p.invest_time, stime) <= 0);
                DateTime etime = DateTime.Now;
                if (!string.IsNullOrEmpty(endTime) && endTime != "")
                {
                    etime = Convert.ToDateTime(endTime);
                }
                etime = etime.AddDays(1).AddSeconds(-1);
                where = where.And(p => DbFunctions.DiffDays(p.invest_time, etime) >= 0);
            }
            var list = ef.V_Channel_Invite.Where(where).OrderByDescending(p => p.invest_time).ToList();

            var dt = new DataTable();
            #region List TO DataTable
            dt.Columns.Add("渠道名称", typeof(string));
            dt.Columns.Add("渠道使用者", typeof(string));
            dt.Columns.Add("被邀请人ID", typeof(string));
            dt.Columns.Add("被邀请账号", typeof(string));
            dt.Columns.Add("被邀请真实姓名", typeof(string));
            dt.Columns.Add("注册时间", typeof(string));
            dt.Columns.Add("首投订单号", typeof(string));
            dt.Columns.Add("首投时间", typeof(string));
            dt.Columns.Add("首投金额", typeof(string));
            dt.Columns.Add("投标信息", typeof(string));
            dt.Columns.Add("投标期限", typeof(string));
            dt.Columns.Add("渠道状态", typeof(string));
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.ChannelName.ToString();
                dr[1] = item.AdminUserName;
                dr[2] = item.registerid;
                dr[3] = item.username;
                dr[4] = item.realname;
                dr[5] = item.registration_time;
                dr[6] = item.OrdId.ToString();
                dr[7] = item.invest_time.ToString();
                dr[8] = item.investment_amount.ToString();
                dr[9] = item.borrowing_title.ToString();
                dr[10] = item.DeadLine.ToString();
                dr[11] = item.Status > 0 ? "启用": "禁用" ;
                dt.Rows.Add(dr);
            }
            #endregion
            string url = Extensions.ExportExcel(dt);
            var result = new { state = 1, msg = "渠道邀请列表导出完成！", data = url };
            if (string.IsNullOrEmpty(url))
            {
                result = new { state = 0, msg = "导出数据为空！", data = string.Empty };
            }
            return Json(result);
        }


        /// <summary>
        /// 渠道个人->渠道用户列表Excel
        /// </summary>
        /// <returns></returns>
        [AdminVaildate(false)]
        public JsonResult UListExcel()
        {
            //渠道用户,非后台登录用户
            hx_Channel_AdminUser adminUser = Session["Channel_AdminUser"] as hx_Channel_AdminUser;

            var result = new { state = 0, msg = "用户没有权限！", data = string.Empty };
            //判定用户
            if (adminUser == null)
            {
                return Json(result);
            }
            string realname = DNTRequest.GetString("Realname");
            string ordId = Utils.CheckSQLHtml(DNTRequest.GetString("OrdId"));
            string startTime = DNTRequest.GetString("StartTime");
            string endTime = DNTRequest.GetString("EndTime");
            string btitle = Utils.CheckSQLHtml(DNTRequest.GetString("BTitle"));

            Expression<Func<V_Channel_Invite, bool>> where = PredicateExtensionses.True<V_Channel_Invite>();
            decimal orderid = 0;
            decimal.TryParse(ordId, out orderid);
            if (!string.IsNullOrEmpty(ordId) && orderid > 0)
            {
                where = where.And(p => p.OrdId.ToString().Contains(ordId.Trim()));
            }
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(p => p.realname.Contains(realname));
            }
            if (!string.IsNullOrEmpty(btitle))
            {
                where = where.And(p => p.borrowing_title.Contains(btitle));
            }

            if (!string.IsNullOrEmpty(startTime) && startTime != "")
            {
                DateTime stime = Convert.ToDateTime(startTime);
                where = where.And(p => DbFunctions.DiffDays(p.invest_time, stime) <= 0);
                DateTime etime = DateTime.Now;
                if (!string.IsNullOrEmpty(endTime) && endTime != "")
                {
                    etime = Convert.ToDateTime(endTime);
                }
                etime = etime.AddDays(1).AddSeconds(-1);
                where = where.And(p => DbFunctions.DiffDays(p.invest_time, etime) >= 0);
            }
            where = where.And(p => p.AdminUserName == adminUser.AdminUserName);
            //查出此用户下所有的数据
            var list = ef.V_Channel_Invite.Where(where).OrderByDescending(p => p.invest_time).ToList();
            var dt = new DataTable();
            #region List TO DataTable
            dt.Columns.Add("被邀请人ID", typeof(string));
            dt.Columns.Add("渠道名称", typeof(string));
            dt.Columns.Add("被邀请账号", typeof(string));
            dt.Columns.Add("被邀请真实姓名", typeof(string));
            dt.Columns.Add("注册时间", typeof(string));
            dt.Columns.Add("首投订单号", typeof(string));
            dt.Columns.Add("首投时间", typeof(string));
            dt.Columns.Add("首投金额", typeof(string));
            dt.Columns.Add("投标信息", typeof(string));
            dt.Columns.Add("投标期限", typeof(string));
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.ID.ToString();
                dr[1] = item.ChannelName.ToString();
                dr[2] = Utils.ReplaceWithSpecialChar(item.username, 3, 4, '*');
                dr[3] = item.realname;
                dr[4] = item.registration_time;
                dr[5] = item.OrdId.ToString();
                dr[6] = item.invest_time.ToString();
                dr[7] = item.investment_amount.ToString();
                dr[8] = item.borrowing_title.ToString();
                dr[9] = item.DeadLine.ToString();
                dt.Rows.Add(dr);
            }
            #endregion
            string url = Extensions.ExportExcel(dt);
            result = new { state = 1, msg = "投资列表导出完成！", data = url };
            if (string.IsNullOrEmpty(url))
            {
                result = new { state = 0, msg = "导出数据为空！", data = string.Empty };
            }
            return Json(result);
        }


        /// <summary>
        /// 渠道个人->渠道注册用户列表Excel
        /// </summary>
        /// <returns></returns>
        [AdminVaildate(false)]
        public JsonResult AdminUserListExcel()
        {
            hx_Channel_AdminUser adminUser = Session["Channel_AdminUser"] as hx_Channel_AdminUser;
            var result = new { state = 0, msg = "用户没有权限！", data = string.Empty };
            //判定用户
            if (adminUser == null)
            {
                return Json(result);
            }
            string realname = Utils.CheckSQLHtml(DNTRequest.GetString("RealName"));
            string username = Utils.CheckSQLHtml(DNTRequest.GetString("UserName"));
            string startTime = DNTRequest.GetString("StartTime");
            string endTime = DNTRequest.GetString("EndTime");
            Expression<Func<V_Channel_UserList, bool>> where = PredicateExtensionses.True<V_Channel_UserList>();
            where = where.And(p => p.AdminUserName == adminUser.AdminUserName);
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(p => p.realname.Contains(realname.Trim()));
            }
            if (!string.IsNullOrEmpty(username))
            {
                where = where.And(p => p.username.Contains(username.Trim()));
            }
            if (!string.IsNullOrEmpty(startTime) && startTime != "")
            {
                DateTime stime = Convert.ToDateTime(startTime);
                where = where.And(p => DbFunctions.DiffDays(p.registration_time, stime) <= 0);
                DateTime etime = DateTime.Now;
                if (!string.IsNullOrEmpty(endTime) && endTime != "")
                {
                    etime = Convert.ToDateTime(endTime);
                }
                etime = etime.AddDays(1).AddSeconds(-1);
                where = where.And(p => DbFunctions.DiffDays(p.registration_time, etime) >= 0);
            }
            var list = ef.V_Channel_UserList.Where(where).OrderByDescending(p => p.registration_time).ToList();

            var dt = new DataTable();
            #region List TO DataTable
            dt.Columns.Add("编号", typeof(string));
            dt.Columns.Add("渠道使用者", typeof(string));
            dt.Columns.Add("被邀请账号", typeof(string));
            dt.Columns.Add("被邀请真实姓名", typeof(string));
            dt.Columns.Add("注册时间", typeof(string));
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.ID.ToString();
                dr[1] = item.AdminUserName;
                dr[2] = Utils.ReplaceWithSpecialChar(item.username, 3, 4, '*');
                dr[3] = item.realname;
                dr[4] = item.registration_time.ToString();
                dt.Rows.Add(dr);
            }
            #endregion
            string url = Extensions.ExportExcel(dt);
            result = new { state = 1, msg = "投资列表导出完成！", data = url };
            if (string.IsNullOrEmpty(url))
            {
                result = new { state = 0, msg = "导出数据为空！", data = string.Empty };
            }
            return Json(result);
        }
        #endregion 
    }
}