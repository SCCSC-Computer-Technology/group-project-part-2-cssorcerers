using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData.Models
{
    public class NFLPlayerCareerPassStat
    {
        [Required]
        [Column("PlayerID")]
        public int ID { get; set; }

        public int CompletePasses { get; set; }
        public int PassAttempts { get; set; }
        public int PassingYards { get; set; }
        public int TouchdownPasses { get; set; }
        public int Interceptions {  get; set; }
        public int LongestPass { get; set; }

        public NFLPlayer Player { get; set; }
    }
}
