using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RusRoadClient
{
    class Program
    {
        const int port = 8888;
        const string address = "127.0.0.1";
        static void Main(string[] args)
        {
            TcpClient client = null;


            client = new TcpClient(address, port);
            using (NetworkStream stream = client.GetStream())
            {

                //string path = @"e:\_CSharp\Project\ClientServerOKR\Счет.pdf";
                string path = @"..\..\..\passage.txt";

                var len = client.SendBufferSize;
                byte[] buffer = new byte[len];

                Console.WriteLine("Начата передача файла");
                using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    int count = 0;
                    while (true)
                    {
                        int l = fstream.Read(buffer, 0, len);
                        if (l <= 0) break;
                        stream.Write(buffer, 0, l);
                        count += l;
                    }
                    
                    
                    Console.WriteLine("Предача файла завершена. Передано {0} байт ", count);
                }
                
            }
            //Console.ReadLine();

            if (client != null) client.Close();
        }
    }

}

