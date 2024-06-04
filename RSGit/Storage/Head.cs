using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSGit
{

    /// <summary>
    /// In git the file content of the file "HEAD" is either an ID or a reference to a branch.eg.
    /// "ref: refs/heads/master"
    /// </summary>
    [Serializable]
    public class Head
    {
        public Id Id { get; private set; }
        public string Branch { get; private set; }

        public void Update(string branch, Storage s)
        {
            if (!s.Branches.ContainsKey(branch))
                throw new ArgumentOutOfRangeException($"No branch named \'{branch}\'");

            Branch = branch;
            Id = null;
        }

        public string Update(Id position, Storage s)
        {
            var b = s.Branches.FirstOrDefault(x => x.Value.Tip.Equals(position));
            if (b.Key == null)
            {
                if (!s.Commits.ContainsKey(position))
                    throw new ArgumentOutOfRangeException($"No commit with id '{position}'");

                Branch = null;
                Id = position;
                return "You are in 'detached HEAD' state. You can look around, make experimental changes and commit them, and you can discard any commits you make in this state without impacting any branches by performing another checkout.";
            }

            Update(b.Key, s);
            return null;
        }

        public bool IsDetachedHead() => Id != null;

        public Id GetId(Storage s) => Id ?? s.Branches[Branch].Tip;
    }
}
