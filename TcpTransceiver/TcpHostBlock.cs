using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;


using System.Net;
using System.Net.Sockets;

namespace Com.Okmer.Communication
{
    public class TcpHostBlock
    {
        protected TcpTransceiverHost Host { set; get; }

        public BroadcastBlock<byte[]> ReadBlock = new BroadcastBlock<byte[]>(v => v);
        public ActionBlock<byte[]> WriteBlock;

        public TcpHostBlock(int port)
        {
            Host = new TcpTransceiverHost(port);

            WriteBlock = new ActionBlock<byte[]>(v =>
            {
                Host.WriteData(v);
            });

            Task.Run(() =>
            {
                Host.Start();

                while (Host.IsRunning)
                {
                    byte[] data = Host.ReadData();
                    ReadBlock.Post(data);
                }
            });
        }
    }
}
