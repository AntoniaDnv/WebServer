using System.Text;
using System.Web;
using WebServer_First.Server;
using WebServer_First.Server.HTTP;
using WebServer_First.Server.NewFolder;
using WebServer_First.Server.Responses;

namespace WebServer_First.Demo
{
    internal class Program
    {
        
        private const string DownloadForm = @"<form action = '/Content' method='POST'>
 <input type='submit' value ='Download Sites Content' />
</form>";
        private const string FileName = "content.txt";
        static async Task Main(string[] args)
        {
            await DownloadSitesAsTextFile(Program.FileName, new string[] { "https://judge.softuni.org/", "https://judge.softuni.org/" });
            var server =  new HttpServer(x =>
             x.MapGet("/html", new HtmlResponse("<h1 style=\"color:blue;\">Hello from my tml response</h1>"))
             .MapGet("/", new TextResponse("Hello from my server, now with routing table!!!"))
             .MapGet("/redirect", new RedirectResponse("https://github.com/"))
             .MapGet("/login", new HtmlResponse(Form.Html))
             .MapGet("/HTML", new TextResponse("", Program.AddFormDataAction))
             .MapPost("/Content", new HtmlResponse(Program.DownloadForm))
             .MapGet("/Content", new TextFileResponse(Program.FileName))  
             .MapGet("/Cookies", new HtmlResponse("", Program.AddCookiesAction))

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

        private static async Task<string> DownloadWebSiteContent(string url)
        {
            var httpClient = new HttpClient();
            using (httpClient)
            {
                var response = await httpClient.GetAsync(url);
                var html = await response.Content.ReadAsStringAsync();
                return html.Substring(0, 2000);
            }
        }private static async Task DownloadSitesAsTextFile(string fileName, string[] urls)
        {
            var downloads = new List<Task<string>>();
            foreach (var url in urls) 
            {
             downloads.Add(DownloadWebSiteContent(url));    
            }
            var responses = await Task.WhenAll( downloads);
            var responsesString = string.Join(Environment.NewLine + new String('-', 100), responses);
            await File.WriteAllTextAsync(fileName, responsesString);
        }

        private static void AddCookiesAction(Request request, Response response)
        {
            var requestHasCookies = request.Cookies.Any();
            var bodyText = "";

            if (requestHasCookies) 
            {
                var cookieText = new StringBuilder();
                cookieText.AppendLine("<h1>Cookies</h1>");
                cookieText.Append("<table border='1'><tr><th>Name</th><th>Value</th></tr>");
                foreach(var cookie in request.Cookies)
                {
                    cookieText.Append("<tr>");
                    cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Name)}</td>");
                    cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Value)}</td>");
                    cookieText.Append("<tr>");
                }
                cookieText.AppendLine("</table>");
                bodyText = cookieText.ToString();
            }
            else
            {
                bodyText = "<h1>Cookies set!</h1>";
            }

            if (!requestHasCookies) 
            {
                response.Cookies.Add("My-Cookie", "My-Value");
                response.Cookies.Add("My-Second-Cookie", "My-Second-Value");
            
            }

        }
    }
}
