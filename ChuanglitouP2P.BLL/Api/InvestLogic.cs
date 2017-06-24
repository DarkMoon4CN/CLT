using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Invest;
using ChuanglitouP2P.DAL.Api;
using ChuanglitouP2P.Model.Invest;

namespace ChuanglitouP2P.BLL.Api
{
    /// <summary>
    /// Class InvestLogic.
    /// </summary>
    public class InvestLogic
    {
        private readonly InvestDal _dal = new InvestDal();

        /// <summary>
        /// 获取特定用户的已投资列表--解志辉.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>InvestEntity.</returns> 
        public BasePage<List<InvestEntity>> SelectInvests(int pageIndex, int pageSize, int userId, string timeFrom, string timeTo)
        {
            return _dal.SelectInvests(pageIndex, pageSize, userId, timeFrom, timeTo);
        }

        /// <summary>
        /// 获取投资详情--解志辉
        /// </summary>
        /// <param name="reqst"></param>
        /// <returns></returns>
        public InvestEntity SelectInvestDetail(int recordId)
        {
            return _dal.SelectInvestDetail(recordId);
        }

        /// <summary>
        /// 获取特定标的的已投资记录
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="recordId">The record identifier.</param>
        /// <returns>BasePage&lt;List&lt;InvestRecordEntity&gt;&gt;.</returns>
        public BasePage<List<InvestRecordEntity>> SelectInvestRecordsByID(int pageIndex, int pageSize, int recordId)
        {
            return _dal.SelectInvestRecordsByID(pageIndex, pageSize, recordId);
        }

        /// <summary>
        /// 获取累计邀请信息--刘佳
        /// </summary>
        /// <param name="Invpeopleid">邀请人ID</param>
        /// <returns></returns>
        public InvestCountEntity SelectInvitationInvestCount(int Invpeopleid)
        {
            var refItems = _dal.SelectInvitationDetail(Invpeopleid);
            return new InvestCountEntity()
            {
                RegisterCount = Convert.ToInt32(refItems.InvitedPeopleCount.Substring(0, refItems.InvitedPeopleCount.Length - 1)),
                InvestCount = Convert.ToInt32(refItems.InvitedInvestedPeopleCount.Substring(0, refItems.InvitedInvestedPeopleCount.Length - 1)),
                strLink = string.Empty
            };
        }

        public InvitationDetail SelectInvitationDetail(int Invpeopleid)
        {
            return _dal.SelectInvitationDetail(Invpeopleid);
        }

        public BasePage<List<UserInvestedStatistics>> SelectInvitationDetailPage(int Invpeopleid, int pageIndex, int pageSize)
        {
            return _dal.SelectInvitationDetailPage(Invpeopleid, pageIndex, pageSize);
        }

        /// <summary>
        /// 回款计划--解志辉
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public BasePage<List<ResponseInvestIncomeEntity>> SelectIncomeList(int pageIndex, int pageSize, int userId)
        {
            return _dal.SelectIncomeList(pageIndex, pageSize, userId);
        }

        /// <summary>
        /// 获取回款总金额--解志辉
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public decimal SelectTotalIncome(int userId)
        {
            return _dal.SelectTotalIncome(userId);
        }
    }
}
