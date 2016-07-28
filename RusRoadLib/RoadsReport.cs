using System;
using System.Threading.Tasks;
using System.Threading;
using RusRoadLib;
using System.Linq;
using System.IO;

namespace RusRoadLib
{
    public class RoadsReport  
    {
        //int CountHandler = 3; //число обработок за сутки
        int CountHandler = 288; //число обработок за сутки
        DateTime BeginTime; // начало периода отчета включительно
        DateTime EndTime;   // конец переода отчета (абсолютное время следущего запуска создания отчета) исключительно
        bool IsStop = false; // признак начала остановки сервиса
        CancellationTokenSource source;
        Task tsk;
        
       

        public async Task OnStartAsync()
        {
            if (RusRoadSettings.CheckingAccessDb())
            {
               
                RusRoadSettings.Settings();

                source = new CancellationTokenSource();
                tsk = ReportMenAsync(source.Token);
            }
            
        }
        public async Task OnStopAsync()
        {
            LogExt.Message("От операционной системы получен сигнал на останоку сервиса подготовки отчетов");
            IsStop = true;
            source.Cancel();
            LogExt.Message("Ждем окончания работы сервиса подготовки отчетов");

            bool r = await CheckTask(tsk, "<Менеджер подготовки отчетов>");
            LogExt.Message("Сервис подготовки отчетов остановлен");
        }
        TimeSpan CalculationTime()
        {
            DateTime curDateTime = DateTime.Now;
            var t = TimeSpan.TicksPerDay / CountHandler; // период в тиках
            var tTick = new TimeSpan(t);
            EndTime = new DateTime((curDateTime.Ticks / t + 1) * t); // Время следующего запуска абсолютное
            // if (EndTime==BeginTime+tTick) EndTime += tTick;
            return EndTime - curDateTime;
        }
        async Task<bool> CheckTask(Task tsk, string tskName)
        {
            bool result = false;
            string str;
            try
            {
                await tsk;
                result = true;
            }
            catch (OperationCanceledException)
            {
                LogExt.Message("Снята задача " + tskName);
            }
            catch (Exception ex)
            {
                str = LogExt.ExeptionMes(ex, "Задача " + tskName + "завершилась с ошибками.");
                LogExt.Message(str, LogExt.MesLevel.Error);

            }
            if (!result)
            {
                source.Dispose();
                tsk.Dispose();
            }
            return result;
        }
        async Task ReportMenAsync(CancellationToken ct) // менеджер подготовки отчетов
        {
            LogExt.Message("Сервис подготовки отчетов стартует.");
            ct.ThrowIfCancellationRequested();  //А может сервис уже остановлен
            BeginTime = RusRoadSettings.ReadLastReport();
            while (!IsStop)
            {
                var t = CalculationTime();  // Расчет временных параметров
                LogExt.Message(String.Format("Начало создания отчета запланировано на {0} ", EndTime));
                Task tsk1 = Task.Delay(t, ct);
                bool r = await CheckTask(tsk1, "<Ожидание времени начала формирования отчета>"); // ждем времени начала формирования отчета
                if (!r) break;

                ct.ThrowIfCancellationRequested();  //А может сервис уже остановлен

                // Создание отчета 
                tsk1 = DatabaseDataProcessingAsync(ct);
                r = await CheckTask(tsk1, "<Создание отчета>"); // ждем времени начала формирования отчета
                if (!r) break;
                BeginTime = EndTime;
                RusRoadSettings.WriteLastReport(EndTime);
                ct.ThrowIfCancellationRequested(); //А может сервис уже остановлен
            }
            LogExt.Message("Завершена работа менеджера создания отчетов");
        }

        async Task DatabaseDataProcessingAsync(CancellationToken ct)
        {
            // тестовая заглушка
            //LogExt.Message(String.Format("Начато создание отчета продолжительностью {0} мсек.", 59000));
            //await Task.Delay(59000, ct);
            //LogExt.Message("Закончено создание отчета.");
            //========================================
            string fReport = RusRoadSettings.DirReport + "Report.csv";
            using (RusRoadsData db = new RusRoadsData())
            {
                var result = from passage in db.Passage
                             join carOwner in db.CarOwner on passage.CarOwner_Id equals carOwner.CarOwner_Id
                             join highway in db.Highway on passage.Highway_Id equals highway.Highway_Id
                             where passage.Time >= BeginTime && passage.Time < EndTime
                             orderby carOwner.Name, passage.Time
                             select new
                             {
                                 fio = carOwner.Name,
                                 carOwner.Govnumber,
                                 highway.Name,
                                 maxSpeed = highway.Speed,
                                 passage.Time,
                                 passage.Speed
                             };
                try
                {
                    
                    using (StreamWriter sw = new StreamWriter(fReport))
                    {
                        foreach (var r in result)
                        {
                            ct.ThrowIfCancellationRequested();  //А может сервис уже остановлен
                            var ss = String.Format("{0},{1},{2},{3},{4},{5}", r.fio, r.Govnumber, r.Name, r.maxSpeed, r.Time, r.Speed);
                            sw.WriteLine(ss);
                            // Формирование уведомлений
                            var fNotif = RusRoadSettings.DirNotification + r.fio.Trim().Replace(" ", "_") + ".txt";

                            using (StreamWriter sw1 = new StreamWriter(fNotif, true))
                            {
                                ss = LogExt.Notification(r.fio, r.Name, r.Speed, r.maxSpeed, r.Time);
                                sw1.WriteLine(ss);
                            }
                        }
                        
                    }
                }
                catch (Exception e)
                {

                    LogExt.ExeptionMes(e, "Ошибка при записи отчета или уведомлений");
                }

            }
        }

     

    }

}
