using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SportsData.Models
{
    [Table("PremierTeam")]
    [Index(nameof(TeamName), IsUnique = true, Name = "IX_PremierTeam_TeamName")]
    public class PremierTeam
    {
        [Key]
        [Column("TeamID")]
        public int TeamID { get; set; }

        [Required]
        [Column("TeamName")]
        [StringLength(20, ErrorMessage = "Team name cannot exceed 20 characters")]
        public string TeamName { get; set; }

        // Navigation properties
        [JsonIgnore]
        public virtual ICollection<PremierMatch> HomeMatches { get; set; } = new List<PremierMatch>();
        [JsonIgnore]
        public virtual ICollection<PremierMatch> AwayMatches { get; set; } = new List<PremierMatch>();
    }
}