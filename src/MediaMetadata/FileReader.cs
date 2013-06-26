using System;
using System.IO;

namespace MediaMetadata
{
    public class FileReader : IDisposable
    {
        private BinaryReader _reader;

        public FileReader(Stream dataStream)
        {
            _reader = new BinaryReader(dataStream);
        }

        public byte[] ReadBytes(int count)
        {
            return _reader.ReadBytes(count);
        }

        internal bool StartsWith(params byte[] header)
        {
            var h = ReadBytes(header.Length);
            _reader.BaseStream.Seek(-h.Length, SeekOrigin.Current);
            return h.StartsWith(header);
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        internal BinaryReader GetBinaryReader()
        {
            return _reader;
        }
    }
}
