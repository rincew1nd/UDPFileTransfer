using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPFileTransfer
{
	public class Client : UdpBase
	{
		private FileModel _fileModel;
		private int _bufferSize;
		private byte[] _buffer;
		
		public Client(string clientAddress, int clientPort, int serverPort)
			: base (clientAddress, clientPort, serverPort)
		{
			_bufferSize = 80000;
			_buffer = new byte[_bufferSize];
		}

		public void SendFile(FileModel fileModel)
		{
			//Set FileModel of file that will be sended
			_fileModel = fileModel;

			//Send file headers
			SendFileHeader();

			//Send file content
			SendFileContent();
		}

		public void SendFileHeader()
		{
			var packet = new Packet();
			packet.PushString(_fileModel.FileName);
			packet.PushString(_fileModel.FilePath);
			packet.PushInt(_fileModel.IndexCount);
			packet.PushInt(_fileModel.ChunkSize);
			Send(packet);

			if (Receive().PullBool())
				Console.WriteLine("Server succesfully received fileheaders");
			else
				Console.WriteLine("Server hasn't recieved fileheaders");
		}

		public void SendFileContent()
		{
			var chunksToSend = _fileModel.ChunksToSend();
			while (chunksToSend.Count != 0)
			{
				var packet = new Packet();
				
				foreach (var chunk in chunksToSend)
					packet.PushInt(chunk);
				Send(packet);
				//Check for riceiving list of chunks to send

				foreach(var chunk in chunksToSend)
				{
					packet.PushInt(chunk);
					packet.PushBytes(_fileModel.GetChunk(chunk));
					Send(packet);
				}

				//Maybe exception
				packet = Receive();
				var lostChunks = new List<int>();
				while(packet.IsAnythingLeft(4))
				{
					lostChunks.Add(packet.PullInt());
				}
				_fileModel.LostedChunks(lostChunks);

				chunksToSend = _fileModel.ChunksToSend();
			}
		}
	}
}
