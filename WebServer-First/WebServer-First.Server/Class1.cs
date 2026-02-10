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
                var requestText = this.ReadRequest(networkStream);
                Console.WriteLine(requestText);

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

        private string ReadRequest(NetworkStream networkStream)
        {
            var bufferLength = 1024;
            var buffer = new byte[bufferLength];
            var totalBytes = 0;

            var requestBuilder = new StringBuilder();
            do
            {
                var bytesRead = networkStream.Read(buffer, 0, bufferLength);
                totalBytes += bytesRead;
                if(totalBytes> 10 * 1024)
                {
                    throw new InvalidOperationException("Request is too large.");
                }
                requestBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

            }
            while (networkStream.DataAvailable);
            return requestBuilder.ToString();
        }

    }

}

  

