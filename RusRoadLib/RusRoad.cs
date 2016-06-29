using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace RusRoadLib
{
    public class RusRoad
    {
      
        private int port = 8888;
        private string ipServer = "127.0.0.1";

        TcpListener tcpListener;
        HashSet<Task> ActiveTask = new HashSet<Task>();
        bool IsStop = false;
        //public static RusRoadsData DB;

        public RusRoad():this("127.0.0.1", 8888) { } //конструктор без параметров
        public RusRoad(string Ip, int port)  // конструктор
        {
            this.ipServer = Ip;
            this.port = port;
            tcpListener = new TcpListener(IPAddress.Parse(ipServer), port);
            //DB = new RusRoadsData();
        } 
        
        
        // Старт прослущивания порта на сервере
        public async void OnStartAsync()
        {
 
            LogExt.Message("Служба сбора данных от датчиков запущена.");
            // Старт прослущивания портов на сервере
            ListeningSensorsAsync();
           
        }
        public async void ListeningSensorsAsync()
        {
            
            
            tcpListener.Start();
            LogExt.Message("Сервер ожидает подключений");
            while (!IsStop)
            {
                var tcpClient = await tcpListener.AcceptTcpClientAsync();
                // Запуск асинхроммой задачи для проведения сессии с клиентом
                if (IsStop) { break; };
                var tsk = ReceivingAndProcessingAsync(tcpClient); // await не нужен
                ProcessTaskAsync(tsk);
            }
           
        }

        //управление получением и обработкой данных с датчиков
        async Task ReceivingAndProcessingAsync(TcpClient c)
        {
            var dataSensorHandler = new DataSensorHandler(c);
            LogExt.Message("[Сервер :] Запус процесса чтения данных");
            bool isRead=await dataSensorHandler.ReadWithTimeoutAsync();
            
            LogExt.Message("[Сервер :] Завершен процесс чтения данных");
            c.Close(); //Говорим что TcpClient нам больше не нужен, делаем запрос на закрытие соединения
            if (isRead)
            {
                // и при закрытом соединении спокойно обрабатываем данные 
                LogExt.Message("[Сервер :] Обработка и запись данных");
                await dataSensorHandler.HandlerDataAsync(); 
            }
            
           
        }

        // Останов сервера
        public async void OnStopAsync()
        {
            LogExt.Message("Начало останова cлужбы сбора данных от датчиков.");
            IsStop = true;
            tcpListener.Stop();
            // ожидание завершения задач
            var arr = ActiveTask.ToArray();

            var count = arr.Length;
            LogExt.Message(String.Format("Ожидают завершения {0} задач.", count));
            Task.WhenAll(arr).Wait();
            LogExt.Message("Завершено ожидание останова работающих задач. Сервис остановлен.");

        }

        async Task ProcessTaskAsync(Task task)
        {
            try
            {
                LogExt.Message(String.Format("Задача {0} помещена в список.", task.Id));
                ActiveTask.Add(task);
                await task;
            }
            finally
            {
                ActiveTask.Remove(task);
                LogExt.Message(String.Format("Задача {0} удалена из списка.", task.Id));
            }

        }

        public bool CheckingAccessDb()
        {
            
            bool result = true;
            using (RusRoadsData db = new RusRoadsData())
            {
               
                if (!db.Database.Exists())
                {
                    LogExt.Message("Базы данных указанной в строке подключения "+ 
                        db.Database.Connection.ConnectionString+" не существует.\n", LogExt.MesLevel.Error);
                    result = false;
                    return result;
                }
               
                try
                {
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
                catch (Exception ex)
                {

                    LogExt.Message(LogExt.ExeptionMes(ex, "Ошибка доступа к базе данных."), LogExt.MesLevel.Error);
                    result = false;
                }

            }
            return result;
        }


    }
}
