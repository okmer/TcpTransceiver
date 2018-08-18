using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;


using System.Net;
using System.Net.Sockets;

namespace Com.Okmer.Communication
{
    public class TcpClientBlock
    {
        protected TcpTransceiverClient Client { set; get; }

        public BroadcastBlock<byte[]> ReadBlock = new BroadcastBlock<byte[]>(v => v);
        public ActionBlock<byte[]> WriteBlock;

        public TcpClientBlock(IPAddress host, int port)
        {
            Client = new TcpTransceiverClient(host, port);

            WriteBlock = new ActionBlock<byte[]>(v => 
            {
                Client.WriteData(v);
            });

            Task.Run(() =>
            {
                Client.Connect();

                while(Client.IsConnected)
                {
                    byte[] data = Client.ReadData();
                    ReadBlock.Post(data);
                }
            });
        }
    }
}
