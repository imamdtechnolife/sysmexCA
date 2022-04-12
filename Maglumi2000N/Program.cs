using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maglumi2000N
{
    class Program
    {
        
            static void Main(string[] args)
            {
                Console.WindowWidth = 120;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Title = "SysmexCA600";
                string stx = char.ConvertFromUtf32(2);
                Console.WriteLine("start of text " + stx);
                string etx = char.ConvertFromUtf32(3);
                Console.WriteLine("end of text " + etx);
                string eot = char.ConvertFromUtf32(4);
                Console.WriteLine("end of transmission " + eot);
                string enq = char.ConvertFromUtf32(5);
                Console.WriteLine("enquiry " + enq);
                string ack = char.ConvertFromUtf32(6);
                Console.WriteLine("acknowledge " + ack);
                Task.Factory.StartNew((Action)(() => FileWatcher.Startwatching()));
                Task.Factory.StartNew((Action)(() => Maglumi2000C.StartReceiver("COM3", 9600)));
                
                 
                Console.WriteLine("Press 'q' to quit.");
                do;
                while (Console.Read() != 113);
            }
    }
}
