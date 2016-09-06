using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UDPFileTransfer.Entities;

namespace UDPFileTransfer
{
    public class FileModel
    {
        /// <summary>
        /// Количество пакетов из которых состоит файл
        /// </summary>
        public int ChunkCount;

        /// <summary>
        /// Размер передаваемого чанка
        /// </summary>
        public int ChunkSize { get; }

        /// <summary>
        /// Имя файла
        /// </summary>
        public string Name => Path.GetFileNameWithoutExtension((FullPath));

        /// <summary>
        /// Получить расширение файла
        /// </summary>
        public string Extension => Path.GetExtension((FullPath));

        /// <summary>
        /// Полный адрес до файла
        /// </summary>
        public string FullPath;

        /// <summary>
        /// Словарик чанков файла для хранения содержимого и индекса
        /// </summary>
        private List<FileChunk> _chunks;

        /// <summary>
        /// Создать объект для приёма файла
        /// </summary>
        /// <param name="path">Путь</param>
        /// <param name="chunkSize">Количество чанков</param>
        public FileModel(string path, int chunkSize) : base()
        {
            FullPath = path;
            ChunkSize = chunkSize;
            _chunks = new List<FileChunk>();
        }

        /// <summary>
        /// Сгенерировать чанки для отправки
        /// </summary>
        public void Generate()
        {
            _chunks = new FileChunkBuilder(FullPath, ChunkSize).Build();
            ChunkCount = _chunks.Count;
        }

        /// <summary>
        /// Принять пакет и сохранить в чанки
        /// </summary>
        /// <param name="packet"></param>
        public void AcceptPacket(Packet packet)
        {
            var info = FileChunk.ReadPacket(packet);
            Console.WriteLine($"Chunk received {info.Index} of {ChunkCount}. Total {_chunks.Count}");
            _chunks.Add(info);
        }

        /// <summary>
        /// Получить лист номеров чанков которые нужно отправить
        /// </summary>
        /// <returns></returns>
        public List<int> ChunksToSend()
        {
            return _chunks.Where(p => !p.IsSended).Select(p => p.Index).ToList();
        }

        /// <summary>
        /// Добавить потерянные чанки в лист чанков на отправку
        /// </summary>
        /// <param name="losedChunks">Индекс потерянного чанка</param>
        public void LostedChunks(List<int> losedChunks)
        {
            foreach (var losedChunk in losedChunks)
                _chunks.FirstOrDefault(p => p.Index == losedChunk)?.Sended();
        }

        /// <summary>
        /// Получить чанк по его индексу
        /// </summary>
        /// <param name="index">Индекс чанка</param>
        /// <returns></returns>
        public FileChunk GetChunk(int index)
        {
            return _chunks.FirstOrDefault(p => p.Index == index);
        }

        /// <summary>
        /// Сохранить файл
        /// </summary>
        /// <returns></returns>
        public bool SaveFile(string path1)
        {
            var allFile = new byte[0];
            try
            {
                for (var i = 0; i < ChunkCount; i++)
                    Buffer.BlockCopy(
                        _chunks.FirstOrDefault(p => p.Index == i).Buffer,
                        0, allFile, i*ChunkSize, ChunkSize
                    );
                File.WriteAllBytes(path1, allFile);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
