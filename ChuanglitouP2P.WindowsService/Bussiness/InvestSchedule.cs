///////////////////////////////////////////////////////////
//Name:业务模型-定期投资计划
//Author:薛洪立
//Datetime:2016-12-22
///////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.WindowsService
{
    /// <summary>
    /// 定期投资计划
    /// </summary>
    public class InvestSchedule : Worker
    {
        public InvestSchedule() { }

        public override string Name
        {
            get
            {
                return "定期投资计划";
            }
        }

        public override void DoWork(WorkRoom workPosition)
        {
            //var planlist, plan, investuser, targetlist, target;
        }
    }
}
