using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace MediaMetadata
{
    public abstract class Metadata
    {
        public string MimeType;
        public ContentCategory ContentCategory;
        public Size Size;
        public List<Frame> Frames { get; private set; }

        protected Metadata()
        {
            this.Frames = new List<Frame>();
        }
    }

    public sealed class ImageMetadata : Metadata
    {
        public PixelFormat PixelFormat;
        public ImageFormat ImageFormat;
        public short BitsPerPixel;
        public bool HasTransparency;
        public bool IsIndexed;
        public int PaletteColorsCount;

        public ImageMetadata()
        {
            this.ContentCategory = ContentCategory.Image;
        }

    }

    public enum ContentCategory
    {
        Image
    }

    public class Frame
    {
        public Size Size { get; set; }
    }
}
