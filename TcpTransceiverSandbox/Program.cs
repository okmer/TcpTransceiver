using System;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.Net.Sockets;

using Com.Okmer.Communication;

namespace TcpTransceiverSandbox
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Tcp Transceiver Sandbox");

            var host = new TcpTransceiverHost(9999);
            host.Start();

            Task hostWriter = Task.Run(() =>
            {
                int counter = 0;
                while (true)
                {
                    host.WriteData(Encoding.ASCII.GetBytes($"Test text number {counter++}{Environment.NewLine}"));
                }
            });

            var client = new TcpTransceiverClient(IPAddress.Loopback, 9999);
            client.Connect();

            Task clientReader = Task.Run(() =>
            {
                while (true)
                {
                    byte[] data = client.ReadData(1024);
                    Console.WriteLine(Encoding.ASCII.GetString(data));
                }
            });

            Console.ReadLine();
        }
    }
}
