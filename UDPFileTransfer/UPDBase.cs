using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPFileTransfer
{
	public class UdpBase
	{
		protected UdpClient _udpReceiver;
		protected UdpClient _udpSender;
		protected IPEndPoint _endPoint;

		protected UdpBase(string clientAddress, int clientPort, int serverPort)
		{
			_udpReceiver = new UdpClient(serverPort);
			_endPoint = new IPEndPoint(IPAddress.Any, 0);
			_udpReceiver.Client.SendTimeout = 10000;
			_udpReceiver.Client.ReceiveTimeout = 10000;

			_udpSender = new UdpClient();
			_udpSender.Connect(clientAddress, clientPort);
			_udpSender.Client.SendTimeout = 10000;
			_udpSender.Client.ReceiveTimeout = 10000;
		}

		protected Packet Receive()
		{
			try
			{
				return new Packet(_udpReceiver.Receive(ref _endPoint));
			}
			catch
			{
				throw;
			}
		}

		protected void Send(Packet packet)
		{
			try
			{
				var bytes = packet.Build();
				_udpSender.Send(bytes, bytes.Length);
			}
			catch
			{
				throw;
			}
		}
	}
}
