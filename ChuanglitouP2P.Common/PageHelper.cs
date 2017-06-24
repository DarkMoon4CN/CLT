#region 描述信息
/*-------------------------------------------------------------------------
 * <copyright>PageHelper ©2012 XieZhihui</copyright>
 * <author>XieZhihui<author>
 *<createdOn>2012/12/22 22:23:57</createdOn>
 * <ver>v1.0</ver>
 *  -------------------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace ChuanglitouP2P.Common
{
    public static partial class PageHelper
    {

        #region 分页
        public static string Paging(this AjaxHelper ajaxHelper, string pageParam,

             string controllerName, string actionName, int pageSize, int recordCount, int pageCount, object routValues, AjaxOptions ajaxOptions)
        {
            return Paging(ajaxHelper, pageParam, pageSize, recordCount, pageCount, actionName, controllerName, routValues, ajaxOptions);
        }
        public static string Paging(this AjaxHelper ajaxHelper, string pageParam, int pageSize, int recordCount, int pageCount, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions)
        {
            var queryString = ajaxHelper.ViewContext.HttpContext.Request.QueryString;
            var outputPager = new StringBuilder("");
            var currentPage = 1;

            var dict = new RouteValueDictionary(routeValues);
            if (string.IsNullOrEmpty(queryString[pageParam]))
            {
                //获取 ～/Page/{page number} 的页号参数
                if (dict[pageParam] != null)
                    int.TryParse(dict[pageParam].ToString(), out currentPage);
            }
            else
            {
                //与相应的QueryString绑定 
                foreach (string key in queryString.Keys)
                    if (!string.IsNullOrEmpty(key) && queryString[key] != null)
                        dict[key] = queryString[key];
                int.TryParse(queryString[pageParam], out currentPage);
            }
            var start = (currentPage - 1) * pageSize + 1;
            if (start < 1) start = 1;
            var end = start + pageSize - 1;
            if (end > recordCount) end = recordCount;

            //处理分页
            const int numbersInMiddle = 10;
            var sideNumber = (int)Math.Floor(10.0 / 2);
            if (!dict.ContainsKey("k") && queryString["k"] != null)
                dict.Add("k", queryString["k"]);
            if (currentPage > 1)
            {
                //处理首页连接
                dict[pageParam] = 1;
                outputPager.AppendFormat("{0} ",
                                ajaxHelper.ActionLink("<", actionName, controllerName, dict, ajaxOptions, new Dictionary<string, object> { { "class", "page_first" } }).ToString().Replace("&lt;", "&nbsp;"));
                //处理上一页的连接  
                dict[pageParam] = currentPage - 1;
                outputPager.AppendFormat("{0}",
                                 ajaxHelper.ActionLink("<", actionName, controllerName, dict, ajaxOptions, new Dictionary<string, object> { { "class", "page_Iprev" } }).ToString().Replace("&lt;", "&nbsp;"));

            }

            //处理中间显示的页码                
            for (var i = 0; i < numbersInMiddle; i++)
            {
                //一共最多显示10个页码，前面5个，后面5个  
                if ((currentPage + i - sideNumber) >= 1 && (currentPage + i - sideNumber) <= pageCount)
                {
                    if (sideNumber == i)
                    {
                        //当前页处理 
                        dict[pageParam] = currentPage + i - sideNumber;
                        outputPager.AppendFormat("{0}",
                             ajaxHelper.ActionLink((currentPage + i - sideNumber).ToString(CultureInfo.InvariantCulture), actionName,
                                                            controllerName, dict, ajaxOptions, new Dictionary<string, object> { { "class", "on" } }));
                    }
                    else
                    {
                        //一般页处理 
                        dict[pageParam] = currentPage + i - sideNumber;
                        outputPager.AppendFormat("{0}",
                             ajaxHelper.ActionLink((currentPage + i - sideNumber).ToString(CultureInfo.InvariantCulture), actionName,
                                                            controllerName, dict, ajaxOptions, new Dictionary<string, object> { { "class", "" } }));
                    }
                    outputPager.Append("|");
                }
            }
            if (outputPager.Length > 19)
                outputPager.Remove(outputPager.Length - 1, 1);
            if (currentPage < pageCount)
            {
                //处理下一页的链接 
                dict[pageParam] = currentPage + 1;
                outputPager.AppendFormat("{0} ",
                             ajaxHelper.ActionLink("<", actionName, controllerName, dict, ajaxOptions, new Dictionary<string, object> { { "class", "page_last" } }).ToString().Replace("&lt;", "&nbsp;"));
                //处理最后一页连接  
                dict[pageParam] = pageCount;
                outputPager.AppendFormat("{0}",
                            ajaxHelper.ActionLink("<", actionName, controllerName, dict, ajaxOptions, new Dictionary<string, object> { { "class", "page_Inext" } }).ToString().Replace("&lt;", "&nbsp;"));
            }
            outputPager.Append("<div class='cl'></div>");
            var replace = "p=" + currentPage;
            var repValue = replace + "&" + queryString;
            if (!dict.ContainsKey("k") && queryString["k"] != null)
            {
                return outputPager.Replace(replace, repValue).ToString();
            }

            return outputPager.ToString();
        }

        public static string NormalPaging(this AjaxHelper ajaxHelper, int ps, int rc, int cp, int pc)
        {

            var outputPager = new StringBuilder("");
            //处理分页
            const int numbersInMiddle = 10;
            var sideNumber = (int)Math.Floor(10.0 / 2);
            var enb = " disabled='disabled'";
            if (cp <= 1)
            {
                outputPager.AppendFormat(
                    "<a href='javascript:void(0);' class='page_first' {0} cmd='fst'><i class='first grey'></i></a><a href='javascript:void(0);' class='page_prev' cmd='prev' {0}><i class='prev grey'></i></a>", enb);
            }

            if (cp > 1)
            {
                outputPager.AppendFormat(
                  "<a href='javascript:void(0);' class='page_first' {0} cmd='fst'><i class='first'></i></a><a href='javascript:void(0);' class='page_prev' cmd='prev' {0}><i class='prev'></i></a>", "");

            }

            //处理中间显示的页码                
            for (var i = 0; i < numbersInMiddle; i++)
            {
                //一共最多显示10个页码，前面5个，后面5个  
                if ((cp + i - sideNumber) >= 1 && (cp + i - sideNumber) <= pc)
                {
                    if (sideNumber == i)
                    {
                        //当前页处理 

                        outputPager.Append(
                   "<a href='javascript:void(0);' class='on'>" + (cp + i - sideNumber) + "</a>");

                    }
                    else
                    {
                        //一般页处理 
                        outputPager.Append(
                   "<a href='javascript:void(0);' cmd='pg'>" + (cp + i - sideNumber) + "</a>");
                    }
                }
            }
            if (cp < pc)
            {
                outputPager.AppendFormat("<a href='javascript:void(0);' class='page_next' cmd='nxt'><i class='next'></i></a><a href='javascript:void(0);' class='page_last' cmd='end'><i class='last'></i></a>");

            }
            if (cp == pc)
            {
                outputPager.AppendFormat("<a href='javascript:void(0);' {0} class='page_next'  cmd='nxt'><i class='next grey'></i></a><a href='javascript:void(0);'  class='page_last' {0} cmd='end'><i class='last grey'></i></a>", enb);

            }

            return outputPager.ToString();
        }
        public static string UlPaging(this AjaxHelper ajaxHelper, int ps, int rc, int cp, int pc)
        {
            var outputPager = new StringBuilder("");
            //处理分页
            const int numbersInMiddle = 10; outputPager.AppendFormat(
                   "<li>共有{0}条记录</li>", rc);
            if (rc > 0)
            {
                var sideNumber = (int)Math.Floor(10.0 / 2);
                var enb = "class='disabled' disabled='disabled'";
                if (cp == 1)
                {
                    outputPager.AppendFormat(
                        "<li {0} cmd='fst'><a href='javascript:void(0);'>首页</a></li><li {0} cmd='prev'><a href='javascript:void(0);'>上一页</a></li>", enb);
                }

                if (cp > 1)
                {
                    outputPager.Append(
                       "<li cmd='fst'><a href='javascript:void(0);'>首页</a></li><li cmd='prev'><a href='javascript:void(0);'>上一页</a></li>");

                }

                //处理中间显示的页码                
                for (var i = 0; i < numbersInMiddle; i++)
                {
                    //一共最多显示10个页码，前面5个，后面5个  
                    if ((cp + i - sideNumber) >= 1 && (cp + i - sideNumber) <= pc)
                    {
                        if (sideNumber == i)
                        {
                            //当前页处理 

                            outputPager.Append(
                       "<li class='curr_page'><a href='javascript:void(0);' class='selected'>" + (cp + i - sideNumber) + "</a></li>");

                        }
                        else
                        {
                            //一般页处理 
                            outputPager.Append(
                       "<li cmd='pg'><a href='javascript:void(0);'>" + (cp + i - sideNumber) + "</a></li>");
                        }
                    }
                }
                if (cp < pc)
                {
                    outputPager.AppendFormat("<li cmd='nxt'><a href='javascript:void(0);'>下一页</a></li><li cmd='end'><a href='javascript:void(0);'>尾页</a></li>");

                }
                if (cp == pc)
                {
                    outputPager.AppendFormat("<li {0} cmd='nxt'><a href='javascript:void(0);'>下一页</a></li><li {0} cmd='end'><a href='javascript:void(0);'>尾页</a></li>", enb);

                }

            }

            return outputPager.ToString();
        }
        //<div class="pagebar">
        //            <span class="page">
        //                <span class="info">第 1/5 页</span>
        //                <a href="" class="ban">前一页</a>
        //                <a href="">下一页</a>
        //           </span>  </div>

        /// <summary>
        /// Divs the paging.
        /// </summary>
        /// <param name="ajaxHelper">The ajax helper.</param>
        /// <param name="ps">page size.</param>
        /// <param name="rc">record count.</param>
        /// <param name="cp">current page.</param>
        /// <param name="pc">page count.</param>
        /// <returns>System.String.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-06-20 18:57:42
        public static string DivPaging(this AjaxHelper ajaxHelper, int ps, int rc, int cp, int pc, string url)
        {

            //    <div class="pagebox">


            //              <span>共69页&nbsp;共687条&nbsp;</span> <span class="previous">上一页</span>

            //<a href="javascript:void(0);" style="background: #079DEA;color:#fff;">1</a><a href="CLT_industry_news.html?page=2">2</a> <a href="CLT_industry_news.html?page=3">3</a> <a href="CLT_industry_news.html?page=4">4</a> <a href="CLT_industry_news.html?page=5">5</a> <a href="CLT_industry_news.html?page=6">6</a> <span>&nbsp;...&nbsp;</span><a href="CLT_industry_news.html?page=69">69</a><a href="CLT_industry_news.html?page=2">下一页</a>  
            //</div>



            var outputPager = new StringBuilder("");
            //处理分页
            const int numbersInMiddle = 10;

            outputPager.AppendFormat(
                   "<span>共{1}页 &nbsp;共有{0}条记录</span>", rc, pc);
            if (rc > 0)
            {
                var sideNumber = (int)Math.Floor(10.0 / 2);
                var enb = "class='disabled' disabled='disabled'";
                if (cp == 1)
                {
                    //outputPager.AppendFormat(
                    //    "<span class=\"previous\" {0} cmd='fst'>首页</span>" +
                    //    "<span class=\"previous\" {0} cmd='prev' href='javascript:void(0);'>上一页</span>", enb); 

                    outputPager.AppendFormat("<span class=\"previous\" {0} cmd='prev' >上一页</span>", enb);
                }

                if (cp > 1)
                {

                    outputPager.Append(
                     "<a href='" + url + "?p=" + (cp - 1) + "' >上一页</a>");
                }

                //处理中间显示的页码                
                for (var i = 0; i < numbersInMiddle; i++)
                {
                    //一共最多显示10个页码，前面5个，后面5个  
                    if ((cp + i - sideNumber) >= 1 && (cp + i - sideNumber) <= pc)
                    {
                        if (sideNumber == i)
                        {
                            //当前页处理 

                            outputPager.Append(
                       "<a href='" + url + "?p=" + (cp + i - sideNumber) + "' style='background: #079DEA;color:#fff;'>" + (cp + i - sideNumber) + "</a>");

                        }
                        else
                        {
                            //一般页处理 
                            outputPager.Append(
                       "<a  href='" + url + "?p=" + (cp + i - sideNumber) + "'>" + (cp + i - sideNumber) + "</a>");
                        }
                    }
                }
                if (cp < pc)
                {
                    outputPager.AppendFormat("<a cmd='nxt'  href='" + url + "?p=" + (cp + 1) + "' >下一页</a>");

                }
                if (cp == pc)
                {
                    outputPager.AppendFormat("<span class='previous'>下一页</span>");

                }

            }

            return outputPager.ToString();
        }

        public static string AccountPaging(this AjaxHelper ajaxHelper, int ps, int rc, int cp, int pc)
        {
            var outputPager = new StringBuilder("");
            //处理分页
            const int numbersInMiddle = 10; outputPager.AppendFormat(
                   " <ul class='page_nav inline_block'><li>共有{0}条记录 | </li>", rc);
            if (rc > 0)
            {
                var sideNumber = (int)Math.Floor(10.0 / 2);
                var enb = "class='prev_page disabled' disabled='disabled'";
                if (cp == 1)
                {
                    outputPager.AppendFormat(
                        "<li {0}><a href='javascript:void(0);'  disabled='disabled' cmd='fst'>首页</a></li><li class='disabled'><a href='javascript:void(0);' disabled='disabled' cmd='prev'>上一页</a></li>", enb);
                }

                if (cp > 1)
                {
                    outputPager.Append(
                       "<li class='prev_page'><a href='javascript:void(0);' cmd='fst' >首页</a></li><li><a href='javascript:void(0);' cmd='prev'>上一页</a></li>");

                }

                //处理中间显示的页码                
                for (var i = 0; i < numbersInMiddle; i++)
                {
                    //一共最多显示10个页码，前面5个，后面5个  
                    if ((cp + i - sideNumber) >= 1 && (cp + i - sideNumber) <= pc)
                    {
                        if (sideNumber == i)
                        {
                            //当前页处理 

                            outputPager.Append(
                       "<li><a href='javascript:void(0);' class='selected'>" + (cp + i - sideNumber) + "</a></li>");

                        }
                        else
                        {
                            //一般页处理 
                            outputPager.Append(
                       "<li><a href='javascript:void(0);'  cmd='pg'>" + (cp + i - sideNumber) + "</a></li>");
                        }
                    }
                }
                if (cp < pc)
                {
                    outputPager.AppendFormat("<li><a href='javascript:void(0);' cmd='nxt'>下一页</a></li><li class='next_page' ><a  cmd='end' href='javascript:void(0);'>尾页</a></li>");

                }
                if (cp == pc)
                {
                    outputPager.AppendFormat("<li  class='disabled'><a   disabled='disabled' cmd='nxt' href='javascript:void(0);'>下一页</a></li><li class='next_page disabled'><a disabled='disabled' cmd='end' href='javascript:void(0);'>尾页</a></li>");

                }

            }
            outputPager.AppendFormat("</ul>");

            return outputPager.ToString();
        }



        #endregion

        /// <summary>
        /// 获取汇付返回的用户号如cltwl_zxjtest001分格出_后半部分
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetUserSplit(string str)
        {
            string str1 = "";
            string[] us = str.Split('_', '）');

            if (us.Length >= 2)
            {
                str1 = us[1];
            }
            return str1;
        }

        /// <summary>
        /// 返回支付的返回代码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetReturnCode(Int32 str)
        {
            EnumCode typef = new EnumCode();
            typef = (EnumCode)Enum.ToObject(typeof(EnumCode), str);
            return Enum.GetName(typeof(EnumCode), str);
        }
    }
}
