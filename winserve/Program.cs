using System.CommandLine;

namespace winserve
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var portOption = new Option<int>(
                name: "--port",
                description: "displays the help information for http-serve",
                getDefaultValue: () => 3000);

            var pathOption = new Option<string>(
                name: "--path",
                description: "",
                getDefaultValue: () => Directory.GetCurrentDirectory());

            var ipOption = new Option<string>(
                name: "--ip",
                description: "",
                getDefaultValue: () => "localhost");

            var rootCommand = new RootCommand("winserve - Serve HTML, CSS and JavaScript files only using a single executable") { portOption, ipOption, pathOption };

            rootCommand.SetHandler((port, ip, path) =>
            {
                Server server = new Server(port, path, ip);
                server.Start();
            }, portOption, ipOption, pathOption);

            return await rootCommand.InvokeAsync(args);
        }
    }
}
