using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model.chinapnr.NetSave;
using ChuangLitouP2P.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class BondingCompanyController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/BondingCompany
        [AdminVaildate()]
        public ActionResult Index(string company_name = "",  int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<hx_bonding_company, bool>> where = PredicateExtensionses.True<hx_bonding_company>();
            where = where.And(p => p.companyid > 0);

         

            if (!string.IsNullOrEmpty(company_name))
            {
                where = where.And(p => p.company_name.Contains(company_name));
            }

            IPagedList<hx_bonding_company> list = ef.hx_bonding_company.Where(where).OrderByDescending(p => p.companyid).ToPagedList(pageNumber, pageSize);

            ViewBag.company_name = company_name;
            ViewBag.page = Page;
            return View(list);
        }


        [AdminVaildate()]
        public ActionResult Add()
        {
            return View();
        }

        [HttpGet]
        [AdminVaildate()]
        public ActionResult Edit(int id = 0)
        {
            if (id < 1)
            {
                return Content(StringAlert.Alert("参数错误", "/admin/BondingCompany/Index"), "text/html");
            }
            var model = ef.hx_bonding_company.Where(p => p.companyid == id).SingleOrDefault();
            if (model == null || model.companyid<1)
            {
                return Content(StringAlert.Alert("记录不存在", "/admin/BondingCompany/Index"), "text/html");
            }
            return View(model);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate(false, true)]
        public ActionResult AddPost(hx_bonding_company p,string company_profile)
        {

            p = (hx_bonding_company)Utils.ValidateModelClass(p);
            p.company_profile = company_profile;
            ef.hx_bonding_company.Add(p);
            //ef.SaveChanges();
            string str = "";
            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("担保公司添加成功!", "/admin/BondingCompany/Index");
            }
            else
            {
                str = StringAlert.Alert("担保公司添加失败!", "/admin/BondingCompany/Add/");
            }
            return Content(str, "text/html");

        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate(false, true)]
        public ActionResult EditPost(hx_bonding_company p, int page = 1,string company_profile="")
        {
            string str = "";

            string[] proNames;

            proNames = new string[] { "company_name", "registered_capital", "Date_incorporation", "company_address", "company_profile", "business_licence", "business_certificate" };


            p = (hx_bonding_company)Utils.ValidateModelClass(p);
            p.company_profile = company_profile;


            DbEntityEntry entry = ef.Entry<hx_bonding_company>(p);
            entry.State = EntityState.Unchanged;

            foreach (string ProName in proNames)
            {
                entry.Property(ProName).IsModified = true;
            }

            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("担保公司修改成功!", "/Admin/BondingCompany/index?page=" + page);
            }
            else
            {
                str = StringAlert.Alert("担保公司修改失败!", "/admin/BondingCompany/Edit?id=" + p.companyid);
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
        [AdminVaildate()]
        public ActionResult DelById(int id, int Page = 1, string company_name = "")
        {
            string str = "";

            hx_bonding_company pDel = new hx_bonding_company() { companyid = id };
            ef.hx_bonding_company.Attach(pDel);
            ef.hx_bonding_company.Remove(pDel);
            int i = ef.SaveChanges();
            if (i > 0)
            {

                str = StringAlert.Alert("担保公司删除成功!", "/admin/BondingCompany/Index?page=" + Page.ToString() + "&company_name=" + company_name);
            }
            else
            {
                str = StringAlert.Alert("担保公司删除失败!", "/admin/BondingCompany/Index?page=" + Page.ToString() + "&company_name=" + company_name);
            }
            return Content(str, "text/html");

        }


        /// <summary>
        /// 担保公司第三方开户申请接口
        /// </summary>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public ActionResult Openbonding(int Cid)
        {
            string sql = "select companyid,company_name,registered_capital,Date_incorporation,company_address,company_profile,business_licence,business_certificate,contract_covers,contract_bottom,legal_representative,agent,agent_name,agent_id_card,createtime,Tax_NO,GuarType,UsrCustId,AuditStat,TrxId,OpenBandkId,CardId,UsrId,Tax_NO from hx_bonding_company where  companyid=" + Cid.ToString() + " order by  companyid asc";     
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            Model.chinapnr.CorpRegister.M_CorpRegister m = new Model.chinapnr.CorpRegister.M_CorpRegister();
            if (dt.Rows.Count > 0)
            {
                m.Version = "10";
                m.CmdId = "CorpRegister";
                m.MerCustId = Utils.GetMerCustID();
                m.UsrId = "ChuangLitou"+dt.Rows[0]["companyid"].ToString();
                m.UsrName = dt.Rows[0]["agent_name"].ToString();
                m.InstuCode = dt.Rows[0]["business_certificate"].ToString();
                m.BusiCode = dt.Rows[0]["business_licence"].ToString();
                m.TaxCode = dt.Rows[0]["Tax_NO"].ToString();
                m.MerPriv = dt.Rows[0]["companyid"].ToString();
                string gt = "N";
                if (dt.Rows[0]["GuarType"].ToString() == "1")
                {
                    gt = "Y";
                }
                m.GuarType = gt;
                m.BgRetUrl = Utils.GetRe_url("admin/Thirdparty/BgCorpRegister");
                m.ReqExt = dt.Rows[0]["companyid"].ToString();
                StringBuilder chkVal = new StringBuilder();
                chkVal.Append(m.Version);
                chkVal.Append(m.CmdId);
                chkVal.Append(m.MerCustId);               
                chkVal.Append(HttpUtility.UrlEncode(m.UsrId));
                // chkVal.Append(m.UsrName);
                chkVal.Append(m.InstuCode);
                chkVal.Append(m.BusiCode);
                chkVal.Append(m.TaxCode);
                chkVal.Append(m.MerPriv);
                chkVal.Append(m.GuarType);
                chkVal.Append(m.BgRetUrl);
               // chkVal.Append(m.ReqExt);
                string chkv = chkVal.ToString();
                LogInfo.WriteLog("签名:" + chkv);
                //私钥文件的位置(这里是放在了站点的根目录下)
                string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
                //需要指定提交字符串的长度
                int len = Encoding.UTF8.GetBytes(chkv).Length;
                StringBuilder sbChkValue = new StringBuilder(256);
                //加签
                int str1 = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

                LogInfo.WriteLog(str1.ToString());

                m.ChkValue = sbChkValue.ToString();


                StringBuilder strz = new StringBuilder();

                strz.Append(" <form id=\"formauto\" name=\"formauto\"  action=\"" + Utils.GetChinapnrUrl() + "\" method=\"post\">");

                strz.Append("<input id=\"Version\"  name=\"Version\"  type=\"hidden\"  value=\"" + m.Version + "\" />");

                strz.Append("<input id=\"CmdId\"  name=\"CmdId\"    type=\"hidden\"  value=\"" + m.CmdId + "\" />");

                strz.Append("<input id=\"MerCustId\" name=\"MerCustId\"   type=\"hidden\"  value=\"" + m.MerCustId + "\" />");

                strz.Append("<input id=\"UsrId\" name=\"UsrId\"   type=\"hidden\"  value=\"" + m.UsrId + "\" />");

                //  strz.Append("<input id=\"UsrName\" name=\"UsrName\"   type=\"hidden\"  value=\"" + m.UsrName + "\" />");
                strz.Append("<input id=\"InstuCode\" name=\"InstuCode\"   type=\"hidden\"  value=\"" + m.InstuCode + "\" />");



                strz.Append("<input id=\"BusiCode\" name=\"BusiCode\" type=\"hidden\"  value=\"" + m.BusiCode + "\" />");

                strz.Append("<input id=\"TaxCode\" name=\"TaxCode\" type=\"hidden\"  value=\"" + m.TaxCode + "\" />");

                strz.Append("<input id=\"MerPriv\" name=\"MerPriv\" type=\"hidden\"  value=\"" + m.MerPriv + "\" />");

                strz.Append("<input id=\"GuarType\" name=\"GuarType\" type=\"hidden\"  value=\"" + gt + "\" />");

                strz.Append("<input id=\"BgRetUrl\"  name=\"BgRetUrl\" type=\"hidden\"  value=\"" + m.BgRetUrl + "\" />");


               // strz.Append("<input id=\"ReqExt\"  name=\"ReqExt\" type=\"hidden\"  value=\"" + m.ReqExt + "\" />");

                strz.Append("<input id=\"ChkValue\"  name=\"ChkValue\" type=\"hidden\"  value=\"" + m.ChkValue + "\" />");


                strz.Append(" </form>");
                strz.Append("<script type=\"text/javascript\">document.getElementById('formauto').submit();</script>");

                LogInfo.WriteLog(strz.ToString());

                //Response.Write(strz.ToString());

                ViewBag.str = strz.ToString();

            }

             

            return View();

        }


        public ActionResult CheckName(string param, int key = 0)
        {
            var item = (from a in ef.hx_bonding_company where a.companyid != key && a.company_name == param select a).SingleOrDefault();
            if (item != null && item.companyid > 0)
            {
                return Content("担保公司名称已存在");
            }
            return Content("y");
        }

        

    }
}