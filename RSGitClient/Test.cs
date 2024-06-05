using RSGit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSGitClient
{
    internal class Test
    {
        public static void PullBranch(int port=49750) {
            var localGit = new RepoBuilder().EmptyRepo().AddLocalHostRemote(port).Git;

            var gitClient = new MiniGitNetworkClient();
            gitClient.PullBranch(localGit.Hd.Remotes.First(), "master", localGit);
            
            var actual = localGit.Log();
            Console.WriteLine(actual);
        }
        public static void CloneMaster(int port = 49750) {
            var localGit = new RepoBuilder("pylib1").EmptyRepo().AddLocalHostRemote(port).Git;

            var gitClient = new MiniGitNetworkClient();
            string remoteName = localGit.Hd.Remotes.First().Name;
            gitClient.CloneBranch(localGit, "origin", $"http://localhost:{port}","master");

        }

    }
}
