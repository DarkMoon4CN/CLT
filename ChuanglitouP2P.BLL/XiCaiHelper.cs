using ChuanglitouP2P.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Web;
using ChuanglitouP2P.DBUtility;
using System.Data;
using ChuangLitouP2P.Models;
using System.Collections.Specialized;

namespace ChuanglitouP2P.BLL
{
    public class XiCaiHelper
    {
        IsoDateTimeConverter timeFormat = new IsoDateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
        };
        private const string _client_id = "6d2a421f3e884a3e99226f449efe95b1";
        private const string _client_secret = "1edb7e1b92a64ce0aea60992d4f9fec0";
        //绑定回调接口地址
        private const string _bangdinghuidiao = "http://api.csai.cn/api/BindUserCallBack?client_id={0}&sign={1}";
        //推送产品接口地址
        private const string _tuisongchanpin = "http://api.csai.cn/api/push_p2p?client_id={0}&client_secret={1}&pid={2}";
        //投资回调接口地址
        private const string _touzihuidiao = "http://api.csai.cn/api/InvestCallBack?client_id={0}&sign={1}";
        // pc端导流地址
        private const string _link_website = "http://www.chuanglitou.cn/invest_borrow_{0}.html?channel={1}&type=1";
        //手机端导流地址
        private const string _mobile_link_website = "http://m.chuanglitou.cn/invest_borrow_{0}.html?channel={1}&type=1";
        ////绑定回调接口地址
        //private const string _bangdinghuidiao = "http://api.csai.cn/apitest/BindUserCallBack?client_id={0}&sign={1}";
        ////推送产品接口地址
        //private const string _tuisongchanpin = "http://api.csai.cn/apitest/push_p2p?client_id={0}&client_secret={1}&pid={2}";
        ////投资回调接口地址
        //private const string _touzihuidiao = "http://api.csai.cn/apitest/InvestCallBack?client_id={0}&sign={1}";
        //// pc端导流地址
        //private const string _link_website = "http://test.chuanglitou.cn/invest_borrow_{0}.html?channel={1}&type=1";
        ////手机端导流地址
        //private const string _mobile_link_website = "http://testm.chuanglitou.cn/invest_borrow_{0}.html?channel={1}&type=1";
        chuangtouEntities ef = new chuangtouEntities();
        private string _webp = "";
        public delegate string register(string Validatecode, string userpassword1, string mobile1, string username1, bool realMobileUser = false);
        public delegate string loginIn(string username, string userpassword, string Validatecode, int remember, bool realMobileUser = false);
        public XiCaiHelper(string webp)
        {
            _webp = webp;
        }
        /// <summary>
        /// 推送产品编号（状态为2 复审通过(开标上线) 并且在招标时间范围内）
        /// </summary>
        /// <returns></returns>
        public string PushAllTarget()
        {
            DateTime now = DateTime.Now;
            var targets = (from item in ef.hx_borrowing_target
                           where item.tender_state == 2
                           select item.targetid + "").ToArray();
            //pid为P2p平台产品唯一id ，如果有多个可用,隔开
            string pid = string.Join(",", targets);
            string res = PushP2PData(pid);
            return (res);
        }
        /// <summary>
        /// 推送标的编号数据方法
        /// </summary>
        /// <param name="pid">标的编号</param>
        /// <returns></returns>
        public string PushP2PData(string pid)
        {
            string html = Http.Post(string.Format(_tuisongchanpin, _client_id, _client_secret, pid));
            string res = "";
            var ret = JsonConvert.DeserializeObject<ReturnMessage>(html);
            if (ret.code == 0)
            {
                //Response.Write("<br>推送成功");
                res = "推送成功";
            }
            else
            {
                //Response.Write("<br>推送失败，原因：" + ret.ErrorMessage);
                res = res = "推送成功";
            }
            return res;
        }
        /// <summary>
        /// 希财获取产品数据接口（提供给希财，用来获取标的信息）
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="client_secret"></param>
        /// <returns></returns>

