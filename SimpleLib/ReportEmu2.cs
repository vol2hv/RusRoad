/*
Вариант с таймером 
При останове создание отчета прерывается
отлажено не до конца 
сейчас ожидание завершения формирования отчета происходит в двух местах
надо ждать это в одном месте
и связывать OnStop через событие и получается очень запутанная схема

*/

using System;
using System.Threading;
using System.Threading.Tasks;


namespace SimpleLib
{
    public class ReportEmu2
    {
        int TCreate = 15000;
        // Событие для отслеживания окончания создания отчета
        Timer tmr;
        AutoResetEvent autoEvent;
        Task tsk; //!!!
        CancellationTokenSource cancelTokSrc;



        public async Task OnStartAsync()
        {

            autoEvent = new AutoResetEvent(false);
            tmr = new Timer(Run, autoEvent, 10000, 20000); // возможно не нужен autoEvent

            LogExt.Message("Сервер создания отчетов стартовал ");
        }
        public async void Run(object state)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)state;
            cancelTokSrc = new CancellationTokenSource();

            // Запустить задачу, передав признак отмены 

            tsk = CreateReportAsync(cancelTokSrc.Token);
            try
            {
                await tsk; //!!!
            }
            catch (Exception)
            {

               //
            }
           
            tsk = null;


        }
        public async Task CreateReportAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested(); // сигнал остановки пришел раньше запуска задачи
            LogExt.Message(String.Format("Начато создание отчета продолжительностью {0} мсек.", TCreate));
            for (int count = 0; count < 15; count++)
            {
                LogExt.Message(String.Format("Создание отчета {0} сек. Статус {1}", count, ct.IsCancellationRequested));
                if (ct.IsCancellationRequested)
                {
                    LogExt.Message("Поступил сигнал на прекращение задачи создания отчета.");
                    ct.ThrowIfCancellationRequested();
                }
                await Task.Delay(1000);
            }


            LogExt.Message("Закончено создание отчета.");
        }
        public async void OnStopAsync()
        {
            LogExt.Message("Сервис подготовки отчетов останавливается.");
            if (tsk != null)
            {
                try
                {
                    // Прерываем задачу создания отчета 
                    cancelTokSrc.Cancel();
                    LogExt.Message("Ожидание остановки сервиса создания отчетов.");
                    await tsk; //!!!
                }
                catch (OperationCanceledException)
                {
                    
                     LogExt.Message("Формирование отчета прервано в связи с остановкой сервиса.");
                }
                catch (Exception e)
                {
                    LogExt.ExeptionMes(e,"Ошибка при прерывании создания отчета.");
                    
                }
                
            }
            
            tmr.Dispose();
            tsk=null;
            cancelTokSrc.Dispose();
            autoEvent.Dispose();
            LogExt.Message("Сервер создания отчетов остановлен.");
        }


    }
}
