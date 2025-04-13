using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData.Models.TableModels.F1
{
    public class F1Race
    {
		[Required]
		[Key]
		public int RaceID { get; set; }
		[Required]
		public int? Year { get; set; }
		[Required]
		public int? Round { get; set; }
		[ForeignKey("F1Circuit")]
		[Required]
		public int? CircuitID { get; set; }
		[Required]
		[StringLength(50)]
		public string? Name { get; set; }
		[Required]
		[StringLength(50)]
		public string? Date { get; set; }
		[StringLength(50)]
		public string? Time { get; set; }
		[StringLength(100)]
		public string? Url { get; set; }
	}
}