        public string get_p2p(string pid, string client_secret)
        {
            try
            {
                if (string.IsNullOrEmpty(pid))
                {
                    //Response.Write("{\"code\":1001,\"msg\":\"缺少参数pid\"}");
                    //Response.End();
                    return ("{\"code\":1001,\"msg\":\"缺少参数pid\"}");
                }

                if (client_secret != _client_secret)
                {
                    //Response.Write("{\"code\":1008,\"msg\":\"参数client_secret错误\"}");
                    //Response.End();
                    return ("{\"code\":1008,\"msg\":\"参数client_secret错误\"}");
                }
                Product product = new Product();
                int targetID = 0;
                if (!int.TryParse(pid, out targetID))
                {
                    return (JsonConvert.SerializeObject(product, Formatting.Indented, timeFormat));
                }

                var target = (from item in ef.hx_borrowing_target
                              where item.targetid == targetID
                              select item).FirstOrDefault();
                string sqlCount = "select count(*) from V_hx_Bid_records_borrowing_target where targetid = " + targetID;
                var investCount = DbHelperSQL.GetSingle(sqlCount);  //ef.V_hx_Bid_records_borrowing_target.Where(p => p.targetid == targetID).Count();
                var channelInvitedCode = ef.hx_Channel.Where(c => c.ChannelName == "xicai").First().Invitedcode;
                if (target == null)
                    return "{\"code\":0,\"data\":{}}";
                product = new Product()
                {
                    p2p_product_id = target.targetid + "",
                    product_name = target.borrowing_title,
                    life_cycle = target.unit_day == 1 ? (target.life_of_loan ?? 0) * 30 : (target.life_of_loan ?? 0),
                    ev_rate = (double)(target.annual_interest_rate ?? 0),
                    amount = target.borrowing_balance ?? 0,
                    invest_amount = target.fundraising_amount ?? 0,
                    inverst_mans =Convert.ToInt32(investCount),
                    underlying_start = Convert.ToDateTime(target.start_time),
                    link_website = string.Format(_link_website, target.targetid, channelInvitedCode),
                    product_state = borrowingStateTransfer((int)target.tender_state),
                    borrower = "借款人",//target.borrower_registerid
                    guarant_mode = 0,//担保方式 0 无担保 1 本息担保 2 本金担保
                    publish_time = Convert.ToDateTime(target.sys_time),
                    borrow_type = 4,//借款担保方式 1 抵押借款 2 信用借款 3 质押借款 4 第三方担保
                    pay_type = borrowingPaymentOptions((int)target.payment_options),
                    start_price = target.minimum ?? 0,
                    mobile_link_website = string.Format(_mobile_link_website, target.targetid, channelInvitedCode)
                };
                string res = "{\"code\":0,\"data\":" + JsonConvert.SerializeObject(product, Formatting.Indented, timeFormat) + "}";
                return (res);
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog("希财获取产品数据接口，异常消息：" + ex.Message + ",Trace:" + ex.StackTrace);
                return ("操作异常，请稍后重试");
            }
        }
        /// <summary>
        /// 转换标的状态为希财要求的值
        /// </summary>
        /// <param name="targetState"></param>
        /// <returns></returns>
        private int borrowingStateTransfer(int targetState)
        {
            //-1 录入 0 审核中 1初审通过 2 复审通过(开标上线) 3 满标(还款中) 4 放款(还款中) 5 已还清 6初审未通过 7 复审未通过 8 流标
            //-1：流标，0：筹款中，1:已满标，2：已开始还款，3：预发布，4：还款完成，5：逾期
            switch (targetState)
            {
                case 8: return -1;
                case 2: return 0;
                case 3: return 1;
                case 4: return 2;
                case 5: return 4;
                default: return 0;
            }
        }
        /// <summary>
        /// 转换还款方式为希财要求的值
        /// </summary>
        /// <param name="targetPaymentOptions"></param>
        /// <returns></returns>
        private int borrowingPaymentOptions(int targetPaymentOptions)
        {
            //1 按月等额本息 3 每月还息，到期还本4 一次性还本付息
            //还款方式 1 按月付息 到期还本 2 按季付息，到期还本3 每月等额本息 4 到期还本息 5 按周等额本息还款 6按周付息，到期还本 7：利随本清；8：等本等息；9：按日付息，到期还本；10：按半年付息，到期还本；11：按一年付息，到期还本；100：其他方式； 
            switch (targetPaymentOptions)
            {
                case 1: return 3;
                case 3: return 1;
                case 4: return 4;
                default: return 1;
            }
        }
        /// <summary>
        /// 注册登录返回值模板
        /// </summary>
        class registerBackData { public string rs { get; set; } public string url { get; set; } public string error { get; set; } }
        /// <summary>
        /// 自动注册登录
        /// </summary>
        /// <param name="sign"></param>
        /// <returns></returns>

