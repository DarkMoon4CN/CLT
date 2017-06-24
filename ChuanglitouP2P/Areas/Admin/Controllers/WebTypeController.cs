using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuanglitouP2P.DBUtility;
using ChuangLitouP2P.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class WebTypeController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/WebType
        [AdminVaildate(false,true)]
        public ActionResult Index(int rootid = 0)
        {

            var list_web_type = (from a in ef.hx_td_web_type orderby a.rootid, a.orderid, a.menu_id select a).ToList<hx_td_web_type>();
            if (rootid > 0)
            {
                list_web_type = (from a in ef.hx_td_web_type orderby a.rootid, a.orderid, a.menu_id where a.rootid == rootid select a).ToList<hx_td_web_type>();
            }
            
            ViewBag.web_types = list_web_type;

            ViewBag.rootid = rootid;

            return View();
        }

        [AdminVaildate()]
        public ActionResult Add(int rootid = 0, int parentid = 0)
        {
            ViewBag.rootid = rootid;
            ViewBag.parentid = parentid;
            var list_web_type = (from a in ef.hx_td_web_type orderby a.rootid, a.orderid, a.menu_id where a.rootid == rootid select a).ToList<hx_td_web_type>();
            ViewBag.web_types = list_web_type;

            return View();
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Addpost(hx_td_web_type p)
        {
            ChuanglitouP2P.BLL.B_td_web_type b1 = new BLL.B_td_web_type();
            ChuanglitouP2P.Model.M_td_web_type obj = new Model.M_td_web_type();

            int rootid = DNTRequest.GetInt("rootid", 0);
            int isresult = 0;


            int ClassID = 0;
            string ClassName;
            int PrevOrderID;
            int RootID = 0;
            int ParentDepth;
            string ParentPath;
            string ParentName;

            int depath;

            int PrevID;

            int Child;
            int ParentID;


            PrevOrderID = 0;
            ParentPath = "0";
            ParentName = "";
            ParentDepth = 0;
            PrevID = 0;

            Child = 0;
            ParentPath = "";
 
            ParentID = Convert.ToInt16(Request.Form["parentid"]);
           ClassName = DNTRequest.GetString("menu_name");

            ClassID = b1.GetMaxId();

            if (ParentID > 0)
            {
                SqlDataReader sdr = DbHelperSQL.Re_dr("select * From hx_td_web_type where menu_id=" + ParentID.ToString() + "");

                if (sdr.Read() == false)
                {
                    Response.Write("<script>alert(\"所属栏目已经被删除！\");history.back();</script>");
                }
                else
                {
                    RootID = Convert.ToInt32(sdr["RootID"]);
                    ParentName = sdr["menu_name"].ToString();
                    ParentDepth = Convert.ToInt32(sdr["Depath"]);
                    ParentPath = sdr["ParentPath"].ToString();
                    Child = Convert.ToInt32(sdr["Child"]);
                    // ParentPath = ParentPath + "," + ParentID;    //得到此栏目的父级栏目路径
                    PrevOrderID = Convert.ToInt32(sdr["OrderID"]);
                    sdr.Close();
                    sdr.Dispose();


                }
                sdr.Close();
                sdr.Dispose();
            }
            else
            {
                RootID = ClassID;
            }

            // p = (hx_td_web_type)Utils.ValidateModelClass(p);
            if (ParentID > 0)
            {
                depath = ParentDepth + 1;
            }
            else
            {
                depath = 0;
            }
            if (ParentPath == "")
            {
                ParentPath = "," + ClassID.ToString() + ",";
            }
            else
            {
                ParentPath += "," + ClassID.ToString() + ",";
            }

            try
            {


                obj.menu_id = ClassID;// +i;
                obj.menu_name = ClassName;// +i.ToString();
                obj.parentid = ParentID;
                obj.parentpath = ParentPath;
                obj.depath = depath;
                obj.rootid = rootid;
                obj.child = 0;
                obj.previd = PrevID;
                obj.nextid = 0;

                obj.createtime = DateTime.Now;
                obj.orderid = DNTRequest.GetInt("orderid", 1);
                obj.path1 = DNTRequest.GetString("path1");

               


                isresult = b1.Add(obj);

            }
            catch (Exception ee)
            {
                Response.Write(ee.Message.ToString());

                
            }

            if (ParentID > 0)
            {
                //更新其父类的子栏目数
                DbHelperSQL.RunSql("update hx_td_web_type set child=child+1 where menu_id=" + ParentID.ToString());
            }
            else if(ParentID==0)
            {
                //DbHelperSQL.RunSql("update hx_td_web_type set rootid=" + isresult + ",parentpath='," + isresult + ",' where menu_id=" + isresult.ToString());
                DbHelperSQL.RunSql("update hx_td_web_type set parentpath='," + isresult + ",' where menu_id=" + isresult.ToString());
            }
            //Response.Write("<script>alert(\"添加成功\");location.href='Add_Edit_web_type.aspx?ParentID=" + ParentID.ToString() + "'</script>");

            //p.createtime = DateTime.Now;
            //p.menu_name = DNTRequest.GetString("menu_name");
            //p.path1 = DNTRequest.GetString("path1");
            //p.orderid = DNTRequest.GetInt("orderid", 0);

           
            //ef.hx_td_web_type.Add(p);
            //ef.SaveChanges();
        

            string str = "";

            if(rootid>0)
            {
                str = StringAlert.Alert("网站分类添加成功!", "/Admin/WebType/index?rootid=" + rootid);
            }
            else
            {
                str = StringAlert.Alert("网站分类添加成功!", "/Admin/WebType/index?rootid=" + isresult);
            }
            

            return Content(str, "text/html");
        }


 

        [HttpGet]
        // GET: Admin/WebType
        [AdminVaildate()]
        public ActionResult Edit(int id = 0, int rootid = 0)
        {

            ViewBag.rootid = rootid;

            var list_web_type = (from a in ef.hx_td_web_type orderby a.rootid, a.orderid, a.menu_id where a.rootid == rootid select a).ToList<hx_td_web_type>();
            ViewBag.web_types = list_web_type;
           
            var model = ef.hx_td_web_type.Where(p => p.menu_id == id).SingleOrDefault();

            return View(model);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Editpost(hx_td_web_type p, int rootid = 0, int id = 0)
        {
            ChuanglitouP2P.BLL.B_td_web_type b1 = new BLL.B_td_web_type();
            ChuanglitouP2P.Model.M_td_web_type obj = new Model.M_td_web_type();
             

            
            string ClassName;
            string path1;
            int orderid = 1;
            int ParentID;


           

            ParentID = Convert.ToInt16(Request.Form["parentid"]);
            ClassName = DNTRequest.GetString("menu_name");

            orderid = DNTRequest.GetInt("orderid", 1);
            path1 = DNTRequest.GetString("path1");


            DbHelperSQL.RunSql("update hx_td_web_type set menu_name='" + ClassName + "',path1='" + path1 + "',orderid=" + orderid + " where menu_id=" + id.ToString());

            string str = "";

            str = StringAlert.Alert("网站分类编辑成功!", "/Admin/WebType/index?rootid=" + rootid);

            return Content(str, "text/html");

        }


        [AdminVaildate()]
        public ActionResult DelById(int id, int rootid = 0)
        {
            string str = "";

            hx_td_web_type pDel = new hx_td_web_type() { menu_id = id };
            ef.hx_td_web_type.Attach(pDel);
            ef.hx_td_web_type.Remove(pDel);
          
            int i = ef.SaveChanges();
            if (i > 0)
            {

                str = StringAlert.Alert("网站分类删除成功!", "/admin/WebType/Index?rootid=" + rootid.ToString());
            }
            else
            {
                str = StringAlert.Alert("网站分类删除失败!", "/admin/WebType/Index?rootid=" + rootid.ToString());
            }
            return Content(str, "text/html");

        }

    }
}