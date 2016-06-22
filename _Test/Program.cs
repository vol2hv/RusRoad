using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace _Test
{
    class Program
    {
        static Timer tmr = new Timer(TimerProc);
        private static void TimerProc(object state)
        {
            Console.WriteLine( String.Format("Сработал таймер {0:H:mm:ss.fff}", DateTime.Now ));
        }
        static void Main(string[] args)
        {
            Console.WriteLine(System.Reflection.Assembly.GetEntryAssembly().Location);
            tmr.Change(10000, 5000);
            Console.WriteLine(DateTime.Now.ToString());
            Stop();

            Console.ReadLine();
        }
         static async void Stop()
        {
            Task tsk = Task.Delay(20200);
            Console.WriteLine(tsk.Status);
            await tsk;
            Console.WriteLine(String.Format("Таймер унитожен {0:H:mm:ss.fff}", DateTime.Now));
            // tmr.Change(0, 0);
            tmr.Dispose();
        }
    }
}
