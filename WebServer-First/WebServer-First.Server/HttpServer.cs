using System.Net;
using System.Net.Sockets;
using System.Text;
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

                var request = Request.Parse(requestText);
               var response =  routes.MatchRequest(request);
               //string content = "Hello from the server!";
                WriteResponse(networkStream, response);
                if(response.PreRenderAction != null)
                {
                    response.PreRenderAction(request, response);
                }
               //connection.Close();
            }
        }

        private void WriteResponse(NetworkStream networkStream, Response response)
        {
           var responseBytes = Encoding.UTF8.GetBytes(response.ToString());
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
                string temp = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                requestBuilder.Append(temp);

            }
            while (networkStream.DataAvailable);
            return requestBuilder.ToString();
        }

    }

}

  

