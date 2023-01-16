using Microsoft.EntityFrameworkCore;

namespace ZTP.Models
{
    public class ZTPDbContext : DbContext
    {
        public ZTPDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Word> Words { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserWord> UserWords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Word>()
                .Property(u => u.PolishWord)
                .IsRequired();

            modelBuilder.Entity<Word>()
                .Property(u => u.ForeignLanguageWord)
                .IsRequired();

            modelBuilder.Entity<User>()
              .Property(u => u.Email)
              .IsRequired();
        }
    }
}