using InfoAPI.Models;

namespace InfoAPI.Data.Repos
{
    public interface ISportsDataService
    {
        IEnumerable<string> GetSports();
        bool IsValidSport(string sport);
        Task<IEnumerable<Highlight>> GetHighlightsAsync(string sport, int page = 1, int size = 10);
        Task<IEnumerable<Highlight>> GetAllHighlightsAsync(int page = 1, int size = 10); // Add this
        int GetTotalHighlightsCount();
        Task<IEnumerable<Schedule>> GetScheduleAsync(string sport, int page = 1, int size = 10);
        Task<IEnumerable<Schedule>> GetAllSchedulesAsync(int page = 1, int size = 10); // Add this
        int GetTotalSchedulesCount();
    }
}