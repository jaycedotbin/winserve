using System.CommandLine;

namespace winserve
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var portOption = new Option<int>(
                name: "--port",
                description: "Specify a custom port to serve files. (default: Port 3000)",
                getDefaultValue: () => 3000);

            var pathOption = new Option<string>(
                name: "--path",
                description: "Specify a custom path for winserve to serve files. (default: Current Directory)",
                getDefaultValue: () => Directory.GetCurrentDirectory());

            var ipOption = new Option<string>(
                name: "--ip",
                description: "Specify a custom IP Address for winserve to serve files. (default: localhost)",
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
