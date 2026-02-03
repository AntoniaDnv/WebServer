using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer_First.Server.HTTP
{
    public class Header
    {
        public Header(string name, string value) 
        {
           Name = name;
           Value = value;
        } 
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
