using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BeerSommelierBot.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BeerSommelierBot.Controllers
{
    [Route("api/[controller]")]
    public class BotController : Controller
    {
        private TelegramOptions _tgOptions { get; }
        private ProxyOptions _proxyOptions { get; }
        private TelegramBotClient client { get; }

        public BotController(IOptions<TelegramOptions> tgOptions, IOptions<ProxyOptions> proxyOptions)
        {
            _tgOptions = tgOptions.Value;
            _proxyOptions = proxyOptions.Value;
            var proxy = new HttpToSocks5Proxy(_proxyOptions.Host, _proxyOptions.Port, _proxyOptions.User, _proxyOptions.Password);
            proxy.ResolveHostnamesLocally = true;
            client = new TelegramBotClient(_tgOptions.Token, proxy);
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public async void Post([FromBody]Update update)
        {
            if (update == null) return;
            var message = update.Message;
            if (message?.Type == MessageType.Text)
            {
                await client.SendTextMessageAsync(message.Chat.Id, message.Text);
            }
        }

        [HttpGet("telegram")]
        public async Task<string> TestApiAsync()
        {
            try
            {
                await client.SetWebhookAsync($"{ _tgOptions.WebhookUrl}");
            }
            catch (Exception ex)
            {

            }
            var me = await client.GetMeAsync();
            return ($"Hello! My name is {me.FirstName}");
        }
    }
}
