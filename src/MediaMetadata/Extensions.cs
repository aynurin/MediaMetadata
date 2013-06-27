using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MediaMetadata
{
    internal static class Extensions
    {
        // http://stackoverflow.com/questions/2871/reading-a-c-c-data-structure-in-c-sharp-from-a-byte-array by Coincoin
        public static T ToStructure<T>(this byte[] bytes) where T : struct
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return stuff;
        }

        public static byte[] ReadBytes(this Stream stream, int count)
        {
            var buffer = new byte[count];
            int read = 0, thisRead;
            do
            {
                thisRead = stream.Read(buffer, read, count - read);
                read += thisRead;
            } while (thisRead > 0 && read < count);
            return buffer;
        }

        public static byte[] PeekBytes(this Stream stream, int count)
        {
            var pos = stream.Position;
            var buffer = new byte[count];
            int read = 0, thisRead;
            do
            {
                thisRead = stream.Read(buffer, read, count - read);
                read += thisRead;
            } while (thisRead > 0 && read < count);
            stream.Position = pos;
            return buffer;
        }

        public static bool StartsWith(this byte[] thisBytes, byte[] thatBytes)
        {
            for (int i = 0; i < thatBytes.Length; i += 1)
            {
                if (thisBytes[i] != thatBytes[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool StartsWith(this Stream s, params byte[] thatBytes)
        {
            var thisBytes = s.PeekBytes(thatBytes.Length);
            return thisBytes.StartsWith(thatBytes);
        }
    }
}
