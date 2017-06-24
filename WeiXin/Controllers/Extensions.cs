using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeiXin.Controllers
{

    public class Extensions
    {
        /// <summary>
        /// 获取首页友情链接图片 和文字链接  0 文字  1图片
        /// </summary>
        /// <param name="ttype">0 文字  1图片</param>
        /// <returns></returns>
        public static string GetLinks(int ttype, int top)
        {
            return ChuanglitouP2P.Common.Utils.GetLinks(ttype, top);
        }

        /// <summary>
        /// 累计投资人数
        /// </summary>
        /// <returns></returns>
        public static string GetInvestment()
        {
            return ChuanglitouP2P.BLL.B_usercenter.GetInvestment();
        }

        /// <summary>
        /// 累计帮助企业和个人融资
        /// </summary>
        /// <returns></returns>
        public static string GetALLFinance()
        {
            return ChuanglitouP2P.BLL.B_usercenter.GetALLFinance();
        }

        /// <summary>
        /// 获取广告
        /// </summary>
        /// <param name="adtypid"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public static string GetWebAD(int adtypid, int top)
        {
            return ChuanglitouP2P.Common.Utils.GetWebAD(adtypid, top);
        }

        /// <summary>
        /// 广告圆点
        /// </summary>
        /// <param name="adtypid"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public static string IndexWebAd(int adtypid, int top)
        {
            return ChuanglitouP2P.Common.Utils.IndexWebAd(adtypid, top);
        }
    }
}