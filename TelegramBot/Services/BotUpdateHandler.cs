using Microsoft.Extensions.Localization;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Resources;

namespace TelegramBot.Services;

public partial class BotUpdateHandler : IUpdateHandler
{
    private readonly IServiceScopeFactory _scopeFactory;

    private readonly ILogger<BotUpdateHandler> _logger;

    private IStringLocalizer _localizer;

    private UserService _userService;

    public BotUpdateHandler(
        ILogger<BotUpdateHandler> logger,
        IServiceScopeFactory serviceScope)
    {
        _scopeFactory = serviceScope;
        _logger = logger;

    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Error occured with Telegram bot: {exception.Message}", exception.Message);

        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {

        using var scope = _scopeFactory.CreateScope();

        _userService = scope.ServiceProvider.GetRequiredService<UserService>();

        var culture = await GetCultureForUser(update);

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        _localizer = scope.ServiceProvider.GetRequiredService<IStringLocalizer<BotLocalizer>>();

        var handler = update.Type switch
        {
            UpdateType.Message => HandleMessageAsync(botClient, update.Message, cancellationToken),
            UpdateType.EditedMessage => HandleEditMessage(botClient, update.EditedMessage, cancellationToken),

            // handle other updates
            _ => HandlerUnknownUpdate(botClient, update, cancellationToken)
        };

        try
        {
            await handler;
        }
        catch (Exception ex)
        {
            await HandlePollingErrorAsync(botClient, ex, cancellationToken);
        }
    }

    private Task HandlerUnknownUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Update type {update.Type} received", update.Type);

        return Task.CompletedTask;
    }

    private async Task<CultureInfo> GetCultureForUser(Update update)
    {
        User? from = update.Type switch
        {
            UpdateType.Message => update?.Message?.From,
            UpdateType.EditedMessage => update?.EditedMessage?.From,
            UpdateType.CallbackQuery => update?.CallbackQuery?.From,
            UpdateType.InlineQuery => update?.InlineQuery?.From,
            _ => update?.Message?.From
        };

        var user = await _userService.GetUserAsync(from?.Id);

        var culture = new CultureInfo(user?.LanguageCode ?? "uz-Uz");

        return new CultureInfo("en-Us");
    }
}
