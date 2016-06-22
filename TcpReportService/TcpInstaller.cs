/*
один установщик на все службы
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace TcpReportService
{
    [RunInstaller(true)]
    public partial class TcpInstaller : System.Configuration.Install.Installer
    {
        private ServiceInstaller serviceInstaller1;
        private ServiceInstaller serviceInstaller2;
        private ServiceProcessInstaller processInstaller;

        public TcpInstaller()
        {
            // Instantiate installers for process and services.
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller1 = new ServiceInstaller();
            serviceInstaller2 = new ServiceInstaller();

            // The services run under the system account.
            processInstaller.Account = ServiceAccount.LocalSystem;

            // The services are started manually.
            serviceInstaller1.StartType = ServiceStartMode.Manual;
            serviceInstaller2.StartType = ServiceStartMode.Manual;

            // ServiceName must equal those on ServiceBase derived classes.
            serviceInstaller1.ServiceName = "TcpService";
            serviceInstaller2.ServiceName = "ReportService";

            // Add installers to collection. Order is not important.
            Installers.Add(serviceInstaller1);
            Installers.Add(serviceInstaller2);
            Installers.Add(processInstaller);
        }

        //public static void Main()
        //{
        //    Console.WriteLine("Usage: InstallUtil.exe [<service>.exe]");
        //}
    }
}


