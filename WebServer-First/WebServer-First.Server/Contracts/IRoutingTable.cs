using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer_First.Server.HTTP;
using WebServer_First.Server.Responses;

namespace WebServer_First.Server.Contracts
{
    public interface IRoutingTable
    {
        IRoutingTable Map(string url, Method method, Response responses);
        IRoutingTable MapGet(string url,  Response responses);
        IRoutingTable MapPost(string url, Response responses);

    }
}
