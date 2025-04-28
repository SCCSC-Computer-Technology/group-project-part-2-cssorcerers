using SportsData.Models;

namespace SportsAPI.Interfaces
{
    public interface INFLRepository
    {

        Task<List<NFLTeamSeasonInfo>> RetreiveAllNFLTeamInfoAsync(string? sortOrderStr, int page); 
        Task<List<NFLTeamSeasonInfo>> RetreiveNFLTeamInfoBySeasonAsync(string? sortOrderStr, int season, int page); 
        Task<List<NFLTeamSeasonInfo>> RetreiveNFLTeamInfoByIDAsync( string? sortOrderStr, int teamID, int page); 
        Task<NFLTeamSeasonInfo> RetreiveNFLTeamSeasonInfoAsync(int teamID, int season);
        Task<List<NFLTeamSeasonInfo>> RetreiveNFLTeamAveragesAsync(string? sortOrderStr, int page); 
        Task<NFLTeamSeasonInfo> RetreiveNFLTeamAverageAsync(int teamID);
        Task<int> RetreiveNFLTeamInfoCountAsync(int? season);

        Task<List<NFLPlayer>> RetreiveAllPlayerInfoAsync(string? sortOrderStr, int page); 
        Task<List<NFLPlayer>> RetreiveNFLPlayerInfoByTeamAsync(string? sortOrderStr, int teamID, int page); 
        Task<List<NFLPlayer>> RetreiveNFLPlayerInfoByStatusAsync(string? sortOrderStr, bool isActive, int page); 
        Task<NFLPlayer> RetreiveNFLPlayerInfoByIDAsync(int playerID);
        Task<int> RetreiveNFLPlayerCountAsync(int? teamID, bool? isActive);

        Task<int> RetreiveMaxSeasonAsync();
        Task<int> RetreiveMinSeasonAsync();
    }
}
