using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySupportLibrary.Debug
{
    public class UnitysSupportsDebug
    {
        public static string Prefix = "[UnityTool]";

        static object _lock = new object();

        public static void PrintWriteLine(string msg)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(Prefix);
                Console.ResetColor();
                Console.WriteLine(": " + msg);
            }
        }
    }
}
