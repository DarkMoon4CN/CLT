using ChuanglitouP2P.Areas.Admin.Controllers.Filters;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model.chinapnr.QueryCardInfo;
using ChuangLitouP2P.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class TemplateController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/Template
        [AdminVaildate(false, true)]
        public ActionResult Index()
        {
            return View();
        }
        [AdminVaildate(false, true)]
        public ActionResult Email()
        {
            var list_SMSEmail = (from a in ef.hx_td_SMSEmail where a.SEtype == 0 select a).ToList<hx_td_SMSEmail>();
            ViewBag.SMSEmail = list_SMSEmail;

            return View();
        }
        [AdminVaildate(false, true)]
        public ActionResult SMS()
        {
            var list_SMSEmail = (from a in ef.hx_td_SMSEmail where a.SEtype == 1 select a).ToList<hx_td_SMSEmail>();
            ViewBag.SMSEmail = list_SMSEmail;
            return View();
        }

        public ActionResult checksmseamil(int data = 0, string Act = "")
        {
            if (Act == "check")
            {
                var model = ef.hx_td_SMSEmail.Where(p => p.SmsEmailId == data).SingleOrDefault();

                return Content(model.SEContext);

            }
            else
            {
                return Content("");
            }

        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate()]
        public ActionResult EmailPost(hx_td_SMSEmail p, string SEContext = "")
        {
            string str = "";

            string[] proNames;

            proNames = new string[] { "SmsEmailId", "SEContext" };


            p = (hx_td_SMSEmail)Utils.ValidateModelClass(p);

            p.SEContext = SEContext;

            DbEntityEntry entry = ef.Entry<hx_td_SMSEmail>(p);
            entry.State = EntityState.Unchanged;

            foreach (string ProName in proNames)
            {
                entry.Property(ProName).IsModified = true;
            }

            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("邮件模板修改成功!", "/Admin/Template/Email");
            }
            else
            {
                str = StringAlert.Alert("邮件模板修改失败!", "/Admin/Template/Email");
            }
            return Content(str, "text/html");
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [AdminVaildate()]
        public ActionResult SMSPost(hx_td_SMSEmail p, string SEContext = "")
        {
            string str = "";

            string[] proNames;

            proNames = new string[] { "SmsEmailId", "SEContext" };


            p = (hx_td_SMSEmail)Utils.ValidateModelClass(p);

            p.SEContext = SEContext;

            DbEntityEntry entry = ef.Entry<hx_td_SMSEmail>(p);
            entry.State = EntityState.Unchanged;

            foreach (string ProName in proNames)
            {
                entry.Property(ProName).IsModified = true;
            }

            int i = ef.SaveChanges();
            if (i > 0)
            {
                str = StringAlert.Alert("短信模板修改成功!", "/Admin/Template/SMS");
            }
            else
            {
                str = StringAlert.Alert("短信模板修改失败!", "/Admin/Template/SMS");
            }
            return Content(str, "text/html");
        }

        public ActionResult QueryPayQuota()
        {
            List<hx_td_Bank> Ucard = new List<hx_td_Bank>();

            Ucard = ef.hx_td_Bank.Where(p => p.Isquick == 1).ToList();


            return View(Ucard);
        }

        public ActionResult QueryPay()
        {

            M_QueryPayQuota m = new M_QueryPayQuota();

            m.Version = "10";
            m.CmdId = "QueryPayQuota";
            m.MerCustId = Utils.GetMerCustID();
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.Version);
            chkVal.Append(m.CmdId);
            chkVal.Append(m.MerCustId);
            string chkv = LiumiTools.MD5(chkVal.ToString());


            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

            //  Response.Write("加签字符:" + str.ToString() + "<br>");

            m.ChkValue = sbChkValue.ToString();
            string strt = StringAlert.Alert("数据同步成功!", "/Admin/Template/QueryPayQuota");
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values.Add("Version", m.Version);
                values.Add("CmdId", m.CmdId);
                values.Add("MerCustId", m.MerCustId);
                values.Add("ChkValue", m.ChkValue);

                string url = Utils.GetChinapnrUrl();
                //同步发送form表单请求
                byte[] result = client.UploadValues(url, "POST", values);
                var retStr = Encoding.UTF8.GetString(result);

                ReQueryPayQuota reg = new ReQueryPayQuota();
                JavaScriptSerializer js = new JavaScriptSerializer();   //实例化一个能够序列化数据的类
                ReQueryPayQuota list = js.Deserialize<ReQueryPayQuota>(retStr);    //将json数据转化为对象类型并赋值给list

                string RespCode = list.RespCode;
                List<M_PayQuotaDetails> orderdetail = list.PayQuotaDetails;

                if (RespCode == "000")
                {
                    string sql = "";
                    for (int i = 0; i < orderdetail.Count; i++)
                    {
                        try
                        {
                            sql = "select  OpenBankId  from  hx_td_Bank where  OpenBankId='" + orderdetail[i].OpenBankId + "'";
                            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                            if (dt.Rows.Count > 0)
                            {
                                sql = " update hx_td_Bank set Isquick=1,SingleTransQuota='" + decimal.Parse(orderdetail[i].SingleTransQuota.ToString()) + "',CardDailyTransQuota='" + decimal.Parse(orderdetail[i].CardDailyTransQuota.ToString()) + "'  where OpenBankId='" + orderdetail[i].OpenBankId + "'";
                                DbHelperSQL.RunSql(sql);
                            }
                            else
                            {
                                sql = " insert into hx_td_Bank (BankName,OpenBankId,CardImage,Isquick,Isordinary,isGren,SingleTransQuota,CardDailyTransQuota,CardImageNew)values('','" + orderdetail[i].OpenBankId + "','',1,0,0,'" + decimal.Parse(orderdetail[i].SingleTransQuota.ToString()) + "','" + decimal.Parse(orderdetail[i].CardDailyTransQuota.ToString()) + "','')";
                                DbHelperSQL.RunSql(sql);
                            }
                        }
                        catch (Exception ex)
                        {
                            strt = StringAlert.Alert(orderdetail[i].OpenBankId + "数据同步失败!", "/Admin/Template/QueryPayQuota");
                            LogInfo.WriteLog("OpenBankId='" + orderdetail[i].OpenBankId + "';快捷卡数据同步失败：" + ex.Message);
                        }
                    }
                }
            }
            return Content(strt, "text/html");
        }
    }
}