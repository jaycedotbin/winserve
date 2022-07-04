using System.CommandLine;

namespace winserve
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var portOption = new Option<int>(
                name: "--port",
                description: "displays the help information for http-serve");

            var rootCommand = new RootCommand("winserve - Serve HTML, CSS and JavaScript files only using a single executable");
            rootCommand.AddOption(portOption);


            rootCommand.SetHandler((port) =>
            {
                Server server = new();
                string[] prefixes = { $"http://localhost:{port | 3000}/" };
                server.Start(prefixes);
            }, portOption);

            return await rootCommand.InvokeAsync(args);
        }
    }
}
