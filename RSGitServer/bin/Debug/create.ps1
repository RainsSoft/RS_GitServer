
function CreateGitWithCommits()
{
    $git = "C:\src\Git\RS_GitServer\RSGitServer\bin\Debug\RSGitServer.exe"
	"init"
	iex "$git init"

	"adding a"
	& echo "aaaa" > a.txt
	iex "$git commit -m 'file a.txt'"

	"adding b"
	& echo "bbbb" > b.txt
	iex "$git commit -m 'b file'"

	iex "$git log"
	iex "$git daemon 8080"
}