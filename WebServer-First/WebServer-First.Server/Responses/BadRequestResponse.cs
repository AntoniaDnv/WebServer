using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer_First.Server.HTTP;

namespace WebServer_First.Server.Responses
{
    public class BadRequestResponse :Response
    {
        public BadRequestResponse() 
            :base(StatusCode.BadRequest)
        {
        
        }
    }
}
