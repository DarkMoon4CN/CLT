using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChuanglitouP2P.topic._20160906
{
    public partial class Index : System.Web.UI.Page
    {
        public string rndstr = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            rndstr = Utils.RndNum(10).ToString();

            var code = Utils.CheckSQLHtml(DNTRequest.GetString("code"));

            if (!IsPostBack)
            {

                string sql = "select registerid,invitedcode from hx_member_table where invitedcode='" + code + "' ";

                DataTable dt = DbHelperSQL.GET_DataTable_List(sql);

                if (dt.Rows.Count > 0)
                {
                    // Session["invitedcode"] = code;
                    var ckes = DateTime.Now.AddDays(1);
                    HttpCookie cok = new HttpCookie("Invitation");

                    if (cok != null)
                    {
                        TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
                        cok.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在                           
                        Response.AppendCookie(cok);
                    }

                    cok.Domain = "chuanglitou.com";
                    cok.Values.Add("InvCode", DESEncrypt.Encrypt(code, ConfigurationManager.AppSettings["webp"].ToString()));
                    cok.Values.Add("CodeUid", DESEncrypt.Encrypt(dt.Rows[0]["registerid"].ToString(), ConfigurationManager.AppSettings["webp"].ToString()));
                    cok.Expires = ckes;
                    Response.AppendCookie(cok);
                }

                IntialBorrowingTarget();
            }
        }
        ///// <summary>
        ///// 累计为投资人赚取收益
        ///// </summary>
        ///// <returns></returns>
        //private string GetIncomeText()
        //{
        //    var chars = ChuanglitouP2P.BLL.B_usercenter.GetIncome().ToCharArray();
        //    //<p><span>1</span>,<span>2</span><span>3</span><span>4</span>,<span>5</span><span>6</span><span>7</span>,<span>8</span><span>9</span><span>4</span>.<span>0</span><span>0</span></p>
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append("<p>");
        //    foreach (char c in chars)
        //    {
        //        if (c == '.' || c == ',')
        //        {
        //            builder.Append(c.ToString());
        //        }
        //        else
        //        {
        //            builder.Append("<span>");
        //            builder.Append(c.ToString());
        //            builder.Append("</span>");
        //        }
        //    }
        //    builder.Append("</p>");
        //    return builder.ToString();
        //}
        /// <summary>
        /// 累计为投资人赚取收益
        /// </summary>
        /// <returns></returns>
        public static decimal GetTmpData()
        {
            return Convert.ToDecimal(B_usercenter.GetIncome());
        }
        /// <summary>
        /// 初始化页面显示的三个借款标数据
        /// </summary>
        private void IntialBorrowingTarget()
        {
            B_borrowing_target bllBorrowingTarget = new B_borrowing_target();
            List<M_borrowing_target_ZhuoLu> listData = new List<M_borrowing_target_ZhuoLu>();
            M_borrowing_target_ZhuoLu targetLifeSix = GetPartialTargetModel(6);
            if (targetLifeSix != null && targetLifeSix.targetid != 0)
                listData.Add(targetLifeSix);
            M_borrowing_target_ZhuoLu targetLifeThree = GetPartialTargetModel(3);
            if (targetLifeThree != null && targetLifeThree.targetid != 0)
                listData.Add(targetLifeThree);
            M_borrowing_target_ZhuoLu targetLifeOne = GetPartialTargetModel(1);
            if (targetLifeOne != null && targetLifeOne.targetid != 0)
                listData.Add(targetLifeOne);
            if (listData.Count < 3)//如果总共得到的标的数量小于3个，则补满三条数据，以使得页面显示不变形
            {
                for (int i = 1; i <= 3 - listData.Count; i++)
                {
                    listData.Add(listData[0]);
                }
            }
            StringBuilder targetLifeHtml = new StringBuilder();
            int itemIndex = 1;
            foreach (M_borrowing_target_ZhuoLu item in listData)
            {
                string percent = (item.fundraising_amount / item.borrowing_balance * 100M).ToString("0");
                targetLifeHtml.Append("<dl class=\"list-libao\">");
                targetLifeHtml.AppendFormat("<a href=\"{0}\">", "/invest_borrow_" + item.targetid + ".html");
                targetLifeHtml.AppendFormat("<dt class=\"libao{0}\">", itemIndex);
                targetLifeHtml.AppendFormat("<div class=\"libao{0}_jindu\">", itemIndex);
                targetLifeHtml.AppendFormat("<div><p style=\"width:{0}%;\"></p></div>", percent);
                targetLifeHtml.AppendFormat("<span>{0}%</span>", percent);
                targetLifeHtml.Append("</div>");
                targetLifeHtml.Append("<div class=\"libao1_main\">");
                targetLifeHtml.Append("<div class=\"libao1_main_1\">");
                targetLifeHtml.AppendFormat("<p class=\"libao1_main_1_biaoti\">{0}</p>", item.borrowing_title);
                targetLifeHtml.Append("</div>");
                targetLifeHtml.Append("<div class=\"libao1_main_2 libao1_diyi\">");
                targetLifeHtml.AppendFormat("<h3>{0}</h3><small>%</small>", item.annual_interest_rate.ToString("0.00"));
                targetLifeHtml.Append("</div>");
                targetLifeHtml.Append("<div class=\"libao1_main_3\">");
                targetLifeHtml.AppendFormat("<p><span>{0}</span>个月</p>", item.life_of_loan);
                targetLifeHtml.Append("</div></div></dt></a></dl>");
                itemIndex++;
            }
            ltrBorrowintTargets.Text = targetLifeHtml.ToString();
        }
        /// <summary>
        /// 根据期限获取借款标数据
        /// </summary>
        /// <param name="month">借款期限（1，3，6）</param>
        /// <returns></returns>
        private M_borrowing_target_ZhuoLu GetPartialTargetModel(int month)
        {
            B_borrowing_target bllBorrowingTarget = new B_borrowing_target();
            M_borrowing_target_ZhuoLu targetLife;
            M_borrowing_target target = bllBorrowingTarget.GetModel(Common.ConfigHelper.GetConfigInt("MonthTargetID_" + month));
            if (target != null && target.targetid != 0 && target.tender_state == 2 && target.end_time > DateTime.Now && target.fundraising_amount < target.borrowing_balance)//如果指定的标不存在，则使用规则查找符合要求的标
            {
                targetLife = new M_borrowing_target_ZhuoLu
                {
                    targetid = target.targetid,
                    annual_interest_rate = target.annual_interest_rate,
                    borrowing_balance = target.borrowing_balance,
                    borrowing_title = target.borrowing_title,
                    fundraising_amount = target.fundraising_amount,
                    life_of_loan = target.life_of_loan
                };
            }
            else
            {
                targetLife = bllBorrowingTarget.GetModelByLifeLoan(month);
            }
            return targetLife;
        }
    }
}