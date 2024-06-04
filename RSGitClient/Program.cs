using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSGit;

namespace RSGitClient
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //args = new string[] { "daemon", "49750" };
            var output = new CommandLineHandling().Handle(new MiniGit(new DirectoryInfo(".").FullName), CommandLineHandling.Config, args);
            Console.WriteLine(output);
        }
    }
}
