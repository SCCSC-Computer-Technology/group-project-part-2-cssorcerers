using Microsoft.EntityFrameworkCore;
using SportsData.Models;
using SportsData.Models.TableModels;
using SportsData.Models.TableModels.NBA;
using System.Collections.Generic;

namespace SportsData
{
    public class SportsDbContext : DbContext
    {
        public SportsDbContext(DbContextOptions<SportsDbContext> options) : base(options) { }

        public DbSet<NFLTeam> NFLTeam { get; set; }
        public DbSet<NFLTeamSeasonStat> NFLTeamSeasonStat { get; set; }

        public DbSet<NFLPlayer> NFLPlayer { get; set; }
        public DbSet<NFLPlayerCareerFumbleStat> NFLPlayerCareerFumbleStat { get; set; }
        public DbSet<NFLPlayerCareerKickStat> NFLPlayerCareerKickStat { get; set; }
        public DbSet<NFLPlayerCareerPassStat> NFLPlayerCareerPassStat { get; set; }
        public DbSet<NFLPlayerCareerReceiveStat> NFLPlayerCareerReceiveStat { get; set; }
        public DbSet<NFLPlayerCareerRushStat> NFLPlayerCareerRushStat { get; set; }
        public DbSet<NFLPlayerCareerSackStat> NFLPlayerCareerSackStat { get; set; }

        public DbSet<NBATeam> NBATeam { get; set; }
        public DbSet<NBATeamSeasonStat> NBATeamSeasonStat { get; set; }
        public DbSet<NBAPlayer> NBAPlayer { get; set; }
        public DbSet<NBAPlayerCareerStat> NBAPlayerCareerStat { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NFLTeamSeasonStat>()
        .HasKey(n => new { n.ID, n.Season });

            modelBuilder.Entity<NBATeamSeasonStat>()
        .HasKey(n => new { n.TeamID, n.Season });

            
        }
    }
}
