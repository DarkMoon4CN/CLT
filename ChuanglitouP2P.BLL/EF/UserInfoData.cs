using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.Extensionses;
using ChuanglitouP2P.Model.chinapnr.QueryBalanceBg;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;
using System.Data;

namespace ChuanglitouP2P.BLL.EF
{

    /// <summary>
    /// 用户详细记录分析
    /// </summary>
    public class UserInfoData
    {
        chuangtouEntities ef = new chuangtouEntities();

        #region  获取资金明细记录列表
        public PagedList<hx_Capital_account_water> Capital_water(int id, string startdatetime, string enddatetime, int? pageIndex = 1, int page = 1, int PageSize = 5)
        {
            Expression<Func<hx_Capital_account_water, bool>> where = PredicateExtensionses.True<hx_Capital_account_water>();
            where = where.And(p => p.account_water_id > 0);
            where = where.And(p => p.membertable_registerid == id);

            DateTime sdatetime = new DateTime();
            DateTime edatetime = new DateTime();
            if (Utils.IsDate(startdatetime))
            {
                sdatetime = DateTime.Parse(startdatetime);
            }

            if (Utils.IsDate(enddatetime))
            {
                edatetime = DateTime.Parse(enddatetime);
            }

            if (sdatetime > DateTime.Parse("0001-01-01 00:00:00") && edatetime > DateTime.Parse("0001-01-01 00:00:00"))
            {
                where = where.And(p => ((DateTime)p.createtime).CompareTo(sdatetime) >= 0);
                DateTime dt2 = Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(p => ((DateTime)p.createtime).CompareTo(dt2) <= 0);
            }

            var list = ef.hx_Capital_account_water.Where(where).OrderByDescending(p => p.account_water_id).ToPagedList(pageIndex ?? 1, PageSize);

            return list;
        }
        #endregion


        #region 获取充值记录列表
        /// <summary>
        /// 获取充值记录列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public PagedList<V_Recharge_user_bank> Recharge(int id, string startdatetime, string enddatetime, int? pageIndex = 1, int page = 1, int PageSize = 5)
        {
            Expression<Func<V_Recharge_user_bank, bool>> where = PredicateExtensionses.True<V_Recharge_user_bank>();
            where = where.And(p => p.recharge_history_id > 0);
            where = where.And(p => p.membertable_registerid == id);

            DateTime sdatetime = new DateTime();
            DateTime edatetime = new DateTime();
            if (Utils.IsDate(startdatetime))
            {
                sdatetime = DateTime.Parse(startdatetime);
            }

            if (Utils.IsDate(enddatetime))
            {
                edatetime = DateTime.Parse(enddatetime);
            }

            if (sdatetime > DateTime.Parse("0001-01-01 00:00:00") && edatetime > DateTime.Parse("0001-01-01 00:00:00"))
            {
                where = where.And(p => ((DateTime)p.recharge_time).CompareTo(sdatetime) >= 0);
                DateTime dt2 = Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(p => ((DateTime)p.recharge_time).CompareTo(dt2) <= 0);
            }

            var list = ef.V_Recharge_user_bank.Where(where).OrderByDescending(p => p.recharge_history_id).ToPagedList(pageIndex ?? 1, PageSize);

            return list;
        }
        #endregion

