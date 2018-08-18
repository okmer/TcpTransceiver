using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
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

            TcpHostBlock hostBlock = new TcpHostBlock(9999);
            TcpClientBlock clientBlock = new TcpClientBlock(IPAddress.Loopback, 9999);

            clientBlock.ReadBlock.LinkTo(new ActionBlock<byte[]>(v =>
            {
                Console.Write($"{Encoding.ASCII.GetString(v)}");
            }));

            hostBlock.ReadBlock.LinkTo(new ActionBlock<byte[]>(v =>
            {
                Console.Write($"{Encoding.ASCII.GetString(v)}");
            }));

            Task.Run(() => 
            {
                int counter = 0;
                while (true)
                {
                    hostBlock.WriteBlock.Post(Encoding.ASCII.GetBytes($"H: Test {counter++}{Environment.NewLine}"));
                }
            });

            Task.Run(() =>
            {
                int counter = 0;
                while (true)
                {
                    clientBlock.WriteBlock.Post(Encoding.ASCII.GetBytes($"C: Test {counter++}{Environment.NewLine}"));
                }
            });

            Console.ReadLine();
        }
    }
}
