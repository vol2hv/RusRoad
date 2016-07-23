using NLog;
using NLog.Targets;
using System;
using System.IO;

namespace RusRoadLib
{
    public class RusRoadSettings
    {
        public static void Settings(out string dirReport, out string dirNotification)
        {
           // Полный путь до ехе шника service 
            string nFile = System.Reflection.Assembly.GetEntryAssembly().Location;
            // корневой каталог проекта в visual studio
            string catalog = nFile.Substring(0, nFile.IndexOf("RusRoad") + 8);

            //каталог логов
            var catalog1 = catalog + "_Log\\";
            DirectoryInfo dirInfo = new DirectoryInfo(catalog1);
            if (!dirInfo.Exists) dirInfo.Create();
            nFile = catalog1 + "${shortdate}.log"; // файл лога
            var fileTarget = (FileTarget)LogManager.Configuration.FindTargetByName("file");
            fileTarget.FileName = nFile; // прописываем файл лога в конфиг пакета Nlog

            // каталог уведомлений
            dirNotification = catalog + "_Notification\\";
           dirInfo = new DirectoryInfo(dirNotification);
            if (!dirInfo.Exists) dirInfo.Create();

            // каталог отчетов
            dirReport = catalog + "_Report\\";
            dirInfo = new DirectoryInfo(dirReport);
            if (!dirInfo.Exists) dirInfo.Create();
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

