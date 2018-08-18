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

            //Client read to console
            clientBlock.ReadBlock.LinkTo(new ActionBlock<byte[]>(v =>
            {
                Console.Write($"{Encoding.ASCII.GetString(v)}");
            }));

            //Host read -> write loop back :-)
            hostBlock.ReadBlock.LinkTo(hostBlock.WriteBlock);

            //Client write to host                           
            Task.Run(() =>
            {
                int counter = 0;
                while (true)
                {
                    clientBlock.WriteBlock.Post(Encoding.ASCII.GetBytes($"Test {counter++}{Environment.NewLine}"));
                }
            });

            Console.ReadLine();
        }
    }
}
