using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Services;

public partial class BotUpdateHandler
{
    private async Task HandleEditMessage(ITelegramBotClient botClient, Message? message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        var from = message.From;

        _logger.LogInformation("Received Edit Message from {from.Firstname}", from?.FirstName);
    }
}
