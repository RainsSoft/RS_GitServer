using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSGit
{
    /// <summary>
    /// 推送分支请求
    /// </summary>
    [Serializable]
    public class GitPushBranchRequest
    {
        public KeyValuePair<Id, CommitNode>[] Commits { get; set; }
        public string Branch { get; set; }
        public Branch BranchInfo { get; set; }
        public Id LatestRemoteBranchPosition { get; set; }
    }
}
