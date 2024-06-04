using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace RSGit
{
    public class MiniGitServer
    {
        private readonly MiniGit git;
        private HttpListener listener;
        public bool? Running { get; private set; }

        public MiniGitServer(MiniGit git)
        {
            this.git = git;
        }

        public void Abort()
        {
            Running = false;
            listener?.Abort();
        }

        public void StartDaemon(int port)
        {
            Console.WriteLine($"Serving on http://localhost:{port}/");

            listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{port}/");
            listener.Start();

            Running = true;
            while (Running.Value)
            {
                var context = listener.GetContext();
                try
                {
                    if (context.Request.HttpMethod == "GET")
                    {
                        var branch = context.Request.QueryString.Get("branch");
                        if (string.IsNullOrEmpty(branch))
                        {
                            continue;
                        }
                        if (!git.Hd.Branches.ContainsKey(branch))
                        {
                            context.Response.StatusCode = 404;
                            context.Response.Close();
                            continue;
                        }

                        context.Response.Close(ByteHelper.Serialize(new GitPullResponse()
                        {
                            BranchInfo = git.Hd.Branches[branch],
                            Commits = git.GetReachableNodes(git.Hd.Branches[branch].Tip).ToArray()
                        }), true);
                    }

                    if (context.Request.HttpMethod == "POST")
                    {
                        var req = ByteHelper.Deserialize<GitPushBranchRequest>(context.Request.InputStream);
                        // todo check if we are loosing commits when updating the branch pointer..we get a fromid with the request
                        git.RawImportCommits(req.Commits, req.Branch, req.BranchInfo);
                        context.Response.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"\n\n{DateTime.Now}\n{e} - {e.Message}");
                    context.Response.StatusCode = 500;
                    context.Response.Close();
                }
            }
        }
    }
}
