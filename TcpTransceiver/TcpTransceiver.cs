using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;


using System.Net;
using System.Net.Sockets;

namespace Com.Okmer.Communication
{
    public class TcpTransceiver : BaseDisposable
    {
        private const int DEFAULT_BUFFER_SIZE = 1024 * 1024;
        private byte[] buffer = new byte[DEFAULT_BUFFER_SIZE];

        protected TcpClient Client { get; set; }
        protected NetworkStream Stream { get; set; }

        protected TcpTransceiver(TcpClient client)
        {
            Client = client;
            Stream = Client.GetStream();
        }

        public bool IsConnected => Client?.Connected ?? false;

        public byte[] ReadData(int buffer_size = DEFAULT_BUFFER_SIZE)
        {
            if (buffer.Length != buffer_size)
            {
                buffer = new byte[buffer_size];
            }
            return ReadData(ref buffer);
        }

        public byte[] ReadData(ref byte[] buffer)
        {
            int numberOfBytes = 0;

            numberOfBytes = Stream?.Read(buffer, 0, buffer.Length) ?? 0;

            return numberOfBytes > 0 ? buffer.Take(numberOfBytes).ToArray() : new byte[0];
        }

        public void WriteData(byte[] buffer)
        {
            Stream?.Write(buffer, 0, buffer.Length);
        }

        protected override void DisposeManaged()
        {
            Stream?.Dispose();
            Stream = null;
        }

        static public TcpTransceiver Connect(IPAddress host, int port)
        {
            var client = new TcpClient();
            client.Connect(host, port);

            return new TcpTransceiver(client);
        }

        static public TcpTransceiver Listen(int port)
        {
            var listener = new TcpListener(IPAddress.Any, port);

            listener.Start();
            var client = listener.AcceptTcpClient();
            listener.Stop();

            return new TcpTransceiver(client);
        }
    }
}
