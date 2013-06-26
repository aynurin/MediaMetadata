using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaMetadata
{
    public abstract class MediaParser
    {
        public static Metadata ExtractMetadata(Stream dataStream)
        {
            var reader = CreateContainerReader(dataStream);
            var parser = CreateMediaParser(reader);
            return parser.ExtractMetadata(reader);
        }

        private static FileReader CreateContainerReader(Stream dataStream)
        {
            return new FileReader(dataStream);
        }

        private static MediaParser CreateMediaParser(FileReader reader)
        {
            var parser = new BMP.BMPParser();
            if (parser.CanParse(reader))
                return parser;
            return null;
        }

        public abstract bool CanParse(FileReader reader);

        public abstract Metadata ExtractMetadata(FileReader reader);
    }
}
