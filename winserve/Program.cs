Console.WriteLine($"{Environment.NewLine}");
Console.WriteLine("\tHTTP Server");

if (args.Length > 0)
{
    foreach (string? arg in args)
    {
        if (arg == "--help")
        {
            Utilities.ShowHelp();
        }
    }

}
else
{
    Server server = new();
    Utilities.GetCurrentDir();
}
