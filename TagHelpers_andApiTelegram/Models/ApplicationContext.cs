using Microsoft.EntityFrameworkCore;

namespace TagHelpers_andApiTelegram.Models
{
    public class ApplicationContext : DbContext

    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<TelegramUser> TelegramUsers { get; set; } = null!;
        public DbSet<Msg> Messages { get; set; } = null!;


        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

            //Database.EnsureDeleted();
            Database.EnsureCreated(); ///Проверяет сущствует база данных, если она не существует - создает базуданных.


        }
    }
}
