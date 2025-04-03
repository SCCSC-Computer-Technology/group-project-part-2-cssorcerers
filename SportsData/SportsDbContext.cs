using Microsoft.EntityFrameworkCore;
using SportsData.Models;
using SportsData.Models.TableModels;
using System.Collections.Generic;

namespace SportsData
{
    public class SportsDbContext : DbContext
    {
        public SportsDbContext(DbContextOptions<SportsDbContext> options) : base(options) { }

        public DbSet<NFLTeam> NFLTeam { get; set; }
        public DbSet<NFLTeamSeasonStat> NFLTeamSeasonStat { get; set; }

        public DbSet<NFLPlayer> NFLPlayer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NFLTeamSeasonStat>()
        .HasKey(n => new { n.ID, n.Season });
        }
    }
}
