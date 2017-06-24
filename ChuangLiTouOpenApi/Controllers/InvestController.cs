using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using ChuanglitouP2P.Common;
using ChuangLiTou.Core.Entities.Request;
using ChuangLiTou.Core.Entities.Request.Borrow;
using ChuangLiTou.Core.Entities.Request.Invest;
using ChuangLiTou.Core.Entities.Request.Member;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Invest;
using ChuangLiTouOpenApi.Factory;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Web;
using ChuanglitouP2P.BLL.Api;
using ChuanglitouP2P.Model.Invest;

namespace ChuangLiTouOpenApi.Controllers
{
    /// <summary>
    ///  投资相关接口
    /// </summary>

    public class InvestController : BaseApi
    {
        private readonly BonusLogic bonusLogic;
        private readonly InvestLogic investLogic;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="bonusLogic">The bonus logic.</param>
        /// <param name="investLogic"></param>
        public InvestController(BonusLogic bonusLogic, InvestLogic investLogic)
        {
            this.bonusLogic = bonusLogic;
            this.investLogic = investLogic;
        }

        /// <summary>
        /// 获取特定用户已投资列表--解志辉
        /// </summary>
        /// <param name="reqst">The reqst.</param>
        /// <returns>ResultInfo&lt;MemberInvestEntity&gt;.</returns>
        [HttpPost]
        public ResultInfo<BasePage<List<InvestEntity>>> SelectInvests(RequestParam<RequestMemberInvest> reqst)
        {
            var ri = new ResultInfo<BasePage<List<InvestEntity>>>("99999");
            try
            {
                int userId;
                int pageIndex = ConvertHelper.ParseValue(reqst.body.pageIndex.ToString(), 0);
                int pageSize = ConvertHelper.ParseValue(reqst.body.pageSize.ToString(), 0);
                try
                {
                    userId = ConvertHelper.ParseValue(reqst.body.userId.ToString(), 0);
                }
                catch (Exception)
                {
                    userId = 0;
                }

                ri.body = investLogic.SelectInvests(pageIndex, pageSize, userId, "", "");
                if (ri.body == null)
                {
                    ri.code = "1000000010";
                }
                else
                {
                    ri.code = "1";
                }

                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }

        /// <summary>
        /// 获取特定用户已投资明细--解志辉
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<InvestEntity> SelectInvestDetail(RequestParam<RequestInvest> reqst)
        {
            var ri = new ResultInfo<InvestEntity>("99999");
            try
            {
                int recordId = ConvertHelper.ParseValue(reqst.body.recordId.ToString(), 0);
                ri.body = investLogic.SelectInvestDetail(recordId);
                if (ri.body == null)
                {
                    ri.code = "1000000010";
                }
                else
                {
                    ri.code = "1";
                }
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }
        /// <summary>
        /// 获取特定用户回款明细列表--解志辉
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<BasePage<List<ResponseIncomeEntity>>> SelectIncomeList(RequestParam<RequestIncome> reqst)
        {
            var ri = new ResultInfo<BasePage<List<ResponseIncomeEntity>>>("99999");
            try
            {
                int userId = ConvertHelper.ParseValue(reqst.body.userId.ToString(), 0);
                int pageIndex = ConvertHelper.ParseValue(reqst.body.pageIndex.ToString(), 0);
                int pageSize = ConvertHelper.ParseValue(reqst.body.pageSize.ToString(), 0);
                var items = investLogic.SelectIncomeList(pageIndex, pageSize, userId);
                if (items != null && items.rows.Count > 0)
                {
                    List<ResponseIncomeEntity> incLst = new List<ResponseIncomeEntity>();
                    List<ResponseInvestIncomeEntity> tempList = new List<ResponseInvestIncomeEntity>();
                    foreach (var item in items.rows)
                    {
                        var temp = new ResponseIncomeEntity() { shortDate = item.interest_payment_date.ToString("yyyy年MM月") };
                        if (!incLst.Any(t => t.shortDate == temp.shortDate))
                        {
                            incLst.Add(temp);
                        }
                    }

                    foreach (var item in incLst)
                    {
                        item.lst = items.rows.Where(t => t.interest_payment_date.ToString("yyyy年MM月") == item.shortDate).ToList();
                    }
                    BasePage<List<ResponseIncomeEntity>> pItem = new BasePage<List<ResponseIncomeEntity>>();
                    pItem.pageCount = items.pageCount;
                    pItem.recordCount = items.recordCount;
                    pItem.rows = incLst;
                    ri.body = pItem;
                }
                if (ri.body == null)
                {
                    ri.code = "1000000010";
                }
                else
                {
                    ri.code = "1";
                }
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }

        /// <summary>
        /// 获取特定用户回款总金额--解志辉
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultInfo<decimal> SelectTotalIncome(RequestParam<RequestMemberDetail> reqst)
        {
            var ri = new ResultInfo<decimal>("99999");
            try
            {
                int userId = ConvertHelper.ParseValue(reqst.body.userId.ToString(), 0);
                var ti = investLogic.SelectTotalIncome(userId);
                ri.body = ti;
                ri.code = "1";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }
        /// <summary>
        /// 获取特定标的的投资记录--冀兴光
        /// <remark>关联Borrow/SelectBorrowInfor</remark>
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        public ResultInfo<BasePage<List<InvestRecordEntity>>> SelectInvestRecordsByID(RequestParam<RequestInvestRecord> reqst)
        {
            var ri = new ResultInfo<BasePage<List<InvestRecordEntity>>>("99999");
            try
            {
                int recordId;
                int pageIndex = ConvertHelper.ParseValue(reqst.body.pageIndex.ToString(), 0);
                int pageSize = ConvertHelper.ParseValue(reqst.body.pageSize.ToString(), 0);
                try
                {
                    recordId = ConvertHelper.ParseValue(reqst.body.recordId.ToString(), 0);
                }
                catch (Exception)
                {
                    recordId = 0;
                }

                ri.body = investLogic.SelectInvestRecordsByID(pageIndex, pageSize, recordId);
                if (ri.body == null)
                {
                    ri.code = "1000000010";
                }
                else
                {
                    ri.code = "1";
                }

                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }

        /// <summary>
        /// 获取特定用户累计邀请信息--刘佳
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        public ResultInfo<InvestCountEntity> SelectInvitationInvestCount(RequestParam<RequestInvestInvpeople> reqst)
        {
            var ri = new ResultInfo<InvestCountEntity>("99999");
            try
            {
                ri.body = investLogic.SelectInvitationInvestCount(reqst.body.userId);
                #region 生成二维码
                saveQRCode(ri.body.strLink);

                string pjUrl = Settings.Instance.SiteDomain + "/FileImg/" + ri.body.strLink + ".jpg";//获取协议+域名 
                ri.body.strLink = pjUrl;
                #endregion
                ri.code = "1";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }

        /// <summary>
        /// 获取邀请好友投资详情界面的上部分统计信息
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        public ResultInfo<InvitationDetail> SelectInvitationDetail(RequestParam<RequestInvestInvpeople> reqst)
        {
            var ri = new ResultInfo<InvitationDetail>("99999");
            try
            {
                ri.body = investLogic.SelectInvitationDetail(reqst.body.userId);
                ri.code = "1";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }

        /// <summary>
        /// 获取邀请好友投资详情界面的下部分的分页详情信息
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        public ResultInfo<BasePage<List<UserInvestedStatistics>>> SelectInvitationDetailPage(RequestParam<RequestInvestInvpeoplePage> reqst)
        {
            var ri = new ResultInfo<BasePage<List<UserInvestedStatistics>>>("99999");
            try
            {
                ri.body = investLogic.SelectInvitationDetailPage(reqst.body.userId, reqst.body.pageIndex, reqst.body.pageSize);
                ri.code = "1";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.ToString());
                LoggerHelper.Error(JsonHelper.Entity2Json(reqst)); ri.code = "500";
                ri.message = Settings.Instance.GetErrorMsg(ri.code);
                return ri;
            }
        }

        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="invitedcode">邀请码</param>
        private void saveQRCode(string invitedcode)
        {
            string Opath = HttpContext.Current.Server.MapPath("~/FileImg");// @"D:\VedioCapture\Photo";
            string path = Opath + "\\" + invitedcode + ".jpg";
            if (!Directory.Exists(Opath))
                Directory.CreateDirectory(Opath);
            if (!System.IO.File.Exists(path))//确定指定的文件是否存在
            {
                Bitmap objPic, objNewPic, bit;
                try
                {
                    objPic = new Bitmap(QRCode.CreateImage(string.Format(Settings.Instance.QRCodeLink, invitedcode)));
                    Image ig = Image.FromFile(HttpContext.Current.Server.MapPath("~/FileImg/PicIco.png"));
                    bit = (Bitmap)QRCode.CombinImage(objPic, ig);
                    objNewPic = new Bitmap(bit, 120, 120);//图片保存的大小尺寸
                    objNewPic.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch (Exception exp)
                {
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
}