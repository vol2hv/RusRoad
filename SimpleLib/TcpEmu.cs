using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLib
{
    public class TcpEmu
    // Эмулятор получения данных по Tcp
    {
        int Tprocessing = 25000;
        int VG = 30000;
        Random rnd1 = new Random();
        int Myid = 0;
        bool IsStop;
        HashSet<Task> ActiveTask = new HashSet<Task>();

        public void OnStartAsync()
        {
            IsStop = false; // в true устанавливает OnStopAsync()
            LogExt.Message("Сервис стартует.");
            SignallManagerAsync();
        }
        async Task SignallManagerAsync()
        {
            while (!IsStop)
            {
                var t = rnd1.Next(VG);
                LogExt.Message(String.Format("Сигнал будет через {0} мсек.", t));
                await Task.Delay(t); //эмуляция прихода запроса
                if (IsStop) break;
                
                Myid += 1;
                var tsk = SessionAsync( Myid);
                LogExt.Message(String.Format("Пришел сигнал {0}. Для обработки запущена без ожидания задача {1}.", Myid, tsk.Id));

                ProcessTask(tsk);
            }
            LogExt.Message("Обработка сигналов от датчиков прекращена так как сервер получил сигнал на остановку.");
        }
        async Task SessionAsync(int myId)
        {
           
            LogExt.Message(String.Format("Начало обработки сигнала {1}. Обработка продлиться {0} мсек.", Tprocessing, myId));
            await Task.Delay(Tprocessing);
           
            LogExt.Message(String.Format("!!! Завершена обработка сигнала {0} ", myId));
           
        }
        async Task ProcessTask(Task task)
        {
            try
            {
                LogExt.Message(String.Format("Задача {0} помещена в список.",task.Id));
                ActiveTask.Add(task);
                await task;
            }
            finally
            {
                ActiveTask.Remove(task);
                LogExt.Message(String.Format("Задача {0} удалена из списка.", task.Id));
            }

        }
        public async Task OnStopAsync()
        {
            LogExt.Message("Сервис останавливается.");
            IsStop = true;

            LogExt.Message("Выставлен IsStop.");
            
            var arr = ActiveTask.ToArray();
            
            var count=arr.Length;
            LogExt.Message(String.Format("Начата остановка сервера. Ожидают завершения {0} задач.",count));
            Task tsk= Task.WhenAll (arr);
            tsk.Wait();
            LogExt.Message("Завершено ожидание останова работающих задач. Сервис остановлен.");
        }
    }
}



