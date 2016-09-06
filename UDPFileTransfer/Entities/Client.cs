using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UDPFileTransfer.Entities;

namespace UDPFileTransfer
{
	public class Client : UdpBase
	{
		private FileModel _fileModel;
		private readonly int _chinkSize;
		private byte[] _buffer;
		
		public Client(string clientAddress, int clientPort, int serverPort)
			: base (clientAddress, clientPort, serverPort)
		{
            _chinkSize = 20000;
			_buffer = new byte[_chinkSize];
		}

		public void SendFile(string path)
		{
			//Set FileModel of file that will be sended
			_fileModel = new FileModel(path, _chinkSize);
            _fileModel.Generate();

			//Send file headers
			SendFileHeader();

			//Send file content
			SendFileContent();
		}

		public void SendFileHeader()
		{
			var packet = new Packet();
			packet.PushString(_fileModel.Name);
			packet.PushString(_fileModel.FullPath);
			packet.PushInt(_fileModel.ChunkCount);
			packet.PushInt(_fileModel.ChunkSize);
			Send(packet);

		    Console.WriteLine(Receive().PullBool()
		        ? "Server succesfully received fileheaders"
		        : "Server hasn't recieved fileheaders");
		}

		public void SendFileContent()
		{
			var chunksToSend = _fileModel.ChunksToSend();
		    Packet response;

			foreach (var chunk in chunksToSend)
			{
			    var info = _fileModel.GetChunk(chunk);
                Send(FileChunk.CreatePacket(info));
			    response = Receive();
			    while (info.Index != response.PullInt())
                {
                    Send(FileChunk.CreatePacket(info));
                    response = Receive();
                }
                Console.WriteLine($"Chunk sended {info.Index} of {_fileModel.ChunkCount}");
            }

            response = new Packet();
            response.PushString("<end>");
            Send(response);
		    Receive();
		}
	}
}
