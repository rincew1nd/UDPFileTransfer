using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDPFileTransfer.Entities;

namespace UDPFileTransfer
{
    class FileChunkBuilder
    {
        private readonly int _chunkSize;
        private readonly string _path;

        public FileChunkBuilder(string path, int chunkSize)
        {
            _chunkSize = chunkSize;
            _path = path;
        }

        public List<FileChunk> Build()
        {
            var chunks = new List<FileChunk>();
            var fileReader = File.ReadAllBytes(_path);
            var chunksCount = fileReader.Length/_chunkSize + 1;
            
            for (var i = 0; i < chunksCount; i++)
            {
                var buffer = new byte[_chunkSize];
                var currentChunkSize = _chunkSize;
                if ((i+1)*_chunkSize > fileReader.Length)
                    currentChunkSize = fileReader.Length - i*_chunkSize;
                Buffer.BlockCopy(fileReader, i * _chunkSize, buffer, 0, currentChunkSize);
                var chunk = new FileChunk(i, buffer);
                chunks.Add(chunk);
            }

            return chunks;
        }
    }
}
