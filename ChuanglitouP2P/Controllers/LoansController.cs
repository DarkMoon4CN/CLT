using ChuanglitouP2P.BLL;
using ChuanglitouP2P.Common;
using ChuanglitouP2P.Model;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Controllers
{
    public class LoansController : Controller
    {
        // GET: Loans
        public ActionResult Index()
        {
            ViewBag.iscompany = DNTRequest.GetString("iscompany");


            return View();
        }

        /// <summary>
        /// 个人申请贷款
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PostPersonLoans()
        {
            string json = "";
            B_td_Myborrow b = new B_td_Myborrow();
            M_td_Myborrow p = new M_td_Myborrow();


            p.Username = DNTRequest.GetString("pUsername");
            p.userTel = DNTRequest.GetString("puserTel");
            p.BorrAMT = DNTRequest.GetString("pBorrAMT");
            p.Area = DNTRequest.GetString("pArea");
            p.BorrPurposes = DNTRequest.GetString("pBorrPurposes");
            p.TimeLimit = int.Parse(DNTRequest.GetString("pTimeLimit"));
            p.Mortgage = int.Parse(DNTRequest.GetString("pMortgage"));
            p.BorrType = 0;
            p.FoundingTime = DateTime.Now;
            if (b.Add(p) > 0)
            {
                json = @" {""rs""    : ""y"", ""url""      :  ""/""}";

            }
            else
            {
                json = @" {""rs""    : ""n"", ""error""      :  ""操作失败""}";
            }

            return Content(json,"text/json");
        }


        public ActionResult Borrloans()
        {
            string json = "";
            B_td_Myborrow b = new B_td_Myborrow();
            M_td_Myborrow p = new M_td_Myborrow();

            p.Username = DNTRequest.GetString("Username");
            p.userTel = DNTRequest.GetString("userTel");
            p.BorrPurposes = DNTRequest.GetString("BorrPurposes");
            p.CompName = DNTRequest.GetString("CompName");
            p.Industry = DNTRequest.GetString("Industry");
            p.RegCapital = DNTRequest.GetString("RegCapital");
            p.TimeLimit = int.Parse(DNTRequest.GetString("TimeLimit"));
            // p.Mortgage = int.Parse(DNTRequest.GetString("pMortgage"));
            p.FoundingTime = DateTime.Parse(DNTRequest.GetString("FoundingTime"));

            p.BorrAMT = DNTRequest.GetString("BorrAMT");
            p.Area = DNTRequest.GetString("pArea");
            p.BorrType = 1;

            if (b.Add(p) > 0)
            {
                json = @" {""rs""    : ""y"", ""url""      :  ""/""}";

            }
            else
            {
                json = @" {""rs""    : ""n"", ""error""      :  ""操作失败""}";
            }
            return Content(json, "text/json");
        }

        public ActionResult checkregister()
        {
          string  param = Utils.CheckSQLHtml(Request["param"]);//ToString()
            string strIdentify = "CheckCodeWeb"; //随机字串存储键值，以便存储到Session中
            if (Session[strIdentify] != null)
            {
                if (param == Session[strIdentify].ToString())
                {
                    return Content("y");
                }
                else
                {
                    return Content("验证码不对!");
                }

            }
            else
            {
                return Content("验证码已过期!");
            }
        }

        public ActionResult ImageValidate()
        {
            string strIdentify = "CheckCodeWeb"; //随机字串存储键值，以便存储到Session中

            string checkCode = Utils.RndNum(4);

            int iwidth = (int)(checkCode.Length * 13);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, 25);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //定义颜色
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //定义字体 
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            Random rand = new Random();
            //随机输出噪点
            for (int i = 0; i < 50; i++)
            {
                int x = rand.Next(image.Width);
                int y = rand.Next(image.Height);
                g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
            }
            //输出不同字体和颜色的验证码字符
            for (int i = 0; i < checkCode.Length; i++)
            {
                int cindex = rand.Next(7);
                int findex = rand.Next(5);
                Font f = new System.Drawing.Font(font[findex], 10, System.Drawing.FontStyle.Bold);
                Brush b = new System.Drawing.SolidBrush(c[cindex]);
                int ii = 4;
                if ((i + 1) % 2 == 0)
                {
                    ii = 2;
                }
                g.DrawString(checkCode.Substring(i, 1), f, b, 3 + (i * 12), ii);
            }
            //画一个边框
            g.DrawRectangle(new Pen(Color.Black, 0), 0, 0, image.Width - 1, image.Height - 1);

            //设置输出流图片格式
            //context.Response.ContentType = "image/gif";

            //输出到浏览器
            // System.IO.MemoryStream ms = new System.IO.MemoryStream();
            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Gif);
            // HttpContext.Current.Response.ClearContent();
            //Response.ClearContent();

            Session[strIdentify] = checkCode;

            return File(stream.ToArray(), "image/gif");
       }     
    }
}