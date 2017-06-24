using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChuanglitouP2P.Common.Task
{
    public class CheckOrderTask : TaskBase
    {

        public override string Name
        {
            get
            {
                return "Check Order Task";
            }
        }
        protected override void Execute()
        {
            try
            {

                var url = Settings.Instance.SiteDomain + "/Admin/DaiKuan/InvalidInvst";
                LogInfo.Info("无效标自动校验开始"+"0"+ url);
                HttpHelper.Get(url);
                LogInfo.Info("无效标自动校验结束");
            }
            catch (Exception ex)
            {
                LogInfo.WriteLog(ex.ToString());
            }
        }




        public override int DefaultInterval
        {
            get { return 0; }
        }

        //定时清理无效投资的默认时间设置为3分钟，每3分钟执行一次定时任务。daikuancontroller每次对6分钟之前的任务进行处理
        protected override void ScheduleNextRun()
        {
            this.NextRun = DateTime.Now.AddMinutes(3);
        }

    }
}
