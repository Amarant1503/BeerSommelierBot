using BeerSommelierBot.Models;
using BeerSommelierBot.Models.Ngrok;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BeerSommelierBot.Services
{
    public class NgrokService
    {
        private string _localUrl { get; }

        public NgrokService(string localUrl)
        {
            _localUrl = localUrl;
        }

        public async Task<NgrokTunnel> GetSecureTunnel()
        {
            var request = $"{_localUrl}/api/tunnels";

            var client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var reponse = JsonConvert.DeserializeObject<TunnelsResponse>(content);

                var tunnel = reponse.Tunnels.Where(t => t.Protocol == ProtocolEnum.Https).FirstOrDefault();

                return tunnel;
            }

            else throw new Exception($"ngrok not available, code {response.StatusCode}");
        }
    }
}
