using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MediaMetadata.PNG
{
    class PNGParser : MediaParser
    {
        private static byte[] _png_header = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

        public override bool CanParse(Stream stream)
        {
            return stream.StartsWith(_png_header);
        }

        public override Metadata ExtractMetadata(Stream stream)
        {
            using (var r = new NGChunksReader(stream))
            {
                var ch = r.NextChunk();
                if (ch.ChunkType == "IHDR")
                {
                    var header = r.ReadChunk(ch).Data.ToStructure<FileHeader>();

                    var meta = new ImageMetadata { ImageFormat = ImageFormat.Png, MimeType = "image/png" };
                    meta.Size = new Size(header.ImageWidth, header.ImageHeight);
                    meta.BitsPerPixel = header.BitDepth;
                    meta.PixelFormat = GetPixelFormat(header.BitDepth);
                    meta.Frames.Add(new Frame { Size = meta.Size });
                    return meta;
                }
            }
            return null;
        }
    }
}
