using System;
using System.Text;

using System.Net;
using System.Net.Sockets;

using System.Threading.Tasks.Dataflow;

using Com.Okmer.Communication;

namespace Com.Okmer.Communication
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 9999;
            IPAddress host = IPAddress.Parse(args.Length > 0 ? args[0] : "127.0.0.1");

            Console.WriteLine("Tcp chat client");

            Console.WriteLine($"Connecting to host: {host.ToString()}:{port}");

            using (TcpTransceiver transceiver = TcpTransceiver.Connect(host, port))
            {
                Console.WriteLine("Client connected");

                TcpBlock block = new TcpBlock(transceiver);

                block.ReadBlock.LinkTo(new ActionBlock<byte[]>(v =>
                {
                    Console.Write($"{Encoding.ASCII.GetString(v)}");
                }));

                while (transceiver.IsConnected)
                {
                    block.WriteBlock.Post(Encoding.ASCII.GetBytes(Console.ReadLine()));
                }

                Console.WriteLine("Client disconnected");
            }
        }
    }
}
