using ChuanglitouP2P.Common;
using ChuangLitouP2P.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChuanglitouP2P.Areas.Admin.Controllers
{
    public class UploadFileController : Controller
    {
        chuangtouEntities ef = new chuangtouEntities();

        // GET: Admin/UploadFile
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ProductImgTest(int targetid, int registerid, int tp, string fname, string rdm)
        {
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string imgPath = "";
            string PhysicalPath = "";
            string json = "{\"ret\":0,\"msg\":\"上传图片失败！\"}";
            string path2 = "/ProductImg/UploadFiles/";
            string fileName = "";
            if (hfc.Count > 0)
            {
                var httpfile = hfc[0];
                string suf = "";
                if (!isImage(httpfile.FileName, out suf))
                {
                    json = "{\"ret\":0,\"msg\":\"图片只支持－－－gif|jpg|jpeg|bmp！\"}";
                    return Content(json, "text/json");
                }
                fileName = string.Format("{0}.{1}", sjname() + rdm, suf);
                imgPath = @"/ProductImg/UploadFiles/";
                var ss = httpfile.ContentType;
                PhysicalPath = Server.MapPath(imgPath);
                if (!System.IO.Directory.Exists(PhysicalPath))
                {
                    System.IO.Directory.CreateDirectory(PhysicalPath);
                }

                PhysicalPath = PhysicalPath + fileName;
                httpfile.SaveAs(PhysicalPath);



                imgPath = imgPath + fileName;

                if (Utils.CheckPictureSafe(PhysicalPath))
                {
                    if (!string.IsNullOrEmpty(imgPath))
                    {
                        hx_borrower_guarantor_picture p = new hx_borrower_guarantor_picture();
                        p.targetid = targetid;
                        p.borrower_registerid = registerid;
                        p.picture_name = Utils.CheckSQLHtml(fname);
                        p.type_picture = tp;
                        p.picture_path = path2 + fileName;
                        p.uploadtime = DateTime.Now;
                        ef.hx_borrower_guarantor_picture.Add(p);
                        ef.SaveChanges();

                        imgPath = imgPath.Replace("/", "//");
                        json = "{\"ret\":1,\"path\":\"" + imgPath + "\",\"key\":" + p.borrower_guarantor_picture_id + "}";
                    }
                }
                else
                {   //图片安全提醒，您试除上传非法文件
                    json = "{\"ret\":0,\"path\":\"图片中含有非法文件\"}";
                }
            }
            return Content(json, "text/json");
        }


        /// <summary>
        ///  兼容 ProductImgTest    tag用于排序
        /// </summary>
        /// <param name="targetid"></param>
        /// <param name="registerid"></param>
        /// <param name="tp"></param>
        /// <param name="fname"></param>
        /// <param name="rdm"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public ActionResult UploadImage(int targetid, int registerid, int tp, string fname, string rdm, int tag)
        {
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string imgPath = "";
            string PhysicalPath = "";
            string json = "{\"ret\":0,\"msg\":\"上传图片失败！\"}";
            string path2 = "/ProductImg/UploadFiles/";
            string fileName = "";
            if (hfc.Count > 0)
            {
                var httpfile = hfc[0];
                string suf = "";
                if (!isImage(httpfile.FileName, out suf))
                {
                    json = "{\"ret\":0,\"msg\":\"图片只支持－－－gif|jpg|jpeg|bmp！\"}";
                    return Content(json, "text/json");
                }
                fileName = string.Format("{0}.{1}", sjname() + rdm, suf);
                imgPath = @"/ProductImg/UploadFiles/";
                var ss = httpfile.ContentType;
                PhysicalPath = Server.MapPath(imgPath);
                if (!System.IO.Directory.Exists(PhysicalPath))
                {
                    System.IO.Directory.CreateDirectory(PhysicalPath);
                }

                PhysicalPath = PhysicalPath + fileName;
                httpfile.SaveAs(PhysicalPath);



                imgPath = imgPath + fileName;

                if (Utils.CheckPictureSafe(PhysicalPath))
                {
                    if (!string.IsNullOrEmpty(imgPath))
                    {
                        hx_borrower_guarantor_picture p = new hx_borrower_guarantor_picture();
                        p.targetid = targetid;
                        p.borrower_registerid = registerid;
                        p.picture_name = Utils.CheckSQLHtml(fname);
                        p.type_picture = tp;
                        p.picture_index = tag;
                        p.picture_path = path2 + fileName;
                        p.uploadtime = DateTime.Now;
                        ef.hx_borrower_guarantor_picture.Add(p);
                        ef.SaveChanges();

                        imgPath = imgPath.Replace("/", "//");
                        json = "{\"ret\":1,\"path\":\"" + imgPath + "\",\"key\":" + p.borrower_guarantor_picture_id + "}";
                    }
                }
                else
                {   //图片安全提醒，您试除上传非法文件
                    json = "{\"ret\":0,\"path\":\"图片中含有非法文件\"}";
                }
            }
            return Content(json, "text/json");
        }


        /// <summary>
        /// 基础材料
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductImg(int targetid, int registerid, int tp)
        {
            string savePath = Server.MapPath("\\ProductImg\\UploadFiles") + "\\";//上传文件保存路径 
            string path2 = "/ProductImg/UploadFiles/";
            string files = Request["UploadFile"];
            HttpFileCollection uploadFiles = System.Web.HttpContext.Current.Request.Files;

            string filename1 = DNTRequest.GetString("filename");
            string[] strArray = filename1.Split(new char[] { ',' });
            string filename;//文件名字
            string json = "{\"ret\":0,\"msg\":\"上传图片失败！\"}";

            string PhysicalPath = "";
            for (int i = 0; i < uploadFiles.Count; i++)
            {
                if (uploadFiles[i].FileName != "")
                {
                    filename = uploadFiles[i].FileName;
                    string suf = "";
                    if (!isImage(filename, out suf))
                    {
                        json = "{\"ret\":0,\"msg\":\"图片只支持－－－gif|jpg|jpeg|bmp！\"}";
                        return Content(json, "text/json");
                    }
                    string newfileName = string.Format("{0}.{1}", sjname(), suf);

                    if (!System.IO.Directory.Exists(savePath))
                    {
                        System.IO.Directory.CreateDirectory(savePath);
                    }
                    PhysicalPath = savePath + newfileName;

                    uploadFiles[i].SaveAs(PhysicalPath);

                    if (Utils.CheckPictureSafe(PhysicalPath))
                    {
                        hx_borrower_guarantor_picture p = new hx_borrower_guarantor_picture();
                        p.targetid = targetid;
                        p.borrower_registerid = registerid;
                        p.picture_name = Utils.CheckSQLHtml(strArray[i].ToString());
                        p.type_picture = tp;
                        p.picture_path = path2 + newfileName;
                        p.uploadtime = DateTime.Now;
                        ef.hx_borrower_guarantor_picture.Add(p);
                        ef.SaveChanges();

                        json = "{\"ret\":1,\"msg\":\"上传成功！\"}";
                    }
                    else
                    {   //图片安全提醒，您试除上传非法文件
                        json = "{\"ret\":0,\"path\":\"图片中含有非法文件\"}";
                        return Content(json, "text/json");
                    }

                }
            }
            return Content(json, "text/json");
        }

        [HttpPost]
        public ActionResult ImgSubmit(HttpPostedFileBase[] upfiles)
        {
            string savePath = Server.MapPath("\\ProductImg\\UploadFiles") + "\\";//上传文件保存路径 
            string path2 = "/ProductImg/UploadFiles/";
            int num = 0;
            if (upfiles != null)
            {
                foreach (HttpPostedFileBase file in upfiles)
                {
                    if (file != null)
                    {
                        var ss = file.FileName;
                        string str = ss;
                    }
                }
            }

            return Content("");
        }

        /// <summary>
        /// 上次贷款图片
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductPicture()
        {
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string imgPath = "";
            string PhysicalPath = "";
            string json = "{\"ret\":0,\"msg\":\"上传图片失败！\"}";
            if (hfc.Count > 0)
            {
                string suf = "";
                if (!isImage(hfc[0].FileName, out suf))
                {
                    json = "{\"ret\":0,\"msg\":\"图片只支持－－－gif|jpg|jpeg|bmp！\"}";
                    return Content(json, "text/json");
                }
                string fileName = string.Format("{0}.{1}", sjname(), suf);
                imgPath = @"/Productpicture/";
                var ss = hfc[0].ContentType;
                PhysicalPath = Server.MapPath(imgPath);
                if (!System.IO.Directory.Exists(PhysicalPath))
                {
                    System.IO.Directory.CreateDirectory(PhysicalPath);
                }

                PhysicalPath = PhysicalPath + fileName;
                hfc[0].SaveAs(PhysicalPath);

                imgPath = imgPath + fileName;
            }
            if (Utils.CheckPictureSafe(PhysicalPath))
            {
                if (!string.IsNullOrEmpty(imgPath))
                {
                    imgPath = imgPath.Replace("/", "//");
                    json = "{\"ret\":1,\"path\":\"" + imgPath + "\"}";
                }
            }
            else
            {   //图片安全提醒，您试除上传非法文件
                json = "{\"ret\":0,\"path\":\"图片中含有非法文件\"}";
            }
            return Content(json, "text/json");
        }



        /// <summary>
        /// 产生个随即名称
        /// </summary>
        /// <returns></returns>
        private string sjname()
        {
            string sj = null;
            Random ra = new Random();
            sj = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.TimeOfDay.Hours.ToString() + DateTime.Now.TimeOfDay.Minutes.ToString() + DateTime.Now.TimeOfDay.Milliseconds.ToString() + ra.Next(1, 1000).ToString();
            return sj;
        }

        /// <summary>
        /// 判断是否图片
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="suf"></param>
        /// <returns></returns>
        private bool isImage(string filename, out string suf)
        {
            string imgtype = "gif|jpg|jpeg|bmp|png";
            //取后最
            int pos = filename.IndexOf(".");
            string hz = filename.Substring((pos + 1)).ToLower();

            suf = hz;
            if (hz != "gif" && hz != "jpg" && hz != "jpeg" && hz != "bmp" && hz != "png")
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 上次贷款图片
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyPicture()
        {
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string imgPath = "";
            string PhysicalPath = "";
            string json = "{\"ret\":0,\"msg\":\"上传图片失败！\"}";
            if (hfc.Count > 0)
            {
                string suf = "";
                if (!isImage(hfc[0].FileName, out suf))
                {
                    json = "{\"ret\":0,\"msg\":\"图片只支持－－－gif|jpg|jpeg|bmp！\"}";
                    return Content(json, "text/json");
                }
                string fileName = string.Format("{0}.{1}", sjname(), suf);
                imgPath = @"/Companypicture/";
                var ss = hfc[0].ContentType;
                PhysicalPath = Server.MapPath(imgPath);
                if (!System.IO.Directory.Exists(PhysicalPath))
                {
                    System.IO.Directory.CreateDirectory(PhysicalPath);
                }

                PhysicalPath = PhysicalPath + fileName;
                hfc[0].SaveAs(PhysicalPath);

                imgPath = imgPath + fileName;
            }
            if (Utils.CheckPictureSafe(PhysicalPath))
            {
                if (!string.IsNullOrEmpty(imgPath))
                {
                    imgPath = imgPath.Replace("/", "//");
                    json = "{\"ret\":1,\"path\":\"" + imgPath + "\"}";
                }
            }
            else
            {   //图片安全提醒，您试除上传非法文件
                json = "{\"ret\":0,\"path\":\"图片中含有非法文件\"}";
            }
            return Content(json, "text/json");
        }



        /// <summary>
        /// 上次贷款图片
        /// </summary>
        /// <returns></returns>
        public ActionResult LinkPicture()
        {
            HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
            string imgPath = "";
            string PhysicalPath = "";
            string json = "{\"ret\":0,\"msg\":\"上传图片失败！\"}";
            if (hfc.Count > 0)
            {
                string suf = "";
                if (!isImage(hfc[0].FileName, out suf))
                {
                    json = "{\"ret\":0,\"msg\":\"图片只支持－－－gif|jpg|jpeg|bmp！\"}";
                    return Content(json, "text/json");
                }
                string fileName = string.Format("{0}.{1}", sjname(), suf);
                imgPath = @"/Linkpicture/";
                var ss = hfc[0].ContentType;
                PhysicalPath = Server.MapPath(imgPath);
                if (!System.IO.Directory.Exists(PhysicalPath))
                {
                    System.IO.Directory.CreateDirectory(PhysicalPath);
                }

                PhysicalPath = PhysicalPath + fileName;
                hfc[0].SaveAs(PhysicalPath);

                imgPath = imgPath + fileName;
            }
            if (Utils.CheckPictureSafe(PhysicalPath))
            {
                if (!string.IsNullOrEmpty(imgPath))
                {
                    imgPath = imgPath.Replace("/", "//");
                    json = "{\"ret\":1,\"path\":\"" + imgPath + "\"}";
                }
                else
                {
                }
            }
            else
            {   //图片安全提醒，您试除上传非法文件
                json = "{\"ret\":0,\"path\":\"图片中含有非法文件\"}";
            }
            return Content(json, "text/json");
        }
        /// <summary>
        /// 上次贷款图片
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadAvatar()
        {
            string json = "{\"ret\":0,\"msg\":\"上传图片失败！\"}";
            int userid = Utils.checkloginsession();
            if (userid > 0)
            {
                HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                string avatarPath = @"/Avatar/";
                string imgPath = "";
                string PhysicalPath = "";

                if (hfc.Count > 0)
                {
                    string suf = "";
                    if (!isImage(hfc[0].FileName, out suf))
                    {
                        json = "{\"ret\":0,\"msg\":\"图片只支持－－－gif|jpg|jpeg|png！\"}";
                        return Content(json, "text/json");
                    }
                    string fileName = string.Format("{0}.{1}", sjname(), suf);
                    var ss = hfc[0].ContentType;
                    PhysicalPath = Server.MapPath(avatarPath);
                    if (!System.IO.Directory.Exists(PhysicalPath))
                    {
                        System.IO.Directory.CreateDirectory(PhysicalPath);
                    }
                    PhysicalPath = PhysicalPath + fileName;
                    hfc[0].SaveAs(PhysicalPath);
                    imgPath = avatarPath + fileName;
                }
                if (Utils.CheckPictureSafe(PhysicalPath))
                {
                    if (!string.IsNullOrEmpty(imgPath))
                    {                      
                        //imgPath = imgPath.Replace("/", "//");
                        json = "{\"ret\":1,\"path\":\"" + imgPath.Replace("/", "//") + "\"}";
                        //清理之前的头像文件
                        hx_member_table user = ef.hx_member_table.Where(p => p.registerid == userid).FirstOrDefault();//.Update(p => new hx_member_table { contactaddress = addr });
                        if (user != null && user.avatar != null && user.avatar != "0")
                        {
                            string oldFile = Server.MapPath("/") + user.avatar;
                            if (System.IO.File.Exists(oldFile))
                            {
                                System.IO.File.Delete(oldFile);
                            }
                        }
                        user.avatar = imgPath;
                        ef.SaveChanges();
                        //string sql = "update hx_member_table  set  avatar = '" + imgPath + "' where registerid =" + userid.ToString();//.Replace("//", "/")
                        //DBUtility.DbHelperSQL.RunSql(sql);
                    }
                }
                else
                {   //图片安全提醒，您试除上传非法文件
                    json = "{\"ret\":0,\"path\":\"图片中含有非法文件\"}";
                }
            }
            else
            {
                json = "{\"ret\":0,\"msg\":\"你未登录，请登录后再上传头像！\"}";
            }
            return Content(json, "text/json");
        }

    }
}