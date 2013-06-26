using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var m1 = MediaParser.ExtractMetadata(asm.GetManifestResourceStream("MediaMetadata.Tests.BMP.Data.a2c38d9af9fcdac5377bda307da5abb2.bmp"));
            Assert.AreEqual(m1.Size.Width, 685);
            Assert.AreEqual(m1.Size.Height, 610);
        }
    }
}
