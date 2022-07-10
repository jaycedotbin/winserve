namespace winserve.Tests
{
    [TestClass]
    public class TestReadingFiles
    {
        [TestMethod]
        public void IsItLoadingHTMLFiles()
        {
            Server server = new Server(3000, Directory.GetCurrentDirectory(), "localhost");

            server.Start();
        }
    }
}