using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSGit
{
    /// <summary>
    /// 分枝
    /// </summary>
    [Serializable]
    public class Branch
    {
        public Id Created { get; }
        public Id Tip { get; set; }

        public Branch(Id created, Id tip)
        {
            Created = created;
            Tip = tip;
        }
    }
}
