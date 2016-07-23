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

            Console.ReadLine();
            }
        string GetRootDir()
        {
            // Полный путь до ехе шника service 
            string nFile = System.Reflection.Assembly.GetEntryAssembly().Location;
            // корневой каталог проекта в visual studio
           return nFile.Substring(0, nFile.IndexOf("RusRoad") + 8);
        }


    }
}
