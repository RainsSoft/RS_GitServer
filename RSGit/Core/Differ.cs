using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RSGit
{
    /// <summary>
    /// 差异
    /// </summary>
    public class Differ
    {
        public enum Kind { Deleted, Added }

        public class Result
        {
            public Tuple<string, int>[] Lines;
            public Kind Kind;
        }

        public List<Result> Diff(string[] a, string[] b)
        {
            return Diff(a.Select((x, i) => Tuple.Create(string.Intern(x), i + 1)).ToArray(),
                    b.Select((x, i) => Tuple.Create(string.Intern(x), i + 1)).ToArray())
                .Where(x => x.Lines.Any()).ToList();
        }

        IEnumerable<Result> Diff(Tuple<string, int>[] a, Tuple<string, int>[] b)
        {
            int longestOverlap = 0, offsetA = -1, offsetB = -1, overlap;
            for (int ia = 0; ia < a.Length; ia++)
            {
                for (int ib = 0; ib < b.Length; ib++)
                {
                    for (overlap = 0; ia + overlap < a.Length && ib + overlap < b.Length && a[ia + overlap].Item1 == b[ib + overlap].Item1; overlap++)
                        ;

                    if (overlap > longestOverlap)
                    {
                        longestOverlap = overlap; offsetA = ia; offsetB = ib;
                    }
                }
            }

            if (longestOverlap == 0)
                return new[] { new Result { Kind = Kind.Deleted, Lines = a }, new Result { Kind = Kind.Added, Lines = b } };

            return Diff(a.Take(offsetA).ToArray(), b.Take(offsetB).ToArray())
                .Union(Diff(a.Skip(offsetA + longestOverlap).ToArray(), b.Skip(offsetB + longestOverlap).ToArray()));
        }
    }
}
