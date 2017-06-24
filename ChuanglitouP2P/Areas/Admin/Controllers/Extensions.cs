using ChuanglitouP2P.Common;
using ChuanglitouP2P.Common.ExcelHelper;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public static class Extensions
    {
        /// <summary>
        /// 取得2个日期的间隔月份
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int GetMonthsByDates(this DateTime start, DateTime end)
        {
            return (end.Year - start.Year) * 12 + (end.Month - 1) - start.Month;
        }

        /// <summary>
        /// 用户提现状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string GetUserCashState(int state)
        {
            string stateName = "待审核 ";
            switch (state)
            {
                case 1:
                    stateName = "待付款";
                    break;
                case 3:
                    stateName = "已付款";
                    break;
                case 4:
                    stateName = "未通过";
                    break;
                default:
                    break;
            }
            return stateName;
        }

        #region 用户权限

        /// <summary>
        /// 左侧列表权限
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, List<hx_AdminLimitInfo>> GetUserLeftLimitByExt()
        {
            var userid = Utils.GetAdmUserID();
            if (userid < 1)
            {
                return null;
            }
            return new BLL.EF.UserLimitByEF().GetUserLeftLimitInfo(userid);

        }

        #endregion

        #region 导出Excel

        public static string ExportExcel(DataTable table, string fileName = "")
        {
            Random rd = new Random();
            int rd1 = rd.Next(111111, 999999);
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + rd1.ToString() + ".xls";
            }
            string filePath = HttpContext.Current.Server.MapPath("~\\Excel\\");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string path = filePath + fileName;
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                bool status = false;
                if (table.Rows.Count > 65536)
                {
                    path += "x";
                    status = ExcelHelper.OutExcel2007(table, path);
                }
                else
                {
                    status = ExcelHelper.SaveExcel(table, path);
                }
                string Redirectpath = "/Excel/" + path.Substring(path.LastIndexOf("\\") + 1);
                if (status)
                {
                    return Redirectpath;
                }
            }
            catch
            {

            }
            return "";
        }
        
        #endregion

        /// <summary>
        /// 身份证号辨别男女
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        public static string GetSexByIdNumber(string idNumber)
        {
            if (String.IsNullOrEmpty(idNumber))
            {
                return "";
            }
            var str = idNumber.Trim();
            string sex = "";
            int num = -1;
            if (str.Length == 15)
            {
                num = TypeParse.StrToInt(str.Substring(14, 1), -1);
            }
            else if (str.Length == 18)
            {
                num = TypeParse.StrToInt(str.Substring(16, 1), -1);
            }
            if (num > 0 && num % 2 == 1)
            {
                sex = "男";
            }
            else if (num > 0 && num % 2 == 0)
            {
                sex = "女";
            }
            return sex;
        }

        /// <summary>
        /// 获取格式化后的身份证
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        public static string GetFormatIdNumber(string idNumber, int start, int middle, int end)
        {
            if (string.IsNullOrEmpty(idNumber))
            {
                return "";
            }
            if (idNumber.Length < (start + end))
            {
                return idNumber;
            }
            return string.Format("{0}{1}{2}", idNumber.Substring(0, start), "".PadRight(middle, '*'), idNumber.Substring(idNumber.Length - end));

        }


    }
}