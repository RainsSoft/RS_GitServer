using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;

namespace RSGit
{
    /// <summary>
    /// Used for communicating with a git server
    /// </summary>
    public class MiniGitNetworkClient
    {
        
        public void PushBranch(Remote remote, string branch, Branch branchInfo, Id fromPosition, KeyValuePair<Id, CommitNode>[] nodes)
        {
            var request = new GitPushBranchRequest() { Branch = branch, BranchInfo = branchInfo, LatestRemoteBranchPosition = fromPosition, Commits = nodes };
            var result = new HttpClient().PostAsync(remote.Url, new ByteArrayContent(ByteHelper.Serialize(request))).GetAwaiter().GetResult();
            Console.WriteLine($"Push status: {result.StatusCode}");
        }

        public Id PullBranch(Remote remote, string branch, MiniGit git)
        {
#if DEBUG
            Console.WriteLine(remote.Url + "?branch=" + branch);
#endif
            var bytes = new HttpClient().GetByteArrayAsync(remote.Url + "?branch=" + branch).GetAwaiter().GetResult();
            var commits = ByteHelper.Deserialize<GitPullResponse>(bytes);
            git.RawImportCommits(commits.Commits, $"{remote.Name}/{branch}", commits.BranchInfo);
            return commits.BranchInfo.Tip;
        }

        public void CloneBranch(MiniGit git, string remotename, string url, string branch)
        {
            git.InitializeRepository();
            git.Remotes.Remotes.Add(new Remote { Name = remotename, Url = new Uri(url) });
            var tip = PullBranch(git.Remotes.Remotes.Single(), branch, git);
            git.Branches.ResetBranchPointer("master", tip);
            git.Branches.Checkout("master");
        }
    }
}
