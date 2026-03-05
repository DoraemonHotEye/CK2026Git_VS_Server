using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.DebugPoint
{
    public class DebugConsole
    {
        public static object _lock = new object();

        public static void PrintWrite(string Message)
        {
            lock (_lock)
            {
                Console.Write("[Server]: ");
                Console.WriteLine(Message);
            }
        }
    }
}
