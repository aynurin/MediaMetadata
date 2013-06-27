using Contribs.aynurin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace MediaMetadata.Tests.SeekStream
{
    /// <summary>
    /// a small test for <see cref="SeekStream" />
    /// </summary>
    [TestClass]
    public class SeekStreamTests
    {
        [TestMethod]
        public void SeekTest()
        {
            var stream1 = new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
            var stream2 = new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });

            var seekStream = new global::Contribs.aynurin.SeekStream(stream1);

            Assert.AreEqual(seekStream.ReadByte(), stream2.ReadByte());

            seekStream.Seek(3, SeekOrigin.Begin);
            stream2.Seek(3, SeekOrigin.Begin);
            Assert.AreEqual(seekStream.ReadByte(), stream2.ReadByte());

            seekStream.Seek(3, SeekOrigin.Current);
            stream2.Seek(3, SeekOrigin.Current);
            Assert.AreEqual(seekStream.ReadByte(), stream2.ReadByte());

            seekStream.Seek(-2, SeekOrigin.Current);
            stream2.Seek(-2, SeekOrigin.Current);
            Assert.AreEqual(seekStream.ReadByte(), stream2.ReadByte());

            seekStream.Seek(-2, SeekOrigin.End);
            stream2.Seek(-2, SeekOrigin.End);
            Assert.AreEqual(seekStream.ReadByte(), stream2.ReadByte());
        }

        [TestMethod]
        public void ReadIntTest()
        {
            var stream1 = new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
            var stream2 = new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });

            var seekStream = new global::Contribs.aynurin.SeekStream(stream1);

            var r1 = new BinaryReader(seekStream);
            var r2 = new BinaryReader(stream2);

            Assert.AreEqual(r1.ReadInt16(), r2.ReadInt16());
            Assert.AreEqual(r1.ReadInt32(), r2.ReadInt32());
            Assert.AreEqual(r1.ReadInt64(), r2.ReadInt64());
        }
    }
}
