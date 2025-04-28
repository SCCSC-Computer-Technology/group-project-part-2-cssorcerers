using Microsoft.AspNetCore.Mvc;
using SportsAPI.Interfaces;
using SportsData;
using SportsData.Models;
using System.Collections.Concurrent;
using System.Globalization;

namespace SportsAPI.Repositories
{
    public class F1Repository : IF1Repository
    {
        private static ConcurrentDictionary<int, F1DriverInfo> F1DriverCache;
        private static ConcurrentDictionary<int, F1Constructor> F1ConstructorCache;
        private static ConcurrentDictionary<int, F1CurrentConstructor> F1CurrentConstructorCache;
        private static ConcurrentDictionary<int, F1RaceInfo> F1RaceCache;
        private SportsDbContext db;

        public F1Repository(SportsDbContext injectedContext)
        {
            db = injectedContext;


            F1DriverCache = new ConcurrentDictionary<int, F1DriverInfo>
            (
                    db.F1Driver.Select
                    (
                        x => new F1DriverInfo()
                        {
                            Code = x.Code,
                            Name = x.Name,
                            Dob = x.Dob,
                            DriverID = x.DriverID,
                            DriverRef = x.DriverRef,
                            Nationality = x.Nationality,
                            Number = x.Number,
                            Url = x.Url
                        }
                    ).ToDictionary(x=>x.DriverID)
            );
            F1ConstructorCache = new ConcurrentDictionary<int, F1Constructor>
            (
                db.F1Constructor.ToDictionary(x => x.ConstructorID)
            );
            F1CurrentConstructorCache = new ConcurrentDictionary<int, F1CurrentConstructor>
            (
                db.F1CurrentConstructor.ToDictionary(x => x.ConstructorID)
            );
            F1RaceCache = new ConcurrentDictionary<int, F1RaceInfo>
            (
                db.F1Race
                .Join(db.F1Circuit,
                      race => race.CircuitID,
                      circuit => circuit.CircuitID,
                      (race, circuit) => new F1RaceInfo
                      {
                          RaceID = race.RaceID,
                          Name = race.Name,
                          Year = race.Year,
                          Round = race.Round,
                          Date = race.Date,
                          Time = race.Time,
                          CircuitName = circuit.Name,
                          Url = race.Url
                      }).ToDictionary(x => x.RaceID)
            );
        }

        public async Task<List<F1Constructor>> RetreiveAllConstructorInfoAsync(string? sort, int page)
        {
            if (sort == null)
            {
                return F1ConstructorCache.Values.OrderBy(x => x.Name).Skip(5 * (page - 1)).Take(5).ToList();
            }

            switch (sort.ToLower())
            {
                case "name_asc":
                    return F1ConstructorCache.Values.OrderBy(x => x.Name).Skip(5 * (page - 1)).Take(5).ToList();
                    break;
                case "name_desc":
                    return F1ConstructorCache.Values.OrderByDescending(x => x.Name).Skip(5 * (page - 1)).Take(5).ToList();
                    break;
                default:
                    return F1ConstructorCache.Values.OrderBy(x => x.Name).Skip(5 * (page - 1)).Take(5).ToList();
                    break;
            }
        }

        public async Task<List<F1CurrentConstructor>> RetreiveAllCurrentConstructorInfoAsync(int page)
        {
            return F1CurrentConstructorCache.Values.OrderBy(x => x.TeamName).Skip(5 * (page - 1)).Take(5).ToList();
        }

        public async Task<List<F1DriverInfo>> RetreiveAllDriversInfoAsync(string? sortOrderStr, int page)
        {
            if (sortOrderStr == null)
            {
                return F1DriverCache.Values.OrderBy(x=>x.Name).Skip(5 * (page - 1)).Take(5).ToList();
            }

            switch(sortOrderStr.ToLower()) 
            {
                case "name_asc":
                    return F1DriverCache.Values.OrderBy(x => x.Name).Skip(5 * (page-1)).Take(5).ToList();
                    break;
                case "name_desc":
                    return F1DriverCache.Values.OrderByDescending(x => x.Name).Skip(5 * (page - 1)).Take(5).ToList();
                    break;
                default:
                    return F1DriverCache.Values.OrderBy(x => x.Name).Skip(5 * (page - 1)).Take(5).ToList();
                    break;
            }
        }

        public async Task<List<F1RaceInfo>> RetreiveAllRaceInfoAsync(int page)
        {
            return F1RaceCache.Values.Skip(10 * (page-1)).Take(10).ToList();
        }

        public async Task<F1Constructor> RetreiveConstructorInfoAsync(int constructorID)
        {

            return F1ConstructorCache.Values.Where(x=>x.ConstructorID == constructorID).FirstOrDefault();
        }

        public async Task<F1CurrentConstructor> RetreiveCurrentConstructorInfoAsync(int constructorID)
        {
            return F1CurrentConstructorCache.Values.Where(x => x.ConstructorID == constructorID).FirstOrDefault();
        }

        public async Task<F1DriverInfo> RetreiveDriverInfoAsync(int driverID)
        {
            return F1DriverCache.Values.Where(x => x.DriverID == driverID).FirstOrDefault();
        }

        public async Task<int> RetreiveMaxYearAsync()
        {
            return F1RaceCache.Values.Select(x=>x.Year).Max()?? DateOnly.MaxValue.Year;
        }

        public async Task<int> RetreiveMinYearAsync()
        {
            return F1RaceCache.Values.Select(x => x.Year).Min() ?? DateOnly.MinValue.Year;
        }

        public async Task<F1RaceInfo> RetreiveRaceInfoAsync(int raceID)
        {
            return F1RaceCache.Values.Where(x=> x.RaceID == raceID).SingleOrDefault();
        }

        public async Task<int> RetrieveConstructorCountAsync()
        {
            return F1ConstructorCache.Values.Count();
        }

        public async Task<int> RetrieveCurrentConstructorCountAsync()
        {
            return F1CurrentConstructorCache.Values.Count();
        }

        public async Task<int> RetrieveDriverCountAsync()
        {
            return F1DriverCache.Values.Count();
        }

        public async Task<int> RetrieveRaceCountAsync()
        {
            return F1RaceCache.Values.Count();
        }
    }
}
