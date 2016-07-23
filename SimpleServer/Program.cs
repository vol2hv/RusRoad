using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLib;

// тест2
namespace SimpleServer
{
    class Program
    {
        static ReportEmu reportEmu;
        static void Main(string[] args)
        {

            //TcpEmu tcpEmu = new TcpEmu();

            //tcpEmu.OnStartAsync();

            //tcpEmu.OnStopAsync();

            reportEmu = new ReportEmu();
            reportEmu.OnStartAsync();


            Stop();


            Console.ReadLine();
        }
        static async void Stop()
        {
            await Task.Delay(7000);
            reportEmu.OnStopAsync();
        }
    }
}
