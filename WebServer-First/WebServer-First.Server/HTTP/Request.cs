using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer_First.Server.HTTP
{
    public class Request
    {
        public Method Method { get; set; }
        public string Url { get; set; }
        public HeaderCollection Headers { get; set; }

        public string Body { get; set; }

        public static Request Parse(string request)
        {
            var lines = request.Split("\r\n");
            var startLine = lines.First().Split(" ");
            var method = ParseMethod(startLine[0]);
            var url = startLine[1];


        }

        private static Method ParseMethod(string method)
        {
            try
            {
                return (Method)Enum.Parse(typeof(Method), method, true);

            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Method '{method}' is not supported");
            }
            var headers = ParseHeadres(lines.Skip(1));
            var bodyLines = bodyLines.Skip(headers.Count + 2).ToArray();
            var body = string.Join("\r\n", bodyLines);
            return new Request
            {
                Method = method,
                Url = url,
                Headers = headers,
                Body = bodyLines

            };
        }
        private static HeaderCollection ParseHeadres(IEnumerable<string> headerLines)
        {
            var headerCollection = new HeaderCollection();
            foreach (var headerLine in headerLines)
            {
                if (headerLine == string.Empty)
                {
                     break;
                }
                var headerParts = headerLine.Split(":", 2);

                if(headerParts.Length != 2)
                {
                    throw new InvalidOperationException("Request is not valid!");
                }
                var headerName = headerParts[0];
                var headerValue = headerParts[1].Trim();
                headerCollection.Add(headerName, headerValue);
            }
            return headerCollection;
        }

    }
}
