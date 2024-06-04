using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSGit
{


    [Serializable]
    public class Id
    {
        public string ShaId { get; private set; }

        public Id(string sha)
        {
            if (sha == null || sha.Length != 64)
                throw new ArgumentException("Not a valid SHA");
            ShaId = sha;
        }

        /// <summary>
        /// Equivalent to "git hash-object -w <file>"
        /// </summary>
        public static Id HashObject(object o) => new Id(ByteHelper.ComputeSha(o));

        public override string ToString() => ShaId;
        public override bool Equals(object obj) => ShaId.Equals((obj as Id)?.ShaId);
        public override int GetHashCode() => ShaId.GetHashCode();
    }
}
