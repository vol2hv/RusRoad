using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RusRoadLib
{
    class RoadsReport  // !!! не отлаживался
    {
        int CountHandler = 288; //число обработок за сутки
        DateTime UpperBound;    // Верхняя граница переода обработки записей (исключительно)
        DateTime BottomBound;    // Нижняя граница переода обработки записей (Включительно) !!! Брать из базы данных
        DateTime BeginTime;
        DateTime EndTime;

        public async Task StartAsync()
        {
            // менеджер формирования отчетов (основной метод)
            while (true)
            {
                DateTime currentTime = DateTime.Now;
                long periodTick = TimeSpan.TicksPerDay / CountHandler;
                long tail = currentTime.TimeOfDay.Ticks % periodTick;
                UpperBound = currentTime - new TimeSpan(tail);
                if (UpperBound > BottomBound)
                {
                    BeginTime = DateTime.Now;
                    // обработка данных
                    await DatabaseDataProcessingAsync();
                    EndTime = DateTime.Now;
                }
                else
                {
                    await Task.Delay((int)(periodTick - tail)/10000);
                }

            }
        }
        public async Task DatabaseDataProcessingAsync()
        {
            Console.WriteLine("Обработка данных базы данных");
            await Task.Delay(10000);
        }

    }
}
