using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData.Models
{
    public class NFLPlayerInfo
    {
        public int ID { get; set; }
        public int? TeamID { get; set; }
        public string? TeamName { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
