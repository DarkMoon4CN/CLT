using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ChuanglitouP2P.Model;
using ChuanglitouP2P.Model.chinapnr.Repayment;
using ChuanglitouP2P.Model.chinapnr.Loans;
using ChuanglitouP2P.Model.chinapnr.Transfer;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model.chinapnr.CashAudit;
using ChuanglitouP2P.Model.chinapnr.UserRegister;
using ChuanglitouP2P.Model.VeryCodes.NetCreditAssistant.Model;
using ChuanglitouP2P.Bll.VeryCodes.NetCreditAssistant.BLL;
using System.Web;
using ChuanglitouP2P.Model.chinapnr.QueryBalanceBg;
using System.Threading;

namespace ChuanglitouP2P.BLL
{
    public class B_usercenter
    {
        public B_usercenter()
        {

        }




        /// <summary>
        /// 统计到帐收益
        /// </summary>
        /// <param name="userid">投资用户id</param>
        /// <returns></returns>
        public string Getaccountincome(int userid)
        {
            StringBuilder str = new StringBuilder();
            string sql = "SELECT COALESCE(sum(interestpayment),0) as interestpayment  from hx_income_statement where investor_registerid=" + userid.ToString() + " and payment_status>0";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            str.Append(Math.Round(decimal.Parse(dt.Rows[0]["interestpayment"].ToString()), 2).ToString());


            return str.ToString();
        }



        public string GetUserID(string UsrCustId)
        {
            string str = "";
            string sql = "SELECT registerid  from hx_member_table where UsrCustId='" + UsrCustId + "'";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            str = dt.Rows[0]["registerid"].ToString();
            return str;

        }


        /// <summary>
        /// 从投标记录返回标的id(Targetid)
        /// </summary>
        /// <param name="bid_records_id"></param>
        /// <returns></returns>
        public string Getbid_records_idTargetid(string bid_records_id)
        {
            string str = "";
            string sql = "SELECT targetid  from hx_Bid_records where bid_records_id='" + bid_records_id + "'";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            str = dt.Rows[0]["targetid"].ToString();
            return str;

        }



        /// <summary>
        /// 统计预期收益
        /// </summary>
        /// <param name="userid">投资用户id</param>
        /// <returns></returns>
        public string Getexpectedrevenue(int userid)
        {
            StringBuilder str = new StringBuilder();
            string sql = "SELECT COALESCE(sum(interestpayment),0) as interestpayment  from V_Bid_income_statement where investor_registerid=" + userid.ToString() + " and payment_status=0";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            str.Append(Math.Round(decimal.Parse(dt.Rows[0]["interestpayment"].ToString()), 2).ToString());
            return str.ToString();
        }


        /// <summary>
        /// 新动态获取累积赚取金额
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="datetime1"></param>
        /// <returns></returns>
        public string getAllDailyrevenue(int userid, DateTime datetime1)
        {

            decimal invest = 0.00M;
            string sql = "select targetid,bid_records_id, investment_amount,annual_interest_rate,payment_options,invest_time,repayment_period from V_hx_Bid_records_borrowing_target  where  investor_registerid=" + userid.ToString() + "   and   tender_state between 2 and 5  group by targetid,bid_records_id, investment_amount,annual_interest_rate,payment_options,invest_time,repayment_period";

            // string sql = "select targetid, investment_amount,annual_interest_rate,payment_options from V_hx_Bid_records_borrowing_target  where  investor_registerid=" + userid.ToString() + "   and  repayment_period>'" + datetime1.ToString() + "'";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                InvestmentParameters mp = new InvestmentParameters();
                mp.Amount = decimal.Parse(dt.Rows[i]["investment_amount"].ToString());
                mp.Circle = 1;
                mp.CircleType = 3;
                mp.NominalYearRate = double.Parse(dt.Rows[i]["annual_interest_rate"].ToString());
                mp.OverheadsRate = 0f;
                mp.RepaymentMode = int.Parse(dt.Rows[i]["payment_options"].ToString());
                mp.RewardRate = 0f;
                mp.IsThirtyDayMonth = false;
                mp.InvestDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                mp.Investmentenddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                mp.Payinterest = 1;
                mp.InvestObject = 1;
                List<InvestmentReceiveRecordInfo> records = Bll.VeryCodes.NetCreditAssistant.BLL.InvestCalculator.CalculateReceiveRecord(mp);
                decimal dayinvst = 0.0M;
                foreach (InvestmentReceiveRecordInfo pr in records)
                {
                    dayinvst = Math.Round(pr.Interest, 2);
                }



                DateTime repaydate = DateTime.Parse(dt.Rows[i]["repayment_period"].ToString());

                if (DateTime.Compare(repaydate, datetime1) <= 0)
                {

                    long allday = Utils.DateDiff("Day", DateTime.Parse(DateTime.Parse(dt.Rows[i]["invest_time"].ToString()).ToString("yyyy-MM-dd")), DateTime.Parse(repaydate.ToString("yyyy-MM-dd")));

                    int ady = int.Parse(allday.ToString());
                    if (ady <= 0)
                    {
                        ady = 1;
                    }

                    dayinvst = dayinvst * (ady + 1);

                }
                else
                {
                    long allday = Utils.DateDiff("Day", DateTime.Parse(DateTime.Parse(dt.Rows[i]["invest_time"].ToString()).ToString("yyyy-MM-dd")), DateTime.Parse(datetime1.ToString("yyyy-MM-dd")));

                    int ady = int.Parse(allday.ToString());
                    if (ady <= 0)
                    {
                        ady = 1;
                    }


                    if (DateTime.Parse(DateTime.Parse(dt.Rows[i]["invest_time"].ToString()).ToString("yyyy-MM-dd")) == DateTime.Parse(datetime1.ToString("yyyy-MM-dd")))
                    {
                        dayinvst = dayinvst * (ady);
                    }
                    else
                    {
                        dayinvst = dayinvst * (ady + 1);
                    }
                }
                invest = invest + dayinvst;


            }




            return Math.Round(invest, 2).ToString(); ;
        }


