using MakePacketTemplate.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakePacketTemplate
{
    public class Program
    {
        static void Main(string[] args)
        {
            DEFAULT_SERVER_HelloPacket packet = new DEFAULT_SERVER_HelloPacket();
            packet.Login1 = new LoginClass();
            packet.Login1.ID = "Hello";
            packet.Login1.Password = "world";
            packet.Login2 = new PacketConvertList<LoginClass>();
            packet.Login2.Enter(new LoginClass() { ID = "Nice", Password = "Meet you" });
            packet.Login2.Enter(new LoginClass() { ID = "Nice2", Password = "Meet you2" });

            var Serialize = packet.Serialize();

            DEFAULT_SERVER_HelloPacket Deserialize = new DEFAULT_SERVER_HelloPacket();
            Deserialize.Desrialize(Serialize);

            Console.WriteLine($"{Deserialize.MsgID}");
            Console.WriteLine($"{Deserialize.Login1.ID}");
            Console.WriteLine($"{Deserialize.Login1.Password}");

            foreach(var item in Deserialize.Login2.Values)
            {
                Console.WriteLine($"{item.ID}");
                Console.WriteLine($"{item.Password}");
            }
        }
    }
}
