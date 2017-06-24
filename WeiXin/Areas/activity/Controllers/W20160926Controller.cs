using ChuanglitouP2P.BLL;
using ChuanglitouP2P.BLL.EF;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.Transfer;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WeiXin.Areas.activity.Controllers
{
    public class W20160926Controller : Controller
    {
        private DateTime _activityTime = Convert.ToDateTime("2016-10-01");
        // GET: activity/W20160926
        public ActionResult Index(string invitedcode = "")
        {
            if (!string.IsNullOrEmpty(invitedcode)) {
                Initial(invitedcode);
            }
            bool hadUsed = false;
            ViewBag.CanUseCount = GetCanUseTimes(ref hadUsed);

            string invitationCode = "";
            int userID = Settings.Instance.CurrentUserId;
            if (userID < 1)
            {
                invitationCode = "";
            }
            else
            {
                B_member_table bllUser = new B_member_table();
                M_member_table user = bllUser.GetModel(userID);
                if (user == null || user.registerid <= 0)
                {
                    invitationCode = "";
                }
                else
                {
                    invitationCode = user.invitedcode;
                }
            }
            TXShareHelper tx = new TXShareHelper();

            #region TXShareHelper 赋值逻辑
            //tx.link = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "") + "/register/?invitedcode=" + invitationCode;//
            tx.link = Request.Url.AbsoluteUri.ToString().Trim();
            tx.CheckSignature(tx.link);

            tx.appid = Utils.GetAppSetting("WeiXinAppid");
            tx.title = "金秋十月收获季，创利投给您送红包啦！";
            if (Utils.GetAppSetting("DeBug") == "1")
            {
                tx.imgUrl = Utils.GetAppSetting("MDeBugURL") + "Images/shareoutimg.jpg";
            }
            else
            {
                tx.imgUrl = Utils.GetAppSetting("MReleaseURL") + "Images/shareoutimg.jpg";
            }
            tx.desc = "金秋十月收获季节，创利投为您锦上添花！动动手指，注册完成就能领现金哦……";

            #endregion
            //ViewBag.InvCode = invitationCode;
            ViewBag.linkOutUrl = Request.Url.AbsoluteUri.ToString().Trim().Split('?')[0] + "?invitedcode=" + invitationCode;
            ViewBag.TXShareHelper = tx;
            return View();
        }
        private void Initial(string invitedcode)
        {
            var code = invitedcode;// Utils.CheckSQLHtml(DNTRequest.GetString("code"));
            string sql = "select registerid,invitedcode from hx_member_table where invitedcode='" + code + "' ";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                HttpCookie cok = new HttpCookie("Invitation");
                cok.Values.Add("InvCode", DESEncrypt.Encrypt(invitedcode, ConfigurationManager.AppSettings["webp"].ToString()));
                cok.Values.Add("CodeUid", DESEncrypt.Encrypt(dt.Rows[0]["registerid"].ToString(), ConfigurationManager.AppSettings["webp"].ToString()));
                cok.Expires = DateTime.Now.AddDays(1);
                Response.AppendCookie(cok);
            }
        }
        /// <summary>
        /// 获取可用的抽奖次数
        /// </summary>
        /// <returns></returns>
        private int GetCanUseTimes(ref bool hadUsed)
        {
            chuangtouEntities ef = new chuangtouEntities();
            int userID = Settings.Instance.CurrentUserId;
            if (userID < 1)
            {
                return 0;
            }
            B_member_table bllUser = new B_member_table();
            M_member_table user = bllUser.GetModel(userID);
            if (user == null || user.registerid <= 0)
            {
                return 0;
            }
            int cut = 0;
            //如果用户是新用户且已经实名认证，则抽奖次数加一
            if (user.Registration_time > _activityTime && !string.IsNullOrWhiteSpace(user.UsrCustId))
            {
                cut += 1;
            }
            //用户邀请好友，好友实名注册且投资的人数
            B_td_Userinvitation bllUserInvitation = new B_td_Userinvitation();
            int tcuc = bllUserInvitation.GetTotalCanUseCount(_activityTime, userID);
            if (tcuc > 0)
            {
                cut += tcuc;
            }
            int awardID = ConfigHelper.GetConfigInt("GrabCash");
            int usedCount = ef.hx_UserAct.Where(c => c.ActID == awardID && c.registerid == userID).Count();
            hadUsed = usedCount > 0;
            if (hadUsed)
            {
                cut -= usedCount;
            }
            return cut;
        }

        /// <summary>
        /// 返回数据模板
        /// </summary>
        public class AjaxResponseData
        {
            /// <summary>
            /// 状态码
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// 信息
            /// </summary>
            public string data { get; set; }
        }
        /// <summary>
        /// 检查是否在活动时间内
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private int CheckActivityTime(ref string msg)
        {
            DateTime startTime = _activityTime;
            DateTime endTime = Convert.ToDateTime("2016-11-01");
            if (DateTime.Now < startTime)
            {
                msg = "活动未开始，敬请关注！";
                return -1;
            }
            if (DateTime.Now >= endTime)
            {
                msg = "活动已结束，请关注其他活动！";
                return -2;
            }
            return 0;
        }
        /// <summary>
        /// 抽奖方法
        /// </summary>
        /// <returns></returns>
        public ActionResult Grab()
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            AjaxResponseData arData = new AjaxResponseData();
            try
            {
                string msg = "";
                int state = CheckActivityTime(ref msg);
                if (state != 0)
                {
                    arData = new AjaxResponseData { code = "4", data = msg };
                    return Content(jss.Serialize(arData));
                }

                int userID = Settings.Instance.CurrentUserId;

                //M_login M_uid = (M_login)DataCache.GetCache(Utils.GetUserIDCookieslocahost().ToString());
                //if (M_uid == null)
                if (userID < 1)
                {
                    arData = new AjaxResponseData { code = "1", data = "请登录后重试" };
                    return Content(jss.Serialize(arData));
                }
                //if (M_uid.codeno != Utils.getSessioncode())
                //{
                //    arData = new AjaxResponseData { code = "1", data = "请登录后重试" };
                //    return Content(jss.Serialize(arData));
                //}
                //获取登录用户编号
                //int userID = M_uid.userid;
                //可抽奖次数
                bool hadUsed = false;
                int CanUseTimes = GetCanUseTimes(ref hadUsed);
                //B_member_table bllMember = new B_member_table();
                ////获取会员信息
                //PartialMemberModel member = bllMember.GetPartialModel(userID);
                ////验证用户是否为今天新注册的用户
                //if (member.registration_time >= DateTime.Now.Date && member.registration_time < DateTime.Now.AddDays(1).Date && member.isrealname == 1)
                //{
                //    CanUseTimes += 1;
                //}
                //B_LuckDraw bllLuckDraw = new B_LuckDraw();
                ////获取用户的抽奖记录数量（当天的）
                //int recordsCount = bllLuckDraw.GetRecordsCount(userID, DateTime.Now.Date, DateTime.Now.AddDays(1).Date);
                //CanUseTimes -= recordsCount;

                if (CanUseTimes <= 0)
                {
                    arData = new AjaxResponseData { code = "2", data = hadUsed.ToString() };
                    return Content(jss.Serialize(arData));
                }
                #region 规则及数据

                //金额 数量  概率
                //1-17
                //10元  60    0.17 %
                //18-1684
                //8元  1750  16.67 %
                //1685-3667
                //7元  2082  19.83 %
                //3668-5667
                //6元  2100  20 %
                //5668-7667
                //5元  2100  20 %
                //7667-10000
                //3元  2450  23.33 %
                #endregion
                //现金50元已抽中的个数
                //int cash50Count = bllLuckDraw.GetCash50RecordsCount(-2, DateTime.Now.Date, DateTime.Now.AddDays(1).Date);
                //B_Activity_schedule bllASchedule = new B_Activity_schedule();
                ActFacade act = new ActFacade();
                //B_Activity bllActivity = new B_Activity();
                //admin.users.AddBonusForUser adfu = new admin.users.AddBonusForUser();
                //奖品编号
                int awardID = ConfigHelper.GetConfigInt("GrabCash");
                //奖品标题
                string title = "";
                //奖金金额
                int awardAmt = 0;
                //轮盘区块编号
                //int awardBlockID = 0;
                Random random = new Random();
                //随机数
                int randomRate = random.Next(1, 10001);
                //randomRate = 9350;

                //10元
                if (randomRate <= 17)
                {
                    title = "现金10元";
                    //awardBlockID = 5;
                    awardAmt = 10;
                }
                //8元
                else if (randomRate <= 1684)
                {

                    title = "现金8元";
                    //awardBlockID = 7;
                    awardAmt = 8;
                }
                //7元
                else if (randomRate <= 3667)
                {
                    title = "现金7元";
                    //awardBlockID = 4;
                    awardAmt = 7;
                }
                //6元
                else if (randomRate <= 5667)
                {
                    title = "现金6元";
                    //awardBlockID = 6;
                    awardAmt = 6;
                }
                //5元
                else if (randomRate <= 7667)
                {
                    title = "现金5元";
                    //awardBlockID = 0;
                    awardAmt = 5;
                }
                //3元
                else if (randomRate <= 10000)
                {
                    title = "现金3元";
                    //awardBlockID = 2;
                    awardAmt = 3;
                }
                if (SendCash(awardID, userID, awardAmt))
                {
                    arData = new AjaxResponseData { code = "0", data = awardAmt.ToString() };
                    return Content(jss.Serialize(arData));
                }
                //插入记录失败时的异常状态
                arData = new AjaxResponseData { code = "3", data = "碰到点小问题，刷新一下试试" };
                return Content(jss.Serialize(arData));
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog("9月抽奖活动异常日志:" + "msg：" + ex.Message + "   StackTrace" + ex.StackTrace);

                //没有增加任何抽奖记录时的异常状态
                arData = new AjaxResponseData { code = "3", data = ex.Message };
                return Content(jss.Serialize(arData));
            }
        }

        public bool SendCash(int actID, int Registerid, decimal amt)
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
                    //Mcoupon mcp = new Mcoupon();
                    //mcp = js.Deserialize<Mcoupon>(ActRule);
                    //mlist = js.Deserialize<List<MAmtList>>(ActRule);

                    //检查分发奖励是否超过顶限，如果超过直接跳过
                    //int TopNum = B_usercenter.GetTopNum(hat.ActID);
                    //项目已经发放的金额
                    //decimal totalAmt = B_usercenter.GetTopAmtCount(hat.ActID);
                    //第一次投资获取对应奖励 
                    decimal actamt = amt;//GetActAmt(mc, InvestAmt, TopNum);

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
                                                    mbaw.bonus_account_id = int.Parse(hua.UserAct.ToString());
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
                }
            }
            return t;
        }
        /// <summary>
        /// 活动结束页面
        /// </summary>
        /// <returns></returns>
        public ActionResult EndShow()
        {
            return View();
        }
    }
}