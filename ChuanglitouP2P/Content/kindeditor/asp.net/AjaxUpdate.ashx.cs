using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ChuanglitouP2P
{
    /// <summary>
    /// AjaxUpdate 的摘要说明
    /// </summary>
    public class AjaxUpdate : IHttpHandler
    {
        /// 增加文章点击量
        public void ProcessRequest(HttpContext context)
        {
            int id = Convert.ToInt32(context.Request.QueryString["id"]);
            if (id != 0)
            {
                chuangtouEntities TF = new chuangtouEntities();
                hx_td_about_news ha = TF.hx_td_about_news.Where(p => p.newid == id).FirstOrDefault();
                if (ha != null)
                {
                    ha.ClickCount = (ha.ClickCount == null ? 0 : ha.ClickCount) + 1;
                    TF.SaveChanges();
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}