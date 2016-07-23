using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using RusRoadLib;

namespace RusRoadService
{
    partial class RRReportService : ServiceBase
    {
        private RoadsReport roadsReport;
        public RRReportService()
        {
            InitializeComponent();
            roadsReport = new RoadsReport();
            
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Добавьте код для запуска службы.
            roadsReport.OnStartAsync();
        }

        protected override void OnStop()
        {
            // TODO: Добавьте код, выполняющий подготовку к остановке службы.
            roadsReport.OnStopAsync();
        }
    }
}
