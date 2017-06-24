using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChuangLiTou.Core.Entities.Response;
using ChuangLiTou.Core.Entities.Response.Borrow;
using ChuangLiTou.Core.Entities.Response.Member;
using ChuangLiTou.Core.Entities.Response.SmsEmail;
using ChuanglitouP2P.DAL.Api;
namespace ChuanglitouP2P.BLL.Api
{
    public class BorrowLogic
    {
        private readonly BorrowDal _dal = new BorrowDal();

        /// <summary>
        /// 标数据
        /// </summary> 
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页数据条数</param>
        /// <param name="status">标状态</param>
        /// <param name="timeFrom">开始时间</param>
        /// <param name="timeTo">结束时间</param>
        /// <returns></returns>
        public BasePage<List<BorrowEntity>> SelectBorrowList(int pageIndex, int pageSize, int status, string timeFrom, string timeTo)
        {

            return _dal.SelectBorrowList(pageIndex, pageSize, status, timeFrom, timeTo);
        }

        /// <summary>
        /// 获取标详细数据
        /// </summary>
        /// <param name="targetId">标ID</param>
        /// <returns></returns>
        public BorrowEntity SelectBorrowDetail(int targetId)
        {
            return _dal.SelectBorrowDetail(targetId);

        }

        /// <summary>
        /// 获取借款标详细信息接口-解志辉
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        public BorrowDetailEntity SelectBorrowInfor(int targetId)
        {

            return _dal.SelectBorrowInfor(targetId);
        }

        /// <summary>
        /// 确认投资接口
        /// </summary>
        /// <param name="usrId">投资者ID</param>
        /// <param name="targetId">标Id</param>
        /// <param name="investAmount">投资金额</param>
        /// <param name="bds">优惠券Id集合</param>
        /// <param name="code">邀请码</param>
        /// <param name="ordCode">订单号</param>
        /// <param name="withoutInterest">投资到期可获取总收益</param>
        /// <param name="frozenidNo">冻结号</param>
        /// <param name="frozenidAmount">冻结金额=投资钱-优惠券钱</param>
        /// <param name="cPeriod">分期总数</param>
        /// <param name="incomeStatementStr">投资人利息收入表</param>
        /// <returns>System.Int32.</returns>
        ///  创 建 者：解志辉
        ///  创建日期：2016-06-01 17:12:52
        /// 创 建 者：解志辉
        /// 创建日期：2016-06-01 10:44:44
        public int SubmitTender(int usrId, int targetId, decimal investAmount, string bds, string code, string ordCode, decimal withoutInterest, string frozenidNo, decimal frozenidAmount, int cPeriod, string incomeStatementStr)
        {
            return _dal.SubmitTender(usrId, targetId, investAmount, bds, code, ordCode, withoutInterest, frozenidNo, frozenidAmount, cPeriod, incomeStatementStr);
        }

        public BorrowEntity SelectNewHandBorrow()
        {
            return _dal.GetNewHand();
        }
    }
}
