using System;
using System.Runtime.InteropServices;

namespace MediaMetadata.PNG
{
    /// <summary>
    /// This block of bytes is at the start of the file and is used to identify the file. 
    /// A typical application reads this block first to ensure that the file is actually a BMP file and that it is not damaged.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 13)]
    public struct FileHeader
    {
        public int ImageWidth;
        public int ImageHeight;
        public byte BitDepth;
        public ColorType ColorType;
        public byte FilterMethod;
        public InterlaceMethod InterlaceMethod;
    }

    [Flags]
    public enum ColorType : byte
    {
        Grayscale = 0,
        Palette = 1,
        Color = 2,
        Alpha = 4
    }

    public enum InterlaceMethod : byte
    {
        NoInterlace = 0,
        Adam7 = 1
    }
}
