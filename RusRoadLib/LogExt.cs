using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RusRoadLib
{
    public class LogExt
    // Раcширение класса Logger пакета Nlog и прочие текстовые бяки
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public enum MesLevel { Trace, Debug, Info, Warn, Error, Fatal };
        // Вывод сообщения логирования
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
        // формирование сообщения об ошибке
        public static string ErrorMes(string pass, string cause, Exception ex)
        {
            string s = "Проезд " + pass + " не записан в базу данных \n" + cause;
            string s1 = ex == null ? "" : "Сообщение : " + ex.Message + "\n" +
            "Внутреннее исключение: " + ex.InnerException.InnerException.Message + "\n" +
            "Стек вызова: " + "\n" + ex.StackTrace;

            return s + s1 + "\n";
        }
        // формирование текста уведомления 
        static string Notification(string fio, string rName, int speed, int mSpeed, DateTime dt)
        {
            string n = "\n";
            string str = "Уважаемый {0}!" + n;
            str += "К нашему сожалению, Вы передвигались по дороге {1} со скоростью {2} км./ час." + n;
            str += "и превысили допустимую скорость для этой дороги {3} км./час." + n;
            str += "Нарушение зафиксировано {4}" + n;
            str += "К Вашему сожалению, за это нарушение Вам необходимо заплатить штраф в ближайшем отделение ГИБДД. " + n;
            return String.Format(str, fio, rName, speed, mSpeed, dt);
        }

    }
}
