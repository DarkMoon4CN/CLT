using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.CashAudit;
using ChuangLitouP2P.Models;
using LLYTPay;
using Newtonsoft.Json.Linq;
using PagedList;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    /// <summary>
    /// 连连批付管理
    /// </summary>
    public class LLManageController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/LLManage
        [AdminVaildate()]
        public ActionResult Index(string username="",int OrdIdState=-1,int ddlType=4, int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<V_LL_Cash_User, bool>> where = PredicateExtensionses.True<V_LL_Cash_User>();
            where = where.And(p => p.LLcashid > 0);

            if (!String.IsNullOrEmpty(username))
            {
                where = where.And(p => p.acct_name.Contains(username));
            }
            if (OrdIdState>=0)
            {
                where = where.And(p => p.paystate == (OrdIdState));
            }
            IPagedList<V_LL_Cash_User> list = ef.V_LL_Cash_User.Where(where).OrderByDescending(p => p.LLcashid).ToPagedList(pageNumber, pageSize);

            ViewBag.username = username;
            ViewBag.OrdIdState = OrdIdState;
            ViewBag.ddlType = ddlType;

            return View(list);
        }

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="norid"></param>
        /// <param name="username"></param>
        /// <param name="OrdIdState"></param>
        /// <param name="ddlType"></param>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult PayMoney(string norid="", string username = "", int OrdIdState = -1, int ddlType = 4)
        {
            if (string.IsNullOrEmpty(norid))
            {
                return Content(StringAlert.Alert("参数错误!"), "text/html");
            }

            var list = ef.hx_td_LL_cash.Where(p => p.no_order == norid && p.paystate == 0);
            if (list!=null && list.Count()>0)
            {
                hx_td_LL_cash llc = list.ToList<hx_td_LL_cash>()[0];
                llc.paystate = 3;
                llc.OperTime = DateTime.Now;

                ef.hx_td_LL_cash.Add(llc);
                ef.SaveChanges();
            }

            return RedirectToAction("index", new { username = username , OrdIdState = OrdIdState , ddlType = ddlType });
        }

        /// <summary>
        /// 批量复核
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminVaildate()]
        public ActionResult LLCashProcessingmore(string str2)
        {
            string json = "";
            if (string.IsNullOrEmpty(str2))
            {
                json = @"{""ret"":0,""msg"":""参数错误""}";
            }
            // json = @"{""ret"":-1,""msg"":""功能待完善""}";

            string bid = str2;

            string[] s = bid.Split(new char[] { ',' });

            string sqllist = "";

            for (int i = 0; i < s.Length; i++)
            {
                if (i == s.Length - 1)
                {
                    sqllist = sqllist + "'" + s[i] + "'";
                }
                else
                {

                    sqllist = sqllist + "'" + s[i] + "',";
                }

            }


            string sql = " select LLcashid,no_order,dt_order,money_order,acct_name,province_code,city_code,brabank_name,ordertime,card_no,paystate,Usrid,BankName,bank_code from V_LL_Cash_User where    LLcashid in (" + sqllist + ")    and  h_state=1 and  paystate=3";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);


            int succ = 0, lost = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                M_LLPay m = new M_LLPay();
                m.Acct_name = dt.Rows[i]["acct_name"].ToString();
                m.Card_no = dt.Rows[i]["card_no"].ToString();
                m.No_order = dt.Rows[i]["no_order"].ToString();
                m.Dt_order = dt.Rows[i]["dt_order"].ToString();
                m.Money_order = dt.Rows[i]["money_order"].ToString();
                m.City_code = dt.Rows[i]["city_code"].ToString();
                m.Bank_code = dt.Rows[i]["bank_code"].ToString();
                m.Brabank_name = dt.Rows[i]["brabank_name"].ToString();


                sql = "update  hx_td_LL_cash  set  paystate=5  where  paystate=3  and   LLcashid in (" + sqllist + ") ";
                DbHelperSQL.RunSql(sql);

               BLL.EF.LLpay llp = new BLL.EF.LLpay();

                string str = llp.cashpay(m);

                string no_order = dt.Rows[i]["no_order"].ToString();

                var Objlist = JObject.Parse(str);




                if (Objlist["ret_code"].ToString() == "0000")
                {
                    sql = "update  hx_td_LL_cash  set  paystate=1,OperTime='" + DateTime.Now.ToString() + "'  where  paystate=5  and  no_order='" + no_order + "'";
                    DbHelperSQL.RunSql(sql);
                    //  CommonOperate.Show_Msg("批付成功");
                    YinTongUtil.writelog(no_order + "批量批付成功sql=" + sql);

                    succ = succ + 1;
                    //Response.End();

                }
                else
                {
                    //  CommonOperate.Show_Msg("批付失败");
                    sql = "update  hx_td_LL_cash  set  paystate=2,OperTime='" + DateTime.Now.ToString() + "'  where  paystate=5  and  no_order='" + no_order + "'";
                    DbHelperSQL.RunSql(sql);
                    //  CommonOperate.Show_Msg("批付成功");
                    YinTongUtil.writelog(no_order + "批付失败sql=" + sql);

                    lost = lost + 1;
                }


               // Response.Write(dt.Rows[i]["card_no"].ToString() + str + "<br>");


                YinTongUtil.writelog(dt.Rows[i]["card_no"].ToString() + "批处理付款:" + str);

            }

            json = @"{""ret"":1,""msg"":""成功AB笔,失败BB笔""}";
            json = json.Replace("AB", succ.ToString()).Replace("BB", lost.ToString());
            return Content(json, "text/json");
        }

        /// <summary>
        /// 重新支付
        /// </summary>
        /// <param name="no_order"></param>
        /// <param name="bu"></param>
        /// <returns></returns>
        [AdminVaildate(false,true)]
        public ActionResult ReLLCash(string no_order,int bu)
        {
            //原页面路径：/admin/ReLLCash.aspx
            string json = "";
            if (string.IsNullOrEmpty(no_order)|| bu<=0)
            {
                json = @"{""ret"":0,""msg"":""参数错误""}";
            }
            if (bu > 0 && no_order != "")
            {


                //生成连连新支付单号
             string   LLNew_ordid = YinTongUtil.getCurrentDateTimeStr();

             string   sql = " select Acct_name,Card_no,no_order,dt_order,money_order,city_code,bank_code,brabank_name from hx_td_LL_cash where paystate=2  and  no_order='" + no_order + "' ";



                // sql = "  select  OutAcctId,InAcctId,TransAmt from  hx_td_LLpay_re_cash where   htype=0 and h_state=0 and OrdId='" + OrdId + "' and   no_order='" + no_order + "'";

                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

                if (dt.Rows.Count > 0)
                {

                 string    Remarks = "原订单号:" + no_order + " 支付失败 替新订单 " + LLNew_ordid + " 重新支付 <br>";

                    sql = "update  hx_td_LL_cash set  no_order='" + LLNew_ordid + "' ,Remarks='" + Remarks + "'    where   paystate=2  and  no_order='" + no_order + "'";

                    DbHelperSQL.RunSql(sql);

                    sql = " update hx_td_LLpay_re_cash set no_order='" + LLNew_ordid + "'  where htype=1  and  no_order='" + no_order + "'";

                    DbHelperSQL.RunSql(sql);


                    M_LLPay m = new M_LLPay();
                    m.Acct_name = dt.Rows[0]["acct_name"].ToString();
                    m.Card_no = dt.Rows[0]["card_no"].ToString();
                    m.No_order = LLNew_ordid;
                    m.Dt_order = dt.Rows[0]["dt_order"].ToString();
                    m.Money_order = dt.Rows[0]["money_order"].ToString();
                    m.City_code = dt.Rows[0]["city_code"].ToString();
                    m.Bank_code = dt.Rows[0]["bank_code"].ToString();
                    m.Brabank_name = dt.Rows[0]["brabank_name"].ToString();

                    sql = "update  hx_td_LL_cash  set  paystate=5 where  paystate=2  and  no_order='" + LLNew_ordid + "'";
                    DbHelperSQL.RunSql(sql);

                    BLL.EF.LLpay llp = new BLL.EF.LLpay();

                    string str = llp.cashpay(m);


                    Response.Write(str);

                    YinTongUtil.writelog("单个重新批付及时处理" + no_order + "批付成功  str=" + str);

                    var Objlist = JObject.Parse(str);

                    if (Objlist["ret_code"].ToString() == "0000")
                    {
                        sql = "update  hx_td_LL_cash  set  paystate=1 ,OperTime='" + DateTime.Now.ToString() + "' where  paystate= 5  and  no_order='" + LLNew_ordid + "'";
                        DbHelperSQL.RunSql(sql);

                        json = @"{""ret"":1,""msg"":""重新批付成功""}";
                        // CommonOperate.Show_Msg("重新批付成功 :" + sql);
                        YinTongUtil.writelog("单个重新批付及时处理" + no_order + "批付成功  ");
                        Response.End();

                    }
                    else
                    {

                        sql = "update  hx_td_LL_cash  set  paystate=2 ,OperTime='" + DateTime.Now.ToString() + "' where  paystate= 5  and  no_order='" + LLNew_ordid + "'";
                        DbHelperSQL.RunSql(sql);

                        json = @"{""ret"":0,""msg"":""批付失败""}";
                        //CommonOperate.Show_Msg("批付失败");
                        Response.End();
                    }





                    YinTongUtil.writelog("前面处理付款:" + str);




                }


            }



            //json = @"{""ret"":-1,""msg"":""功能待完善""}";

            return Content(json, "text/json");
        }



        /// <summary>
        /// 支付
        /// </summary>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult LLCashProcessing(int LLcashid)
        {


            V_LL_Cash_User m = ef.V_LL_Cash_User.Where(p => p.LLcashid == LLcashid  && p.h_state == 1 &&  p.paystate == 3).FirstOrDefault();

            IEnumerable<SelectListItem> ddlList = Utils.GetEnumToList(typeof(EnumLLPayState)).Select(c => new SelectListItem { Value = c.key.ToString(), Text = c.value.ToString() });

            ViewBag.ddlList = ddlList;

            return View(m);
        }



        #region 连连复核处理
        /// <summary>
        /// 连连复核处理
        /// </summary>
        /// <param name="LLcashid"></param>
        /// <returns></returns>
        public ActionResult LLCashPay(int LLcashid)
        {

            string str11 = "";

            DataTable dt = getinfo(LLcashid);
            if (dt.Rows.Count > 0)
            {
                string no_order = dt.Rows[0]["no_order"].ToString();
                M_LLPay m = new M_LLPay();
                m.Acct_name = dt.Rows[0]["acct_name"].ToString();
                m.Card_no = dt.Rows[0]["card_no"].ToString();
                m.No_order = dt.Rows[0]["no_order"].ToString();
                m.Dt_order = dt.Rows[0]["dt_order"].ToString();
                m.Money_order = dt.Rows[0]["money_order"].ToString();
                m.City_code = dt.Rows[0]["city_code"].ToString();
                m.Bank_code = dt.Rows[0]["bank_code"].ToString();
                m.Brabank_name = dt.Rows[0]["brabank_name"].ToString();

                string sql = "update  hx_td_LL_cash  set  paystate=5 where  paystate=3  and  no_order='" + no_order + "'";
                DbHelperSQL.RunSql(sql);


                BLL.EF.LLpay llp = new BLL.EF.LLpay();

                string str = llp.cashpay(m);


                //Response.Write(str);

                YinTongUtil.writelog("单个批付及时处理" + no_order + "批付成功  str=" + str);

                var Objlist = JObject.Parse(str);

                if (Objlist["ret_code"].ToString() == "0000")
                {
                    sql = "update  hx_td_LL_cash  set  paystate=1 ,OperTime='" + DateTime.Now.ToString() + "' where  paystate= 5  and  no_order='" + no_order + "'";
                    DbHelperSQL.RunSql(sql);
                    //  CommonOperate.Show_Msg("批付成功");

                    str11= StringAlert.Alert("批付成功");
                    YinTongUtil.writelog("单个批付及时处理" + no_order + "批付成功  sql=" + sql);
                    Response.End();

                }
                else

                {

                    sql = "update  hx_td_LL_cash  set  paystate=2 ,OperTime='" + DateTime.Now.ToString() + "' where  paystate= 5  and  no_order='" + no_order + "'";
                    DbHelperSQL.RunSql(sql);

                    str11 = StringAlert.Alert("批付失败");
                    //CommonOperate.Show_Msg("批付失败");
                    // Response.End();
                }





                YinTongUtil.writelog("前面处理付款:" + str);


            }



            return Content(str11);
           // return View();
        }

        #endregion



        private DataTable getinfo(int UserCashId)
        {

            string sql = " select LLcashid,no_order,dt_order,money_order,acct_name,province_code,city_code,brabank_name,ordertime,card_no,paystate,Usrid,BankName,bank_code from V_LL_Cash_User where LLcashid=" + UserCashId.ToString() + " and  h_state=1 and  paystate=3";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            return dt;
        }


    }
}