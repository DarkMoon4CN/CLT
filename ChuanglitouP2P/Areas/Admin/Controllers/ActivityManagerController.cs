using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.Transfer;
using ChuangLitouP2P.Models;
using OfficeOpenXml;
using PagedList;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class ActivityManagerController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();
        // GET: Admin/ActivityManager
        [AdminVaildate()]
        public ActionResult Index(int Reward = -1, int Page = 1, int pageSize = 10)
        {
            int pageNumber = Page / 1;
            Expression<Func<GrabIphone, bool>> where = PredicateExtensionses.True<GrabIphone>();
            where = where.And(p => p.ID > 0);
            if (Reward != -1)
            {
                where = where.And(p => p.WinningState == Reward);
            }


            IPagedList<GrabIphone> list = ef.GrabIphone.Where(where).OrderByDescending(p => p.ID).ToPagedList(pageNumber, pageSize);


            ViewBag.Reward = Reward;

            return View(list);
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid;
        /// <summary>
        /// 主题总数
        /// </summary>
        public int RecordCount = 0;
        /// <summary>
        /// 分页总数
        /// </summary>
        public int pagecount = 0;
        /// <summary>
        /// 每页主题数
        /// </summary>
        public int pagesize = 20;
        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers = "";
        ///// <summary>
        ///// 登录用户编号
        ///// </summary>
        //public int idc = 0;
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [AdminVaildate(false)]
        public ActionResult LuckDraw(int page = 1, int awardType = 0, string ActivityName="")
        {
            pageid = page;
            BindActivityName();
            DataBind(awardType, ActivityName, pageid);
            ViewBag.pageIndex = page;
            ViewBag.awardType = awardType;
            return View();
        }
        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public ActionResult LuckDrawData(int awardType, string ActivityName, int page = 1)
        {
            BindActivityName();
            DataBind(awardType, ActivityName, page);
            return View("LuckDraw");
        }

        /// <summary>
        /// 绑定活动名称下拉框
        /// </summary>
        private void BindActivityName()
        {
            B_LuckDraw bllLuckDraw = new B_LuckDraw();
            List<M_LuckActivityNameData> dataList = bllLuckDraw.GetActivityNameList();
            StringBuilder builder = new StringBuilder();
            foreach (M_LuckActivityNameData item in dataList)
            {
                builder.AppendFormat("<option value='{0}'>{1}</option>", item.ActivityName, item.ActivityName);
            }
            ViewBag.ActivityNameData = builder.ToString();
        }

        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <param name="awardType"></param>
        /// <param name="pageIndex"></param>
        private void DataBind(int awardType,string ActivityName, int pageIndex)
        {
            chuangtouEntities ef = new chuangtouEntities();
            pageid = pageIndex;
            B_LuckDraw bllLuckDraw = new B_LuckDraw();
            List<M_LuckData> dataList = bllLuckDraw.GetLuckDraw(awardType, ActivityName, pageIndex, pagesize, out RecordCount);
            //IPagedList<M_LuckData> dataListd= 
            //计算总页数
            pagecount = (RecordCount - 1) / pagesize + 1;
            if (pageid <= 0)
                pageid = 1;
            //生成分页字符串
            pagenumbers = "共" + pagecount + "页&nbsp;共" + RecordCount + "条&nbsp;" + Utils.GetPageNumbers(pageid, pagecount, "LuckDrawData?awardType=" + awardType, 6, "page").ToString();
            pagenumbers = pagenumbers + "跳转到第 <input name=\"page1\" id=\"page1\" type=\"text\" style=\"width: 26px\"  /> <span class=\"button white small\" style=\"cursor:pointer;\" onclick=\"page()\">转跳</span>  ";

            StringBuilder builder = new StringBuilder();
            foreach (M_LuckData item in dataList)
            {
                builder.Append("<tr>");
                builder.AppendFormat("<td class=\"txtb20 c\">{0}</td>", item.AwardName);//奖品名称
                builder.AppendFormat("<td class=\"txtb20 c\">{0}</td>", item.UserName);//获奖会员
                builder.AppendFormat("<td class=\"txtb40 c\">{0}</td>", item.AwardTime.ToString("yyyy-MM-dd HH:mm"));//中奖时间
                builder.AppendFormat("<td class=\"txtb40 c\">{0}</td>", item.ActivityName);//活动名称
                builder.AppendFormat("<td class=\"txtb20 c\">{0}</td>", item.AwardType == (int)EnumHelp.LuckDrawEnum.E_AwardType.type0 ? "<a class=\"opt\" style=\"display:inline\" actionData=\"" + item.ID + "\" title=\"审核\">审核</a>" : (item.AwardType == (int)EnumHelp.LuckDrawEnum.E_AwardType.type4 ? "已发放" : ""));//操作
                builder.Append("</tr>");
            }
            ViewBag.PageData = pagenumbers;
            ViewBag.TBData = builder.ToString();
        }
        /// <summary>
        /// 发放现金奖励
        /// </summary>
        /// <param name="luckDrawID"></param>
        /// <returns></returns>
        public ActionResult CheckCashAward(string luckDrawID)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            int online_adminuserid = 0;
            M_login M_uid = (M_login)DataCache.GetCache(CacheRemove._loginCachePrefix + Utils.GetUserIDCookieslocahost().ToString());
            if (M_uid == null)
            {
                return Content(jss.Serialize(new { code = -1, data = "请登录后在进行该操作！" }));
            }
            B_LuckDraw bllLuckDraw = new B_LuckDraw();
            M_LuckDrawRecord record = bllLuckDraw.GetModel(luckDrawID);
            if (record.Ldre_AwardType != (int)EnumHelp.LuckDrawEnum.E_AwardType.type0)
            {
                return Content(jss.Serialize(new { code = -1, data = "只有未发放的现金奖励才能进行该操作！" }));
            }
            B_member_table bllUser = new B_member_table();
            M_member_table user = bllUser.GetModel(record.Ldre_UserID);

            decimal orderID = decimal.Parse(Utils.Createcode());
            string errMsg = "";
            if (string.IsNullOrWhiteSpace(user.UsrCustId))
            {
                return Content(jss.Serialize(new { code = -1, data = "会员未开通汇付账号，无法完成转账操作" }));
            }
            record.Ldre_AwardType = (int)EnumHelp.LuckDrawEnum.E_AwardType.type4;
            record.Ldre_OrderID = orderID.ToString();
            if (!bllLuckDraw.UpdateModel(record))
            {
                LogInfo.WriteLog("九月抽奖活动，后台列表审核，更新抽奖记录状态失败：" + string.Format(" operater:{0};operateTime:{1};", online_adminuserid, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                return Content(jss.Serialize(new { code = -1, data = "更新抽奖记录失败" }));
            }
            //hx_Activity_schedule 活动表中 50元红包的ID 为 53，对应于LuckDrawRecord中的awardID 为 -2 的数据均替换为 53 
            //if (!LuckDrawSendCash(orderID, 50, user.UsrCustId, 53, user.registerid, record.Ldre_AwardName, ref errMsg))
            if (SendCash(record.Ldre_AwardID, user.registerid))
            {
                LogInfo.WriteLog("九月抽奖活动，后台列表审核，现金奖励发放失败：" + string.Format(" operater:{0};operateTime:{1};errMsg:{2}", online_adminuserid, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), errMsg));
                return Content(jss.Serialize(new { code = -1, data = errMsg }));
            }
            LogInfo.WriteLog("九月抽奖活动，后台列表审核，现金发放成功：" + string.Format(" operater:{0};operateTime:{1};", online_adminuserid, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            return Content(jss.Serialize(new { code = 0, data = "现金发放成功" }));
        }
        public bool SendCash(int actID, int Registerid)
        {
            bool t = false;
            chuangtouEntities ef = new chuangtouEntities();
            hx_ActivityTable hat = new ActFacade().GetActivityModel(actID); //GetActTableInfo(ActTypeId, ActUser, 1);
            if (hat != null)
            {
                //判是否过期
                if (hat.ActStarttime <= DateTime.Now && DateTime.Now <= hat.ActEndtime)
                {
                    string ActRule = hat.ActRule;
                    List<MAmtList> mlist = new List<MAmtList>();
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    //MActCash mc = new MActCash();
                    Mcoupon mcp = new Mcoupon();
                    mcp = js.Deserialize<Mcoupon>(ActRule);
                    //mlist = js.Deserialize<List<MAmtList>>(ActRule);

                    //检查分发奖励是否超过顶限，如果超过直接跳过
                    //int TopNum = B_usercenter.GetTopNum(hat.ActID);
                    //项目已经发放的金额
                    //decimal totalAmt = B_usercenter.GetTopAmtCount(hat.ActID);
                    //第一次投资获取对应奖励 
                    decimal actamt = mcp.cash;//GetActAmt(mc, InvestAmt, TopNum);

                    //if (mc.TopAmt > totalAmt && mc.TopNum > TopNum)
                    //{

                    t = true;
                    if (actamt > 0)//大于 0里写入对应的奖励数据
                    {
                        hx_UserAct hua = new hx_UserAct();
                        hua.ActTypeId = hat.ActTypeId;
                        hua.registerid = Registerid;
                        hua.RewTypeID = hat.RewTypeID;
                        hua.ActID = hat.ActID;
                        hua.Amt = actamt;
                        hua.Uselower = 0.00M;
                        hua.Usehight = 0.00M;
                        hua.AmtEndtime = DateTime.Parse(hat.ActEndtime.ToString()).AddMonths(1);
                        hua.AmtUses = 1; //没指定情况下默认为单独使用
                        hua.UseState = 5;  //现金未转账
                        hua.UseTime = DateTime.Now;
                        hua.AmtProid = 0; //未使用默认为0
                        hua.ISSmsOne = 0;
                        hua.IsSmsThree = 0;
                        hua.isSmsFifteen = 0;
                        hua.IsSmsSeven = 0;
                        hua.isSmsSixteen = 0;
                        hua.OrderID = decimal.Parse(Utils.Createcode());
                        hua.Createtime = DateTime.Now;
                        hua.Title = hat.ActName;
                        hua.UseLifeLoan = "";
                        ef.hx_UserAct.Add(hua);
                        int i = ef.SaveChanges();
                        if (i > 0)
                        {
                            //录入成功，后进行转账操作
                            //1.获取用户对向
                            M_member_table p = new M_member_table();
                            B_member_table o = new B_member_table();
                            p = o.GetModel(Registerid);

                            if (p != null)
                            {
                                //2.调用商户向用户转账接口
                                Transfer tf = new Transfer();
                                ReTransfer retf = tf.ToUserTransfer(p.UsrCustId, actamt, hua.OrderID.ToString(), hua.ActID.ToString(), "/Thirdparty/ToUserTransfer");
                                if (retf != null)
                                {
                                    if (retf.RespCode == "000")
                                    {
                                        //3.事务处理操作账户及插入流水

                                        #region 验签缓存处理
                                        string cachename = retf.OrdId + "ToUserTransfer" + retf.InCustId;

                                        if (Utils.GeTThirdCache(cachename) == 0)
                                        {
                                            Utils.SetThirdCache(cachename);
                                            B_usercenter BUC = new B_usercenter();
                                            int ic = BUC.UpateActToUserTransfer(retf, 0);  //用户余更新
                                            if (ic > 0)
                                            {
                                                string sql = "SELECT registerid,username,mobile  from hx_member_table where UsrCustId='" + retf.InCustId + "'";
                                                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                                                if (dt.Rows.Count > 0)
                                                {
                                                    /*短信接口*/


                                                    #region 流水信息
                                                    B_usercenter ors = new B_usercenter();
                                                    decimal di = ors.GetUsridAvailable_balance(int.Parse(dt.Rows[0]["registerid"].ToString()));
                                                    // di = di + decimal.Parse(hua.Amt.ToString());
                                                    StringBuilder strSql = new StringBuilder();
                                                    strSql.Append("insert into hx_Capital_account_water(");
                                                    strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                                                    strSql.Append(" values (");
                                                    strSql.Append("" + int.Parse(dt.Rows[0]["registerid"].ToString()) + "," + decimal.Parse(hua.Amt.ToString()) + ",0,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + di + "," + (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.现金奖励.ToString()) + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'" + "现金奖励" + "')");

                                                    DbHelperSQL.RunSql(strSql.ToString());

                                                    strSql.Clear();
                                                    #endregion

                                                    #region 奖励流水
                                                    M_bonus_account_water mbaw = new M_bonus_account_water();
                                                    B_bonus_account_water bbaw = new B_bonus_account_water();
                                                    DateTime dte = DateTime.Now;
                                                    mbaw.bonus_account_id = int.Parse(hua.ActID.ToString());
                                                    mbaw.membertable_registerid = int.Parse(dt.Rows[0]["registerid"].ToString());
                                                    mbaw.income = decimal.Parse(retf.TransAmt);
                                                    mbaw.expenditure = 0.00M;
                                                    mbaw.time_of_occurrence = DateTime.Now;

                                                    mbaw.award_description = hat.ActName + "奖励已汇入个人账户";
                                                    mbaw.water_type = 0;
                                                    bbaw.Add(mbaw);

                                                    #endregion


                                                    #region MyRegion  系统消息
                                                    DateTime dti = DateTime.Now;
                                                    M_td_System_message pm = new M_td_System_message();
                                                    pm.MReg = int.Parse(dt.Rows[0]["registerid"].ToString());
                                                    pm.Mstate = 0;
                                                    pm.MTitle = hat.ActName;
                                                    pm.MContext = "尊敬的用户" + dt.Rows[0]["username"].ToString() + "：您好！恭喜您成功" + hat.ActName + "，现金奖励 " + retf.TransAmt + "元。如有问题可咨询创利投的客服！";
                                                    pm.PubTime = dti;
                                                    B_usercenter.AddMessage(pm);
                                                    #endregion


                                                }
                                            }

                                            t = true;

                                        }
                                        #endregion
                                    }
                                }
                            }
                        }
                    }
                    //}

                }

            }


            return t;

        }
        ///// <summary>
        ///// 发放现金奖励
        ///// </summary>
        ///// <param name="amount">发放金额</param>
        ///// <param name="UsrCustId">用户的汇付账号</param>
        ///// <param name="awardID">奖品编号</param>
        ///// <param name="userID">用户编号</param>
        ///// <param name="errMsg">错误信息</param>
        ///// <returns></returns>
        //public bool LuckDrawSendCash(decimal orderID, decimal amount, string UsrCustId, int awardID, int userID, string AwardName, ref string errMsg)
        //{
        //    M_CashAwards mc = new M_CashAwards();
        //    mc.OrdId = orderID;
        //    mc.membertable_registerid = userID;
        //    mc.UsrCustId = UsrCustId;
        //    //mc.targetid = int.Parse(dt.Rows[0]["targetid"].ToString());
        //    mc.proid = awardID;
        //    mc.OrdIdstate = 1;
        //    mc.Amounts = amount;
        //    B_CashAwards bllCashAwards = new B_CashAwards();
        //    if (bllCashAwards.Add(mc) <= 0)
        //    {
        //        errMsg = "奖励记录插入失败";
        //        return false;
        //    }

        //    M_Transfer m = new M_Transfer();
        //    m.Version = "10";
        //    m.CmdId = "Transfer";

        //    // m.OrdId = Utils.Createcode();

        //    m.OrdId = mc.OrdId.ToString();
        //    m.OutCustId = Utils.GetMerCustID();
        //    m.OutAcctId = "MDT000001";
        //    m.TransAmt = amount.ToString("0.00");
        //    m.InCustId = UsrCustId;
        //    m.BgRetUrl = Utils.GetRe_url("Thirdparty/ToUserTransfer.aspx");
        //    m.MerPriv = awardID.ToString();


        //    StringBuilder chkVal = new StringBuilder();
        //    chkVal.Append(m.Version);
        //    chkVal.Append(m.CmdId);
        //    chkVal.Append(m.OrdId);
        //    chkVal.Append(m.OutCustId);
        //    chkVal.Append(m.OutAcctId);
        //    chkVal.Append(m.TransAmt);
        //    chkVal.Append(m.InCustId);
        //    chkVal.Append(m.RetUrl);
        //    chkVal.Append(m.BgRetUrl);
        //    chkVal.Append(m.MerPriv);

        //    string chkv = chkVal.ToString();
        //    LogInfo.WriteLog("9月抽奖活动平台向用户发放现金奖励加签chkv字符:" + chkv);

        //    //私钥文件的位置(这里是放在了站点的根目录下)
        //    string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
        //    //需要指定提交字符串的长度
        //    int len = Encoding.UTF8.GetBytes(chkv).Length;
        //    StringBuilder sbChkValue = new StringBuilder(256);
        //    //加签
        //    int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

        //    LogInfo.WriteLog("9月抽奖活动平台向用户发放现金奖励加签字符:" + str.ToString());

        //    m.ChkValue = sbChkValue.ToString();

        //    LogInfo.WriteLog("9月抽奖活动平台向用户发放现金奖励转账提交信息：" + FastJSON.toJOSN(m));
        //    LogInfo.WriteLog("ChkValue:" + m.ChkValue);


        //    using (var client = new WebClient())
        //    {
        //        var values = new NameValueCollection();
        //        values.Add("Version", m.Version);
        //        values.Add("CmdId", m.CmdId);
        //        values.Add("OrdId", m.OrdId);
        //        values.Add("OutCustId", m.OutCustId);
        //        values.Add("OutAcctId", m.OutAcctId);
        //        values.Add("TransAmt", m.TransAmt);
        //        values.Add("InCustId", m.InCustId);
        //        values.Add("InAcctId", m.InAcctId);
        //        values.Add("RetUrl", m.RetUrl);
        //        values.Add("BgRetUrl", m.BgRetUrl);
        //        values.Add("MerPriv", m.MerPriv);
        //        values.Add("ChkValue", m.ChkValue);
        //        string url = Utils.GetChinapnrUrl();
        //        //同步发送form表单请求
        //        byte[] result = client.UploadValues(url, "POST", values);
        //        var retStr = Encoding.UTF8.GetString(result);
        //        // Response.Write(retStr);
        //        LogInfo.WriteLog("9月抽奖活动平台向用户发放现金奖励自动扣款转账（商户用）返回报文" + retStr);
        //        ReTransfer reg = new ReTransfer();

        //        var retloan = (ReTransfer)FastJSON.ToObject(retStr, reg);
        //        StringBuilder builder = new StringBuilder();
        //        builder.Append(retloan.CmdId);
        //        builder.Append(retloan.RespCode);
        //        builder.Append(retloan.OrdId);
        //        builder.Append(retloan.OutCustId);
        //        builder.Append(retloan.OutAcctId);
        //        builder.Append(retloan.TransAmt);
        //        builder.Append(retloan.InCustId);
        //        builder.Append(retloan.InAcctId);
        //        builder.Append(HttpUtility.UrlDecode(retloan.RetUrl));
        //        builder.Append(HttpUtility.UrlDecode(retloan.BgRetUrl));
        //        builder.Append(retloan.MerPriv);
        //        var msg = builder.ToString();

        //        LogInfo.WriteLog("9月抽奖活动平台向用户发放现金奖励转账返回参数:" + msg);
        //        //验签
        //        string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
        //        int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, retloan.ChkValue);

        //        if (ret != 0 || retloan.RespCode != "000")
        //        {
        //            errMsg = "验签失败，汇付平台转账异常";
        //            return false;
        //        }

        //        //BLL.hx_ChouJiang bChou = new BLL.hx_ChouJiang();
        //        B_usercenter BUC = new B_usercenter();
        //        int dint = BUC.UpateAwa(retloan);
        //        LogInfo.WriteLog("9月抽奖活动平台向用户发放现金奖励事务执行返回:" + dint.ToString());

        //        if (dint <= 0)
        //        {
        //            errMsg = "奖励记录更新失败";
        //            return false;
        //        }
        //        string sql = "SELECT registerid,username,mobile,iD_number,realname  from hx_member_table where UsrCustId='" + retloan.InCustId + "'";
        //        DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
        //        string name = dt.Rows[0]["username"].ToString();
        //        if (dt.Rows.Count > 0)
        //        {
        //            //活动奖历
        //            B_bonus_account bb = new B_bonus_account();
        //            M_bonus_account mb = new M_bonus_account();

        //            M_bonus_account_water mbaw = new M_bonus_account_water();
        //            B_bonus_account_water bbaw = new B_bonus_account_water();
        //            DateTime dte = DateTime.Now;
        //            mb.activity_schedule_id = awardID;
        //            mb.membertable_registerid = int.Parse(dt.Rows[0]["registerid"].ToString());
        //            mb.activity_schedule_name = AwardName;
        //            mb.amount_of_reward = decimal.Parse(retloan.TransAmt);
        //            mb.use_lower_limit = 0;
        //            mb.reward = 0;
        //            mb.start_date = dte;
        //            mb.end_date = dte;
        //            mb.entry_time = dte;
        //            mb.reward_state = 3;
        //            int bbid = bb.Add(mb);
        //            if (bbid > 0) //奖励记录成功后插入明细记录
        //            {
        //                mbaw.bonus_account_id = bbid;
        //                mbaw.membertable_registerid = mb.membertable_registerid;
        //                mbaw.income = mb.amount_of_reward;
        //                mbaw.expenditure = 0.00M;
        //                mbaw.time_of_occurrence = mb.entry_time;
        //                // mbaw.
        //                mbaw.award_description = "9月抽奖50元现金奖励";
        //                mbaw.water_type = 0;
        //                bbaw.Add(mbaw);
        //            }
        //        }
        //        LogInfo.WriteLog("9月抽奖活动平台向用户发放现金验签更新成功，需要写入消息");
        //        return true;
        //    }
        //}

        /// <summary>
        /// 券妈妈页面列表查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [AdminVaildate]
        public ActionResult QuanmamaList(string mobile, string usrCustID, string investMoneyStart, string investMoneyEnd, string giftMoneyStart, string giftMoneyEnd, string investTimeStart, string investTimeEnd, string sendTimeStart, string sendTimeEnd, string sltState, int page = 1)
        {
            ViewBag.mobile = mobile;
            ViewBag.usrCustID = usrCustID;
            ViewBag.investMoneyStart = investMoneyStart;
            ViewBag.investMoneyEnd = investMoneyEnd;
            ViewBag.giftMoneyStart = giftMoneyStart;
            ViewBag.giftMoneyEnd = giftMoneyEnd;
            ViewBag.investTimeStart = investTimeStart;
            ViewBag.investTimeEnd = investTimeEnd;
            ViewBag.sendTimeStart = sendTimeStart;
            ViewBag.sendTimeEnd = sendTimeEnd;
            ViewBag.page = page;
            ViewBag.state = sltState;
            int pageSize = 10;
            var query = from item in ef.hx_QuanmamaRecord
                        select item;
            Expression<Func<hx_QuanmamaRecord, bool>> whereFilter = PredicateExtensionses.True<hx_QuanmamaRecord>();
            if (!string.IsNullOrWhiteSpace(mobile))
            {
                whereFilter = whereFilter.And(c => c.RegisterMobile == mobile);
            }
            if (!string.IsNullOrWhiteSpace(usrCustID))
            {
                whereFilter = whereFilter.And(c => c.UsrCustID == usrCustID);
            }
            decimal money1 = 0;
            if (!string.IsNullOrWhiteSpace(investMoneyStart) && decimal.TryParse(investMoneyStart, out money1))
            {
                whereFilter = whereFilter.And(c => c.InvestMoney >= money1);
            }
            decimal money2 = 0;
            if (!string.IsNullOrWhiteSpace(investMoneyEnd) && decimal.TryParse(investMoneyEnd, out money2))
            {
                whereFilter = whereFilter.And(c => c.InvestMoney <= money2);
            }
            decimal money3 = 0;
            if (!string.IsNullOrWhiteSpace(giftMoneyStart) && decimal.TryParse(giftMoneyStart, out money3))
            {
                whereFilter = whereFilter.And(c => c.GiftMoney >= money3);
            }
            decimal money4 = 0;
            if (!string.IsNullOrWhiteSpace(giftMoneyEnd) && decimal.TryParse(giftMoneyEnd, out money4))
            {
                whereFilter = whereFilter.And(c => c.GiftMoney <= money4);
            }
            DateTime time1;
            if (!string.IsNullOrWhiteSpace(investTimeStart) && DateTime.TryParse(investTimeStart, out time1))
            {
                time1 = time1.Date;
                whereFilter = whereFilter.And(c => c.InvestTime >= time1);
            }
            DateTime time2;
            if (!string.IsNullOrWhiteSpace(investTimeEnd) && DateTime.TryParse(investTimeEnd, out time2))
            {
                time2 = time2.Date.AddDays(1);
                whereFilter = whereFilter.And(c => c.InvestTime <= time2);
            }
            DateTime time3;
            if (!string.IsNullOrWhiteSpace(sendTimeStart) && DateTime.TryParse(sendTimeStart, out time3))
            {
                time3 = time3.Date;
                whereFilter = whereFilter.And(c => c.SendTime >= time3);
            }
            DateTime time4;
            if (!string.IsNullOrWhiteSpace(sendTimeEnd) && DateTime.TryParse(sendTimeEnd, out time4))
            {
                time4 = time4.Date.AddDays(1);
                whereFilter = whereFilter.And(c => c.SendTime <= time4);
            }
            int state = 0;
            if(!string.IsNullOrWhiteSpace(sltState)&&int.TryParse(sltState,out state))
            {
                whereFilter = whereFilter.And(c => c.SendState == state);
            }
            var list = (query).Where(whereFilter).OrderBy(c => c.ID).ToPagedList(page, pageSize);
            ViewBag.ListData = list;
            ViewBag.TotalItemCount = list.TotalItemCount;
            ViewBag.TotalPageCount = (list.TotalItemCount - 1) / pageSize + 1;
            return View("QuanmamaList");
        }
        /// <summary>
        /// 上传Excel文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [AdminVaildate]
        public ActionResult UploadExcelData(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return Content("没有文件！", "text/plain");
            }
            List<hx_QuanmamaRecord> data = GetDataFromExcel(file.InputStream);
            if (data == null)
                return Content("请检查Excel文件内容！", "text/plain");
            var mobileData = data.Select(c => c.RegisterMobile).ToList();
            var existMobiles = (from item in ef.hx_member_table
                                where mobileData.Contains(item.mobile)
                                select new { mobile = item.mobile, usrCustID = item.UsrCustId }).ToList();
            data = (from item in data
                    join dr in existMobiles
                    on item.RegisterMobile equals dr.mobile
                    //on new { mobile = item.RegisterMobile, usrCustID = item.UsrCustID } equals new { mobile = dr.mobile, usrCustID = dr.usrCustID }
                    select item).ToList();
            var noNeedInsertData = (from item in ef.hx_QuanmamaRecord
                                    where mobileData.Contains(item.RegisterMobile) //|| usrCustIDData.Contains(item.UsrCustID)
                                    select item).ToList();
            var dataInsert = (from item in data
                              where !noNeedInsertData.Select(c => c.RegisterMobile).Contains(item.RegisterMobile) //&& !noNeedInsertData.Select(c => c.UsrCustID).Contains(item.UsrCustID)
                              select item).ToList();
            dataInsert.ForEach(c =>
            {
                var usrCustID = existMobiles.Where(d => d.mobile == c.RegisterMobile).First().usrCustID;
                c.UsrCustID = usrCustID;
            });
            //var usrCustIDData = data.Select(c => c.UsrCustID).ToList();
            ef.hx_QuanmamaRecord.AddRange(dataInsert);
            ef.SaveChanges();
            return RedirectToAction("QuanmamaList");
        }
        /// <summary>
        /// 从上传的Excel文件中读取数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private List<hx_QuanmamaRecord> GetDataFromExcel(Stream stream)
        {
            #region read excel
            using (stream)
            {
                ExcelPackage package = new ExcelPackage(stream);

                ExcelWorksheet sheet = package.Workbook.Worksheets.First();

                #region check excel format
                if (sheet == null)
                {
                    return null;
                }
                if (!sheet.Cells[1, 1].Value.Equals("注册手机号") ||
                     !sheet.Cells[1, 2].Value.Equals("客户号") ||
                     !sheet.Cells[1, 3].Value.Equals("注册时间") ||
                     !sheet.Cells[1, 4].Value.Equals("投资时间") ||
                     !sheet.Cells[1, 5].Value.Equals("首投金额") ||
                     !sheet.Cells[1, 6].Value.Equals("总投资金额") ||
                     !sheet.Cells[1, 7].Value.Equals("投资期限") ||
                     !sheet.Cells[1, 8].Value.Equals("返客户金额"))
                {
                    return null;
                }
                #endregion

                #region get last row index
                int lastRow = sheet.Dimension.End.Row;
                while (sheet.Cells[lastRow, 1].Value == null)
                {
                    lastRow--;
                }
                #endregion

                #region read datas
                DateTime createTime = DateTime.Now;
                int userID = Utils.GetAdmUserID();
                List<hx_QuanmamaRecord> res = new List<hx_QuanmamaRecord>();
                for (int i = 2; i <= lastRow; i++)
                {
                    if (sheet.Cells[i, 1].Value == null || sheet.Cells[i, 3].Value == null || sheet.Cells[i, 4].Value == null || sheet.Cells[i, 5].Value == null || sheet.Cells[i, 6].Value == null || sheet.Cells[i, 7].Value == null || sheet.Cells[i, 8].Value == null) continue;
                    res.Add(new hx_QuanmamaRecord()
                    {
                        CraeteTime = createTime,
                        Creater = userID,
                        RegisterMobile = sheet.Cells[i, 1].Value.ToString(),
                        UsrCustID = "",//sheet.Cells[i, 2].Value.ToString(),
                        RegisterTime = Convert.ToDateTime(sheet.Cells[i, 3].Value),
                        InvestTime = Convert.ToDateTime(sheet.Cells[i, 4].Value.ToString()),
                        InvestMoney = Convert.ToDecimal(sheet.Cells[i, 5].Value.ToString()),
                        TotalInvestMoney = Convert.ToDecimal(sheet.Cells[i, 6].Value.ToString()),
                        InvestPeriod = sheet.Cells[i, 7].Value.ToString(),
                        GiftMoney = Convert.ToDecimal(sheet.Cells[i, 8].Value.ToString()),
                        SendState = 0
                    });

                }
                return res;
                #endregion

            }
            #endregion
        }
        /// <summary>
        /// 发放奖励金额
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [AdminVaildate]
        public ActionResult SendGiftMoney(string ids)
        {
            var userid = Utils.GetAdmUserID();
            if (string.IsNullOrWhiteSpace(ids)) return Content("参数错误");
            var preFilter = from item in ef.hx_QuanmamaRecord
                            join member in ef.hx_member_table
                            on item.RegisterMobile equals member.mobile
                            where item.SendState == 0
                            select new
                            {
                                quanmamaID = item.ID,
                                registerID = member.registerid,
                                giftMoney = item.GiftMoney
                            };
            if (ids != "-1")
            {
                List<int> quanmamaIDs = (from item in ids.Split(',')
                                         where !string.IsNullOrWhiteSpace(item)
                                         select Convert.ToInt32(item)).ToList();
                preFilter = preFilter.Where(c => quanmamaIDs.Contains(c.quanmamaID));
            }
            var preFilterData = preFilter.ToList();
            var quanmamaRecords = (from item in ef.hx_QuanmamaRecord
                                   where preFilter.Select(c => c.quanmamaID).Contains(item.ID)
                                   select item).ToList();
            DateTime nowDate = DateTime.Now;
            var members = ef.hx_member_table.Where(c => preFilter.Select(d => d.registerID).Contains(c.registerid)).ToList();
            foreach (hx_member_table item in members)
            {
                var quanmama = quanmamaRecords.Where(d => d.RegisterMobile == item.mobile).First();
                bool sendStatus = SendGiftCash(quanmama.GiftMoney, quanmama.UsrCustID);
                if (!sendStatus)
                    continue;
                quanmama.SendState = 1;
                quanmama.Sender = userid;
                quanmama.SendTime = nowDate;

                decimal gm = preFilterData.Where(d => d.registerID == item.registerid).First().giftMoney;
                item.available_balance += gm;
                item.account_total_assets += gm;
                hx_Capital_account_water water = new hx_Capital_account_water()
                {
                    membertable_registerid = item.registerid,
                    income = gm,
                    expenditure = 0,
                    time_of_occurrence = nowDate,
                    account_balance = item.available_balance,
                    types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.现金奖励.ToString()),
                    createtime = nowDate,
                    keyid = 0,
                    remarks = "现金奖励"
                };
                ef.hx_Capital_account_water.Add(water);
                ef.SaveChanges();

                string sms = "尊敬的客户您好，您已成功投资创利投金服，您的账户已有返利到账，请登录官网查看。客服热线：010-53732056。";
                YMSendSMS.Send_SMS(item.mobile, sms);
            }
            return Content("操作完成");
        }
        /// <summary>
        /// 判断汇付调用是否成功
        /// </summary>
        /// <param name="money"></param>
        /// <param name="UsrCustId"></param>
        /// <returns></returns>
        private bool SendGiftCash(decimal money, string UsrCustId)
        {
            bool res = false;
            ReTransfer retloan = ChuanglitouP2P.Common.chinapnr.ChinapnrFacade.platformToUserMoney(money, UsrCustId);
            if (retloan == null)
            {
                res = false;
            }
            else if (retloan.RespCode == "000")
            {
                res = true;
            }
            return res;
        }

        private void test()
        {
            //web wap android ios
            //1开0关
        }
    }
}