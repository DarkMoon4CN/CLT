using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.BLL;
using System.Data;
using ChuangLitouP2P.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PagedList;
using System.Configuration;
using ChuanglitouP2P.Common;

namespace ChuanglitouP2P.Controllers
{
    public class WangDaiController : Controller
    {
        IsoDateTimeConverter timeFormat = new IsoDateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
        };

        chuangtouEntities ef = new chuangtouEntities();

        private const string targetaddress = "www.chuanglitou.cn/invest_borrow_";

        private const string key = "wangDZJ0";

        WangDaiHelper wangdai = new WangDaiHelper();


        public ActionResult Login(string username, string password)
        {
            return Content(WangDailogin(username, password));
        }

        public ActionResult Index(string token = "", string date = "", int page = 1, int pageSize = 10)
        {
            return Content(GetBorrowingTarget(token, date, page, pageSize));
        }

        /// <summary>
        /// 网贷之家登录接口
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string WangDailogin(string username, string password)
        {
            try
            {
                string rightusername = ConfigurationManager.AppSettings["WangDaiUsername"].ToString();
                string rightpassword = ConfigurationManager.AppSettings["WangDaiPassword"].ToString();

                string returncontent = "";

                if (username != rightusername || password != rightpassword)
                {
                    returncontent = "{'data':{'token':''}}";
                    return returncontent;
                }
                string datenow = SystemTimeTransferToUnix();//将系统时间转换为时间戳

                datenow = EncryptHelper.Encrypt(datenow, key);//将时间戳加密

                returncontent = "{'data':{'token':'" + datenow + "'}}";

                return returncontent;
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog("网贷之家登录异常：" + ex.Message);
                return "{'data':{'token':''}}";
            }
        }

