using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _OkrDB
{
    class SyncTest
    {
        async public static void  SingleAsync()
        {
            using (RusRoadsData db = new RusRoadsData())
            {
                var p1 = new Passage { Highway_Id = 3, CarOwner_Id = 2, Time = DateTime.Now, Speed =202 };
                db.Passage.Add(p1);
                await db.SaveChangesAsync();
                DispPassage();


            }
        }
        public static void DispPassage()
        {
            using (RusRoadsData db = new RusRoadsData())
            {
                Console.WriteLine("Таблица Passage");
                foreach (var p in db.Passage)
                {
                    
                    Console.WriteLine("{0} {1} {2} {3} {4}",p.Passage_Id,p.Time,p.Highway_Id,p.CarOwner_Id,p.Speed);
                }
            }
        }
    }
}
