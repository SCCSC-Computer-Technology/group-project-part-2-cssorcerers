using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    public class NFLTeam
    {
        [Key, Column("TeamID")]
        public int ID { get; set; }

        [Required, StringLength(50), Column("TeamName")]
        public string Name { get; set; }

        public ICollection<NFLTeamSeasonStat>? SeasonStats { get; set; }
    }
}
