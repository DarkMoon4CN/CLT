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

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class ActivityScheduleController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/ActivitySchedule

        [AdminVaildate(false)]
        public ActionResult Index(string activity_schedule_name = "", int reward = -1, int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<hx_Activity_schedule, bool>> where = PredicateExtensionses.True<hx_Activity_schedule>();
            where = where.And(p => p.activity_schedule_id > 0);

            if(reward>-1)
            {
                where = where.And(p => p.reward == reward);
            }


            if (!string.IsNullOrEmpty(activity_schedule_name))
            {
                where = where.And(p => p.activity_schedule_name.Contains(activity_schedule_name));
            }

            IPagedList<hx_Activity_schedule> list = ef.hx_Activity_schedule.Where(where).OrderByDescending(p => p.activity_schedule_id).ToPagedList(pageNumber, pageSize);

            ViewBag.activity_schedule_name = activity_schedule_name;
            ViewBag.reward = reward;
            ViewBag.page = Page;
            return View(list);
        }

        [AdminVaildate(false)]
        public ActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
  
            var model = ef.hx_Activity_schedule.Where(p => p.activity_schedule_id == id).SingleOrDefault();

            return View(model);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult AddPost(hx_Activity_schedule p)
        {

            p = (hx_Activity_schedule)Utils.ValidateModelClass(p);
            ef.hx_Activity_schedule.Add(p);
            //ef.SaveChanges();
            string str = "";
            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("活动计划添加成功!", "/admin/ActivitySchedule/Index");
            }
            else
            {
                str = StringAlert.Alert("活动计划添加失败!", "/admin/ActivitySchedule/Add/");
            }
            return Content(str, "text/html");

        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult EditPost(hx_Activity_schedule p,  int page = 1)
        {
            string str = "";

            string[] proNames;

            proNames = new string[] { "activity_schedule_name", "amount_of_reward", "use_lower_limit", "reward", "start_date", "end_date" };


            p = (hx_Activity_schedule)Utils.ValidateModelClass(p);

           

            DbEntityEntry entry = ef.Entry<hx_Activity_schedule>(p);
            entry.State = EntityState.Unchanged;

            foreach (string ProName in proNames)
            {
                entry.Property(ProName).IsModified = true;
            }

            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("活动计划修改成功!", "/Admin/ActivitySchedule/index?page=" + page );
            }
            else
            {
                str = StringAlert.Alert("活动计划修改失败!", "/admin/ActivitySchedule/Edit?id=" + p.activity_schedule_id);
            }
            return Content(str, "text/html");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Page"></param>
        /// <param name="rootid"></param>
        /// <param name="news_title"></param>
        /// <returns></returns>
        [AdminVaildate(false)]
        public ActionResult DelById(int id, int Page = 1, int reward = -1, string activity_schedule_name = "")
        {
            string str = "";

            hx_Activity_schedule pDel = new hx_Activity_schedule() { activity_schedule_id = id };
            ef.hx_Activity_schedule.Attach(pDel);
            ef.hx_Activity_schedule.Remove(pDel);
            int i = ef.SaveChanges();
            if (i > 0)
            {

                str = StringAlert.Alert("活动计划删除成功!", "/admin/ActivitySchedule/Index?page=" + Page.ToString() + "&reward=" + reward.ToString() + "&activity_schedule_name=" + activity_schedule_name);
            }
            else
            {
                str = StringAlert.Alert("活动计划删除失败!", "/admin/ActivitySchedule/Index?page=" + Page.ToString() + "&reward=" + reward.ToString() + "&activity_schedule_name=" + activity_schedule_name);
            }
            return Content(str, "text/html");

        }
    }
}