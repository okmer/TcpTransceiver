using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;


using System.Net;
using System.Net.Sockets;

namespace Com.Okmer.Communication
{
    public class TcpTransceiverHost
    {
        public IPAddress Host { get; private set; }
        public int Port { get; private set; }

        protected TcpListener Listener { get; set; }
        protected TcpClient Client { get; set; }
        protected NetworkStream Stream { get; set; }

        public TcpTransceiverHost(int port)
        {
            Host = IPAddress.Any;
            Port = port;
            Listener = new TcpListener(Host, Port);
        }

        public bool IsConnected => Client?.Connected ?? false;

        public void Start()
        {
            Listener.Start();
            Listener.AcceptTcpClientAsync().ContinueWith(t =>
            {
                Client = t.Result;
                Stream = Client?.GetStream();
            });
        }

        public void Stop()
        {
            Listener.Stop();

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
