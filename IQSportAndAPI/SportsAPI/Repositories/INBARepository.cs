using SportsData.Models;

namespace SportsAPI.Interfaces
{
    public interface INBARepository
    {

        Task<List<NBATeamSeasonInfo>> RetrieveAllTeamInfoAsync(string? sortOrderStr, int page); 
        Task<List<NBATeamSeasonInfo>> RetrieveTeamInfoBySeasonAsync(string? sortOrderStr, int season, int page); 
        Task<List<NBATeamSeasonInfo>> RetrieveTeamInfoByIDAsync( string? sortOrderStr, int teamID, int page); 
        Task<NBATeamSeasonInfo> RetrieveTeamSeasonInfoAsync(int teamID, int season);
        Task<List<NBATeamSeasonInfo>> RetrieveTeamAveragesAsync(string? sortOrderStr, int page); 
        Task<NBATeamSeasonInfo> RetrieveTeamAverageAsync(int teamID);
        Task<int> RetrieveTeamCountAsync(int? season, int? teamID);


        Task<List<NBAPlayer>> RetrieveAllPlayerInfoAsync(string? sortOrderStr, int page); 
        Task<List<NBAPlayer>> RetrievePlayerInfoByTeamAsync(string? sortOrderStr, int teamID, int page); 
        Task<List<NBAPlayer>> RetrievePlayerInfoByStatusAsync(string? sortOrderStr, bool isActive, int page); 
        Task<NBAPlayer> RetrievePlayerInfoByIDAsync(int playerID);
        Task<int> RetrievePlayerCountAsync(int? teamID, bool? isActive);

        Task<int> RetrieveMaxSeasonAsync();
        Task<int> RetrieveMinSeasonAsync();
    }
}
