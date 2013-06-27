using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Reflection;

namespace MediaMetadata.Tests
{
    [TestClass]
    public class PNGParserTest
    {
        [TestMethod]
        public void ProcessTestFiles()
        {
            var asm = Assembly.GetExecutingAssembly();
            var files = asm.GetManifestResourceNames();
            foreach (var file in files)
            {
                if (file.EndsWith(".png"))
                {
                    using (var s = asm.GetManifestResourceStream(file))
                    using (var s2 = asm.GetManifestResourceStream(file))
                    {
                        var m1 = MediaParser.Process(s) as ImageMetadata;
                        Assert.IsNotNull(m1);
                        var img = Image.FromStream(s2);
                        Assert.AreEqual(m1.Size.Width, img.Size.Width);
                        Assert.AreEqual(m1.Size.Height, img.Size.Height);
                        Assert.AreEqual(m1.PixelFormat, img.PixelFormat);
                        //Assert.AreEqual(m1.ImageFormat, img.RawFormat);
                    }
                }
            }
        }
    }
}
