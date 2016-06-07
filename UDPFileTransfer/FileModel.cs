using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPFileTransfer
{
	public class FileModel
	{
		/// <summary>
		/// Путь по которому нужно сохранить файл
		/// </summary>
		public string FilePath;

		/// <summary>
		/// Название файла и его расширение
		/// </summary>
		public string FileName;

		/// <summary>
		/// Количество пакетов из которых состоит файл
		/// </summary>
		public int IndexCount;

		/// <summary>
		/// Размер передаваемого чанка
		/// </summary>
		public int ChunkSize;

		/// <summary>
		/// Словарик чанков файла для хранения содержимого и индекса
		/// </summary>
		private Dictionary<int, byte[]> _chunks;

		/// <summary>
		/// Создать объект для приёма файла
		/// </summary>
		/// <param name="path">Путь</param>
		/// <param name="name">Название и расширение</param>
		/// <param name="indexCount">Количество чанков</param>
		public FileModel(string path, string name, int indexCount, int chunkSize)
		{
			FilePath = path;
			FileName = name;
			IndexCount = indexCount;
			ChunkSize = chunkSize;

			_chunks = new Dictionary<int, byte[]>();
		}

		/// <summary>
		/// Принять пакет и сохранить в чанки
		/// </summary>
		/// <param name="packet"></param>
		public void AcceptPacket(FilePacket packet)
		{
			_chunks.Add(packet.Index, packet.FileBytes);
		}
		
		/// <summary>
		/// Получить список потерянных пакетов
		/// </summary>
		/// <returns></returns>
		public List<int> LostChanks()
		{
			List<int> lostChankIndexes = new List<int>();
			for (var i = 0; i < IndexCount; i++)
				lostChankIndexes.Add(i);
			
			return lostChankIndexes.Except(_chunks.Keys).ToList();
		}

		/// <summary>
		/// Сохранить файл
		/// </summary>
		/// <returns></returns>
		public bool SaveFile()
		{
			byte[] allFile = new byte[0];
			for(int i = 0; i < IndexCount; i++)
				Buffer.BlockCopy(_chunks[i], 0, allFile, i * ChunkSize, ChunkSize);

			File.WriteAllBytes(FilePath + "\\" + FileName, allFile);

			return true;
		}
	}
}
