using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSGit
{

    public interface ITreeLine
    {
        void Visit(Action<ITreeLine> code);
    }

    [Serializable]
    public class BlobTreeLine : ITreeLine
    {
        public Id Id { get; private set; }
        public BlobNode Blob { get; private set; }
        public string Path { get; private set; }

        public BlobTreeLine(Id id, BlobNode blob, string path)
        {
            Id = id;
            Blob = blob;
            Path = path;
        }

        public override string ToString() => $"blob {Path}";

        public void Visit(Action<ITreeLine> code) => code(this);
    }

    [Serializable]
    public class TreeTreeLine : ITreeLine
    {
        public Id Id { get; private set; }
        public TreeNode Tree { get; private set; }
        public string Path { get; private set; }

        public TreeTreeLine(Id id, TreeNode tree, string path)
        {
            Id = id;
            Tree = tree;
            Path = path;
        }

        public override string ToString() => $"tree {Tree.Lines.Length} {Path}\r\n{string.Join("\r\n", Tree.Lines.Select(x => x.ToString()))}";

        public void Visit(Action<ITreeLine> code)
        {
            code(this);

            foreach (var line in Tree.Lines)
                line.Visit(code);
        }
    }
}
