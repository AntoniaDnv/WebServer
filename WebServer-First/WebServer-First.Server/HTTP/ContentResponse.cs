using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer_First.Server.Common;
using WebServer_First.Server.Responses;

namespace WebServer_First.Server.HTTP
{
    public class ContentResponse :Response
    {
        public ContentResponse(string content, string contentType)
            :base(StatusCode.OK)
        {
            Guard.AgainstNull(content);
            Guard.AgainstNull(contentType);
            Headers.Add(Header.ContentType, contentType);
            Body = content;
        }

        public override string ToString()
        {
            var contentLength = Encoding.UTF8.GetByteCount(Body);
            this.Headers.Add(Header.ContentLength, contentLength.ToString());
            return base.ToString();
        }
    }
}
