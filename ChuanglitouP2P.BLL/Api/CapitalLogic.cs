using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuangLiTou.Core.Entities.ProEnt;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Bonus;
using ChuangLiTou.Core.Entities.Response.Capital;
using ChuangLiTou.Core.Entities.Response.Invest;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuangLiTou.Core.Entities.Response.SmsEmail;
using ChuanglitouP2P.DAL.Api;
namespace ChuanglitouP2P.BLL.Api
{
    /// <summary>
    /// Class CapitalLogic.
    /// </summary>
    public class CapitalLogic
    {
        private readonly CapitalDal _dal = new CapitalDal();


        /// <summary>
        /// 获取用户资金总体概括.
        /// </summary>
        /// <param name="userId">用户id.</param>
        /// <returns>MemberEntity.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 13:09:08
        public MemberEntity SelectMemberFundsInformation(int userId)
        {

            return _dal.SelectMemberFundsInformation(userId);

        }

        /// <summary>
        /// 获取用户资金流水列表.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>BasePage&lt;List&lt;CapitalAccountWater&gt;&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-17 15:24:21
        public BasePage<List<CapitalAccountWater>> SelectFundWater(int pageIndex, int pageSize, int userId)
        {


            return _dal.SelectFundWater(pageIndex, pageSize, userId);
        }

        /// <summary>
        /// 资金总计接口.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <returns>MemberEntity.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-18 17:23:55
        public MemberEntity TotalCapital(int userId)
        {

            return _dal.TotalCapital(userId);

        }

        /// <summary>
        /// 累计投资接口
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>BasePage&lt;List&lt;CapitalAccountWater&gt;&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-25 17:04:23

        public BasePage<List<InvestEntity>> SelectTotalInvest(int pageIndex, int pageSize, int userId)
        {


            return _dal.SelectTotalInvest(pageIndex, pageSize, userId);
        }


        /// <summary>
        /// 获取体现记录
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>BasePage&lt;List&lt;ResponsePresentEntity&gt;&gt;.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-25 17:30:35
        public BasePage<List<PresentEntity>> SelectPresentRecord(int pageIndex, int pageSize, int userId)
        {

            return _dal.SelectPresentRecord(pageIndex, pageSize, userId);
        }

        /// <summary>
        /// 累计收益接口
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>MemberEntity.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-05-25 18:09:46
        public MemberEntity SelectTotalGains(int userId)
        {
            return _dal.SelectTotalGains(userId);
        }

        /// <summary>
        /// 获取提现总金额--解志辉
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal SelectTotalPresent(int userId)
        {
            return _dal.SelectTotalPresent(userId);
        }
    }
}
