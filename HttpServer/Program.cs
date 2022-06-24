Console.Write($"{Environment.NewLine}");
Console.WriteLine(@"  HTTP Server");

foreach (var arg in args)
{
    if (arg == null)
    {

    }

    if (arg == "--help")
    {
        Console.WriteLine(@"  Usage: http-serve <options>");


        Console.WriteLine($"{Environment.NewLine}");
        Console.WriteLine(@"  Options:");
        Console.WriteLine(@"    --help      - displays the help information for http-serve");
        Console.WriteLine(@"    --port      - tell http-serve on which port to serve the index.html file");

    }
}
