using RusRoadLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;


namespace RusRoadService
{
    partial class RRTcpService : ServiceBase
    {
        RusRoadSettings RRSetting = new RusRoadSettings();
        RusRoad RR = new RusRoad();
        public RRTcpService()
        {
            InitializeComponent();

            RRSetting.Settings();
            
        }

        protected override void OnStart(string[] args)
        {
            if (!RR.CheckingAccessDb())
            {
                LogExt.Message("Тест соединения с базой данных не пройден. Сервис опроса датчиков не запущен.");
                this.Stop();
            }
            RR.OnStartAsync();
        }

        protected override void OnStop()
        {
            RR.OnStopAsync();
        }
    }
}
