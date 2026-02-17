using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer_First.Server.Common;
using WebServer_First.Server.Contracts;
using WebServer_First.Server.Responses;

namespace WebServer_First.Server.HTTP
{
    public class RoutingTable : IRoutingTable
    {
        private readonly Dictionary<Method, Dictionary<string, Response>> routes;
        public RoutingTable()
        {
            routes = new Dictionary<Method, Dictionary<string, Response>>()
            {
                [Method.Get] = new Dictionary<string, Response>(),
                [Method.Post] = new Dictionary<string, Response>(),
                [Method.Delete] = new Dictionary<string, Response>(),
                [Method.Put] = new Dictionary<string, Response>()
            };
        }
        public IRoutingTable Map(string url, Method method, Response responses)
        {
            switch (method) 
            {
                    case Method.Get:
                    return MapGet(url, responses);
                    
                    case Method.Post:
                    return MapPost(url, responses); 
                 
                   default:
                    throw new InvalidOperationException($"{method} is not implemented");
                  
            }
        }

        public IRoutingTable MapGet(string url, Response response)
        {
        Guard.AgainstNull(url, nameof(url));
            Guard.AgainstNull(response, nameof(response));

            this.routes[Method.Get][url] = response;
            return this;
        }

        public IRoutingTable MapPost(string url, Response response)
        {
            Guard.AgainstNull(url, nameof(url));
            Guard.AgainstNull(response, nameof(response));

            this.routes[Method.Post][url] = response;
            return this;
        }

        public Response MatchRequest(Request request)
        {
            var requestMethod = request.Method;
            var requestUrl = request.Url;
            if (routes.ContainsKey(requestMethod)== false || routes[requestMethod].ContainsKey(requestUrl) == false)
            {
                return new NotFoundResponse();
            }
            return routes[requestMethod][requestUrl];
        }
     }
}
