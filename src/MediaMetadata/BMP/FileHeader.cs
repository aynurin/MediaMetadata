using System.Runtime.InteropServices;

namespace MediaMetadata.BMP
{
    /// <summary>
    /// This block of bytes is at the start of the file and is used to identify the file. 
    /// A typical application reads this block first to ensure that the file is actually a BMP file and that it is not damaged.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 14)]
    public struct FileHeader
    {
        public BitmapSignarute Signature;
        public int DataLength;
        public short Reserverd1;
        public short Reserverd2;
        public int PixelDataOffset;
    }

    /// <summary>
    /// DIB header (bitmap information header) <see cref="DibHeaderType.BITMAPCOREHEADER"/>
    /// </summary>
    /// <remarks>OS/2 and also all Windows versions since Windows 3.0</remarks>
    [StructLayout(LayoutKind.Sequential, Size = 12)]
    public struct BitmapCoreHeader
    {
        /// <summary>
        /// the bitmap width in pixels (signed integer).
        /// </summary>
        public short ImageWidth;
        /// <summary>
        /// the bitmap height in pixels (signed integer).
        /// </summary>
        public short ImageHeight;
        /// <summary>
        /// the number of color planes being used. Must be set to 1.
        /// </summary>
        public short ColorPlanes;
        /// <summary>
        /// the number of bits per pixel, which is the color depth of the image. Typical values are 1, 4, 8, 16, 24 and 32.
        /// </summary>
        public short BitsPerPixel;
    }

    /// <summary>
    /// DIB header (bitmap information header)  <see cref="DibHeaderType.BITMAPINFOHEADER"/>
    /// </summary>
    /// <remarks>all Windows versions since Windows 3.0</remarks>
    [StructLayout(LayoutKind.Sequential, Size = 40)]
    public struct BitmapInfoHeader
    {
        /// <summary>
        /// the bitmap height in pixels (signed integer).
        /// </summary>
        public int ImageWidth;
        /// <summary>
        /// the bitmap width in pixels (signed integer).
        /// </summary>
        public int ImageHeight;
        /// <summary>
        /// the number of color planes being used. Must be set to 1.
        /// </summary>
        public short ColorPlanes;
        /// <summary>
        /// the number of bits per pixel, which is the color depth of the image. Typical values are 1, 4, 8, 16, 24 and 32.
        /// </summary>
        public short BitsPerPixel;
        /// <summary>
        /// the compression method being used. See the next table for a list of possible values.
        /// </summary>
        public CompressionMethod CompressionMethod;
        /// <summary>
        /// the image size. This is the size of the raw bitmap data (see below), and should not be confused with the file size.
        /// </summary>
        public int RawImageSize;
        /// <summary>
        /// the horizontal resolution of the image. (pixel per meter, signed integer)
        /// </summary>
        public int HorizontalResolution;
        /// <summary>
        /// the vertical  resolution of the image. (pixel per meter, signed integer)
        /// </summary>
        public int VerticalResolution;
        /// <summary>
        /// the number of colors in the color palette, or 0 to default to 2^n.
        /// </summary>
        public int ColorsInPalette;
        /// <summary>
        /// the number of important colors used, or 0 when every color is important; generally ignored.
        /// </summary>
        public int ImportantColors;
    }

    // ReSharper disable InconsistentNaming
    public enum DibHeaderType : int
    {
        BITMAPCOREHEADER = 12,
        BITMAPCOREHEADER2 = 64,
        BITMAPINFOHEADER = 40,
        BITMAPV2INFOHEADER = 52,
        BITMAPV3INFOHEADER = 56,
        BITMAPV4HEADER = 108,
        BITMAPV5HEADER = 124
    }

    /// <summary>
    /// The compression method field (bytes #30-33)
    /// </summary>
    public enum CompressionMethod : int
    {
        /// <summary>
        /// none
        /// </summary>
        RGB = 0,
        /// <summary>
        /// RLE 8-bit/pixel. Can be used only with 8bpp bitmaps.
        /// </summary>
        RLE8,
        /// <summary>
        /// RLE 4-bit/pixel. Can be used only with 4bpp bitmaps.
        /// </summary>
        RLE4,
        /// <summary>
        /// Pixel format defined by bit masks or Huffman 1D compressed bitmap for <see cref="DibHeaderType.BITMAPCOREHEADER2"/>
        /// </summary>
        BITFIELDS,
        /// <summary>
        /// The bitmap contains a JPEG image or RLE-24 compressed bitmap for <see cref="DibHeaderType.BITMAPCOREHEADER2"/>
        /// </summary>
        JPEG,
        /// <summary>
        /// PNG
        /// </summary>
        PNG,
        /// <summary>
        /// Bit field
        /// </summary>
        ALPHABITFIELDS
    }

    /// <summary>
    /// Bitmap file header field
    /// </summary>
    public enum BitmapSignarute : short
    {
        /// <summary>
        /// Windows Bitmap
        /// </summary>
        BM = 19778, // "BM" as Int16
        /// <summary>
        /// OS/2 struct Bitmap Array
        /// </summary>
        BA = 16706,
        /// <summary>
        ///  OS/2 struct Color Icon
        /// </summary>
        CI = 18755,
        /// <summary>
        /// OS/2 const Color Pointer
        /// </summary>
        CP = 20547,
        /// <summary>
        /// OS/2 struct Icon
        /// </summary>
        IC = 17225,
        /// <summary>
        /// OS/2 Pointer
        /// </summary>
        PT = 21584
    }
    // ReSharper restore InconsistentNaming
}
