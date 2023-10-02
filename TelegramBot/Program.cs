using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
using TelegramBot.Data;
using TelegramBot.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BotDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("BotConnection")));

var token = builder.Configuration.GetValue("BotToken", string.Empty);

builder.Services.AddSingleton(p => new TelegramBotClient(token!));
builder.Services.AddSingleton<IUpdateHandler, BotUpdateHandler>();
builder.Services.AddHostedService<BotBackgroundService>();

builder.Services.AddScoped<UserService>();

builder.Services.AddLocalization();

var app = builder.Build();

var supportedCultures = new[] { "uz-Uz", "en-US", "ru-Ru" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.Run();
