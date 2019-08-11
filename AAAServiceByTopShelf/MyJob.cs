#region 文件描述
/******************************************************************
Copyright @ 苏州瑞泰信息技术有限公司 All rights reserved. 
创建人   : Lin Quanjin
创建时间 : 2019/8/10 9:20:52
说明    : 
******************************************************************/
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Quartz;

namespace AAAServiceByTopShelf
{
    public class MyJob : IJob
    {
        private ILog Log { get; }

     
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(()=> RealJobWork());
        }

        public void RealJobWork()
        {
            string FileName = DateTime.Now.ToString("yyyy-MM-dd Hmmss");


            FileStream fs = new FileStream("D:\\serviceTest\\" + FileName + ".txt", FileMode.Create, FileAccess.Write);//创建写入文件                //设置文件属性为隐藏
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("现在是 "+FileName );//开始写入值
            sw.Close();
            fs.Close();

        }

    }
}
