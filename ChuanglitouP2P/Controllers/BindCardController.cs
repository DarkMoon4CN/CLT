using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.QueryCardInfo;
using ChuangLitouP2P.Models;
using EntityFramework.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ChuanglitouP2P.Controllers
{
    public class BindCardController : Controller
    {

        chuangtouEntities ef = new chuangtouEntities();
        // GET: BindCard
        public ActionResult Index()
        {
            int userid = Utils.checkloginsession();

            B_member_table b = new B_member_table();
            M_member_table pu = new M_member_table();
            pu = b.GetModel(userid);



            //判断用户是否开户
            if (string.IsNullOrEmpty(pu.UsrCustId))
            {

                string temstr = "/opening_account/Index/" + userid.ToString();
                return Redirect(temstr);
            }


            //if (pu.isbankcard == 0)
            //{
            checkbank(pu.UsrCustId);

            //}

            V_UsrBindCardBank Ucard = new V_UsrBindCardBank();

            Ucard = ef.V_UsrBindCardBank.Where(p => p.registerid == userid).OrderByDescending(p => p.defCard).Take(1).FirstOrDefault();


            List<V_UsrBindCardBank> listcard = ef.V_UsrBindCardBank.Where(c => c.registerid == userid).ToList();
            listcard = BusinessLogicHelper.LeftOne(listcard);
            ViewBag.listcard = listcard;
            return View(Ucard);
        }


        public ActionResult thirdparty_bindbank()
        {
            int userid = Utils.checkloginsession();
            string url = Utils.GetChinapnrUrl();

            B_member_table b = new B_member_table();
            M_member_table p = new M_member_table();
            p = b.GetModel(userid);
            string Version = "10";
            string CmdId = "UserBindCard";
            string MerCustId = Utils.GetMerCustID();
            string UsrCustId = p.UsrCustId;
            string BgRetUrl = Utils.GetRe_url("Thirdparty/Bgthirdpartybindbank");
            string MerPriv = Utils.Base64Encoder("chuanglitou");

            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(Version);
            chkVal.Append(CmdId);
            chkVal.Append(MerCustId);
            chkVal.Append(UsrCustId);
            chkVal.Append(BgRetUrl);
            chkVal.Append(MerPriv);

            string chkv = chkVal.ToString();

            LogInfo.WriteLog(chkv);

            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

            string ChkValue = sbChkValue.ToString();

            ViewBag.url = url;
            ViewBag.Version = Version;
            ViewBag.CmdId = CmdId;
            ViewBag.MerCustId = MerCustId;
            ViewBag.UsrCustId = UsrCustId;
            ViewBag.BgRetUrl = BgRetUrl;
            ViewBag.MerPriv = MerPriv;
            ViewBag.ChkValue = ChkValue;

            return View();
        }



        public void checkbank(string UsrCustId)
        {

            M_QueryCardInfo m = new M_QueryCardInfo();

            m.Version = "10";
            m.CmdId = "QueryCardInfo";
            m.MerCustId = Utils.GetMerCustID();
            m.UsrCustId = UsrCustId;
            StringBuilder chkVal = new StringBuilder();
            chkVal.Append(m.Version);
            chkVal.Append(m.CmdId);
            chkVal.Append(m.MerCustId);
            chkVal.Append(m.UsrCustId);

            string chkv = chkVal.ToString();


            //私钥文件的位置(这里是放在了站点的根目录下)
            string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
            //需要指定提交字符串的长度
            int len = Encoding.UTF8.GetBytes(chkv).Length;
            StringBuilder sbChkValue = new StringBuilder(256);
            //加签
            int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

            //  Response.Write("加签字符:" + str.ToString() + "<br>");

            m.ChkValue = sbChkValue.ToString();

            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values.Add("Version", m.Version);
                values.Add("CmdId", m.CmdId);
                values.Add("MerCustId", m.MerCustId);
                values.Add("UsrCustId", m.UsrCustId);
                values.Add("ChkValue", m.ChkValue);

                string url = Utils.GetChinapnrUrl();
                //同步发送form表单请求
                byte[] result = client.UploadValues(url, "POST", values);
                var retStr = Encoding.UTF8.GetString(result);
                // Response.Write(retStr);




                ReQueryCardInfo reg = new ReQueryCardInfo();

                JavaScriptSerializer js = new JavaScriptSerializer();   //实例化一个能够序列化数据的类
                ReQueryCardInfo list = js.Deserialize<ReQueryCardInfo>(retStr);    //将json数据转化为对象类型并赋值给list


                LogInfo.WriteLog("卡查询：" + retStr);

                string RespCode = list.RespCode;

                List<M_UsrCardInfolist> orderdetail = list.UsrCardInfolist;



                StringBuilder builder = new StringBuilder();
                builder.Append(list.CmdId);
                builder.Append(list.RespCode);
                builder.Append(list.MerCustId);
                builder.Append(list.UsrCustId);
                var msg = builder.ToString();
                string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
                int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, list.ChkValue);

                if (ret == 0)
                {
                    delCard(orderdetail, list);

                    if (list.RespCode == "000")
                    {
                        string sql = "";




                        for (int i = 0; i < orderdetail.Count; i++)
                        {

                            if (checkcard(orderdetail[i].CardId) <= 0)
                            {

                                int iy = 0;

                                if (orderdetail[i].IsDefault == "Y")
                                {

                                    iy = 1;
                                }

                                int bindcartype = 0;

                                if (orderdetail[i].ExpressFlag == "Y")
                                {
                                    bindcartype = 1;
                                }

                                //  orderdetail[i].
                                sql = "  update hx_member_table   set isbankcard=1  where  UsrCustId='" + orderdetail[i].UsrCustId + "' and  isbankcard=0 ";

                                DbHelperSQL.RunSql(sql);
                                sql = " insert into hx_UsrBindCardC (UsrCustId,OpenAcctId,OpenBankId,defCard,BindCardType)values('" + orderdetail[i].UsrCustId + "','" + orderdetail[i].CardId + "','" + orderdetail[i].BankId + "'," + iy + "," + bindcartype + ")";

                                // Response.Write(sql + "<br>");

                                DbHelperSQL.RunSql(sql);
                                // Response.Write(orderdetail[i].BankId + "  " + orderdetail[i].IsDefault + "  " + orderdetail[i].CardId + "  " + orderdetail[i].UsrCustId + "<br>");

                            }
                            else
                            {
                                //如果卡在

                                if (orderdetail[i].IsDefault == "Y")
                                {
                                    int bindcartype = 0;
                                    if (orderdetail[i].ExpressFlag == "Y")
                                    {
                                        bindcartype = 1;
                                    }


                                    sql = "update hx_UsrBindCardC  set defCard = 0 ,BindCardType=" + bindcartype + " where UsrCustId = '" + orderdetail[i].UsrCustId + "'";
                                    DbHelperSQL.RunSql(sql);
                                    sql = "update hx_UsrBindCardC  set defCard = 1 ,BindCardType=" + bindcartype + "   where UsrCustId = '" + orderdetail[i].UsrCustId + "' and OpenAcctId = '" + orderdetail[i].CardId + "'";
                                    DbHelperSQL.RunSql(sql);

                                }




                                // Response.Write("没有执行");
                            }
                        }

                    }
                }
            }



        }



        public int checkcard(string CardId)
        {
            string sql = "select  OpenAcctId  from  hx_UsrBindCardC where  OpenAcctId='" + CardId + "'";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            return dt.Rows.Count;


        }




        public void delCard(List<M_UsrCardInfolist> orderdetail, ReQueryCardInfo list)
        {
            ArrayList cc = new ArrayList();

            List<hx_UsrBindCardC> ubc = ef.hx_UsrBindCardC.Where(p => p.UsrCustId == list.UsrCustId).ToList();

            //foreach (var item in ubc)
            //{

            //    foreach (M_UsrCardInfolist it in orderdetail)
            //    {

            //        if (!item.OpenAcctId.Contains(it.CardId))
            //        {
            //            cc.Add(item.OpenAcctId);

            //            break;
            //        }
            //    }
            //}
            cc.AddRange(ubc.Where(c => !orderdetail.Select(d => d.CardId).Contains(c.OpenAcctId)).Select(c=>c.OpenAcctId).ToArray());


            foreach (var d in cc)
            {
                ef.hx_UsrBindCardC.Where(p => p.OpenAcctId == d.ToString()).Delete();

            }








        }

    }
  
}