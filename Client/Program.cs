using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UDPFileTransfer;
using UDPFileSender = UDPFileTransfer.UDPFileSender;

namespace Client
{
	class Program
	{
		static void Main(string[] args)
		{
			var client = new UDPFileTransfer.Client("127.0.0.1", 8990, 8991);
			client.SendFile(new FileModel("test", "test1", 4, 1032));
			Console.Read();
		}
	}
}
