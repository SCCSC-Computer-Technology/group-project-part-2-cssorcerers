using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SportsData.Models
{
    [Table("PremierMatch")]
    [Index(nameof(Date), nameof(HomeTeam), nameof(AwayTeam), Name = "IX_PremierMatch_DateTeams")]
    public class PremierMatch
    {
        [Key]
        [Column("MatchID")]
        public int MatchID { get; set; }

        [Required]
        [Column("Date")]
        public DateTime Date { get; set; }

        [Column("HomeTeam")]
        [Required]
        [ForeignKey(nameof(HomeTeamNavigation))]
        public int HomeTeam { get; set; }

        [Column("AwayTeam")]
        [Required]
        [ForeignKey(nameof(AwayTeamNavigation))]
        public int AwayTeam { get; set; }

        [Column("FullTimeHomeGoals")]
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Goals cannot be negative")]
        public int FullTimeHomeGoals { get; set; }

        [Column("FullTimeAwayGoals")]
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Goals cannot be negative")]
        public int FullTimeAwayGoals { get; set; }

        [Column("FullTimeResult")]
        [Required]
        [StringLength(1)]
        [RegularExpression("[HAD]", ErrorMessage = "Result must be H (Home), A (Away), or D (Draw)")]
        public string FullTimeResult { get; set; }

        [Column("HalfTimeHomeGoals")]
        [Range(0, int.MaxValue, ErrorMessage = "Goals cannot be negative")]
        public int? HalfTimeHomeGoals { get; set; }

        [Column("HalfTimeAwayGoals")]
        [Range(0, int.MaxValue, ErrorMessage = "Goals cannot be negative")]
        public int? HalfTimeAwayGoals { get; set; }

        [Column("HalfTimeResult")]
        [StringLength(1)]
        [RegularExpression("[HAD]?", ErrorMessage = "Result must be H, A, D, or null")]
        public string? HalfTimeResult { get; set; }

        [Column("Referee")]
        [StringLength(25)]
        public string? Referee { get; set; }

        [Column("HomeShots")]
        [Range(0, int.MaxValue)]
        public int? HomeShots { get; set; }

        [Column("AwayShots")]
        [Range(0, int.MaxValue)]
        public int? AwayShots { get; set; }

        [Column("HomeShotsOnTarget")]
        [Range(0, int.MaxValue)]
        public int? HomeShotsOnTarget { get; set; }

        [Column("AwayShotsOnTarget")]
        [Range(0, int.MaxValue)]
        public int? AwayShotsOnTarget { get; set; }

        [Column("HomeFouls")]
        [Range(0, int.MaxValue)]
        public int? HomeFouls { get; set; }

        [Column("AwayFouls")]
        [Range(0, int.MaxValue)]
        public int? AwayFouls { get; set; }

        [Column("HomeCorners")]
        [Range(0, int.MaxValue)]
        public int? HomeCorners { get; set; }

        [Column("AwayCorners")]
        [Range(0, int.MaxValue)]
        public int? AwayCorners { get; set; }

        [Column("HomeYellowCards")]
        [Range(0, int.MaxValue)]
        public int? HomeYellowCards { get; set; }

        [Column("AwayYellowCards")]
        [Range(0, int.MaxValue)]
        public int? AwayYellowCards { get; set; }

        [Column("HomeRedCards")]
        [Range(0, int.MaxValue)]
        public int? HomeRedCards { get; set; }

        [Column("AwayRedCards")]
        [Range(0, int.MaxValue)]
        public int? AwayRedCards { get; set; }

        // Navigation properties
        public virtual PremierTeam HomeTeamNavigation { get; set; }
        public virtual PremierTeam AwayTeamNavigation { get; set; }

    }
}