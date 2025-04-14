using IQSport.Models.AuthModels;
using IQSport.Models.SportsModels.NFL.TableModels;
using Microsoft.EntityFrameworkCore;

namespace IQSport.Data.DbContext
{
    public class UsersDbContext
    {
        public class UserDbContext : Microsoft.EntityFrameworkCore.DbContext
        {
            public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

            public DbSet<User> Users { get; set; }
            public DbSet<SecurityQuestion> SecurityQuestions { get; set; }
            public DbSet<UserSecurityAnswer> UserSecurityAnswers { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<NFLTeamSeasonStat>()
            .HasKey(n => new { n.ID, n.Season });
            }
        }
    }
}
