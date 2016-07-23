/*
схема с Task.Delay и прерывание создания отчета
схема довольно громоздкая. Предпочтительнее схема с Timer (ReportEmu1)???
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
        Task tsk;
        CancellationTokenSource source;

        public async Task OnStartAsync()
        {
            source = new CancellationTokenSource();
            tsk =ReportMenAsync(source.Token);
        }
        async Task ReportMenAsync(CancellationToken ct ) // менеджер подготовки отчетов
        {

            LogExt.Message("Сервис подготовки отчетов стартует.");
            while (!IsStop)
            {
                ct.ThrowIfCancellationRequested();
                LogExt.Message(String.Format("Начало обработки отчета запланировано через {0} мсек.", TWait));
                
                Task tsk1=Task.Delay(TWait, ct); 
                bool r = await CheckTask(tsk1, "<Ожидание времени начала формирования отчета>"); // ждем времени начала формирования отчета
                if (!r) break;

                ct.ThrowIfCancellationRequested();
                r = await CheckTask(CreateReportAsync(ct), "<Создание отчета>"); // ждем времени начала формирования отчета
                if (!r) break;
            }
            LogExt.Message("Завершена работа менеджера создания отчетов");
        }
        async Task CreateReportAsync(CancellationToken ct)
        {
            LogExt.Message(String.Format("Начато создание отчета продолжительностью {0} мсек.", TCreate));
            await Task.Delay(TCreate,ct);

            LogExt.Message("Закончено создание отчета.");
        }
        public async Task OnStopAsync()
        {
            LogExt.Message("От операционной системы получен сигнал на останоку сервиса подготовки отчетов");
            IsStop = true;
            source.Cancel();
            LogExt.Message("Ждем окончания работы сервиса подготовки отчетов");

            bool r = await CheckTask(tsk, "<Менеджер подготовки отчетов>"); // ждем времени начала формирования отчета
            LogExt.Message("Сервис подготовки отчетов остановлен");
        }

        async Task<bool> CheckTask(Task tsk, string tskName)
        {
            bool result=false;
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
                LogExt.Message(str,LogExt.MesLevel.Error);
                
            }
            if (!result)
            {
                source.Dispose();
                tsk.Dispose();
            }
            return result;
        }

    }
}




