using IQSport.Models.NFL.TableModels;
using IQSport.Models.SportModels.Models;
using IQSport.Models.SportModels.NBA.Models;
using IQSport.Models.SportsModels.CSGO.Models;
using IQSport.Models.SportsModels.F1.Models;
using IQSport.Models.SportsModels.NBA.Models;
using IQSport.Models.SportsModels.NFL.TableModels;
using Microsoft.EntityFrameworkCore;

namespace IQSport.Data.DbContext
{
    public class SportsDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public SportsDbContext(DbContextOptions<SportsDbContext> options) : base(options) { }

        // ------------------ NFL -------------------- //
        public DbSet<NFLTeam> NFLTeam { get; set; }
        public DbSet<NFLTeamSeasonStat> NFLTeamSeasonStat { get; set; }
        public DbSet<NFLPlayer> NFLPlayer { get; set; }
        public DbSet<NFLPlayerCareerFumbleStat> NFLPlayerCareerFumbleStat { get; set; }
        public DbSet<NFLPlayerCareerKickStat> NFLPlayerCareerKickStat { get; set; }
        public DbSet<NFLPlayerCareerPassStat> NFLPlayerCareerPassStat { get; set; }
        public DbSet<NFLPlayerCareerReceiveStat> NFLPlayerCareerReceiveStat { get; set; }
        public DbSet<NFLPlayerCareerRushStat> NFLPlayerCareerRushStat { get; set; }
        public DbSet<NFLPlayerCareerSackStat> NFLPlayerCareerSackStat { get; set; }

        // ------------------ Premier -------------------- //
        public DbSet<Models.SportsModels.Premier.Classes.PremierMatch> PremierMatch { get; set; }
        public DbSet<Models.SportsModels.Premier.Classes.PremierTeam> PremierTeam { get; set; }

        // ------------------ NBA -------------------- //
        public DbSet<NBATeam> NBATeam { get; set; }
        public DbSet<NBATeamSeasonStat> NBATeamSeasonStat { get; set; }
        public DbSet<NBAPlayer> NBAPlayer { get; set; }
        public DbSet<NBAPlayerCareerStat> NBAPlayerCareerStat { get; set; }

        // ------------------ CS:GO -------------------- //
        public DbSet<CSGOPlayer> CSGOPlayers { get; set; }
        public DbSet<CSGOPlayerStat> CSGOPlayerStats { get; set; }
        public DbSet<CSGOTeam> CSGOTeams { get; set; }
        public DbSet<CSGOTeamStat> CSGOTeamStats { get; set; }
        public DbSet<CSGOPlayerTeam> CSGOPlayerTeams { get; set; }

        // ------------------ F1 -------------------- //
        public DbSet<F1Circuit> F1Circuit { get; set; }
        public DbSet<F1Constructor> F1Constructor { get; set; }
        public DbSet<F1CurrentConstructor> F1CurrentConstructor { get; set; }
        public DbSet<F1Driver> F1Driver { get; set; }
        public DbSet<F1Race> F1Race { get; set; }
        public DbSet<F1Result> F1Result { get; set; }
        public DbSet<F1Status> F1Status { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ----------------- NFL ------------------- //
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<NFLTeamSeasonStat>()
        .HasKey(n => new { n.ID, n.Season });

            // ---------------- PREMIER ----------------- //
            // Configure PremierTeam
            modelBuilder.Entity<Models.SportsModels.Premier.Classes.PremierTeam>()
                .Property(t => t.TeamName)
                .HasMaxLength(20)
                .IsRequired();

            // Configure PremierMatch
            modelBuilder.Entity<Models.SportsModels.Premier.Classes.PremierMatch>()
                .Property(m => m.FullTimeResult)
                .HasMaxLength(1)
                .IsRequired();

            modelBuilder.Entity<Models.SportsModels.Premier.Classes.PremierMatch>()
                .Property(m => m.HalfTimeResult)
                .HasMaxLength(1);

            modelBuilder.Entity<Models.SportsModels.Premier.Classes.PremierMatch>()
                .Property(m => m.Referee)
                .HasMaxLength(25);

            // Configure relationships
            modelBuilder.Entity<Models.SportsModels.Premier.Classes.PremierMatch>()
                .HasOne(m => m.HomeTeamNavigation)
                .WithMany(t => t.HomeMatches)
                .HasForeignKey(m => m.HomeTeam)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.SportsModels.Premier.Classes.PremierMatch>()
                .HasOne(m => m.AwayTeamNavigation)
                .WithMany(t => t.AwayMatches)
                .HasForeignKey(m => m.AwayTeam)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------- NBA ----------------- //
            modelBuilder.Entity<NBATeamSeasonStat>()
       .HasKey(n => new { n.TeamID, n.Season });

            // ---------------- CSGO ----------------- //

            // Map CSGO entities to their correct table names
            modelBuilder.Entity<CSGOPlayer>()
                .ToTable("CsgoPlayer")
                .HasKey(p => p.PlayerID);

            modelBuilder.Entity<CSGOPlayerStat>()
                .ToTable("CsgoPlayerStat")
                .HasKey(ps => ps.PlayerID);

            modelBuilder.Entity<CSGOTeam>()
                .ToTable("CsgoTeam")
                .HasKey(t => t.TeamID);

            modelBuilder.Entity<CSGOTeamStat>()
                .ToTable("CsgoTeamStat")
                .HasKey(ts => ts.TeamID);

            modelBuilder.Entity<CSGOPlayerTeam>()
                .ToTable("CsgoPlayerTeam")
                .HasKey(pt => new { pt.PlayerID, pt.TeamID });

            // Configure relationships
            modelBuilder.Entity<CSGOPlayer>()
                .HasOne(p => p.PlayerStat)
                .WithOne(ps => ps.Player)
                .HasForeignKey<CSGOPlayerStat>(ps => ps.PlayerID);

            modelBuilder.Entity<CSGOTeam>()
                .HasOne(t => t.TeamStat)
                .WithOne(ts => ts.Team)
                .HasForeignKey<CSGOTeamStat>(ts => ts.TeamID);

            modelBuilder.Entity<CSGOPlayerTeam>()
                .HasOne(pt => pt.Player)
                .WithMany(p => p.PlayerTeams)
                .HasForeignKey(pt => pt.PlayerID);

            modelBuilder.Entity<CSGOPlayerTeam>()
                .HasOne(pt => pt.Team)
                .WithMany(t => t.TeamPlayers)
                .HasForeignKey(pt => pt.TeamID);
        }
    }
}