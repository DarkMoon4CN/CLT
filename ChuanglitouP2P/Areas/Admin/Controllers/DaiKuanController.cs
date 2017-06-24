using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.VeryCodes.NetCreditAssistant.Model;
using ChuangLitouP2P.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using EntityFramework.Extensions;
using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.BLL;
using System.Data;
using System.Text;


namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class DaiKuanController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        [AdminVaildate()]
        public ActionResult Index(string title1 = "", string realname = "", int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<V_borrowing_target_addlist, bool>> where = PredicateExtensionses.True<V_borrowing_target_addlist>();
            where = where.And(p => p.targetid > 0);
            if (!string.IsNullOrEmpty(title1))
            {
                where = where.And(p => p.borrowing_title.Contains(title1));
            }
            if (!string.IsNullOrEmpty(realname))
            {
                where = where.And(p => p.realname == realname);
            }
            IPagedList<V_borrowing_target_addlist> list = ef.V_borrowing_target_addlist.Where(where).OrderByDescending(p => p.targetid).ToPagedList(pageNumber, pageSize);

            ViewBag.title2 = title1;
            ViewBag.realname = realname;

            return View(list);
        }


        #region 审核结果

        [AdminVaildate()]
        public ActionResult Waitverify(int id, string state = "view")
        {
            hx_borrowing_target item = (from a in ef.hx_borrowing_target where a.targetid == id select a).SingleOrDefault();
            IEnumerable<V_borrowing_target_review> list = ef.V_borrowing_target_review.Where(p => p.targetid == id).OrderBy(p => p.reviewtime).ToList();

            ViewBag.id = id;
            ViewBag.state = state;
            ViewBag.item = item;

            return View(list);
        }


        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Waitverify(int id, decimal borrowing_balance, decimal consultingAMT, decimal guaranteeAMT, int tender_state, string reviewremarks)
        {
            var userid = Utils.GetAdmUserID();
            var controllerName = this.RouteData.Values["controller"].ToString();
            var actionName = this.RouteData.Values["action"].ToString();
            if (!new UserLimitByEF().CheckAdminLimit(userid, controllerName, actionName))
            {   //无权限
                return Content("<script>alert('您没有操作权限');closewindows();</script>", "text/html");
            }
            var result = ef.hx_borrowing_target.Where(p => p.targetid == id).Update(p => new hx_borrowing_target { consultingAMT = consultingAMT, guaranteeAMT = guaranteeAMT, tender_state = tender_state });

            string str = string.Format("<script>alert('审核操作失败');closewindows();</script>");
            if (result > 0)
            {
                hx_td_reviewremarks model = new hx_td_reviewremarks();
                model.targetid = id;
                model.tender_state = tender_state;
                model.reviewremarks = reviewremarks;
                model.reviewtime = DateTime.Now;
                model.admin_operator = Utils.GetAdmUserID();

                ef.hx_td_reviewremarks.Add(model);

                if (ef.SaveChanges() > 0)
                {
                    str = string.Format("<script>alert('审核操作成功');closewindows();</script>");

                }
            }
            return Content(str, "text/html");
        }


        #endregion

        #region 推荐、未推荐

        [AdminVaildate(false)]
        public ActionResult Recommend(int id, int state)
        {
            string json = "{\"ret\":0,\"msg\":\"操作失败\"}";
            var userid = Utils.GetAdmUserID();
            var controllerName = this.RouteData.Values["controller"].ToString();
            var actionName = this.RouteData.Values["action"].ToString();
            if (!new UserLimitByEF().CheckAdminLimit(userid, controllerName, actionName))
            {   //无权限
                json = "{\"ret\":0,\"msg\":\"您没有操作权限\"}";
                return Content(json, "text/json");
            }
            if (id < 1)
            {
                json = "{\"ret\":-1,\"msg\":\"参数错误\"}";
            }
            else
            {
                var i = ef.hx_borrowing_target.Where(a => a.targetid == id).Update(a => new hx_borrowing_target { recommend = state });
                if (i > 0)
                {
                    json = "{\"ret\":1,\"msg\":\"操作成功\"}";
                }
            }
            return Content(json, "text/json");
        }

        #endregion

        #region 提交初审

        [AdminVaildate(false)]
        public ActionResult Examine(int id)
        {
            string json = "{\"ret\":0,\"msg\":\"操作失败\"}";
            if (id < 1)
            {
                json = "{\"ret\":-1,\"msg\":\"参数错误\"}";
            }
            else
            {
                var i = ef.hx_borrowing_target.Where(p => p.targetid == id).Update(p => new hx_borrowing_target { tender_state = 0 });
                if (i > 0)
                {
                    hx_td_reviewremarks review = new hx_td_reviewremarks();
                    review.targetid = id;
                    review.tender_state = 0;
                    review.reviewremarks = "提交初审";
                    review.reviewtime = DateTime.Now;
                    review.admin_operator = Utils.GetAdmUserID();

                    ef.hx_td_reviewremarks.Add(review);

                    ef.SaveChanges();
                    json = "{\"ret\":1,\"msg\":\"操作成功\"}";
                }
            }

            return Content(json, "text/json");
        }

        #endregion

        #region 基础材料、担保材料，现场图片 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetid"></param>
        /// <param name="registerid"></param>
        /// <param name="tp"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult BaseInfo(int targetid, int registerid, int tp)
        {
            IEnumerable<hx_borrower_guarantor_picture> list = ef.hx_borrower_guarantor_picture.Where(p => p.type_picture == tp && p.targetid == targetid).OrderBy(p => p.picture_index).ToList();

            ViewBag.targetid = targetid;
            ViewBag.registerid = registerid;
            ViewBag.tp = tp;

            return View(list);
        }

        /// <summary>
        /// 获取存放图片的最大ID
        /// </summary>
        /// <param name="targetid"></param>
        /// <returns></returns>
        public JsonResult GetMaxPictureIndex(int targetid)
        {
            var maxPictureIndex = ef.hx_borrower_guarantor_picture.Where(p => p.targetid == targetid).Max(p => p.picture_index);
            var tmp = new { ret = 1, msg = "操作成功", data = maxPictureIndex == null ? 0 : maxPictureIndex };
            return Json(tmp);
        }

        public JsonResult GetPicture(int targetid, int tp)
        {
            var list = ef.hx_borrower_guarantor_picture.Where(p => p.type_picture == tp && p.targetid == targetid).OrderBy(p => p.picture_index).ToList();
            return Json(list);
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminVaildate(false)]
        public ActionResult RemoveImg(int id)
        {
            string json = "{\"ret\":0,\"msg\":\"操作失败\"}";
            var i = ef.hx_borrower_guarantor_picture.Where(p => p.borrower_guarantor_picture_id == id).Delete();
            if (i > 0)
            {
                json = "{\"ret\":1,\"msg\":\"操作成功\"}";
            }
            return Content(json, "text/json");
        }

        #endregion

        #region 详细内容

        [AdminVaildate()]
        public ActionResult Detail(int registerid, int targetid, int id, string oper = "")
        {

            Utils.SetSYSDateTimeFormat();
            hx_borrowing_target_detailed td = (from a in ef.hx_borrowing_target_detailed where a.target_detailed_id == id select a).SingleOrDefault();

            ViewBag.registerid = registerid;
            ViewBag.targetid = targetid;
            ViewBag.id = id;
            ViewBag.oper = oper;

            return View(td);
        }


        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate(false)]
        public ActionResult DetailEdit(hx_borrowing_target_detailed p)
        {
            string str = "";
            if (p.target_detailed_id > 0)
            {   //修改
                if (EditDetail(p))
                {
                    str = StringAlert.Alert("修改操作成功!", "/admin/DaiKuan/Index");
                }
                else
                {
                    str = StringAlert.Alert("修改操作失败!", "/admin/DaiKuan/Index");
                }
            }
            else
            {   //新增
                if (AddDetail(p))
                {
                    str = StringAlert.Alert("添加操作成功!", "/admin/DaiKuan/Index");
                }
                else
                {
                    str = StringAlert.Alert("添加操作失败!", "/admin/DaiKuan/Index");
                }
            }

            return Content(str, "text/html");
        }

        [AdminVaildate(false)]
        private bool EditDetail(hx_borrowing_target_detailed p)
        {
            string[] proNames;

            proNames = new string[] { "item_details", "borrower_circumstances", "use_funds", "independent_advice", "guarantee_agency_views", "risk_control_measures" };
            p = (hx_borrowing_target_detailed)Utils.ValidateModelClass(p);

            p.hx_borrowing_target = (from a in ef.hx_borrowing_target where a.targetid == p.targetid select a).SingleOrDefault();

            DbEntityEntry entry = ef.Entry<hx_borrowing_target_detailed>(p);
            entry.State = EntityState.Unchanged;

            foreach (string ProName in proNames)
            {
                entry.Property(ProName).IsModified = true;
            }
            int i = ef.SaveChanges();
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [AdminVaildate(false)]
        private bool AddDetail(hx_borrowing_target_detailed t)
        {

            Utils.SetSYSDateTimeFormat();

            t = (hx_borrowing_target_detailed)Utils.ValidateModelClass(t);
            t.createtime = DateTime.Now;
            ef.hx_borrowing_target_detailed.Add(t);

            return ef.SaveChanges() > 0;
        }

        #endregion

        #region 排序

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult IndexOrder(int id)
        {
            var model = (from a in ef.hx_borrowing_target where a.targetid == id select a).SingleOrDefault();

            var sortid = 0;
            if (id > 0 && model != null && model.targetid > 0)
            {
                sortid = Convert.ToInt32(model.indexorder);
            }

            ViewBag.sortList = GetIndexSortList(sortid);
            ViewBag.id = id;
            return View();
        }
        private IEnumerable<SelectListItem> GetIndexSortList(object obj)
        {
            int sortid = obj == null ? 0 : Convert.ToInt32(obj.ToString());
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "0", Text = "未排序" });
            for (int i = 1; i < 11; i++)
            {
                if (i == sortid)
                {
                    list.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString(), Selected = true });
                }
                else
                {
                    list.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
                }
            }
            return list;
        }

        /// <summary>
        /// 设置排序
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sortid"></param>
        /// <returns></returns>
        [AdminVaildate(false)]
        public ActionResult SetIndexOrder(int id, int sortid)
        {
            string json = "";
            if (id < 1)
            {
                json = "{\"ret\":-1,\"msg\":\"参数错误\"}";
            }
            else
            {
                int i = ef.hx_borrowing_target.Where(a => a.targetid == id).Update(a => new hx_borrowing_target { indexorder = sortid });
                if (i > 0)
                {
                    json = "{\"ret\":1,\"msg\":\"保存成功\"}";
                }
                else
                {
                    json = "{\"ret\":0,\"msg\":\"保存失败\"}";
                }
            }
            return Content(json, "text/json");
        }

        #endregion

        #region 新增，修改，删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult remove(int id)
        {
            string str = "";
            var result = ef.hx_borrowing_target.Where(p => p.targetid == id).Update(a => new hx_borrowing_target { isDel = 1 });
            if (result > 0)
            {   //成功
                str = StringAlert.Alert("操作成功！", "/admin/DaiKuan/Index");
            }
            else
            {
                str = StringAlert.Alert("操作失败！", "/admin/DaiKuan/Index");
            }
            return Content(str, "text/html");
        }

        #region 新增，修改 标的
        /// <summary>
        /// 新增，修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Editor(long id = 0, string oper = "")
        {
            Utils.SetSYSDateTimeFormat();
            hx_borrowing_target borrow = new hx_borrowing_target();
            if (id > 0)
            {
                borrow = ef.hx_borrowing_target.SingleOrDefault(p => p.targetid == id);
            }
            var daikuanren = "";
            if (borrow != null && borrow.targetid > 0)
            {
                borrow.release_date = String.IsNullOrEmpty(borrow.release_date.ToString()) ? Convert.ToDateTime("") : Convert.ToDateTime(((DateTime)borrow.release_date).ToString("yyyy-MM-dd HH:mm:ss"));
                borrow.repayment_date = String.IsNullOrEmpty(borrow.repayment_date.ToString()) ? Convert.ToDateTime("") : Convert.ToDateTime(((DateTime)borrow.repayment_date).ToString("yyyy-MM-dd HH:mm:ss"));
                borrow.start_time = String.IsNullOrEmpty(borrow.start_time.ToString()) ? Convert.ToDateTime("") : Convert.ToDateTime(((DateTime)borrow.start_time).ToString("yyyy-MM-dd HH:mm:ss"));
                borrow.sys_time = String.IsNullOrEmpty(borrow.sys_time.ToString()) ? Convert.ToDateTime("") : Convert.ToDateTime(((DateTime)borrow.sys_time).ToString("yyyy-MM-dd HH:mm:ss"));
                borrow.end_time = String.IsNullOrEmpty(borrow.end_time.ToString()) ? Convert.ToDateTime("") : Convert.ToDateTime(((DateTime)borrow.end_time).ToString("yyyy-MM-dd HH:mm:ss"));

                var member = (from b in ef.hx_member_table where b.registerid == borrow.borrower_registerid select b).SingleOrDefault();
                daikuanren = member == null || member.registerid < 1 ? "" : member.realname;
            }
            else
            {
                borrow.IsUse = 1;
            }

            ViewBag.ProjectType = ef.hx_Project_type.OrderBy(p => p.project_type_id).Where(p => p.project_type_id != 5).Select(p => new SelectListItem { Value = p.project_type_id.ToString(), Text = p.project_type_name });
            ViewBag.Company = ef.hx_bonding_company.OrderByDescending(p => p.companyid).Select(p => new SelectListItem { Value = p.companyid.ToString(), Text = p.company_name });
            ViewBag.Way = ef.guarantee_way.OrderByDescending(p => p.guarantee_way_id).Select(p => new SelectListItem { Value = p.guarantee_way_id.ToString(), Text = p.guarantee_way_name });
            ViewBag.Option = GetRepayMay(typeof(EnumRepaymentMode));
            ViewBag.Recommend = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "不推荐" }, new SelectListItem { Value = "1", Text = "推荐", Selected = true } };
            ViewBag.IsUse = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "个人连带" }, new SelectListItem { Value = "1", Text = "担保公司", Selected = true } };

            ViewBag.id = id;
            ViewBag.oper = oper;
            ViewBag.daikuanren = daikuanren;

            return View(borrow);
        }
        #endregion

        #region 还款方式枚举类型
        /// <summary>
        /// 还款方式枚举类型
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="defaultValue"></param>
        /// <param name="defaultText"></param>
        /// <returns></returns>
        private IEnumerable<SelectListItem> GetRepayMay(Type enumType, string defaultValue = "", string defaultText = "")
        {
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException("参数不是枚举类型。");
            }
            List<SelectListItem> list = new List<SelectListItem>();
            if (!String.IsNullOrEmpty(defaultText))
            {
                list.Add(new SelectListItem { Value = defaultValue, Text = defaultText });
            }
            try
            {
                Type attributeType = typeof(DescriptionAttribute);
                foreach (FieldInfo info in enumType.GetFields())
                {
                    if (info.FieldType.IsEnum)
                    {
                        SelectListItem item = new SelectListItem();
                        item.Value = Convert.ToString((int)enumType.InvokeMember(info.Name, BindingFlags.GetField, null, null, null));
                        object[] customAttributes = info.GetCustomAttributes(attributeType, true);
                        if (customAttributes.Length > 0)
                        {
                            DescriptionAttribute attribute = (DescriptionAttribute)customAttributes[0];
                            item.Text = attribute.Description;
                        }
                        else
                        {
                            item.Text = info.Name;
                        }
                        list.Add(item);
                    }
                }
            }
            catch { }
            return list;

        }
        #endregion

        #region 提交或修改标的
        /// <summary>
        /// 提交或修改标的
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate(false)]
        public ActionResult EditorPost(hx_borrowing_target p)
        {
            bool result = false;



            p.loan_management_fee = 0.00M;
            p.investors_management_fee = 0.00M;
            p.ordinary_overdue_management_fees = 0.00M;
            p.seriously_overdue_management_fees = 0.00M;
            p.ordinary_overdue_penalty = 0.00M;
            p.seriously_overdue_penalty = 0.00M;
            p.transfer_Expenses = 0.00M;
            p.guarantee_fee = 0.00M;
            p.IsIRC = 0;

            this.TempData["DaiKuanAdd"] = p;
            if (p.targetid > 0)
            {   //修改
                return RedirectToAction("Modify");
                //result = Modify(p);
                //return Content(StringAlert.Alert(result ? "操作成功" : "操作失败", "/admin/DaiKuan/Index"), "text/html");
            }
            else
            {   //新增
                //return RedirectToAction("Add", new object { t = p });
                return RedirectToAction("Add", "DaiKuan");
                //int tid = 0;
                //var pid = Add(p,out tid);
                //if (pid>0)
                //{   //成功
                //    return Content(StringAlert.Alert("操作成功", "/admin/DaiKuan/Detail?registerid="+p.borrower_registerid+ "&targetid="+pid+ "&id="+ tid), "text/html");
                //}
            }
            //return Content(StringAlert.Alert("操作失败", "/admin/DaiKuan/Index"), "text/html");
        }
        #endregion

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        [AdminVaildate(false)]
        public ActionResult Add()
        {
            hx_borrowing_target t = (hx_borrowing_target)this.TempData["DaiKuanAdd"];
            this.TempData["DaiKuanAdd"] = null;
            Utils.SetSYSDateTimeFormat();
            var tid = 0;
            t = (hx_borrowing_target)Utils.ValidateModelClass(t);
            t.indexorder = 0;
            t.fundraising_amount = 0.00M;
            t.tender_state = -1;
            t.full_scale_loan = 0;
            t.flow_return = 0;
            t.repaymentperiods = 0;
            t.isDel = 0;

            ef.hx_borrowing_target.Add(t);
            int id = ef.SaveChanges();
            if (id > 0)
            {
                hx_borrowing_target_detailed detail = new hx_borrowing_target_detailed();
                detail.targetid = t.targetid;
                detail.borrower_registerid = t.borrower_registerid;
                detail.item_details = "";
                detail.borrower_circumstances = "";
                detail.use_funds = "";
                detail.independent_advice = "";
                detail.guarantee_agency_views = "";
                detail.risk_control_measures = "";
                detail.createtime = DateTime.Now;

                ef.hx_borrowing_target_detailed.Add(detail);
                var num = ef.SaveChanges();
                tid = detail.target_detailed_id;
                if (t.targetid > 0)
                {
                    return Content(StringAlert.Alert("操作成功", "/admin/DaiKuan/Detail?registerid=" + t.borrower_registerid + "&targetid=" + t.targetid + "&id=" + tid), "text/html");
                }


            }

            return Content(StringAlert.Alert("操作失败", "/admin/DaiKuan/Index"), "text/html");
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Modify()
        {
            hx_borrowing_target p = (hx_borrowing_target)this.TempData["DaiKuanAdd"];
            this.TempData["DaiKuanAdd"] = null;
            string[] proNames;

            proNames = new string[] { "borrowing_title","borrowing_thumbnail", "project_type_id", "borrowing_thumbnail","borrowing_balance","annual_interest_rate","B_Rates","release_date","life_of_loan","unit_day","repayment_date","month_payment_date","minimum","maxmum", "companyid",
                "guarantee_way_id", "payment_options", "start_time","sys_time","end_time","service_charge","loan_management_fee","investors_management_fee","ordinary_overdue_management_fees","seriously_overdue_management_fees","ordinary_overdue_penalty","seriously_overdue_penalty",
                "transfer_Expenses","guarantee_fee","recommend","IsUse","IsIRC","Purpose","PaySource","Collateral","Isinterest"};
            p = (hx_borrowing_target)Utils.ValidateModelClass(p);

            p.hx_borrowing_target_detailed = (from a in ef.hx_borrowing_target_detailed where a.targetid == p.targetid select a).ToList<hx_borrowing_target_detailed>();//  .Select(d => d.targetid == p.targetid).ToList<hx_borrowing_target_detailed>();
            p.Contract_management = (from b in ef.Contract_management where b.targetid == p.targetid select b).ToList<Contract_management>();
            // p.hx_member_table = (from c in ef.hx_member_table where c.registerid == p.borrower_registerid select c).SingleOrDefault();

            DbEntityEntry entry = ef.Entry<hx_borrowing_target>(p);
            entry.State = EntityState.Unchanged;

            foreach (string ProName in proNames)
            {
                entry.Property(ProName).IsModified = true;
            }
            int i = ef.SaveChanges();
            if (i > 0)
            {
                return Content(StringAlert.Alert("操作成功", "/admin/DaiKuan/Index"), "text/html");
            }
            else
            {
                return Content(StringAlert.Alert("操作失败", "/admin/DaiKuan/Index"), "text/html");
            }

        }

        /// <summary>
        /// 判断模板名称是否存在
        /// </summary>
        /// <param name="param"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult CheckTitle(string param, int key = 0)
        {
            if (IsExist(param, key))
            {
                return Content("模板名称已经存在");
            }
            return Content("y");
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool IsExist(string name, int key = 0)
        {
            var list = (from a in ef.hx_borrowing_target where a.borrowing_title == name select a).ToList<hx_borrowing_target>();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.targetid != key)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 获取合同编号
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLoanNumber()
        {
            string number = new ChuanglitouP2P.BLL.B_borrowing_target().GetLoanNumber();

            string json = "{\"ret\":1,\"number\":\"" + number + "\"}";

            return Content(json, "text/json");

        }

        /// <summary>
        /// 上次图片
        /// </summary>
        /// <returns></returns>
        [AdminVaildate(false)]
        public ActionResult Upload()
        {
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string imgPath = "";
            string PhysicalPath = "";
            string json = "{\"ret\":0,\"msg\":\"上传图片失败！\"}";
            if (hfc.Count > 0)
            {
                string suf = "";
                if (!isImage(hfc[0].FileName, out suf))
                {
                    json = "{\"ret\":0,\"msg\":\"图片只支持－－－gif|jpg|jpeg|bmp！\"}";
                    return Content(json, "text/json");
                }
                string fileName = string.Format("{0}.{1}", sjname(), suf);
                imgPath = @"/Productpicture/";
                var ss = hfc[0].ContentType;
                PhysicalPath = Server.MapPath(imgPath);
                if (!System.IO.Directory.Exists(PhysicalPath))
                {
                    System.IO.Directory.CreateDirectory(PhysicalPath);
                }

                PhysicalPath = PhysicalPath + fileName;
                hfc[0].SaveAs(PhysicalPath);

                imgPath = imgPath + fileName;
            }
            if (Utils.CheckPictureSafe(PhysicalPath))
            {
                if (!string.IsNullOrEmpty(imgPath))
                {
                    imgPath = imgPath.Replace("/", "//");
                    json = "{\"ret\":1,\"path\":\"" + imgPath + "\"}";
                }
            }
            else
            {   //图片安全提醒，您试除上传非法文件
                json = "{\"ret\":0,\"path\":\"图片中含有非法文件\"}";
            }
            return Content(json, "text/json");
        }

        /// <summary>
        /// 产生个随即名称
        /// </summary>
        /// <returns></returns>
        private string sjname()
        {
            string sj = null;
            Random ra = new Random();
            sj = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.TimeOfDay.Hours.ToString() + DateTime.Now.TimeOfDay.Minutes.ToString() + DateTime.Now.TimeOfDay.Milliseconds.ToString() + ra.Next(1, 1000).ToString();
            return sj;
        }

        private bool isImage(string filename, out string suf)
        {
            string imgtype = "gif|jpg|jpeg|bmp";
            //取后最
            int pos = filename.IndexOf(".");
            string hz = filename.Substring((pos + 1)).ToLower();

            suf = hz;
            if (hz != "gif" && hz != "jpg" && hz != "jpeg" && hz != "bmp")
            {
                return false;
            }
            return true;
        }

        #endregion




        #region 自动计算返款日期+Calculationdate()
        /// <summary>
        /// 自动计算返款日期
        /// </summary>
        /// <returns></returns>
        public ActionResult Calculationdate()
        {
            int selectid = DNTRequest.GetInt("selectid", 1);
            int lift = DNTRequest.GetInt("lift", 1);
            string strdate = Utils.CheckSQL(DNTRequest.GetString("release_date"));
            DateTime dtime;
            string ctime = "";
            if (strdate.Length > 0)
            {
                dtime = DateTime.Parse(strdate);
            }
            else
            {
                dtime = DateTime.Now;
            }

            if (selectid == 1)
            {
                //月
                ctime = dtime.AddMonths(lift).ToString("yyyy-MM-dd HH:mm:ss");

            }
            else
            {
                //日
                ctime = dtime.AddDays(lift).ToString("yyyy-MM-dd HH:mm:ss");
            }


            return Content(ctime);
        }
        #endregion


        /// <summary>
        /// 未成功列表
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Invalidinvestment()
        {
            List<V_Bid_records_Lost> list = (from a in ef.V_Bid_records_Lost select a).ToList();
            return View(list);
        }


        #region 未成功列表 校验处理
        /// <summary>
        /// 未成功列表 校验处理
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult postInvalidinvestment()
        {
            List<V_Bid_records_Lost> list = (from a in ef.V_Bid_records_Lost select a).ToList();

            foreach (V_Bid_records_Lost item in list)
            {
                setRecords_Lost(item.investor_registerid.ToString(), item.bid_records_id.ToString(), item.OrdId.ToString(), DateTime.Parse(item.invest_time.ToString()).ToString("yyyyMMdd"), item.UsrCustId.ToString(), item.FrozenidNo, item.investment_amount.ToString(), item.FreezeTrxId == null ? "" : item.FreezeTrxId.ToString(), "", "TENDER");
            }


            return Redirect("/Admin/DaiKuan/Invalidinvestment");
        }
        #endregion

        public ActionResult InvalidInvst()
        {
            Expression<Func<V_Bid_records_Lost, bool>> where = PredicateExtensionses.True<V_Bid_records_Lost>();
            where = where.And(p => p.bid_records_id > 0);
            //清理10分钟之前的无效标的
            DateTime dt2 = DateTime.Now.AddMinutes(-10);
            where = where.And(a => ((DateTime)a.invest_time) <= dt2);
            List<V_Bid_records_Lost> list = ef.V_Bid_records_Lost.Where(where).AsNoTracking().ToList();
            foreach (V_Bid_records_Lost item in list)
            {
                setRecords_Lost(item.investor_registerid.ToString(), item.bid_records_id.ToString(), item.OrdId.ToString(), DateTime.Parse(item.invest_time.ToString()).ToString("yyyyMMdd"), item.UsrCustId.ToString(), item.FrozenidNo, item.investment_amount.ToString(), item.FreezeTrxId == null ? "" : item.FreezeTrxId.ToString(), "", "TENDER");
            }
            return Content("远程调用成功");
        }


        /// <summary>
        /// 无效投资标的处理
        /// </summary>
        /// <param name="investor_registerid">投资用户id</param>
        /// <param name="bid_records_id">投资记录id</param>
        /// <param name="OrdId">投资订单号</param>
        /// <param name="OrdDate">投资订单日期</param>
        /// <param name="UsrCustId">客户号</param>
        /// <param name="FreezeOrdId">冻结订单号</param>
        /// <param name="TransAmt">交易金额</param>
        /// <param name="FreezeTrxId">冻结唯一标识</param>
        /// <param name="MerPriv">优惠券字符串</param>
        /// <param name="QueryTransType">查询类型   LOANS：放款交易查询 REPAYMENT：还款交易查询 TENDER：投标交易查询 CASH：取现交易查询     FREEZE：冻结解冻交易查询     </param>
        public void setRecords_Lost(string investor_registerid, string bid_records_id, string OrdId, string OrdDate, string UsrCustId, string FreezeOrdId, string TransAmt, string FreezeTrxId, string MerPriv, string QueryTransType = "TENDER")
        {
            TransStat ts = new TransStat();
            bool d = ts.checktrans(OrdId, OrdDate, QueryTransType);
            if (d == false)
            {
                string sql = "update hx_UserAct set UseState=0,AmtProid=0 where UseState=3 and AmtProid=" + bid_records_id + "and registerid=" + investor_registerid;
                DbHelperSQL.RunSql(sql);
                sql = "delete hx_Bid_records where OrdId ='" + OrdId + "' and ordstate=0 and bid_records_id=" + bid_records_id;
                DbHelperSQL.RunSql(sql);
            }
            else
            {//TODO 此处有bug，自动查询的更新冻结表时缺少 FreezeTrxId 值，放款时没有该值会报错。 考虑注释该逻辑，等待汇付异步回调？？？？
                B_usercenter BUC = new B_usercenter();
                //取得投标记录使用的优惠券
                string AmtProid = BUC.GetBid_AmtProid(int.Parse(bid_records_id));
                int de = BUC.ReInvest_success(UsrCustId, FreezeOrdId, TransAmt, FreezeTrxId, OrdId, AmtProid);
                if (de > 0)
                {
                    string sql = "select targetid,bid_records_id, borrowing_title,investor_registerid ,username,mobile,invitationcode,investment_amount,life_of_loan,unit_day,borrowing_balance,bonusAmt  from  V_hx_Bid_records_borrowing_target where OrdId='" + OrdId + "'";
                    DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                    if (dt.Rows.Count > 0)
                    {
                        decimal investAmt = decimal.Parse(dt.Rows[0]["investment_amount"].ToString());
                        int registerid = int.Parse(dt.Rows[0]["investor_registerid"].ToString());
                        string targetid = dt.Rows[0]["targetid"].ToString();

                        #region 待提取为公共方法
                        #region MyRegion  系统消息
                        DateTime dti = DateTime.Now;
                        M_td_System_message pm = new M_td_System_message();
                        pm.MReg = registerid;
                        pm.Mstate = 0;
                        pm.MTitle = "投资成功";
                        pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功投资了项目【" + dt.Rows[0]["borrowing_title"].ToString() + "】，投资金额是：" + investAmt + "。如有问题可咨询创利投的客服！谢谢！";
                        pm.PubTime = dti;
                        pm.Mtype = 1;
                        B_usercenter.AddMessage(pm);
                        #endregion

                        #region MyRegion//短信通知
                        string contxt = Utils.GetMSMEmailContext(15, 1); // 获取注册成功邮件内容
                        StringBuilder sbsms = new StringBuilder(contxt);
                        sbsms = sbsms.Replace("#USERANEM#", dt.Rows[0]["username"].ToString());
                        sbsms = sbsms.Replace("#PID#", targetid);
                        sbsms = sbsms.Replace("#MONEY#", investAmt.ToString());
                        string mobile = dt.Rows[0]["mobile"].ToString();
                        M_td_SMS_record psms = new M_td_SMS_record();
                        B_td_SMS_record osms = new B_td_SMS_record();
                        int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.投资成功.ToString());
                        psms.phone_number = mobile;
                        psms.sendtime = DateTime.Now;
                        psms.senduserid = registerid;
                        psms.smstype = smstype;
                        psms.smscontext = sbsms.ToString();
                        psms.orderid = SendSMS.Send_SMS(mobile, sbsms.ToString());
                        psms.vcode = "";
                        osms.Add(psms);
                        #endregion

                        #region 远程调用生成合同？？？ 稍后替换为本地方法调用  微信端可远程调用
                        string postString = "action=MUserPDF&data=" + targetid.ToString() + "&uc=" + registerid.ToString() + "&OrdId=" + OrdId;
                        string sr = Utils.PostWebRequest(Utils.GetRemote_url("pdf/index"), postString, Encoding.UTF8);
                        #endregion

                        #region 渠道合作 第一投标调用接口？？？
                        B_member_table bmt = new B_member_table();
                        M_member_table mmt = new M_member_table();
                        mmt = bmt.GetModel(registerid);
                        if (mmt.Tid != null && mmt.Channelsource == 1)
                        {
                            if (B_usercenter.GetInvestCountByUserid(mmt.registerid) == 1)
                            {
                                string ret3 = Utils.GetCoopAPI(mmt.Tid, investAmt.ToString("0.00"), 2);
                                LogInfo.WriteLog("前台渠道合作第一次返回结果:" + ret3 + "  用户id:" + mmt.registerid + " 订单id " + OrdId);
                            }
                        }
                        #endregion
                        #endregion 待提取为公共方法

                        //发放奖励
                        ActFacade act = new ActFacade();
                        act.SendBonusAfterInvest(dt, EnumCommon.E_hx_ActivityTable.E_ActTargetPlatform.web);
                    }
                }
            }
        }

        [AdminVaildate()]
        public ActionResult MyborrowidList(string Username = "", string userTel = "", string time1 = "", string time2 = "", int BorrType = -1, int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<hx_td_Myborrow, bool>> where = PredicateExtensionses.True<hx_td_Myborrow>();
            where = where.And(p => p.Myborrowid > 0);
            if (!string.IsNullOrEmpty(Username))
            {
                where = where.And(p => p.Username.Contains(Username));
            }
            if (!string.IsNullOrEmpty(userTel))
            {
                where = where.And(p => p.userTel.Contains(userTel));
            }
            if (!string.IsNullOrEmpty(time1))
            {
                DateTime dt1 = Convert.ToDateTime(time1);
                where = where.And(a => ((DateTime)a.EntryTime).CompareTo(dt1) >= 0);
            }

            if (!string.IsNullOrEmpty(time2))
            {
                DateTime dt2 = Convert.ToDateTime(time2);
                dt2 = Convert.ToDateTime(dt2.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(a => ((DateTime)a.EntryTime).CompareTo(dt2) <= 0);
            }

            if (BorrType > -1)
            {
                where = where.And(p => p.BorrType == BorrType);
            }

            IPagedList<hx_td_Myborrow> list = ef.hx_td_Myborrow.Where(where).OrderByDescending(p => p.Myborrowid).ToPagedList(pageNumber, pageSize);

            ViewBag.Username = Username;
            ViewBag.userTel = userTel;
            ViewBag.time1 = time1;
            ViewBag.time2 = time2;
            ViewBag.BorrType = BorrType;

            return View(list);
        }










    }
}