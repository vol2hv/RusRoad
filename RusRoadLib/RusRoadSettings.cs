using NLog;
using NLog.Targets;
using System;
using System.IO;

namespace RusRoadLib
{
    public class RusRoadSettings
    {
        public static string DirRoot; //корневой каталог
        public static string DirReport; //каталог отчетов
        public static string DirNotification; //каталог уведомлений
        public static string ConnString; //Строка подключения
        public static void Settings()
        {
            string nFile;
            // корневой каталог проекта в visual studio
            DirRoot = GetRootDir();

            //каталог логов
            var catalog1 = DirRoot + "_Log\\";
            DirectoryInfo dirInfo = new DirectoryInfo(catalog1);
            if (!dirInfo.Exists) dirInfo.Create();
            nFile = catalog1 + "${shortdate}.log"; // файл лога
            var fileTarget = (FileTarget)LogManager.Configuration.FindTargetByName("file");
            fileTarget.FileName = nFile; // прописываем файл лога в конфиг пакета Nlog

            // каталог уведомлений
            DirNotification = DirRoot + "_Notification\\";
           dirInfo = new DirectoryInfo(DirNotification);
            if (!dirInfo.Exists) dirInfo.Create();

            // каталог отчетов
            DirReport = DirRoot + "_Report\\";
            dirInfo = new DirectoryInfo(DirReport);
            if (!dirInfo.Exists) dirInfo.Create();

            ConnString = CreateConnectString();
        }
        // Проверка доступа к базе данных
        private static bool CheckingAccessDb1()
        {

            bool result = true;
            using (RusRoadsData db = new RusRoadsData())
            {

                if (!db.Database.Exists())
                {
                    LogExt.Message("Базы данных указанной в строке подключения " +
                        db.Database.Connection.ConnectionString + " не существует.\n", LogExt.MesLevel.Error);
                    result = false;
                    return result;
                }

                var com = db.RusRoadCommon.Find(1);
                if (com != null)
                {
                    com.Test = DateTime.Now;
                    db.SaveChanges();
                }
                else
                {
                    LogExt.Message("Не удалось прочитать из базы данных сведения о системе.", LogExt.MesLevel.Error);
                    result = false;
                }

            }
            return result;
        }
        public static bool CheckingAccessDb()
        {
            bool isCheck=false;
            try
            {
                if (RusRoadSettings.CheckingAccessDb1())
                {
                    isCheck = true;
                }
                else
                {
                    LogExt.Message("Тест соединения с базой данных не пройден. Сервис опроса датчиков не запущен.");
                }
            }
            catch (Exception ex)
            {
                LogExt.Message(LogExt.ExeptionMes(ex, "Ошибка доступа к базе данных."), LogExt.MesLevel.Error);
               
            }

            return isCheck;
           
        }
        public static string GetRootDir()
        {
            // Полный путь до ехе шника service 
            string nFile = System.Reflection.Assembly.GetEntryAssembly().Location;
            // корневой каталог проекта в visual studio
            return nFile.Substring(0, nFile.IndexOf("RusRoad") + 8);
        }
        public static string CreateConnectString()
        {
            string result = "name=RusRoadsData";
            FileInfo fileInfo = new FileInfo(GetRootDir()+ "RusRoads.mdf");
           
            if (fileInfo.Exists)
            {
                result = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                         @"AttachDbFilename = E:\USERa\madv\Project\RusRoad\RusRoads.mdf;" +
                         @"Integrated Security = True;";
            }
            //LogExt.Message(result);
            return result;
        }
        public static DateTime ReadLastReport()
        {
            using (RusRoadsData db = new RusRoadsData())
            {
                var com = db.RusRoadCommon.Find(1);
                if (com.LastReport == null)
                    return new DateTime(2000, 1, 1);
                else return (DateTime)com.LastReport;
            }
        }

        public static bool WriteLastReport(DateTime dt)
        {
            bool result = false;
            using (RusRoadsData db = new RusRoadsData())
            {
                var com = db.RusRoadCommon.Find(1);
                if (com.LastReport != null)
                {
                    com.LastReport = dt;
                    try
                    {
                        db.SaveChanges();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                       string err = LogExt.ExeptionMes(ex, "Ошибка записи в базу данных");
                       LogExt.Message(err, LogExt.MesLevel.Error);
                    }
                }
                else
                {
                    LogExt.Message("Не удалось прочитать из базы данных сведения о системе.", LogExt.MesLevel.Error);
                    
                }
            }
            return result;
        }
    }
}