        public string AutoRegisterLogin(string sign, HttpResponseBase Response, register Register, HttpRequestBase Request, loginIn LoginIN)
        {
            try
            {
                string result = Decrypt(sign, _client_secret.Substring(0, 8));

                NameValueCollection collection = HttpUtility.ParseQueryString(result);

                string phone = collection["phone"];
                string name = collection["name"];
                string pid = collection["pid"];
                string t = collection["t"];
                string userid = collection["userid"];
                string display = collection["display"];
                if (display == null) { display = "pc"; }

                if (CheckTimeOut(t)) { return ("该操作已经失效，请重新操作"); }

                var checkExistUser = (from item in ef.hx_member_table where item.mobile == phone || item.username == phone select item).FirstOrDefault();
                if (checkExistUser == null)
                {
                    //注册逻辑
                    string password = DateTime.Now.ToString("yyyyMMddHHmmssfffff");
                    string code = ef.hx_Channel.Where(c => c.ChannelName == "xicai").First().Invitedcode;
                    AddCache(code, Response);
                    string res = Register("", password.Substring(password.Length - 8, 8), phone, name + phone.Substring(phone.Length - 4, 4), true).ToString();
                    registerBackData registerRes = JsonConvert.DeserializeObject<registerBackData>(res);
                    if (registerRes.rs == "y")
                    {
                        var user = ef.hx_member_table.Where(c => c.mobile == phone).FirstOrDefault();
                        if (user == null)
                        {
                            LogInfo.WriteLog("注册成功，单用户信息不存在");
                            return ("操作失败，请重新操作");
                        }
                        if (!InserXiCaiUser(user, userid, display))
                            return ("操作失败，请重新操作");
                        //YMSendSMS.Send_SMS(phone, "您已在创利投成功注册，账号为：密码为：");
                        string signData = string.Format("phone={0}&name={1}&result={2}&display={3}&userid={4}", phone, name + phone.Substring(phone.Length - 4, 4), 1, display, userid);
                        string signPostData = Encrypt(signData, _client_secret.Substring(0, 8));
                        signPostData = System.Web.HttpContext.Current.Server.UrlEncode(signPostData);
                        string html = Http.Post(string.Format(_bangdinghuidiao, _client_id, signPostData));
                        var ret = Newtonsoft.Json.JsonConvert.DeserializeObject<ReturnMessage>(html);
                        if (ret.code != 0)
                        {
                            LogInfo.WriteLog("希财自动注册成功之后推送失败,原因：" + ret.ErrorMessage);
                        }

                        return XiCaiLogin(phone, pid, display, Request, LoginIN);
                    }
                    else
                    {
                        return (registerRes.error);
                    }
                }
                else//登录逻辑
                {
                    return XiCaiLogin(phone, pid, display, Request, LoginIN);
                }
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog("希财自动注册接口异常，异常消息：" + ex.Message + ",Trace:" + ex.StackTrace);
                return ("操作异常，请稍后重试");
            }
        }

