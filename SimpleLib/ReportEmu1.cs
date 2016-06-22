using System;
using System.Threading;
using System.Threading.Tasks;


namespace SimpleLib
{
    public class ReportEmu1
    {
        int TCreate = 15000;
       
        
        Task tsk,tsk1;
        Timer tmr;

        public async Task OnStartAsync()
        {
            tmr = new Timer(Run);
            tmr.Change(10000, 20000);
            LogExt.Message("Сервер создания отчетов стартовал "+ DateTime.Now.ToString());
        }
        public async void Run(object state)
        {
            tsk = CreateReportAsync();
            tsk1 = tsk;
            await tsk;
            

        }
        public async Task CreateReportAsync()
        {
            LogExt.Message(String.Format("Начато создание отчета продолжительностью {0} мсек.", TCreate));
            await Task.Delay(TCreate);
            
            LogExt.Message("Закончено создание отчета.");
        }
        public async Task OnStopAsync()
        {
            LogExt.Message("Сервис подготовки отчетов останавливается. Статус: " + tsk.Status);
            
            //if (tsk.Status==TaskStatus.Running)
            {
                LogExt.Message("Ждем завершения создания отчета.");
               
                tsk.Wait();

                LogExt.Message("Завершено ожидание создания отчета.");
                ;
               
            }
            tmr.Dispose();
            LogExt.Message("Таймер очищен");
            LogExt.Message("Сервер создания отчетов остановлен.");


        }

    }
}
