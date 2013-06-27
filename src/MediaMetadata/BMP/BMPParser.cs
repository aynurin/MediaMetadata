using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace MediaMetadata.BMP
{
    class BMPParser : MediaParser
    {
        public override bool CanParse(Stream reader)
        {
            return reader.StartsWith(0x42, 0x4D); // "BM"
        }

        public override Metadata ExtractMetadata(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                var fileHeader = reader.ReadBytes(14).ToStructure<FileHeader>();
                if (fileHeader.Signature != BitmapSignarute.BM)
                    throw new NotImplementedException("This bitmap format is not supported: " + fileHeader.Signature);

                var meta = new ImageMetadata {ImageFormat = ImageFormat.Bmp, MimeType = "image/bmp"};

                // parsing DIB
                var dibHeaderType = (DibHeaderType) reader.ReadInt32();
                if (dibHeaderType == DibHeaderType.BITMAPCOREHEADER || dibHeaderType == DibHeaderType.BITMAPCOREHEADER2)
                {
                    var header = reader.ReadBytes(12 - 4).ToStructure<BitmapCoreHeader>();
                    meta.Size = new Size(header.ImageWidth, header.ImageHeight);
                    meta.BitsPerPixel = header.BitsPerPixel;
                    meta.PixelFormat = GetPixelFormat(header);
                }
                else if (Enum.IsDefined(typeof (DibHeaderType), dibHeaderType))
                {
                    var header = reader.ReadBytes(40 - 4).ToStructure<BitmapInfoHeader>();
                    meta.Size = new Size(header.ImageWidth, header.ImageHeight);
                    meta.BitsPerPixel = header.BitsPerPixel;
                    meta.PixelFormat = GetPixelFormat(header);
                }
                else
                    throw new NotImplementedException("This bitmap's DIB header is not supported: " + dibHeaderType);

                meta.Frames.Add(new Frame {Size = meta.Size});

                return meta;
            }
        }

        private PixelFormat GetPixelFormat(BitmapInfoHeader header)
        {
            return GetPixelFormat(header.BitsPerPixel);
        }

        private PixelFormat GetPixelFormat(BitmapCoreHeader header)
        {
            return GetPixelFormat(header.BitsPerPixel);
        }

    }
}
