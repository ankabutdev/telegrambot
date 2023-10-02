using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBot.Services;

public partial class BotUpdateHandler
{
    private async Task HandleMessageAsync(ITelegramBotClient botClient, Message? message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);

        var from = message.From;
        _logger.LogInformation("Received message from {from.Firstname}", from?.FirstName);

        var handler = message.Type switch
        {
            MessageType.Text => HandleTextMessageAsync(botClient, message, cancellationToken),
            _ => HandleUnknownMessageAsync(botClient, message, cancellationToken)
        };

        await handler;
    }

    private Task HandleUnknownMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received mesage type {message.Type}", message.Type);

        return Task.CompletedTask;
    }

    private async Task HandleTextMessageAsync(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
    {
        var from = message.From;
        _logger.LogInformation("From: {from.Firstname}", from?.FirstName);

        if (message.Text.Contains("uz", StringComparison.CurrentCultureIgnoreCase))
        {
            await _userService.UpdateLanguageCodeAsync(from.Id, "uz-Uz");
        }
        else if (message.Text.Contains("en", StringComparison.CurrentCultureIgnoreCase))
        {
            await _userService.UpdateLanguageCodeAsync(from.Id, "en-Us");
        }
        else
        {
            await botClient.SendTextMessageAsync(
                chatId: from.Id,
                text: _localizer["greeting"],
                replyToMessageId: message.MessageId,
                cancellationToken: cancellationToken
                );
        }
    }
}