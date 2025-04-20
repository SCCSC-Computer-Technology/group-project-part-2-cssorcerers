namespace IQSport.Models
{
    public class SportsGuidanceViewModel
    {
        public List<Highlight> RecentHighlights { get; set; }
        public List<Stat> QuickStats { get; set; }
    }

    public class Highlight
    {
        public string Sport { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }

    public class Stat
    {
        public string Sport { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public string Link { get; set; }
    }
}