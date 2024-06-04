using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RSGit
{

    /// <summary>
    /// 储存
    /// </summary>
    [Serializable]
    public class Storage
    {
        public Dictionary<Id, BlobNode> Blobs = new Dictionary<Id, BlobNode>();
        public Dictionary<Id, TreeNode> Trees = new Dictionary<Id, TreeNode>();
        public Dictionary<Id, CommitNode> Commits = new Dictionary<Id, CommitNode>();

        public Dictionary<string, Branch> Branches = new Dictionary<string, Branch>();
        public Head Head = new Head();
        public List<Remote> Remotes = new List<Remote>();

        internal void ResetCodeFolder(string codeFolder, Id position)
        {
            Directory.EnumerateDirectories(codeFolder).Where(x => { Console.WriteLine($"delete '{x}'"); return true; }).ToList().ForEach(x => Directory.Delete(x, true));
            Directory.EnumerateFiles(codeFolder).Where(x => x != Path.Combine(codeFolder, ".git")).Where(x => { Console.WriteLine($"delete '{x}'"); return true; }).ToList().ForEach(x => File.Delete(x));

            if (position != null)
            {
                var commit = Commits[position];
                foreach (BlobTreeLine line in commit.Tree.Lines)
                {
                    //var path = Path.Combine(codeFolder, line.Path);
                    //Console.WriteLine($"Restoring \'{path}\' <- '{codeFolder}' + '{line.Path}'");
                    File.WriteAllText(Path.Combine(codeFolder, line.Path), line.Blob.Content);
                }
            }
        }
    }
}
