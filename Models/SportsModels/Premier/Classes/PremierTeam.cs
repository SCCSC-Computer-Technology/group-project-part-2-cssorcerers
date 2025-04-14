using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQSport.Models.SportsModels.Premier.Classes
{
    [Table("PremierTeam")]
    [Index(nameof(TeamName), IsUnique = true, Name = "IX_PremierTeam_TeamName")]
    public class PremierTeam
    {
        [Key]
        [Column("TeamID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamID { get; set; }

        [Required]
        [Column("TeamName")]
        [StringLength(20, ErrorMessage = "Team name cannot exceed 20 characters")]
        public string TeamName { get; set; }

        // Navigation properties
        public virtual ICollection<PremierMatch> HomeMatches { get; set; } = new List<PremierMatch>();
        public virtual ICollection<PremierMatch> AwayMatches { get; set; } = new List<PremierMatch>();
    }
}