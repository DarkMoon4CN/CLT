using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using System.Data;
using ChuanglitouP2P.DBUtility;
using System.Text;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class DefaultController : Controller
    {

        // GET: Admin/Default
        [AdminVaildate(false, true)]
        public ActionResult Index()
        {
          
            

            return View();
        }


        [AdminVaildate(false, true)]
        public ActionResult Home()
        {
            int uid = Utils.GetAdmUserID();

            string sql = "select adminuser,lastLoginTime,lastLoginIP,department_name from v_adminuser_department where adminuserid=" + uid.ToString();

            DataTable dt = new DataTable();

            dt = DbHelperSQL.GET_DataTable_List(sql);

            string infos = "";
            if (dt.Rows.Count > 0)
            {
                infos = " 您好，" + dt.Rows[0]["adminuser"].ToString() + "        <br />    所属部门：" + dt.Rows[0]["department_name"].ToString() + "      <br />   上次登录时间：" + dt.Rows[0]["lastLoginTime"].ToString() + "        <br />     上次登录IP：" + dt.Rows[0]["lastLoginIP"].ToString() + "   ";

            }

            StringBuilder str = new StringBuilder();

            string info1 = "";

            //等待初审的标
            sql = "select  COALESCE(count(targetid),0) as 初审数量  from V_borrowing_target_addlist  where  tender_state=0";

            info1 = DbHelperSQL.Re_String(sql);


            str.Append("<div style=\"float: left; width:300px;\">  等待初审的标[   <a href=\"/admin/Examine/waitverify\">  " + info1 + " </a>  ]个     </div>   <br />");


            //等待复审的标

            sql = "select  COALESCE(count(targetid),0) as 复审数量  from V_borrowing_target_addlist  where  tender_state=1";

            info1 = DbHelperSQL.Re_String(sql);
            str.Append("<div style=\"float: left; width:300px;\">  等待复审的标[   <a href=\"/admin/Examine/Rewaitverify\">  " + info1 + " </a>  ]个     </div>   <br />");


            //无效投资

            sql = "select COALESCE(count(bid_records_id),0) as 无效投资  from V_hx_Bid_records_borrowing_target_uc";

            info1 = DbHelperSQL.Re_String(sql);
            str.Append("<div style=\"float: left; width:300px;\">  无效投资的标[   <a href=\"/admin/DaiKuan/Invalidinvestment\">  " + info1 + " </a>  ]个     </div>   <br />");



            //等待审核提现

            sql = "select  COALESCE(count(UserCashId),0) as 审核提现  from  V_UserCash_Bank where OrdIdState=0";

            info1 = DbHelperSQL.Re_String(sql);
            str.Append("<div style=\"float: left; width:300px;\">  等待审核提现[   <a href=\"/admin/UserCash/Index?OrdIdState=0\">  " + info1 + " </a>  ]个     </div>   <br />");


            //等待付款

            sql = "select  COALESCE(count(UserCashId),0) as 等待付款  from  V_UserCash_Bank where OrdIdState=1";

            info1 = DbHelperSQL.Re_String(sql);
            str.Append("<div style=\"float: left; width:300px;\">  等待付款[   <a href=\"/admin/UserCash/Index?OrdIdState=1\">  " + info1 + " </a>  ]个     </div>   <br />");


            str.Append("<br><br>");


            ViewBag.infolist = str.ToString();

            ViewBag.infos = infos;

            return View();
        }


    }
}