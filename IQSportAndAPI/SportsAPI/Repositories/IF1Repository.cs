using Microsoft.AspNetCore.Mvc;
using SportsData.Models;

namespace SportsAPI.Interfaces
{
    public interface IF1Repository
    {

        Task<List<F1DriverInfo>> RetreiveAllDriversInfoAsync(string? sortOrderStr, int page);
        Task<F1DriverInfo> RetreiveDriverInfoAsync(int driverID);
        Task<List<F1Constructor>> RetreiveAllConstructorInfoAsync(string? sort, int page);
        Task<F1Constructor> RetreiveConstructorInfoAsync(int constructorID);
        Task<List<F1RaceInfo>> RetreiveAllRaceInfoAsync(int page);
        Task<F1RaceInfo> RetreiveRaceInfoAsync(int raceID);
        Task<List<F1CurrentConstructor>> RetreiveAllCurrentConstructorInfoAsync(int page);
        Task<F1CurrentConstructor> RetreiveCurrentConstructorInfoAsync(int constructorID);
        

        Task<int> RetreiveMaxYearAsync();
        Task<int> RetreiveMinYearAsync();
        Task<int> RetrieveDriverCountAsync();
        Task<int> RetrieveConstructorCountAsync();
        Task<int> RetrieveCurrentConstructorCountAsync();
        Task<int> RetrieveRaceCountAsync();
    }
}
