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
			var server = UDPFileSender.Start(8990);
			server.RecieveFile();
		}
	}
}
