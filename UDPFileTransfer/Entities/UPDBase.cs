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
		protected UdpClient UdpReceiver;
		protected UdpClient UdpSender;
		protected IPEndPoint EndPoint;
	    protected int RetryCount;

		protected UdpBase(string clientAddress, int clientPort, int serverPort, int timeout = 5000, int retryCount = 5)
		{
			UdpReceiver = new UdpClient(serverPort);
			EndPoint = new IPEndPoint(IPAddress.Any, 0);
			UdpReceiver.Client.SendTimeout = timeout;
			UdpReceiver.Client.ReceiveTimeout = timeout;

			UdpSender = new UdpClient();
			UdpSender.Connect(clientAddress, clientPort);
			UdpSender.Client.SendTimeout = timeout;
			UdpSender.Client.ReceiveTimeout = timeout;
            
		    RetryCount = retryCount;
		}

		protected Packet Receive()
		{
		    var i = 0;
		    while (i < RetryCount)
            {
                try
                {
                    i++;
                    return new Packet(UdpReceiver.Receive(ref EndPoint));
                }
                catch
                {
                    if (i == RetryCount - 1)
                        throw;
                }
            }

		    return null;
		}

		protected void Send(Packet packet)
        {
            var i = 0;
            while (i < RetryCount)
            {
                try
                {
                    i++;
                    var bytes = packet.Build();
                    UdpSender.Send(bytes, bytes.Length);
                }
                catch (Exception ex)
                {
                    if (i == RetryCount - 1)
                        throw;
                }
            }
		}
	}
}
