namespace InfoAPI.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Sport { get; set; }
        public string EventName { get; set; }
        public DateTime MatchDate { get; set; }
        public string Teams { get; set; }
        public string Location { get; set; }
    }
}
