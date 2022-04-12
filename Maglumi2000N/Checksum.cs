using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maglumi2000N
{
    
        public class Checksum
        {
            public const byte ENQ = 5;
            public const byte ACK = 6;
            public const byte NAK = 21;
            public const byte EOT = 4;
            public const byte ETX = 3;
            public const byte ETB = 23;
            public const byte STX = 2;
            public const byte NEWLINE = 10;
            public static string GetCheckSumValue(string frame)
            {
                string checksum = "00";

                int byteVal = 0;
                int sumOfChars = 0;
                bool complete = false;

                //take each byte in the string and add the values
                for (int idx = 0; idx < frame.Length; idx++)
                {
                    byteVal = Convert.ToInt32(frame[idx]);

                    switch (byteVal)
                    {
                        case STX:
                            sumOfChars = 0;
                            break;
                        case ETX:
                        case ETB:
                            sumOfChars += byteVal;
                            complete = true;
                            break;
                        default:
                            sumOfChars += byteVal;
                            break;
                    }

                    if (complete)
                        break;
                }

                if (sumOfChars > 0)
                {
                    //hex value mod 256 is checksum, return as hex value in upper case
                    checksum = Convert.ToString(sumOfChars % 256, 16).ToUpper();
                }
                //if checksum is only 1 char then prepend a 0
                return (string)(checksum.Length == 1 ? "0" + checksum : checksum);
            }
        }
}
