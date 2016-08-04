using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using RusRoadLib;
using FluentValidation.Results;
using System.Collections.Generic;

namespace RusRoadLib
{
    // получение и обработка данных от датчика
    class DataSensorHandler : IDisposable
    {
        NetworkStream s;
        TcpClient tcpClient;
        byte[] FileBuf = new byte[1]; // место для сбора всего получаемого файла
        int Lbuf;
        int Timeout = 30000; //Величина тайм-аута
        PassageSrc PassageSrc = new PassageSrc();
        PassageSrcValidator Validator = new PassageSrcValidator();  // валидатор для проверки записи проезда

        private enum fields : int { Govnumber = 0, Highway_Id, Time, Speed };

        // Конструктор
        public DataSensorHandler(TcpClient c)
        {
            s = c.GetStream();
            Lbuf = c.ReceiveBufferSize;
            tcpClient = c;
          
        }
        // Чтение данных
        public async Task ReadDataAsync(CancellationToken ct)
        {

            var buf = new byte[Lbuf];

            int count; int readpos = 0; int writepos = 0;
            while (true)
            {
                count = await s.ReadAsync(buf, 0, Lbuf);
                if (count <= 0) break;

                Array.Resize(ref FileBuf, readpos + count);
                for (int i = 0; i < count; i++)
                {
                    FileBuf[writepos + i] = buf[i];
                }
                readpos += count;
                writepos += count;
                LogExt.Message(string.Format("[Сервер :] прочитано {0} байт", readpos));
            }
        }
        // Чтение данных с тайм-аутом
        public async Task<bool> ReadWithTimeoutAsync()
        {
            using (var cts = new CancellationTokenSource())
            {
                var readTask = ReadDataAsync(cts.Token);
                var timeoutTask = Task.Delay(Timeout);
                await Task.WhenAny(readTask, timeoutTask);
                if (!readTask.IsCompleted)
                {
                    cts.Cancel(); // cancel read task
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        // Обработка принятых данных
        public async Task HandlerDataAsync()
        {
            using (StringReader text = new StringReader(Encoding.UTF8.GetString(FileBuf)))
            {
                {
                    while (true)
                    {
                        string str = await text.ReadLineAsync();
                        if (str == null) { break; }
                        if (str.Substring(0, 1) == "#") { continue; }
                        Console.WriteLine(str);
                        try
                        {
                            await WritePassageAsync(str);
                        }
                        catch (Exception ex)
                        {
                            LogExt.Message(LogExt.ExeptionMes(ex),LogExt.MesLevel.Error);
                           
                        }
                       
                    }
                }
            }
        }
        // Формирование полей и запись в БД сущности Проезд
        private async Task WritePassageAsync(string str)
        {
            // разбить строку и заполнить данные для проверки
            PreparationPassageSrc(str);

            // проверка и логирование
            string allErrors= await CheckPassageAsync();
            if (allErrors != null)
            {
                LogExt.Message( LogExt.ErrorMes(str, allErrors),LogExt.MesLevel.Error);
                return;
            }

            // сформировать данные о проезде в формате базы данных, запись в базу данных
            await WritePassageDBAsync(str);
        }
        async Task<string> CheckPassageAsync()
        {

            string allErrors = null;
            ValidationResult results = await Validator.ValidateAsync(PassageSrc);
            bool validationSucceeded = results.IsValid;

            if (!validationSucceeded)
            
            {
                IList<ValidationFailure> failures = results.Errors;
                allErrors= LogExt.AllErrors(failures);
            }
            return allErrors;
        }
        // Разбиение строки записb о проезде
        private void PreparationPassageSrc(string str)
        {
            string[] items = str.Split(new char[] { ',' });

            PassageSrc.Govnumber = items[(int)fields.Govnumber];
            PassageSrc.Highway_Id = items[(int)fields.Highway_Id];
            PassageSrc.Time = items[(int)fields.Time];
            PassageSrc.Speed = items[(int)fields.Speed];
        }
        // запись строки сущности Проезд в БД
        private async Task WritePassageDBAsync(string str)
        {
            var p1 = new Passage();
            p1.Time = DateTime.Parse(PassageSrc.Time);
            p1.CarOwner_Id = RusRoadSettings.CarOwner_Id;
            p1.Highway_Id = int.Parse(PassageSrc.Highway_Id);
            p1.Speed = int.Parse(PassageSrc.Speed);


            using (RusRoadsData db = new RusRoadsData())
            {
                var pps = db.Passage.Add(p1);
                try
                {

                    await db.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    db.Passage.Local.Remove(pps);
                    string mes = LogExt.ErrorMes(str, "", ex);
                    LogExt.Message(mes, LogExt.MesLevel.Error);

                    //Console.WriteLine("Сообщение : "+ex.Message);

                }
            }
        }

        public void Dispose()
        {
            s.Dispose();
        }
        // Запись полученных данных в файл только для отладки
        public void WriteFile()
        {
            using (FileStream fstream = new FileStream(@"e:\_CSharp\Project\ClientServerOKR\Счет2.pdf", FileMode.Create))
            {
                fstream.Write(FileBuf, 0, FileBuf.Length);
            }
        }
    }
}
