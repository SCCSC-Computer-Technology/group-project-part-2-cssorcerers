using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsAPI.Interfaces;
using SportsData;
using SportsData.Models;
using System.Collections.Concurrent;
using System.Globalization;
using System.Runtime.InteropServices.Marshalling;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SportsAPI.Repositories
{
    public class CSGORepository : ICSGORepository
    {
        private static ConcurrentDictionary<int, CSGOPlayer> PlayerCache;
        private SportsDbContext db;

        public CSGORepository(SportsDbContext injectedContext)
        {
            db = injectedContext;

            PlayerCache = new ConcurrentDictionary<int, CSGOPlayer>
            (
                db.CSGOPlayers.Include(x=>x.PlayerTeams).ThenInclude(x=>x.Team).Include(x=>x.PlayerStat).ToDictionary(x=>x.PlayerID)
            );
        }

        public async Task<int> RetrievePlayerCount(string? name, double? minRating, int? minMaps)
        {
            IEnumerable<CSGOPlayer> data = PlayerCache.Values.AsEnumerable();
            if (name != null && data.Any())
            {
                data = data.Where(x => x.PlayerName.ToLower().Contains(name.ToLower()));
            }

            if (minRating.HasValue && data.Any())
            {
                data = data.Where(x => x.PlayerStat?.Rating >= (decimal)(minRating.Value));
            }

            if (minMaps.HasValue && data.Any())
            {
                data = data.Where(x => x.PlayerStat?.TotalMaps >= minMaps.Value);
            }

            return data.Count();
        }

        public async Task<List<CSGOPlayer>> RetrievePlayers(string? name, double? minRating, int? minMaps, int page, int pageSize, string? sort, bool isDesc)
        {
            IEnumerable<CSGOPlayer> data = PlayerCache.Values.AsEnumerable();
            if(name != null && data.Any())
            {
                data = data.Where(x=>x.PlayerName.ToLower().Contains(name.ToLower()));
            }

            if(minRating.HasValue && data.Any())
            {
                data = data.Where(x=>x.PlayerStat?.Rating >= (decimal)(minRating.Value));
            }

            if (minMaps.HasValue && data.Any())
            {
                data = data.Where(x=>x.PlayerStat?.TotalMaps >= minMaps.Value);
            }

            if(!string.IsNullOrEmpty(sort))
            {
                if (isDesc)
                {
                    data = sort switch
                    {
                        "Rating" => data.OrderByDescending(p => p.PlayerStat.Rating),
                        "Kd" =>
                            data.OrderByDescending(p => p.PlayerStat.Kd),
                        "Maps" => data.OrderByDescending(p => p.PlayerStat.TotalMaps),
                        _ => data.OrderByDescending(p => p.PlayerName)
                    };
                }
                else
                {
                    data = sort switch
                    {
                        "Rating" => data.OrderBy(p => p.PlayerStat.Rating),
                        "Kd" => data.OrderBy(p => p.PlayerStat.Kd),
                        "Maps" => data.OrderBy(p => p.PlayerStat.TotalMaps),
                        _ => data.OrderBy(p => p.PlayerName)
                    };
                }
            }

            return data?.Skip(pageSize * (page - 1)).Take(pageSize).ToList();
        }
    }
}
