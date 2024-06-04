using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSGit
{
    public class CommandLineHandling
    {
        public static readonly GrammarLine[] Config =
        {
            new GrammarLine("Initialize an empty repo", new[] { "init"}, (git, args) => git.InitializeRepository()),
            new GrammarLine("Make a commit", new[] { "commit", "-m", "<message>"}, (git, args) => { git.Commit(args[2], "author", DateTime.Now); }),
            new GrammarLine("Show the commit log", new[] { "log"}, (git, args) => git.Log()),
            new GrammarLine("Create a new new branch at HEAD", new[] { "checkout", "-b", "<branchname>"}, (git, args) => git.Branches.CreateBranch(args[2])),
            new GrammarLine("Create a new new branch at commit id", new[] { "checkout", "-b", "<branchname>", "<id>"}, (git, args) => git.Branches.CreateBranch(args[2], new Id(args[3]))),
            new GrammarLine("Update HEAD", new[] { "checkout", "<id|name>"}, (git, args) => git.Hd.Branches.ContainsKey(args[1]) ? git.Branches.Checkout(args[1]) : git.Branches.Checkout(new Id(args[1]))),
            new GrammarLine("Delete a branch", new[] { "branch", "-D", "<branchname>"}, (git, args) => git.Branches.DeleteBranch(args[2])),
            new GrammarLine("List existing branches", new[] { "branch"}, (git, args) => git.Branches.ListBranches()),
            new GrammarLine("Garbage collect", new[] { "gc" }, (git, args) => { git.Gc(); }),
            new GrammarLine("Start git as a git", new[] { "daemon", "<port>" }, (git, args) => { new MiniGitServer(git).StartDaemon(int.Parse(args[1])); }),
            new GrammarLine("Pull code", new[] { "pull", "<remote-name>", "<branch>"}, (git, args) => { new MiniGitNetworkClient().PullBranch(git.Hd.Remotes.First(x => x.Name == args[1]), args[2], git);}),
            new GrammarLine("Push code", new[] { "push", "<remote-name>", "<branch>"}, (git, args) => { new MiniGitNetworkClient().PushBranch(git.Hd.Remotes.First(x => x.Name == args[1]), args[2], git.Hd.Branches[args[2]], null, git.GetReachableNodes(git.Hd.Branches[args[2]].Tip).ToArray()); }),
            new GrammarLine("Clone code from other server", new[] { "clone", "<url>", "<branch>"}, (git, args) => { new MiniGitNetworkClient().CloneBranch(git, "origin", args[1], args[2]); }),
            new GrammarLine("List remotes", new[] { "remote", "-v"}, (git, args) => { git.Remotes.List(); }),
            new GrammarLine("Add remote", new[] { "remote", "add", "<remote-name>", "<url>"}, (git, args) => { git.Remotes.Remotes.Add(new Remote(){Name = args[2], Url = new Uri(args[3])}); }),
            new GrammarLine("Remove remote", new[] { "remote", "rm", "<remote-name>"}, (git, args) => { git.Remotes.Remove(args[2]); }),
        };

        public string Handle(MiniGit git, GrammarLine[] config, string[] cmdParams)
        {
            var match = config.SingleOrDefault(x => x.Grammar.Length == cmdParams.Length
                                                 && x.Grammar.Zip(cmdParams, (gramar, arg) => gramar.StartsWith("<") || gramar == arg).All(m => m));

            if (match == null)
                return $"KBGit Help\r\n----------\r\ngit {string.Join("\r\ngit ", config.Select(x => $"{string.Join(" ", x.Grammar),-34} - {x.Explanation}."))}";

            git.LoadState();
            var result = match.ActionOnMatch(git, cmdParams);
            git.StoreState();

            return result;
        }
    }
}
