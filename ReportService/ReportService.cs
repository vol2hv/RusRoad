using SimpleLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ReportService
{
    partial class ReportService : ServiceBase
    {
        ReportEmu1 reportEmu;
        public ReportService()
        {
            InitializeComponent();
            reportEmu = new ReportEmu1();

        }

        protected override void OnStart(string[] args)
        {
            
            reportEmu.OnStartAsync();
        }

        protected override void OnStop()
        {
            
            reportEmu.OnStopAsync();
        }
    }
}
