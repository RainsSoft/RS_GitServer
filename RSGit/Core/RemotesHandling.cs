using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSGit
{
    public class RemotesHandling
    {
        public readonly List<Remote> Remotes;

        public RemotesHandling(List<Remote> remotes)
        {
            Remotes = remotes;
        }

        /// <summary>
        /// List remotes Git remote -v
        /// </summary>
        public string List() => string.Join("\r\n", Remotes.Select(x => $"{x.Name,-12} {x.Url}"));

        /// <summary>
        /// Remove a remote
        /// </summary>
        public void Remove(string name) => Remotes.RemoveAll(x => x.Name == name);
    }
}
