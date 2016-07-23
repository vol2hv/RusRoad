﻿using RusRoadLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace RusRoadService
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            
            ServicesToRun = new ServiceBase[]
            {
                new RRTcpService(),new RRReportService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