        #region 获取连连充值记录
        public PagedList<V_td_LLpay_bank> LLRecharge(int id, string startdatetime, string enddatetime, int? pageIndex = 1, int page = 1, int PageSize = 5)
        {
            Expression<Func<V_td_LLpay_bank, bool>> where = PredicateExtensionses.True<V_td_LLpay_bank>();
            where = where.And(p => p.Rechargeid > 0);
            where = where.And(p => p.UsrId == id);

            DateTime sdatetime = new DateTime();
            DateTime edatetime = new DateTime();
            if (Utils.IsDate(startdatetime))
            {
                sdatetime = DateTime.Parse(startdatetime);
            }
            if (Utils.IsDate(enddatetime))
            {
                edatetime = DateTime.Parse(enddatetime);
            }


            var list = ef.V_td_LLpay_bank.Where(where).OrderByDescending(p => p.Rechargeid).ToPagedList(pageIndex ?? 1, PageSize);

            var listwheretime = list;

            if (sdatetime > DateTime.Parse("0001-01-01 00:00:00") && edatetime > DateTime.Parse("0001-01-01 00:00:00"))
            {
                //where = where.And(p => (p.ordertime.ToDateTime()).CompareTo(sdatetime) >= 0);
                //DateTime dt2 = Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59");
                //where = where.And(p => (Convert.ToDateTime(p.ordertime).CompareTo(dt2) <= 0));

                listwheretime.Clear();
                foreach (var item in list)
                {
                    var ordertime = Convert.ToDateTime(item.ordertime);
                    if (ordertime >= sdatetime && ordertime <= Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59"))
                    {
                        listwheretime.Add(item);
                    }

                }
            }
            listwheretime = listwheretime.ToPagedList(pageIndex ?? 1, PageSize);



            return listwheretime;
        }
        #endregion


        /// <summary>
        /// 用户提现列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public PagedList<hx_td_UserCash> UserCash(int id, string startdatetime, string enddatetime, int? pageIndex = 1, int page = 1, int pgaesize = 5)
        {
            Expression<Func<hx_td_UserCash, bool>> where = PredicateExtensionses.True<hx_td_UserCash>();
            where = where.And(p => p.registerid > 0);
            where = where.And(p => p.registerid == id);
            where = where.And(p => p.OpenBankId != null);


            DateTime sdatetime = new DateTime();
            DateTime edatetime = new DateTime();
            if (Utils.IsDate(startdatetime))
            {
                sdatetime = DateTime.Parse(startdatetime);

            }

            if (Utils.IsDate(enddatetime))
            {
                edatetime = DateTime.Parse(enddatetime);
            }

            if (sdatetime > DateTime.Parse("0001-01-01 00:00:00") && edatetime > DateTime.Parse("0001-01-01 00:00:00"))
            {
                where = where.And(p => ((DateTime)p.OrdIdTime).CompareTo(sdatetime) >= 0);
                DateTime dt2 = Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(p => ((DateTime)p.OrdIdTime).CompareTo(dt2) <= 0);
            }

            var list = ef.hx_td_UserCash.Where(where).OrderByDescending(p => p.UserCashId).ToPagedList(pageIndex ?? 1, pgaesize);

            return list;
        }

        #region 获取连连提现列表
        public PagedList<hx_td_LL_cash> UserLLCash(int id, string startdatetime, string enddatetime, int? pageIndex = 1, int page = 1, int pagesize = 5)
        {
            Expression<Func<hx_td_LL_cash, bool>> where = PredicateExtensionses.True<hx_td_LL_cash>();
            where = where.And(p => p.LLcashid > 0);
            where = where.And(p => p.Usrid == id);
            where = where.And(p => p.bank_code != null);


            DateTime sdatetime = new DateTime();
            DateTime edatetime = new DateTime();
            if (Utils.IsDate(startdatetime))
            {
                sdatetime = DateTime.Parse(startdatetime);

            }

            if (Utils.IsDate(enddatetime))
            {
                edatetime = DateTime.Parse(enddatetime);
            }

            if (sdatetime > DateTime.Parse("0001-01-01 00:00:00") && edatetime > DateTime.Parse("0001-01-01 00:00:00"))
            {
                where = where.And(p => ((DateTime)p.ordertime).CompareTo(sdatetime) >= 0);
                DateTime dt2 = Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(p => ((DateTime)p.ordertime).CompareTo(dt2) <= 0);
            }

            var list = ef.hx_td_LL_cash.Where(where).OrderByDescending(p => p.LLcashid).ToPagedList(pageIndex ?? 1, pagesize);

            return list;
        }
        #endregion


        #region 用户投资记录
        /// <summary>
        /// 用户投资记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startdatetime"></param>
        /// <param name="enddatetime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="page"></param>
        /// <param name="pgaesize"></param>
        /// <returns></returns>
        public PagedList<V_hx_Bid_records_borrowing_target> Bid_RecordsList(int id, string startdatetime, string enddatetime, int? pageIndex = 1, int page = 1, int pgaesize = 5)
        {
            Expression<Func<V_hx_Bid_records_borrowing_target, bool>> where = PredicateExtensionses.True<V_hx_Bid_records_borrowing_target>();
            where = where.And(p => p.registerid > 0);
            where = where.And(p => p.registerid == id);
            DateTime sdatetime = new DateTime();
            DateTime edatetime = new DateTime();
            if (Utils.IsDate(startdatetime))
            {
                sdatetime = DateTime.Parse(startdatetime);

            }

            if (Utils.IsDate(enddatetime))
            {
                edatetime = DateTime.Parse(enddatetime);
            }

            if (sdatetime > DateTime.Parse("0001-01-01 00:00:00") && edatetime > DateTime.Parse("0001-01-01 00:00:00"))
            {
                where = where.And(p => ((DateTime)p.invest_time).CompareTo(sdatetime) >= 0);
                DateTime dt2 = Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(p => ((DateTime)p.invest_time).CompareTo(dt2) <= 0);
            }

            var list = ef.V_hx_Bid_records_borrowing_target.Where(where).OrderByDescending(p => p.bid_records_id).ToPagedList(pageIndex ?? 1, pgaesize);

            return list;
        }


        #endregion


        #region 用户回款记录
        /// <summary>
        /// 用户回款记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startdatetime"></param>
        /// <param name="enddatetime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="page"></param>
        /// <param name="pgaesize"></param>
        /// <returns></returns>
        public PagedList<V_borrowing_Bid_records_income_statement> Bid_Records_income(int id, string startdatetime, string enddatetime, int? pageIndex = 1, int page = 1, int pgaesize = 5)
        {
            Expression<Func<V_borrowing_Bid_records_income_statement, bool>> where = PredicateExtensionses.True<V_borrowing_Bid_records_income_statement>();
            where = where.And(p => p.registerid > 0);
            where = where.And(p => p.registerid == id);
            DateTime sdatetime = new DateTime();
            DateTime edatetime = new DateTime();
            if (Utils.IsDate(startdatetime))
            {
                sdatetime = DateTime.Parse(startdatetime);

            }

            if (Utils.IsDate(enddatetime))
            {
                edatetime = DateTime.Parse(enddatetime);
            }

            if (sdatetime > DateTime.Parse("0001-01-01 00:00:00") && edatetime > DateTime.Parse("0001-01-01 00:00:00"))
            {
                where = where.And(p => ((DateTime)p.repayment_period).CompareTo(sdatetime) >= 0);
                DateTime dt2 = Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(p => ((DateTime)p.repayment_period).CompareTo(dt2) <= 0);
            }

            var list = ef.V_borrowing_Bid_records_income_statement.Where(where).OrderByDescending(p => p.bid_records_id).ToPagedList(pageIndex ?? 1, pgaesize);

            return list;
        }


        #endregion


        #region 活动奖
        /// <summary>
        /// 用户回款记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startdatetime"></param>
        /// <param name="enddatetime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="page"></param>
        /// <param name="pgaesize"></param>
        /// <returns></returns>
        public PagedList<hx_UserAct> Actcash(int id, string startdatetime, string enddatetime, int? acttype = 1, int? pageIndex = 1, int page = 1, int pgaesize = 5)
        {
            Expression<Func<hx_UserAct, bool>> where = PredicateExtensionses.True<hx_UserAct>();
            where = where.And(p => p.registerid == id);
            where = where.And(p => p.RewTypeID == acttype);
            DateTime sdatetime = new DateTime();
            DateTime edatetime = new DateTime();
            if (Utils.IsDate(startdatetime))
            {
                sdatetime = DateTime.Parse(startdatetime);

            }

            if (Utils.IsDate(enddatetime))
            {
                edatetime = DateTime.Parse(enddatetime);
            }

            if (sdatetime > DateTime.Parse("0001-01-01 00:00:00") && edatetime > DateTime.Parse("0001-01-01 00:00:00"))
            {
                where = where.And(p => ((DateTime)p.Createtime).CompareTo(sdatetime) >= 0);
                DateTime dt2 = Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(p => ((DateTime)p.Createtime).CompareTo(dt2) <= 0);
            }

            var list = ef.hx_UserAct.Where(where).OrderByDescending(p => p.UserAct).ToPagedList(pageIndex ?? 1, pgaesize);

            return list;
        }


        #endregion


        #region 邀请
        /// <summary>
        /// 邀请记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startdatetime"></param>
        /// <param name="enddatetime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="page"></param>
        /// <param name="pgaesize"></param>
        /// <returns></returns>
        public PagedList<V_YaoQinList> yaoqin(int id, string startdatetime, string enddatetime, int? acttype = 1, int? pageIndex = 1, int page = 1, int pgaesize = 5)
        {
            Expression<Func<V_YaoQinList, bool>> where = PredicateExtensionses.True<V_YaoQinList>();
            where = where.And(p => p.membertable_registerid == id);

            DateTime sdatetime = new DateTime();
            DateTime edatetime = new DateTime();
            if (Utils.IsDate(startdatetime))
            {
                sdatetime = DateTime.Parse(startdatetime);

            }

            if (Utils.IsDate(enddatetime))
            {
                edatetime = DateTime.Parse(enddatetime);
            }

            if (sdatetime > DateTime.Parse("0001-01-01 00:00:00") && edatetime > DateTime.Parse("0001-01-01 00:00:00"))
            {
                where = where.And(p => ((DateTime)p.Createtime).CompareTo(sdatetime) >= 0);
                DateTime dt2 = Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(p => ((DateTime)p.Createtime).CompareTo(dt2) <= 0);
            }

            var list = ef.V_YaoQinList.Where(where).OrderByDescending(p => p.Createtime).ToPagedList(pageIndex ?? 1, pgaesize);

            return list;
        }


        #endregion



        #region 用户登录来源
        /// <summary>
        /// 邀请记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startdatetime"></param>
        /// <param name="enddatetime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="page"></param>
        /// <param name="pgaesize"></param>
        /// <returns></returns>
        public PagedList<hx_td_usrlogininfo> usrlogin(int id, string startdatetime, string enddatetime, int? acttype = 1, int? pageIndex = 1, int page = 1, int pgaesize = 5)
        {
            Expression<Func<hx_td_usrlogininfo, bool>> where = PredicateExtensionses.True<hx_td_usrlogininfo>();
            where = where.And(p => p.registerid == id);

            DateTime sdatetime = new DateTime();
            DateTime edatetime = new DateTime();
            if (Utils.IsDate(startdatetime))
            {
                sdatetime = DateTime.Parse(startdatetime);

            }

            if (Utils.IsDate(enddatetime))
            {
                edatetime = DateTime.Parse(enddatetime);
            }

            if (sdatetime > DateTime.Parse("0001-01-01 00:00:00") && edatetime > DateTime.Parse("0001-01-01 00:00:00"))
            {
                where = where.And(p => ((DateTime)p.logintime).CompareTo(sdatetime) >= 0);
                DateTime dt2 = Convert.ToDateTime(edatetime.ToString("yyyy-MM-dd") + " 23:59:59");
                where = where.And(p => ((DateTime)p.logintime).CompareTo(dt2) <= 0);
            }

            var list = ef.hx_td_usrlogininfo.Where(where).OrderByDescending(p => p.loginid).ToPagedList(pageIndex ?? 1, pgaesize);

            return list;
        }


        #endregion


        #region 更新用户第三方余额
        /// <summary>
        /// 更新用户第三方余额
        /// </summary>
        /// <param name="usrid"></param>
        /// <returns></returns>
        public ReQueryBalanceBg Querybalance(int usrid)
        {
            M_QueryBalanceBg mbg = new M_QueryBalanceBg();
            ReQueryBalanceBg retloan = new ReQueryBalanceBg();
            hx_member_table mt = ef.hx_member_table.Where(p => p.registerid == usrid).FirstOrDefault();
            string UsrCustId = "";

            if (mt != null)
            {
                UsrCustId = mt.UsrCustId;
            }


            if (UsrCustId != null && UsrCustId != "")
            {


                if (UsrCustId.Length > 0)
                {
                    mbg.Version = "10";
                    mbg.CmdId = "QueryBalanceBg";
                    mbg.MerCustId = Utils.GetMerCustID();
                    mbg.UsrCustId = UsrCustId;

                    StringBuilder chkVal = new StringBuilder();
                    chkVal.Append(mbg.Version);
                    chkVal.Append(mbg.CmdId);
                    chkVal.Append(mbg.MerCustId);
                    chkVal.Append(mbg.UsrCustId);
                    string chkv = chkVal.ToString();
                    string log = "更新用户第三方余额";
                    log += "<br>加签chkv字符:" + chkv;

                    //私钥文件的位置(这里是放在了站点的根目录下)
                    string merKeyFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetMerPr();
                    //需要指定提交字符串的长度
                    int len = Encoding.UTF8.GetBytes(chkv).Length;
                    StringBuilder sbChkValue = new StringBuilder(256);
                    //加签
                    int str = DllInterop.SignMsg(Utils.GetMerId(), merKeyFile, chkv, len, sbChkValue);

                    LogInfo.WriteLog("加签字符:" + str.ToString());

                    mbg.ChkValue = sbChkValue.ToString();

                    log += "<br>提交信息：" + FastJSON.toJOSN(mbg);
                    log += "<br>ChkValue:" + mbg.ChkValue;


                    using (var client = new WebClient())
                    {
                        var values = new NameValueCollection();
                        values.Add("Version", mbg.Version);
                        values.Add("CmdId", mbg.CmdId);
                        values.Add("MerCustId", mbg.MerCustId);
                        values.Add("UsrCustId", mbg.UsrCustId);
                        values.Add("ChkValue", mbg.ChkValue);


                        string url = Utils.GetChinapnrUrl();
                        //同步发送form表单请求
                        byte[] result = client.UploadValues(url, "POST", values);
                        var retStr = Encoding.UTF8.GetString(result);
                        // Response.Write(retStr);

                        LogInfo.WriteLog(retStr);

                        ReQueryBalanceBg reg = new ReQueryBalanceBg();
                        retloan = (ReQueryBalanceBg)FastJSON.ToObject(retStr, reg);


                        StringBuilder builder = new StringBuilder();
                        builder.Append(retloan.CmdId);
                        builder.Append(retloan.RespCode);
                        //builder.Append(retloan.RespDesc);
                        builder.Append(retloan.MerCustId);
                        builder.Append(retloan.UsrCustId);
                        builder.Append(retloan.AvlBal);
                        builder.Append(retloan.AcctBal);
                        builder.Append(retloan.FrzBal);
                        //  builder.Append(retloan.ChkValue);

                        var msg = builder.ToString();

                        log += "<br>返回参数:" + msg;
                        //验签
                        string pgPubkFile = AppDomain.CurrentDomain.BaseDirectory + Utils.GetPgPubk();
                        int ret = DllInterop.VeriSignMsg(pgPubkFile, msg, msg.Length, retloan.ChkValue);

                        log += "<br>验签ret:" + ret.ToString();
                        if (ret == 0)
                        {



                            if (retloan.RespCode == "000")
                            {

                                B_usercenter bu = new B_usercenter();
                                bu.DataSync(retloan, UsrCustId, 1);


                                //  Response.Write("账户余额:  " + retloan.AcctBal + "<br>");

                                //  Response.Write("可用余额:  " + retloan.AvlBal + "<br>");

                                //   Response.Write("冻结余额:  " + retloan.FrzBal + "<br>");


                                //  AvlBal.Value = retloan.AvlBal;

                                //  FrzBal.Value = retloan.FrzBal;


                                //  string sql = "update  hx_member_table  set  available_balance=" + decimal.Parse(retloan.AvlBal) + " ,frozen_sum=" + decimal.Parse(retloan.FrzBal) + " where  UsrCustId='" + UsrCustId + "'";

                                // DbHelperSQL.RunSql(sql);


                            }
                            else
                            {

                                // Response.Write(HttpUtility.UrlDecode(retloan.RespDesc));
                            }
                        }
                    }
                    LogInfo.WriteLog(log);
                }
            }

            return retloan;
        }


        #endregion
    }
}
