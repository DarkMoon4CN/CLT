using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChuangLitouP2P.Models;
using ChuanglitouP2P.Common;

namespace ChuanglitouP2P.BLL.Api
{
    public class B_UserAct
    {
        private chuangtouEntities ef = new chuangtouEntities();

        public hx_UserAct GetUserAct(string userActId)
        {
            int userAct = Convert.ToInt32(userActId);
            return ef.hx_UserAct.Where(q => q.UserAct == userAct).FirstOrDefault();
        }

        public hx_UserAct GetAddRateModel(string ids)
        {
            string[] temp = ids.Split(',');
            foreach (string id in temp)
            {
                var item = GetUserAct(id);
                if (item == null) continue;
                else if (item.RewTypeID.Value == 3)//return back add-rate bonun entity only
                    return item;
            }
            return null;
        }

        /// <summary>
        /// 获取基于订单号的数据合计数值(hx_UserAct)
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="amtProId">订单号，或项目编号</param>
        /// <param name="rewTypeId">代金券类型</param>
        /// <param name="useState">状态</param>
        /// <returns></returns>
        public decimal GetOrderTotalAmt(int userId, int amtProId, int rewTypeId, int useState)
        {
            string sql = string.Format(" select isnull(sum(amt),0) from hx_UserAct where registerid={0} and AmtProid={1} and RewTypeID={2} and UseState={3} ", userId, amtProId, rewTypeId, useState);
            var items = DbHelper.Query(sql);
            return Convert.ToDecimal(items.Tables[0].Rows[0][0]);
        }
    }
}
