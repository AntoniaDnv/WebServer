using WebServer_First.Server;
using WebServer_First.Server.Responses;

namespace WebServer_First.Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new HttpServer(x =>
             x.MapGet("/html", new HtmlResponse("<h1 style=\"color:blue;\">Hello from my tml response</h1>"))
             .MapGet("/", new TextResponse("Hello from my server, now with routing table!!!"))
            );
            server.Start();
        }
    }
}
