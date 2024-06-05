
function CreateGitWithCommits()
{
    $git = "C:\Work\irobotq\probuct\src\第三方类库\Git\RS_GitServer\Bin\RSGitServer.exe"
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
