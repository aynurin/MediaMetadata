using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MediaMetadata
{
    class MediaParserFactory
    {
        private static List<MediaParser> _knownParsers = new List<MediaParser>();
        private static List<Type> _knownReaderTypes = new List<Type>();

        static MediaParserFactory()
        {
            _knownParsers = LoadTypes<MediaParser>();
            _knownReaderTypes = LoadTypes(typeof(FileReader));
        }

        private static List<T> LoadTypes<T>()
            where T : class
        {
            var instances = new List<T>();
            var types = LoadTypes(typeof(T));
            foreach (var type in types)
                instances.Add(Activator.CreateInstance(type) as T);
            return instances;
        }

        private static List<Type> LoadTypes(Type baseType)
        {
            var types = new List<Type>();
            var allTypes = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in allTypes)
            {
                if (type.IsSubclassOf(baseType))
                    types.Add(type);
            }
            return types;
        }

        internal static MediaParser CreateParser(System.IO.Stream dataStream)
        {
            foreach (var parser in _knownParsers)
            {
                if (parser.CanParse(dataStream))
                    return parser;
            }
            // TODO: find an appropriate container reader if no media parser found
            return null;
        }
    }
}
