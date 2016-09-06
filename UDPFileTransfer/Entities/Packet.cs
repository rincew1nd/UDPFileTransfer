using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPFileTransfer
{
    public class Packet
    {
        private byte[] _buffer;
        private int pointer;

        public Packet()
        {
            _buffer = new byte[0];
            pointer = 0;
        }

        public Packet(byte[] buffer)
        {
            _buffer = buffer;
            pointer = 0;
        }

        public int PullInt()
        {
            pointer += 4;
            return BitConverter.ToInt32(_buffer, pointer - 4);
        }

        public string PullString()
        {
            try
            {
                var size = PullInt();
                var stringBuffer = new byte[size];
                Buffer.BlockCopy(_buffer, pointer, stringBuffer, 0, size);
                pointer += size;
                return Encoding.ASCII.GetString(stringBuffer);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
		}

		public byte[] PullBytes(int count = 0)
		{
			if (count == 0) count = _buffer.Length - pointer;
			var dummyBuffer = new byte[count];
			Buffer.BlockCopy(_buffer, pointer, dummyBuffer, 0, count);
			pointer += count;
			return dummyBuffer;
		}

		public bool PullBool()
		{
			pointer++;
			return BitConverter.ToBoolean(_buffer, --pointer);
		}

		public void PushInt(int i)
		{
            Array.Resize(ref _buffer, pointer + 4);
			Buffer.BlockCopy(BitConverter.GetBytes(i), 0, _buffer, pointer, 4);
		    pointer += 4;
		}

	    public void PushString(string str)
        {
            var strArray = Encoding.ASCII.GetBytes(str);
            PushInt(strArray.Length);

            Array.Resize(ref _buffer, pointer + strArray.Length);
            Buffer.BlockCopy(strArray, 0, _buffer, pointer, strArray.Length);
            pointer += strArray.Length;
        }

	    public void PushBytes(byte[] bytes)
        {
            Array.Resize(ref _buffer, pointer + bytes.Length);
            Buffer.BlockCopy(bytes, 0, _buffer, pointer, bytes.Length);
            pointer += bytes.Length;
        }

		public void PushBool(bool boolean)
		{
			Array.Resize(ref _buffer, pointer + 1);
			Buffer.BlockCopy(BitConverter.GetBytes(boolean), 0, _buffer, pointer, 1);
			pointer++;
		}

		public bool IsAnythingLeft(int size)
		{
			return (pointer + size) > _buffer.Length;
		}

	    public byte[] Build()
	    {
	        return _buffer;
	    }

	    public void ResetPointer()
	    {
	        pointer = 0;
	    }
	}
}
