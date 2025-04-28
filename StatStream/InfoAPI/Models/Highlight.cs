namespace InfoAPI.Models
{
    public class Highlight
    {
        public int Id { get; set; }
        public string Sport { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime MatchDate { get; set; }
        public string VideoUrl { get; set; }
        public string ImageUrl { get; set; }
    }
}
