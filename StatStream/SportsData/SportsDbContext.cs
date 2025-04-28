using Microsoft.EntityFrameworkCore;
using SportsData.Models;

namespace SportsData
{
    public class SportsDbContext : DbContext
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
        public DbSet<PremierMatch> PremierMatch { get; set; }
        public DbSet<PremierTeam> PremierTeam { get; set; }

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

            modelBuilder.Entity<NFLTeamSeasonStat>()
        .HasOne(n => n.Team)
        .WithMany(n => n.SeasonStats)
        .HasForeignKey(n => n.ID);


            modelBuilder.Entity<NFLPlayerCareerFumbleStat>()
        .HasOne(n => n.Player)
        .WithOne(n => n.FumbleStats)
        .HasForeignKey<NFLPlayerCareerFumbleStat>(n=>n.ID);

            modelBuilder.Entity<NFLPlayerCareerKickStat>()
        .HasOne(n => n.Player)
        .WithOne(n => n.KickStats)
        .HasForeignKey<NFLPlayerCareerKickStat>(n => n.ID);

            modelBuilder.Entity<NFLPlayerCareerPassStat>()
        .HasOne(n => n.Player)
        .WithOne(n => n.PassStats)
        .HasForeignKey<NFLPlayerCareerPassStat>(n => n.ID);

            modelBuilder.Entity<NFLPlayerCareerReceiveStat>()
        .HasOne(n => n.Player)
        .WithOne(n => n.ReceiveStats)
        .HasForeignKey<NFLPlayerCareerReceiveStat>(n => n.ID);

            modelBuilder.Entity<NFLPlayerCareerRushStat>()
        .HasOne(n => n.Player)
        .WithOne(n => n.RushStats)
        .HasForeignKey<NFLPlayerCareerRushStat>(n => n.ID);

            modelBuilder.Entity<NFLPlayerCareerSackStat>()
        .HasOne(n => n.Player)
        .WithOne(n => n.SackStats)
        .HasForeignKey<NFLPlayerCareerSackStat>(n => n.ID);

            // ---------------- PREMIER ----------------- //
            // Configure PremierTeam
            modelBuilder.Entity<PremierTeam>()
                .Property(t => t.TeamName)
                .HasMaxLength(20)
                .IsRequired();

            // Configure PremierMatch
            modelBuilder.Entity<PremierMatch>()
                .Property(m => m.FullTimeResult)
                .HasMaxLength(1)
                .IsRequired();

            modelBuilder.Entity<PremierMatch>()
                .Property(m => m.HalfTimeResult)
                .HasMaxLength(1);

            modelBuilder.Entity<PremierMatch>()
                .Property(m => m.Referee)
                .HasMaxLength(25);

            // Configure relationships
            modelBuilder.Entity<PremierMatch>()
                .HasOne(m => m.HomeTeamNavigation)
                .WithMany(t => t.HomeMatches)
                .HasForeignKey(m => m.HomeTeam)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PremierMatch>()
                .HasOne(m => m.AwayTeamNavigation)
                .WithMany(t => t.AwayMatches)
                .HasForeignKey(m => m.AwayTeam)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------------- NBA ----------------- //
            modelBuilder.Entity<NBATeamSeasonStat>()
       .HasKey(n => new { n.TeamID, n.Season });

            modelBuilder.Entity<NBATeamSeasonStat>()
        .HasOne(n => n.Team)
        .WithMany(n => n.SeasonStats)
        .HasForeignKey(n => n.TeamID);

            modelBuilder.Entity<NBAPlayerCareerStat>()
        .HasOne(n => n.Player)
        .WithOne(n => n.CareerStats)
        .HasForeignKey<NBAPlayerCareerStat>(n => n.PlayerID);

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