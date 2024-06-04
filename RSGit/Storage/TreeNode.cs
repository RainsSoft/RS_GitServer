using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSGit
{
    [Serializable]
    public class TreeNode
    {
        public ITreeLine[] Lines;
        public TreeNode(ITreeLine[] lines)
        {
            Lines = lines;
        }

        public override string ToString() => string.Join("\n", Lines.Select(x => x.ToString()));
    }
}
