using ChuanglitouP2P.Model.chinapnr.ChinapnrAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Common.chinapnr
{
    /// <summary>
    ///  ChinapnrFormHelper  CreateForm:创建表单,CreateWaitScriptString:创建等待脚本,GetChkValue:组装加签字符串
    /// </summary>
    public class CFormHelper
    {
        /// <summary>
        /// 创建一个表单
        /// </summary>
        /// <typeparam name="T">需要转换的实体</typeparam>
        /// <param name="entity">需要转换的实体</param>
        /// <param name="action">跳转的地址</param>
        /// <param name="formName">表单名</param>
        /// <param name="method">post 或 get</param>
        /// <returns></returns>
        public static string CreateForm<T>(T entity, string action,string formName = "form1", string method = "post")
        {
            StringBuilder builder = new StringBuilder(265);
            builder.AppendFormat("<form id=\"{0}\" action=\"{1}\" method=\"{2}\">", formName, action, method);
            builder.Append("<div>");
            System.Reflection.PropertyInfo[] properties = entity.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (properties.Length > 0)
            {
                foreach (System.Reflection.PropertyInfo item in properties)
                {
                    builder.AppendFormat("<input id = \"{0}\" name =\"{1}\" value = \"{2}\" type = \"hidden\" />", item.Name, item.Name, item.GetValue(entity, null));
                }
            }
            builder.Append("</div>");
            builder.Append("</form>");
            builder.AppendFormat("<script type =\"text/javascript\">$(\"#{0}\").submit();</script>", formName);
            return builder.ToString();
        }

        /// <summary>
        ///  按 ChkValueSort  Attribute 设置规则,进行组装加签字符串
        /// </summary>
        /// <typeparam name="T">需要设置的实体</typeparam>
        /// <param name="entity">需要设置的实体</param>
        ///  <param name="mode">0:ChkValueSortAttribute所标记顺序组装加签  1:直接按类属性顺序进行组装加签</param>
        /// <returns></returns>
        public static string GetChkValue<T>(T entity,int mode=0)
        {
            //结果字符串
            StringBuilder chkVal = new StringBuilder(256);
            System.Reflection.PropertyInfo[] properties = entity.GetType().GetProperties();
            if (mode == 0)
            {
                //数据集合
                SortedDictionary<int, string> dict = new SortedDictionary<int, string>();
                for (int i = 0; i < properties.Length; i++)
                {
                    ChkValueSort flag = properties[i].GetCustomAttribute<ChkValueSort>();
                    if (flag != null)
                    {
                        dict.Add(flag.Index, properties[i].GetValue(entity, null).ToString());
                    }
                }
                foreach (KeyValuePair<int, string> item in dict)
                {
                    chkVal.Append(item.Value);
                }
            }
            else if (mode == 1)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    if (properties[i].Name.ToLower() != "ChkValue".ToLower())
                    {
                        chkVal.Append(properties[i].GetValue(entity, null).ToString());
                    }
                }
            }
            return chkVal.ToString();
        }

        /// <summary>
        /// 生成表单之前是否需要 加入等待Script
        /// </summary>
        /// <returns></returns>
        public static string CreateWaitScriptString()
        {
            string str = string.Empty;
            str += "<script type = \"text /javascript\" src =\"/Scripts/jquery-1.9.1.min.js\"></script>";
            str += "<script src=\"/Scripts/layer/layer.min.js\" type=\"text/javascript\" charset=\"utf- 8\"></script>";
            str += "<script type=\"text/javascript\">$(function() {layer.msg(\"数据处理中....！请不要关闭\", 60, 1);});</script > ";
            return str;
        }
    }
}
