#region 文件描述
/******************************************************************
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
创建人   : Lin Quanjin
创建时间 : 2019/8/10 8:36:43
说明    : 
******************************************************************/
#endregion
using log4net;
using Quartz;
using System;

namespace AAAServiceByTopShelf
{
    public class Service
    {
        private IScheduler Scheduler { get; }
        private ILog Log { get; }
        public Service(IScheduler scheduler) =>
            Scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));

        public void OnStart()
        {


            Scheduler.Start();
            IJobDetail job = JobBuilder
                  .Create<MyJob>()
                  .WithIdentity("JobName", "JobGroup")
                  .Build();
            // 5秒 一次    0/5 * * * * ? *
            // 30分钟 一次  0 0/30 * * * ? *
            // 每小时一次 0 0 0/1 * * ? *
            ITrigger trigger = TriggerBuilder.Create().StartNow().WithCronSchedule("0/5 * * * * ? *").Build();

            Scheduler.ScheduleJob(job, trigger);
        }
        
      


        public void OnPaused() =>
            Scheduler.PauseAll();

        public void OnContinue() =>
            Scheduler.ResumeAll();

        public void OnStop() =>
            Scheduler.Shutdown();
    }
}
