using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IQSport.Models.SportModels.NBA.Models

{
    public class NBAPlayer
    {
        [Key, Column("PlayerID")]
        public int ID { get; set; }

        public int? TeamID { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
