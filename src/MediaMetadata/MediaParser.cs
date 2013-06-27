using System;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Contribs.aynurin;

namespace MediaMetadata
{
    public abstract class MediaParser
    {
        public static Metadata Process(Stream dataStream)
        {
            var stream = dataStream.CanSeek ? dataStream : new SeekStream(dataStream);
            try
            {
                var parser = MediaParserFactory.CreateParser(stream);
                if (parser == null)
                    throw new NotSupportedException("This data type is not supported: " +
                                                    Encoding.ASCII.GetString(dataStream.ReadBytes(8)));
                return parser.ExtractMetadata(stream);
            }
            finally
            {
                if (stream != dataStream)
                    stream.Dispose();
            }
        }

        public abstract bool CanParse(Stream stream);

        public abstract Metadata ExtractMetadata(Stream stream);

        protected PixelFormat GetPixelFormat(short bpp, bool hasAlphaChannel = false)
        {
            switch (bpp)
            {
                case 1: return PixelFormat.Format1bppIndexed;
                case 4: return PixelFormat.Format4bppIndexed;
                case 8: return PixelFormat.Format8bppIndexed;
                case 16: return hasAlphaChannel ? PixelFormat.Format16bppArgb1555 : PixelFormat.Format16bppRgb555;
                case 24: return PixelFormat.Format24bppRgb;
                case 32: return hasAlphaChannel ? PixelFormat.Format32bppArgb : PixelFormat.Format32bppRgb;
                default: throw new NotImplementedException("This bpp value (" + bpp + ") is not supported");
            }
        }
    }
}
