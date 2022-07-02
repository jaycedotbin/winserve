using System.Net;
using System.Net.Sockets;

internal class Server
{
    private TcpListener? listener;
    internal void Start(int port = 3000)
    {

        try
        {
            IPAddress localAddress = IPAddress.Parse("127.0.0.1");

            listener = new TcpListener(localAddress, port);

            listener.Start();

            string currentDirectory = Directory.GetCurrentDirectory();
            var indexHTML = Directory.EnumerateFiles(
                currentDirectory,
                "index.html",
                SearchOption.AllDirectories).FirstOrDefault();


            if (File.Exists(indexHTML))
            {
                Console.WriteLine("index.html has been found!");
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketeException: {0}", e);
        }
        finally
        {
            ArgumentNullException.ThrowIfNull(listener);
            listener.Stop();
        }

        Console.WriteLine($"{Environment.NewLine}Hit enter to continue");
    }
}