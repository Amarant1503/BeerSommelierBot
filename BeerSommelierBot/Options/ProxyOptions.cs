using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeerSommelierBot.Options
{
    public class ProxyOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
