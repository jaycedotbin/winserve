internal class Utilities
{
     internal static void ShowHelp()
    {
        Console.WriteLine("\tUsage: http-serve <options>");


        Console.WriteLine($"{Environment.NewLine}");
        Console.WriteLine("\tOptions:");
        Console.WriteLine("\t\t--help      - displays the help information for http-serve");
        Console.WriteLine("\t\t--port      - tell http-serve on which port to serve the index.html file");
    }

    internal static void GetCurrentDir()
    {
        string currentDir = Environment.CurrentDirectory;
        Console.WriteLine(currentDir);
    }
}
