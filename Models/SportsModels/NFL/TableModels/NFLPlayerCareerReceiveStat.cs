using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQSport.Models.NFL.TableModels
{
    public class NFLPlayerCareerReceiveStat
    {
        [Required]
        [Column("PlayerID")]
        public int ID { get; set; }
        public int Receptions { get; set; }
        public int ReceivingYards { get; set; }
        public int ReceivingTouchdowns { get; set; }
    }
}
