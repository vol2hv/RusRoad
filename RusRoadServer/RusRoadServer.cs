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
            //RusRoadSettings.CheckingAccessDb();

            Console.ReadLine();
            }
        


    }
}
