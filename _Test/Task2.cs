using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Test
{
    class Task2
    {
       
        public async void test()
        {
            //await NamedMethod();
            //await unnamed();
            await Test2();
            Console.WriteLine("Конец всему");
        }

        async Task Test2()
        {
            Task tsk1, tsk2;
           
            tsk1=Task.Delay(5000);
            Console.WriteLine("{0}  ", tsk1.Status);
            await tsk1;
            Console.WriteLine("{0}  ", tsk1.Status);
            tsk2 = WorkAsync();
            Console.WriteLine("{0}  {1}", tsk1.Status, tsk2.Status);
            await tsk2;
            
            Console.WriteLine("{0}  {1}", tsk1.Status, tsk2.Status);
        }
        async Task WorkAsync()
        {
            Console.WriteLine("Начало Work");
            await Task.Delay(7000);
            Console.WriteLine("Конец Work");


        }
    }
}
