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
            FileName = filename;
            Headers.Add(Header.ContentType, ContentType.PlainText);
        }

        public string FileName { get; }

        public override string ToString()
        {
            if (File.Exists(FileName))
            {
                Body = File.ReadAllTextAsync(FileName).Result;
                var fileBytesCount = new FileInfo(this.FileName).Length;
                Headers.Add(Header.ContentLength, fileBytesCount.ToString());
                Headers.Add(Header.ContentDisposition, $"attachment; filename=\"{FileName}\"");
            }
            return base.ToString();
        }
    }
}
