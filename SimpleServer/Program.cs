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
        static ReportEmu1 reportEmu;
        static void Main(string[] args)
        {
            //LogExt Log1 = new LogExt();
            //LogExt.Message("Информационное сообщение.", LogExt.MesLevel.Info);
            //LogExt.Message("А это тоже информационное сообщение");
            //TcpEmu tcpEmu = new TcpEmu();

            //tcpEmu.OnStartAsync();

            //tcpEmu.OnStopAsync();

            reportEmu = new ReportEmu1();
            reportEmu.OnStartAsync();


            Stop();


            Console.ReadLine();
        }
        static async void Stop()
        {
            await Task.Delay(28000);
            reportEmu.OnStopAsync();
        }
    }
}
