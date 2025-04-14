using IQSport.Data.DbContext;
using IQSport.Models.SportsModels.F1.Models;
using Microsoft.EntityFrameworkCore;

namespace SportsData.Services
{
    public class F1Services
    {


        public static async Task<F1DriverInfo> GetF1DriverInfo(int driverID, SportsDbContext context)
        {
            var stats = context.F1Driver.Where(x => x.DriverID == driverID).SingleOrDefault();
            if (stats != null)
            {


                return new F1DriverInfo()
                {
                    DriverID = driverID,
                    DriverRef = stats.DriverRef,
                    Number = stats.Number,
                    Code = stats.Code,
                    Name = stats.Name,
                    Dob = stats.Dob.HasValue ? stats.Dob.Value.Date : null,
                    Nationality = stats.Nationality,
                    Url = stats.Url,
                };
            }

            throw new Exception($"Could not find data for driver ID {driverID} in the database");
        }

        public static async Task<F1CurrentConstructorInfo> GetF1CurrentConstructorInfo(int constructorID, SportsDbContext context)
        {
            var entity = await context.F1CurrentConstructor
                .FirstOrDefaultAsync(x => x.ConstructorID == constructorID);

            if (entity == null)
                throw new Exception("Constructor not found.");

            return new F1CurrentConstructorInfo
            {
                ConstructorID = entity.ConstructorID,
                TeamName = entity.TeamName,
                Principal = entity.Principal,
                Driver1 = entity.Driver1,
                D1Num = entity.D1Num,
                Driver2 = entity.Driver2,
                D2Num = entity.D2Num,
                Championships = entity.Championships,
                Base = entity.Base,
                PowerUnit = entity.PowerUnit
            };
        }

        public static async Task<F1RaceInfo> GetF1RaceInfo(int raceID, SportsDbContext context)
        {
            return await context.F1Race
                .Where(r => r.RaceID == raceID)
                .Join(context.F1Circuit,
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
                      })
                .FirstOrDefaultAsync() ?? throw new Exception("Race not found.");
        }


    }
}
