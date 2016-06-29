using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RusRoadLib;

namespace _OkrDB
{
    class SyncTest
    {
        async public static void SingleAsync()
        {
            using (RusRoadsData db = new RusRoadsData())
            {
                var p1 = new Passage { Highway_Id = 3, CarOwner_Id = 2, Time = DateTime.Now, Speed = 202 };
                db.Passage.Add(p1);
                await db.SaveChangesAsync();
                DispPassage();


            }
        }
        async public static void Single2Async()
        {
            using (RusRoadsData db = new RusRoadsData())
            {
                db.Database.Exists();
                try
                {
                    var com = db.RusRoadCommon.Find(1);
                    if (com != null)
                    {
                        com.Test = DateTime.Now;
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        LogExt.Message("Не удалось прочитать из базы данных сведения о системе.",LogExt.MesLevel.Error);
                    }
                }
                catch (Exception ex)
                {

                    LogExt.Message(LogExt.ExeptionMes(ex, "Ошибка доступа к базе данных."),LogExt.MesLevel.Error); 
                }

            }
        }
        public static void DispPassage()
        {
            using (RusRoadsData db = new RusRoadsData())
            {
                Console.WriteLine("Таблица Passage");
                foreach (var p in db.Passage)
                {

                    Console.WriteLine("{0} {1} {2} {3} {4}", p.Passage_Id, p.Time, p.Highway_Id, p.CarOwner_Id, p.Speed);
                }
            }
        }
    }
}
