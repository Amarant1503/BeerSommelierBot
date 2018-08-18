using System.Threading.Tasks;
using Telegram.Bot;

namespace BeerSommelierBot.Services
{
    public static class TelegramService
    {
        public static void SetWebhook(TelegramBotClient client, string publicUrl)
        {
            Task.Run(() => client.SetWebhookAsync($"{publicUrl}/api/bot")).Wait();
        }
    }
}
