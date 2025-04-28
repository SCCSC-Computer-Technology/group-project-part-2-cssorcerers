using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData.Models
{
    public class NBATeam
    {
        [Key, Column("TeamID")]
        public int ID { get; set; }

        [Required, StringLength(50), Column("TeamName")]
        public string Name { get; set; }

        [Required, StringLength(3), Column("TeamAbreviation")]
        public string Abreviation { get; set; }

        public ICollection<NBATeamSeasonStat>? SeasonStats { get; set; } //allows relationship between stats and team
    }
}
