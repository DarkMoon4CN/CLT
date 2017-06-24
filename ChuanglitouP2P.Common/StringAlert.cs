using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Common
{
    public class StringAlert
    {
        #region 弹出提示框退回上一页 + string Alert(string str) 
        /// <summary>
        /// 弹出提示框退回上一页
        /// </summary>
        /// <param name="str">提示内容</param>
        /// <returns></returns>
        public static string Alert(string str)
        {
            return string.Format("<script>alert('" + str + "');javascript:history.back(-1);</script>");
        }
        #endregion


        #region 弹出提示框 转跳到指定页面 +string Alert(string str, string url)
        /// <summary>
        /// 弹出提示框 转跳到指定页面
        /// </summary>
        /// <param name="str">提示内容</param>
        /// <param name="url">转跳页面</param>
        /// <returns></returns>
        public static string Alert(string str, string url)
        {
            return string.Format("<script>alert('" + str + "');javascript:location.href = '" + url + "';</script>");

        }
        #endregion



        #region 弹出提示框 关闭当前页 +string AlertClose(string str) 
        /// <summary>
        /// 弹出提示框 关闭当前页
        /// </summary>
        /// <param name="str">提示内容</param>
        /// <returns></returns>
        public static string AlertClose(string str)
        {
            return string.Format("<script>alert('" + str + "');javascript:window.close();</script>");

        }
        #endregion
    }
}
