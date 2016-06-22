using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLib
{
    public class LogExt
    // Раcширение класса Logger пакета Nlog
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public enum MesLevel { Trace, Debug, Info, Warn, Error, Fatal };
        public static void Message(string Mes, MesLevel Lev=MesLevel.Info)
        {
            switch (Lev)
            {
                case MesLevel.Trace:
                    logger.Trace(Mes);
                    break;
                case MesLevel.Debug:
                    logger.Debug(Mes);
                    break;
                case MesLevel.Info:
                    logger.Info(Mes);
                    break;
                case MesLevel.Warn:
                    logger.Warn(Mes);
                    break;
                case MesLevel.Error:
                    logger.Error(Mes);
                    break;
                case MesLevel.Fatal:
                    logger.Fatal(Mes);
                    break;
                default:
                    logger.Trace(Mes);
                    break;
            }
        }

    }
}
