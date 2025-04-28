using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData.Models
{
    public class NFLPlayerCareerRushStat
    {
        [Required]
        [Column("PlayerID")]
        public int ID { get; set; }
        public int RushAttempts { get; set; }
        public int RushYards { get; set; }
        public int RushTouchdowns { get; set; }
        public int RushFirstdowns { get; set; }

        public NFLPlayer Player { get; set; }
    }
}
