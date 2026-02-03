using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WebServer_First
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 8080;
            var serverListener = new TcpListener(ipAddress, port);
            
            serverListener.Start();
            Console.WriteLine("Srever started");
            Console.WriteLine($"Listening on port: {port}");
           var connection = serverListener.AcceptTcpClient();
            var networkStream = connection.GetStream();

            string content = "Hello from the server!";
            int contentLength = Encoding.UTF8.GetByteCount(content);
            var response = $@"HTTP/1.1
Content-Type:text/plain; charset=UTF-8
Content-Length: {contentLength}

{content}";
            var responseBytes = Encoding.UTF8.GetBytes(response);
            networkStream.Write(responseBytes);
            connection.Close();
        }
    }
}
