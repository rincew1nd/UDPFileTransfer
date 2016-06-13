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

		private List<int> _sendedChunks;

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

			_sendedChunks = new List<int>();
			for (int i = 0; i < IndexCount; i++)
				_sendedChunks.Add(i);
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
		/// Получить лист номеров чанков которые нужно отправить
		/// </summary>
		/// <returns></returns>
		public List<int> ChunksToSend()
		{
			List<int> chunksToSend = new List<int>();
			for (var i = 0; i < (_sendedChunks.Count >= 10 ? 10 : _sendedChunks.Count); i++)
			{
				chunksToSend.Add(i);
				_sendedChunks.Remove(i);
			}
			
			return chunksToSend;
		}

		/// <summary>
		/// Добавить потерянные чанки в лист чанков на отправку
		/// </summary>
		/// <param name="lostedChunks"></param>
		public void LostedChunks(List<int> lostedChunks)
		{
			_sendedChunks.AddRange(lostedChunks.Except(_sendedChunks));
		}

		public byte[] GetChunk(int chunkNumber)
		{
			return _chunks[chunkNumber];
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
