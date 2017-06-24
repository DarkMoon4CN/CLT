using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Areas.Topic.Controllers
{
    public class T20170106Controller : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();
        private Dictionary<int, string> dict = new Dictionary<int, string>();
        // GET: Topic/T20170106
        public ActionResult Index()
        {

            int userid = Utils.checkloginsessiontop();
            string userUrl = string.Empty;
            if (userid > 0)
            {

                hx_member_table hUser = null;
                if (userid == 0)
                {
                    userUrl = "/usercenter/yaoqing";
                }
                else
                {
                    hUser = ef.hx_member_table.Where(p => p.registerid == userid).FirstOrDefault();
                }
                ViewBag.UserUrl = userUrl != string.Empty ? userUrl : Utils.GetRe_url("Invitation/" + hUser.invitedcode + ".html");

            }
            else
            {

                ViewBag.UserUrl = "/usercenter/yaoqing";


            }





            string codesql = "select TOP 5 (SELECT  mobile from hx_member_table where registerid = Invpeopleid) mobile,count(invcode) invcode from hx_td_Userinvitation where invtime >= '2017/1/6 00:00:00' AND invtime <= '2017/3/31 23:59:59' GROUP BY Invpeopleid ORDER BY  invcode desc";//查询累计邀请人数排名

            DataTable dtcode = DbHelperSQL.GET_DataTable_List(codesql);

            if (dtcode.Rows.Count > 0)
            {
                ViewBag.RenShu = dtcode;
            }
            else
            {
                ViewBag.RenShu = null;
            }

            string codesql2 = "SELECT TOP 5 (SELECT  t.mobile from hx_member_table t where t.registerid=a.registerid) mobile , SUM(Amt)amt FROM hx_UserAct a where RewTypeID = 1 AND Createtime >= '2017/1/6 00:00:00' AND Createtime <= '2017/3/31 23:59:59' AND  registerid IN(SELECT distinct Invpeopleid from hx_td_Userinvitation where invtime >= '2017/1/6 00:00:00' AND invtime <= '2017/3/31 23:59:59') GROUP BY registerid ORDER BY amt desc";//查询累计邀请返现排名

            DataTable dtcode2 = DbHelperSQL.GET_DataTable_List(codesql2);

            if (dtcode2.Rows.Count > 0)
            {
                ViewBag.JinE = dtcode2;
            }
            else
            {
                ViewBag.JinE = null;
            }



            return View();
        }
    }
}