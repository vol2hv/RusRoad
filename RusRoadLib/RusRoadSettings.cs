using NLog;
using NLog.Targets;
using System.IO;

namespace RusRoadLib
{
    public class RusRoadSettings
    {
        private string _DirNotification;
        public string DirNotification { get { return _DirNotification; } }

        private string _DirReport;
        public string DirReport { get { return _DirReport; } }

        public void Settings()
        {
           // Полный путь до ехе шника service 
            string nFile = System.Reflection.Assembly.GetEntryAssembly().Location;
            // корневой каталог проекта в visual studio
            string catalog = nFile.Substring(0, nFile.IndexOf("RusRoad") + 8);
            
            // каталог логов
            var catalog1 = catalog + "_Log\\";
            DirectoryInfo dirInfo = new DirectoryInfo(catalog1);
            if (!dirInfo.Exists)  dirInfo.Create();
            nFile = catalog1 + "RusRoad.log"; // файл лога
            var fileTarget = (FileTarget)LogManager.Configuration.FindTargetByName("file");
            fileTarget.FileName = nFile; // прописываем файл лога в конфиг пакета Nlog
           
            // каталог уведомлений
            _DirNotification = catalog + "_Notification\\";
           dirInfo = new DirectoryInfo(_DirNotification);
            if (!dirInfo.Exists) dirInfo.Create();

            // каталог отчетов
            _DirReport = catalog + "_Report\\";
            dirInfo = new DirectoryInfo(_DirReport);
            if (!dirInfo.Exists) dirInfo.Create();
        }
    }
}
