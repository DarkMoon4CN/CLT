using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Areas.Topic.Controllers
{
    public class T20160910Controller : Controller
    {
        // GET: Topic/T20160910
        public ActionResult Index()
        {
            B_GrabIphone gi = new B_GrabIphone();
            ViewBag.ltrCurrentCount = gi.GetRecordCount("LuckDrawState=0") + "";
            ViewBag.getLDCount = getLDCount();
            ViewBag.getLJJRCount = getLJJRCount();
            ViewBag.getCYLIst = getCYLIst();
            ViewBag.getZJLIst = getZJLIst();


            return View();
        }

        /// <summary>
        /// 获取当前阶段投资人数
        /// </summary>
        /// <returns></returns>
        public double getLDCount()
        {
            B_GrabIphone gi = new B_GrabIphone();
            double count = Convert.ToDouble(gi.GetRecordCount("LuckDrawState=0"));
            int ljcount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["GrabIphone"].ToString());
            //LogInfo.WriteLog("count:"+ count+ ";ljcount:" + ljcount);//写入日志
            return (count / ljcount)*100;
        }
        /// <summary>
        /// 获取累计加入人数
        /// </summary>
        /// <returns></returns>
        public double getLJJRCount()
        {
            B_GrabIphone gi = new B_GrabIphone();
            int count = gi.GetRecordCount("");
            return count;
        }
        /// <summary>
        /// 已参与用户
        /// </summary>
        /// <returns></returns>
        public List<M_GrabIphone> getCYLIst()
        {
            B_GrabIphone gi = new B_GrabIphone();
            List<M_GrabIphone> giList = gi.GetModelList(30, "", "ID desc");
            return giList;
        }
        /// <summary>
        /// 已中奖用户
        /// </summary>
        /// <returns></returns>
        public List<M_GrabIphone> getZJLIst()
        {
            B_GrabIphone gi = new B_GrabIphone();
            List<M_GrabIphone> giList = gi.GetModelList(20, "WinningState=1", "ID desc");
            return giList;
        }

        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="registerid"></param>
        /// <returns></returns>
        public M_member_table getMemberInfo(int registerid)
        {
            B_member_table bmt = new B_member_table();
            M_member_table mmt = bmt.GetModel(registerid);
            return mmt;
        }
     
    }
}