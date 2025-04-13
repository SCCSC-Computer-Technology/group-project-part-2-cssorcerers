
using SportsData.Models;

namespace SportIQ.Models
{
	public class F1DriversViewModel
	{
		public List<F1DriverInfo> F1Driver { get; set; }
		public string? SortOrder { get; set; }
		public int? Number { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Dob { get; set; }
		public string Nationality { get; set; }
		public int CurrentPage { get; set; }
		public int MaxPage { get; set; }
		public int DriverID { get; set; }
	}
}
