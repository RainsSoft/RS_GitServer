using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSGit
{
    /// <summary>
    /// 提交节点
    /// </summary>
    [Serializable]
    public class CommitNode
    {
        public DateTime Time;
        public TreeNode Tree;
        public Id TreeId;
        public string Author;
        public string Message;
        public Id[] Parents = new Id[0];
    }
}
