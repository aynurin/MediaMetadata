using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace MediaMetadata
{
    public abstract class Metadata
    {
        public string MimeType { get; set; }
        public ContentCategory ContentCategory { get; set; }
        public Size Size { get; set; }
        public List<Frame> Frames { get; private set; }

        protected Metadata()
        {
            this.Frames = new List<Frame>();
        }
    }

    public sealed class ImageMetadata : Metadata
    {
        public PixelFormat PixelFormat { get; set; }
        public ImageFormat ImageFormat { get; set; }
        public short BitsPerPixel { get; set; }

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
