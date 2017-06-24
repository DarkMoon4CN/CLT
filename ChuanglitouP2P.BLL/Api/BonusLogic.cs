using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Bonus;
using ChuangLiTou.Core.Entities.Response.Borrow;
using ChuangLiTou.Core.Entities.Response.Invest;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuangLiTou.Core.Entities.Response.NormalArea;
using ChuangLiTou.Core.Entities.Response.SmsEmail;
using ChuangLiTou.Core.Entities.Response.UserAddress;
using ChuanglitouP2P.DAL.Api;
using ChuanglitouP2P.BLL.EF;
namespace ChuanglitouP2P.BLL.Api
{
    public class BonusLogic
    {
        private readonly BonusDal _dal = new BonusDal();


        /// <summary>
        /// 获取用户 余额..
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>MemberInvestEntity.</returns>
        public MemberInvestEntity SelectBalance(int userId)
        {

            return _dal.SelectBalance(userId);
        }
        public ActivityBonusEntity SelectBonus(int userId, string sltd)
        {

            return _dal.SelectBonus(userId, sltd);
        }
        /// <summary>
        /// 获取优惠券列表.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="rewardState">0 未使用   1已使用  2 已过期 3 锁定中 -1 查询全部.</param>
        /// <returns>BasePage&lt;List&lt;BonusEntity&gt;&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-06-03 09:57:19 
        public BasePage<List<BonusEntity>> SelectBonuses(int pageIndex, int pageSize, int userId, int rewardState)
        {

            return _dal.SelectBonuses(pageIndex, pageSize, userId, rewardState);
        }
        /// <summary>
        /// 获取抵扣券、加息券列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public BasePage<List<OwnBonusEntity>> SelectOwnBonuses(int pageIndex, int pageSize, int userID)
        {
            BasePage<List<OwnBonusEntity>> result = null;
            result = _dal.SelectOwnBonuses(pageIndex, pageSize, userID);
            ActBase actBase = new ActBase();
            string temp = string.Empty;
            if (result.rows != null && result.rows.Count > 0)
            {
                foreach (var item in result.rows)
                {
                    if (item.UseLifeLoan == null) { item.UseLifeLoan = string.Empty; item.UseLifeLoanMessage = string.Empty; }
                    actBase.GetCanUseLimit(item.UseLifeLoan, out temp);
                    if (!string.IsNullOrWhiteSpace(temp))
                        item.UseLifeLoanMessage = temp;
                    temp = string.Empty;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取抵扣券、加息券详情
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public OwnBonusDetailEntity SelectOwnBonusesDetail(int userAct)
        {
            OwnBonusDetailEntity result = null;
            result = _dal.SelectOwnBonusesDetail(userAct);
            ActBase actBase = new ActBase();
            string temp = string.Empty;
            if (result != null)
            {
                actBase.GetCanUseLimit(result.UseLifeLoan, out temp);
                if (!string.IsNullOrWhiteSpace(temp))
                    result.UseLifeLoanMessage = temp;
                temp = string.Empty;
            }
            return result;
        }

        /// <summary>
        /// 获取用户常用地址
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>NormalAreaEntity.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-26 16:46:25
        public ResponseUserAddressEntity SelectUserAddress(int userId)
        {

            return _dal.SelectUserAddress(userId);
        }

        /// <summary>
        /// 通过id获取优惠券实体
        /// </summary>
        /// <param name="bonusId">The bonus identifier.</param>
        /// <returns>BonusEntity.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-06-03 09:25:55
        public List<BonusEntity> SelectBonusById(int userId, string bonusId)
        {

            return _dal.SelectBonusById(userId, bonusId);
        }

        /// <summary>
        /// 新版获取用户 余额+优惠券 刘佳
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sltd"></param>
        /// <returns></returns>
        public ActivityBonusEntity SelectActivity(int userId, string sltd)
        {
            ActivityBonusEntity result = null;
            result = _dal.SelectActivity(userId, sltd);
            ActBase actBase = new ActBase();
            string temp = string.Empty;
            if (result.bonus != null && result.bonus.Count > 0)
            {
                foreach (var item in result.bonus)
                {   //默认无限制
                    if (string.IsNullOrWhiteSpace(item.UseLifeLoan)||item.UseLifeLoan=="0") { item.UseLifeLoan = "0-0"; item.UseLifeLoanMessage = string.Empty; }
                    actBase.GetCanUseLimit(item.UseLifeLoan, out temp);
                    if (!string.IsNullOrWhiteSpace(temp))
                        item.UseLifeLoanMessage = temp;
                    temp = string.Empty;
                }
            }
            if (result.addRate != null && result.addRate.Count > 0)
            {
                foreach (var item in result.addRate)
                {
                    if (string.IsNullOrWhiteSpace(item.UseLifeLoan) || item.UseLifeLoan == "0") { item.UseLifeLoan = "0-0"; item.UseLifeLoanMessage = string.Empty; }
                    actBase.GetCanUseLimit(item.UseLifeLoan, out temp);
                    if (!string.IsNullOrWhiteSpace(temp))
                        item.UseLifeLoanMessage = temp;
                    temp = string.Empty;
                }
            }
            return result;
        }
    }
}
