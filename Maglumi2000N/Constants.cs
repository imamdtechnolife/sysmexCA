using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maglumi2000N
{
    public static class Constants
    {
        public static string soh = char.ConvertFromUtf32(1);
        public static string stx = char.ConvertFromUtf32(2);
        public static string etx = char.ConvertFromUtf32(3);
        public static string eot = char.ConvertFromUtf32(4);
        public static string enq = char.ConvertFromUtf32(5);
        public static string ack = char.ConvertFromUtf32(6);
        public static string nack = char.ConvertFromUtf32(21);
        public static string etb = char.ConvertFromUtf32(23);
        public static string lf = char.ConvertFromUtf32(10);
        public static string cr = char.ConvertFromUtf32(13);
        public static string DumpPath = "D:\\SysmexCA600DumpPath\\";
        public static readonly string[] LowNames = new string[32]
    {
      "<NUL>",
      "<SOH>",
      "<STX>",
      "<ETX>",
      "<EOT>",
      "<ENQ>",
      "<ACK>",
      "<BEL>",
      "<BS>",
      "<HT>",
      "<LF>",
      "<VT>",
      "<FF>",
      "<CR>",
      "<SO>",
      "<SI>",
      "<DLE>",
      "<DC1>",
      "<DC2>",
      "<DC3>",
      "<DC4>",
      "<NAK>",
      "<SYN>",
      "<ETB>",
      "<CAN>",
      "<EM>",
      "<SUB>",
      "<ESC>",
      "<FS>",
      "<GS>",
      "<RS>",
      "<US>"
    };
    }
}