        /// <summary>
        /// 网贷之家获取数据接口
        /// </summary>
        /// <param name="token"></param>
        /// <param name="date"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public string GetBorrowingTarget(string token = "", string date = "", int page = 1, int pageSize = 10)
        {
            try
            {
                WDZJModel.JsonProject jsonproject = new WDZJModel.JsonProject();

                #region  判断时间戳

                if (String.IsNullOrEmpty(token))
                {
                    jsonproject.totalPage = "0";//总页数
                    jsonproject.currentPage = "1";//当前页数（从1开始）
                    jsonproject.totalCount = 0;//总标数
                    jsonproject.totalAmount = 0.00;//当天借款标总额
                    jsonproject.borrowList = null;//借款标信息

                    return (JsonConvert.SerializeObject(jsonproject, Formatting.Indented, timeFormat));//序列化json串
                }

                token = EncryptHelper.Decrypt(token, key);//将时间戳解密

                token = UnixTransferToDateTime(token).ToString();//将时间戳转换为时间

                TimeSpan ts = DateTime.Now - Convert.ToDateTime(token);

                if (ts.TotalHours > 1)
                {
                    jsonproject.totalPage = "0";//总页数
                    jsonproject.currentPage = "1";//当前页数（从1开始）
                    jsonproject.totalCount = 0;//总标数
                    jsonproject.totalAmount = 0.00;//当天借款标总额
                    jsonproject.borrowList = null;//借款标信息

                    return (JsonConvert.SerializeObject(jsonproject, Formatting.Indented, timeFormat));//序列化json串s
                }

                #endregion

                #region  获取借款标信息列表  Project

                DateTime dt;
                if (!DateTime.TryParse(date, out dt))
                {
                    date = DateTime.Now.ToString("yyyy-MM-dd");
                }

                if (page < 1)
                {
                    page = 1;
                }
                if (pageSize < 1)
                {
                    pageSize = 1;
                }
                else if (pageSize > 50)
                {
                    pageSize = 50;
                }

                int totalCount = 0;//总标数

                double totalAmount = 0;//当天借款标总额

                int pageNumber = page / 1;

                List<hx_Project_type> listprotype = ef.hx_Project_type.ToList();

                List<WDZJModel.Project> ListWDZJ = new List<WDZJModel.Project>();

                DataTable borrowdt = wangdai.GetBorrowTargetList(date, page, pageSize, out totalCount, out totalAmount);//满标的借款标列表

                if (borrowdt != null)
                {
                    List<int> listtargetid = new List<int>();
                    for (int a = 0; a < borrowdt.Rows.Count; a++)
                    {
                        listtargetid.Add(Convert.ToInt32(borrowdt.Rows[a]["targetid"]));
                    }
                    var recordlist = ef.hx_Bid_records.Where(x => listtargetid.Contains(x.targetid ?? 0)).ToList();

                    for (int i = 0; i < borrowdt.Rows.Count; i++)
                    {
                        WDZJModel.Project wdzjp = new WDZJModel.Project();
                        wdzjp.projectId = borrowdt.Rows[i]["targetid"].ToString();//项目主键
                        wdzjp.title = borrowdt.Rows[i]["borrowing_title"].ToString();//借款标题
                        wdzjp.amount = Convert.ToDouble(borrowdt.Rows[i]["borrowing_balance"]);//借款金额
                        wdzjp.schedule = (Convert.ToDouble(borrowdt.Rows[i]["fundraising_amount"]) / Convert.ToDouble(borrowdt.Rows[i]["borrowing_balance"]) * 100).ToString();///进度
                        wdzjp.interestRate = borrowdt.Rows[i]["annual_interest_rate"].ToString() + "%";//利率
                        wdzjp.deadline = Convert.ToInt32(borrowdt.Rows[i]["life_of_loan"]);//借款期限
                        if (Convert.ToInt32(borrowdt.Rows[i]["unit_day"]) == 1)//借款期限单位
                        {
                            wdzjp.deadlineUnit = "月";
                        }
                        else if (Convert.ToInt32(borrowdt.Rows[i]["unit_day"]) == 3)
                        {
                            wdzjp.deadlineUnit = "天";
                        }
                        else
                        {
                            wdzjp.deadlineUnit = "";
                        }

                        wdzjp.reward = 0;//奖励
                        wdzjp.type = listprotype.FirstOrDefault(x => x.project_type_id == Convert.ToInt32(borrowdt.Rows[i]["project_type_id"])).project_type_name;//借款标类型
                        if (Convert.ToInt32(borrowdt.Rows[i]["payment_options"]) == 1)//还款方法
                        {
                            wdzjp.repaymentType = 2;//按月等额本息
                        }
                        else if (Convert.ToInt32(borrowdt.Rows[i]["payment_options"]) == 3)
                        {
                            wdzjp.repaymentType = 5;//每月还息到期还本
                        }
                        else if (Convert.ToInt32(borrowdt.Rows[i]["payment_options"]) == 4)
                        {
                            wdzjp.repaymentType = 1;//一次性还本付息
                        }

                        wdzjp.subscribes = GetSubscribe(recordlist.Where(x => x.targetid == Convert.ToInt32(borrowdt.Rows[i]["targetid"])).ToList());//投标记录
                        wdzjp.userName = borrowdt.Rows[i]["borrower_registerid"].ToString();//借款人id
                        wdzjp.loanUrl = targetaddress + borrowdt.Rows[i]["targetid"].ToString() + ".html";//标的详细页面地址链接
                        wdzjp.successTime =Convert.ToDateTime(borrowdt.Rows[i]["end_time"]).ToString("yyyy-MM-dd HH:mm:ss");//满标时间

                        ListWDZJ.Add(wdzjp);
                    }
                }


                #endregion


                jsonproject.totalPage = Convert.ToInt32((totalCount - 1) / pageSize + 1).ToString();//总页数
                jsonproject.currentPage = page.ToString();//当前页数（从1开始）
                jsonproject.totalCount = totalCount;//总标数
                jsonproject.totalAmount = totalAmount;//当天借款标总额
                jsonproject.borrowList = ListWDZJ;//借款标信息

                return (JsonConvert.SerializeObject(jsonproject, Formatting.Indented, timeFormat));//序列化json串
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog("网贷之家获取数据异常：" + ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 根据标的id获得投标记录
        /// </summary>
        /// <param name="borrowtargetid"></param>
        /// <returns></returns>
        private List<WDZJModel.target> GetSubscribe(List<hx_Bid_records> recordlist)
        {
            List<WDZJModel.target> list = new List<WDZJModel.target>();

            if (recordlist != null)
            {
                foreach (var item in recordlist)
                {
                    WDZJModel.target target = new WDZJModel.target();
                    target.subscribeUserName = item.investor_registerid.ToString();//投标人id
                    target.amount = Convert.ToDouble(item.investment_amount);//投标金额
                    target.validAmount = Convert.ToDouble(item.investment_amount);//有效金额
                    target.addDate = Convert.ToDateTime(item.invest_time).ToString("yyyy-MM-dd HH:mm:ss");//投资时间
                    target.status = 1;//投标状态投标状态*1：全部通过 2：部分通过,注意：平台没有这个字段的默认为1
                    target.type = 0;//	标识手动或自动投标0：手动 1：自动，注意:平台没有这个字段的默认为0

                    list.Add(target);
                }
            }
            else
            {
                list = null;
            }
            return list;

        }


        /// <summary>
        /// 将系统时间转换为时间戳
        /// </summary>
        /// <returns></returns>
        private string SystemTimeTransferToUnix()
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);

            return timeStamp;
        }

        /// <summary>
        /// 将Unix时间戳转换为时间
        /// </summary>
        /// <returns></returns>
        private DateTime UnixTransferToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);

            return dtResult;
        }
    }
}