using ChuanglitouP2P.Bll;
using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.DBUtility;
using ChuanglitouP2P.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CWX.activity._20160914
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
                    #region modified by fangjianmin 该段代码可能导致客户端浏览器清理cookie
                    //if (cok != null)
                    //{
                    //    TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
                    //    cok.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在                           
                    //    Response.AppendCookie(cok);
                    //}
                    #endregion
                    cok.Domain = PublicURL.NewUrl;
                    cok.Values.Add("InvCode", DESEncrypt.Encrypt(code, ConfigurationManager.AppSettings["webp"].ToString()));
                    cok.Values.Add("CodeUid", DESEncrypt.Encrypt(dt.Rows[0]["registerid"].ToString(), ConfigurationManager.AppSettings["webp"].ToString()));
                    cok.Expires = ckes;
                    Response.AppendCookie(cok);
                }
                            }
        }
        /// <summary>
        /// 获取累计加入人数
        /// </summary>
        /// <returns></returns>
        public double getLJJRCount()
        {
            B_GrabIphone gi = new B_GrabIphone();
            int count = gi.GetRecordCount("");
            return count;
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
        /// 已参与用户
        /// </summary>
        /// <returns></returns>
        public List<M_GrabIphone> getCYLIst()
        {
            B_GrabIphone gi = new B_GrabIphone();
            List<M_GrabIphone> giList = gi.GetModelList(30, "", "ID desc");
            return giList;
        }
    }
}