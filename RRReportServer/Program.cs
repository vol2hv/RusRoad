using RusRoadLib;
using System;
using System.Threading.Tasks;

namespace RRReportServer
{
    class Program
    {
        static RoadsReport rr = new RoadsReport();
        static void Main(string[] args)
        {
            LogExt.Message("QQйй");
            
            rr.OnStartAsync();
            //Stop();


            Console.ReadLine();
        }
        static async void Stop()
        {
            await Task.Delay(TimeSpan.FromSeconds(130));
            rr.OnStopAsync();
        }
    }
}
