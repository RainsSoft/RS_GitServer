using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSGit
{
    /// <summary>
    /// 文件信息
    /// </summary>
    public class Fileinfo
    {
        public readonly string Path;
        public readonly string Content;

        public Fileinfo(string path, string content)
        {
            Path = path;
            Content = content;
        }
    }
}
