using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class StartUpIpEndPoint
    {
        public string IPAddress;
        public int Port;
    }

    public class IPConfig
    {
        public StartUpIpEndPoint TCPIPEndPoint = new StartUpIpEndPoint();
        public StartUpIpEndPoint UDPIPEndPoint = new StartUpIpEndPoint();
    }

    public class IpConfigManager
    {
        private string IPConfigFile = "ipConfig.config";

        private static IpConfigManager inst = null;
        public static IpConfigManager Instance => inst ?? (inst = new IpConfigManager());

        public IPConfig ipConfig = null;

        public IpConfigManager()
        {
            if (!File.Exists(IPConfigFile))
            {
                ipConfig = new IPConfig();
                ipConfig.TCPIPEndPoint.IPAddress = "127.0.0.1";
                ipConfig.TCPIPEndPoint.Port = 5555;

                ipConfig.UDPIPEndPoint.IPAddress = "127.0.0.1";
                ipConfig.UDPIPEndPoint.Port = 5556;

                string json = JsonConvert.SerializeObject(ipConfig, Formatting.Indented);
                File.WriteAllText(IPConfigFile, json);
            }
            else
            {
                string IPConfigText = File.ReadAllText(IPConfigFile);

                ipConfig = JsonConvert.DeserializeObject<IPConfig>(IPConfigText);
            }
        }
    }
}
