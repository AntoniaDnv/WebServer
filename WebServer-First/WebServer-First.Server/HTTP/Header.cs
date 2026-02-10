using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer_First.Server.Common;

namespace WebServer_First.Server.HTTP
{
    public class Header
    {
        public Header(string name, string value) 
        {
            Guard.AgainstNull(name, nameof(name));
            Guard.AgainstNull(value, nameof(value));
           Name = name;
           Value = value;
        } 
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
