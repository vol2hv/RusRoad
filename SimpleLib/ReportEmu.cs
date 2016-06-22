/*
схема довольно громоздкая. Предпочтительнее схема с Timer (ReportEmu1)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace SimpleLib
{
    public class ReportEmu
    {
        static int TCreate = 15000;
        static int TWait = 10000;
        Boolean IsStop;
        Task tsk1, tsk2;
        CancellationTokenSource source = new CancellationTokenSource();

        public async Task OnStartAsync()
        {
            ReportMenAsync();
        }
        async Task ReportMenAsync() // менеджер подготовки отчетов
        {
            LogExt.Message("Сервис подготовки отчетов стартует.");
            while (!IsStop)
            {
                LogExt.Message(String.Format("Начало обработки отчета запланировано через {0} мсек.", TWait));
                tsk2 = null;
                tsk1 = Task.Delay(TWait, source.Token); //эмуляция прихода запроса
                await tsk1;
                tsk1 = null;
                tsk2 = CreateReportAsync();
                await tsk2;

            }
            LogExt.Message("Cоздание отчетов прекращено. Сервер создания отчетов получил сигнал на остановку");
        }
        public async Task CreateReportAsync()
        {
            LogExt.Message(String.Format("Начато создание отчета продолжительностью {0} мсек.", TCreate));
            await Task.Delay(TCreate);

            LogExt.Message("Закончено создание отчета.");
        }
        public async Task OnStopAsync()
        {
            LogExt.Message("Сервис подготовки отчетов останавливается.");
            IsStop = true;

            LogExt.Message("Выставлен IsStop.");
            if (tsk1 != null)
            {
                source.Cancel();
                LogExt.Message("снято задание на подготовку отчета");
            }
           
            if (tsk2 != null)
            {
                source.Cancel();
                LogExt.Message("Ждем окончания подготовки отчета");
                await tsk2;
                LogExt.Message("Завершено ожидание создания отчета.");
            }
            LogExt.Message("Сервер создания отчетов остановлен.");

        }

    }
}




