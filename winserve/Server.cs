using System.Net;
using System.Net.Sockets;
using Utilities;

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

            string currentDir = FileManagement.GetCurrentDir();

            byte[] bytes = new byte[256];
            string? data = null;

            while (true)
            {
                Console.Write("Waiting for a connection...");

                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Connected!");

                data = null;

                NetworkStream stream = client.GetStream();

                int i;

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);

                    data = data.ToUpper();

                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine("Sent: {0}", data);
                }

                client.Close();
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