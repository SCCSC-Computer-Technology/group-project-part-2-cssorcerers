using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData.Models.TableModels.F1
{
    public class F1Result
    {
		[Required]
		[Key]
		public int ResultID { get; set; }
		[ForeignKey ("F1Race")]
		[Required]
		public int? RaceID { get; set; }
		[ForeignKey("F1Driver")]
		[Required]
		public int? DriverID { get; set; }
		[ForeignKey("F1Constructor")]
		[Required]
		public int? ConstructorID { get; set; }
		public int? Number { get; set; }
		[Required]
		public int? Grid { get; set; }
		public int? Position { get; set; }
		[StringLength(50)]
		public string? PositionText { get; set; }
		public int? PositionOrder { get; set; }
		public decimal? Points { get; set; }
		public int? Laps { get; set; }
		[ForeignKey("F1Status")]
		public int? StatusID { get; set; }
	}
}
