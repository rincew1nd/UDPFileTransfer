using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UDPFileTransfer.Entities
{
    public class FileChunk
    {
        public byte[] Buffer;
        public int Index;
        public bool IsSended;

        /// <summary>
        /// Создать пакет к отправке
        /// </summary>
        /// <param name="index">Индекс чанка файла</param>
        /// <param name="buffer">Содержимое чанка файла</param>
        public FileChunk(int index, byte[] buffer)
        {
            Index = index;
            Buffer = buffer;
            IsSended = false;
        }

        /// <summary>
        /// Был ли отправлен чанк файла
        /// </summary>
        /// <returns></returns>
        public FileChunk Sended()
        {
            IsSended = true;
            return this;
        }

        /// <summary>
        /// Буфер в пакет
        /// </summary>
        /// <param name="fileChunk">Чанк пакета</param>
        public static Packet CreatePacket(FileChunk fileChunk)
        {
            var packet = new Packet();
            packet.PushInt(fileChunk.Index);
            packet.PushBytes(fileChunk.Buffer);
            return packet;
        }

        /// <summary>
        /// Создать экземпляр чанка файла из полученного пакета
        /// </summary>
        /// <param name="packet">Пакет</param>
        /// <returns></returns>
        public static FileChunk ReadPacket(Packet packet)
        {
            return new FileChunk(packet.PullInt(), packet.PullBytes()).Sended();
        }
    }
}
