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

            Console.WriteLine("Tcp chat host");

            Console.WriteLine($"Waiting on a client on port: {port}");

            using(TcpTransceiver transceiver = TcpTransceiver.Listen(port))
            {
                Console.WriteLine("Client connected");

                TcpBlock block = new TcpBlock(transceiver);

                block.ReadBlock.LinkTo(new ActionBlock<byte[]>(v =>
                {
                    Console.Write($"{Encoding.ASCII.GetString(v)}");
                }));

                while(transceiver.IsConnected)
                {
                    block.WriteBlock.Post(Encoding.ASCII.GetBytes(Console.ReadLine()));
                }

                Console.WriteLine("Client disconnected");
            }
        }
    }
}
