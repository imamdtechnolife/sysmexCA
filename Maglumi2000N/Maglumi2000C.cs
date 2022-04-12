using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;

namespace Maglumi2000N
{
    public static class Maglumi2000C
    {
      
         private static StringBuilder sb = new StringBuilder();
        public const string MachineName = "SysmexCA600";
        private const int wait = 3000;
        public static SerialPort sp;
        private static EventWaitHandle wh;
        public static int flag = 0;
        public static void StartReceiver(string comPort, int baudRate = 9600)
        {
            Console.WriteLine("Port : "+comPort);
            Maglumi2000C.sp = new SerialPort(comPort);
            Maglumi2000C.sp.BaudRate = baudRate;
            Maglumi2000C.sp.Parity = Parity.None;
           // Maglumi2000C.sp.Handshake = Handshake.None;
            Maglumi2000C.sp.DataBits = 8;
            Maglumi2000C.sp.StopBits = StopBits.One;
            Maglumi2000C.sp.DataReceived += new SerialDataReceivedEventHandler(Maglumi2000C.sp_DataReceived);
            Maglumi2000C.sp.Open();
        }
        private static void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine("Data Incomming");
            Maglumi2000C.Eval(Maglumi2000C.sp.ReadLine());
        }
        public static void Eval(string data)
        {
            Console.WriteLine("Data Incomming2");
            Console.WriteLine(data);
            if(data==Constants.enq||data.StartsWith(Constants.enq))
            {
                Maglumi2000C.sb.Clear();
                Maglumi2000C.sp.Write(Constants.ack);
            }
            else if(data==Constants.ack)
            {
                Maglumi2000C.flag = 1;
            }
            else if(data==Constants.stx||data.StartsWith(Constants.stx))
            {
                Maglumi2000C.sb.Append(data);
                Maglumi2000C.sp.Write(Constants.ack);
            }
            else if(data==Constants.eot||data.Contains(Constants.eot)||data.EndsWith(Constants.eot))
            {
                Console.WriteLine("Found End of transmission");
                //if (sb.ToString().Length > 100)
                //{
                    try
                    {
                        Maglumi2000C.sp.Write(Constants.ack);
                        using (StreamWriter streamWriter = new StreamWriter(Constants.DumpPath + "SysmexCA600_" + DateTime.Now.Ticks.ToString() + ".txt"))
                            streamWriter.Write(Maglumi2000C.sb.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ERROR (Write Error): " + ex.Message);
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                //}
            }
            else
            {
                Maglumi2000C.sb.Append(data);
            }
        }
    }
    
}
