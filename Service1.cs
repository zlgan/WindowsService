using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Messaging;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        MessageQueue messageQueue = null;
        public Service1()
        {
            InitializeComponent();

            System.Timers.Timer tm = new System.Timers.Timer();
            tm.Elapsed += Tm_Elapsed;
            tm.Interval = 5000;
            tm.Enabled = true;

            try
            {
                if (MessageQueue.Exists(@".\Private$\MyQueues"))
                {
                    messageQueue = new MessageQueue(@".\Private$\MyQueues");
                }
                else
                {
                    messageQueue = MessageQueue.Create(@".\Private$\MyQueues");
                    messageQueue.Label = "Newly Created Queue";
                }
            }
            catch (Exception ex)
            {
                this.WriteLog(ex.ToString());
            }
            
        }

        private void Tm_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            try
            {
                Message msg = messageQueue.Receive();
                msg.Formatter = new System.Messaging.XmlMessageFormatter(new Type[] { typeof(string) });
                string sql = string.Format("insert into TestLog values('{0}','{1}')", DateTime.Now.ToString(), msg.Body.ToString());
                DBUtil.SqlExecute(sql);
            }
            catch (Exception ex)
            {
                this.WriteLog(ex.ToString());
            }
        }


        protected override void OnStart(string[] args)
        {
            this.WriteLog("我的服务启动");
        }

        protected override void OnStop()
        {
            this.WriteLog("我的服务器停止");
        }

        #region 记录日志
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="msg"></param>
        private void WriteLog(string msg)
        {

            //string path = @"C:\log.txt";

            //该日志文件会存在windows服务程序目录下
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\log.txt";
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                FileStream fs;
                fs = File.Create(path);
                fs.Close();
            }

            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(DateTime.Now.ToString() + "   " + msg);
                }
            }
        }
        #endregion
}
}
