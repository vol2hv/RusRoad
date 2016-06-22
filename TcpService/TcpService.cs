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

namespace TcpService
{
    partial class TcpService : ServiceBase
    {
        TcpEmu tcpEmu;
        public TcpService()
        {
            InitializeComponent();
           tcpEmu = new TcpEmu();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Добавьте код для запуска службы.
            tcpEmu.OnStartAsync();
        }

        protected override void OnStop()
        {
            // TODO: Добавьте код, выполняющий подготовку к остановке службы.
            tcpEmu.OnStopAsync();
        }
    }
}
