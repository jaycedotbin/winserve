using System.Net;

internal class Server
{
    internal void Start(string[] prefixes)
    {
        HttpListener httpListener = new HttpListener();

        if (!HttpListener.IsSupported)
        {
            Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
            return;
        }

        if (prefixes is null || prefixes.Length == 0)
        {
            throw new ArgumentException("prefixes");

        }
        foreach (string s in prefixes)
        {
            httpListener.Prefixes.Add(s);
        }

        httpListener.Start();
        Console.WriteLine("Listening...");
        // Note: The GetContext method blocks while waiting for a request.
        HttpListenerContext context = httpListener.GetContext();
        HttpListenerRequest request = context.Request;
        // Obtain a response object.
        HttpListenerResponse response = context.Response;
        // Construct a response.
        string responseString = GetIndexHTMLFile();
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        // Get a response stream and write the response to it.
        response.ContentLength64 = buffer.Length;
        Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        // You must close the output stream.
        output.Close();
        httpListener.Stop();
    }

    internal string GetIndexHTMLFile()
    {
        string path = Directory.GetCurrentDirectory();

        try
        {
            var htmlFilesInDirectory = Directory.EnumerateFiles(path, "*.html", SearchOption.AllDirectories);

            foreach (string currentFile in htmlFilesInDirectory)
            {
                string content = File.ReadAllText(currentFile);
                return content;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return "";
    }
}