        public bool InserXiCaiUser(hx_member_table user, string userid = "0", string display = "pc")
        {
            XiCaiUser xiCaiUser = new XiCaiUser()
            {
                Mobile = user.mobile,
                XiCaiUserID = int.Parse(userid),
                RegisterUserID = user.registerid,
                Display = display
            };
            if (!ef.XiCaiUsers.Any(c => c.Mobile == user.mobile))
            {
                ef.XiCaiUsers.Add(xiCaiUser);
                if (ef.SaveChanges() < 1)
                {
                    LogInfo.WriteLog("注册成功，插入希财数据失败");
                    return false; //("操作失败，请重新操作");
                }
            }
            return true;
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="channelCode"></param>
        private void AddCache(string channelCode, HttpResponseBase Response)
        {
            string cInvitedcode = channelCode;
            if (string.IsNullOrEmpty(cInvitedcode))
            {//此前放出了一批错误连接channnel
                cInvitedcode = Utils.CheckSQLHtml(cInvitedcode);
            }
            if (!string.IsNullOrEmpty(cInvitedcode))
            {
                var keyValue = new Dictionary<string, string>();
                keyValue.Add("Invitedcode", cInvitedcode);
                Utils.SetInvCookie("channel", keyValue);
            }

            var invitedcode = Utils.CheckSQLHtml("invitedcode");
            if (!string.IsNullOrWhiteSpace(invitedcode))
            {
                string sql = "select registerid,invitedcode from hx_member_table where invitedcode='" + invitedcode + "' ";

                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                if (dt.Rows.Count > 0)
                {
                    HttpCookie cok = new HttpCookie("Invitation");
                    cok.Values.Add("InvCode", DESEncrypt.Encrypt(invitedcode, _webp));
                    cok.Values.Add("CodeUid", DESEncrypt.Encrypt(dt.Rows[0]["registerid"].ToString(), _webp));
                    cok.Expires = DateTime.Now.AddDays(30);
                    Response.AppendCookie(cok);
                }
            }
        }
        /// <summary>
        /// 自动登录逻辑
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        private string XiCaiLogin(string phone, string pid, string display, HttpRequestBase Request, loginIn LoginIN)
        {
            var user = ef.hx_member_table.Where(c => c.mobile == phone).FirstOrDefault();
            //string sourcePwd = DESEncrypt.Decrypt(, _webp);
            string res = LoginIN(phone, user.password, "", 1, true).ToString();
            registerBackData loginRes = JsonConvert.DeserializeObject<registerBackData>(res);
            if (loginRes.rs == "y")
            {
                return "RedictXXX" + (loginRes.url + "invest_borrow_" + pid + ".html");
            }
            else
            {
                return (loginRes.error);
            }
        }
        public void InvestCallBack(int bidRecordID, HttpRequestBase Request)
        {
            //var bidRecord =  ef.hx_Bid_records.Where(c => c.bid_records_id == bidRecordID).FirstOrDefault();
            var dataTotal = (from item in ef.hx_Bid_records
                             join target in ef.hx_borrowing_target
                             on item.targetid equals target.targetid
                             join user in ef.hx_member_table
                             on item.investor_registerid equals user.registerid
                             join income in (from item in ef.hx_income_statement
                                             group item by new { targetID = item.targetid, borrower = item.borrower_registerid, investor = item.investor_registerid } into g
                                             select new
                                             {
                                                 investor = g.Key.investor,
                                                 borrower = g.Key.borrower,
                                                 targetID = g.Key.targetID,
                                                 interestPayment = g.Sum(c => c.interestpayment ?? 0)
                                             })
                             on new { targetID = item.targetid, investor = item.investor_registerid, borrower = item.borrower_registerid }
                             equals new { targetID = income.targetID, investor = income.investor, borrower = income.borrower }
                             join xicai in ef.XiCaiUsers
                             on item.investor_registerid equals xicai.RegisterUserID
                             where item.bid_records_id == bidRecordID
                             select new
                             {
                                 phone = user.mobile,
                                 invest_time = item.invest_time,
                                 targetid = target.targetid,
                                 invest_Amt = item.investment_amount,
                                 earnings = income.interestPayment,
                                 term = target.unit_day == 1 ? (target.life_of_loan ?? 0) * 30 : (target.life_of_loan ?? 0),
                                 rate = item.annual_interest_rate ?? 0,
                                 xicaiUserID = xicai.XiCaiUserID
                             }).FirstOrDefault();
            if (dataTotal == null)
            {
                LogInfo.WriteLog("希财渠道接口投资回调，投资记录不存在，记录编号为：" + bidRecordID);
                return;
            }
            //var user = ef.hx_member_table.Where(c => c.registerid == bidRecord.investor_registerid).FirstOrDefault();
            string query = string.Format("id={0}&phone={1}&datetime={2}&commision={3}&pid={4}&url={5}&investamount={6}&earnings={7}&term={8}&rate={9}&display={10}&userid={11}", bidRecordID, dataTotal.phone, Convert.ToDateTime(dataTotal.invest_time).ToString("yyyy-MM-dd HH:mm:ss"), 0, dataTotal.targetid, Request.Url.GetLeftPart(UriPartial.Authority) + "/invest_borrow_" + dataTotal.targetid + ".html", (dataTotal.invest_Amt ?? 0).ToString("0.00"), dataTotal.earnings.ToString("0.00"), dataTotal.term, dataTotal.rate, "pc", dataTotal.xicaiUserID);
            string sign = System.Web.HttpContext.Current.Server.UrlEncode(Encrypt(query, _client_secret.Substring(0, 8)));
            string html = Http.Post(string.Format(_touzihuidiao, _client_id, sign));
            LogInfo.WriteLog(string.Format("希财渠道接口投资回调,发送数据：client_id:{0},sign:{1}，返回数据:{2}", _client_id, sign, html));
        }
        /// <summary>
        /// 希财用户统计接口
        /// </summary>
        /// <param name="t"></param>
        /// <param name="token"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>

        public string Tongji_User(string t = "1", string token = "", string startdate = "", string enddate = "", int page = 1, int pagesize = 10)
        {
            try
            {
                ///t	时间戳（1970-1-1到现在的总秒数）
                //token	token=md5(md5(t)+client_secret) 其中t值为上面的时间戳，client_secret为对应网站的client_secret
                //贵方可以通过t和token(小写)两个参数的值来判断请求的合法性，md5为32位小写
                //startdate	开始时间（用户注册时间），为空则不限制（格式为 2015-1-20）
                //enddate	结束时间（用户注册时间），为空则不限制（格式为 2015-1-20）
                //page	页码，默认为1
                //pagesize	返回记录条数，默认为10
                if (CheckTimeOut(t))
                {
                    return ("{\"code\":1008,\"msg\":\"请求失效\"}");
                }
                if (string.IsNullOrEmpty(token))
                {
                    return ("{\"code\":1008,\"msg\":\"参数token错误\"}");
                }
                string localToken = Utils.MD5(Utils.MD5(t) + _client_secret); // LiumiTools.MD5(LiumiTools.MD5(t) + _client_secret); //EncryptHelper.GetMd5Str32(EncryptHelper.GetMd5Str32(t, "x") + _client_secret, "x");
                if (localToken != token)
                {
                    return ("{\"code\":1008,\"msg\":\"请求被篡改\"}");
                }
                var retData = from item in ef.hx_member_table
                              join channel in ef.hx_Channel
                              on item.channel_invitedcode equals channel.Invitedcode
                              where channel.ChannelName == "xicai"
                              select item;
                if (!string.IsNullOrWhiteSpace(startdate))
                {
                    DateTime sTime = DateTime.Parse(startdate);
                    retData = from item in retData
                              where item.registration_time >= sTime
                              select item;
                }
                if (!string.IsNullOrWhiteSpace(enddate))
                {
                    DateTime eTime = DateTime.Parse(enddate);
                    retData = from item in retData
                              where item.registration_time <= eTime
                              select item;
                }
                if (page <= 0) { page = 1; }
                if (pagesize > 50) { pagesize = 50; }
                if (pagesize <= 0) { pagesize = 1; }
                var resData = from item in retData
                              join xicai in ef.XiCaiUsers
                              on item.registerid equals xicai.RegisterUserID
                              join money in
                              (from item in ef.hx_Bid_records
                               group item by item.investor_registerid into g
                               select new
                               {
                                   investor = g.Key,
                                   investMoney = g.Sum(c => c.investment_amount)
                               })
                               on item.registerid equals money.investor into moneyL
                              from moneyLeft in moneyL.DefaultIfEmpty()
                              select new
                              {
                                  id = item.registerid.ToString(),
                                  username = item.username,
                                  realname = item.realname,
                                  email = "",
                                  ip = item.lastloginIP,
                                  phone = item.mobile,
                                  qq = "",
                                  regtime = item.registration_time,
                                  totalmoney = moneyLeft == null ? 0 : moneyLeft.investMoney,
                                  display = xicai.Display
                              };
                int totalCount = resData.Count();
                List<tj_user> res = (from item in resData.OrderBy(c => c.regtime).Skip((page - 1) * pagesize).Take(pagesize).ToList()
                                     select new tj_user
                                     {
                                         email = item.email,
                                         id = item.id,
                                         ip = item.ip,
                                         phone = item.phone,
                                         qq = item.qq,
                                         realname = item.realname,
                                         regtime = Convert.ToDateTime(item.regtime).ToString("yyyy-MM-dd HH:mm:ss"),
                                         totalmoney = (item.totalmoney ?? 0),
                                         username = item.username,
                                         display = item.display
                                     }).ToList();
                string r = "{\"list\":" + JsonConvert.SerializeObject(res, Formatting.Indented, timeFormat) + ",\"total\":" + totalCount + ",\"code\":0}";
                return (r);
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog("希财用户统计接口，异常消息：" + ex.Message + ",Trace:" + ex.StackTrace);
                return ("操作异常，请稍后重试");
            }
        }
        /// <summary>
        /// 投资统计接口
        /// </summary>
        /// <param name="t"></param>
        /// <param name="token"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public string Tongji_Invest(HttpRequestBase Request, string t = "1", string token = "", string startdate = "", string enddate = "", int page = 1, int pagesize = 10)
        {
            try
            {
                if (CheckTimeOut(t))
                {
                    return ("{\"code\":1008,\"msg\":\"请求失效\"}");
                }
                if (string.IsNullOrEmpty(token))
                {
                    return ("{\"code\":1008,\"msg\":\"参数token错误\"}");
                }
                string localToken = Utils.MD5(Utils.MD5(t) + _client_secret); //EncryptHelper.GetMd5Str32(EncryptHelper.GetMd5Str32(t, "x") + _client_secret, "x");
                if (localToken != token)
                {
                    return ("{\"code\":1008,\"msg\":\"请求被篡改\"}");
                }
                //var retData = from item in ef.hx_member_table
                //              join channel in ef.hx_Channel
                //              on item.channel_invitedcode equals channel.Invitedcode
                //              join invest in ef.hx_Bid_records
                //              on item.registerid equals invest.investor_registerid
                //              where channel.ChannelName == "xicai"
                //              select new
                //              {
                //                  username = item.username,
                //                  invest_time = invest.invest_time,
                //                  targetid = invest.targetid,
                //                  investor_registerid = invest.investor_registerid,
                //                  borrower_registerid = invest.borrower_registerid,
                //                  bid_records_id = invest.bid_records_id,
                //                  investment_amount = invest.investment_amount,
                //                  mobile = item.mobile
                //              };
                StringBuilder sql = new StringBuilder(@"
	                            select 
                                row_number() over (order by c.registration_time asc) as RowNumber,
	                            '' url,
	                            0 commision,
	                            e.targetid pid,
	                            e.invest_time [datetime],
	                            [money] Earnings,
	                            bid_records_id id,
	                            investment_amount [money],
	                            c.mobile phone,
	                            f.annual_interest_rate rate,
	                            case f.unit_day when 1 then ISNULL(life_of_loan,0)*30 else life_of_loan end life_cycle,
	                            c.username username
	                            from hx_member_table c
	                            join hx_Channel d
	                            on c.channel_invitedcode = d.Invitedcode
	                            join hx_Bid_records e
	                            on c.registerid = e.investor_registerid
	                            join hx_borrowing_target f
	                            on e.targetid = f.targetid
	                            left join 
	                            (select sum(interestpayment) [money],
	                            investor_registerid,
	                            borrower_registerid,
	                            targetid 
	                            from hx_income_statement 
	                            group by investor_registerid,borrower_registerid,targetid) g
	                            on e.investor_registerid = g.investor_registerid and e.borrower_registerid = g.borrower_registerid and e.targetid = g.targetid
	                            where d.ChannelName = 'xicai'");

                StringBuilder sqlCount = new StringBuilder(@"
	                            select count(*)
	                            from hx_member_table c
	                            join hx_Channel d
	                            on c.channel_invitedcode = d.Invitedcode
	                            join hx_Bid_records e
	                            on c.registerid = e.investor_registerid
	                            join hx_borrowing_target f
	                            on e.targetid = f.targetid
	                            left join 
	                            (select sum(interestpayment) [money],
	                            investor_registerid,
	                            borrower_registerid,
	                            targetid 
	                            from hx_income_statement 
	                            group by investor_registerid,borrower_registerid,targetid) g
	                            on e.investor_registerid = g.investor_registerid and e.borrower_registerid = g.borrower_registerid and e.targetid = g.targetid
	                            where d.ChannelName = 'xicai'");
                if (!string.IsNullOrWhiteSpace(startdate))
                {
                    DateTime sTime = DateTime.Parse(startdate);
                    //retData = from item in retData
                    //          where item.invest_time >= sTime
                    //          select item;
                    sql.Append(" and e.invest_time >=");
                    sql.Append(sTime.ToString("yyyy-MM-dd"));

                    sqlCount.Append(" and e.invest_time >=");
                    sqlCount.Append(sTime.ToString("yyyy-MM-dd"));
                }
                if (!string.IsNullOrWhiteSpace(enddate))
                {
                    DateTime eTime = DateTime.Parse(enddate);
                    //retData = from item in retData
                    //          where item.invest_time <= eTime
                    //          select item;
                    sql.Append(" and e.invest_time <=");
                    sql.Append(eTime.ToString("yyyy-MM-dd"));
                    sqlCount.Append(" and e.invest_time <=");
                    sqlCount.Append(eTime.ToString("yyyy-MM-dd"));
                }
                //var resData = from item in retData
                //              join target in ef.hx_borrowing_target
                //              on item.targetid equals target.targetid
                //              join earning in
                //              (from item in ef.hx_income_statement
                //               group item by new
                //               {
                //                   investor = item.investor_registerid,
                //                   borrower = item.borrower_registerid,
                //                   targetID = item.targetid
                //               } into g
                //               select new
                //               {
                //                   investor = g.Key.investor,
                //                   borrower = g.Key.borrower,
                //                   targetID = g.Key.targetID,
                //                   money = g.Sum(c => c.interestpayment ?? 0)
                //               })
                //               on new { investor = item.investor_registerid, borrower = item.borrower_registerid, targetID = item.targetid }
                //               equals new { investor = earning.investor, borrower = earning.borrower, targetID = earning.targetID }
                //              select new
                //              {
                //                  datetime = item.invest_time,
                //                  Earnings = earning.money,
                //                  id = item.bid_records_id,
                //                  money = item.investment_amount,
                //                  phone = item.mobile,
                //                  pid = item.targetid,
                //                  rate = target.annual_interest_rate,
                //                  term = target.unit_day == 1 ? (target.life_of_loan ?? 0) * 30 : (target.life_of_loan ?? 0),
                //                  username = item.username
                //              };

                if (page <= 0) { page = 1; }
                if (pagesize > 50) { pagesize = 50; }
                if (pagesize <= 0) { pagesize = 1; }
                DataTable dtCount = DbHelperSQL.GET_DataTable_List(sqlCount.ToString());
                string filnalSql = "select top " + pagesize + @" * from (" + sql.ToString() + ")as temp where RowNumber > " + (pagesize * (page - 1));
                DataTable dtRecord = DbHelperSQL.GET_DataTable_List(filnalSql);
                //var totalCount = resData.Count();
                //var cc = resData.OrderBy(c => c.datetime).Skip((page - 1) * pagesize).Take(pagesize);
                //var res = (from item in cc.ToList()
                //           select new tj_data
                //           {
                //               url = Request.Url.GetLeftPart(UriPartial.Authority) + "/invest_borrow_" + item.pid + ".html",
                //               commision = 0,
                //               pid = (item.pid ?? 0).ToString(),
                //               datetime = (Convert.ToDateTime(item.datetime).ToString("yyyy-MM-dd HH:mm:ss")),
                //               Earnings = item.Earnings,
                //               id = item.id.ToString(),
                //               money = item.money ?? 0,
                //               phone = item.phone,
                //               rate = item.rate ?? 0,
                //               life_cycle = item.term,
                //               username = item.username
                //           }).ToList();
                List<tj_data> res = new List<tj_data>();
                res = ConvertDataTable<tj_data>.ConvertToList(dtRecord);
                int totalCount = Convert.ToInt32(dtCount.Rows[0][0]);
                string r = "{\"list\":" + JsonConvert.SerializeObject(res, Formatting.Indented, timeFormat) + ",\"total\":" + totalCount + ",\"code\":0}";
                return (r);
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog("投资统计接口，异常消息：" + ex.Message + ",Trace:" + ex.StackTrace+ex.InnerException.StackTrace);
                return ("操作异常，请稍后重试");
            }
        }
        /// <summary>
        /// Des加密
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        private string Encrypt(string Text, string Key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(Key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(Key);
            des.Mode = CipherMode.CBC;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                cs.Close();
            }
            string str = Convert.ToBase64String(ms.ToArray());
            ms.Close();
            return str;

        }

        /// <summary>
        /// Des解密
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        private string Decrypt(string Text, string Key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Convert.FromBase64String(Text);//Encoding.UTF8.GetBytes(source);

            des.Key = ASCIIEncoding.ASCII.GetBytes(Key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(Key);
            des.Mode = CipherMode.CBC;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                cs.Close();
            }
            string str = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return str;
        }

        /// <summary>
        /// 根据时间戳判定操作是否已经失效
        /// </summary>
        /// <param name="t"></param>
        /// <returns>true：失效，false：有效</returns>
        private bool CheckTimeOut(string t)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(double.Parse(t));
            TimeSpan diff = DateTime.Now - time;
            return diff.TotalMinutes > 1000;
        }
    }
    /// <summary>
    /// 希财返回消息模板
    /// </summary>
    class ReturnMessage
    {
        private int _code;
        private string _message;
        private string _errormessage;

        public int code { get { return _code; } set { _code = value; } }

        public string Message { get { return _message; } set { _message = value; } }

        public string ErrorMessage { get { return _errormessage; } set { _errormessage = value; } }
    }
    /// <summary>
    /// Http请求帮助类
    /// </summary>
    public class Http
    {
        /// <summary>
        /// HttpPost请求
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string Post(string Url)
        {
            return Post(Url, "");
        }
        /// <summary>
        /// HttpPost请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Post(string Url, string param)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Url);

            byte[] bs = Encoding.UTF8.GetBytes(param);
            string responseData = String.Empty;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = bs.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Close();
            }
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseData = reader.ReadToEnd().ToString();
                    }
                }
            }
            catch (WebException ex)
            {
                using (HttpWebResponse response = (HttpWebResponse)ex.Response)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseData = reader.ReadToEnd().ToString();
                    }
                }
            }
            return responseData;
        }
    }
    /// <summary>
    /// 希财产品信息模板
    /// </summary>
    class Product
    {
        private string _p2p_product_id;
        private string _product_name;
        private int _product_state;
        private int _isexp;
        private int _life_cycle;
        private double _ev_rate;
        private decimal _amount;
        private decimal _invest_amount;
        private int _inverst_mans;
        private DateTime _underlying_start;
        private DateTime? _underlying_end;
        private string _link_website;
        private string _borrower;
        private int _guarant_mode;
        private string _guarantors;
        private DateTime _publish_time;
        private DateTime? _repay_start_time;
        private DateTime? _repay_end_time;
        private int _borrow_type;
        private int _pay_type;
        private decimal _start_price;
        private int _step_price;
        private string _mobile_link_website;

        private string _charge;

        /// <summary>
        /// P2P平台产品唯一id
        /// </summary>
        public string p2p_product_id { get { return _p2p_product_id; } set { _p2p_product_id = value; } }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string product_name { get { return _product_name; } set { _product_name = value; } }

        /// <summary>
        /// 产品募集状态 -1：流标，0：筹款中，1:已满标，2：已开始还款，3：预发布，4：还款完成，5：逾期
        /// </summary>
        public int product_state { get { return _product_state; } set { _product_state = value; } }

        /// <summary>
        /// 是否为新手标，1为是，0为否。 默认为0
        /// </summary>
        public int isexp { get { return _isexp; } set { _isexp = value; } }

        /// <summary>
        /// 产品周期（天）, 如果为其他单位，请转换为天
        /// </summary>
        public int life_cycle { get { return _life_cycle; } set { _life_cycle = value; } }

        /// <summary>
        /// 年收益率(%)， 不要带 %
        /// </summary>
        public double ev_rate { get { return _ev_rate; } set { _ev_rate = value; } }

        /// <summary>
        /// 募集金额(元)
        /// </summary>
        public decimal amount { get { return _amount; } set { _amount = value; } }

        /// <summary>
        /// 已募集金额(元)
        /// </summary>
        public decimal invest_amount { get { return _invest_amount; } set { _invest_amount = value; } }

        /// <summary>
        /// 投资人数
        /// </summary>
        public int inverst_mans { get { return _inverst_mans; } set { _inverst_mans = value; } }

        /// <summary>
        /// 标的开始时间
        /// </summary>
        public DateTime underlying_start { get { return _underlying_start; } set { _underlying_start = value; } }

        /// <summary>
        /// 标的结束时间
        /// </summary>
        public DateTime? underlying_end { get { return _underlying_end; } set { _underlying_end = value; } }

        /// <summary>
        /// 在P2P平台的链接地址 
        /// </summary>
        public string link_website { get { return _link_website; } set { _link_website = value; } }

        /// <summary>
        /// 借款人名称
        /// </summary>
        public string borrower { get { return _borrower; } set { _borrower = value; } }

        /// <summary>
        /// 担保方式 0 无担保 1 本息担保 2 本金担保
        /// </summary>
        public int guarant_mode { get { return _guarant_mode; } set { _guarant_mode = value; } }

        /// <summary>
        /// 担保方名称 第三方担保时，需提供担保方名称，否则置为空
        /// </summary>
        public string guarantors { get { return _guarantors; } set { _guarantors = value; } }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime publish_time { get { return _publish_time; } set { _publish_time = value; } }

        /// <summary>
        /// 还款开始时间，满标时必须，未满标时没有可不需要
        /// </summary>
        public DateTime? repay_start_time { get { return _repay_start_time; } set { _repay_start_time = value; } }

        /// <summary>
        /// 还款结束时间，满标时必须，未满标时没有可不需要
        /// </summary>
        public DateTime? repay_end_time { get { return _repay_end_time; } set { _repay_end_time = value; } }

        /// <summary>
        /// 借款担保方式 1 抵押借款 2 信用借款 3 质押借款 4 第三方担保
        /// </summary>
        public int borrow_type { get { return _borrow_type; } set { _borrow_type = value; } }

        /// <summary>
        /// 还款方式 1 按月付息 到期还本 2 按季付息，到期还本3 每月等额本息 4 到期还本息 5 按周等额本息还款 6按周付息，到期还本 7：利随本清；8：等本等息；9：按日付息，到期还本；10：按半年付息，到期还本；11：按一年付息，到期还本；100：其他方式；
        /// </summary>
        public int pay_type { get { return _pay_type; } set { _pay_type = value; } }

        /// <summary>
        /// 起投金额，单位元
        /// </summary>
        public decimal start_price { get { return _start_price; } set { _start_price = value; } }

        /// <summary>
        /// 追加投入金额，单位元
        /// </summary>
        public int step_price { get { return _step_price; } set { _step_price = value; } }

        /// <summary>
        /// 手续费说明
        /// </summary>
        public string charge { get { return _charge; } set { _charge = value; } }
        /// <summary>
        /// 移动端链接地址
        /// </summary>
        public string mobile_link_website { get { return _mobile_link_website; } set { _mobile_link_website = value; } }
    }
    /// <summary>
    /// 希财用户统计模板
    /// </summary>
    class tj_user
    {
        private string _id;
        private string _username;
        private string _realname;
        private string _display;
        private string _phone;
        private string _qq;
        private string _email;
        private string _ip;
        private string _regtime;
        private decimal _totalmoney;
        /// <summary>
        /// 用户ID
        /// </summary>
        public string id { get { return _id; } set { _id = value; } }
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get { return _username; } set { _username = value; } }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string realname { get { return _realname; } set { _realname = value; } }
        /// <summary>
        /// 来源 pc or mobile
        /// </summary>
        public string display { get { return _display; } set { _display = value; } }
        /// <summary>
        /// 电话
        /// </summary>
        public string phone { get { return _phone; } set { _phone = value; } }

        public string qq { get { return _qq; } set { _qq = value; } }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get { return _email; } set { _email = value; } }
        /// <summary>
        /// ip地址
        /// </summary>
        public string ip { get { return _ip; } set { _ip = value; } }
        /// <summary>
        /// 注册时间
        /// </summary>
        public string regtime { get { return _regtime; } set { _regtime = value; } }
        /// <summary>
        /// 投资总金额
        /// </summary>
        public decimal totalmoney { get { return _totalmoney; } set { _totalmoney = value; } }

    }
    /// <summary>
    /// 希财数据统计模板
    /// </summary>
    class tj_data
    {
        private string _id;
        private string _pid;
        private string _username;
        private string _datetime;
        private decimal _money;
        private decimal _commision;
        private string _phone;
        private string _url;
        private int _life_cycle;
        private decimal _rate;
        private decimal _Earnings;

        /// <summary>
        /// 投资id
        /// </summary>
        public string id { get { return _id; } set { _id = value; } }
        /// <summary>
        /// 平台产品id
        /// </summary>
        public string pid { get { return _pid; } set { _pid = value; } }
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get { return _username; } set { _username = value; } }
        /// <summary>
        /// 投资时间
        /// </summary>
        public string datetime { get { return _datetime; } set { _datetime = value; } }
        /// <summary>
        /// 投资金额
        /// </summary>
        public decimal money { get { return _money; } set { _money = value; } }
        /// <summary>
        /// 返佣金额
        /// </summary>
        public decimal commision { get { return _commision; } set { _commision = value; } }
        /// <summary>
        /// 用户手机
        /// </summary>
        public string phone { get { return _phone; } set { _phone = value; } }
        /// <summary>
        /// 投资的众筹平台产品地址 
        /// </summary>
        public string url { get { return _url; } set { _url = value; } }
        /// <summary>
        /// 投资期限(以天为参数)
        /// </summary>
        public int life_cycle { get { return _life_cycle; } set { _life_cycle = value; } }
        /// <summary>
        /// 产品利率
        /// </summary>
        public decimal rate { get { return _rate; } set { _rate = value; } }
        /// <summary>
        /// 用户投资收益(若无请填0)
        /// </summary>
        public decimal Earnings { get { return _Earnings; } set { _Earnings = value; } }
    }
}
