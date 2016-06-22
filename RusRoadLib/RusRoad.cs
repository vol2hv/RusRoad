using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;

namespace RusRoadLib
{
    public class RusRoad
    {
      
        private int port = 8888;
        private string ipServer = "127.0.0.1";

        TcpListener tcpListener;
        HashSet<Task> ActiveTask = new HashSet<Task>();
        Boolean IsStop = false;
        

        public RusRoad():this("127.0.0.1", 8888) { } //конструктор без параметров
        public RusRoad(string Ip, int port)  // конструктор
        {
            this.ipServer = Ip;
            this.port = port;
            tcpListener = new TcpListener(IPAddress.Parse(ipServer), port);
            
        } 
        
        
        // Старт прослущивания порта на сервере
        public async void OnStartAsync()
        {
            
            // Старт прослущивания порта на сервере
            
            tcpListener.Start();  //где делать стоп
            while (true) // как выходить
            {
                var tcpClient = await tcpListener.AcceptTcpClientAsync();
                LogExt.Message("Сервер ожидает подключений");

                // Запуск асинхроммой задачи для проведения сессии с клиентом
                var tsk = ReceivingAndProcessingAsync(tcpClient); // await не нужен
                ProcessTaskAsync(tsk);

            }

        }
        
        //управление получением и обработкой данных с датчиков
        async Task ReceivingAndProcessingAsync(TcpClient c)
        {
            var dataSensorHandler = new DataSensorHandler(c,db);
            LogExt.Message("[Сервер :] Запус процесса чтения данных");
            bool isRead=await dataSensorHandler.ReadWithTimeoutAsync();
            
            LogExt.Message("[Сервер :] Завершен процесс чтения данных");
            c.Close(); //Говорим что TcpClient нам больше не нужен, делаем запрос на закрытие соединения
            if (isRead)
            {
                // и при закрытом соединении спокойно обрабатываем данные await не нужен
                await dataSensorHandler.HandlerDataAsync();
            }
            
           
        }

        // Останов сервера
        public async void OnStopAsync()
        {
            tcpListener.Stop();
            IsStop = true;
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


    }
}
