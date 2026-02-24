using WebServer_First.Server;
using WebServer_First.Server.HTTP;
using WebServer_First.Server.NewFolder;
using WebServer_First.Server.Responses;

namespace WebServer_First.Demo
{
    internal class Program
    {
        private static const string Downloadform
        static async Task Main(string[] args)
        {
            var server = new HttpServer(x =>
             x.MapGet("/html", new HtmlResponse("<h1 style=\"color:blue;\">Hello from my tml response</h1>"))
             .MapGet("/", new TextResponse("Hello from my server, now with routing table!!!"))
             .MapGet("/redirect", new RedirectResponse("https://github.com/"))
             .MapGet("/login", new HtmlResponse(Form.Html))
             .MapGet("/HTML", new TextResponse("", Program.AddFormDataAction))
             .MapPost("./")
            );
            server.Start();
        }
        private static void AddFormDataAction(Request request, Response response)
        {
            response.Body = "";
            foreach (var (key, value) in request.FormData ) //Form not Form data?
            {
                response.Body += $"{key} -  {value}";
                response.Body += Environment.NewLine;
            }
        }
    }
}
