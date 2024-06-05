using System;
using System.IO;
using RSGit;

namespace RSGit
{
	public class RepoBuilder
	{
		readonly string basePath;
		public MiniGit Git;

		public RepoBuilder() : this(@"d:\Git\", Guid.NewGuid()) { }

		public RepoBuilder(Guid unittestguid) : this(@"d:\Git\", unittestguid) { }

		public RepoBuilder(string basePath) : this(basePath, Guid.NewGuid()) { }

		public RepoBuilder(string basePath, Guid unittestguid)
		{
			this.basePath = Path.Combine(basePath, $"rsgit\\{unittestguid}");
			Directory.CreateDirectory(this.basePath);
		}
		/// <summary>
		/// 新建空库
		/// </summary>
		/// <returns></returns>
		public MiniGit BuildEmptyRepo()
		{
			Git = new MiniGit(basePath);
			Git.InitializeRepository();
			return Git;
		}

		public RepoBuilder EmptyRepo()
		{
			Git = new MiniGit(basePath);
			Git.InitializeRepository();
			return this;
		}

		public MiniGit Build2Files3Commits()
		{
			Git = BuildEmptyRepo();

			AddFile("a.txt", "aaaaa");
			Git.Commit("Add a", "kasper", new DateTime(2017,1,1,1,1,1));

			AddFile("b.txt", "bbbb");
			Git.Commit("Add b", "kasper", new DateTime(2017, 2, 2, 2, 2, 2));

			AddFile("a.txt", "v2av2av2av2a");
			Git.Commit("Add a2", "kasper", new DateTime(2017, 3, 3, 3, 3, 3));

			return Git;
		}

		public RepoBuilder AddFile(string path) => AddFile(path, Guid.NewGuid().ToString());

		public RepoBuilder AddFile(string path, string content)
		{
			var filepath = Path.Combine(Git.CodeFolder, path);
			new FileInfo(filepath).Directory.Create();

			File.WriteAllText(filepath, content);
			return this;
		}

		public string ReadFile(string path)
		{
			return File.ReadAllText(Path.Combine(Git.CodeFolder, path));
		}

		public RepoBuilder DeleteFile(string path)
		{
			File.Delete(Path.Combine(basePath, path));
			return this;
		}

		public Id Commit() => Commit("Some message");

		public Id Commit(string message)
		{
			return Git.Commit(message, "author", DateTime.Now);
		}

		public RepoBuilder NewBranch(string branch)
		{
			Git.Branches.CreateBranch(branch);
			return this;
		}

		public RepoBuilder AddLocalHostRemote(int port)
		{
			Git.Hd.Remotes.Add(new Remote()
			{
				Name = "origin",
				Url = new Uri($"http://localhost:{port}")
			});
			return this;
		}
	}
}