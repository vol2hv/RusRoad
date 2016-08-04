using System;
using RusRoadLib;


namespace RusRoadServer
{
    class RusRoadServer
    {
        static void Main(string[] args)
        {
            RusRoad rr = new RusRoad();
            rr.OnStartAsync();
            RoadsReport roadsReport = new RoadsReport();
            roadsReport.OnStartAsync();

            //RusRoadSettings.Settings();
            //RusRoadSettings.CheckingAccessDb();
            //TestValid.Test();

            Console.ReadLine();
            }
        


    }
}
