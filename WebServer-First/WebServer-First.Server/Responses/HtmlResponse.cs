using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer_First.Server.HTTP;

namespace WebServer_First.Server.Responses
{
    public class HtmlResponse : ContentResponse
    {
        public HtmlResponse(string html,
            Action<Request, Response> preRenderAction = null)
            :base(html, ContentType.Html, preRenderAction)
        {
            
        }

    }
}
