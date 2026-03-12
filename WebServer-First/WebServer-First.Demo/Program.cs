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
        private const string Username = "user";
        private const string Password = "user123";

        private const string LoginForm =
            @"<form action='/Login' method='POST'>
        Username: <input type='text' name='Username'/>
        Password: <input type='text' name='Password'/>
        <input type='submit' value='Log In' />
    </form>";
        private const string DownloadForm = @"<form action = '/Content' method='POST'>
 <input type='submit' value ='Download Sites Content' />
</form>";
        private const string FileName = "content.txt";
        static async Task Main(string[] args)
        {
            await DownloadSitesAsTextFile(Program.FileName, new string[] { "https://judge.softuni.org/", "https://judge.softuni.org/" });
            var server = new HttpServer(x =>
             x.MapGet("/html", new HtmlResponse("<h1 style=\"color:blue;\">Hello from my tml response</h1>"))
             .MapGet("/", new TextResponse("Hello from my server, now with routing table!!!"))
             .MapGet("/redirect", new RedirectResponse("https://github.com/"))
             .MapGet("/login", new HtmlResponse(Form.Html))
             .MapGet("/HTML", new TextResponse("", Program.AddFormDataAction))
             .MapPost("/Content", new HtmlResponse(Program.DownloadForm))
             .MapGet("/Content", new TextFileResponse(Program.FileName))
             .MapGet("/Cookies", new HtmlResponse("", Program.AddCookiesAction))
             .MapGet("/Session", new TextResponse("", Program.DisplaySessionInfoAction))
             .MapGet("/Login", new HtmlResponse(Program.LoginForm))
             .MapPost("/Login", new HtmlResponse("",Program.LoginAction))
             .MapGet("/Logout", new HtmlResponse("",  Program.LogoutAction))
             .MapGet("/UserProfile", new HtmlResponse ("", Program.GetUserDataAction)

            );
            server.Start();
        }
        private static void AddFormDataAction(Request request, Response response)
        {
            response.Body = "";
            foreach (var (key, value) in request.FormData ) 
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
            var requestHasCookies = request.Cookies.Any(c => c.Name != Session.SessionCookieName);
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
        private static void DisplaySessionInfoAction(Response response, Request request) 
        {
            var sessionExists = request.Session
                .ContainsKey(Session.SessionCurrentDataKey);
            var bodyText = "";
            if (sessionExists) 
            {
                var currentDate = request.Session[Session.SessionCurrentDataKey];
                bodyText = $"Stored date: {currentDate}!";
            
            }
            else
            {
                bodyText = "Current date stored!";
            }
            response.Body = "";
            response.Body += bodyText;
        }
        private static void  LoginAction(Request request, Response response)
        {
            request.Session.Clear();

            var bodyText = "";
            var usernameMatch = request.FormData["Username"] == Program.Username;
                
            var passwordMatch =  request.FormData["Password"] == Program.Password;

            if (usernameMatch && passwordMatch)
            {
                request.Session[Session.SessionUserKey] = "MyUserId";
                response.Cookies.Add(Session.SessionCookieName, request.Session.Id);
                bodyText = "<h1>Logged in successfully!</h1>";
            }
            else
            {
                bodyText = Program.LoginForm;
            }

            response.Body = "";
            response.Body += bodyText;
        }

        private static void LogoutAction(Request request, Response response)
        {
            request.Session.Clear();
            response.Body = "";
            response.Body += "<h1>Logged out successfully!</h1>";
           
        }

        private static void GetUserDataAction(Request request, Response response)
        {
            if (request.Session.ContainsKey(Session.SessionUserKey))
            {
                response.Body = "";
                response.Body += $"<h3>Currently logged in user" + $"is with username '{Username}' </h3>";
                
            }
            else
            {
                response.Body = "";
                response.Body += $"<h3> You should first log in" + $"-<a href='/Login>Login</a> </h3>";

            }

        }
    }
}
