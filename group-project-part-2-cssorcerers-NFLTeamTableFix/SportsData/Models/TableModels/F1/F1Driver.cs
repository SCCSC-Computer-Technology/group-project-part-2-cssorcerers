using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsData.Models.TableModels.F1
{
    public class F1Driver
    {
		[Required]
		[Key]
		public int DriverID { get; set; }
		[Required]
		[StringLength(50)]
		public string? DriverRef { get; set; }
		public int? Number { get; set; }
		[StringLength(50)]
		public string? Code { get; set; }
		[Required]
		[StringLength(50)]
		public string? Name { get; set; }
		public DateTime? Dob { get; set; }
		[Required]
		[StringLength(50)]
		public string? Nationality { get; set; }
		[StringLength(100)]
		public string? Url { get; set; }
	}
}
