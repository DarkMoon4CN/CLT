using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Share;
using ChuangLiTouOpenApi.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChuanglitouP2P.Common;
using ChuangLiTou.Core.Entities.Request.Share;
using ChuanglitouP2P.BLL.Api;
using ChuangLiTou.Core.Entities.Response.Member;
using System.IO;
using System.Drawing;

namespace ChuangLiTouOpenApi.Controllers
{

    /// <summary>
    /// 分享接口
    /// </summary>
    public class ShareController : BaseApi
    {
        private readonly MemberLogic _logic;
        public ShareController(MemberLogic logic)
        {
            _logic = logic;

        }

        /// <summary>
        /// 获取邀请好友的分享信息实体
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<ShareEntity> SelectShareInfor(RequestParam reqst)
        {
            ResultInfo<ShareEntity> res = new ResultInfo<ShareEntity>();
            res.code = "1";
            res.message = Settings.Instance.GetErrorMsg(res.code);
            ShareEntity se = new ShareEntity();
            se.shareTitle = "100元做投资人，成为创利投会员即送1260大礼包";
            se.shareContent = "掌上理财，坐享收益。100元起投，投资好礼送不停";
            se.shareContent += "###http://" + ChuanglitouP2P.Common.PublicURL.NewWXUrl + "//zhuolu.html?code=###http://" + ChuanglitouP2P.Common.PublicURL.NewPCUrl + "/tuiguang.html?code=";
            se.shareImg = Settings.Instance.SiteDomain + "/Static/Images/Share.jpg";
            res.body = se;
            return res;
        }

        [HttpPost]
        public ResultInfo<WeiXinEntity> SelectWeiXinImage(RequestParam reqst)
        {
            ResultInfo<WeiXinEntity> res = new ResultInfo<WeiXinEntity>();
            WeiXinEntity se = new WeiXinEntity();
            se.imageUrl = Settings.Instance.SiteDomain + "/Static/Images/weixin.jpg";


            res.code = "1";
            res.message = Settings.Instance.GetErrorMsg(res.code);
            res.body = se;


            return res;
        }


        /// <summary>
        /// 获取用户分享的二维码
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        public ResultInfo<QRCodeEntity> SelectShareQRCode(RequestParam<RequestQRCode> reqst)
        {
            var ri = new ResultInfo<QRCodeEntity>("99999");
            int userid = ConvertHelper.ParseValue(reqst.body.UserId, 0);
            if (userid == 0)
            {
                ri.code = "1000000000";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }

            QRCodeEntity entity = new QRCodeEntity();
            MemberEntity ent = _logic.SelectMemberByUserId(userid);

            if (ent == null)
            {
                ri.code = "1000000015";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            else
            {
                #region 组装数据
                string path = System.Web.HttpContext.Current.Server.MapPath("/Static/Images/");
                string fileName = ent.username + ent.invitedcode + ".jpg";
                #endregion

                ri.code = "1";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                entity.LinkUrl = Settings.Instance.SiteDomain + "/Static/Images/" + fileName;
                ri.body = entity;
                string shareUrl = "http://" + ChuanglitouP2P.Common.PublicURL.NewWXUrl + "/register/index?invitedcode=" + ent.invitedcode;
                try
                {
                    SaveQRCode(shareUrl, path, fileName);
                    return ri;
                }
                catch (Exception ex)
                {
                    LoggerHelper.Error(JsonHelper.Entity2Json(reqst));
                    LoggerHelper.Error(ex.ToString());
                    LoggerHelper.Error(JsonHelper.Entity2Json(reqst));
                    ri.code = "500";
                    ri.message = Settings.Instance.GetErrorMsg(ri.code);
                    return ri;
                }
            }
        }

        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="str">二维码信息</param>
        /// <param name="path">文件保存路径</param>
        /// <param name="fileName">文件名 包含后缀</param>
        private bool SaveQRCode(string str, string path, string fileName)
        {
            //创建文件夹
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            //存在转发送
            if (File.Exists(path + fileName))
            {
                return true;
            }
            Bitmap objPic, objNewPic, bit;
            try
            {
                objPic = new Bitmap(QRCode.CreateImage(str));
                Image ig = Image.FromFile(HttpContext.Current.Server.MapPath("~/FileImg/PicIco.png"));
                bit = (Bitmap)QRCode.CombinImage(objPic, ig);
                //objNewPic = new Bitmap(bit, 150, 150);
                objNewPic = new Bitmap(bit, 3000, 3000);
                objNewPic.Save(path + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                objNewPic.Dispose();
                objNewPic = null;
                objPic.Dispose();
                objPic = null;
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                return false;
            }
            finally
            {
                objPic = null;
                objNewPic = null;
                bit = null;
            }

        }

    }


}
