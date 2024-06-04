using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSGit
{
    /// <summary>
    ///  二进制大对象
    /// </summary>
    [Serializable]
    public class BlobNode
    {
        public string Content { get; }

        public BlobNode(string content) => Content = content;
    }
}
