namespace TelegramBot.Entity;

public class User
{
    public long UserId { get; set; }

    public long ChatId { get; set; }

    public bool IsBot { get; set; }

    public string? FirstName { get; set; } = default!;

    public string? LastName { get; set; }

    public string? Username { get; set; }

    public string? LanguageCode { get; set; } = "uz";

    public DateTimeOffset? CreatedAt { get; set; }

    public DateTimeOffset? LastInteractionAt { get; set; }

}
