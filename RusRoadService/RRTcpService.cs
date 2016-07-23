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
using System.Threading;


namespace RusRoadService
{
    partial class RRTcpService : ServiceBase
    {
        RusRoad RR = new RusRoad();
       
        public RRTcpService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (RusRoadSettings.CheckingAccessDb())
            {
                RR.OnStartAsync();
            }
        }

        protected override void OnStop()
        {
            RR.OnStopAsync();
        }
    }
}
