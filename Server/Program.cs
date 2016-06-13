using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDPFileTransfer;

namespace Server
{
	class Program
	{
		static void Main(string[] args)
		{
			var server = new UDPFileTransfer.Server("127.0.0.1", 8991, 8990);
			server.RecieveFile();
			Console.Read();
		}
	}
}
