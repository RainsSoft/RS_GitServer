using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace RSGit
{
    public static class ByteHelper
    {
        static readonly SHA256 Sha = SHA256.Create();

        public static string ComputeSha(object o) => string.Join("", Sha.ComputeHash(Serialize(o)).Select(x => String.Format("{0:x2}", x)));

        public static byte[] Serialize(object o)
        {
            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, o);
                stream.Seek(0, SeekOrigin.Begin);
                return stream.GetBuffer();
            }
        }

        public static T Deserialize<T>(Stream s) where T : class => (T)new BinaryFormatter().Deserialize(s);

        public static T Deserialize<T>(byte[] o) where T : class
        {
            using (var ms = new MemoryStream(o))
            {
                return (T)new BinaryFormatter().Deserialize(ms);
            }
        }
    }
}
