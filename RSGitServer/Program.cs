using RSGit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSGitServer
{
    internal class Program
    {
        static void Main(string[] args)
        {  
            args = new string[] { "d:\\Git\\daemon", "49750" };
            //new GrammarLine("Start git as a git", new[] { "daemon", "<port>" }, (git, args) => { new MiniGitServer(git).StartDaemon(int.Parse(args[1])); }),
            var git = new MiniGit(new DirectoryInfo(args[0]).FullName);
            var gitServer= new MiniGitServer(git);
            gitServer.StartDaemon(int.Parse(args[1]));
            Console.WriteLine("Start git as a GitServer");
        
        }
        
    }
}
