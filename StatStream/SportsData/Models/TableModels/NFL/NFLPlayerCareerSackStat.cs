using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData.Models
{
    public class NFLPlayerCareerSackStat
    {
        [Required]
        [Column("PlayerID")]
        public int ID { get; set; }
        public int Sacks { get; set; }
        public int YardsLostToSacks { get; set; }

        public NFLPlayer Player { get; set; }
    }
}
