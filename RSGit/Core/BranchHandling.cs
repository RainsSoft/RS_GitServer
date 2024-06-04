using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSGit
{
    /// <summary>
    /// 分枝操作
    /// </summary>
    public class BranchHandling
    {
        private readonly Storage Hd;
        private readonly string codeFolder;

        public BranchHandling(Storage hd, string codeFolder)
        {
            Hd = hd;
            this.codeFolder = codeFolder;
        }

        /// <summary> Create a branch: e.g "git checkout -b foo" </summary>
        public string CreateBranch(string name) => CreateBranch(name, Hd.Head.GetId(Hd));

        /// <summary> Create a branch: e.g "git checkout -b foo fb1234.."</summary>
        public string CreateBranch(string name, Id position)
        {
            Hd.Branches.Add(name, new Branch(position, position));
            Hd.ResetCodeFolder(codeFolder, position);
            Hd.Head.Update(name, Hd);
            return $"Switched to a new branch '{name}'";
        }

        /// <summary>
        /// return all branches and highlight current branch: "git branch"
        /// </summary>
        public string ListBranches()
        {
            var branched = Hd.Branches
                .OrderBy(x => x.Key)
                .Select(x => $"{(Hd.Head.Branch == x.Key ? "*" : " ")} {x.Key}");

            var detached = Hd.Head.IsDetachedHead() ? $"* (HEAD detached at {Hd.Head.Id.ToString().Substring(0, 7)})\r\n" : "";

            return detached + string.Join("\r\n", branched);
        }

        /// <summary>
        /// Delete a branch. eg. "git branch -D name"
        /// </summary>
        public string DeleteBranch(string branch)
        {
            if (Hd.Head.Branch == branch)
                throw new Exception($"error: Cannot delete branch '{branch}' checked out");

            var id = Hd.Head.GetId(Hd);
            Hd.Branches.Remove(branch);
            return $"Deleted branch {branch} (was {id.ShaId}).";
        }

        /// <summary>
        /// Change HEAD to branch,e.g. "git checkout featurebranch"
        /// </summary>
        public string Checkout(string branch)
        {
            Checkout(Hd.Branches[branch].Tip);
            return $"Switched to a new branch {branch}";
        }

        /// <summary>
        /// Change folder content to commit position and move HEAD 
        /// </summary>
        public string Checkout(Id id)
        {
            Hd.ResetCodeFolder(codeFolder, id);
            return Hd.Head.Update(id, Hd);
        }

        public void ResetBranchPointer(string branch, Id newTip) => Hd.Branches[branch].Tip = newTip;
    }
}
