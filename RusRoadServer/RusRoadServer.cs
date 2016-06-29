using System;
using RusRoadLib;


namespace RusRoadServer
{
    class RusRoadServer
    {
        static void Main(string[] args)
        {
            RusRoadSettings rrs =new RusRoadSettings();
            rrs.Settings();
            //LogExt.Message(rrs.DirNotification + " " + rrs.DirReport);

            RusRoad rr = new RusRoad();
            rr.OnStartAsync();
            
           

            Console.ReadLine();
            }


    }
}
