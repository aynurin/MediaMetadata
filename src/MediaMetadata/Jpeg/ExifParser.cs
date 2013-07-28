using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MediaMetadata.Jpeg
{
    class ExifParser : MediaParser
    {
        //http://www.media.mit.edu/pia/Research/deepview/exif.html
        private static readonly byte[] ExifHeader = new [] { ExifMarkersReader.ChunkStart, (byte)ExifMarkersReader.MarkerType.StartOfImage };

        public override bool CanParse(Stream stream)
        {
            return stream.StartsWith(ExifHeader);
        }

        public override Metadata ExtractMetadata(Stream stream)
        {
            var meta = new ImageMetadata { ImageFormat = ImageFormat.Jpeg, MimeType = "image/jpeg" };
            using (var reader = new ExifMarkersReader(stream))
            {
                ExifMarkersReader.ExifMarker marker = null;
                while ((marker = reader.NextChunk(marker)) != null)
                {
                    Trace.WriteLine(marker.MarkerType + ":" + marker.DataLength);
                    if (marker.MarkerType == ExifMarkersReader.MarkerType.StartOfFrameBaseline || 
                        marker.MarkerType == ExifMarkersReader.MarkerType.StartOfFrameProgressive)
                    {
                        var frame = StartOfFrameBaseline.Read(stream);
                        meta.Size = new Size(frame.Width, frame.Height);
                        if (frame.Components.Length == 3)
                            meta.PixelFormat = PixelFormat.Format24bppRgb;
                        else if (frame.Components.Length == 1)
                            meta.PixelFormat = PixelFormat.Format8bppIndexed;
                    }
                }
            }
            return meta;
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 9)]
    public struct App0JfifHeader
    {
        public short Version;
        public Density DensityUnits;
        public short HorizontalDensity;
        public short VerticalDensity;
        public byte ThumbnailWidth;
        public byte ThumbnailHeight;

        public const string HeaderString = "JFIF\0";
        public static byte[] HeaderBytes = Encoding.ASCII.GetBytes(HeaderString);

        public enum Density : byte
        {
            AspectRationOnly = 0,
            PixelsPerInch = 1,
            PixelsPerCentimeter = 2
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct App0JfxxHeader
    {
        public ThumbnailFormat DensityUnits;

        public const string HeaderString = "JFXX\0";
        public static byte[] HeaderBytes = Encoding.ASCII.GetBytes(HeaderString);

        public enum ThumbnailFormat : byte
        {
            Jpeg = 0x10,
            Palettised1Bpp = 0x11,
            Rgb3Bpp = 0x13
        }
    }

    //http://www.videotechnology.com/jpeg/j1.html

    public struct StartOfFrameBaseline
    {
        public byte PrecisionBits;
        public short Height;
        public short Width;
        public ComponentInfo[] Components;

        public struct ComponentInfo
        {
            public byte ComponentId;
            public byte H;
            public byte V;
            public byte QuantizationTableNum;
        }

        public static StartOfFrameBaseline Read(Stream data)
        {
            var frame = new StartOfFrameBaseline();
            frame.PrecisionBits = (byte)data.ReadByte();
            frame.Height = data.ReadLittleEndianInt16();
            frame.Width = data.ReadLittleEndianInt16();
            int componentsCount = data.ReadByte();
            frame.Components = new ComponentInfo[componentsCount];
            for (; componentsCount > 0; componentsCount--)
            {
                var c = new ComponentInfo();
                c.H = (byte)data.ReadByte();
                c.V = (byte)(c.H & 11);
                c.H = (byte)(c.H >> 4);
                c.QuantizationTableNum = (byte)data.ReadByte();
                frame.Components[frame.Components.Length - componentsCount] = c;
            }
            return frame;
        }
    }
}
