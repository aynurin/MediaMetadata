using Contribs.aynurin;
using System;
using System.IO;

namespace MediaMetadata.Jpeg
{
    class ExifMarkersReader : IDisposable //FileReader
    {
        private readonly Stream _data;

        public ExifMarkersReader(Stream dataStream)
            //: base(dataStream)
        {
            _data = dataStream.CanSeek ? dataStream : new SeekStream(dataStream);
        }

        public ExifMarker NextChunk(ExifMarker previous = null)
        {
            if (previous != null)
                _data.Position = previous.DataOffset + previous.DataLength;
            if (_data.ReadByte() == ChunkStart)
            {
                var marker = new ExifMarker
                {
                    MarkerType = (MarkerType) _data.ReadByte(),
                    DataOffset = _data.Position
                };
                if (marker.RequiresData())
                {
                    marker.DataLength = _data.ReadLittleEndianInt16() - 2;
                    marker.DataOffset += 2;
                }
                return marker;
            }
            return null;
        }

        public ExifMarker ReadChunk(ExifMarker chunk)
        {
            _data.Position = chunk.DataOffset;
            chunk.Data = _data.ReadBytes(chunk.DataLength);
            return chunk;
        }

        public class ExifMarker
        {
            public int DataLength;
            public MarkerType MarkerType;
            public long DataOffset;
            public byte[] Data;

            internal bool RequiresData()
            {
                return MarkerType != ExifMarkersReader.MarkerType.EndOfImage &&
                       MarkerType != ExifMarkersReader.MarkerType.StartOfImage;
            }
        }

        //https://en.wikipedia.org/wiki/JPEG#Syntax_and_structure
        public enum MarkerType : byte
        {
            StartOfImage = 0xD8,
            EndOfImage = 0xD9,
            StartOfFrameBaseline = 0xC0,
            StartOfFrameProgressive = 0xC2,
            DefineHuffmanTable = 0xC4,
            DefineQuantizationTable = 0xDB,
            DefineRestartInterval = 0xDD,
            StartOfScan = 0xDA,
            Comment = 0xFE,

            Restart0 = 0xD0,
            Restart1 = 0xD1,
            Restart2 = 0xD2,
            Restart3 = 0xD3,
            Restart4 = 0xD4,
            Restart5 = 0xD5,
            Restart6 = 0xD6,
            Restart7 = 0xD7,

            App0 = 0xE0,
            App1 = 0xE1,
            App2 = 0xE2,
            App3 = 0xE3,
            App4 = 0xE4,
            App5 = 0xE5,
            App6 = 0xE6,
            App7 = 0xE7,
            App8 = 0xE8,
            App9 = 0xE9,
            AppA = 0xEA,
            AppB = 0xEB,
            AppC = 0xEC,
            AppD = 0xED,
            AppE = 0xEE,
            AppF = 0xEF
        }

        public void Dispose()
        {
            _data.Dispose();
        }

        public const byte ChunkStart = 0xFF;
    }
}
