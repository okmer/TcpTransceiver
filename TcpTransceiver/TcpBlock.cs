using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;


using System.Net;
using System.Net.Sockets;

namespace Com.Okmer.Communication
{
    public class TcpBlock
    {
        protected TcpTransceiver Transceiver { set; get; }

        public BroadcastBlock<byte[]> ReadBlock = new BroadcastBlock<byte[]>(v => v);
        public ActionBlock<byte[]> WriteBlock;

        public TcpBlock(TcpTransceiver transceiver)
        {
            Transceiver = transceiver;

            WriteBlock = new ActionBlock<byte[]>(v =>
            {
                Transceiver.WriteData(v);
            });

            Task.Run(() =>
            {
                while (Transceiver.IsConnected)
                {
                    byte[] data = Transceiver.ReadData();
                    ReadBlock.Post(data);
                }
            });
        }
    }
}
