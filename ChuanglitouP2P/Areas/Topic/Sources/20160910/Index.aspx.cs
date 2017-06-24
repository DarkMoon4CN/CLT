using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChuanglitouP2P.topic._20160910
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bll.B_GrabIphone gi = new Bll.B_GrabIphone();
                ltrCurrentCount.Text = gi.GetRecordCount("LuckDrawState=0") + "";
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public void zf()
        {
            B_member_table bmt = new B_member_table();
            M_member_table mmt = new M_member_table();

            mmt = bmt.GetModel(42620);
            #region 投资六月专享标一元抢Iphone 2016年9月11日 9点 至2016年10月31日
            if (true)
            {
                DateTime nowdate = DateTime.Now;
                //DateTime startdate = new DateTime(2016, 09, 10, 9, 00, 00);
                //DateTime enddate = new DateTime(2016, 10, 31, 23, 59, 59);2016-09-07 17:42:59.430
                DateTime startdate = new DateTime(2016, 09, 11, 9, 00, 00);
                DateTime enddate = new DateTime(2016, 10, 31, 23, 59, 59);
                string log = "一元抢Iphone";
                if (nowdate > startdate && nowdate < enddate)
                {

                    DateTime dt = mmt.Registration_time;
                    log += " 用户注册时间：" + dt.ToString();
                    if (dt > new DateTime(2016, 09, 11, 0, 0, 0))
                    {


                        //if (dt.Rows[0]["unit_day"].ToString() == "1" && dt.Rows[0]["life_of_loan"].ToString() == "6")//是否六月标
                        //{

                        Bll.B_GrabIphone gi = new Bll.B_GrabIphone();

                        bool isCount = gi.Exists(mmt.registerid);//查询是否存在该用户
                        log += "; 用户ID：" + mmt.registerid + ";查询是否存在该用户:" + isCount;
                        if (isCount != true)
                        {
                            M_GrabIphone model = new M_GrabIphone();
                            model.RegrsterID = mmt.registerid;
                            model.Color = "";
                            model.Addtime = nowdate;
                            model.LuckDrawState = 0;
                            model.WinningState = 0;
                            model.WinningTime = nowdate;
                            model.TargetID = 232;// int.Parse(dt.Rows[0]["targetid"].ToString());
                            model.BidRecordsID = 3333;// int.Parse(dt.Rows[0]["bid_records_id"].ToString());
                            model.InvestmentAmount = "22";//dt.Rows[0]["investment_amount"].ToString();
                            gi.Add(model); //增加一条数据

                            List<M_GrabIphone> giList = gi.GetModelList(5388, "LuckDrawState=0", "ID");//获取当前阶段投资人数
                            if (giList != null)
                            {
                                log += "; 当前阶段投资人数:" + giList.Count;
                                int ljcount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["GrabIphone"].ToString());//获取启动抽奖人数
                                if (giList.Count >= ljcount)
                                {
                                    bool bo = gi.UpdateLuckDrawState();//批量更新抽奖状态
                                    log += "; 批量更新抽奖状态:" + bo;
                                    if (bo == true)
                                    {
                                        int count = giList.Count;
                                        int index = new Random().Next(count);
                                        M_GrabIphone randowitem = giList[index];

                                        if (randowitem != null)
                                        {
                                            log += "; 获奖用户ID:" + randowitem.RegrsterID;
                                            bool co = gi.Update("", 1, DateTime.Now, randowitem.RegrsterID);//更新中奖用户状态
                                            log += "; 更新中奖用户状态:" + co;
                                        }
                                    }
                                }
                            }
                        }
                        // }
                    }
                    Common.LogInfo.WriteLog(log);//写入日志
                }
            }
            #endregion
        }
        /// <summary>
        /// 获取当前阶段投资人数
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public double getLDCount()
        {
            Bll.B_GrabIphone gi = new Bll.B_GrabIphone();
            double count = Convert.ToDouble(gi.GetRecordCount("LuckDrawState=0"));
            int ljcount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["GrabIphone"].ToString());
            return (count / ljcount);
        }
        /// <summary>
        /// 获取累计加入人数
        /// </summary>
        /// <returns></returns>
        public double getLJJRCount()
        {
            Bll.B_GrabIphone gi = new Bll.B_GrabIphone();
            int count = gi.GetRecordCount("");
            return count;
        }
        /// <summary>
        /// 已参与用户
        /// </summary>
        /// <returns></returns>
        public List<M_GrabIphone> getCYLIst()
        {
            Bll.B_GrabIphone gi = new Bll.B_GrabIphone();
            List<M_GrabIphone> giList = gi.GetModelList(30, "", "ID desc");
            return giList;
        }
        /// <summary>
        /// 已中奖用户
        /// </summary>
        /// <returns></returns>
        public List<M_GrabIphone> getZJLIst()
        {
            Bll.B_GrabIphone gi = new Bll.B_GrabIphone();
            List<M_GrabIphone> giList = gi.GetModelList(20, "WinningState=1", "ID desc");
            return giList;
        }

        /// <summary>
        /// 根据用户ID获取用户信息
        /// </summary>
        /// <param name="registerid"></param>
        /// <returns></returns>
        public M_member_table getMemberInfo(int registerid)
        {
            B_member_table bmt = new B_member_table();
            M_member_table mmt = bmt.GetModel(registerid);
            return mmt;
        }
        /// <summary>
        /// 获取用户省信息
        /// </summary>
        /// <returns></returns>
        public static Hashtable GetHashtable()
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("11", "北京市");
            hashtable.Add("12", "天津市");
            hashtable.Add("13", "河北省");
            hashtable.Add("14", "山西省");
            hashtable.Add("15", "内蒙古自治区");
            hashtable.Add("21", "辽宁省");
            hashtable.Add("22", "吉林省");
            hashtable.Add("23", "黑龙江省");
            hashtable.Add("31", "上海市");
            hashtable.Add("32", "江苏省");
            hashtable.Add("33", "浙江省");
            hashtable.Add("34", "安徽省");
            hashtable.Add("35", "福建省");
            hashtable.Add("36", "江西省");
            hashtable.Add("37", "山东省");
            hashtable.Add("41", "河南省");
            hashtable.Add("42", "湖北省");
            hashtable.Add("43", "湖南省");
            hashtable.Add("44", "广东省");
            hashtable.Add("45", "广西壮族自治区");
            hashtable.Add("46", "海南省");
            hashtable.Add("50", "重庆市");
            hashtable.Add("51", "四川省");
            hashtable.Add("52", "贵州省");
            hashtable.Add("53", "云南省");
            hashtable.Add("54", "西藏自治区");
            hashtable.Add("61", "陕西省");
            hashtable.Add("62", "甘肃省");
            hashtable.Add("63", "青海省");
            hashtable.Add("64", "宁夏回族自治区");
            hashtable.Add("65", "新疆维吾尔自治区");
            return hashtable;
        }
    }
}