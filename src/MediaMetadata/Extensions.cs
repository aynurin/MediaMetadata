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

        public static short ReadLittleEndianInt16(this BinaryReader binaryReader)
        {
            var bytes = new byte[sizeof(short)];
            for (int i = 0; i < sizeof(short); i += 1)
            {
                bytes[sizeof(short) - 1 - i] = binaryReader.ReadByte();
            }
            return BitConverter.ToInt16(bytes, 0);
        }

        public static short ReadLittleEndianInt16(this Stream stream)
        {
            var bytes = stream.ReadBytes(sizeof(short)).Reverse().ToArray();
            return BitConverter.ToInt16(bytes, 0);
        }

        public static short ReadInt16(this Stream stream)
        {
            var bytes = stream.ReadBytes(sizeof(short));
            return BitConverter.ToInt16(bytes, 0);
        }

        public static int ReadLittleEndianInt32(this BinaryReader binaryReader)
        {
            var bytes = new byte[sizeof(int)];
            for (int i = 0; i < sizeof(int); i += 1)
            {
                bytes[sizeof(int) - 1 - i] = binaryReader.ReadByte();
            }
            return BitConverter.ToInt32(bytes, 0);
        }

        public static int ReverseBytes(this int num)
        {
            var bytes = BitConverter.GetBytes(num);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
