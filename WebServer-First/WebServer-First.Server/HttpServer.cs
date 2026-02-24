using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebServer_First.Server.Contracts;
using WebServer_First.Server.HTTP;
using WebServer_First.Server.Responses;

namespace WebServer_First.Server
{
    public class HttpServer
    {
        private readonly IPAddress iPAddress;
        private readonly int port;
        private readonly TcpListener serverListener;

        private readonly RoutingTable routes;
 
        public HttpServer(string ipAddress, int port,
            Action<IRoutingTable> routingTableConfiuration)
        {
            this.iPAddress = IPAddress.Parse(ipAddress);
            this.port = port;
            this.serverListener = new TcpListener(this.iPAddress, this.port);

            routingTableConfiuration(routes = new RoutingTable());
        }
        public HttpServer(int port, Action<IRoutingTable> config)
            :this("127.0.0.1", port, config)
        {
            
        }

        public HttpServer(Action<IRoutingTable> config)
            :this(8080, config)
        {
            
        }
        public async Task Start()
        {
            serverListener.Start();
            Console.WriteLine("Srever started");
            Console.WriteLine($"Listening on port: {port}");
            while (true)
            {
                var connection = await serverListener.AcceptTcpClientAsync();
                _ = Task.Run(async () =>
                {
                    var networkStream = connection.GetStream();
                    var requestText = await ReadRequest(networkStream);
                    Console.WriteLine(requestText);

                    var request = Request.Parse(requestText);
                    var response = routes.MatchRequest(request);
                    //string content = "Hello from the server!";
                    await WriteResponse(networkStream, response);
                    if (response.PreRenderAction != null)
                    {
                        response.PreRenderAction(request, response);
                    }
                    WriteResponse(networkStream, response);
                        connection.Close();
                });
            }
        }

        private async Task WriteResponse(NetworkStream networkStream, Response response)
        {
           var responseBytes = Encoding.UTF8.GetBytes(response.ToString());
           await networkStream.WriteAsync(responseBytes);     
        }

        private async Task<string> ReadRequest(NetworkStream networkStream)
        {
            var bufferLength = 1024;
            var buffer = new byte[bufferLength];
            var totalBytes = 0;

            var requestBuilder = new StringBuilder();
            do
            {
                var bytesRead = await networkStream.ReadAsync(buffer, 0, bufferLength);
                totalBytes += bytesRead;
                if(totalBytes> 10 * 1024)
                {
                    throw new InvalidOperationException("Request is too large.");
                }
                string temp = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                requestBuilder.Append(temp);

            }
            while (networkStream.DataAvailable);
            return requestBuilder.ToString();
        }

    }

}

  

