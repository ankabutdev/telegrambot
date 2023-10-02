using TelegramBot.Entity;

namespace TelegramBot.Services;

public class UserService
{
    public UserService()
    {

    }

    public async Task<User?> GetUserAsync(long? accountId)
    {
        return new User()
        {
            LanguageCode = "uz-Uz",
        };
    }
}
