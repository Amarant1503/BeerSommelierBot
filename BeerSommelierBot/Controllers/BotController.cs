using BeerSommelierBot.Options;
using BeerSommelierBot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MihaZupan;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private TelegramBotClient _client { get; }
        private NgrokService _ngrokService { get; }

        public BotController(IOptions<TelegramOptions> tgOptions, IOptions<ProxyOptions> proxyOptions, IOptions<NgrokOptions> ngrokOptions)
        {
            _ngrokService = new NgrokService(ngrokOptions.Value.LocalUrl);
            _tgOptions = tgOptions.Value;
            _proxyOptions = proxyOptions.Value;
            var proxy = new HttpToSocks5Proxy(_proxyOptions.Host, _proxyOptions.Port, _proxyOptions.User, _proxyOptions.Password);
            proxy.ResolveHostnamesLocally = true;
            _client = new TelegramBotClient(_tgOptions.Token, proxy);
        }

        [HttpPost]
        public async void Post([FromBody]Update update)
        {
            if (update == null) return;
            var message = update.Message;
            if (message?.Type == MessageType.Text)
            {
                await _client.SendTextMessageAsync(message.Chat.Id, message.Text);
            }
        }

        /// <summary>
        /// manual webhook set
        /// </summary>
        /// <returns></returns>
        [HttpGet("telegram")]
        public async Task<string> SetWebhookAsync()
        {
            try
            {
                var tunnel = await _ngrokService.GetSecureTunnel();
                TelegramService.SetWebhook(_client, tunnel.PublicUrl);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"cannot establish Telegram webhook via Ngrok, error details: ", ex);
            }

            var me = await _client.GetMeAsync();
            return ($"Hello! My name is {me.FirstName}");
        }

        [HttpGet("healthcheck")]
        public async Task<IActionResult> TestBotAvailabilityAsync()
        {
            return Ok("Success!");
        }
    }
}
