using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UDPFileTransfer
{
    public class UDPFileSender
	{
		/// <summary>
		/// Запуск клиента
		/// </summary>
		/// <param name="host">адрес хоста</param>
		/// <param name="port">порт хоста</param>
		/// <param name="bufferSize">размер буфера</param>
		public static Client Start(string host, int port, int bufferSize)
		{
			return new Client().Start(host, port, bufferSize);
		}

		/// <summary>
		/// Запуск сервера
		/// </summary>
		/// <param name="port">порт на котором будет запущен сервер</param>
		public static Server Start(int port)
		{
			return new Server().Start(port);
		}
    }
}
