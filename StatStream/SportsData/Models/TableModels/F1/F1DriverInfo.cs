namespace SportsData.Models
{
    public class F1DriverInfo
    {
        public int DriverID { get; set; }
        public string? DriverRef { get; set; }
        public int? Number { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public DateTime? Dob { get; set; }
        public string? Nationality { get; set; }
        public string? Url { get; set; }

        public DateTime? DobDateOnly => Dob?.Date;
    }
}
