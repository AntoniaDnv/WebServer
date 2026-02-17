using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using WebServer_First.Server.HTTP;
using ContentType = WebServer_First.Server.HTTP.ContentType;

namespace WebServer_First.Server.Responses
{
    public class TextResponse:ContentResponse
    {
        public TextResponse(string text, Action<Request, Response> preRenderAction = null)
            :base(text, ContentType.PlainText, preRenderAction)
        {
            
        }
    }
}
