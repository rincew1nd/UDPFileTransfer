using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPFileTransfer
{
	public class Server
	{
		private UdpClient _udpClient;
		private IPEndPoint _endPoint;

		public Server Start(int port)
		{
			_udpClient = new UdpClient(port);
			_endPoint = new IPEndPoint(IPAddress.Any, 0);

			return this;
		}

		public bool RecieveFile()
		{
		    var packet = new Packet(_udpClient.Receive(ref _endPoint));

            var name = packet.PullString();
            var path = packet.PullString();
		    var chunkSize = packet.PullInt();

            return true;
		}
	}
}
