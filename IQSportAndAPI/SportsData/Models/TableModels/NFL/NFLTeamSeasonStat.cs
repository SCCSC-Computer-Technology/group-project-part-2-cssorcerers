using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SportsData.Models
{
    public class NFLTeamSeasonStat
    {
        [Column("TeamID")]
        public int ID { get; set; }
        
        public int Season { get; set; }
        [Required]
        public int RushingTouchdowns { get; set; }
        [Required]
        public int ReceivingTouchdowns { get; set; }
        [Required]
        public int TotalTouchdowns { get; set; }
        [Required]
        public int TwoPoints { get; set; }
        [Required]
        public int Wins { get; set; }
        [Required]
        public int Losses { get; set; }
        [Required]
        public int Ties { get; set; }
        [Required]
        public int FieldGoalsMade { get; set; }
        [Required]
        public int FieldGoalAttempts { get; set; }
        [Required]
        public int ExtraPointsMade { get; set; }
        [Required]
        public int RushYards { get; set; }
        [Required]
        public int PassingYards { get; set; }
        [Required]
        public int Interceptions { get; set; }

        
        public NFLTeam Team { get; set; }   
    }
}
