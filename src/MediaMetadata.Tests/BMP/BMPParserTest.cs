using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Reflection;

namespace MediaMetadata.Tests
{
    [TestClass]
    public class BMPParserTest
    {
        [TestMethod]
        public void ProcessTestFiles()
        {
            var asm = Assembly.GetExecutingAssembly();
            var files = asm.GetManifestResourceNames();
            foreach (var file in files)
            {
                if (file.EndsWith(".bmp"))
                {
                    using (var s = asm.GetManifestResourceStream(file))
                    {
                        var m1 = MediaParser.Process(asm.GetManifestResourceStream(file)) as ImageMetadata;
                        Assert.IsNotNull(m1);
                        var img = Image.FromStream(s);
                        Assert.AreEqual(m1.Size.Width, img.Size.Width);
                        Assert.AreEqual(m1.Size.Height, img.Size.Height);
                        Assert.AreEqual(m1.PixelFormat, img.PixelFormat);
                        Assert.AreEqual(m1.ImageFormat, img.RawFormat);
                    }
                }
            }
        }
    }
}
