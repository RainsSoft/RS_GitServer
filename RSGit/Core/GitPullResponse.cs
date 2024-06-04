using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSGit
{
    /// <summary>
    /// 拉取响应
    /// </summary>
    [Serializable]
    public class GitPullResponse
    {
        public KeyValuePair<Id, CommitNode>[] Commits { get; set; }
        public Branch BranchInfo { get; set; }
    }
}
