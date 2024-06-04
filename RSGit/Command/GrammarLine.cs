using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSGit
{
    /// <summary>
    /// 语法行
    /// </summary>
    public class GrammarLine
    {
        public readonly string Explanation;
        public readonly string[] Grammar;
        public readonly Func<MiniGit, string[], string> ActionOnMatch;

        public GrammarLine(string explanation, string[] grammar, Action<MiniGit, string[]> actionOnMatch) : this(explanation, grammar, (git, arg) => { actionOnMatch(git, arg); return null; })
        { }

        public GrammarLine(string explanation, string[] grammar, Func<MiniGit, string[], string> actionOnMatch)
        {
            Explanation = explanation; Grammar = grammar; ActionOnMatch = actionOnMatch;
        }
    }
}
