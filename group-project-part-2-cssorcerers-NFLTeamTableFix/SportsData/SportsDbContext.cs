using Microsoft.EntityFrameworkCore;
using SportsData.Models;
using SportsData.Models.TableModels;
using SportsData.Models.TableModels.F1;
using System.Collections.Generic;
using System.Diagnostics;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NFLTeamSeasonStat>()
        .HasKey(n => new { n.ID, n.Season });

            //modelBuilder.Entity<F1Races>()
                //.ToTable("F1Race");
        }

        public DbSet<F1Circuit>F1Circuit { get; set; }
        public DbSet<F1Constructor>F1Constructor { get; set; }
        public DbSet<F1CurrentConstructor> F1CurrentConstructor { get; set; }
		public DbSet<F1Driver>F1Driver { get; set; }
		public DbSet<F1Race> F1Race { get; set; }
		public DbSet<F1Result> F1Result { get; set; }
        public DbSet<F1Status> F1Status { get; set; }

        

    }
}
