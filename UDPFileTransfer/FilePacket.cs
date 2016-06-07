using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPFileTransfer
{
	public class FilePacket
	{
		/// <summary>
		/// Порядковый номер пакета
		/// </summary>
		public int Index;

		/// <summary>
		/// Содержимое пакета
		/// </summary>
		public byte[] FileBytes;

		/// <summary>
		/// Буфер в пакет
		/// </summary>
		/// <param name="buffer"></param>
		public FilePacket(byte[] buffer)
		{
			Index = BitConverter.ToInt32(buffer, 0);
			Buffer.BlockCopy(buffer, 0, FileBytes, 0, buffer.Length - 4);
		}
	}
}
