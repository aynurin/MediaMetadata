using System;
using System.Diagnostics;
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
            stream.Position = _png_header.Length;
            FileHeader header = new FileHeader();
            var meta = new ImageMetadata { ImageFormat = ImageFormat.Png, MimeType = "image/png" };
            using (var r = new NGChunksReader(stream))
            {
                NGChunksReader.NGChunk ch = null;
                while ((ch = r.NextChunk(ch)) != null && ch.ChunkType != "IDAT")
                {
                    if (ch.ChunkType == "IHDR")
                    {
                        header = r.ReadChunk(ch).Data.ToStructure<FileHeader>();
                        header.FixByteOrder();
                        header.DebugWrite();
                        meta.IsIndexed = header.ColorType.HasFlag(ColorType.Palette);
                        meta.HasTransparency = header.ColorType.HasFlag(ColorType.Alpha);
                        meta.Size = new Size(header.ImageWidth, header.ImageHeight);
                    }
                    else if (ch.ChunkType == "PLTE")
                    {
                        meta.PaletteColorsCount = ch.DataLength / 3;
                    }
                    else if (ch.ChunkType == "tRNS")
                    {
                        meta.HasTransparency = true;
                    }
                }
            }

            DetectPixelDepth(meta, header);
            meta.Frames.Add(new Frame { Size = meta.Size });
            return meta;
        }

        private void DetectPixelDepth(ImageMetadata meta, FileHeader header)
        {
            int samplesPerPixel = 0;
            int bitsPerSample = header.BitDepth;
            bool alpha = header.ColorType.HasFlag(ColorType.Alpha);
            bool index = header.ColorType.HasFlag(ColorType.Palette);
            bool color = header.ColorType.HasFlag(ColorType.Color);
            if (alpha)
                samplesPerPixel += 1;
            if (color)
                samplesPerPixel += 3;
            else // grayscale
                samplesPerPixel += 1;
            if (index)
                bitsPerSample = 8;

            meta.BitsPerPixel = (short)(bitsPerSample * samplesPerPixel);
            // PixelFormat requires more attention, probably avoiding System.Drawing.PixelFormat
            meta.PixelFormat = PixelFormat.Format32bppArgb;

            switch (header.ColorType)
            {
                case ColorType.Grayscale:
                    switch (header.BitDepth)
                    {
                        //case 1: meta.PixelFormat = PixelFormat.Format16bppGrayScale; break;
                        //case 2: meta.PixelFormat = PixelFormat.Format16bppGrayScale; break;
                        //case 4: meta.PixelFormat = PixelFormat.Format16bppGrayScale; break;
                        //case 8: meta.PixelFormat = PixelFormat.Format16bppGrayScale; break;
                        case 16: meta.PixelFormat = PixelFormat.Format16bppGrayScale; break;
                    }
                    break;
                //case ColorType.Alpha:
                //    switch (header.BitDepth)
                //    {
                //        case 8: meta.PixelFormat = PixelFormat.Format32bppArgb; break;
                //        case 16: meta.PixelFormat = PixelFormat.Format32bppArgb; break;
                //    }
                //    break;
                case ColorType.Color:
                    switch (header.BitDepth)
                    {
                        case 8: meta.PixelFormat = PixelFormat.Format24bppRgb; break;
                        case 16: meta.PixelFormat = PixelFormat.Format48bppRgb; break;
                    }
                    break;
                case ColorType.Color | ColorType.Alpha:
                    switch (header.BitDepth)
                    {
                        case 8: meta.PixelFormat = PixelFormat.Format32bppArgb; break;
                        case 16: meta.PixelFormat = PixelFormat.Format64bppArgb; break;
                    }
                    break;
                case ColorType.Color | ColorType.Palette:
                    if (meta.HasTransparency)
                        meta.PixelFormat = PixelFormat.Format32bppArgb;
                    else
                    {
                        switch (header.BitDepth)
                        {
                            case 1:
                                meta.PixelFormat = PixelFormat.Format8bppIndexed;
                                break;
                            case 2:
                                meta.PixelFormat = PixelFormat.Format8bppIndexed;
                                break;
                            case 4:
                                meta.PixelFormat = PixelFormat.Format8bppIndexed;
                                break;
                            case 8:
                                meta.PixelFormat = PixelFormat.Format8bppIndexed;
                                break;
                        }
                    }
                    break;
            }
        }
    }
}
