using System.IO;
using System;
using System.Text;
using MediaMetadata.Contribs.damieng;

namespace MediaMetadata.PNG
{
    class NGChunksReader : FileReader
    {
        public NGChunksReader(Stream dataStream) : base(dataStream)
        {
        }

        public NGChunk NextChunk(NGChunk previous = null)
        {
            var r = GetBinaryReader();
            if (previous != null)
                r.BaseStream.Position = previous.DataOffset + previous.DataLength + 4;
            var pos = r.BaseStream.Position;
            if (pos + 12 < r.BaseStream.Length)
            {
                var ch = new NGChunk
                {
                    DataLength = r.ReadLittleEndianInt32(),
                    ChunkType = Encoding.ASCII.GetString(r.ReadBytes(4)),
                    DataOffset = r.BaseStream.Position
                };
                if (ch.DataOffset + ch.DataLength + 4 <= r.BaseStream.Length)
                {
                    r.BaseStream.Seek(ch.DataLength, SeekOrigin.Current);
                    ch.CRC = r.ReadInt32();
                    return ch;
                }
                throw new UnexpectedEndOfStreamException("Stream length: " + r.BaseStream.Position + ", expected length, at least: " + ch.DataLength + ch.DataLength + 4);
            }
            return null;
        }

        public NGChunk ReadChunk(NGChunk chunk)
        {
            var r = GetBinaryReader();
            r.BaseStream.Position = chunk.DataOffset;
            chunk.Data = r.ReadBytes(chunk.DataLength);
            chunk.Validate();
            return chunk;
        }

        public class NGChunk
        {
            public int DataLength;
            public string ChunkType;
            public int CRC;
            public long DataOffset;
            public byte[] Data;

            internal bool Validate()
            {
                var crc32 = new Crc32();
                var checkData = new byte[4 + this.DataLength];
                Array.Copy(Encoding.ASCII.GetBytes(this.ChunkType), checkData, 4);
                Array.Copy(this.Data, 0, checkData, 4, this.Data.Length);
                var bytes = crc32.ComputeHash(checkData);
                if (!BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);
                var crc = BitConverter.ToInt32(bytes, 0);
                return CRC == crc;
            }
        }
    }
}
