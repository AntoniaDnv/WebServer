using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer_First.Server.HTTP;

namespace WebServer_First.Server.Responses
{
    public class TextFileResponse :Response
    {
        public TextFileResponse(string filename)
            : base(HTTP.StatusCode.OK)
        {
            Filename = filename;
            Headers.Add(Header.ContentType, ContentType.PlainText);
        }

        public string Filename { get; }

        public override string ToString()
        {
            if (File.Exists(Filename))
            {
                Body = File.ReadAllText(Filename);
                Headers.Add(Header.ContentDisposition, $"attachment; filename=\"{Filename}\"");
            }
            return base.ToString();
        }
    }
}
