using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Controllers
{
    public class GuaranteeController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();
        // GET: Guarantee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Agency(int id)
        {

            hx_bonding_company pbc = ef.hx_bonding_company.Where(p => p.companyid == id).FirstOrDefault();

            return View(pbc);
        }



    }
}