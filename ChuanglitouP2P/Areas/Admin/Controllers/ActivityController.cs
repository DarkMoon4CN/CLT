using ChuanglitouP2P.BLL.EF;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.Common;
using System.Web.Script.Serialization;
using EntityFramework.Extensions;
using Webdiyer.WebControls.Mvc;
using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using ChuanglitouP2P.BLL;
using System.Text;
using System.Data;
using ChuanglitouP2P.DBUtility;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class ActivityController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();


        #region 常规活动列表
        /// <summary>
        /// 常规活动列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pgaesize"></param>
        /// <returns></returns>
        // GET: Admin/Activity
        [AdminVaildate(false)]
        public ActionResult Index(int? pageIndex, int pgaesize = 5)
        {
            var list = ef.hx_ActivityTable.Where(p => p.ActTypeId == 3).OrderByDescending(p => p.ActID).ToPagedList(pageIndex ?? 1, pgaesize);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_GeneralACTList", list);
            }

            return View(list);
        }
        #endregion


        [AdminVaildate()]
        public ActionResult Add()
        {
            CateCacheList ccl = new CateCacheList();
            ViewBag.CateList = ccl.GetCacheRewardType();


            return View();
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate(false)]
        public ActionResult AddPost(hx_ActivityTable p)
        {
            string str = "";
            int suc = 0;

            ///  ef.areas.Where(p1 => p1.areaID == 12).Delete();

            /// ef.bonus_account.Where(p3 => p3.activity_schedule_id == 111 || p3.activity_schedule_name.Contains("dfdf")).Update(p3=>new bonus_account {  act_state=1, entry_time=DateTime.Now });



            int RewTypeID1 = DNTRequest.GetInt("RewTypeID", 1);
            if (RewTypeID1 == 1) //活动类型现金
            {
                suc = GetcashAct();
            }
            else if (RewTypeID1 == 2) //抵扣券
            {
                suc = GetCashActJiuan();

            }
            else if (RewTypeID1 == 3)
            {

                suc = GetCashActJianXi();


            }


            if (suc > 0)
            {
                str = @" {""rs"": ""y"", ""info"": ""数据添加成功"", ""url"" :  ""/""}";
            }
            CacheRemove.RemoveWebCache("Act"); //请除广告位缓存

            return Content(str, "text/json");
        }

        private DateTime RegetDateTime(DateTime dTime, int type)
        {
            if (type == 0)
            {
                return dTime.Date;
            }
            if (type == 1)
            {
                return dTime.Date.AddDays(1).AddSeconds(-1);
            }
            return dTime;
        }

        #region 奖励类型为现金模块 +GetcashAct()


        public int GetcashAct()
        {
            MActCash mc = new MActCash();
            List<MAmtList> mlist = new List<MAmtList>();
            hx_ActivityTable m = new hx_ActivityTable();
            mc.ActName = Utils.CheckSQLHtml(DNTRequest.GetString("ActName"));
            mc.ActStarttime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActStarttime")), 0);
            mc.ActEndtime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActEndtime")), 1);
            mc.ActUser = DNTRequest.GetInt("ActUser", 1);
            mc.require = DNTRequest.GetInt("require", 1);
            mc.TopNum = DNTRequest.GetDecimal("TopNum", 0M);
            mc.TopAmt = DNTRequest.GetDecimal("TopAmt", 0.00M);
            string targetPlatform = DNTRequest.GetString("hidTargetPlatform");

            int suc = 0;
            if (mc.require == 1)
            {
                MAmtList ml;
                string startAmt = DNTRequest.GetString("startAmt");
                string endAmt = DNTRequest.GetString("endAmt");
                string percent = DNTRequest.GetString("percent");
                string lifeLoan = DNTRequest.GetString("LifeLoan");
                string[] SstartAmt = startAmt.Split(',');
                string[] SendAmt = endAmt.Split(',');
                string[] Spercent = percent.Split(',');
                string[] Slifelone = lifeLoan.Split(',');
                for (int i = 0; i < SstartAmt.Length; i++)
                {
                    ml = new MAmtList();
                    ml.startAmt = decimal.Parse(SstartAmt[i]);
                    ml.endAmt = decimal.Parse(SendAmt[i]);
                    ml.percent = decimal.Parse(Spercent[i]);
                    ml.LifeLoan = int.Parse(Slifelone[i]);
                    ml.Amtstr = "";
                    ml.num = 0;
                    mlist.Add(ml);
                }

                mc.MAmtList = mlist;
                m.ActName = mc.ActName;
                m.ActTypeId = DNTRequest.GetInt("ActTypeId", 2);
                m.RewTypeID = DNTRequest.GetInt("RewTypeID", 1);
                m.ActUser = mc.ActUser;
                m.ActStarttime = mc.ActStarttime;
                m.ActEndtime = mc.ActEndtime;
                m.ActRule = mc.SerializeJSON();
                m.ActState = 0;
                m.createtime = DateTime.Now;
                //MActCash p1 = js.Deserialize<MActCash>(m.ActRule);
                m.ActTargetPlatform = targetPlatform;
                ef.hx_ActivityTable.Add(m);
                suc = ef.SaveChanges();
            }
            else if (mc.require == 2)
            {
                MAmtList ml;
                string startAmt = DNTRequest.GetString("startAmt1");
                string endAmt = DNTRequest.GetString("endAmt1");
                string percent = DNTRequest.GetString("percent1");
                string lifeLoan = DNTRequest.GetString("LifeLoan1");
                string[] SstartAmt = startAmt.Split(',');
                string[] SendAmt = endAmt.Split(',');
                string[] Spercent = percent.Split(',');
                string[] Slifelone = lifeLoan.Split(',');
                for (int i = 0; i < SstartAmt.Length; i++)
                {
                    ml = new MAmtList();
                    ml.startAmt = decimal.Parse(SstartAmt[i]);
                    ml.endAmt = decimal.Parse(SendAmt[i]);
                    ml.percent = decimal.Parse(Spercent[i]);
                    ml.LifeLoan = int.Parse(Slifelone[i]);
                    ml.Amtstr = "";
                    ml.num = 0;
                    mlist.Add(ml);
                }
                mc.MAmtList = mlist;

                m.ActName = mc.ActName;
                m.ActTypeId = DNTRequest.GetInt("ActTypeId", 2);

                m.RewTypeID = DNTRequest.GetInt("RewTypeID", 1);
                m.ActUser = mc.ActUser;
                m.ActStarttime = mc.ActStarttime;
                m.ActEndtime = mc.ActEndtime;
                m.ActRule = mc.SerializeJSON();
                m.ActState = 0;
                m.createtime = DateTime.Now;
                m.ActTargetPlatform = targetPlatform;
                ef.hx_ActivityTable.Add(m);
                suc = ef.SaveChanges();

            }
            return suc;
        }
        #endregion

        #region 抵扣券模块+int GetCashActJiuan()

        private int GetCashActJiuan()
        {
            int ic = 0;
            Mcoupon mcp = new Mcoupon();

            hx_ActivityTable m = new hx_ActivityTable();
            MActCash mc = new MActCash();
            Msplitarr msp = new Msplitarr();
            List<Msplitarr> msplist = new List<Msplitarr>();
            int rule = DNTRequest.GetInt("rule", 1);
            string targetPlatform = DNTRequest.GetString("hidTargetPlatform");
            if (rule == 1)
            {
                mcp.rule = rule;
                mcp.cash = DNTRequest.GetDecimal("cash1", 0.00M);
                mcp.ISsplit = DNTRequest.GetInt("ISsplit", 2);
                mcp.Uses = DNTRequest.GetInt("Uses", 1);
                #region 拆分处理

                GetSpilt(mcp, msp, msplist);

                #endregion
                mcp.Msplitarr = msplist;
                m.ActName = Utils.CheckSQLHtml(DNTRequest.GetString("ActName"));
                m.ActTypeId = DNTRequest.GetInt("ActTypeId", 2);
                m.RewTypeID = DNTRequest.GetInt("RewTypeID", 1);
                m.ActUser = DNTRequest.GetInt("ActUser", 1);
                m.ActStarttime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActStarttime")), 0);
                m.ActEndtime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActEndtime")), 1);
                m.ActRule = mcp.SerializeJSON();
                m.ActState = 0;
                m.createtime = DateTime.Now;
            }
            else if (rule == 2)
            {
                mcp.rule = rule;
                mcp.cash = DNTRequest.GetDecimal("cash1", 0.00M);
                mcp.ISsplit = DNTRequest.GetInt("ISsplit", 2);
                mcp.Uses = DNTRequest.GetInt("Uses", 1);
                List<MAmtList> mlist = new List<MAmtList>();
                MAmtList ml;
                string startAmt = DNTRequest.GetString("startAmt2_0");
                string endAmt = DNTRequest.GetString("endAmt2_0");
                string percent = DNTRequest.GetString("percent2_0");
                string[] SstartAmt = startAmt.Split(',');
                string[] SendAmt = endAmt.Split(',');
                string[] Spercent = percent.Split(',');
                for (int i = 0; i < SstartAmt.Length; i++)
                {
                    ml = new MAmtList();
                    ml.startAmt = decimal.Parse(SstartAmt[i]);
                    ml.endAmt = decimal.Parse(SendAmt[i]);
                    ml.percent = decimal.Parse(Spercent[i]);
                    ml.Amtstr = "";
                    ml.num = 0;
                    mlist.Add(ml);
                }
                #region 拆分处理
                GetSpilt(mcp, msp, msplist);
                #endregion
                mcp.MAmtList = mlist;
                mcp.Msplitarr = msplist;
                m.ActName = Utils.CheckSQLHtml(DNTRequest.GetString("ActName"));
                m.ActTypeId = DNTRequest.GetInt("ActTypeId", 2);
                m.RewTypeID = DNTRequest.GetInt("RewTypeID", 1);
                m.ActUser = DNTRequest.GetInt("ActUser", 1);
                m.ActStarttime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActStarttime")), 0);
                m.ActEndtime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActEndtime")), 0);
                m.ActRule = mcp.SerializeJSON();
                m.ActState = 0;
                m.createtime = DateTime.Now;
            }
            else if (rule == 3)
            {
                mcp.rule = rule;
                mcp.cash = DNTRequest.GetDecimal("cash1", 0.00M);
                mcp.ISsplit = DNTRequest.GetInt("ISsplit", 2);
                mcp.Uses = DNTRequest.GetInt("Uses", 1);
                List<MAmtList> mlist = new List<MAmtList>();
                MAmtList ml;
                string startAmt = DNTRequest.GetString("startAmt3_0");
                string endAmt = DNTRequest.GetString("endAmt3_0");
                string num3 = DNTRequest.GetString("num3_0");
                string Amtstr3_01 = DNTRequest.GetString("Amtstr3_01");
                string Amtstr3_02 = DNTRequest.GetString("Amtstr3_02");

                // string percent = DNTRequest.GetString("percent2_0");

                string[] SstartAmt = startAmt.Split(',');
                string[] SendAmt = endAmt.Split(',');
                string[] Snum3 = num3.Split(',');
                string[] SAmtstr3_01 = Amtstr3_01.Split(',');
                string[] SAmtstr3_02 = Amtstr3_02.Split(',');

                for (int i = 0; i < SstartAmt.Length; i++)
                {
                    ml = new MAmtList();
                    ml.startAmt = decimal.Parse(SstartAmt[i]);
                    ml.endAmt = decimal.Parse(SendAmt[i]);
                    ml.percent = 0.00M;
                    ml.Amtstr = SAmtstr3_01[i] + "," + SAmtstr3_02[i];
                    ml.num = int.Parse(Snum3[i]); ;
                    mlist.Add(ml);
                }

                #region 拆分处理
                GetSpilt(mcp, msp, msplist);
                #endregion

                mcp.MAmtList = mlist;
                mcp.Msplitarr = msplist;

                m.ActName = Utils.CheckSQLHtml(DNTRequest.GetString("ActName"));
                m.ActTypeId = DNTRequest.GetInt("ActTypeId", 2);
                m.RewTypeID = DNTRequest.GetInt("RewTypeID", 1);
                m.ActUser = DNTRequest.GetInt("ActUser", 1);
                m.ActStarttime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActStarttime")), 0);
                m.ActEndtime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActEndtime")), 0);
                m.ActRule = mcp.SerializeJSON();
                m.ActState = 0;
                m.createtime = DateTime.Now;
            }
            m.ActTargetPlatform = targetPlatform;
            ef.hx_ActivityTable.Add(m);
            ic = ef.SaveChanges();
            return ic;
        }
        #endregion

        #region 短期活动加息券+int GetCashActJianXi()
        /// <summary>
        /// 短期活动加息券
        /// </summary>
        /// <returns></returns>
        private int GetCashActJianXi()
        {
            int ic = 0;
            Mcoupon mcp = new Mcoupon();
            hx_ActivityTable m = new hx_ActivityTable();
            MActCash mc = new MActCash();
            Msplitarr msp = new Msplitarr();
            List<Msplitarr> msplist = new List<Msplitarr>();
            int rule = DNTRequest.GetInt("jiaxirule", 1);
            string targetPlatform = DNTRequest.GetString("hidTargetPlatform");

            if (rule == 1)
            {

                mcp.rule = rule;
                mcp.cash = DNTRequest.GetDecimal("cash2", 0.00M);
                mcp.ISsplit = 2;
                mcp.Uses = DNTRequest.GetInt("Uses2", 1);
                #region 获取使用条件
                GetJiaXiSpilt(mcp, msp, msplist);
                #endregion
                mcp.Msplitarr = msplist;

                m.ActName = Utils.CheckSQLHtml(DNTRequest.GetString("ActName"));
                m.ActTypeId = DNTRequest.GetInt("ActTypeId", 2);
                m.RewTypeID = DNTRequest.GetInt("RewTypeID", 1);
                m.ActUser = DNTRequest.GetInt("ActUser", 1);
                m.ActStarttime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActStarttime")), 0);
                m.ActEndtime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActEndtime")), 1);
                m.ActRule = mcp.SerializeJSON();
                m.ActState = 0;
                m.createtime = DateTime.Now;

            }
            else if (rule == 2)
            {

                mcp.rule = rule;
                mcp.cash = DNTRequest.GetDecimal("cash2", 0.00M);
                mcp.ISsplit = 2;
                mcp.Uses = DNTRequest.GetInt("Uses2", 1);
                List<MAmtList> mlist = new List<MAmtList>();
                MAmtList ml;
                string startAmt = DNTRequest.GetString("startAmt4_0");
                string endAmt = DNTRequest.GetString("endAmt4_0");
                string percent = DNTRequest.GetString("percent4");
                string[] SstartAmt = startAmt.Split(',');
                string[] SendAmt = endAmt.Split(',');
                string[] Spercent = percent.Split(',');
                for (int i = 0; i < SstartAmt.Length; i++)
                {
                    ml = new MAmtList();
                    ml.startAmt = decimal.Parse(SstartAmt[i]);
                    ml.endAmt = decimal.Parse(SendAmt[i]);
                    ml.percent = decimal.Parse(Spercent[i]);
                    ml.Amtstr = "";
                    ml.num = 0;
                    mlist.Add(ml);
                }

                #region 获取使用条件
                GetJiaXiSpilt(mcp, msp, msplist);
                #endregion
                mcp.MAmtList = mlist;
                mcp.Msplitarr = msplist;
                m.ActName = Utils.CheckSQLHtml(DNTRequest.GetString("ActName"));
                m.ActTypeId = DNTRequest.GetInt("ActTypeId", 2);
                m.RewTypeID = DNTRequest.GetInt("RewTypeID", 1);
                m.ActUser = DNTRequest.GetInt("ActUser", 1);
                m.ActStarttime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActStarttime")), 0);
                m.ActEndtime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActEndtime")), 1);
                m.ActRule = mcp.SerializeJSON();
                m.ActState = 0;
                m.createtime = DateTime.Now;

            }
            else if (rule == 3)
            {

                mcp.rule = rule;
                mcp.cash = DNTRequest.GetDecimal("cash5", 0.00M);
                mcp.ISsplit = 2;
                mcp.Uses = DNTRequest.GetInt("Uses2", 1);
                List<MAmtList> mlist = new List<MAmtList>();
                MAmtList ml;
                string startAmt = DNTRequest.GetString("startAmt5_0");
                string endAmt = DNTRequest.GetString("endAmt5_0");
                string num3 = DNTRequest.GetString("num5_0");
                string Amtstr3_01 = DNTRequest.GetString("Amtstr5_01");
                string Amtstr3_02 = DNTRequest.GetString("Amtstr5_02");
                string Amtstr3_03 = DNTRequest.GetString("Amtstr5_03");

                // string percent = DNTRequest.GetString("percent2_0");

                string[] SstartAmt = startAmt.Split(',');
                string[] SendAmt = endAmt.Split(',');
                string[] Snum3 = num3.Split(',');
                string[] SAmtstr3_01 = Amtstr3_01.Split(',');
                string[] SAmtstr3_02 = Amtstr3_02.Split(',');
                string[] SAmtstr3_03 = Amtstr3_03.Split(',');

                for (int i = 0; i < SstartAmt.Length; i++)
                {
                    ml = new MAmtList();
                    ml.startAmt = decimal.Parse(SstartAmt[i]);
                    ml.endAmt = decimal.Parse(SendAmt[i]);
                    ml.percent = 0.00M;
                    ml.Amtstr = SAmtstr3_01[i] + "," + SAmtstr3_02[i] + "," + SAmtstr3_02[i];
                    ml.num = int.Parse(Snum3[i]); ;
                    mlist.Add(ml);
                }


                #region 获取使用条件
                GetJiaXiSpilt(mcp, msp, msplist);
                #endregion
                mcp.MAmtList = mlist;
                mcp.Msplitarr = msplist;

                m.ActName = Utils.CheckSQLHtml(DNTRequest.GetString("ActName"));
                m.ActTypeId = DNTRequest.GetInt("ActTypeId", 2);
                m.RewTypeID = DNTRequest.GetInt("RewTypeID", 1);
                m.ActUser = DNTRequest.GetInt("ActUser", 1);
                m.ActStarttime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActStarttime")), 0);
                m.ActEndtime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActEndtime")), 1);
                m.ActRule = mcp.SerializeJSON();
                m.ActState = 0;
                m.createtime = DateTime.Now;


            }
            m.ActTargetPlatform = targetPlatform;
            ef.hx_ActivityTable.Add(m);
            ic = ef.SaveChanges();

            return ic;
        }
        #endregion

        #region 加息券折分
        /// <summary>
        /// 加息券折分
        /// </summary>
        /// <param name="mcp"></param>
        /// <param name="msp"></param>
        /// <param name="msplist"></param>
        private void GetJiaXiSpilt(Mcoupon mcp, Msplitarr msp, List<Msplitarr> msplist)
        {
            string cashAmtsplit = DNTRequest.GetString("cashAmtsplit3");
            string startAmtsplit = DNTRequest.GetString("startAmtsplit3");
            string endAmtsplit = DNTRequest.GetString("endAmtsplit3");
            string endTimesplit = DNTRequest.GetString("endTimesplit3");
            string startLife = DNTRequest.GetString("startLife2");
            string endLife = DNTRequest.GetString("endLife2");
            string validity = DNTRequest.GetString("validity2");
            string[] ScashAmtsplit = cashAmtsplit.Split(',');
            string[] SstartAmtsplit = startAmtsplit.Split(',');
            string[] SendAmtsplit = endAmtsplit.Split(',');
            string[] SendTimesplit = endTimesplit.Split(',');
            string[] startLifesplit = startLife.Split(',');
            string[] endLifesplit = endLife.Split(',');
            string[] validitysplit = validity.Split(',');
            msplist.Clear();
            int min = 0, max = 0, vdate = 0;
            for (int i = 0; i < ScashAmtsplit.Length; i++)
            {
                msp = new Msplitarr();
                msp.cashAmt = decimal.Parse(ScashAmtsplit[i].ToString());
                msp.startAmt = decimal.Parse(SstartAmtsplit[i].ToString());
                msp.endAmt = decimal.Parse(SendAmtsplit[i].ToString());
                
                if (SendTimesplit[i].ToString() != "")
                {
                    //msp.endTime = DateTime.Parse(SendTimesplit[i].ToString());
                    msp.endTime = RegetDateTime(DateTime.Parse(SendTimesplit[i].ToString()), 1);
                }
                else
                {
                    msp.endTime = new DateTime(2000, 1, 1);
                }

                msp.UseLifeLoan = (int.TryParse(startLifesplit[i], out min) ? min : 0) + "-" + (int.TryParse(endLifesplit[i], out max) ? max : 0);
                msp.validity = (int.TryParse(validitysplit[i], out vdate) ? vdate : 0);
                msplist.Add(msp);
            }
        }

        #endregion

        #region 抵扣券折分
        /// <summary>
        /// 抵扣券折分
        /// </summary>
        /// <param name="mcp"></param>
        /// <param name="msp"></param>
        /// <param name="msplist"></param>
        private void GetSpilt(Mcoupon mcp, Msplitarr msp, List<Msplitarr> msplist)
        {
            #region 拆分处理
            string cashAmtsplit;
            string startAmtsplit;
            string endAmtsplit;
            string endTimesplit;
            string startLife;
            string validity;//新加有效期（天）
            string endLife;
            string[] ScashAmtsplit;
            string[] SstartAmtsplit;
            string[] SendAmtsplit;
            string[] SendTimesplit;
            string[] startLifesplit;
            string[] endLifesplit;
            string[] validitysplit;
            int min = 0, max = 0, vdate = 0;
            if (mcp.ISsplit == 1) //拆分
            {
                cashAmtsplit = DNTRequest.GetString("cashAmtsplit1");
                startAmtsplit = DNTRequest.GetString("startAmtsplit1");
                endAmtsplit = DNTRequest.GetString("endAmtsplit1");
                endTimesplit = DNTRequest.GetString("endTimesplit1");
                startLife = DNTRequest.GetString("startLife");
                endLife = DNTRequest.GetString("endLife");
                validity = DNTRequest.GetString("validity");
                ScashAmtsplit = cashAmtsplit.Split(',');
                SstartAmtsplit = startAmtsplit.Split(',');
                SendAmtsplit = endAmtsplit.Split(',');
                SendTimesplit = endTimesplit.Split(',');
                startLifesplit = startLife.Split(',');
                endLifesplit = endLife.Split(',');
                validitysplit = validity.Split(',');
                msplist.Clear();
                for (int i = 0; i < ScashAmtsplit.Length; i++)
                {
                    msp = new Msplitarr();
                    msp.cashAmt = decimal.Parse(ScashAmtsplit[i].ToString());
                    msp.startAmt = decimal.Parse(SstartAmtsplit[i].ToString());
                    msp.endAmt = decimal.Parse(SendAmtsplit[i].ToString());
                    if (SendTimesplit[i].ToString() != "")
                    {
                        msp.endTime = RegetDateTime(DateTime.Parse(SendTimesplit[i].ToString()), 1);
                    }
                    else
                    {
                        msp.endTime = new DateTime(2000,1,1);
                    }

                    msp.UseLifeLoan = (int.TryParse(startLifesplit[i], out min) ? min : 0) + "-" + (int.TryParse(endLifesplit[i], out max) ? max : 0);
                    msp.validity = (int.TryParse(validitysplit[i], out vdate) ? vdate : 0);
                    msplist.Add(msp);
                }
            }
            else if (mcp.ISsplit == 2)  //不折分
            {

                cashAmtsplit = DNTRequest.GetString("cashAmtsplit2");
                startAmtsplit = DNTRequest.GetString("startAmtsplit2");
                endAmtsplit = DNTRequest.GetString("endAmtsplit2");
                endTimesplit = DNTRequest.GetString("endTimesplit2");
                startLife = DNTRequest.GetString("startLife1");
                endLife = DNTRequest.GetString("endLife1");
                validity = DNTRequest.GetString("validity1");
                ScashAmtsplit = cashAmtsplit.Split(',');
                SstartAmtsplit = startAmtsplit.Split(',');
                SendAmtsplit = endAmtsplit.Split(',');
                SendTimesplit = endTimesplit.Split(',');
                startLifesplit = startLife.Split(',');
                endLifesplit = endLife.Split(',');
                validitysplit = validity.Split(',');
                msplist.Clear();
                for (int i = 0; i < ScashAmtsplit.Length; i++)
                {
                    msp = new Msplitarr();
                    msp.cashAmt = decimal.Parse(ScashAmtsplit[i].ToString());
                    msp.startAmt = decimal.Parse(SstartAmtsplit[i].ToString());
                    msp.endAmt = decimal.Parse(SendAmtsplit[i].ToString());
                    if (SendTimesplit[i].ToString() != "")
                    {
                        msp.endTime = RegetDateTime(DateTime.Parse(SendTimesplit[i].ToString()), 1);
                    }
                    else
                    {
                        msp.endTime = new DateTime(2000, 1, 1);
                    }

                    msp.UseLifeLoan = (int.TryParse(startLifesplit[i], out min) ? min : 0) + "-" + (int.TryParse(endLifesplit[i], out max) ? max : 0);
                    msp.validity = (int.TryParse(validitysplit[i], out vdate) ? vdate : 0);
                    msplist.Add(msp);
                }
            }
            #endregion


        }
        #endregion

        #region 常规投资


        [AdminVaildate()]
        public ActionResult GeneralAdd()
        {


            return View();
        }

        #endregion

        #region 活动操作状态处理
        /// <summary>
        /// 活动操作状态处理
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [AdminVaildate(false, true)]
        public ActionResult SetACTState(int id, int state)
        {
            string json = "{\"ret\":0,\"msg\":\"操作失败\"}";
            if (id < 1)
            {
                json = "{\"ret\":-1,\"msg\":\"参数错误\"}";
            }
            else
            {
                var i = 0;

                if (state == -2)
                {
                    i = ef.hx_ActivityTable.Where(a => a.ActID == id).Delete();
                }
                else
                {
                    i = ef.hx_ActivityTable.Where(a => a.ActID == id).Update(a => new hx_ActivityTable { ActState = state });
                }


                if (i > 0)
                {
                    json = "{\"ret\":1,\"msg\":\"操作成功\"}";
                }
            }


            CacheRemove.RemoveWebCache("Act"); //请除广告位缓存

            return Content(json, "text/json");

        }
        #endregion


        #region 常规活动统计
        /// <summary>
        /// 常规红包统计
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult GeneralCount(int? pageIndex, int pgaesize = 10)
        {
            Utils.SetSYSDateTimeFormat();

            string realname = Utils.CheckSQLHtml(DNTRequest.GetString("realname"));
            string mobile = Utils.CheckSQLHtml(DNTRequest.GetString("mobile"));
            decimal sinvst = DNTRequest.GetDecimal("sinvst", 0M);
            decimal einvst = DNTRequest.GetDecimal("einvst", 0M);
            int RewTypeID = DNTRequest.GetInt("RewTypeID", -1);
            int useridentity = DNTRequest.GetInt("useridentity", -1);
            int UseState = DNTRequest.GetInt("UseState", -1);
            string SDate = DNTRequest.GetString("SDate");
            string EDate = DNTRequest.GetString("EDate");
            int ActDateTime = DNTRequest.GetInt("ActDateTime", -1);

            int actid = DNTRequest.GetInt("actid", 0);
            pageIndex = DNTRequest.GetInt("pageIndex", 1);

            // string where = " where  ActTypeId = 3  and ActID =" + actid.ToString() + " ";

            string where = " where   ActID =" + actid.ToString() + " ";

            if (realname.Length > 0)
            {

                where += " and  realname like '%" + realname + "%'";

            }

            if (mobile.Length > 0)
            {

                where += " and  mobile like '%" + mobile + "%'";
            }
            if (RewTypeID > 0)
            {
                where += " and  RewTypeID =" + RewTypeID.ToString() + " ";
            }

            if (useridentity > -1)
            {
                where += " and  useridentity =" + useridentity.ToString() + " ";
            }


            if (UseState > -1)
            {
                where += " and  UseState =" + UseState.ToString() + " ";
            }

            if (ActDateTime == 0)//获得时间
            {
                if (SDate.Length > 0)
                {
                    where += " and  Createtime >='" + SDate + " 00:00:00' ";
                }
                if (EDate.Length > 0)
                {
                    where += " and  Createtime <='" + EDate + " 23:59:59' ";

                }

            }
            else if (ActDateTime == 1) //使用时间
            {
                if (SDate.Length > 0)
                {
                    where += " and  UseTime >='" + SDate + " 00:00:00' ";
                }
                if (EDate.Length > 0)
                {
                    where += " and  UseTime <='" + EDate + " 23:59:59' ";

                }

            }
            else if (ActDateTime == 2) //过期时间
            {
                if (SDate.Length > 0)
                {
                    where += " and  AmtEndtime >='" + SDate + " 00:00:00' ";
                }
                if (EDate.Length > 0)
                {
                    where += " and  AmtEndtime <='" + EDate + " 23:59:59' ";

                }

            }
            else if (ActDateTime == 3) //注册时间
            {
                if (SDate.Length > 0)
                {
                    where += " and  registration_time >='" + SDate + " 00:00:00' ";
                }
                if (EDate.Length > 0)
                {
                    where += " and  registration_time <='" + EDate + " 23:59:59' ";

                }

            }


            string sql = "select UserAct,realname,mobile,useridentity,RewTypeID,Amt,ActName,Createtime,UseState,UseTime,AmtEndtime,ISSmsOne,registration_time,ActID,registerid,(select COALESCE(investment_amount,0) from hx_Bid_records where hx_Bid_records.bid_records_id = V_ACT.AmtProid) as investment_amount  from V_ACT " + where + " order by  UserAct desc";

            var list = ef.Database.SqlQuery<ActMode>(sql).ToPagedList(pageIndex ?? 1, pgaesize);


            ViewBag.GetScheduleDropDownList = new SelectListByEF().GetScheduleDropDownList("0", "请选择");

            if (Request.IsAjaxRequest())
            {
                return PartialView("_GeneralCountList", list);
            }


            ActCount cou = new ActCount();

            hx_ActivityTable ht = ef.hx_ActivityTable.Where(p => p.ActID == actid).FirstOrDefault();
            DateTime dtime = DateTime.Now;
            DataTable dtc = new DataTable();
            DataTable dty = new DataTable();
            DataTable dtw = new DataTable();
            StringBuilder str = new StringBuilder();
            if (ht != null)
            {

                dtime = DateTime.Parse(ht.ActStarttime.ToString());

                dtc = cou.GetGeneralNumTotal(actid);
                dty = cou.GetGeneralYNum(actid);
                dtw = cou.GetGeneralWNum(actid);
                if (ht.RewTypeID == 1)
                {
                    if (dtc.Rows.Count > 0)
                    {
                        str.Append("     现金 " + dtc.Rows[0]["num"].ToString() + "份，共计  " + dtc.Rows[0]["amt"].ToString() + "  元。");
                    }
                    else
                    {
                        str.Append("     现金 0 份，共计 0 元 ");
                    }

                    if (dty.Rows.Count > 0)
                    {
                        str.Append("    已使用 " + dty.Rows[0]["num"].ToString() + "份，共计  " + dty.Rows[0]["amt"].ToString() + " 元。");
                    }
                    else
                    {
                        str.Append("     已使用 0 份，共计 0 元。");
                    }

                    if (dtw.Rows.Count > 0)
                    {
                        //  str.Append("     还剩 " + dtw.Rows[0]["num"].ToString() + "份，共计  " + dtw.Rows[0]["amt"].ToString() + " 元");
                    }
                    else
                    {
                        //   str.Append("    还剩 0 份，共计 0 元");
                    }


                }
                else if (ht.RewTypeID == 2)
                {
                    if (dtc.Rows.Count > 0)
                    {
                        str.Append("抵扣券 " + dtc.Rows[0]["num"].ToString() + "份，共计" + dtc.Rows[0]["amt"].ToString() + "元。");
                    }
                    else
                    {
                        str.Append("抵扣券 0 份，共计 0 元。");
                    }

                    if (dty.Rows.Count > 0)
                    {
                        str.Append("已使用 " + dty.Rows[0]["num"].ToString() + "份，共计" + dty.Rows[0]["amt"].ToString() + "元。");
                    }
                    else
                    {
                        str.Append("已使用 0 份，共计 0 元。");
                    }

                    if (dtw.Rows.Count > 0)
                    {
                        str.Append("还剩 " + dtw.Rows[0]["num"].ToString() + "份，共计" + dtw.Rows[0]["amt"].ToString() + "元。");
                    }
                    else
                    {
                        str.Append("还剩 0 份，共计 0 元。");
                    }


                }
                else if (ht.RewTypeID == 3)
                {
                    if (dtc.Rows.Count > 0)
                    {
                        str.Append("加息券 " + dtc.Rows[0]["num"].ToString() + "份，共计" + dtc.Rows[0]["amt"].ToString() + "。");
                    }
                    else
                    {
                        str.Append("加息券 0 份，共计 0 。");
                    }

                    if (dty.Rows.Count > 0)
                    {
                        str.Append("已使用 " + dty.Rows[0]["num"].ToString() + "份，共计" + dty.Rows[0]["amt"].ToString() + "。");
                    }
                    else
                    {
                        str.Append("已使用 0 份，共计 0 。");
                    }

                    if (dtw.Rows.Count > 0)
                    {
                        str.Append("还剩 " + dtw.Rows[0]["num"].ToString() + "份，共计" + dtw.Rows[0]["amt"].ToString() + "。");
                    }
                    else
                    {
                        str.Append("还剩 0 份，共计 0 。");
                    }


                }




            }

            ViewBag.text = str.ToString();


            ViewBag.totalc = cou.GetGeneralCount(actid);
            ViewBag.NewReg = cou.GetGeneralNewReg(actid, dtime);
            ViewBag.olduser = cou.GetGeneralOldUser(actid, dtime);
            ViewBag.totinv = cou.GetGeneralTotalUser(actid);




            return View(list);


        }
        /// <summary>
        /// 导出Excel数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GeneralCountToExcel()
        {
            Utils.SetSYSDateTimeFormat();

            string realname = Utils.CheckSQLHtml(DNTRequest.GetString("realname"));
            string mobile = Utils.CheckSQLHtml(DNTRequest.GetString("mobile"));
            int RewTypeID = DNTRequest.GetInt("RewTypeID", -1);
            int useridentity = DNTRequest.GetInt("useridentity", -1);
            int UseState = DNTRequest.GetInt("UseState", -1);
            string SDate = DNTRequest.GetString("SDate");
            string EDate = DNTRequest.GetString("EDate");
            int ActDateTime = DNTRequest.GetInt("ActDateTime", -1);

            int actid = DNTRequest.GetInt("actid", 0);

            // string where = " where  ActTypeId = 3  and ActID =" + actid.ToString() + " ";

            string where = " where   ActID =" + actid.ToString() + " ";

            if (realname.Length > 0)
            {
                where += " and  realname like '%" + realname + "%'";
            }

            if (mobile.Length > 0)
            {
                where += " and  mobile like '%" + mobile + "%'";
            }
            if (RewTypeID > 0)
            {
                where += " and  RewTypeID =" + RewTypeID.ToString() + " ";
            }
            if (useridentity > -1)
            {
                where += " and  useridentity =" + useridentity.ToString() + " ";
            }
            if (UseState > -1)
            {
                where += " and  UseState =" + UseState.ToString() + " ";
            }
            if (ActDateTime == 0)//获得时间
            {
                if (SDate.Length > 0)
                {
                    where += " and  Createtime >='" + SDate + " 00:00:00' ";
                }
                if (EDate.Length > 0)
                {
                    where += " and  Createtime <='" + EDate + " 23:59:59' ";
                }
            }
            else if (ActDateTime == 1) //使用时间
            {
                if (SDate.Length > 0)
                {
                    where += " and  UseTime >='" + SDate + " 00:00:00' ";
                }
                if (EDate.Length > 0)
                {
                    where += " and  UseTime <='" + EDate + " 23:59:59' ";
                }
            }
            else if (ActDateTime == 2) //过期时间
            {
                if (SDate.Length > 0)
                {
                    where += " and  AmtEndtime >='" + SDate + " 00:00:00' ";
                }
                if (EDate.Length > 0)
                {
                    where += " and  AmtEndtime <='" + EDate + " 23:59:59' ";
                }
            }
            else if (ActDateTime == 3) //注册时间
            {
                if (SDate.Length > 0)
                {
                    where += " and  registration_time >='" + SDate + " 00:00:00' ";
                }
                if (EDate.Length > 0)
                {
                    where += " and  registration_time <='" + EDate + " 23:59:59' ";
                }
            }
            string sql = @"select 
                            UserAct ID,(case when realname is not null and useridentity>=1 then substring(realname,1,1)+'**' else realname end) '姓名',
                            (case when useridentity>=1 then substring(mobile,1,3)+'****'+substring(mobile,8,4) else mobile end) '手机',
                            case useridentity when 0 then '普通' when 1 then 'vip' when 2 then '黄金' when 3 then '虚假' when 4 then '渠道' when 5 then '白金' when 6 then '钻石' when 7 then '钻石1' when 8 then '钻石2' else '未知' end 等级,
                            case RewTypeID when 1 then '现金' when 2 then '抵扣券' when 3 then '加息券' else '未知' end 获得奖励,
                            case RewTypeID when 3 then convert(varchar,Amt)+ '%' else convert(varchar,Amt)+'元' end 额度,
                            ActName 来源,
                            Createtime 获得时间,
                            case UseState when 0 then '未使用' when 1 then '已使用' when 2 then '已过期' when 3 then '锁定中' when 4 then '现金转账成功' when 5 then '未转账' else '未知' end 使用状态,
                            UseTime 使用时间,
                            AmtEndtime 过期时间,
                            case ISSmsOne when 0 then '否' else '是' end 短信提醒,
                            ISNULL((select COALESCE(investment_amount,0) from hx_Bid_records where hx_Bid_records.bid_records_id = V_ACT.AmtProid),0) as 投资金额,
                            registration_time 注册时间,registerid 用户Id,(select invest_time from hx_Bid_records where hx_Bid_records.bid_records_id = V_ACT.AmtProid) as 投资时间,
                            (select borrowing_title from [dbo].[hx_borrowing_target] where targetid in (select targetid from hx_Bid_records where hx_Bid_records.bid_records_id=V_ACT.AmtProid)) as 投资项目,(select (case when unit_day=1 then CAST(life_of_loan AS varchar(50))+'月'  when unit_day=3 then CAST(life_of_loan AS varchar(50))+'天' end) from [dbo].[hx_borrowing_target] where targetid in (select targetid from hx_Bid_records where hx_Bid_records.bid_records_id=V_ACT.AmtProid)) as 投资期限
                            from V_ACT " + where + " order by  UserAct desc";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql.ToString());
            var path = Extensions.ExportExcel(dt);

            return Content(path);
        }
        #endregion

        #region 新加活动详情2016.12.22
        /// <summary>
        /// 常规活动统计
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult ActiveDetails(int? pageIndex, int pgaesize = 10)
        {
            Utils.SetSYSDateTimeFormat();

            string realname = Utils.CheckSQLHtml(DNTRequest.GetString("realname"));
            //string mobile = Utils.CheckSQLHtml(DNTRequest.GetString("mobile"));
            int registerid = DNTRequest.GetInt("registerid", 0);
            decimal sinvst = DNTRequest.GetDecimal("sinvst", 0M);
            decimal einvst = DNTRequest.GetDecimal("einvst", 0M);
            //int RewTypeID = DNTRequest.GetInt("RewTypeID", -1);
            int useridentity = DNTRequest.GetInt("useridentity", -1);
            int UseState = DNTRequest.GetInt("UseState", -1);
            string SDate = DNTRequest.GetString("SDate");//开始时间
            string EDate = DNTRequest.GetString("EDate");//结束时间
            int ActDateTime = DNTRequest.GetInt("ActDateTime", -1);
            int actid = DNTRequest.GetInt("actid", 0);
            pageIndex = DNTRequest.GetInt("pageIndex", 1);

            // string where = " where  ActTypeId = 3  and ActID =" + actid.ToString() + " ";
            string where = " where br.ordstate=1 and  ua.ActID =" + actid.ToString() + " ";

            if (registerid > 0)
            {
                where += " and  ua.registerid =" + registerid + " ";
            }

            if (realname.Length > 0)
            {
                where += " and  mt.realname like '%" + realname + "%'";
            }

            //if (mobile.Length > 0)
            //{
            //    where += " and  mobile like '%" + mobile + "%'";
            //}
            //if (RewTypeID > 0)
            //{
            //    where += " and  RewTypeID =" + RewTypeID.ToString() + " ";
            //}
            if (useridentity > -1)
            {
                where += " and  mt.useridentity =" + useridentity.ToString() + " ";
            }
            //if (UseState > -1)
            //{
            //    where += " and  UseState =" + UseState.ToString() + " ";
            //}
            if (ActDateTime == 0)//投资时间
            {
                if (SDate.Length > 0)
                {
                    where += " and  br.invest_time >='" + SDate + " 00:00:00' ";
                }
                if (EDate.Length > 0)
                {
                    where += " and  br.invest_time <='" + EDate + " 23:59:59' ";
                }
            }
            else if (ActDateTime == 1) //注册时间
            {
                if (SDate.Length > 0)
                {
                    where += " and  mt.registration_time >='" + SDate + " 00:00:00' ";
                }
                if (EDate.Length > 0)
                {
                    where += " and  mt.registration_time <='" + EDate + " 23:59:59' ";
                }
            }

            //string sql = "  SELECT br.bid_records_id,ua.ActID, ua.UserAct,ua.registerid,ua.RewTypeID,mt.realname,mt.useridentity,mt.registration_time ,br.invest_time,br.investment_amount, (CASE WHEN ua.RewTypeID=2 THEN '抵扣' ELSE '' END) hbtype,br.BonusAmt, (CASE WHEN ua.RewTypeID=2 THEN ua.Amt ELSE 0 END) hdhbje,(CASE WHEN ua.RewTypeID=3 THEN ua.Amt ELSE 0 END) jxq,(CASE WHEN ua.RewTypeID=1 THEN ua.Amt ELSE 0 END) xjjl,(SELECT borrowing_title from hx_borrowing_target where br.targetid=targetid) borrowingTitle FROM hx_Bid_records br INNER JOIN hx_UserAct ua ON ua.AmtProid=br.bid_records_id INNER JOIN hx_member_table mt ON ua.registerid = mt.registerid " + where+" ORDER BY br.invest_time desc";
            string sql = "SELECT ua.ActID,ua.registerid,ua.RewTypeID,mt.realname,mt.useridentity,mt.registration_time ,br.invest_time,br.investment_amount, (CASE WHEN ua.RewTypeID=2 THEN '抵扣' ELSE '' END) hbtype,br.BonusAmt,  sum((CASE WHEN ua.RewTypeID=2 THEN ua.Amt ELSE 0 END)) hdhbje,(CASE WHEN ua.RewTypeID=3 THEN ua.Amt ELSE 0 END) jxq,(CASE WHEN ua.RewTypeID=1 THEN ua.Amt ELSE 0 END) xjjl,(SELECT borrowing_title from hx_borrowing_target where br.targetid=targetid) borrowingTitle FROM hx_Bid_records br INNER JOIN hx_UserAct ua ON ua.AmtProid=br.bid_records_id INNER JOIN hx_member_table mt ON ua.registerid = mt.registerid " + where + " GROUP BY ua.ActID,ua.registerid,ua.RewTypeID,mt.realname,mt.useridentity,mt.registration_time ,br.invest_time,br.investment_amount,br.BonusAmt,ua.Amt,br.targetid ORDER BY br.invest_time desc";
            var list = ef.Database.SqlQuery<ActDetailMode>(sql).ToPagedList(pageIndex ?? 1, pgaesize);
           
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ActiveDetailsList", list);
            }
            
            return View(list);
        }

        /// <summary>
        /// 导出Excel数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ActiveDetailsToExcel()
        {
            Utils.SetSYSDateTimeFormat();

            string realname = Utils.CheckSQLHtml(DNTRequest.GetString("realname"));
            int registerid = DNTRequest.GetInt("registerid", 0);
            int useridentity = DNTRequest.GetInt("useridentity", -1);
           
            string SDate = DNTRequest.GetString("SDate");
            string EDate = DNTRequest.GetString("EDate");
            int ActDateTime = DNTRequest.GetInt("ActDateTime", -1);

            int actid = DNTRequest.GetInt("actid", 0);

            // string where = " where  ActTypeId = 3  and ActID =" + actid.ToString() + " ";

            string where = " where    br.ordstate=1 and  ua.ActID =" + actid.ToString() + " ";
            if (registerid > 0)
            {
                where += " and  ua.registerid =" + registerid + " ";
            }
            if (realname.Length > 0)
            {
                where += " and  mt.realname like '%" + realname + "%'";
            }

            if (useridentity > -1)
            {
                where += " and  mt.useridentity =" + useridentity.ToString() + " ";
            }
          
            if (ActDateTime == 0)//投资时间
            {
                if (SDate.Length > 0)
                {
                    where += " and  br.invest_time >='" + SDate + " 00:00:00' ";
                }
                if (EDate.Length > 0)
                {
                    where += " and  br.invest_time <='" + EDate + " 23:59:59' ";
                }
            }
            else if (ActDateTime == 1) //注册时间
            {
                if (SDate.Length > 0)
                {
                    where += " and  mt.registration_time >='" + SDate + " 00:00:00' ";
                }
                if (EDate.Length > 0)
                {
                    where += " and  mt.registration_time <='" + EDate + " 23:59:59' ";
                }
            }
            //string sql = @"SELECT ua.registerid ID,mt.realname 姓名,(CASE WHEN mt.useridentity = '0' THEN '普通'  WHEN mt.useridentity = '1' THEN 'vip' WHEN mt.useridentity = '2' THEN '黄金' WHEN mt.useridentity = '3' THEN '虚假' WHEN mt.useridentity = '4' THEN '渠道' WHEN mt.useridentity = '5' THEN '白金' WHEN mt.useridentity = '6' THEN '钻石' WHEN mt.useridentity = '7' THEN '钻石1' WHEN mt.useridentity = '8' THEN '钻石2'  ELSE '未知' END) 等级,mt.registration_time 注册时间 ,br.invest_time 投资时间,br.investment_amount 投资金额, (CASE WHEN ua.RewTypeID=2 THEN '抵扣' ELSE '' END) 红包类型,br.BonusAmt 总使用红包金额, (CASE WHEN ua.RewTypeID=2 THEN ua.Amt ELSE 0 END) 使用本次活动红包金额,(CASE WHEN ua.RewTypeID=3 THEN ua.Amt ELSE 0 END) 使用本次活动加息券,(CASE WHEN ua.RewTypeID=1 THEN ua.Amt ELSE 0 END) 现金奖励,(SELECT borrowing_title from hx_borrowing_target where br.targetid=targetid) 投资标的 FROM hx_Bid_records br INNER JOIN hx_UserAct ua ON ua.AmtProid=br.bid_records_id INNER JOIN hx_member_table mt ON ua.registerid = mt.registerid  "+where+" ORDER BY br.invest_time desc";
            string sql = @"SELECT ua.registerid ID,mt.realname 姓名,(CASE WHEN mt.useridentity = '0' THEN '普通'  WHEN mt.useridentity = '1' THEN 'vip' WHEN mt.useridentity = '2' THEN '黄金' WHEN mt.useridentity = '3' THEN '虚假' WHEN mt.useridentity = '4' THEN '渠道' WHEN mt.useridentity = '5' THEN '白金' WHEN mt.useridentity = '6' THEN '钻石' WHEN mt.useridentity = '7' THEN '钻石1' WHEN mt.useridentity = '8' THEN '钻石2'  ELSE '未知' END) 等级,mt.registration_time 注册时间 ,br.invest_time 投资时间,br.investment_amount 投资金额, (CASE WHEN ua.RewTypeID=2 THEN '抵扣' ELSE '' END) 红包类型,br.BonusAmt 总使用红包金额, sum((CASE WHEN ua.RewTypeID=2 THEN ua.Amt ELSE 0 END)) 使用本次活动红包金额,(CASE WHEN ua.RewTypeID=3 THEN ua.Amt ELSE 0 END) 使用本次活动加息券,(CASE WHEN ua.RewTypeID=1 THEN ua.Amt ELSE 0 END) 现金奖励,(SELECT borrowing_title from hx_borrowing_target where br.targetid=targetid) 投资标的 FROM hx_Bid_records br INNER JOIN hx_UserAct ua ON ua.AmtProid=br.bid_records_id INNER JOIN hx_member_table mt ON ua.registerid = mt.registerid  " + where + " GROUP BY ua.registerid,ua.RewTypeID,mt.realname,mt.useridentity,mt.registration_time ,br.invest_time,br.investment_amount,br.BonusAmt,ua.Amt,br.targetid  ORDER BY br.invest_time desc";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql.ToString());
            var path = Extensions.ExportExcel(dt);

            return Content(path);
        }
        #endregion

        #region 新人注册活动列表
        /// <summary>
        /// 新人注册活动列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pgaesize"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult ActRegList(int? pageIndex, int pgaesize = 5)
        {
            var list = ef.hx_ActivityTable.Where(p => p.ActTypeId == 1).OrderByDescending(p => p.ActID).ToPagedList(pageIndex ?? 1, pgaesize);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ActRgList", list);
            }


            return View(list);
        }

        #endregion

        #region 邀请好友活动列表
        /// <summary>
        /// 邀请好友活动列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pgaesize"></param>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult ActInvitefriendsList(int? pageIndex, int pgaesize = 5)
        {
            var list = ef.hx_ActivityTable.Where(p => p.ActTypeId == 4).OrderByDescending(p => p.ActID).ToPagedList(pageIndex ?? 1, pgaesize);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ActInvitefriendsList", list);
            }


            return View(list);
        }
        #endregion


        #region 邀请好友，活动添加
        /// <summary>
        /// 邀请好友，活动添加
        /// </summary>
        /// <returns></returns>
        [AdminVaildate()]
        public ActionResult Actfriends()
        {


            return View();
        }
        #endregion




        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate(false)]
        public ActionResult AddFriendsPost()
        {
            string str = "";
            int suc = 0;

            int RewTypeID1 = DNTRequest.GetInt("RewTypeID", 1);
            if (RewTypeID1 == 1) //活动类型现金
            {
                suc = GetcashActYao();
            }
            else if (RewTypeID1 == 2) //抵扣券
            {
                suc = GetCashActJiuanYao();
            }
            else if (RewTypeID1 == 3) //加息券
            {
                suc = GetCashActJianXiYao();
            }


            if (suc > 0)
            {
                str = @" {""rs"": ""y"", ""info"": ""数据添加成功"", ""url"" :  ""/""}";
            }
            CacheRemove.RemoveWebCache("Act"); //请除广告位缓存

            return Content(str, "text/json");

        }




        #region 邀请奖励类型为现金模块 +GetcashActYao()
        public int GetcashActYao()
        {
            MActCash mc = new MActCash();
            List<MAmtList> mlist = new List<MAmtList>();
            hx_ActivityTable m = new hx_ActivityTable();
            mc.ActName = Utils.CheckSQLHtml(DNTRequest.GetString("ActName"));
            mc.ActStarttime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActStarttime")), 0);
            mc.ActEndtime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActEndtime")), 1);
            mc.ActUser = DNTRequest.GetInt("ActUser", 5);
            mc.require = DNTRequest.GetInt("require", 0);
            mc.require1 = DNTRequest.GetInt("require1", 0);
            mc.TopNum = DNTRequest.GetDecimal("TopNum", 0M);
            mc.TopAmt = DNTRequest.GetDecimal("TopAmt", 0.00M);
            mc.TopAmt1 = DNTRequest.GetDecimal("TopAmt1", 0.00M);
            mc.Cash = DNTRequest.GetDecimal("cash1", 0.00M);

            if (mc.require != 1 || mc.require1 != 1)
            {
                Response.Write(StringAlert.Alert("受邀好友首次成功投资和续投按一定金额赠送 为必选"));
                Response.End();
            }


            int suc = 0;
            if (mc.require == 1)
            {
                MAmtList ml;
                string startAmt = DNTRequest.GetString("startAmt");
                string endAmt = DNTRequest.GetString("endAmt");
                string percent = DNTRequest.GetString("percent");
                string[] SstartAmt = startAmt.Split(',');
                string[] SendAmt = endAmt.Split(',');
                string[] Spercent = percent.Split(',');
                for (int i = 0; i < SstartAmt.Length; i++)
                {
                    ml = new MAmtList();
                    ml.startAmt = decimal.Parse(SstartAmt[i]);
                    ml.endAmt = decimal.Parse(SendAmt[i]);
                    ml.percent = decimal.Parse(Spercent[i]);
                    ml.Amtstr = "";
                    ml.num = 0;
                    mlist.Add(ml);
                }

                mc.MAmtList = mlist;
                m.ActName = mc.ActName;
                m.ActTypeId = DNTRequest.GetInt("ActTypeId", 4);
                m.RewTypeID = DNTRequest.GetInt("RewTypeID", 1);
                m.ActUser = mc.ActUser;
                m.ActStarttime = mc.ActStarttime;
                m.ActEndtime = mc.ActEndtime;
                m.ActRule = mc.SerializeJSON();
                m.ActState = 0;
                m.createtime = DateTime.Now;
                //MActCash p1 = js.Deserialize<MActCash>(m.ActRule);

                ef.hx_ActivityTable.Add(m);
                suc = ef.SaveChanges();
            }
            else if (mc.require == 2)
            {
                MAmtList ml;
                string startAmt = DNTRequest.GetString("startAmt1");
                string endAmt = DNTRequest.GetString("endAmt1");
                string percent = DNTRequest.GetString("percent1");
                string[] SstartAmt = startAmt.Split(',');
                string[] SendAmt = endAmt.Split(',');
                string[] Spercent = percent.Split(',');
                for (int i = 0; i < SstartAmt.Length; i++)
                {
                    ml = new MAmtList();
                    ml.startAmt = decimal.Parse(SstartAmt[i]);
                    ml.endAmt = decimal.Parse(SendAmt[i]);
                    ml.percent = decimal.Parse(Spercent[i]);
                    ml.Amtstr = "";
                    ml.num = 0;
                    mlist.Add(ml);
                }
                mc.MAmtList = mlist;

                m.ActName = mc.ActName;
                m.ActTypeId = DNTRequest.GetInt("ActTypeId", 2);

                m.RewTypeID = DNTRequest.GetInt("RewTypeID", 1);
                m.ActUser = mc.ActUser;
                m.ActStarttime = mc.ActStarttime;
                m.ActEndtime = mc.ActEndtime;
                m.ActRule = mc.SerializeJSON();
                m.ActState = 0;
                m.createtime = DateTime.Now;
                ef.hx_ActivityTable.Add(m);
                suc = ef.SaveChanges();

            }
            return suc;
        }
        #endregion

        #region 邀请好友抵扣券模块+int GetCashActJiuanYao()
        private int GetCashActJiuanYao()
        {
            int ic = 0;
            Mcoupon mcp = new Mcoupon();

            hx_ActivityTable m = new hx_ActivityTable();
            MActCash mc = new MActCash();
            Msplitarr msp = new Msplitarr();
            List<Msplitarr> msplist = new List<Msplitarr>();
            int rule = DNTRequest.GetInt("rule", 1);

            mcp.rule = rule;
            mcp.cash = DNTRequest.GetDecimal("cash2", 0.00M);
            mcp.ISsplit = DNTRequest.GetInt("ISsplit", 2);
            mcp.Uses = DNTRequest.GetInt("Uses", 1);
            #region 拆分处理
            GetSpilt(mcp, msp, msplist);
            #endregion
            mcp.Msplitarr = msplist;

            m.ActName = Utils.CheckSQLHtml(DNTRequest.GetString("ActName"));
            m.ActTypeId = DNTRequest.GetInt("ActTypeId", 2);
            m.RewTypeID = DNTRequest.GetInt("RewTypeID", 1);
            m.ActUser = DNTRequest.GetInt("ActUser", 1);
            m.ActStarttime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActStarttime")), 0);
            m.ActEndtime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActEndtime")), 1);
            m.ActRule = mcp.SerializeJSON();
            m.ActState = 0;
            m.createtime = DateTime.Now;
            ef.hx_ActivityTable.Add(m);
            ic = ef.SaveChanges();
            return ic;
        }
        #endregion



        #region 邀请好友活动加息券+int GetCashActJianXiYao()
        /// <summary>
        /// 短期活动加息券
        /// </summary>
        /// <returns></returns>
        private int GetCashActJianXiYao()
        {
            int ic = 0;
            Mcoupon mcp = new Mcoupon();
            hx_ActivityTable m = new hx_ActivityTable();
            MActCash mc = new MActCash();
            Msplitarr msp = new Msplitarr();
            List<Msplitarr> msplist = new List<Msplitarr>();
            int rule = DNTRequest.GetInt("jiaxirule", 1);
            mcp.rule = rule;
            mcp.cash = DNTRequest.GetDecimal("cash3", 0.00M);
            mcp.ISsplit = 2;
            mcp.Uses = DNTRequest.GetInt("Uses2", 1);
            #region 获取使用条件
            GetJiaXiSpilt(mcp, msp, msplist);
            #endregion
            mcp.Msplitarr = msplist;

            m.ActName = Utils.CheckSQLHtml(DNTRequest.GetString("ActName"));
            m.ActTypeId = DNTRequest.GetInt("ActTypeId", 2);
            m.RewTypeID = DNTRequest.GetInt("RewTypeID", 1);
            m.ActUser = DNTRequest.GetInt("ActUser", 1);
            m.ActStarttime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActStarttime")), 0);
            m.ActEndtime = RegetDateTime(DateTime.Parse(DNTRequest.GetString("ActEndtime")), 1);
            m.ActRule = mcp.SerializeJSON();
            m.ActState = 0;
            m.createtime = DateTime.Now;



            ef.hx_ActivityTable.Add(m);
            ic = ef.SaveChanges();

            return ic;
        }
        #endregion

        #region  短期活动

        public ActionResult ShortActivity(int? pageIndex, int pgaesize = 5)
        {
            var list = ef.hx_ActivityTable.Where(p => p.ActTypeId == 2 || p.ActTypeId == 5).OrderByDescending(p => p.ActID).ToPagedList(pageIndex ?? 1, pgaesize);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ShortACTList", list);
            }

            return View(list);
        }

        #endregion
    }
}