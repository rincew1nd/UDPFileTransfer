using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPFileTransfer
{
	public class Server : UdpBase
	{
		public FileModel _fileModel;

		public Server(string clientAddress, int clientPort, int serverPort)
			: base (clientAddress, clientPort, serverPort)
		{
		}

		public void RecieveFile()
		{
			//Recieve file headers
			ReceiveFileHeaders();

			//Receive file bytes
			ReceiveFileBytes();

			//Save file
			_fileModel.SaveFile("E:\\testshit1");
		}

		private void ReceiveFileHeaders()
		{
			var packet = Receive();

			var name = packet.PullString();
			var path = packet.PullString();
			var indexCount = packet.PullInt();
			var chunkSize = packet.PullInt();

			_fileModel = new FileModel(path, chunkSize) { ChunkCount = indexCount};
			Console.WriteLine($"Recieved {name} with location {path}, index count {indexCount} and chunk size {chunkSize}!");
		}

	    private void ReceiveFileBytes()
        {
            var packet = new Packet();
            packet.PushBool(true);
            Send(packet);

            while (true)
            {
                try
                {
                    var info = Receive();

                    if (info.PullString() == "<end>")
                        break;
                    else 
                        info.ResetPointer();

                    _fileModel.AcceptPacket(info);

                    var response = new Packet();
                    response.PushInt(info.PullInt());
                    Send(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //private void ReceiveFileBytes()
        //{
        //	var chunksToReceive = new List<int>();
        //	ReceiveChunksCount(ref chunksToReceive);
        //    while (chunksToReceive.Count != 0)
        //	{
        //		var receivedPackets = new List<int>();
        //		for(int i = 0; i < chunksToReceive.Count; i++)
        //		{
        //			try
        //			{
        //				var filePacket = new FilePacket(Receive());
        //				_fileModel.AcceptPacket(filePacket);
        //				receivedPackets.Add(filePacket.Index);
        //			}
        //			catch (SocketException)
        //			{
        //				var packet = new Packet();
        //				foreach (var received in receivedPackets)
        //					packet.PushInt(received);
        //				Send(packet);
        //				break;
        //            }
        //		}
        //		ReceiveChunksCount(ref chunksToReceive);
        //	}
        //}

        public void ReceiveChunksCount(ref List<int> chunks)
		{
			var packet = Receive();
			chunks = new List<int>();

			while (packet.IsAnythingLeft(4))
				chunks.Add(packet.PullInt());
		}
	}
}
