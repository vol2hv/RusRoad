using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            LogExt.Message("Служба сбора данных от датчиков запущена.");

            Console.ReadLine();
            }


    }
}
