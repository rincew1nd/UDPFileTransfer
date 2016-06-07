using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPFileTransfer
{
	public class Client
	{
		private UdpClient _udpClient;
		private int _bufferSize;
		private byte[] _buffer;
		
		public Client Start(string host, int port, int bufferSize)
		{
			_udpClient = new UdpClient();
			_udpClient.Connect(host, port);
			_bufferSize = bufferSize;
			_buffer = new byte[bufferSize];

			return this;
		}

		public bool SendFile(FileModel fileModel)
		{
			var packet = new Packet();
            packet.PushString(fileModel.FileName);
            packet.PushString(fileModel.FilePath);
            packet.PushInt(fileModel.ChunkSize);

		    _buffer = packet.Build();

			_udpClient.Send(_buffer, _buffer.Length);

			return true;
		}
	}
}
