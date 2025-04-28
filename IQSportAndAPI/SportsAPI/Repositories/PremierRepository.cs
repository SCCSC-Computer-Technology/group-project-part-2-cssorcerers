using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SportsAPI.Interfaces;
using SportsData;
using SportsData.Models;
using System.Collections.Concurrent;
using System.Globalization;

namespace SportsAPI.Repositories
{
    public class PremierRepository : IPremierRepository
    {
        private static ConcurrentDictionary<int, PremierMatch> PremierMatches;
        private static ConcurrentDictionary<int, PremierTeam> PremierTeams;
        private SportsDbContext db;

        public PremierRepository(SportsDbContext injectedContext)
        {
            db = injectedContext;

            PremierMatches = new ConcurrentDictionary<int, PremierMatch>
            (
                db.PremierMatch.Include(x => x.AwayTeamNavigation).Include(x => x.HomeTeamNavigation).AsSplitQuery().ToDictionary(x => x.MatchID)
            );

            PremierTeams = new ConcurrentDictionary<int, PremierTeam>
            (
                db.PremierTeam.Include(x=>x.HomeMatches).Include(x=>x.AwayMatches).AsSplitQuery().ToDictionary(x=>x.TeamID)
            );

            
        }

        public async Task<DateTime> RetrieveMaxDateAsync()
        {
            return PremierMatches.Values.Select(x=>x.Date).Max(x=>x.Date);
        }

        public async Task<DateTime> RetrieveMinDateAsync()
        {
            return PremierMatches.Values.Select(x => x.Date).Min(x => x.Date);
        }

        public async Task<List<PremierMatch>> RetrievePremierMatchByTeamIDAsync(string? sortOrderStr, int id, int page)
        {
            return sortOrderStr switch
            {
                    "Goals" => PremierMatches.Values.Where(x => x.HomeTeam == id || x.AwayTeam == id).OrderBy(x=>x.FullTimeHomeGoals + x.FullTimeAwayGoals).Skip(5 * (page - 1)).Take(5).ToList(),
                    "Date" => PremierMatches.Values.Where(x => x.HomeTeam == id || x.AwayTeam == id).OrderBy(x=>x.Date).Skip(5 * (page - 1)).Take(5).ToList(),
                    _ => PremierMatches.Values.Where(x => x.HomeTeam == id || x.AwayTeam == id).Skip(5 * (page - 1)).Take(5).ToList()
            };
        }

        public async Task<List<PremierMatch>> RetrievePremierMatchesAsync(string? sortOrderStr, int page,
            int itemsPerPage, string? searchTerm, DateTime? dateFrom, DateTime? dateTo)
        {
            var data = PremierMatches.Values.AsEnumerable();
            if (dateFrom != null)
            {
                data = data.Where(x => x.Date > dateFrom);
            }
            if (dateTo != null)
            {
                data = data.Where(x=>x.Date < dateTo);
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                data = data.Where(x=>x.AwayTeamNavigation.TeamName.ToLower().Contains(searchTerm) ||
                                    x.HomeTeamNavigation.TeamName.ToLower().Contains(searchTerm));
            }

            data = data.Skip(itemsPerPage * (page-1)).Take(itemsPerPage);
            return sortOrderStr switch
            {
                "date_asc" => data.OrderBy(m => m.Date).ToList(),
                "date_desc" => data.OrderByDescending(m => m.Date).ToList(),
                "referee_asc" => data.OrderBy(m => m.Referee).ToList(),
                "referee_desc" => data.OrderByDescending(m => m.Referee).ToList(),
                "hometeam_asc" => data.OrderBy(m => m.HomeTeamNavigation.TeamName).ToList(),
                "hometeam_desc" => data.OrderByDescending(m => m.HomeTeamNavigation.TeamName).ToList(),
                "goals_asc" => data.OrderBy(m => m.FullTimeHomeGoals + m.FullTimeAwayGoals).ToList(),
                "goals_desc" => data.OrderByDescending(m => m.FullTimeHomeGoals + m.FullTimeAwayGoals).ToList(),
                _ => data.OrderByDescending(m => m.Date).ToList()
            };
        }

        public async Task<int> RetrievePremierMatchesCountAsync( string? searchTerm, DateTime? dateFrom, DateTime? dateTo)
        {
            var data = PremierMatches.Values.AsEnumerable();
            if (dateFrom != null)
            {
                data = data.Where(x => x.Date > dateFrom);
            }
            if (dateTo != null)
            {
                data = data.Where(x => x.Date < dateTo);
            }
            if (searchTerm != null)
            {
                data = data.Where(x => x.AwayTeamNavigation.TeamName.ToLower().Contains(searchTerm) ||
                                    x.HomeTeamNavigation.TeamName.ToLower().Contains(searchTerm));
            }

            return data.Count();
        }

        public async Task<List<PremierMatch>> RetrievePremierMatchesByDateAsync(string? sortOrderStr, DateTime date, int page)
        {
            return sortOrderStr switch
            {
                "Team" => PremierMatches.Values.Where(x => x.Date == date).OrderBy(x=>x.HomeTeamNavigation.TeamName).Skip(5 * (page - 1)).Take(5).ToList(),
                "Goals" => PremierMatches.Values.Where(x => x.Date == date).OrderBy(x => x.FullTimeAwayGoals + x.FullTimeAwayGoals).Skip(5 * (page - 1)).Take(5).ToList(),
                _ => PremierMatches.Values.Where(x => x.Date == date).Skip(5 * (page - 1)).Take(5).ToList(),
            };
        }

        public async Task<List<PremierMatch>> RetrievePremierMatchesByTeamNameAsync(string? sortOrderStr, string teamName, int page)
        {
            int? id = PremierTeams.Values.Where(x=>x.TeamName.ToUpper() == teamName.ToUpper()).Select(x=>x.TeamID).FirstOrDefault();

            if(id != null)
            {
                return await RetrievePremierMatchByTeamIDAsync(sortOrderStr, id.Value, page);
            }
            else
            {
                return null;
            }
        }

        public async Task<PremierTeam> RetrievePremierTeamAsync(int id)
        {
            return PremierTeams.Values.Where(x => x.TeamID == id).FirstOrDefault();
        }

        public async Task<List<PremierTeam>> RetrievePremierTeamsAsync()
        {
            return PremierTeams.Values.ToList();
        }
    }
}
