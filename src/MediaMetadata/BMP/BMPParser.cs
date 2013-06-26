using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace MediaMetadata.BMP
{
    class BMPParser : MediaParser
    {
        public override bool CanParse(FileReader reader)
        {
            return reader.StartsWith(0x42, 0x4D); // "BM"
        }

        public override Metadata ExtractMetadata(FileReader file)
        {

            var reader = file.GetBinaryReader();
            var fileHeader = reader.ReadBytes(14).ToStructure<FileHeader>();
            if (fileHeader.Signature != BitmapSignarute.BM)
                throw new NotImplementedException("This bitmap format is not supported: " + fileHeader.Signature);

            var meta = new ImageMetadata();
            meta.ImageFormat = ImageFormat.Bmp;
            meta.MimeType = "image/bmp";

            // parsing DIB
            var dibHeaderType = (DibHeaderType)reader.ReadInt32();
            if (dibHeaderType == DibHeaderType.BITMAPCOREHEADER || dibHeaderType == DibHeaderType.BITMAPCOREHEADER2)
            {
                var header = reader.ReadBytes(12 - 4).ToStructure<BitmapCoreHeader>();
                meta.Size = new Size(header.ImageWidth, header.ImageHeight);
                meta.PixelFormat = GetPixelFormat(header);
            }
            else if (Enum.IsDefined(typeof(DibHeaderType), dibHeaderType))
            {
                var header = reader.ReadBytes(40 - 4).ToStructure<BitmapInfoHeader>();
                meta.Size = new Size(header.ImageWidth, header.ImageHeight);
                meta.PixelFormat = GetPixelFormat(header);
            }
            else
                throw new NotImplementedException("This bitmap's DIB header is not supported: " + dibHeaderType);

            meta.Frames.Add(new Frame { Size = meta.Size });

            return meta;
        }

        private PixelFormat GetPixelFormat(BitmapInfoHeader header)
        {
            return GetPixelFormat(header.BitsPerPixel);
        }

        private PixelFormat GetPixelFormat(BitmapCoreHeader header)
        {
            return GetPixelFormat(header.BitsPerPixel);
        }

        private PixelFormat GetPixelFormat(short bpp)
        {
            switch (bpp)
            {
                case 1: return PixelFormat.Format1bppIndexed;
                case 4: return PixelFormat.Format4bppIndexed;
                case 8: return PixelFormat.Format8bppIndexed;
                case 16: return PixelFormat.Format16bppArgb1555;
                case 24: return PixelFormat.Format24bppRgb;
                case 32: return PixelFormat.Format32bppArgb;
                default: throw new NotImplementedException("This bpp value (" + bpp + ") is not supported");
            }
        }

    }
}
