using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer_First.Server.Common;

namespace WebServer_First.Server.HTTP
{
    public class Cookie
    {
        public Cookie(string name, string value) 
        {
          Guard.AgainstNull(name, nameof(name));
          Guard.AgainstNull(value, nameof(value));

            this.Name = name;
            this.Value = value; 
        }

        public string Name { get; init; }
        public string Value { get; init; }

        public override string ToString()
        {
            return $"{this.Name}={this.Value}";
        }
    }
}
