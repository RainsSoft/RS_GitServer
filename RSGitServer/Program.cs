using RSGit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RSGitServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            Console.WriteLine("creat git lib and commit");
            var remoteGit = new RepoBuilder().Build2Files3Commits();
            //args = new string[] { "d:\\Git\\daemon", "49750","pylib1"};
            var server = new MiniGitServer(remoteGit);
            //var t = new TaskFactory().StartNew(
            //    () => 
            //    server.StartDaemon(49750))
            //    ;

            //while (!server.Running.HasValue)
            //    Thread.Sleep(50);
            Console.WriteLine("Start git as a GitServer");
            server.StartDaemon(49750);
            return;
#endif
           
            //new GrammarLine("Start git as a git", new[] { "daemon", "<port>" }, (git, args) => { new MiniGitServer(git).StartDaemon(int.Parse(args[1])); }),
            var git = new RSGit.MiniGit(new DirectoryInfo(args[0]).FullName);
            git.InitializeRepository();
           
            var gitServer= new MiniGitServer(git);
            gitServer.StartDaemon(int.Parse(args[1]));
            Console.WriteLine("Start git as a GitServer");
        
        }
        
    }
}
