using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;


using System.Net;
using System.Net.Sockets;

namespace Com.Okmer.Communication
{
    public class TcpTransceiverClient
    {
        public IPAddress Host { get; private set; }
        public int Port { get; private set; }

        protected TcpClient Client { get; set; }
        protected NetworkStream Stream { get; set; }

        public TcpTransceiverClient(IPAddress host, int port)
        {
            Host = host;
            Port = port;
        }

        public bool IsConnected => Client?.Connected ?? false;

        public bool Connect()
        {
            if (IsConnected) return true;

            Client = new TcpClient();
            Client.Connect(Host, Port);
            Stream = Client.GetStream();

            return IsConnected;
        }

        public void Disconnect()
        {
            Stream?.Dispose();
            Stream = null;
            Client?.Dispose();
            Client = null;
        }

        public byte[] ReadData(int buffer_size = 1024 * 1024)
        {
            byte[] buffer = new byte[buffer_size];
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
    }
}
