using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SportsData.Models
{
    public class NBATeamSeasonStat
    {
        [Column("TeamID")]
        public int TeamID { get; set; }

        public int Season { get; set; }
        public int? FieldGoals { get; set; }
        public int? FieldGoalAttempts { get; set; }
        public int? ThreePoints { get; set; }
        public int? ThreePointAttempts { get; set; }
        public int? TwoPoints { get; set; }
        public int? TwoPointAttempts { get; set; }
        public int? FreeThrows { get; set; }
        public int? FreeThrowAttempts { get; set; }
        public int? OffensiveRebounds { get; set; }
        public int? DefesniveRebounds { get; set; }
        public int? Assists { get; set; }
        public int? Steals { get; set; }
        public int? Blocks { get; set; }
        public int? Turnovers { get; set; }
        public int? PersonalFouls { get; set; }
        public int? TotalPoints { get; set; }

        [JsonIgnore] // prevents infinite references
        public NBATeam Team { get; set; } //allows relationship between stats and team
    }
}
