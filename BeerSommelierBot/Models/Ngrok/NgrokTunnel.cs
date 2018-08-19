using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeerSommelierBot.Models.Ngrok
{
    public class NgrokTunnel
    {
        public string Name { get; set; }
        [JsonProperty("public_url")]
        public string PublicUrl { get; set; }
        [JsonProperty("proto")]
        public ProtocolEnum Protocol { get; set; }
    }
}
