using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OkrDB
{
    class Program
    {

        private static byte[] file = new byte[1]; // место для сбора всего получаемого файла
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private enum fields : int { Govnumber = 0, Highway_Id, Time, Speed };
        private static string path = @"e:\_CSharp\Project\RusRoad\passage.txt";
        
        // формирование сообщения об ошибке в LogExt
        private static string ErrorMes(string pass, string cause, Exception ex)
        {
            string s = "Проезд " + pass + " не записан в базу данных \n" + cause;
            string s1 = ex == null ? "" : "Сообщение : " + ex.Message + "\n" +
            "Внутреннее исключение: " + ex.InnerException.InnerException.Message + "\n" +
            "Стек вызова: " + "\n" + ex.StackTrace;

            return s + s1 + "\n";
        }
        // формирование текста уведомления в LogExt
        static string Notification(string fio, string rName, int speed,  int mSpeed, DateTime dt)
        {
            string n = "\n";
            string str = "Уважаемый {0}!" + n;
            str += "К нашему сожалению, Вы передвигались по дороге {1} со скоростью {2} км./ час." + n;
            str += "и превысили допустимую скорость для этой дороги {3} км./час." + n;
            str += "Нарушение зафиксировано {4}" + n;
            str += "К Вашему сожалению, за это нарушение Вам необходимо заплатить штраф в ближайшем отделение ГИБДД. " + n;
            return String.Format(str, fio, rName, speed,mSpeed,dt);
        }


        static void Main(string[] args)
        {

            //SyncTest.SingleAsync();
            // Research();
            //RunAsync(); // из Main() c await вызов не возможен
            Report();

            Console.ReadLine();

        }
        static async void RunAsync()
        {
            ReadFile(path);
            await HandlerDataAsync();

        }


        static void Research()
        {
            using (RusRoadsData db = new RusRoadsData())
            {
                //var h1 = new Highway {Name="М5 Большая дорога",Speed=222};
                //db.Highway.Add(h1);
                //var a = db.SaveChanges();
                //Console.WriteLine("Изменения сохранены с кодом {0}", a);
                foreach (Highway h in db.Highway)
                    Console.WriteLine("{0} {1}", h.Name, h.Speed);
                //var p1 = new Passage { Time = DateTime.Now, Highway_Id = 3, CarOwner_Id = 2, Speed = 140 };
                //db.Passage.Add(p1);
                //db.SaveChanges();
                var carOwner = db.CarOwner;
                var cars = from c in carOwner
                           where c.CarOwner_Id > 0
                           select new { c.CarOwner_Id, c.Govnumber, c.Name, a33 = c.CarOwner_Id * 3 + 3 };
                var cars1 = db.CarOwner.Where(c => (c.CarOwner_Id > 0) && (c.CarOwner_Id % 2 == 0));
                foreach (var c in cars1)
                {
                    Console.WriteLine("{0} {1} {2}", c.CarOwner_Id, c.Govnumber, c.Name);
                }

                var cars2 = from c in carOwner
                            where c.Govnumber == "22А888АА36"
                            select new { c.CarOwner_Id, c.Govnumber, c.Name };

                var cl = cars2.ToList();
                var a = cl[0].CarOwner_Id;
                Console.WriteLine("{0}", a);
                var p1 = new Passage { Highway_Id = 5, CarOwner_Id = a, Time = DateTime.Now, Speed = 234 };
                DateTime dd = DateTime.Now;
                if (DateTime.TryParse("26.05.2016 13:30:19", out dd))
                {
                    Console.WriteLine("Время {0}", dd);
                }

                db.Passage.Add(p1);
                db.SaveChanges();
                try
                {
                    var ff = "ssss"[101];
                }
                catch (Exception ex)
                {
                    string fstr = "исключение однако \n" + ex.Message + "\n" + ex.StackTrace;
                    logger.Error(fstr);
                }

                //var contacts = from c in context.Contacts
                //               where c.FirstName == "Robert"
                //               select new { c.Title, c.FirstName, c.LastName }
            }
        }
        static void ReadFile(string nameFile)
        {
            FileInfo fileInfo = new FileInfo(nameFile);
            var len = (int)fileInfo.Length;
            Array.Resize(ref file, len);
            using (FileStream fstream = new FileStream(nameFile, FileMode.Open))
            {
                fstream.Read(file, 0, len);
            }
            // использовалось для проверки чтения длинных файлов
            //using (FileStream outstream = new FileStream(@"e:\_CSharp\Project\RusRoad\Programming_Entity_Framework_2nd_Edition1.pdf", FileMode.OpenOrCreate))
            //{
            //    outstream.Write(largeBuffer, 0, len);
            //}
        }
        private static async Task HandlerDataAsync()
        {
            using (StringReader text = new StringReader(Encoding.UTF8.GetString(file)))
            {
                using (RusRoadsData db = new RusRoadsData())   //!!! открывать данные надо при старте сервера и закрывать их при остановке
                {
                    while (true)
                    {
                        string str = await text.ReadLineAsync();
                        if (str == null) { break; }
                        Console.WriteLine(str);
                        await WritePassage(str, db);
                    }
                }
            }
        }
        private static async Task WritePassage(string str, RusRoadsData db)
        {
            string[] items = str.Split(new char[] { ',' });
            var p1 = new Passage();
            string w = items[(int)fields.Govnumber];
            var cars = from c in db.CarOwner
                       where c.Govnumber == w
                       select new { c.CarOwner_Id };
            if (cars.Count() > 0)
            {
                p1.CarOwner_Id = cars.ToList()[0].CarOwner_Id;
            }
            else
            {
                logger.Error(ErrorMes(str, "Госномер " + w + " отсутствует в системе", null));
                return;
            };
            DateTime dt = new DateTime();
            w = items[(int)fields.Time];
            if (DateTime.TryParse(w, out dt))
            {
                p1.Time = dt;
            }
            else
            {
                logger.Error(ErrorMes(str, "Не правильный формат даты " + w, null));
                return;
            }
            p1.Highway_Id = int.Parse(items[(int)fields.Highway_Id]);
            p1.Speed = int.Parse(items[(int)fields.Speed]);
            var pps = db.Passage.Add(p1);


            try
            {

                await db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                db.Passage.Local.Remove(pps);
                logger.Error(ErrorMes(str, "Другая ошибка ", ex));
                //Console.WriteLine("Сообщение : "+ex.Message);

            }
        }
        static void Report()
        {
            using (RusRoadsData db = new RusRoadsData())
            {
                var result = from passage in db.Passage
                             join carOwner in db.CarOwner on passage.CarOwner_Id equals carOwner.CarOwner_Id
                             join highway in db.Highway on passage.Highway_Id equals highway.Highway_Id
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
                    StreamWriter sw = new StreamWriter("Report.csv.");
                    foreach (var r in result)
                    {
                        var ss = String.Format("{0},{1},{2},{3},{4},{5}", r.fio, r.Govnumber, r.Name, r.maxSpeed, r.Time, r.Speed);
                        sw.WriteLine(ss);
                        // Формирование уведомлений
                        var s = @"..\..\..\_Notification\" + r.fio.Trim().Replace(" ","_") + ".txt";

                        StreamWriter sw1 = new StreamWriter(s, true);
                        ss = Notification(r.fio, r.Name, r.Speed,r.maxSpeed,r.Time);
                        sw1.WriteLine(ss);
                        sw1.Close();
                        //Console.WriteLine(ss);

                    }
                    sw.Close();
                }
                catch (Exception e)
                {

                    Console.WriteLine("Exception: " + e.Message);
                }


            }
        }
    }

}

