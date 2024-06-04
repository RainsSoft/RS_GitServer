using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RSGit
{

    /// <summary>
    /// Mini clone of git
    /// Supporting
    /// * commits
    /// * branches
    /// * detached heads
    /// * checkout old commits
    /// * logging
    /// </summary>
    public class MiniGit
    {
        public const string KbGitDataFile = ".git";
        public string CodeFolder { get; }
        public Storage Hd;
        public RemotesHandling Remotes;
        public BranchHandling Branches;

        public MiniGit(string startpath)
        {
            CodeFolder = startpath;
        }

        public string GitStateFile => Path.Combine(CodeFolder, ".git");

        public void LoadState()
        {
            if (File.Exists(GitStateFile))
            {
                Hd = ByteHelper.Deserialize<Storage>(File.ReadAllBytes(GitStateFile));
                Remotes = new RemotesHandling(Hd.Remotes);
                Branches = new BranchHandling(Hd, CodeFolder);
            }
        }

        public void StoreState()
        {
            if (Hd != null)
                File.WriteAllBytes(GitStateFile, ByteHelper.Serialize(Hd));
        }

        /// <summary>
        /// Initialize a repo. eg. "git init"
        /// </summary>
        public string InitializeRepository()
        {
            Hd = new Storage();
            Remotes = new RemotesHandling(Hd.Remotes);
            Branches = new BranchHandling(Hd, CodeFolder);
            Branches.CreateBranch("master", null);
            return "Initialized empty Git repository";
        }

        /// <summary>
        /// Simulate syntax: e.g. "HEAD~2"
        /// </summary>
        public Id HeadRef(int numberOfPredecessors)
        {
            var result = Hd.Head.GetId(Hd);
            for (int i = 0; i < numberOfPredecessors; i++)
            {
                result = Hd.Commits[result].Parents.First();
            }

            return result;
        }

        /// <summary>
        /// Equivalent to "git hash-object -w <file>"
        /// </summary>
        public Id HashObject(string content) => Id.HashObject(content);

        public Id Commit(string message, string author, DateTime now)
        {
            var composite = FileSystemScanFolder(CodeFolder);
            composite.Visit(x =>
            {
                if (x is TreeTreeLine t)
                    Hd.Trees.TryAdd(t.Id, t.Tree);
                if (x is BlobTreeLine b)
                    Hd.Blobs.TryAdd(b.Id, b.Blob);
            });

            var parentCommitId = Hd.Head.GetId(Hd);
            var isFirstCommit = parentCommitId == null;
            var commit = new CommitNode
            {
                Time = now,
                Tree = composite.Tree,
                TreeId = composite.Id,
                Author = author,
                Message = message,
                Parents = isFirstCommit ? new Id[0] : new[] { parentCommitId },
            };

            var commitId = Id.HashObject(commit);
            Hd.Commits.Add(commitId, commit);

            if (Hd.Head.IsDetachedHead())
                Hd.Head.Update(commitId, Hd);
            else
                Hd.Branches[Hd.Head.Branch].Tip = commitId;

            return commitId;
        }

        internal void AddOrSetBranch(string branch, Branch branchInfo)
        {
            if (Hd.Branches.ContainsKey(branch))
                Hd.Branches[branch].Tip = branchInfo.Tip;
            else
                Hd.Branches.Add(branch, branchInfo);
        }

        /// <summary> eg. "git log" </summary>
        public string Log()
        {
            var sb = new StringBuilder();
            foreach (var branch in Hd.Branches)
            {
                sb.AppendLine($"\r\nLog for {branch.Key}");

                if (branch.Value.Tip == null) // empty repo
                    continue;

                var nodes = GetReachableNodes(branch.Value.Tip, branch.Value.Created);
                foreach (var comit in nodes.OrderByDescending(x => x.Value.Time))
                {
                    var commitnode = comit.Value;
                    var key = comit.Key.ToString();
                    var msg = commitnode.Message.Substring(0, Math.Min(40, commitnode.Message.Length));
                    var author = $"{commitnode.Author}";

                    sb.AppendLine($"* {key} - {msg} ({commitnode.Time:yyyy\\/MM\\/dd hh\\:mm\\:ss}) <{author}> ");
                }
            }

            return sb.ToString();
        }

        /// <summary> Clean out unreferences nodes. Equivalent to "git gc" </summary>
        public void Gc()
        {
            var reachables = Hd.Branches.Select(x => x.Value.Tip)
                .Union(new[] { Hd.Head.GetId(Hd) })
                .SelectMany(x => GetReachableNodes(x))
                .Select(x => x.Key);

            var deletes = Hd.Commits.Select(x => x.Key)
                .Except(reachables).ToList();

            deletes.ForEach(x => Hd.Commits.Remove(x));
        }

        internal void RawImportCommits(KeyValuePair<Id, CommitNode>[] commits, string branch, Branch branchInfo)
        {
            //Console.WriteLine("RawImportCommits");

            foreach (var commit in commits)
            {
                //Console.WriteLine("import c" + commit.Key);
                Hd.Commits.TryAdd(commit.Key, commit.Value);
                Hd.Trees.TryAdd(commit.Value.TreeId, commit.Value.Tree);

                foreach (var treeLine in commit.Value.Tree.Lines)
                {
                    if (treeLine is BlobTreeLine b)
                    {
                        //Console.WriteLine("import b " + b.Id);
                        Hd.Blobs.TryAdd(b.Id, b.Blob);
                    }

                    if (treeLine is TreeTreeLine t)
                    {
                        //Console.WriteLine("import t " + t.Id);
                        Hd.Trees.TryAdd(t.Id, t.Tree);
                    }
                }
            }

            AddOrSetBranch(branch, branchInfo);
        }

        public List<KeyValuePair<Id, CommitNode>> GetReachableNodes(Id from, Id downTo = null)
        {
            var result = new List<KeyValuePair<Id, CommitNode>>();
            GetReachableNodes(from);

            void GetReachableNodes(Id currentId)
            {
                var commit = Hd.Commits[currentId];
                result.Add(new KeyValuePair<Id, CommitNode>(currentId, commit));

                foreach (var parent in commit.Parents.Where(x => !x.Equals(downTo)))
                {
                    GetReachableNodes(parent);
                }
            }

            return result;
        }

        public TreeTreeLine FileSystemScanFolder(string path) => MakeTreeTreeLine(path);

        public ITreeLine[] FileSystemScanSubFolder(string path)
        {
            var entries = new DirectoryInfo(path).EnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly).ToArray();

            var lines = new List<ITreeLine>(entries.OfType<FileInfo>()
                .Where(x => x.FullName != Path.Combine(CodeFolder, ".git"))
                .Select(x => new { Content = File.ReadAllText(x.FullName), x.FullName })
                .Select(x => new BlobTreeLine(new Id(ByteHelper.ComputeSha(x.Content)), new BlobNode(x.Content), x.FullName.Substring(CodeFolder.Length + 1))));

            lines.AddRange(entries.OfType<DirectoryInfo>()
                .Select(x => MakeTreeTreeLine(EnsurePathEndsInSlash(x.FullName))));

            return lines.ToArray();
        }

        private TreeTreeLine MakeTreeTreeLine(string path)
        {
            var folderentries = FileSystemScanSubFolder(path);
            var treenode = new TreeNode(folderentries);
            var id = Id.HashObject(folderentries);

            return new TreeTreeLine(id, treenode, EnsurePathEndsInSlash(path).Substring(CodeFolder.Length + 1));
        }

        public string EnsurePathEndsInSlash(string folderPath) => folderPath.EndsWith(Path.DirectorySeparatorChar.ToString()) ? folderPath : (folderPath + Path.DirectorySeparatorChar);
    }

}
