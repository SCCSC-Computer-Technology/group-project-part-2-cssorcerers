using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData.Models
{
    public class NFLPlayer
    {
        [Required]
        [Column("PlayerID")]
        public int ID { get; set; }
        [StringLength(100)]
        public string? Name { get; set; }
        public bool? IsActive { get; set; }

        public NFLTeam Team { get; set; }

        public NFLPlayerCareerFumbleStat? FumbleStats { get; set; }
        public NFLPlayerCareerKickStat? KickStats { get; set; }
        public NFLPlayerCareerPassStat? PassStats { get; set; }
        public NFLPlayerCareerReceiveStat? ReceiveStats { get; set; }
        public NFLPlayerCareerRushStat? RushStats { get; set; }
        public NFLPlayerCareerSackStat? SackStats { get; set; }
    }
}
