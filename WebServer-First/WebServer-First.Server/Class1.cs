using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WebServer_First.Server
{
    public class HttpServer
    {
        private readonly IPAddress iPAddress;
        private readonly int port;
        private readonly TcpListener serverListener;

        public HttpServer(string ipAddress, int port)
        {
            this.iPAddress = IPAddress.Parse(ipAddress);
            this.port = port;
            this.serverListener = new TcpListener(this.iPAddress, this.port);    
        }

        public void Start()
        {
            serverListener.Start();
            Console.WriteLine("Srever started");
            Console.WriteLine($"Listening on port: {port}");
            while (true)
            {
                var connection = serverListener.AcceptTcpClient();
                var networkStream = connection.GetStream();

                string content = "Hello from the server!";
                WriteResponse(networkStream, content);
                
                connection.Close();
            }
        }

        private void WriteResponse(NetworkStream networkStream, string message)
        {
            int contentLength = Encoding.UTF8.GetByteCount(message);
            var response = $@"HTTP/1.1
Content-Type:text/plain; charset=UTF-8
Content-Length: {contentLength}

{message}";
            var responseBytes = Encoding.UTF8.GetBytes(response);
            networkStream.Write(responseBytes);
        }
    }

}

  

