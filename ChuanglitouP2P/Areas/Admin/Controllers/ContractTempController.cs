using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuangLitouP2P.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using EntityFramework.Extensions;
using ChuanglitouP2P.BLL.EF;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class ContractTempController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/ContractTemp
        [AdminVaildate()]
        public ActionResult Index(string contract_template_name = "", int contract_type_id = -1, int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<V_contract_type_template, bool>> where = PredicateExtensionses.True<V_contract_type_template>();
            where = where.And(p => p.contract_template_id > 0);

            if (!String.IsNullOrEmpty(contract_template_name))
            {
                where = where.And(p => p.contract_template_name.Contains(contract_template_name));
            }
            if (contract_type_id >= 0)
            {
                where = where.And(p => p.contract_type_id == contract_type_id);
            }
            IPagedList<V_contract_type_template> list = ef.V_contract_type_template.Where(where).OrderByDescending(p => p.contract_template_id).ToPagedList(pageNumber, pageSize);

            //IEnumerable<SelectListItem> list_ConType = ef.hx_contract_type.OrderByDescending(p => p.contract_type_id).Select(p => new SelectListItem { Value = p.contract_type_id.ToString(), Text = p.contract_type_name });

            ViewBag.list_ConType = GetContractTypeList();
            ViewBag.contract_template_name = contract_template_name;
            ViewBag.contract_type_id = contract_type_id;

            return View(list);
        }

        private IEnumerable<SelectListItem> GetContractTypeList()
        {
            List<SelectListItem> list_ConType = new List<SelectListItem>();
            var list = ef.hx_contract_type.OrderByDescending(p => p.contract_type_id);
            list_ConType.Add(new SelectListItem { Value = "-1", Text = "请选择类型" });
            if (list != null && list.Count() > 0)
            {
                foreach (hx_contract_type item in list)
                {
                    list_ConType.Add(new SelectListItem { Value = item.contract_type_id.ToString(), Text = item.contract_type_name });
                }
            }

            return list_ConType;
        }

        /// <summary>
        /// 模板
        /// </summary>
        /// <returns></returns>
        [AdminVaildate(false)]
        public ActionResult Template(int id = 0)
        {
            hx_Contract_template temp = new hx_Contract_template();
            if (id > 0)
            {
                temp = ef.hx_Contract_template.SingleOrDefault(p => p.contract_template_id == id);
            }
            IEnumerable<SelectListItem> list_ConType = ef.hx_contract_type.OrderByDescending(p => p.contract_type_id).Select(p => new SelectListItem { Value = p.contract_type_id.ToString(), Text = p.contract_type_name });

            ViewBag.list_ConType = list_ConType;
            ViewBag.id = id;
            return View(temp);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate()]
        public ActionResult TemplateEdit(int contract_template_id, int contract_type_id, string contract_template_name, string content1)
        {
            string str;
            hx_Contract_template p = new hx_Contract_template();
            if (contract_template_id == 0)
            {
                p.cretatetime = DateTime.Now;
                p.usestate = 1;
            }
            else
            {
                p = (from a in ef.hx_Contract_template where a.contract_template_id == contract_template_id select a).FirstOrDefault();
            }
            p.contract_template_id = contract_template_id;
            p.contract_template_context = content1;
            p.contract_type_id = contract_type_id;
            p.contract_template_name = contract_template_name;

            if (IsExist(contract_template_name, contract_template_id))
            {
                str = StringAlert.Alert("模板名称已经存在!");
            }
            else
            {
                bool result = false;
                if (contract_template_id == 0)
                {
                    result = Add(p);
                }
                else
                {
                    result = Editr(p);
                }

                if (result)
                {
                    str = StringAlert.Alert("操作成功!", "/admin/ContractTemp/Index");
                }
                else
                {
                    str = StringAlert.Alert("操作失败!", "/admin/ContractTemp/Template?id=" + contract_template_id);
                }
            }
            return Content(str, "text/html");
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool Add(hx_Contract_template t)
        {
            t = (hx_Contract_template)Utils.ValidateModelClass(t);
            ef.hx_Contract_template.Add(t);
            ef.SaveChanges();

            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool Editr(hx_Contract_template p)
        {
            //string[] proNames;

            //proNames = new string[] { "contract_type_id", "contract_template_name", "contract_template_context", "usestate" };
            p = (hx_Contract_template)Utils.ValidateModelClass(p);

            //DbEntityEntry entry = ef.Entry<hx_Contract_template>(p);
            //entry.State = EntityState.Unchanged;

            //foreach (string ProName in proNames)
            //{
            //    entry.Property(ProName).IsModified = true;
            //}
            //int i = ef.SaveChanges();

            int i = ef.hx_Contract_template.Where(a => a.contract_template_id == p.contract_template_id).Update(a => new hx_Contract_template { contract_type_id=p.contract_type_id, contract_template_name=p.contract_template_name, contract_template_context=p.contract_template_context });

            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// 判断模板名称是否存在
        /// </summary>
        /// <param name="param"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public ActionResult CheckTemplateName(string param, int key = 0)
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
            var list = (from a in ef.hx_Contract_template where a.contract_template_name == name select a).ToList<hx_Contract_template>();
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.contract_template_id != key)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult ModifyStateById(int id, int state = 0)
        {
            try
            {
                if (id < 1)
                {
                    return Content(StringAlert.Alert("参数错误!"), "text/html");
                }
                var ct = ef.hx_Contract_template.Single(p => p.contract_template_id == id);
                if (ct == null || ct.contract_template_id < 1)
                {
                    return Content(StringAlert.Alert("参数错误!"), "text/html");
                }
                if (state == 0)
                {
                    ct.usestate = 0;
                }
                else
                {
                    ct.usestate = 1;
                }
                DbEntityEntry entry = ef.Entry<hx_Contract_template>(ct);
                entry.State = EntityState.Modified;

                ef.SaveChanges();
            }
            catch
            {

                throw;
            }
           
            return RedirectToAction("index", "ContractTemp");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult DelTempliate(int id)
        {
            var controllerName = this.RouteData.Values["controller"].ToString();
            var actionName = this.RouteData.Values["action"].ToString();

            var userid = Utils.GetAdmUserID();
            if (!new UserLimitByEF().CheckAdminLimit(userid, controllerName, actionName))
            {   //无权限                
                return Content(StringAlert.Alert("您没有操作权限!"), "text/html");
            }
            if (id < 1)
            {
                return Content(StringAlert.Alert("参数错误!"), "text/html");
            }
            hx_Contract_template pDel = new hx_Contract_template() { contract_template_id = id };
            ef.hx_Contract_template.Attach(pDel);
            ef.hx_Contract_template.Remove(pDel);
            int i = ef.SaveChanges();
            if (i > 0)
            {
                return Content(StringAlert.Alert("删除成功!", "/admin/ContractTemp/Index"), "text/html");
            }
            else
            {
                return Content(StringAlert.Alert("删除失败!"), "text/html");
            }
        }


    }
}