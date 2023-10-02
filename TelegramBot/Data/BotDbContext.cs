using Microsoft.EntityFrameworkCore;
using TelegramBot.Entity;

namespace TelegramBot.Data;

public class BotDbContext : DbContext
{
    public DbSet<User>? Users { get; set; }

    public BotDbContext(DbContextOptions<BotDbContext> options)
        : base(options) { }

}