        /// <summary>
        /// 获取每天赚取
        /// </summary>
        /// <param name="userid">投资用户id</param>
        /// <param name="datetime1">当前时间</param>
        /// <returns></returns>
        public string getDailyrevenue(int userid, DateTime datetime1)
        {
            decimal invest = 0.00M;
            string sql = "select targetid,bid_records_id, investment_amount,annual_interest_rate,payment_options from V_hx_Bid_records_borrowing_target  where  investor_registerid=" + userid.ToString() + "  and  tender_state  between 2 and 4   and  repayment_period>'" + datetime1.ToString() + "' group by targetid,bid_records_id, investment_amount,annual_interest_rate,payment_options";

            // string sql = "select targetid, investment_amount,annual_interest_rate,payment_options from V_hx_Bid_records_borrowing_target  where  investor_registerid=" + userid.ToString() + "   and  repayment_period>'" + datetime1.ToString() + "'";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                InvestmentParameters mp = new InvestmentParameters();
                mp.Amount = decimal.Parse(dt.Rows[i]["investment_amount"].ToString());
                mp.Circle = 1;
                mp.CircleType = 3;
                mp.NominalYearRate = double.Parse(dt.Rows[i]["annual_interest_rate"].ToString());
                mp.OverheadsRate = 0f;
                mp.RepaymentMode = int.Parse(dt.Rows[i]["payment_options"].ToString());
                mp.RewardRate = 0f;
                mp.IsThirtyDayMonth = false;
                mp.InvestDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                mp.Investmentenddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                mp.Payinterest = 1;
                mp.InvestObject = 1;
                List<InvestmentReceiveRecordInfo> records = Bll.VeryCodes.NetCreditAssistant.BLL.InvestCalculator.CalculateReceiveRecord(mp);
                foreach (InvestmentReceiveRecordInfo pr in records)
                {
                    invest = invest + Math.Round(pr.Interest, 2);
                }

            }
            return Math.Round(invest, 2).ToString();
        }




        /// <summary>
        /// 累计赚取
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal GetCumulativeearned(int userid)
        {
            decimal dec = 0M;
            //获取已赚利息
            decimal yinvs = GetInterest(userid, 2);
            //decimal yinvs =0.0M;

            //分批次计算 未付利息按天计算
            //第一步获取该用户所有有效投资项目
            //第二步 分别计算出每个项目未付的利息(按日计息) 以当天为准计算
            //第三步 用已赚利息+ 每个项未付利息
            /*
           DataTable dt = GetTargetId(userid);
           for (int i = 0; i < dt.Rows.Count;i++ )
           {
               //for处理每个投资未付利息
               //通过用户投资项目id 计算返回未付利息

               dec = dec + GetUnpaidInterest(dt.Rows[i]["targetid"].ToString(), userid, dt.Rows[i]["bid_records_id"].ToString());


           }*/






            dec = dec + yinvs;

            /*
           if (dec == 0M)
           {
              dec = decimal.Parse(getAllDailyrevenue(userid, DateTime.Now));
           }*/

            return dec;
        }


        /// <summary>
        /// 通过项目id 和投资用户id 计算出未付利息
        /// </summary>
        /// <param name="TargetId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal GetUnpaidInterest(string TargetId, int userid, string bid_records_id)
        {
            decimal dec = 0M;
            //实现业务逻辑 获得上一次付息日，然后获取当天与下次付息日的时间，按天付息计算出未付利息

            string sql = "select top 1 targetid, investment_amount,annual_revenue,invest_time,value_date,payment_options from V_borrowing_Bid_records_income_statement where investor_registerid=" + userid.ToString() + " and payment_status=0  and targetid=" + TargetId + " and bid_records_id = " + bid_records_id + "  order by value_date asc";




            //string sql = "select top 1 targetid, investment_amount,annual_revenue,invest_time,value_date,payment_options from V_borrowing_Bid_records_income_statement_uc where investor_registerid=" + userid.ToString() + " and payment_status=0  and targetid=" + TargetId + " and bid_records_id = " + bid_records_id + "  order by value_date asc";


            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            if (dt.Rows.Count > 0)
            {
                DateTime invest_time = DateTime.Parse(dt.Rows[0]["invest_time"].ToString());

                DateTime value_date = DateTime.Parse(dt.Rows[0]["value_date"].ToString());


                DateTime nows = DateTime.Now;

                if (DateTime.Compare(invest_time, nows) <= 0)
                {
                    // Response.Write(Utils.DateDiff("Day", invest_time, nows).ToString());
                    //判断是否超过一个月  如果超过一个月则 按付息日记，不超一个月则按投资日计

                    long allday = Utils.DateDiff("Day", invest_time, nows);



                    InvestmentParameters mp = new InvestmentParameters();
                    mp.Amount = decimal.Parse(dt.Rows[0]["investment_amount"].ToString());
                    mp.Circle = 1;
                    mp.CircleType = 3;
                    mp.NominalYearRate = double.Parse(dt.Rows[0]["annual_revenue"].ToString());
                    mp.OverheadsRate = 0f;
                    mp.RepaymentMode = int.Parse(dt.Rows[0]["payment_options"].ToString());
                    mp.RewardRate = 0f;
                    mp.IsThirtyDayMonth = false;
                    /*
                      mp.InvestDate = DateTime.Parse(invest_time.ToString("yyyy-MM-dd"));
                      mp.Investmentenddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                     */

                    mp.InvestDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
                    mp.Investmentenddate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

                    mp.Payinterest = 1;
                    mp.InvestObject = 1;
                    List<InvestmentReceiveRecordInfo> records = Bll.VeryCodes.NetCreditAssistant.BLL.InvestCalculator.CalculateReceiveRecord(mp);
                    foreach (InvestmentReceiveRecordInfo pr in records)
                    {
                        dec = dec + decimal.Parse(RMB.GetWebConvertdisp(pr.Interest, 2, false));
                    }

                    int ady = int.Parse(allday.ToString());
                    if (ady <= 0)
                    {
                        ady = 1;
                    }

                    dec = dec * ady;



                    //Response.Write("fdfdsf:" + invest.ToString());



                    ////  int days = DateTime.DaysInMonth(invest_time.Year, invest_time.Month);


                    //  Response.Write(Utils.DateDiff("Day", invest_time, value_date).ToString());


                }



            }
            else
            {




            }


            return dec;
        }




        /// <summary>
        /// 获取用户所有的有效投资项目id
        /// select targetid from V_hx_Bid_records_borrowing_target where investor_registerid=57 and tender_state between 2 and 5  group by  targetid
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetTargetId(int userid)
        {
            string sql = "select targetid,bid_records_id from V_hx_Bid_records_borrowing_target where investor_registerid=" + userid.ToString() + " and tender_state between 2 and 5  group by  targetid,bid_records_id";

            return DbHelperSQL.GET_DataTable_List(sql);

        }





        /// <summary>
        /// 获取总资产
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal TetTotalCapital(int userid)
        {
            decimal dec = 0.00M;
            try
            {
                string sql = "select  available_balance  from  hx_member_table where registerid=" + userid.ToString();
                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                if (dt.Rows.Count > 0)
                {
                    dec = dec + decimal.Parse(dt.Rows[0]["available_balance"].ToString());

                }
                dec = dec + GetBonuses(userid);
            }
            catch (Exception ex)
            {

            }

            return dec;
        }


        /// <summary>
        /// 返回已赚利息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal GetInterest(int userid, int project_type_id = 0)
        {
            decimal dec = 0.00M;

            string sql = "";
            if (project_type_id == 0)
            {
                sql = "select  COALESCE(sum(haveinterest),0)  as haveinterest   from   V_hx_Bid_records_borrowing_target  where investor_registerid=" + userid.ToString() + " and  haveinterest>0 and tender_state between 2 and 5 ";
                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                dec = decimal.Parse(dt.Rows[0]["haveinterest"].ToString());

            }
            else
            {
                sql = "select  COALESCE(sum(haveinterest),0)  as haveinterest   from   V_hx_Bid_records_borrowing_target  where investor_registerid=" + userid.ToString() + " and  haveinterest>0 and tender_state between 2 and 5  and  project_type_id=" + project_type_id;
                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                dec = decimal.Parse(dt.Rows[0]["haveinterest"].ToString());

            }




            return dec;

        }


        /// <summary>
        /// 获取已经生效的奖励
        /// </summary>
        /// <returns></returns>
        public decimal GetEffectivereward(int userid)
        {

            decimal dec = 0.00M;

            string sql = "select COALESCE(sum(amount_of_reward),0) as amount_of_reward  from bonus_account where membertable_registerid=" + userid.ToString() + " and reward_state=1 and reward_state=3 ";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            dec = decimal.Parse(dt.Rows[0]["amount_of_reward"].ToString());


            return dec;
        }


        /// <summary>
        /// 获取可用奖励
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal GetBonuses(int userid)
        {
            decimal dec = 0.00M;
            //注需要过期时间处理         
            string sql = "select COALESCE(sum(Amt),0) as amount_of_reward  from hx_UserAct where UseState=0  and RewTypeID=2  and  registerid=" + userid.ToString() + " ";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            dec = decimal.Parse(dt.Rows[0]["amount_of_reward"].ToString());
            return dec;
        }       /// <summary>
                /// 获取可用奖励
                /// </summary>
                /// <param name="userid"></param>
                /// <returns></returns>
        public decimal GetBonuses1(int userid)
        {
            decimal dec = 0.00M;
            //注需要过期时间处理         
            string sql = "select COALESCE(sum(amount_of_reward),0) as amount_of_reward  from bonus_account where  reward_state=0 and  membertable_registerid=" + userid.ToString() + " ";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            dec = decimal.Parse(dt.Rows[0]["amount_of_reward"].ToString());
            return dec;
        }


        /// <summary>
        /// 获取已过期的奖励
        /// </summary>
        /// <returns></returns>
        public decimal GetNotEffectivereward(int userid)
        {

            decimal dec = 0.00M;
            string sql = "select COALESCE(sum(Amt),0) as amount_of_reward  from hx_UserAct where registerid=" + userid.ToString() + " and UseState=2 and (RewTypeID=1 or RewTypeID=2)";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            dec = decimal.Parse(dt.Rows[0]["amount_of_reward"].ToString());


            return dec;
        }


        /// <summary>
        /// 待收本金
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal GetPrincipal(int userid)
        {
            decimal dec = 0.00M;
            string sql = "select  COALESCE(sum(investment_amount),0)  as investment_amount from hx_Bid_records where investor_registerid=" + userid.ToString() + " and payment_status=0 and ordstate=1 and IsLoans=1";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            dec = decimal.Parse(dt.Rows[0]["investment_amount"].ToString());


            return dec;
        }





        #region 获取用户本息
        /// <summary>
        ///  获取用户本息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal Getbeixi(int userid)
        {
            decimal dec = 0.00M;

            string sql = "";

            sql = "select  COALESCE(sum(repayment_amount),0)  as withoutinterest   from   V_borrowing_Bid_records_income_statement_uc  where investor_registerid=" + userid.ToString() + "   and payment_status=0 and ordstate=1";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            dec = decimal.Parse(dt.Rows[0]["withoutinterest"].ToString());



            return dec;
        }
        #endregion


        // select COALESCE(sum(repayment_amount),0)  as withoutinterest from   V_borrowing_Bid_records_income_statement_uc where investor_registerid=307   and payment_status = 0 and ordstate = 1


        /// <summary>
        /// 待收利息总额
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal Getcollecttotalamountinterest(int userid, int project_type_id = 0)
        {
            decimal dec = 0.00M;

            string sql = "";
            if (project_type_id == 0)
            {
                sql = "select  COALESCE(sum(withoutinterest),0)  as withoutinterest   from   V_hx_Bid_records_borrowing_target  where investor_registerid=" + userid.ToString() + "  and tender_state between 2 and 5 and payment_status=0 and ordstate=1";
                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                dec = decimal.Parse(dt.Rows[0]["withoutinterest"].ToString());
            }
            else
            {
                sql = "select  COALESCE(sum(withoutinterest),0)  as withoutinterest   from   V_hx_Bid_records_borrowing_target  where investor_registerid=" + userid.ToString() + "  and tender_state between 2 and 5 and payment_status=0 and ordstate=1 and project_type_id=" + project_type_id;
                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                dec = decimal.Parse(dt.Rows[0]["withoutinterest"].ToString());
            }

            return dec;
        }


        //投资总金额 
        public decimal GetTotalinvestment(int userid, int project_type_id = 0)
        {
            decimal dec = 0.00M;
            string sql = "";
            if (project_type_id == 0)
            {
                sql = "select  COALESCE(sum(investment_amount),0)  as investment_amount from V_hx_Bid_records_borrowing_target where investor_registerid=" + userid.ToString() + " and tender_state between 2 and 5";
                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                dec = decimal.Parse(dt.Rows[0]["investment_amount"].ToString());
            }
            else
            {
                sql = "select  COALESCE(sum(investment_amount),0)  as investment_amount from V_hx_Bid_records_borrowing_target where investor_registerid=" + userid.ToString() + " and tender_state between 2 and 5 and project_type_id=" + project_type_id;
                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                dec = decimal.Parse(dt.Rows[0]["investment_amount"].ToString());
            }


            return dec;
        }


        #region 投资总数量
        //投资总数量
        public decimal GetInvesTotal(int userid, int project_type_id = 0)
        {
            decimal dec = 0.00M;
            string sql = "";
            if (project_type_id == 0)
            {
                sql = "select  COALESCE(count(investor_registerid),0)  as investment_amount from V_hx_Bid_records_borrowing_target where investor_registerid=" + userid.ToString() + " and tender_state between 2 and 5";
                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                dec = decimal.Parse(dt.Rows[0]["investment_amount"].ToString());
            }
            else
            {
                sql = "select  COALESCE(count(investor_registerid),0)  as investment_amount from V_hx_Bid_records_borrowing_target where investor_registerid=" + userid.ToString() + " and tender_state between 2 and 5 and project_type_id=" + project_type_id;
                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                dec = decimal.Parse(dt.Rows[0]["investment_amount"].ToString());
            }


            return dec;
        }
        #endregion


        #region 获取用投资，最后一笔回款时间
        /// <summary>
        /// 获取用投资，最后一笔回款时间
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="project_type_id"></param>
        /// <returns></returns>
        public string GetValue_date(int userid, int project_type_id = 0)
        {
            string str = "";
            string sql = "select top 1  value_date from hx_income_statement where investor_registerid=" + userid.ToString() + "  and  payment_status=1  order by  value_date desc";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                str = dt.Rows[0]["value_date"].ToString();

            }
            return str;
        }
        #endregion


        /// <summary>
        /// 提现总金额
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal GetCashamount(int userid)
        {
            decimal dec = 0.00M;
            string sql = "select  COALESCE(sum(TransAmt),0)  as TransAmt from hx_td_UserCash where registerid=" + userid.ToString() + " and OrdIdState=3";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            dec = decimal.Parse(dt.Rows[0]["TransAmt"].ToString());

            return dec;
        }


        /// <summary>
        /// 充值总额
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal GetRechargeamount(int userid)
        {
            decimal dec = 0.00M;

            string sql = "select  COALESCE(sum(recharge_amount),0)  as recharge_amount from hx_Recharge_history where membertable_registerid=" + userid.ToString() + " and recharge_condition=1";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            dec = decimal.Parse(dt.Rows[0]["recharge_amount"].ToString());



            return dec;
        }




        /// <summary>
        /// 还款成功对投资人和借款人帐户等更新操作
        /// </summary>
        /// <param name="re">支付返回对象</param>
        /// <param name="baw">借款人流水对象  当平台代偿时为null 担保人代偿时为担保客户号</param>
        /// <param name="iaw">投资人流水对像</param>
        /// <returns></returns>
        public int Repayment_Successfully(ReRepayment re, M_Capital_account_water baw, M_Capital_account_water iaw, bool lastrepamt, decimal investment_amountlast, decimal Interest)
        {
            Utils.SetSYSDateTimeFormat();
            int i = 0;
            List<string> SQLStringList = new List<string>();
            StringBuilder strSql = new StringBuilder();

            // re.MerPriv = Utils.Base64Decoder(re.MerPriv);
            //1 第投资人计划是否有过更新
            LogInfo.WriteLog("MerPriv:" + HttpUtility.UrlDecode(re.MerPriv));
            DateTime dtime = DateTime.Parse(re.MerPriv);
            string sql = "SELECT  top 1  income_statement_id , targetid, bid_records_id,borrower_registerid,current_investment_period from hx_income_statement where payment_status=0  and InCustId='" + re.InCustId + "' and BidOrderid=" + re.SubOrdId + " and CONVERT(varchar(10), interest_payment_date, 23)=CONVERT(varchar(10), '" + dtime.ToString("yyyy-MM-dd") + "', 23)  order by interest_payment_date asc ";
            LogInfo.WriteLog("还款时判断sql1:" + sql);
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                //更新还款计划原态为还款状态
                // strSql.Append("update hx_income_statement set repayment_period='" + re.OrdDate + "',payment_status=1,orderid='" + re.OrdId + "'  where payment_status=0 and OutCustId='" + re.OutCustId + "' and InCustId='" + re.InCustId + "' and bid_records_id=" + re.SubOrdId + " and CONVERT(varchar(10), interest_payment_date, 23)=CONVERT(varchar(10), '" + dtime.ToString("yyyy-MM-dd") + "', 23)  ");

                DateTime OrdDate = DateTime.ParseExact(re.OrdDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                sql = "update hx_income_statement set repayment_period='" + OrdDate.ToString("yyyy-MM-dd") + "',payment_status=1,orderid='" + re.OrdId + "',BorrFees=" + re.Fee + " ,OutCustId='" + re.OutCustId + "' where payment_status=0 and  income_statement_id=" + dt.Rows[0]["income_statement_id"].ToString() + "";
                LogInfo.WriteLog("sql2:" + sql);
                strSql.Append(sql);
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();

                //更新投资人资金
                ///investment_amountlast  这个值传进来的是本期的利息
                sql = "update hx_member_table set account_total_assets=account_total_assets+" + Interest.ToString() + ",available_balance=available_balance+" + re.TransAmt.ToString() + "  where  UsrCustId='" + re.InCustId + "' ";
                LogInfo.WriteLog("sql______investment_amount:" + sql);
                strSql.Append(sql);
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();
                #region MyRegion 这个位置是处理本息还款出现在用户表里多加一个投资金额 --- 这样处理是有问题的，数据库已经做了处理 加了本息和本金字段，已经不用这个了，直接用本息 和本金
                /*

                if (lastrepamt == true)
                {
                    decimal lamt = decimal.Parse(re.TransAmt) - investment_amountlast;
                    sql = "update hx_member_table set account_total_assets=account_total_assets+" + lamt.ToString() + ",available_balance=available_balance+" + re.TransAmt + "  where  UsrCustId='" + re.InCustId + "' ";
                    LogInfo.WriteLog("最后一期investment_amount:" + sql);
                    strSql.Append(sql);
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();

                }
                else
                {

                    sql = "update hx_member_table set account_total_assets=account_total_assets+" + re.TransAmt + ",available_balance=available_balance+" + re.TransAmt + "  where  UsrCustId='" + re.InCustId + "' ";
                    LogInfo.WriteLog("sql3:" + sql);
                    strSql.Append(sql);
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();

                }
               */
                #endregion
                if (baw != null)
                {
                    //更新借款人资金
                    sql = "update hx_member_table set account_total_assets=account_total_assets-" + re.TransAmt + ",available_balance=available_balance-" + re.TransAmt + "  where  UsrCustId='" + re.OutCustId + "' ";
                    LogInfo.WriteLog("sql4:" + sql);
                    strSql.Append(sql);
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();

                    //借款人还款流水记录
                    strSql.Append("insert into hx_Capital_account_water(");
                    strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                    strSql.Append(" values (");
                    strSql.Append("" + baw.membertable_registerid + "," + baw.income + "," + baw.expenditure + ",'" + baw.time_of_occurrence + "'," + baw.account_balance + "," + baw.types_Finance + ",'" + baw.createtime + "'," + baw.keyid + ",'" + baw.remarks + "')");
                    LogInfo.WriteLog("借款人还款流水记录:" + strSql.ToString());
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();

                    decimal fees = decimal.Parse(re.Fee);
                    //借款人服务费流水记录
                    if (fees > 0)
                    {
                        baw.income = 0.00M;
                        baw.expenditure = fees;
                        B_usercenter o = new B_usercenter();
                        decimal di = o.GetUsridAvailable_balance(baw.membertable_registerid);
                        baw.account_balance = di - decimal.Parse(re.TransAmt);
                        baw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.手续费.ToString());
                        strSql.Append("insert into hx_Capital_account_water(");
                        strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                        strSql.Append(" values (");
                        strSql.Append("" + baw.membertable_registerid + "," + baw.income + "," + baw.expenditure + ",'" + baw.time_of_occurrence + "'," + baw.account_balance + "," + baw.types_Finance + ",'" + baw.createtime + "'," + baw.keyid + ",'" + baw.remarks + "')");
                        LogInfo.WriteLog("借款人服务费流水记录:" + strSql.ToString());
                        SQLStringList.Add(strSql.ToString());
                        strSql.Clear();
                    }
                }

                ////更新借款人还款计划表当前期数还款数据  单个还款不代表整个借款计划
                //sql = "update hx_repayment_plan  set repayment_state=1   where targetid=" + dt.Rows[0]["targetid"].ToString() + "  and borrower_registerid=" + dt.Rows[0]["borrower_registerid"].ToString() + "  and CONVERT(varchar(10), repayment_period, 23)=CONVERT(varchar(10), '" + dtime.ToString("yyyy-MM-dd") + "', 23)";
                //LogInfo.WriteLog("sql5:" + sql);
                //strSql.Append(sql);
                //SQLStringList.Add(strSql.ToString());
                //strSql.Clear();

                // 更新标的表的，还款金额和最近还款时间还款期数
                sql = "update hx_borrowing_target set  H_Repayment_Amt=H_Repayment_Amt+" + re.TransAmt + ",repaymentperiods=" + dt.Rows[0]["current_investment_period"].ToString() + ",Repay_Time='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'  where targetid=" + dt.Rows[0]["targetid"].ToString() + "";
                LogInfo.WriteLog("sql6:" + sql);
                strSql.Append(sql);
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();

                #region MyRegion  更新投资记录 更新投标记录应收利息
                sql = "select  investment_amount  from   hx_Bid_records  where bid_records_id=" + dt.Rows[0]["bid_records_id"].ToString() + "";
                DataTable dts = DbHelperSQL.GET_DataTable_List(sql);
                decimal investment_amount = 0M;
                if (dts.Rows.Count > 0)
                {
                    investment_amount = decimal.Parse(dts.Rows[0]["investment_amount"].ToString());
                }
                decimal trAtm = decimal.Parse(re.TransAmt);
                if (trAtm >= investment_amount)
                {
                    decimal dc = trAtm - investment_amount;
                    sql = "update hx_Bid_records set  repayment_amount=" + re.TransAmt + ",haveinterest=haveinterest+" + dc.ToString() + ",withoutinterest=withoutinterest-" + dc.ToString() + "  where bid_records_id=" + dt.Rows[0]["bid_records_id"].ToString() + "";
                }
                else
                {
                    sql = "update hx_Bid_records set repayment_amount=" + re.TransAmt + ",haveinterest=haveinterest+" + re.TransAmt + ",withoutinterest=withoutinterest-" + re.TransAmt + "  where bid_records_id=" + dt.Rows[0]["bid_records_id"].ToString() + "";
                }
                LogInfo.WriteLog("sql6:" + sql);
                strSql.Append(sql);
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();
                #endregion                

                //投资人入账记录
                strSql.Append("insert into hx_Capital_account_water(");
                strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                strSql.Append(" values (");
                strSql.Append("" + iaw.membertable_registerid + "," + iaw.income + "," + iaw.expenditure + ",'" + iaw.time_of_occurrence + "'," + iaw.account_balance + "," + iaw.types_Finance + ",'" + iaw.createtime + "'," + iaw.keyid + ",'" + iaw.remarks + "')");
                LogInfo.WriteLog("投资人入账记录:" + strSql.ToString());
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();
            }
            i = DbHelperSQL.ExecuteSqlTran(SQLStringList);
            return i;
        }




        /// <summary>
        /// 放款成功后进行对借款人和投资的人业务操作
        /// </summary>
        /// <param name="RL">放款成功返回对象</param>
        /// <param name="baw">借款人流水对象</param>
        /// <param name="iaw">投资人流水对象</param>
        /// <returns></returns>
        public int Loan_Successfully(ReLoans RL, M_Capital_account_water baw, M_Capital_account_water iaw, string bid_records_id, decimal Amt = 0.0M)
        {
            Utils.SetSYSDateTimeFormat();

            int i = 0;
            List<string> SQLStringList = new List<string>();
            StringBuilder strSql = new StringBuilder();

            B_usercenter BUC = new B_usercenter();

            //1 第判断放款记录是否存在，存在不进行写入，不存在写入操作

            string sql = "select * from hx_td_Loan_records where bid_orderid='" + RL.SubOrdId + "' and  LoanOrdId ='" + RL.OrdId + "'";
            LogInfo.WriteLog("第判断放款记录是否存在，存在不进行写入，不存在写入操作" + sql);

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count <= 0)
            {
                string targetid = Getbid_records_idTargetid(bid_records_id);

                //更新投标记录状态
                sql = "update  hx_Bid_records set IsLoans=1  where  bid_records_id=" + bid_records_id + " and  targetid=" + targetid + " and  ordstate=1 and IsLoans=0";
                LogInfo.WriteLog("更新投标记录状态 " + sql);
                strSql.Append(sql);
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();


                //写入放款记录
                DateTime dtime = DateTime.ParseExact(RL.OrdDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime SubOrdDate = DateTime.ParseExact(RL.SubOrdDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

                sql = "insert into hx_td_Loan_records (targetid,bid_records_id,InCustId,OutCustId,LoanAMT,LoanOrdId,LoanDate,Free,SubOrdid,SubOrdDate,unFreezeOrdId,FreezeTrxId,bid_orderid)values('" + targetid + "'," + bid_records_id + ",'" + RL.InCustId + "','" + RL.OutCustId + "'," + RL.TransAmt + ",'" + RL.OrdId + "','" + dtime.ToString() + "'," + RL.Fee + "," + RL.SubOrdId + ",'" + SubOrdDate.ToString() + "','" + RL.UnFreezeOrdId + "','" + RL.FreezeTrxId + "'," + RL.SubOrdId + ")";

                LogInfo.WriteLog("写入放款记录 " + sql);
                strSql.Append(sql);
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();

                //2 向借出人(投资人) 进行冻结和可用余额进行列新操作

                // decimal decfree = decimal.Parse(RL.TransAmt) - decimal.Parse(RL.Fee);


                decimal lastamt = decimal.Parse(RL.TransAmt) - Amt;


                if (RL.FeeObjFlag == "I") //向入款人(就是借款人收取) 需要扣除手续费
                {


                    //3 投资人资金记录变化明细记录写入
                    sql = "update hx_member_table set frozen_sum=frozen_sum-" + lastamt + " where  UsrCustId='" + RL.OutCustId + "' ";
                    LogInfo.WriteLog("投资人资金记录变化明细记录写入 " + sql);
                    strSql.Append(sql);
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();

                    //4向借入人(借款人) 进行可用余额进行更新操作

                    sql = "update hx_member_table set account_total_assets=account_total_assets+" + RL.TransAmt + ",available_balance=available_balance+" + RL.TransAmt + "  where  UsrCustId='" + RL.InCustId + "' ";
                    LogInfo.WriteLog("向借入人(借款人) 进行可用余额进行更新操作 " + sql);
                    strSql.Append(sql);
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();

                }
                else if (RL.FeeObjFlag == "O") //向出款人(投资人)收取手续费
                {
                    //3 投资人资金记录变化明细记录写入
                    sql = "update hx_member_table set frozen_sum=frozen_sum-" + lastamt + " where  UsrCustId='" + RL.OutCustId + "' ";

                    LogInfo.WriteLog("投资人资金记录变化明细记录写入 " + sql);
                    strSql.Append(sql);
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();

                    //4向借入人(借款人) 进行可用余额进行更新操作

                    sql = "update hx_member_table set account_total_assets=account_total_assets+" + RL.TransAmt + ",available_balance=available_balance+" + RL.TransAmt + "  where  UsrCustId='" + RL.InCustId + "' ";
                    LogInfo.WriteLog("向借入人(借款人) 进行可用余额进行更新操作 " + sql);
                    strSql.Append(sql);
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();


                }




                //5 借款人资金记录明细记录写入 方法外层操作成功后再写入,由于汲及到一些其他的参数

                //借款人还款流水记录

                decimal totalaccount_balance = baw.account_balance + decimal.Parse(RL.TransAmt);

                strSql.Append("insert into hx_Capital_account_water(");
                strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                strSql.Append(" values (");
                strSql.Append("" + baw.membertable_registerid + "," + baw.income + "," + baw.expenditure + ",'" + baw.time_of_occurrence + "'," + totalaccount_balance + "," + baw.types_Finance + ",'" + baw.createtime + "'," + baw.keyid + ",'" + baw.remarks + "')");
                LogInfo.WriteLog("借款人放款流水记录:" + strSql.ToString());
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();

                //借款人服务费流水记录

                decimal fees = decimal.Parse(RL.Fee);

                if (fees > 0)
                {
                    baw.income = 0.00M;
                    baw.expenditure = fees;
                    // B_usercenter o = new B_usercenter();
                    //decimal di = o.GetUsridAvailable_balance(baw.membertable_registerid);


                    //5向借入人(借款人) 扣除手续费可用余额进行更新操作

                    baw.account_balance = totalaccount_balance - fees;


                    sql = "update hx_member_table set  available_balance=" + baw.account_balance + "  where  UsrCustId='" + RL.InCustId + "' ";
                    LogInfo.WriteLog("向借入人(借款人) 扣除手续费可用余额进行更新操作 " + sql);
                    strSql.Append(sql);
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();



                    baw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.服务费.ToString());

                    strSql.Append("insert into hx_Capital_account_water(");
                    strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                    strSql.Append(" values (");
                    strSql.Append("" + baw.membertable_registerid + "," + baw.income + "," + baw.expenditure + ",'" + baw.time_of_occurrence + "'," + baw.account_balance + "," + baw.types_Finance + ",'" + baw.createtime + "'," + baw.keyid + ",'" + baw.remarks + "')");

                    LogInfo.WriteLog("借款人放款服务费流水记录:" + strSql.ToString());
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();
                }
                //投资人入账记录

                /*  //由于投资里写入了流水，这里再写入就造成了重复
                strSql.Append("insert into hx_Capital_account_water(");
                strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                strSql.Append(" values (");
                strSql.Append("" + iaw.membertable_registerid + "," + iaw.income + "," + iaw.expenditure + ",'" + iaw.time_of_occurrence + "'," + iaw.account_balance + "," + iaw.types_Finance + ",'" + iaw.createtime + "'," + iaw.keyid + ",'" + iaw.remarks + "')");

                LogInfo.WriteLog("投资人入账记录:" + strSql.ToString());
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();
                */

            }

            i = DbHelperSQL.ExecuteSqlTran(SQLStringList);

            return i;
        }


        /// <summary>
        /// 解并订单时更新 删除订单，还原状历,恢复标的金额，更新用户冻结数，和还原可用余额
        /// 王雪松  2016-09-26  注释掉该方法，如果需要该方法逻辑，请先验证逻辑是否正确，更改用户余额时，可能需要添加资金流水 hx_Capital_account_water
        /// </summary>
        /// <param name="bid_records_id"></param>
        /// <param name="FreezeTrxId"></param>
        /// <param name="investment_amount"></param>
        /// <param name="registerid"></param>
        /// <returns></returns>
        //public int UUFZ(string bid_records_id, string FreezeTrxId, decimal investment_amount, string registerid)
        //{
        //    int i = 0;
        //    List<string> SQLStringList = new List<string>();
        //    StringBuilder strSql = new StringBuilder();
        //    string sql = "";


        //    //1更新还原金额

        //    sql = "  update  hx_member_table  set available_balance=available_balance+" + investment_amount + ",frozen_sum=frozen_sum-" + investment_amount + " where  registerid=" + registerid;

        //    LogInfo.WriteLog("解冻更新还原金额:" + sql);
        //    strSql.Append(sql);
        //    SQLStringList.Add(strSql.ToString());
        //    strSql.Clear();



        //    //2更新奖励

        //    sql = "update  bonus_account  set reward_state=0, proid=null   where  proid=" + bid_records_id + " and  reward_state=3";
        //    strSql.Append(sql);

        //    LogInfo.WriteLog("解冻更新奖励:" + sql);
        //    SQLStringList.Add(strSql.ToString());
        //    strSql.Clear();

        //    //3删除原有订单
        //    sql = "delete hx_Bid_records where bid_records_id =" + bid_records_id + "";
        //    strSql.Append(sql);
        //    LogInfo.WriteLog("解冻删除原有订单:" + sql);
        //    SQLStringList.Add(strSql.ToString());
        //    strSql.Clear();



        //    return i;
        //}


        /// <summary>
        /// 事务处理更新奖励
        /// </summary>
        /// <param name="re"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public int UpateAwa(ReTransfer re)
        {
            int i = 0;
            List<string> SQLStringList = new List<string>();
            StringBuilder strSql = new StringBuilder();
            string sql = "";

            sql = "SELECT OrdId FROM hx_CashAwards where OrdIdstate=1 and OrdId='" + re.OrdId + "' and  proid =" + re.MerPriv;

            LogInfo.WriteLog("查询奖励奖" + sql);
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            if (dt.Rows.Count > 0)
            {
                sql = "update hx_CashAwards  set  OrdIdstate=3  where OrdIdstate=1 and OrdId='" + re.OrdId + "' and  proid =" + re.MerPriv;
                //更新冻结订单
                strSql.Append(sql);
                LogInfo.WriteLog("更新奖励奖态" + sql);
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();

                sql = "update hx_member_table  set account_total_assets=account_total_assets+" + re.TransAmt + ",available_balance=available_balance+" + re.TransAmt + " where UsrCustId='" + re.InCustId + "'";

                strSql.Append(sql);
                LogInfo.WriteLog("更新奖励的客户账户金额" + sql);
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();



                i = DbHelperSQL.ExecuteSqlTran(SQLStringList);


            }



            //sql = "update hx_CashAwards  set  OrdIdstate=3  where OrdIdstate=1 and OrdId=" + retloan.OrdId + "




            return i;
        }




        /// <summary>
        /// 交易时对冻结资金处理
        /// </summary>
        /// <param name="UsrCustId"></param>
        /// <param name="FrozenidNo"></param>
        /// <param name="TransAmt"></param>
        /// <returns></returns>
        public int ReInvest_success(string UsrCustId, string FrozenidNo, string TransAmt, string FreezeTrxId, string ordid, string MerPriv)
        {
            int i = 0;

            List<string> SQLStringList = new List<string>();
            StringBuilder strSql = new StringBuilder();

            DataTable dt = GetLonansVocherAmtByOrdId(ordid);

            decimal VocherAmt = 0.00M;
            string bid = "";



            if (dt.Rows.Count > 0)
            {
                VocherAmt = decimal.Parse(dt.Rows[0]["amount_of_reward"].ToString());
                bid = dt.Rows[0]["proid"].ToString();
            }
            // decimal tempvocherAmt = decimal.Parse(TransAmt) - VocherAmt;

            decimal tempvocherAmt = 0.00M;

            string sql = "SELECT Frozenid,FrozenidAmount From hx_td_frozen  where FrozenState=0  and FrozenidNo='" + FrozenidNo + "' and UsrCustId='" + UsrCustId + "'  ";


            DataTable CHKF = DbHelperSQL.GET_DataTable_List(sql);
            if (CHKF.Rows.Count > 0)
            {
                tempvocherAmt = CHKF.Rows[0]["FrozenidAmount"].ToDecimal();//decimal.Parse(CHKF.Rows[0]["FrozenidAmount"].ToString());

                sql = "update hx_td_frozen set FrozenState=1,FreezeTrxId='" + FreezeTrxId + "'  where  FrozenidNo='" + FrozenidNo + "' and UsrCustId='" + UsrCustId + "'";

                //更新冻结订单
                strSql.Append(sql);

                LogInfo.WriteLog("更新冻结订单sql" + sql);
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();

                //更新用户主表 余额-冻结金额    冻结总计=冻结总计+冻结金额
                // strSql.Append("update hx_member_table set available_balance=available_balance-" + tempvocherAmt + ",collect_total_amount=collect_total_amount+" + TransAmt + " where  UsrCustId='" + UsrCustId + "' ");

                sql = "update hx_member_table set  InvestNum=InvestNum+1, frozen_sum=frozen_sum+" + tempvocherAmt + ",available_balance=available_balance-" + tempvocherAmt + "  where  UsrCustId='" + UsrCustId + "' ";
                LogInfo.WriteLog("更新用户主表 余额-冻结金额    冻结总计=冻结总计+冻结金额sql" + sql);
                strSql.Append(sql);
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();


                //更新订单记录 20150409状态，为 1为投投有效订单

                sql = "update hx_Bid_records set ordstate=1 where OrdId='" + ordid + "'";
                LogInfo.WriteLog("更新投标记录状态:" + sql);
                strSql.Append(sql);
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();



                //更新优惠券支金额 MerPriv

                if (MerPriv != null && MerPriv != "")
                {
                    sql = "update bonus_account_water set expenditure=income where bonus_account_id  in (" + MerPriv + ")";

                    LogInfo.WriteLog("更新优惠券支出金额:" + sql);
                    strSql.Append(sql);
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();
                }
                /*
                //插入流水 GetAvailable_balance registerid,COALESCE(available_balance,0.00) as available_balance

                 */
                DataTable dec = GetAvailable_balance(UsrCustId);

                if (dec.Rows.Count > 0)
                {
                    decimal di = decimal.Parse(dec.Rows[0]["available_balance"].ToString()) - tempvocherAmt;

                    strSql.Append("insert into hx_Capital_account_water(");
                    strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                    strSql.Append(" values (");
                    strSql.Append("" + dec.Rows[0]["registerid"].ToString() + ",0," + tempvocherAmt + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + di + "," + (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.项目投资.ToString()) + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'项目投资')");

                    LogInfo.WriteLog("项目投资入账记录:" + strSql.ToString());
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();
                }


                if (dt.Rows.Count > 0)
                {
                    //更新代金券使用成功状态   从 3 锁定中  更新到 1已使用 
                    // sql = "update bonus_account set reward_state=1  where  reward_state=3 and  proid=" + bid + "";
                    sql = "update hx_UserAct set UseState=1,UseTime='" + DateTime.Now.ToString() + "'  where  UseState=3 and  AmtProid=" + bid + "";
                    LogInfo.WriteLog("更新代金券使用成功状态 从3 锁定中 更新到 1已使用sql" + sql);
                    strSql.Append(sql);

                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();
                }

            }



            i = DbHelperSQL.ExecuteSqlTran(SQLStringList);
            return i;
        }

        /// <summary>
        /// 投标失败时删除投标记录及冻结记录 由于冻结状态没有更新记录直接删除就可以
        /// </summary>
        /// <param name="UsrCustId">用户客户号</param>
        /// <param name="FrozenidNo">冻结订订单id</param>
        /// <param name="OrdId">投标记录订单id</param>

        /// <returns></returns>
        public int ReInvest_lost(string UsrCustId, string FrozenidNo, string OrdId)
        {
            int i = 0;
            List<string> SQLStringList = new List<string>();
            StringBuilder strSql = new StringBuilder();



            string sql = "select  bid_records_id,Frozenid from  V_Bid_Frozen where OrdId='" + OrdId + "' and  UsrCustId='" + UsrCustId + "' and  FrozenidNo='" + FrozenidNo + "'";


            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            if (dt.Rows.Count > 0)
            {

                //sql = "delete hx_td_frozen where  FrozenidNo='" + FrozenidNo + "'";

                sql = "delete hx_td_frozen where  FrozenidNo='" + FrozenidNo + "' and  Frozenid=" + dt.Rows[0]["Frozenid"].ToString();
                //更新冻结订单
                strSql.Append(sql);

                LogInfo.WriteLog("删除冻结订单sql" + sql);
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();





                sql = "delete hx_Bid_records where bid_records_id= " + dt.Rows[0]["bid_records_id"].ToString();
                LogInfo.WriteLog("删除投标记录sql" + sql);
                strSql.Append(sql);

                SQLStringList.Add(strSql.ToString());

                strSql.Clear();



                sql = "delete hx_income_statement where bid_records_id= " + dt.Rows[0]["bid_records_id"].ToString() + " and  BidOrderid=" + OrdId;
                LogInfo.WriteLog("删除收入明细sql" + sql);
                strSql.Append(sql);

                SQLStringList.Add(strSql.ToString());

                strSql.Clear();


                sql = "update  bonus_account  set reward_state=0, proid=null   where  proid=" + dt.Rows[0]["bid_records_id"].ToString() + " and  reward_state=3";

                LogInfo.WriteLog("还原优惠券sql" + sql);
                strSql.Append(sql);

                SQLStringList.Add(strSql.ToString());

                strSql.Clear();

            }

            i = DbHelperSQL.ExecuteSqlTran(SQLStringList);


            return i;
        }

        /// <summary>
        /// 取款审核多事务操作
        /// </summary>
        /// <param name="Rh"></param>
        /// <param name="aw"></param>
        /// <returns></returns>
        public int Su_CashProcessing(ReCashAudit Rh, M_Capital_account_water aw)
        {

            Utils.SetSYSDateTimeFormat();

            int i = 0;

            string sql = "";

            sql = "select registerid,available_balance,usertypes,FeeObjFlag from V_UserCash_Bank where OrdId='" + Rh.OrdId + "'  and (OrdIdState=0 or OrdIdState=1) ";

            LogInfo.WriteLog("取现sql:" + sql);
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            if (dt.Rows.Count > 0)
            {
                //Rh.TransAmt = decimal.Parse(Rh.TransAmt) + decimal.Parse(Rh.FeeAmt) > decimal.Parse(dt.Rows[0]["available_balance"].ToString()) ? (decimal.Parse(Rh.TransAmt) - decimal.Parse(Rh.FeeAmt)).ToString("0.00") : Rh.TransAmt;
                decimal dec = GetUsrBalanceAndForzen(Convert.ToInt32(dt.Rows[0]["registerid"].ToString()));

                aw.membertable_registerid = int.Parse(dt.Rows[0]["registerid"].ToString());
                aw.income = 0.00M;
                aw.expenditure = decimal.Parse(Rh.TransAmt);
                aw.time_of_occurrence = DateTime.Now;
                aw.account_balance = dec - aw.expenditure;  //要得么帐户余额
                aw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.提现成功.ToString());
                aw.createtime = DateTime.Now;
                aw.keyid = 0;
                aw.remarks = "用户提现：订单号" + Rh.OrdId;




                List<string> SQLStringList = new List<string>();
                StringBuilder strSql = new StringBuilder();
                //更新用户取现表状态

                if (Rh.AuditFlag == "S")  //复审通过
                {
                    //更新用户取现表状态
                    sql = "update hx_td_UserCash set OrdIdState=3,OperTime='" + DateTime.Now.ToString() + "',TransState='" + Rh.RespCode + "' where OrdId='" + Rh.OrdId + "'";
                    strSql.Append(sql);
                    SQLStringList.Add(sql);
                    //   LogInfo.WriteLog("复审通过更新用户取现表状态sql:" + sql);
                    strSql.Clear();
                    //更新帐户可用资产即扣款


                    strSql.Append("update hx_member_table set CashNum=CashNum+1,frozen_sum=frozen_sum-" + Rh.TransAmt + " where registerid=" + aw.membertable_registerid);
                    SQLStringList.Add(strSql.ToString());
                    // LogInfo.WriteLog("复审通过更新帐户可用资产即扣款 状态sql:" + strSql.ToString());
                    strSql.Clear();
                    //更新取现流水
                    strSql.Append("insert into hx_Capital_account_water(");
                    strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                    strSql.Append(" values (");
                    strSql.Append("" + aw.membertable_registerid + "," + aw.income + "," + aw.expenditure + ",'" + aw.time_of_occurrence + "'," + Convert.ToDecimal(dt.Rows[0]["available_balance"]) + "," + aw.types_Finance + ",'" + aw.createtime + "'," + aw.keyid + ",'" + aw.remarks + "')");
                    SQLStringList.Add(strSql.ToString());

                    //LogInfo.WriteLog("复审通过更新取现流水 状态sql:" + strSql.ToString());
                    strSql.Clear();


                    if (dt.Rows[0]["FeeObjFlag"].ToString() == "U")
                    {
                        if (Rh.FeeAmt != null)
                        {
                            decimal feeamt = decimal.Parse(Rh.FeeAmt);

                            if (feeamt > 0) //如果有手续费并大于0时帐户和流水应进行操作
                            {
                                //更新帐户可用资产即扣款
                                strSql.Append("update hx_member_table set frozen_sum=frozen_sum-" + Rh.FeeAmt + " where registerid=" + aw.membertable_registerid);
                                SQLStringList.Add(strSql.ToString());


                                // LogInfo.WriteLog("手续费更新帐户可用资产即扣款 状态sql:" + strSql.ToString());
                                strSql.Clear();


                                //手续费流水记录
                                aw.income = 0.00M;
                                aw.expenditure = decimal.Parse(Rh.FeeAmt);
                                aw.account_balance = aw.account_balance - decimal.Parse(Rh.FeeAmt);
                                aw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.手续费.ToString());

                                //更新取现流水
                                //strSql.Append("insert into hx_Capital_account_water(");
                                //strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                                //strSql.Append(" values (");
                                //strSql.Append("" + aw.membertable_registerid + "," + aw.income + "," + aw.expenditure + ",'" + aw.time_of_occurrence + "'," + Convert.ToDecimal(dt.Rows[0]["available_balance"]) + "," + aw.types_Finance + ",'" + aw.createtime + "'," + aw.keyid + ",'" + aw.remarks + "')");
                                //SQLStringList.Add(strSql.ToString());

                                //  LogInfo.WriteLog("手续费更新取现流水 状态sql:" + strSql.ToString());
                                strSql.Clear();

                                //服务费流水记录

                                //手续费流水记录
                                decimal servf = 0.00M;


                                if (dt.Rows[0]["usertypes"].ToString() == "0")
                                {
                                    servf = decimal.Parse(Rh.TransAmt) * Utils.GetinCashser();
                                }
                                else
                                {
                                    servf = decimal.Parse(Rh.TransAmt) * Utils.GetBoCashser();
                                }


                                if (servf > 0)
                                {
                                    aw.income = 0.00M;
                                    aw.expenditure = servf;
                                    aw.account_balance = aw.account_balance - servf;
                                    aw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.服务费.ToString());

                                    //更新取现流水
                                    strSql.Append("insert into hx_Capital_account_water(");
                                    strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                                    strSql.Append(" values (");
                                    strSql.Append("" + aw.membertable_registerid + "," + aw.income + "," + aw.expenditure + ",'" + aw.time_of_occurrence + "'," + Convert.ToDecimal(dt.Rows[0]["available_balance"]) + "," + aw.types_Finance + ",'" + aw.createtime + "'," + aw.keyid + ",'" + aw.remarks + "')");
                                    SQLStringList.Add(strSql.ToString());

                                    // LogInfo.WriteLog("更新取现流水1 状态sql:" + strSql.ToString());
                                    strSql.Clear();
                                }


                            }


                        }
                    }

                }
                else //复审拒绝
                {

                    sql = "update hx_td_UserCash set OrdIdState=4,OperTime='" + DateTime.Now.ToString() + "',TransState='" + Rh.RespCode + "' where OrdId='" + Rh.OrdId + "'";
                    // LogInfo.WriteLog("提现未通过:" + sql);
                    strSql.Append(sql);
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();

                    sql = "update  hx_member_table set available_balance=available_balance+" + decimal.Parse(Rh.TransAmt) + ",frozen_sum=frozen_sum-" + decimal.Parse(Rh.TransAmt) + " where registerid=" + dt.Rows[0]["registerid"].ToString();
                    strSql.Append(sql);
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();

                    strSql.Append("insert into hx_Capital_account_water(");
                    strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                    strSql.Append(" values (");
                    strSql.Append("" + aw.membertable_registerid + "," + aw.expenditure + "," + aw.income + ",'" + aw.time_of_occurrence + "'," + (Convert.ToDecimal(dt.Rows[0]["available_balance"]) + decimal.Parse(Rh.TransAmt)) + "," + (int)EnumTypesFinance.提现审核未通过 + ",'" + aw.createtime + "'," + aw.keyid + ",'提现未通过')");

                    // LogInfo.WriteLog("提现未通过:"+ sql);
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();

                    if (dt.Rows[0]["FeeObjFlag"].ToString() == "U")
                    {
                        if (Rh.FeeAmt != null)
                        {
                            decimal feeamt = decimal.Parse(Rh.FeeAmt);

                            if (feeamt > 0) //如果有手续费并大于0时帐户和流水应进行操作
                            {
                                //更新帐户可用资产
                                strSql.Append("update hx_member_table set available_balance=available_balance+" + Rh.FeeAmt + ",frozen_sum=frozen_sum-" + Rh.FeeAmt + " where registerid=" + aw.membertable_registerid);
                                SQLStringList.Add(strSql.ToString());


                                // LogInfo.WriteLog("手续费更新帐户可用资产即扣款 状态sql:" + strSql.ToString());
                                strSql.Clear();


                                //手续费流水记录
                                aw.income = 0.00M;
                                aw.expenditure = decimal.Parse(Rh.FeeAmt);
                                aw.account_balance = aw.account_balance - decimal.Parse(Rh.FeeAmt);
                                aw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.手续费退回.ToString());

                                //更新取现流水
                                strSql.Append("insert into hx_Capital_account_water(");
                                strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                                strSql.Append(" values (");
                                strSql.Append("" + aw.membertable_registerid + "," + aw.expenditure + "," + aw.income + ",'" + aw.time_of_occurrence + "'," + (Convert.ToDecimal(dt.Rows[0]["available_balance"]) + decimal.Parse(Rh.TransAmt) + decimal.Parse(Rh.FeeAmt)) + "," + aw.types_Finance + ",'" + aw.createtime + "'," + aw.keyid + ",'" + aw.remarks + "')");
                                SQLStringList.Add(strSql.ToString());

                                //  LogInfo.WriteLog("手续费更新取现流水 状态sql:" + strSql.ToString());
                                strSql.Clear();

                                //服务费流水记录

                                //手续费流水记录
                                decimal servf = 0.00M;


                                if (dt.Rows[0]["usertypes"].ToString() == "0")
                                {
                                    servf = decimal.Parse(Rh.TransAmt) * Utils.GetinCashser();
                                }
                                else
                                {
                                    servf = decimal.Parse(Rh.TransAmt) * Utils.GetBoCashser();
                                }


                                if (servf > 0)
                                {
                                    aw.income = servf;
                                    aw.expenditure = 0.00M;
                                    aw.account_balance = aw.account_balance + servf;
                                    aw.types_Finance = (int)Enum.Parse(typeof(EnumTypesFinance), EnumTypesFinance.服务费.ToString());

                                    //更新取现流水
                                    strSql.Append("insert into hx_Capital_account_water(");
                                    strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                                    strSql.Append(" values (");
                                    strSql.Append("" + aw.membertable_registerid + "," + aw.income + "," + aw.expenditure + ",'" + aw.time_of_occurrence + "'," + (Convert.ToDecimal(dt.Rows[0]["available_balance"]) + servf) + "," + aw.types_Finance + ",'" + aw.createtime + "'," + aw.keyid + ",'" + aw.remarks + "')");
                                    SQLStringList.Add(strSql.ToString());

                                    // LogInfo.WriteLog("更新取现流水1 状态sql:" + strSql.ToString());
                                    strSql.Clear();
                                }
                            }
                        }
                    }
                }



                i = DbHelperSQL.ExecuteSqlTran(SQLStringList);

            }

            return i;
        }


        #region 取现成功金额冻结处理,后等待复核 +int CashTran(string OpenAcctId, string OpenBankId, string OrdId, string TransAmt, string UsrCustId) 
        /// <summary>
        /// 取现成功金额冻结处理,后等待复核
        /// </summary>
        /// <param name="OpenAcctId">银行卡号</param>
        /// <param name="OpenBankId">银行号</param>
        /// <param name="OrdId">订单号</param>
        /// <param name="TransAmt">交易金额</param>
        /// <param name="UsrCustId">客户号</param>
        /// <returns></returns>
        public int CashTran(string OpenAcctId, string OpenBankId, string OrdId, string TransAmt, string UsrCustId)
        {
            int i = 0;
            List<string> SQLStringList = new List<string>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update hx_td_UserCash set OpenAcctId='" + OpenAcctId + "',OpenBankId='" + OpenBankId + "' where OrdId='" + OrdId + "'");
            LogInfo.WriteLog("1更新提现记录表:" + strSql.ToString());
            SQLStringList.Add(strSql.ToString());
            strSql.Clear();
            strSql.Append("update hx_member_table set available_balance= available_balance -'" + TransAmt + "',frozen_sum= frozen_sum + '" + TransAmt + "' where UsrCustId='" + UsrCustId + "'");
            LogInfo.WriteLog("1更新会员可用余额记录表:" + strSql.ToString());
            SQLStringList.Add(strSql.ToString());
            strSql.Clear();

            i = DbHelperSQL.ExecuteSqlTran(SQLStringList);
            SQLStringList.Clear();

            strSql.Append(" SELECT TOP 1 registerid,available_balance FROM [hx_member_table]  WHERE UsrCustId='" + UsrCustId + "' ");
            DataTable dt = DbHelperSQL.GET_DataTable_List(strSql.ToString());
            strSql.Clear();

            strSql.Append(" INSERT INTO [hx_Capital_account_water] (membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks) ");
            strSql.Append(" VALUES(" + Convert.ToInt32(dt.Rows[0]["registerid"]) + ",'0.00','" + TransAmt + "',GETDATE(),'" + Convert.ToDecimal(dt.Rows[0]["available_balance"]) + "','" + (int)EnumTypesFinance.提现审核中 + "',GETDATE(),'0','提现审核中'); ");
            LogInfo.WriteLog("记录取现资金流水表:" + strSql.ToString());
            SQLStringList.Add(strSql.ToString());
            DbHelperSQL.ExecuteSqlTran(SQLStringList);
            return i;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OpenAcctId"></param>
        /// <param name="OpenBankId"></param>
        /// <param name="OrdId"></param>
        /// <param name="TransAmt"></param>
        /// <param name="UsrCustId"></param>
        /// <param name="FeeAmt"></param>
        /// <param name="FeeObjFlag"></param>
        /// <param name="CashChl"></param>
        /// <returns></returns>
        public int CashTran(string OpenAcctId, string OpenBankId, string OrdId, string RealTransAmt, string UsrCustId, string FeeAmt, string FeeObjFlag, string CashChl)
        {
            int i = 0;
            List<string> SQLStringList = new List<string>();
            StringBuilder strSql = new StringBuilder();
            string sql = "select * from hx_member_table where UsrCustId='" + UsrCustId + "'";
            DataTable dtmember = DbHelperSQL.GET_DataTable_List(sql);
            decimal aballance = decimal.Parse(dtmember.Rows[0]["available_balance"].ToString());
            //if (FeeObjFlag == "U")
            //{
            //    if (dtmember != null && dtmember.Rows.Count > 0)
            //    {
            //        if (decimal.Parse(dtmember.Rows[0]["available_balance"].ToString()) < (decimal.Parse(TransAmt) + decimal.Parse(FeeAmt)))
            //        {
            //            TransAmt = (decimal.Parse(TransAmt) - decimal.Parse(FeeAmt)).ToString("0.00");
            //        }
            //    }
            //}
            //decimal overAvailableAmt = 0;
            //string transAmt = RealTransAmt;
            //if (aballance < decimal.Parse(RealTransAmt))
            //{
            //    overAvailableAmt = decimal.Parse(RealTransAmt) - aballance;
            //    transAmt = aballance.ToString("0.00");
            //}

            strSql.Append("update hx_td_UserCash set OpenAcctId='" + OpenAcctId + "',OpenBankId='" + OpenBankId + "',CashChl='" + CashChl + "' where OrdId='" + OrdId + "'");
            LogInfo.WriteLog("1更新提现记录表:" + strSql.ToString());
            SQLStringList.Add(strSql.ToString());
            strSql.Clear();
            strSql.Append("update hx_member_table set available_balance= available_balance -'" + RealTransAmt + "',frozen_sum= frozen_sum + '" + RealTransAmt + "' where UsrCustId='" + UsrCustId + "'");
            LogInfo.WriteLog("1更新会员可用余额记录表:" + strSql.ToString());
            SQLStringList.Add(strSql.ToString());
            strSql.Clear();

            //i = DbHelperSQL.ExecuteSqlTran(SQLStringList);
            //SQLStringList.Clear();

            //strSql.Append(" SELECT TOP 1 registerid,available_balance FROM [hx_member_table]  WHERE UsrCustId='" + UsrCustId + "' ");
            //DataTable dt = DbHelperSQL.GET_DataTable_List(strSql.ToString());
            //strSql.Clear();

            aballance -= decimal.Parse(RealTransAmt);

            strSql.Append(" INSERT INTO [hx_Capital_account_water] (membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks) ");
            strSql.Append(" VALUES(" + Convert.ToInt32(dtmember.Rows[0]["registerid"]) + ",'0.00','" + RealTransAmt + "',GETDATE(),'" + aballance.ToString("0.00") + "','" + (int)EnumTypesFinance.提现审核中 + "',GETDATE(),'0','提现审核中'); ");
            LogInfo.WriteLog("记录取现资金流水表:" + strSql.ToString());
            SQLStringList.Add(strSql.ToString());
            strSql.Clear();
            //string allAmt = TransAmt;
            if (FeeObjFlag == "U")
            {
                if (dtmember != null && dtmember.Rows.Count > 0 && decimal.Parse(FeeAmt) > 0)
                {
                    strSql.Append("update hx_td_UserCash set FeeAmt='" + FeeAmt + "' where OrdId='" + OrdId + "'");
                    LogInfo.WriteLog("1更新提现记录表:" + strSql.ToString());
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();

                    aballance -= decimal.Parse(FeeAmt);
                    if (aballance > 0)
                    {
                        strSql.Append("update hx_member_table set available_balance= available_balance -'" + FeeAmt + "',frozen_sum= frozen_sum + '" + FeeAmt + "' where UsrCustId='" + UsrCustId + "'");
                        LogInfo.WriteLog("1更新会员可用余额记录表:" + strSql.ToString());
                        SQLStringList.Add(strSql.ToString());
                        strSql.Clear();
                    }

                    strSql.Append(" INSERT INTO [hx_Capital_account_water] (membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks) ");
                    strSql.Append(" VALUES(" + Convert.ToInt32(dtmember.Rows[0]["registerid"]) + ",'0.00','" + FeeAmt + "',GETDATE(),'" + (aballance > 0 ? aballance.ToString("0.00") : "0.00") + "','" + (int)EnumTypesFinance.手续费 + "',GETDATE(),'0','手续费'); ");
                    LogInfo.WriteLog("记录取现资金流水表:" + strSql.ToString());
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();
                }
            }
            i = DbHelperSQL.ExecuteSqlTran(SQLStringList);
            return i;
            //return CashTran(OpenAcctId, OpenBankId, OrdId, TransAmt, UsrCustId);
        }
        #endregion



        /// <summary>
        /// 充值事务处理  更新充值明细，资金记录，主表总资产更新
        /// </summary>
        /// <param name="SQLStringList"></param>
        /// <returns></returns>
        public int rechargeTran(M_Recharge_history Rh, M_Capital_account_water aw)
        {
            int i = 0;
            string sql = "select * from hx_Capital_account_water where remarks='" + Rh.order_No + "'";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            LogInfo.WriteLog("充值查找是否更新状态sql:" + sql);
            LogInfo.WriteLog("list1:" + dt.Rows.Count.ToString());
            List<string> SQLStringList = new List<string>();
            if (dt.Rows.Count <= 0)
            {



                StringBuilder strSql = new StringBuilder();
                ///列新充值关状态记录
                // strSql.Append("insert into hx_Recharge_history(");
                //  strSql.Append("membertable_registerid,recharge_amount,recharge_time,account_amount,order_No,recharge_condition,recharge_bank)");
                //  strSql.Append(" values (");
                //  strSql.Append(""+Rh.membertable_registerid+","+Rh.recharge_amount+",'"+Rh.recharge_time+"',"+Rh.account_amount+",'"+Rh.order_No+"',"+Rh.recharge_condition+",'"+Rh.recharge_bank+"')");

                strSql.Append("update hx_Recharge_history set recharge_condition=1,recharge_bank='" + Rh.recharge_bank + "' where  order_No='" + Rh.order_No + "' and recharge_history_id=" + Rh.recharge_history_id);

                LogInfo.WriteLog("1更新充值记录表:" + strSql.ToString());

                SQLStringList.Add(strSql.ToString());
                strSql.Clear();
                //充值流水记录
                strSql.Append("insert into hx_Capital_account_water(");
                strSql.Append("membertable_registerid,income,expenditure,time_of_occurrence,account_balance,types_Finance,createtime,keyid,remarks)");
                strSql.Append(" values (");
                strSql.Append("" + aw.membertable_registerid + "," + aw.income + "," + aw.expenditure + ",'" + aw.time_of_occurrence + "'," + aw.account_balance + "," + aw.types_Finance + ",'" + aw.createtime + "'," + aw.keyid + ",'" + aw.remarks + "')");

                LogInfo.WriteLog("2充值流水记录:" + strSql.ToString());
                SQLStringList.Add(strSql.ToString());

                strSql.Clear();



                //更新帐户总资产

                //account_total_assets

                /*

                sql = "select registerid,account_total_assets,available_balance from hx_member_table  where registerid=" + Rh.membertable_registerid;


                DataTable dct = DbHelperSQL.GET_DataTable_List(sql);

                if (dct.Rows.Count > 0)
                {
                    strSql.Append("update hx_member_table  WITH (ROWLOCK) set account_total_assets=account_total_assets+" + Rh.recharge_amount + ",available_balance=available_balance+" + Rh.recharge_amount + " where registerid=" + Rh.membertable_registerid);
                    LogInfo.WriteLog("3更新帐户总资产:" + strSql.ToString());
                    SQLStringList.Add(strSql.ToString());
                    strSql.Clear();
                }
                 */
                strSql.Append("update hx_member_table  set RechargeNum=RechargeNum+1, account_total_assets=account_total_assets+" + Rh.recharge_amount + ",available_balance=available_balance+" + Rh.recharge_amount + " where registerid=" + Rh.membertable_registerid);
                LogInfo.WriteLog("3更新帐户总资产:" + strSql.ToString());
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();

            }
            i = DbHelperSQL.ExecuteSqlTran(SQLStringList);
            return i;

        }



        /// <summary>
        /// 汇付开户成功后进用户数据更新
        /// </summary>
        /// <param name="ReUse"></param>
        public int Succ_Reg(ReUserRegister m)
        {
            string sql = " update hx_member_table  set  UsrCustId ='" + m.UsrCustId + "', realname='" + m.UsrName + "',iD_number='" + m.IdNo + "',UsrId='" + m.UsrId + "',open_tonto_account=1,isrealname=1 where username='" + Utils.GetUserSplit(m.UsrId) + "'";
            LogInfo.WriteLog("注册第三方开户成功:" + sql);
            return DbHelperSQL.ExecuteSql(sql);
        }







        #region 由于加息券与抵扣券 放款时获取对应项目投标记录,及类型 的代金券金额 
        /// <summary>
        /// 放款时获取对应项目投标记录的代金券金额  RewTypeID 1 现金  2 抵扣券  3加券息
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="bid_records_id">投标记录id</param>
        /// <returns></returns>
        public DataTable GetLoansVocherAmtDT(string userid, string bid_records_id)
        {
            // string str = "0.00";

            //  string sql = "SELECT  COALESCE(SUM( amount_of_reward),0.00) as  amount_of_reward  from bonus_account  where reward_state=1 and  proid =" + bid_records_id + "  and membertable_registerid=" + userid.ToString();
            //活动更新后的语句
            string sql = "SELECT  COALESCE(SUM(Amt),0.00) as  amount_of_reward,RewTypeID  from hx_UserAct  where UseState=1  and RewTypeID=2  and  AmtProid =" + bid_records_id + "  and registerid=" + userid.ToString() + " GROUP BY  RewTypeID";


            LogInfo.WriteLog("放款时代金券查询sql:" + sql);
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            //str = decimal.Parse(dt.Rows[0][0].ToString()).ToString("0.00");

            return dt;
        }
        #endregion


        #region 放款时获取对应项目投标记录的代金券金额 
        /// <summary>
        /// 放款时获取对应项目投标记录的代金券金额
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="bid_records_id">投标记录id</param>
        /// <returns></returns>
        public string GetLoansVocherAmt(string userid, string bid_records_id)
        {
            string str = "0.00";

            //  string sql = "SELECT  COALESCE(SUM( amount_of_reward),0.00) as  amount_of_reward  from bonus_account  where reward_state=1 and  proid =" + bid_records_id + "  and membertable_registerid=" + userid.ToString();
            //活动更新后的语句 只取红包
            string sql = "SELECT  COALESCE(SUM( Amt),0.00) as  amount_of_reward  from hx_UserAct  where UseState=1 and RewTypeID=2 and  AmtProid =" + bid_records_id + "  and registerid=" + userid.ToString();


            LogInfo.WriteLog("放款时代金券查询sql:" + sql);
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            str = decimal.Parse(dt.Rows[0][0].ToString()).ToString("0.00");

            return str;
        }
        #endregion

        /// <summary>
        /// 投标成功后通过视图获取投标项目对应使用的代金券/加息券 返回 DataTable 第一列 amount_of_reward 第二列  proid
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="OrdId">投标记录订单id</param>
        /// <returns></returns>
        public DataTable GetLonansVocherAmtByOrdId(string OrdId)
        {
            // string sql = "SELECT  COALESCE(SUM( amount_of_reward),0.00) as  amount_of_reward,proid  from V_bid_records_bonus_account where  reward_state=3 and OrdId =" + OrdId + " group by proid ";

            string sql = "SELECT  COALESCE(SUM(Amt),0.00) as  amount_of_reward,AmtProid as proid  from View_BId_userAct where  UseState=3 and (RewTypeID=2 or RewTypeID=3) and OrdId ='" + OrdId + "' group by AmtProid ";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            return dt;

        }


        /// <summary>
        /// 根据 UsrCustId获取用户余额
        /// </summary>
        /// <param name="UsrCustId"></param>
        /// <returns></returns>
        public DataTable GetAvailable_balance(string UsrCustId)
        {
            DataTable dec = new DataTable();

            string sql = "select registerid,COALESCE(available_balance,0.00) as available_balance   from hx_member_table where UsrCustId='" + UsrCustId + "'";

            dec = DbHelperSQL.GET_DataTable_List(sql);

            return dec;
        }


        /// <summary>
        /// 根据 userid 获取用户余额
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal GetUsridAvailable_balance(int userid)
        {
            decimal dec = 0.00M;

            string sql = "select COALESCE(available_balance,0.00) as available_balance   from hx_member_table where registerid=" + userid + "";

            dec = decimal.Parse(DbHelperSQL.Re_String(sql));

            return dec;
        }


        public decimal GetUsrBalanceAndForzen(int userid)
        {
            decimal dec = 0.00M;

            string sql = "select ISNULL((COALESCE(available_balance,0.00)),0.00) as available_balance   from hx_member_table where registerid = " + userid + "";

            dec = decimal.Parse(DbHelperSQL.Re_String(sql));

            return dec;
        }

        public decimal NewGetUsrBalanceAndForzen(int userid)
        {
            decimal dec = 0.00M;

            string sql = "select ISNULL((COALESCE(available_balance,0.00)),0.00) as available_balance   from hx_member_table where registerid = " + userid + "";

            dec = decimal.Parse(DbHelperSQL.Re_String(sql));

            return dec;
        }



        /// <summary>
        /// 获取用户可用奖历
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal GetRewards(int userid)
        {
            decimal dec = 0.00M;
            string sql = "SELECT  COALESCE(SUM( amount_of_reward),0.00) as  amount_of_reward  from bonus_account  where reward_state=0 and membertable_registerid=" + userid.ToString();
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            dec = decimal.Parse(dt.Rows[0][0].ToString());
            return dec;
        }

        /// <summary>
        /// 返回用户有效投资积极总数
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public decimal GetUserAmt(int userid)
        {
            string sql = "select  COALESCE(sum(investment_amount),0.00) as 有效投资金额  from hx_Bid_records where ordstate=1 and investor_registerid=" + userid.ToString();
            return decimal.Parse(DbHelperSQL.Re_String(sql));
        }



        /// <summary>
        /// 检查手机号是否存在   20150706 进行升级后判断手机或用户名是否存在
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="tmpeid"></param>
        /// <returns></returns>
        public string checkmobilenew(string mobile, int tmpeid)
        {
            string sql = "select registerid,mobile from hx_member_table where mobile='" + mobile + "' or username='" + mobile + "' ";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            string str = "";
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["mobile"].ToString() == mobile && dt.Rows[0]["registerid"].ToString() == tmpeid.ToString() && tmpeid > 0)
                {
                    str = "y";

                }
                else
                {

                    str = "手机号已被注册!";

                }
            }
            else
            {
                str = "y";

            }

            return str;

        }


        /// <summary>
        /// ip间隔的60秒后才能再次发送  为 false 不能再发了
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="smstype"></param>
        /// <param name="smstype1"></param>
        /// <returns></returns>
        public bool checkipsess(string ip, int smstype, int smstype1)
        {
            bool t = false;

            //string sql = "select  ip  from  hx_td_SMS_record  where ip='" + ip + "' and ( smstype=" + smstype + "  or  smstype=" + smstype1 + ")  and abs(datediff(ss,sendtime,GETDATE()))>60  and hits<4  order by sms_record_id desc ";

            string sql = "select  ip ,sendtime, hits  from  hx_td_SMS_record  where ip='" + ip + "' and ( smstype=" + smstype + "  or  smstype=" + smstype1 + ")   order by sms_record_id desc ";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            if (dt.Rows.Count > 0)
            {

                long sec = Utils.DateDiff("Second", DateTime.Parse(dt.Rows[0]["sendtime"].ToString()), DateTime.Now);
                if (sec > 60)
                {
                    if (int.Parse(dt.Rows[0]["hits"].ToString()) <= 8)
                    {
                        t = true;
                    }
                    else
                    {
                        t = false;
                    }
                }
                else
                {
                    t = false;
                }

            }
            else
            {
                t = true;
            }


            return t;
        }


        /// <summary>
        /// 判断返回手机号发送过的次数实现第二次发送需要收入验证码   isip =fase 验证手机  isip=true 验证ip
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="smstype"></param>
        /// <param name="smstype1"></param>
        /// <returns></returns>
        public int checkmobnum(string tel, int smstype, int smstype1, bool isip = true)
        {
            int num = 0;
            string sql = "";
            if (isip == false)
            {
                sql = "select  COALESCE(sum(hits),0) as hitsnum  from  hx_td_SMS_record  where phone_number='" + tel + "' and ( smstype=" + smstype + "  or  smstype=" + smstype1 + ") ";
            }
            else
            {
                sql = "select  COALESCE(sum(hits),0) as hitsnum  from  hx_td_SMS_record  where ip='" + tel + "' and ( smstype=" + smstype + "  or  smstype=" + smstype1 + ") ";
            }
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            if (dt.Rows.Count > 0)
            {
                num = int.Parse(dt.Rows[0]["hitsnum"].ToString());


            }
            else
            {
                num = 0;
            }




            return num;
        }



        /// <summary>
        /// 返回同一ip发送某种类别短信的次数
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="smstype"></param>
        /// <param name="smstype1"></param>
        /// <returns></returns>
        public int checkipnum(string ip, int smstype, int smstype1)
        {
            int num = 0;

            string sql = "select  count(ip) as ipnum  from  hx_td_SMS_record  where ip='" + ip + "' and ( smstype=" + smstype + "  or  smstype=" + smstype1 + ") and DATEDIFF(day, sendtime, getdate())<=14 ";
            LogInfo.WriteLog("xxxx:" + sql);
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            if (dt.Rows.Count > 0)
            {
                try
                {
                    num = int.Parse(dt.Rows[0]["ipnum"].ToString());

                }
                catch
                {
                    num = 0;
                }

            }


            return num;
        }



        /// <summary>
        /// 检查手机号是否存在
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="tmpeid"></param>
        /// <returns></returns>
        public string checkmobile(string mobile, int tmpeid)
        {
            string sql = "select registerid,mobile from hx_member_table where mobile='" + mobile + "'";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            string str = "";
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["mobile"].ToString() == mobile && dt.Rows[0]["registerid"].ToString() == tmpeid.ToString() && tmpeid > 0)
                {
                    str = "y";

                }
                else
                {

                    str = "手机号已被注册!";

                }
            }
            else
            {
                str = "y";

            }

            return str;

        }


        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string checkname(string username, int tmpeid)
        {

            string sql = "select registerid,username from hx_member_table where username='" + username + "'";

            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            string str = "";
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["username"].ToString() == username && dt.Rows[0]["registerid"].ToString() == tmpeid.ToString() && tmpeid > 0)
                {
                    str = "y";
                }
                else
                {

                    str = "用户名已被注册!";

                }
            }
            else
            {
                str = "y";

            }

            return str;
        }






        /// <summary>
        /// 验证码证码是正确
        /// </summary>
        /// <param name="param">验证码</param>
        /// <param name="mobilec">手机号</param>
        /// <returns></returns>
        public string GetVcode(string param, string mobilec)
        {

            string str = "";

            int smstype = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.短信验证码.ToString());

            int smstype1 = (int)Enum.Parse(typeof(EnumSMSType), EnumSMSType.语音短信验证码.ToString());

            string sql = "select sms_record_id,smscontext,phone_number,vcode from hx_td_SMS_record where ( smstype=" + smstype + "  or  smstype=" + smstype1 + " ) and phone_number='" + mobilec + "' and  DATEDIFF(MINUTE,sendtime,getDate())<7 order by sms_record_id desc";
            string vcode = string.Empty;
            DataTable dt1 = DbHelperSQL.GET_DataTable_List(sql);
            if (dt1.Rows.Count > 0)
            {
                vcode = dt1.Rows[0]["vcode"].ToString();
                if (vcode == param)
                {
                    str = "y";
                }
                else
                {
                    str = "验证码不正确!";
                }

            }
            else
            {
                str = "验证码不存在!";
            }

            LogInfo.WriteLog("验证码验证:sql=" + sql + "；查询结果vcode=" + vcode + "；param=" + param + "；返回信息=" + str);
            return str;
        }




        /// <summary>
        /// 添加新消息
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static int AddMessage(M_td_System_message p)
        {
            B_td_System_message b = new B_td_System_message();
            return b.Add(p);
        }


        /// <summary>
        /// 累计帮助企业和个人融资
        /// </summary>
        /// <returns></returns>
        public static string GetALLFinance()
        {
            string str = "0.00";
            string sql = "  SELECT CONVERT(VARCHAR(100),CAST(CONVERT(DECIMAL(38,2),LTRIM(SUM(borrowing_balance)))+ 30000000.00 AS MONEY),1) money FROM hx_borrowing_target where  tender_state  between 2 and 5";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["money"] != null)
                {
                    str = dt.Rows[0]["money"].ToString();
                }
                else
                {
                    str = "0.00";
                }
            }
            else
            {
                str = "0.00";
            }
            return str;
        }

        /// <summary>
        /// 累计为投资人赚取收益
        /// </summary>
        /// <returns></returns>
        public static string GetIncome()
        {
            string str = "0.00";
            string sql = "select CONVERT(VARCHAR(100),CAST(CONVERT(DECIMAL(38,2),LTRIM(SUM(interestpayment)))+ 321646.58 + 700000.00 AS MONEY),1)  money   from  hx_income_statement";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["money"] != null)
                {
                    str = dt.Rows[0]["money"].ToString();
                }
                else
                {
                    str = "0.00";
                }
            }
            else
            {
                str = "0.00";
            }
            return str;
        }
        /// <summary>
        ///  累计投资笔数，累计赚取，交易金额（累计帮助企业和个人融资）
        /// </summary>
        /// <returns>dt  1.累计投资笔数 2.累计赚取 3.交易金额</returns>
        public static string[] GetTotal()
        {
            string cacheName = "HOME_TOTAL_DATATABLE";
            string[] results = (string[])Utils.GetThirdCacheObject(cacheName);
            if (results == null)
            {
                string sql = string.Empty;
                sql += " (SELECT CAST(CONVERT(DECIMAL(38,2),LTRIM(COUNT(bid_records_id))) AS MONEY)  value   FROM  hx_Bid_records WHERE ordstate=1)   ";
                sql += " UNION ALL ";
                sql += " (SELECT CAST(CONVERT(DECIMAL(38,2),LTRIM(SUM(interestpayment)))+ 321646.58 + 700000.00 AS MONEY)  value  FROM  hx_income_statement) ";
                sql += " UNION ALL ";
                sql += " (SELECT CAST(CONVERT(DECIMAL(38,2),LTRIM(SUM(borrowing_balance)))+ 30000000.00 AS MONEY) value FROM hx_borrowing_target WHERE   tender_state  between 2 and 5) ";
                results = new string[3];
                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results[0] = dt.Rows[2]["value"].ToString();
                    results[1] = dt.Rows[1]["value"].ToString();
                    results[2] = dt.Rows[0]["value"].ToString();
                    Utils.SetThirdCacheName(cacheName, results, 2);
                }
            }
            return results;
        }
        /// <summary>
        /// 累计投资笔数
        /// </summary>
        /// <returns></returns>
        public static string GetInvestment()
        {
            string str = "0";
            string sql = " select COALESCE(COUNT(bid_records_id),0) as num   from  hx_Bid_records where ordstate=1";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                str = dt.Rows[0]["num"].ToString();
            }
            return str;
        }


        /// <summary>
        /// 返回标的项目的借款天数
        /// </summary>
        /// <param name="targetid"></param>
        /// <returns></returns>
        public static int GetTargetidDays(string targetid)
        {
            int days = 0;
            string sql = "select  targetid,loan_number,borrowing_title,borrowing_thumbnail,annual_interest_rate,borrowing_balance,life_of_loan,unit_day,start_time,end_time, recommend,release_date,month_payment_date,repayment_date,minimum,maxmum,company_name,guarantee_way_name,fundraising_amount,tender_state,payment_options,G_contract_Path,sys_time,IsUse,companyid from V_borrowing_target_addlist where tender_state>=2 and targetid = " + targetid;
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                DateTime rpdt = DateTime.Parse(dt.Rows[0]["repayment_date"].ToString());
                DateTime rest = DateTime.Parse(dt.Rows[0]["release_date"].ToString());
                long diffdays = Utils.DateDiff("Day", DateTime.Parse(rest.ToString("yyyy-MM-dd")), DateTime.Parse(rpdt.ToString("yyyy-MM-dd")));
                days = int.Parse(diffdays.ToString());
            }
            return days;


        }

        /// <summary>
        /// 查取投资排名  top 获取记录数 days  为0是所有的天数的排名  
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public static string GetInvAmtlist(int top, int days)
        {
            StringBuilder str = new StringBuilder();

            string sql = "";

            if (days > 0)
            {
                sql = "select top " + top.ToString() + " investor_registerid,mobile,realname,sum(investment_amount) as 投资金额  from V_hx_Bid_records_borrowing_target   where DATEDIFF(day,invest_time,getdate())< " + days.ToString() + "     group by investor_registerid,mobile,realname order by sum(investment_amount) desc";
            }
            else
            {
                sql = "select  top " + top.ToString() + " investor_registerid,mobile,realname,sum(investment_amount) as 投资金额  from V_hx_Bid_records_borrowing_target    group by investor_registerid,mobile,realname order by sum(investment_amount) desc";
            }


            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

            if (dt.Rows.Count > 0)
            {
                int n = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str.Append("<li><div class=\"ranking_icon\">");


                    if (n <= 3)
                    {
                        str.Append("<span class=\"crown_icon\">" + n.ToString() + "</span>");
                    }
                    else
                    {
                        str.Append("<span class=\"crown_icon_grey\">" + n.ToString() + "</span>");

                    }

                    str.Append("</div><div class=\"ranking_user\">" + Utils.hidemobile(dt.Rows[i]["mobile"].ToString()) + "</div><div class=\"ranking_money\">" + dt.Rows[i]["投资金额"].ToString().Replace(".00", "") + "元</div></li>");
                    n = n + 1;

                }
            }
            return str.ToString();
        }


        /// <summary>
        /// 获取成功的投资次数
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int GetInvestCountByUserid(int uid)
        {
            int i = 0;
            string sql = "select count(*) from hx_Bid_records where investor_registerid=" + uid + " and  invest_state=1 and ordstate = 1  group by  investor_registerid";
            LogInfo.WriteLog(" 获取成功的投资次数:" + sql);
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                i = dt.Rows[0][0].ToInt();
            }

            return i;
        }


        /// <summary>
        /// 获取成功的投资金额（11月活动，获取15天内首投满2000奖励10元现金活动，新手标除外）
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static DataTable GetInvestCountByUseridNew(int uid)
        {
            string sql = "select investment_amount as InvCount_Amt from hx_Bid_records where investor_registerid=" + uid + " and  invest_state=1 and ordstate = 1 ";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            return dt;
        }
        ///// <summary>
        ///// 废弃  检查本人是否是第一次投标，如果大于0则不是新手 ???待整合
        ///// </summary>
        ///// <param name="uid"></param>
        ///// <returns></returns>
        //public static int GetIsNews(int uid)
        //{
        //    int i = 0;
        //    // string sql = "select a.registerid,a.username,a.mobile from hx_member_table a, hx_Bid_records b where a.registerid = b.investor_registerid and  b.invest_state=2  and a.registerid=" + uid + "  group by  a.registerid,a.username,a.mobile";

        //    string sql = "select a.registerid,a.username,a.mobile from hx_member_table a, hx_Bid_records b where a.registerid = b.investor_registerid  and a.registerid=" + uid + " and  b.invest_state=1  group by  a.registerid,a.username,a.mobile";

        //    LogInfo.WriteLog("是否第一次投标:" + sql);
        //    DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

        //    i = dt.Rows.Count;

        //    return i;
        //}
        ///// <summary>
        ///// 废弃   检查本人是否是第一次投标,会查询处所有投资记录，投资次数与新手验证那个不一样???待整合
        ///// </summary>
        ///// <param name="uid"></param>
        ///// <returns></returns>
        //public static int GetIsNews1(int uid)
        //{
        //    int i = 0;
        //    // string sql = "select a.registerid,a.username,a.mobile from hx_member_table a, hx_Bid_records b where a.registerid = b.investor_registerid and  b.invest_state=2  and a.registerid=" + uid + "  group by  a.registerid,a.username,a.mobile";

        //    string sql = "select a.registerid,a.username,a.mobile,b.targetid,b.bid_records_id from hx_member_table a, hx_Bid_records b where a.registerid = b.investor_registerid  and a.registerid=" + uid + " and  b.invest_state=1  group by  a.registerid,a.username,a.mobile,b.targetid,b.bid_records_id";

        //    LogInfo.WriteLog("是否第一次投标:" + sql);
        //    DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

        //    i = dt.Rows.Count;

        //    return i;
        //}


        #region 获取投资最大的用户 +static DataTable Topinvestor(int targetid)
        /// <summary>
        /// 获取投资最大的用户 返回 本标所有投次金额 InvCount_Amt, 最大投次金额maxamt 及投资用户investor_registerid
        /// </summary>
        /// <param name="targetid"></param>
        /// <returns></returns>
        public static DataTable Topinvestor(int targetid)
        {
            DataTable dt = new DataTable();
            string sql = "select  COALESCE(SUM(investment_amount),0) as InvCount_Amt,MAX(investment_amount) as maxamt, investor_registerid from  hx_Bid_records where targetid  = " + targetid + "  group by investor_registerid";
            dt = DbHelperSQL.GET_DataTable_List(sql);
            return dt;
        }
        #endregion

        #region 第一单投次用户+static int TopNum(string targetid)？？？？
        /// <summary>
        /// 第一单投次用户
        /// </summary>
        /// <param name="targetid"></param>
        /// <returns></returns>
        public static int TopNum(string targetid)
        {
            int i = 0;
            string sql = "select COALESCE(count(bid_records_id),0) as TopNum  from  hx_Bid_records where  targetid=" + targetid;
            i = DbHelperSQL.Execint(sql);
            return i;
        }
        #endregion

        #region 检查活动是否超过封顶人数+static int GetTopNum(int ActID)
        /// <summary>
        /// 检查活动是否超过封顶人数
        /// </summary>
        /// <param name="ActID"></param>
        /// <returns></returns>
        public static int GetTopNum(int ActID)
        {
            int i = 0;
            string sql = "select COALESCE(count(ActID),0) as TopNum from  hx_UserAct where  ActID=" + ActID.ToString();
            // DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            i = DbHelperSQL.Execint(sql);
            return i;
        }
        #endregion


        #region 统计活动封顶金额
        /// <summary>
        /// 统计活动封顶金额
        /// </summary>
        /// <param name="ActID"></param>
        /// <returns></returns>
        public static decimal GetTopAmtCount(int ActID)
        {
            decimal i = 0M;
            string sql = "select COALESCE(sum(amt),0) as TotalAmt from  hx_UserAct where  actid=" + ActID.ToString();
            i = decimal.Parse(DbHelperSQL.Re_String(sql));
            return i;

        }
        #endregion


        #region 统计邀请客户投资奖励总金额
        /// <summary>
        /// 统计邀请客户投资奖励总金额
        /// </summary>
        /// <param name="Registerid"></param>
        /// <param name="biyaoUsrid"></param>
        /// <returns></returns>
        public static decimal GetInviUserTotalAmt(int Registerid, int biyaoUsrid)
        {
            decimal i = 0M;
            string sql = "select COALESCE(count(income),0) as TotalAmt from  bonus_account_water where  membertable_registerid=" + Registerid.ToString() + " and water_type=" + biyaoUsrid.ToString();
            i = decimal.Parse(DbHelperSQL.Re_String(sql));
            return i;
        }
        #endregion



        #region 获取邀请码
        /// <summary>
        /// 获取邀请码
        /// </summary>
        /// <param name="invpersonid">投资人ID</param>
        /// <returns></returns>
        public static string GetUserInvCode(string invpersonid)
        {
            string i = "";
            string sql = "select invcode from hx_td_Userinvitation where invpersonid = " + invpersonid;
            i = DbHelperSQL.Re_String(sql);
            return i;
        }
        #endregion


        #region 活动向用户现金额转账+int UpateActToUserTransfer(ReTransfer re)
        /// <summary>
        /// 活动向用户现金额转账
        /// </summary>
        /// <param name="re"></param>
        /// <returns></returns>
        public int UpateActToUserTransfer(ReTransfer re, int bid_records_id)
        {


            int i = 0;
            List<string> SQLStringList = new List<string>();
            StringBuilder strSql = new StringBuilder();
            string sql = "";
            sql = "SELECT OrderID FROM hx_UserAct where UseState=5 and OrderID='" + re.OrdId + "' and  ActID =" + re.MerPriv;

            string log = "活动企业现金转账查询活动表语句:" + sql;
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                //modify by fangjianmin 现金状态为1 已使用 而不是4
                sql = "update hx_UserAct  set  UseState=1  where UseState=5 and OrderID='" + re.OrdId + "' and  ActID =" + re.MerPriv;
                //更新用户活动表
                strSql.Append(sql);
                log += "<br>更新用户活动表:" + sql;
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();
                sql = "update hx_member_table  set account_total_assets=account_total_assets+" + re.TransAmt + ",available_balance=available_balance+" + re.TransAmt + " where UsrCustId='" + re.InCustId + "'";
                strSql.Append(sql);
                log += "<br>更新奖励的客户账户金额:" + sql;
                SQLStringList.Add(strSql.ToString());
                strSql.Clear();
                ///更新记录表奖励 
                if (bid_records_id > 0)
                {
                    //sql = "update hx_Bid_records set BonusAmt=" + re.TransAmt + " where   bid_records_id=" + bid_records_id;
                    //strSql.Append(sql);
                    //LogInfo.WriteLog("更新记录表奖励金额" + sql);
                    //SQLStringList.Add(strSql.ToString());
                    //strSql.Clear();
                }


                i = DbHelperSQL.ExecuteSqlTran(SQLStringList);
                log += "<br>执行sql语句返回:" + i;
            }
            LogInfo.WriteLog(log);
            return i;
        }

        #endregion




        /// <summary>
        /// 待收本金收益
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetToatlPrincipal(int userid)
        {

            string sql = "  select  top  5  COALESCE(sum(investment_amount),0)  as withoutinterest,convert(varchar,year(invest_time))+'-'+convert(varchar,month(invest_time)) as dateime  from   V_hx_Bid_records_borrowing_target  where investor_registerid=" + userid + "  group by convert(varchar,year(invest_time))+'-'+convert(varchar,month(invest_time))  order by  convert(varchar,year(invest_time))+'-'+convert(varchar,month(invest_time)) desc";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            return dt;
        }

        /// <summary>
        /// 待收本金收益+本金
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetToatlPrincipalALL(int userid)
        {

            string sql = "select top  5  COALESCE(sum(investment_amount + withoutinterest+haveinterest),0)  as ben, convert(varchar, year(invest_time))+'-'+convert(varchar, month(invest_time)) as dateime from   V_hx_Bid_records_borrowing_target where investor_registerid=" + userid + "  group by convert(varchar, year(invest_time))+'-'+convert(varchar, month(invest_time))  order by  convert(varchar, year(invest_time))+'-'+convert(varchar, month(invest_time)) desc";
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            return dt;
        }


        /// <summary>
        /// 利息收益 withoutinterest  dateime
        /// </summary>
        /// <param name="usrid"></param>
        /// <returns></returns>
        public DataTable GetTotalAmt(int usrid)
        {
            DataTable dt = new DataTable();

            string sql = " select  top  5  COALESCE(sum(withoutinterest),0)  as withoutinterest,convert(varchar,year(invest_time))+'-'+convert(varchar,month(invest_time)) as dateime  from   hx_Bid_records  where investor_registerid=" + usrid + "  group by convert(varchar,year(invest_time))+'-'+convert(varchar,month(invest_time))  order by  convert(varchar,year(invest_time))+'-'+convert(varchar,month(invest_time)) desc";

            dt = DbHelperSQL.GET_DataTable_List(sql);

            return dt;
        }

        /// <summary>
        /// 根据投次记录id取得优优惠券
        /// </summary>
        /// <param name="bid_records_id"></param>
        /// <returns></returns>
        public string GetBid_AmtProid(int bid_records_id)
        {
            string str = "";
            string sql = "select AmtProid  FROM hx_UserAct where AmtProid= " + bid_records_id;
            DataTable dt = DbHelperSQL.GET_DataTable_List(sql);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == dt.Rows.Count - 1)
                    {
                        str = str + dt.Rows[i]["AmtProid"].ToString();
                    }
                    else
                    {
                        str = str + dt.Rows[i]["AmtProid"].ToString() + ",";
                    }
                }
            }
            return str;
        }




        /// <summary>
        /// 同步更新用户金额
        /// </summary>
        /// <param name="retloan"></param>
        /// <param name="userid"></param>
        /// <param name="itype"> 0 用户id  1 UsrCustId </param>
        /// <returns></returns>
        public int DataSync(ReQueryBalanceBg retloan, string userid, int itype = 0)
        {
            int i = 0;
            string sql = "";
            //查询提现冻结金额
            sql = "select  COALESCE(sum(TransAmt),0) as TransAmt from hx_td_UserCash where OrdIdState=0 and OpenBankId is not null  and registerid=" + userid.ToString();
            decimal dec = decimal.Parse(DbHelperSQL.Re_String(sql));
            decimal AvlBal = decimal.Parse(retloan.AvlBal); //账户可用金额
            decimal FrzBal = decimal.Parse(retloan.FrzBal);  //账户冻结金额
            if (AvlBal >= dec)
            {
                AvlBal = AvlBal - dec;

                FrzBal = FrzBal + dec;
            }
            if (itype == 0)
            {
                sql = "update  hx_member_table  set  available_balance=" + AvlBal + " ,frozen_sum=" + FrzBal + " where  registerid=" + userid + "";
            }
            else
            {
                sql = "update  hx_member_table  set  available_balance=" + AvlBal + " ,frozen_sum=" + FrzBal + " where  UsrCustId=" + userid + "";


            }



            DbHelperSQL.RunSql(sql);

            return i;
        }

        /// <summary>
        /// 获取用户邀请关系
        /// </summary>
        /// <param name="useridentity">4渠道用户</param>
        /// <returns></returns>
        public string GetYQGX(int registerid, int useridentity)
        {
            string channel = "", sql = "", mobile = "";
            if (useridentity == 4)
            {
                sql = "SELECT Invpeopleid from  hx_td_Userinvitation where invpersonid=" + registerid;
                channel = DbHelperSQL.Re_String(sql);
                if (channel.Length > 0)
                {
                    sql = "select mobile from hx_member_table where registerid =" + channel;
                    mobile = DbHelperSQL.Re_String(sql);
                }

            }
            else
            {
                sql = "select hc.ChannelName from hx_Channel hc inner join hx_member_table mt on hc.Invitedcode = mt.channel_invitedcode where mt.registerid = " + registerid;
                mobile = DbHelperSQL.Re_String(sql);
            }
            return mobile;
        }
    }
}
