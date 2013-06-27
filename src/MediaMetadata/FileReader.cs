using System;
using System.IO;

namespace MediaMetadata
{
    public class FileReader : IDisposable
    {
        private readonly BinaryReader _reader;

        public FileReader(Stream dataStream)
        {
            if (dataStream != null)
                _reader = new BinaryReader(dataStream);
        }

        internal bool StartsWith(params byte[] header)
        {
            var h = _reader.ReadBytes(header.Length);
            _reader.BaseStream.Seek(-h.Length, SeekOrigin.Current);
            return h.StartsWith(header);
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public BinaryReader GetBinaryReader()
        {
            return _reader;
        }

        public virtual bool CanRead(string fileName, string mimeType, Stream data)
        {
            return true;
        }
    }
}
